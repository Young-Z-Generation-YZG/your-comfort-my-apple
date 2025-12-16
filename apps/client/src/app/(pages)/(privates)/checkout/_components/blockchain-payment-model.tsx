'use client';

import useBlockchainPayment from '~/hooks/blockchain/use-blockchain-payment';
import { X } from 'lucide-react';
import { useSolana } from '~/components/providers/solana-provider';
import useBasketService from '~/hooks/api/use-basket-service';
import { useMemo, useState } from 'react';
import { UseFormReturn } from 'react-hook-form';
import { CheckoutFormType } from '~/domain/schemas/basket.schema';
import { LoadingOverlay } from '~/components/client/loading-overlay';
import { TCheckoutResponse } from '../page';
import { useRouter } from 'next/navigation';
import { useDispatch } from 'react-redux';
import { CleanSelectedItems } from '~/infrastructure/redux/features/cart.slice';

interface BlockchainPaymentModelProps {
   form: UseFormReturn<CheckoutFormType>;
   isOpen: boolean;
   onClose: () => void;
   amount: string;
}

const BlockchainPaymentModel = ({
   form,
   isOpen,
   onClose,
   amount,
}: BlockchainPaymentModelProps) => {
   const router = useRouter();
   const [processing, setProcessing] = useState(false);

   const dispatch = useDispatch();

   const {
      createPaymentOrder,
      isLoading: isLoadingBlockchain,
      ORDER_ID,
   } = useBlockchainPayment();

   const { isConnected } = useSolana();

   const { checkoutBasketWithBlockchainAsync, isLoading: isLoadingBasket } =
      useBasketService();

   const handlePaymentWithSolana = async () => {
      if (!isConnected) {
         alert('Please connect your wallet first');

         return;
      }

      setProcessing(true);

      const result = await createPaymentOrder(amount);

      if (
         result?.isSuccess &&
         result.data &&
         result.data.orderId &&
         result.data.customerPublicKey &&
         result.data.signature
      ) {
         const checkoutResult = await checkoutBasketWithBlockchainAsync({
            ...form.getValues(),
            crypto_uuid: result.data.orderId,
            customer_public_key: result.data.customerPublicKey,
            tx: result.data.signature,
         });

         if (checkoutResult.isSuccess) {
            dispatch(CleanSelectedItems());

            if (
               (checkoutResult.data as unknown as TCheckoutResponse)
                  .payment_redirect_url
            ) {
               window.location.href = (
                  checkoutResult.data as unknown as TCheckoutResponse
               ).payment_redirect_url;
            } else if (
               (checkoutResult.data as unknown as TCheckoutResponse)
                  .order_details_redirect_url
            ) {
               router.replace(
                  (checkoutResult.data as unknown as TCheckoutResponse)
                     .order_details_redirect_url,
               );
            }
         }
      }

      setProcessing(false);

      onClose();
   };

   const isLoading = useMemo(() => {
      return isLoadingBlockchain || isLoadingBasket || processing;
   }, [isLoadingBlockchain, isLoadingBasket, processing]);

   if (!isOpen) return null;

   return (
      <>
         <LoadingOverlay isLoading={isLoading} fullScreen />
         {/* Overlay with blur effect */}
         <div
            className="fixed inset-0 z-50 bg-black/50 backdrop-blur-sm transition-opacity"
            onClick={onClose}
            aria-hidden="true"
         />

         {/* Modal Content */}
         <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
            <div
               className="relative w-full max-w-md bg-white rounded-lg shadow-xl transform transition-all"
               onClick={(e) => e.stopPropagation()}
            >
               {/* Header */}
               <div className="flex items-center justify-between p-6 border-b border-gray-200">
                  <h2 className="text-xl font-semibold text-gray-900">
                     Blockchain Payment
                  </h2>
                  <button
                     onClick={onClose}
                     className="text-gray-400 hover:text-gray-600 transition-colors"
                     disabled={isLoading}
                  >
                     <X className="w-5 h-5" />
                  </button>
               </div>

               {/* Body */}
               <div className="p-6 space-y-4">
                  <div className="space-y-2">
                     <div className="flex justify-between text-sm">
                        <span className="text-gray-600">Order ID:</span>
                        <span className="font-medium text-gray-900">
                           {ORDER_ID}
                        </span>
                     </div>
                     <div className="flex justify-between text-sm">
                        <span className="text-gray-600">Amount:</span>
                        <span className="font-medium text-gray-900">
                           ${amount}
                        </span>
                     </div>
                  </div>

                  {!isConnected && (
                     <div className="p-3 bg-yellow-50 border border-yellow-200 rounded-md">
                        <p className="text-sm text-yellow-800">
                           Please connect your Solana wallet to proceed with
                           payment.
                        </p>
                     </div>
                  )}

                  {isConnected && (
                     <div className="p-3 bg-green-50 border border-green-200 rounded-md">
                        <p className="text-sm text-green-800">
                           Wallet connected. Ready to process payment.
                        </p>
                     </div>
                  )}
               </div>

               {/* Footer */}
               <div className="flex gap-3 p-6 border-t border-gray-200">
                  <button
                     onClick={onClose}
                     className="flex-1 px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 transition-colors"
                     disabled={isLoading}
                  >
                     Cancel
                  </button>
                  <button
                     onClick={handlePaymentWithSolana}
                     disabled={isLoading || !isConnected}
                     className="flex-1 px-4 py-2 text-sm font-medium text-white bg-blue-600 rounded-md hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
                  >
                     {isLoading ? (
                        <span className="flex items-center justify-center">
                           <svg
                              className="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                              xmlns="http://www.w3.org/2000/svg"
                              fill="none"
                              viewBox="0 0 24 24"
                           >
                              <circle
                                 className="opacity-25"
                                 cx="12"
                                 cy="12"
                                 r="10"
                                 stroke="currentColor"
                                 strokeWidth="4"
                              ></circle>
                              <path
                                 className="opacity-75"
                                 fill="currentColor"
                                 d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                              ></path>
                           </svg>
                           Processing...
                        </span>
                     ) : (
                        'Pay with Solana'
                     )}
                  </button>
               </div>
            </div>
         </div>
      </>
   );
};

export default BlockchainPaymentModel;
