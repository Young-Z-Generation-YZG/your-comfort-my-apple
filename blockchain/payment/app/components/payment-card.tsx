"use client";

import { useState } from "react";
import { useSolana } from "./solana-provider";
import { useWalletAccountTransactionSendingSigner } from "@solana/react";
import { type UiWalletAccount } from "@wallet-standard/react";
import {
    address,
    getProgramDerivedAddress,
    getAddressEncoder,
} from "@solana/addresses";
import {
    pipe,
    createTransactionMessage,
    appendTransactionMessageInstruction,
    setTransactionMessageFeePayerSigner,
    setTransactionMessageLifetimeUsingBlockhash,
    signAndSendTransactionMessageWithSigners,
    getBase58Decoder,
    type Signature,
} from "@solana/kit";
import { getU64Encoder } from "@solana/codecs-numbers";
import { getUtf8Encoder } from "@solana/codecs-strings";
import { mergeBytes } from "@solana/codecs-core";
import { type Instruction } from "@solana/instructions";
import { type Payment } from "../../target/types/payment";

const PROGRAM_ID = address<Payment["address"]>(
    "HRbJQmmbR6i16CZppbvspAq6bN9NQSgU1ZaZSYb7Dpwt"
);
const SYSTEM_PROGRAM_ID = address("11111111111111111111111111111111");

// Instruction discriminator for create_order (from IDL)
const CREATE_ORDER_DISCRIMINATOR = new Uint8Array([
    141, 54, 37, 207, 237, 210, 250, 215,
]);

const FAKE_ORDER_ID = "e824f6fa-92e4-4911-8b9e-98bd2ea4b82d";

// Seeds for PDA derivation
const ORDER_SEED = new TextEncoder().encode("order");

