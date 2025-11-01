'use client';

import { useState } from 'react';
import { useSolana } from '@components/providers/solana-provider';
import { useWalletAccountTransactionSendingSigner } from '@solana/react';
import { type UiWalletAccount } from '@wallet-standard/react';
import {
   pipe,
   createTransactionMessage,
   appendTransactionMessageInstruction,
   setTransactionMessageFeePayerSigner,
   setTransactionMessageLifetimeUsingBlockhash,
   signAndSendTransactionMessageWithSigners,
   getBase58Decoder,
   type Signature,
} from '@solana/kit';
import { getAddMemoInstruction } from '@solana-program/memo';

// Component that only renders when wallet is connected
function ConnectedMemoCard({ account }: { account: UiWalletAccount }) {
   const { rpc, chain } = useSolana();
   const [isLoading, setIsLoading] = useState(false);
   const [memoText, setMemoText] = useState('');
   const [txSignature, setTxSignature] = useState('');

   const signer = useWalletAccountTransactionSendingSigner(account, chain);

   const sendMemo = async () => {
      if (!signer) return;

      setIsLoading(true);
      try {
         const { value: latestBlockhash } = await rpc
            .getLatestBlockhash({ commitment: 'confirmed' })
            .send();

         const memoInstruction = getAddMemoInstruction({ memo: memoText });

         const message = pipe(
            createTransactionMessage({ version: 0 }),
            (m) => setTransactionMessageFeePayerSigner(signer, m),
            (m) =>
               setTransactionMessageLifetimeUsingBlockhash(latestBlockhash, m),
            (m) => appendTransactionMessageInstruction(memoInstruction, m),
         );

         const signature =
            await signAndSendTransactionMessageWithSigners(message);
         const signatureStr = getBase58Decoder().decode(signature) as Signature;

         setTxSignature(signatureStr);
         setMemoText('');
      } catch (error) {
         console.error('Memo failed:', error);
      } finally {
         setIsLoading(false);
      }
   };

   return (
      <div className="space-y-4">
         <div>
            <label className="block text-sm mb-1">Memo Message</label>
            <textarea
               value={memoText}
               onChange={(e) => setMemoText(e.target.value)}
               placeholder="Enter your memo message"
               className="w-full p-2 border rounded min-h-[100px]"
               maxLength={566}
            />
         </div>

         <button
            onClick={sendMemo}
            disabled={isLoading || !memoText.trim()}
            className="w-full p-2 bg-blue-500 text-white rounded hover:bg-blue-600 disabled:bg-gray-400"
         >
            {isLoading ? 'Sending...' : 'Send Memo'}
         </button>

         {txSignature && (
            <div className="p-2 border rounded text-sm">
               <p className="mb-1">Memo Sent</p>
               <a
                  href={`https://explorer.solana.com/tx/${txSignature}?cluster=devnet`}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-blue-500 hover:underline"
               >
                  View on Solana Explorer →
               </a>
            </div>
         )}
      </div>
   );
}

// Main memo component
export function MemoCard() {
   const { selectedAccount, isConnected } = useSolana();

   return (
      <div className="space-y-4 p-4 border rounded-lg">
         <h3 className="text-lg font-semibold">Send Memo</h3>
         {isConnected && selectedAccount ? (
            <ConnectedMemoCard account={selectedAccount} />
         ) : (
            <p className="text-gray-500 text-center py-4">
               Connect your wallet to send a memo
            </p>
         )}
      </div>
   );
}
