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
        // Airdrop SOL to user for transaction fees
        const airdropSignature = await anchor
            .getProvider()
            .connection.requestAirdrop(
                user.publicKey,
                2 * anchor.web3.LAMPORTS_PER_SOL
            );
        await anchor
            .getProvider()
            .connection.confirmTransaction(airdropSignature);

        // Test data
        const orderId = new anchor.BN(12345);
        const amount = new anchor.BN(1000000); // 0.001 SOL (in lamports)
        const currency = "USD";

        // Derive PDA for the order account (for verification)
        const [orderPda] = PublicKey.findProgramAddressSync(
            [
                Buffer.from("order"),
                user.publicKey.toBuffer(),
                orderId.toArrayLike(Buffer, "le", 8),
            ],
            program.programId
        );

        // Create the order - Anchor will automatically derive the PDA
        const tx = await program.methods
            .createOrder(orderId, amount, currency)
            .accounts({
                user: user.publicKey,
            })
            .signers([user])
            .rpc();

        console.log("Order creation transaction signature:", tx);

        // Fetch and verify the order account
        const orderAccount = await program.account.order.fetch(orderPda);
        console.log("Order account:", {
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
        expect(orderAccount.amount.toString()).to.equal(amount.toString());
        expect(orderAccount.currency).to.equal(currency);
        expect(orderAccount.createdAt.toString()).to.equal(
            orderAccount.updatedAt.toString()
        );
    });
});
