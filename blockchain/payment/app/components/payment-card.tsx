"use client";

import { useState } from "react";
import { useSolana } from "./solana-provider";
import { Program, AnchorProvider, BN } from "@coral-xyz/anchor";
import { PublicKey, Connection } from "@solana/web3.js";

const PROGRAM_ID = new PublicKey(
    "HRbJQmmbR6i16CZppbvspAq6bN9NQSgU1ZaZSYb7Dpwt"
);

function ConnectedPaymentCard({ account }: { account: { address: string } }) {
    const { selectedWallet } = useSolana();
    const [usdAmount, setUsdAmount] = useState<string>("");
    const [currency, setCurrency] = useState<string>("USD");
    const [solPriceUsd, setSolPriceUsd] = useState<string>("150");
    const [isLoading, setIsLoading] = useState(false);
    const [txSignature, setTxSignature] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);

    const sendPayment = async () => {
        if (!usdAmount || parseFloat(usdAmount) <= 0) {
            setError("Please enter a valid USD amount");
            return;
        }

        setIsLoading(true);
        setError(null);
        setTxSignature(null);

        try {
            // Convert USD to SOL
            const usd = parseFloat(usdAmount);
            const solPrice = parseFloat(solPriceUsd);
            const solAmount = usd / solPrice;
            const amountLamports = Math.floor(solAmount * 1e9); // Convert to lamports

            // Generate a unique order ID (timestamp based)
            const orderId = new BN(Date.now());

            if (!selectedWallet?.features["solana:signAndSendTransaction"]) {
                throw new Error(
                    "Wallet does not support signAndSendTransaction"
                );
            }

            // Create connection
            const connection = new Connection(
                "http://127.0.0.1:8899",
                "confirmed"
            );

            // Create provider with wallet adapter
            const provider = new AnchorProvider(
                {
                    connection,
                    publicKey: new PublicKey(account.address),
                    sendTransaction: async (tx, signers) => {
                        const signAndSendFeature =
                            selectedWallet.features[
                                "solana:signAndSendTransaction"
                            ];

                        // Serialize transaction
                        tx.recentBlockhash = (
                            await connection.getLatestBlockhash()
                        ).blockhash;
                        tx.feePayer = new PublicKey(account.address);

                        const serialized = tx.serialize({
                            requireAllSignatures: false,
                            verifySignatures: false,
                        });

                        // Sign and send using wallet
                        const result =
                            await signAndSendFeature.signAndSendTransaction({
                                account: { address: account.address },
                                chain: "solana:localnet",
                                transaction: {
                                    format: "base64",
                                    message:
                                        Buffer.from(serialized).toString(
                                            "base64"
                                        ),
                                },
                            });

                        return result as string;
                    },
                } as any,
                { commitment: "confirmed" }
            );

            // Load program
            const idl = require("../../target/idl/payment.json");
            const program = new Program(idl, PROGRAM_ID, provider);

            // Send transaction - Anchor will handle PDA derivation
            const signature = await program.methods
                .createOrder(orderId, new BN(amountLamports), currency)
                .accounts({
                    user: new PublicKey(account.address),
                })
                .rpc();

            setTxSignature(signature);

            console.log("Payment successful:", {
                orderId: orderId.toString(),
                amount: amountLamports,
                solAmount: solAmount.toFixed(4),
                signature,
            });
        } catch (err: any) {
            console.error("Payment failed:", err);
            setError(err.message || "Failed to process payment");
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
                disabled={isLoading || !usdAmount || parseFloat(usdAmount) <= 0}
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
                        href={`http://localhost:8899/tx/${txSignature}?cluster=localnet`}
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
