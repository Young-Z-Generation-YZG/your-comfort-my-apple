import * as anchor from "@coral-xyz/anchor";
import { Program } from "@coral-xyz/anchor";
import { Payment } from "../target/types/payment";
import { PublicKey, Keypair } from "@solana/web3.js";
import { expect } from "chai";
import * as bip39 from "bip39";
import { derivePath } from "ed25519-hd-key";
import bs58 from "bs58";
import { createHash } from "crypto";

describe("payment", () => {
    const DEFAULT_PUBLIC_KEY_RECEIVER =
        "ErG6pggTvvahaqon1kvGMPZj9C6V6KMPPymbTAziiWQK";

    const DEFAULT_PUBLIC_KEY_USER =
        "8Vp7jR2qUcK6LhKzk1TWAgR6wwLFpAC8NuPgsinQaKRU";

    // Helper function to convert object with numeric string keys to Uint8Array
    const objectToUint8Array = (obj: { [key: string]: number }): Uint8Array => {
        const maxIndex = Math.max(...Object.keys(obj).map(Number));
        const arr = new Uint8Array(maxIndex + 1);
        for (const [key, value] of Object.entries(obj)) {
            arr[Number(key)] = value;
        }
        return arr;
    };

    const USER_KEYPAIR_RAW = {
        _keypair: {
            publicKey: {
                "0": 124,
                "1": 48,
                "2": 179,
                "3": 90,
                "4": 59,
                "5": 162,
                "6": 121,
                "7": 167,
                "8": 166,
                "9": 247,
                "10": 153,
                "11": 161,
                "12": 148,
                "13": 95,
                "14": 181,
                "15": 199,
                "16": 77,
                "17": 199,
                "18": 59,
                "19": 82,
                "20": 219,
                "21": 1,
                "22": 124,
                "23": 153,
                "24": 63,
                "25": 83,
                "26": 79,
                "27": 224,
                "28": 186,
                "29": 116,
                "30": 225,
                "31": 2,
            },
            secretKey: {
                "0": 3,
                "1": 215,
                "2": 149,
                "3": 199,
                "4": 24,
                "5": 189,
                "6": 255,
                "7": 90,
                "8": 61,
                "9": 2,
                "10": 36,
                "11": 210,
                "12": 225,
                "13": 76,
                "14": 128,
                "15": 199,
                "16": 7,
                "17": 133,
                "18": 199,
                "19": 135,
                "20": 21,
                "21": 254,
                "22": 140,
                "23": 152,
                "24": 208,
                "25": 87,
                "26": 135,
                "27": 189,
                "28": 20,
                "29": 173,
                "30": 16,
                "31": 8,
                "32": 124,
                "33": 48,
                "34": 179,
                "35": 90,
                "36": 59,
                "37": 162,
                "38": 121,
                "39": 167,
                "40": 166,
                "41": 247,
                "42": 153,
                "43": 161,
                "44": 148,
                "45": 95,
                "46": 181,
                "47": 199,
                "48": 77,
                "49": 199,
                "50": 59,
                "51": 82,
                "52": 219,
                "53": 1,
                "54": 124,
                "55": 153,
                "56": 63,
                "57": 83,
                "58": 79,
                "59": 224,
                "60": 186,
                "61": 116,
                "62": 225,
                "63": 2,
            },
        },
    };

    // Convert the raw keypair object to Solana Keypair
    const userSecretKey = objectToUint8Array(
        USER_KEYPAIR_RAW._keypair.secretKey
    );
    const USER_KEYPAIR = Keypair.fromSecretKey(userSecretKey);

    // Verify the public key matches expected
    const expectedPublicKey = DEFAULT_PUBLIC_KEY_USER;
    const actualPublicKey = USER_KEYPAIR.publicKey.toString();
    if (actualPublicKey !== expectedPublicKey) {
        console.warn(
            `⚠️  Public key mismatch! Expected: ${expectedPublicKey}, Got: ${actualPublicKey}`
        );
    } else {
        console.log("✅ User Keypair Public Key matches:", actualPublicKey);
    }

    const FAKE_ORDER_ID = "e824f6fa-92e4-4911-8b9e-98bd2ea4b82d";

    // Configure the client to use the local cluster.
    anchor.setProvider(anchor.AnchorProvider.env());

    const program = anchor.workspace.payment as Program<Payment>;

    // Use the converted USER_KEYPAIR instead of generating a random one
    const user = USER_KEYPAIR;

    // Generate mnemonic and convert to receiver Keypair
    const mnemonic = bip39.generateMnemonic();
    console.log("Generated Receiver Mnemonic:", mnemonic);

    // Convert mnemonic to seed (synchronous version)
    const seed = bip39.mnemonicToSeedSync(mnemonic);

    // Derive keypair using Solana's standard derivation path (m/44'/501'/0'/0')
    // Solana uses Ed25519 and derivation path m/44'/501'/0'/0'
    const derivedSeed = derivePath(
        "m/44'/501'/0'/0'",
        seed.toString("hex")
    ).key;
    const receiver = Keypair.fromSeed(derivedSeed);

    // Extract private key from secretKey
    // secretKey is 64 bytes: first 32 bytes = private key, last 32 bytes = public key
    const secretKey = receiver.secretKey;
    const privateKey = secretKey.slice(0, 32); // First 32 bytes = private key

    // Convert to different formats for display
    const privateKeyHex = Buffer.from(privateKey).toString("hex");
    const privateKeyBase58 = bs58.encode(privateKey); // Base58 encoding (Solana standard)
    const secretKeyBase58 = bs58.encode(secretKey); // Full secret key in Base58
    const derivedSeedHex = Buffer.from(derivedSeed).toString("hex");

    console.log("\n=== Receiver Keypair Information ===");
    console.log("Mnemonic:", mnemonic);
    console.log("Public Key (Base58):", receiver.publicKey.toString());
    console.log("Private Key (Hex - 32 bytes):", privateKeyHex);
    console.log("Private Key (Base58):", privateKeyBase58);
    console.log("Secret Key (Full 64 bytes - Base58):", secretKeyBase58);
    console.log("Derived Seed (32 bytes - Hex):", derivedSeedHex);
    console.log("Keypair:", JSON.stringify(receiver, null, 2));
    console.log("=====================================\n");

    it("Creates an order", async () => {
        // Calculate order amount first to determine airdrop size
        // Test data: $1000 USD order
        const usdAmount = 1000; // $1000 USD
        const solPriceUsd = 10000;
        const solAmount = usdAmount / solPriceUsd; // ~0.1 SOL

        // Airdrop SOL to user for transaction fees and payment
        // Need: order amount (~6.67 SOL) + account rent (~0.0001 SOL) + transaction fees (~0.0001 SOL) + buffer
        // const airdropAmount = Math.ceil(
        //     (solAmount + 0.1) * anchor.web3.LAMPORTS_PER_SOL
        // ); // ~0.1 SOL + buffer

        // const airdropSignature = await anchor
        //     .getProvider()
        //     .connection.requestAirdrop(user.publicKey, airdropAmount);
        // await anchor
        //     .getProvider()
        //     .connection.confirmTransaction(airdropSignature);

        const currency = "USD";

        // Convert USD to SOL (example rate: 1 SOL = $150 USD)
        // In production, you'd fetch this from an oracle or price feed
        const amountLamports = new anchor.BN(
            Math.floor(solAmount * anchor.web3.LAMPORTS_PER_SOL)
        );

        console.log("Order Details:");
        console.log(`  USD Amount: $${usdAmount}`);
        console.log(`  SOL Price (USD): $${solPriceUsd}`);
        console.log(`  SOL Amount: ${solAmount.toFixed(4)} SOL`);
        console.log(
            `  Lamports: ${amountLamports.toString()} (${
                amountLamports.toNumber() / anchor.web3.LAMPORTS_PER_SOL
            } SOL)`
        );

        // Derive order PDA
        // Split UUID (36 bytes) into two seeds: first 32 bytes + remaining 4 bytes
        // Solana allows multiple seeds, each up to 32 bytes
        const orderIdBytes = Buffer.from(FAKE_ORDER_ID);
        const orderIdFirst32 = orderIdBytes.slice(0, 32); // First 32 bytes
        const orderIdRemaining = orderIdBytes.slice(32); // Remaining 4 bytes

        const [orderPda] = PublicKey.findProgramAddressSync(
            [
                Buffer.from("order"),
                user.publicKey.toBuffer(),
                orderIdFirst32, // First 32 bytes of UUID
                orderIdRemaining, // Remaining 4 bytes of UUID
            ],
            program.programId
        );

        // Determine which receiver to use
        const receiverPublicKey = DEFAULT_PUBLIC_KEY_RECEIVER
            ? new PublicKey(DEFAULT_PUBLIC_KEY_RECEIVER)
            : receiver.publicKey;

        // Airdrop SOL to receiver account (for it to exist as an account)
        // Not needed if using DEFAULT_PUBLIC_KEY_RECEIVER that already exists
        // if (!DEFAULT_PUBLIC_KEY_RECEIVER) {
        //     const receiverAirdropAmount = 0.1 * anchor.web3.LAMPORTS_PER_SOL;
        //     const receiverAirdropSignature = await anchor
        //         .getProvider()
        //         .connection.requestAirdrop(
        //             receiver.publicKey,
        //             receiverAirdropAmount
        //         );
        //     await anchor
        //         .getProvider()
        //         .connection.confirmTransaction(receiverAirdropSignature);
        // }

        // Get balances before transaction
        const userBalanceBefore = await anchor
            .getProvider()
            .connection.getBalance(user.publicKey);

        const receiverBalanceBefore = await anchor
            .getProvider()
            .connection.getBalance(receiverPublicKey);

        console.log("\nBalances Before:");
        console.log(
            `  User: ${userBalanceBefore / anchor.web3.LAMPORTS_PER_SOL} SOL`
        );
        console.log(
            `  Receiver: ${
                receiverBalanceBefore / anchor.web3.LAMPORTS_PER_SOL
            } SOL`
        );

        // Create the order - Provide all accounts including the order PDA
        const tx = await program.methods
            .createOrder(FAKE_ORDER_ID, amountLamports, currency) // orderId is now a string
            .accounts({
                user: user.publicKey,
                receiver: receiverPublicKey, // Use the determined receiver public key
                order: orderPda, // Explicitly provide the order PDA
            } as any) // Type assertion needed until IDL is regenerated after program rebuild
            .signers([user])
            .rpc();

        console.log("\nOrder creation transaction signature:", tx);

        // Get balances after transaction
        const userBalanceAfter = await anchor
            .getProvider()
            .connection.getBalance(user.publicKey);
        const receiverBalanceAfter = await anchor
            .getProvider()
            .connection.getBalance(receiverPublicKey);

        console.log("\nBalances After:");
        console.log(
            `  User: ${userBalanceAfter / anchor.web3.LAMPORTS_PER_SOL} SOL`
        );
        console.log(
            `  Receiver: ${
                receiverBalanceAfter / anchor.web3.LAMPORTS_PER_SOL
            } SOL`
        );

        const paymentReceived = receiverBalanceAfter - receiverBalanceBefore;
        console.log(
            `\nPayment Received: ${
                paymentReceived / anchor.web3.LAMPORTS_PER_SOL
            } SOL (${paymentReceived} lamports)`
        );

        // Fetch and verify the order account
        const orderAccount = await program.account.order.fetch(orderPda);
        console.log("\nOrder account:", {
            id: orderAccount.id, // id is now a string, no need for toString()
            owner: orderAccount.owner.toString(),
            amount: orderAccount.amount.toString(),
            currency: orderAccount.currency,
            createdAt: orderAccount.createdAt.toString(),
            updatedAt: orderAccount.updatedAt.toString(),
        });

        // Verify the order data
        expect(orderAccount.id).to.equal(FAKE_ORDER_ID); // id is now a string, compare directly
        expect(orderAccount.owner.toString()).to.equal(
            user.publicKey.toString()
        );
        expect(orderAccount.amount.toString()).to.equal(
            amountLamports.toString()
        );
        expect(orderAccount.currency).to.equal(currency);
        expect(orderAccount.createdAt.toString()).to.equal(
            orderAccount.updatedAt.toString()
        );

        // Verify payment was received
        expect(paymentReceived).to.equal(amountLamports.toNumber());
    });
});
