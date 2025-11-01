import * as anchor from "@coral-xyz/anchor";
import { Program } from "@coral-xyz/anchor";
import { Payment } from "../target/types/payment";
import { PublicKey } from "@solana/web3.js";
import { expect } from "chai";

describe("payment", () => {
    // Configure the client to use the local cluster.
    anchor.setProvider(anchor.AnchorProvider.env());

    const program = anchor.workspace.payment as Program<Payment>;

    const user = anchor.web3.Keypair.generate();

    it("Creates an order", async () => {
        // Calculate order amount first to determine airdrop size
        // Test data: $1000 USD order
        const usdAmount = 1000; // $1000 USD
        const solPriceUsd = 150;
        const solAmount = usdAmount / solPriceUsd; // ~6.67 SOL

        // Airdrop SOL to user for transaction fees and payment
        // Need: order amount (~6.67 SOL) + account rent (~0.0001 SOL) + transaction fees (~0.0001 SOL) + buffer
        const airdropAmount = Math.ceil(
            (solAmount + 0.1) * anchor.web3.LAMPORTS_PER_SOL
        ); // ~7 SOL + buffer
        const airdropSignature = await anchor
            .getProvider()
            .connection.requestAirdrop(user.publicKey, airdropAmount);
        await anchor
            .getProvider()
            .connection.confirmTransaction(airdropSignature);

        const orderId = new anchor.BN(12345);
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

        // Derive PDAs
        const [orderPda] = PublicKey.findProgramAddressSync(
            [
                Buffer.from("order"),
                user.publicKey.toBuffer(),
                orderId.toArrayLike(Buffer, "le", 8),
            ],
            program.programId
        );

        const [treasuryPda] = PublicKey.findProgramAddressSync(
            [Buffer.from("treasury")],
            program.programId
        );

        // Get balances before transaction
        const userBalanceBefore = await anchor
            .getProvider()
            .connection.getBalance(user.publicKey);
        const treasuryBalanceBefore = await anchor
            .getProvider()
            .connection.getBalance(treasuryPda);

        console.log("\nBalances Before:");
        console.log(
            `  User: ${userBalanceBefore / anchor.web3.LAMPORTS_PER_SOL} SOL`
        );
        console.log(
            `  Treasury: ${
                treasuryBalanceBefore / anchor.web3.LAMPORTS_PER_SOL
            } SOL`
        );

        // Create the order - Anchor will automatically derive the PDAs
        const tx = await program.methods
            .createOrder(orderId, amountLamports, currency)
            .accounts({
                user: user.publicKey,
            })
            .signers([user])
            .rpc();

        console.log("\nOrder creation transaction signature:", tx);

        // Get balances after transaction
        const userBalanceAfter = await anchor
            .getProvider()
            .connection.getBalance(user.publicKey);
        const treasuryBalanceAfter = await anchor
            .getProvider()
            .connection.getBalance(treasuryPda);

        console.log("\nBalances After:");
        console.log(
            `  User: ${userBalanceAfter / anchor.web3.LAMPORTS_PER_SOL} SOL`
        );
        console.log(
            `  Treasury: ${
                treasuryBalanceAfter / anchor.web3.LAMPORTS_PER_SOL
            } SOL`
        );

        const paymentReceived = treasuryBalanceAfter - treasuryBalanceBefore;
        console.log(
            `\nPayment Received: ${
                paymentReceived / anchor.web3.LAMPORTS_PER_SOL
            } SOL (${paymentReceived} lamports)`
        );

        // Fetch and verify the order account
        const orderAccount = await program.account.order.fetch(orderPda);
        console.log("\nOrder account:", {
            id: orderAccount.id.toString(),
            owner: orderAccount.owner.toString(),
            amount: orderAccount.amount.toString(),
            currency: orderAccount.currency,
            createdAt: orderAccount.createdAt.toString(),
            updatedAt: orderAccount.updatedAt.toString(),
        });

        // Verify the order data
        expect(orderAccount.id.toString()).to.equal(orderId.toString());
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
