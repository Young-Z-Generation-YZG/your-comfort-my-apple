'use client';

import { useSolana } from '@components/providers/solana-provider';
import { useWalletAccountTransactionSendingSigner } from '@solana/react';
import { UiWalletAccount } from '@wallet-standard/react';
import usdToLamports from '~/infrastructure/utils/blockchain/usd-to-sol';
import {
   address,
   getAddressEncoder,
   getProgramDerivedAddress,
} from '@solana/addresses';
import { getU64Encoder } from '@solana/codecs-numbers';
import {
   appendTransactionMessageInstruction,
   createTransactionMessage,
   Instruction,
   mergeBytes,
   pipe,
   setTransactionMessageFeePayerSigner,
   setTransactionMessageLifetimeUsingBlockhash,
   signAndSendTransactionMessageWithSigners,
   getBase58Decoder,
   type Signature,
} from '@solana/kit';
import { type Payment } from './types/payment';
import { useState } from 'react';

const PROGRAM_ID = address<Payment['address']>(
   'HRbJQmmbR6i16CZppbvspAq6bN9NQSgU1ZaZSYb7Dpwt',
);
const SYSTEM_PROGRAM_ID = address('11111111111111111111111111111111');

// Instruction discriminator for create_order (from IDL)
const CREATE_ORDER_DISCRIMINATOR = new Uint8Array([
   141, 54, 37, 207, 237, 210, 250, 215,
]);

const DEFAULT_PUBLIC_KEY_RECEIVER =
   'ErG6pggTvvahaqon1kvGMPZj9C6V6KMPPymbTAziiWQK';

// Seeds for PDA derivation
const ORDER_SEED = new TextEncoder().encode('order');

const FAKE_ORDER_ID = crypto.randomUUID();

const useBlockchainPayment = () => {
   const { selectedAccount, rpc, chain } = useSolana();

   const [isLoading, setIsLoading] = useState(false);

   const signer = useWalletAccountTransactionSendingSigner(
      selectedAccount as UiWalletAccount,
      chain,
   );

   const addressEncoder = getAddressEncoder();
   const u64Encoder = getU64Encoder();

   const createPaymentOrder = async (orderId: string, amount: string) => {
      try {
         setIsLoading(true);

         const amountLamports = usdToLamports(amount);

         // Get latest blockhash
         const { value: latestBlockhash } = await rpc
            .getLatestBlockhash({ commitment: 'confirmed' })
            .send();

         // Derive user address
         const userAddress = address(
            (selectedAccount as UiWalletAccount).address,
         );

         // Validate receiver address format
         let receiverAddressValidated;
         try {
            receiverAddressValidated = address(
               DEFAULT_PUBLIC_KEY_RECEIVER.trim(),
            );
         } catch (err) {
            console.log('error', err);

            return;
         }

         console.log('receiverAddressValidated', receiverAddressValidated);
         console.log('account.address', selectedAccount?.address);

         // Derive order PDA
         // Split UUID (36 bytes) into two seeds: first 32 bytes + remaining 4 bytes
         // Solana allows multiple seeds, each up to 32 bytes
         const orderIdBytes = new TextEncoder().encode(FAKE_ORDER_ID);
         const orderIdFirst32 = orderIdBytes.slice(0, 32); // First 32 bytes
         const orderIdRemaining = orderIdBytes.slice(32); // Remaining 4 bytes

         const userAddressBytes = addressEncoder.encode(userAddress);

         const [orderAddressPDA] = await getProgramDerivedAddress({
            programAddress: PROGRAM_ID,
            seeds: [
               ORDER_SEED,
               userAddressBytes,
               orderIdFirst32, // First 32 bytes of UUID
               orderIdRemaining, // Remaining 4 bytes of UUID
            ],
         });

         // Build instruction data in Borsh format
         // Borsh String format: 4 bytes (u32 length) + actual bytes

         // Encode order_id as Borsh String
         const orderIdStr = FAKE_ORDER_ID;
         const orderIdStrBytes = new TextEncoder().encode(orderIdStr);
         const orderIdLength = new Uint8Array(4);
         new DataView(orderIdLength.buffer).setUint32(
            0,
            orderIdStrBytes.length,
            true,
         );
         const orderIdEncoded = mergeBytes([orderIdLength, orderIdStrBytes]);

         // Encode amount as u64 (8 bytes little endian)
         const amountEncoded = new Uint8Array(
            u64Encoder.encode(amountLamports),
         );

         // Encode currency as Borsh String
         const currencyStr = 'USD';
         const currencyStrBytes = new TextEncoder().encode(currencyStr);
         const currencyLength = new Uint8Array(4);
         new DataView(currencyLength.buffer).setUint32(
            0,
            currencyStrBytes.length,
            true,
         );
         const currencyEncoded = mergeBytes([currencyLength, currencyStrBytes]);

         // Instruction data = discriminator + order_id + amount + currency
         const instructionData = new Uint8Array(
            mergeBytes([
               CREATE_ORDER_DISCRIMINATOR,
               orderIdEncoded,
               amountEncoded,
               currencyEncoded,
            ]),
         );

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
                  address: orderAddressPDA,
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
               setTransactionMessageLifetimeUsingBlockhash(latestBlockhash, m),
            (m) =>
               appendTransactionMessageInstruction(createOrderInstruction, m),
         );

         const signature =
            await signAndSendTransactionMessageWithSigners(message);

         const signatureStr = getBase58Decoder().decode(signature) as Signature;

         console.log('Payment successful:', {
            orderId: FAKE_ORDER_ID,
            amount: amountLamports.toString(),
            amountLamports: amountLamports,
            signature: signatureStr,
         });
      } catch (error: unknown) {
         console.log('error', error);
      } finally {
         setIsLoading(false);
      }
   };

   return {
      createPaymentOrder,
      isLoading,
   };
};

export default useBlockchainPayment;