function ConnectedPaymentCard({ account }: { account: UiWalletAccount }) {
    const { rpc, chain } = useSolana();
    const signer = useWalletAccountTransactionSendingSigner(account, chain);
    const [usdAmount, setUsdAmount] = useState<string>("");
    const [currency, setCurrency] = useState<string>("USD");
    const [solPriceUsd, setSolPriceUsd] = useState<string>("150");
    const [receiverAddress, setReceiverAddress] = useState<string>("");
    const [isLoading, setIsLoading] = useState(false);
    const [txSignature, setTxSignature] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);

    const sendPayment = async () => {
        if (!usdAmount || parseFloat(usdAmount) <= 0) {
            setError("Please enter a valid USD amount");
            return;
        }

        if (!receiverAddress || receiverAddress.trim() === "") {
            setError("Please enter a receiver address");
            return;
        }

        if (!signer) {
            setError("Wallet signer not available");
            return;
        }

        // Validate receiver address format
        let receiverAddressValidated;
        try {
            receiverAddressValidated = address(receiverAddress.trim());
        } catch (err) {
            setError("Invalid receiver address format");
            return;
        }

        console.log("receiverAddressValidated", receiverAddressValidated);
        console.log("account.address", account.address);

        setIsLoading(true);
        setError(null);
        setTxSignature(null);

        try {
            // Convert USD to SOL
            const usd = parseFloat(usdAmount);
            const solPrice = parseFloat(solPriceUsd);
            const solAmount = usd / solPrice;
            const amountLamports = BigInt(Math.floor(solAmount * 1e9)); // Convert to lamports

            // Generate a unique order ID (timestamp based as string)
            const orderId = FAKE_ORDER_ID;

            // Get latest blockhash
            const { value: latestBlockhash } = await rpc
                .getLatestBlockhash({ commitment: "confirmed" })
                .send();

            // Derive user address
            const userAddress = address(account.address);

            // Derive order PDA
            // Split UUID (36 bytes) into two seeds: first 32 bytes + remaining 4 bytes
            // Solana allows multiple seeds, each up to 32 bytes
            const orderIdBytes = new TextEncoder().encode(orderId);
            const orderIdFirst32 = orderIdBytes.slice(0, 32); // First 32 bytes
            const orderIdRemaining = orderIdBytes.slice(32); // Remaining 4 bytes

            // Encode user address to bytes for PDA seed
            const addressEncoder = getAddressEncoder();
            const userAddressBytes = addressEncoder.encode(userAddress);

            const [orderAddress] = await getProgramDerivedAddress({
                programAddress: PROGRAM_ID,
                seeds: [
                    ORDER_SEED,
                    userAddressBytes,
                    orderIdFirst32, // First 32 bytes of UUID
                    orderIdRemaining, // Remaining 4 bytes of UUID
                ],
            });

            // Build instruction data
            const u64Encoder = getU64Encoder();
            const utf8Encoder = getUtf8Encoder();

            // Encode arguments: order_id (string), amount (u64), currency (string)
            const orderIdEncoded = new Uint8Array(utf8Encoder.encode(orderId));
            const amountEncoded = new Uint8Array(
                u64Encoder.encode(amountLamports)
            );
            const currencyEncoded = new Uint8Array(
                utf8Encoder.encode(currency)
            );

            // Instruction data = discriminator + order_id + amount + currency
            const instructionData = mergeBytes([
                CREATE_ORDER_DISCRIMINATOR,
                orderIdEncoded,
                amountEncoded,
                currencyEncoded,
            ]);

            // Create the instruction
            const createOrderInstruction: Instruction<typeof PROGRAM_ID> = {
                programAddress: PROGRAM_ID,
                accounts: [
                    {
                        address: userAddress,
                        role: 3, // writable-signer
                    },
                    {
                        address: receiverAddressValidated,
                        role: 1, // writable
                    },
                    {
                        address: orderAddress,
                        role: 1, // writable
                    },
                    {
                        address: SYSTEM_PROGRAM_ID,
                        role: 0, // readonly
                    },
                ],
                data: instructionData,
            };

            // Build and send transaction
            const message = pipe(
                createTransactionMessage({ version: 0 }),
                (m) => setTransactionMessageFeePayerSigner(signer, m),
                (m) =>
                    setTransactionMessageLifetimeUsingBlockhash(
                        latestBlockhash,
                        m
                    ),
                (m) =>
                    appendTransactionMessageInstruction(
                        createOrderInstruction,
                        m
                    )
            );

            const signature = await signAndSendTransactionMessageWithSigners(
                message
            );
            const signatureStr = getBase58Decoder().decode(
                signature
            ) as Signature;

            setTxSignature(signatureStr);

            console.log("Payment successful:", {
                orderId: orderId,
                amount: amountLamports.toString(),
                solAmount: solAmount.toFixed(4),
                signature: signatureStr,
            });
        } catch (err) {
            console.error("Payment failed:", err);
            const errorMessage =
                err instanceof Error
                    ? err.message
                    : "Failed to process payment";
            setError(errorMessage);
        } finally {
            setIsLoading(false);
        }
    };

    const solAmount =
        usdAmount && solPriceUsd
            ? (parseFloat(usdAmount) / parseFloat(solPriceUsd)).toFixed(4)
            : "0";

    return (
        <div className="space-y-4">
            <div>
                <label className="block text-sm font-medium mb-1">
                    Amount (USD)
                </label>
                <input
                    type="number"
                    value={usdAmount}
                    onChange={(e) => setUsdAmount(e.target.value)}
                    placeholder="Enter amount in USD"
                    className="w-full p-2 border rounded-md dark:bg-gray-800 dark:border-gray-700"
                    min="0"
                    step="0.01"
                />
            </div>

            <div>
                <label className="block text-sm font-medium mb-1">
                    SOL Price (USD)
                </label>
                <input
                    type="number"
                    value={solPriceUsd}
                    onChange={(e) => setSolPriceUsd(e.target.value)}
                    placeholder="150"
                    className="w-full p-2 border rounded-md dark:bg-gray-800 dark:border-gray-700"
                    min="0"
                    step="0.01"
                />
            </div>

            <div>
                <label className="block text-sm font-medium mb-1">
                    Currency
                </label>
                <select
                    value={currency}
                    onChange={(e) => setCurrency(e.target.value)}
                    className="w-full p-2 border rounded-md dark:bg-gray-800 dark:border-gray-700"
                >
                    <option value="USD">USD</option>
                    <option value="EUR">EUR</option>
                    <option value="GBP">GBP</option>
                </select>
            </div>

            <div>
                <label className="block text-sm font-medium mb-1">
                    Receiver Address
                </label>
                <input
                    type="text"
                    value={receiverAddress}
                    onChange={(e) => setReceiverAddress(e.target.value)}
                    placeholder="Enter receiver Solana address"
                    className="w-full p-2 border rounded-md dark:bg-gray-800 dark:border-gray-700 font-mono text-sm"
                />
            </div>

            {usdAmount && (
                <div className="p-3 bg-blue-50 dark:bg-blue-900/20 rounded-md">
                    <p className="text-sm">
                        <span className="font-medium">${usdAmount} USD</span> ={" "}
                        <span className="font-medium">{solAmount} SOL</span>
                    </p>
                </div>
            )}

            <button
                onClick={sendPayment}
                disabled={
                    isLoading ||
                    !usdAmount ||
                    parseFloat(usdAmount) <= 0 ||
                    !receiverAddress ||
                    receiverAddress.trim() === ""
                }
                className="w-full p-3 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
            >
                {isLoading ? "Processing Payment..." : "Pay Now"}
            </button>

            {error && (
                <div className="p-3 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-md">
                    <p className="text-sm text-red-600 dark:text-red-400">
                        {error}
                    </p>
                </div>
            )}

            {txSignature && (
                <div className="p-3 bg-green-50 dark:bg-green-900/20 border border-green-200 dark:border-green-800 rounded-md">
                    <p className="text-sm font-medium mb-2 text-green-800 dark:text-green-200">
                        Payment Successful!
                    </p>
                    <a
                        href={`https://explorer.solana.com/tx/${txSignature}?cluster=${
                            chain === "solana:devnet" ? "devnet" : "mainnet"
                        }`}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="text-sm text-blue-600 dark:text-blue-400 hover:underline"
                    >
                        View on Solana Explorer →
                    </a>
                </div>
            )}
        </div>
    );
}

export function PaymentCard() {
    const { selectedAccount, isConnected } = useSolana();

    return (
        <div className="space-y-4 p-6 border rounded-lg shadow-lg bg-white dark:bg-gray-800">
            <h3 className="text-lg font-semibold">Create Payment Order</h3>
            {isConnected && selectedAccount ? (
                <ConnectedPaymentCard account={selectedAccount} />
            ) : (
                <p className="text-gray-500 dark:text-gray-400 text-center py-4">
                    Connect your wallet to create a payment order
                </p>
            )}
        </div>
    );
}
