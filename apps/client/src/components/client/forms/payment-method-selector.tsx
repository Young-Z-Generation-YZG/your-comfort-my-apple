'use client';

import Image from 'next/image';
import { Check } from 'lucide-react';
import type { UseFormRegister, FormState, UseFormWatch } from 'react-hook-form';
import { CheckoutFormType } from '~/domain/schemas/basket.schema';
import svgs from '@assets/svgs';
import { EPaymentType } from '~/domain/enums/payment-type.enum';

// Payment method icons
const paymentMethodIcons = {
   COD: '/placeholder.svg?height=40&width=40',
   VNPAY: '/placeholder.svg?height=40&width=40',
   MOMO: '/placeholder.svg?height=40&width=40',
};

interface PaymentMethodSelectorProps {
   register: UseFormRegister<CheckoutFormType>;
   errors: FormState<CheckoutFormType>['errors'];
   watch: UseFormWatch<CheckoutFormType>;
}

export function PaymentMethodSelector({
   register,
   errors,
   watch,
}: PaymentMethodSelectorProps) {
   const paymentMethod = watch('payment_method');

   return (
      <div className="mt-5">
         {errors.payment_method && (
            <p className="mb-4 text-sm text-red-500">
               {errors.payment_method.message}
            </p>
         )}

         <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            {/* COD Payment Option */}
            <div className="relative">
               <input
                  type="radio"
                  id="payment-cod"
                  value={EPaymentType.COD.toString()}
                  {...register('payment_method')}
                  className="peer sr-only"
               />
               <label
                  htmlFor="payment-cod"
                  className="flex flex-col items-center justify-between rounded-md border-2 border-gray-200 bg-white p-4 cursor-pointer hover:border-gray-300 peer-checked:border-gray-900 peer-checked:bg-gray-50 duration-200 ease-in-out"
               >
                  <div className="mb-2 rounded-full bg-gray-100 p-2">
                     <Image
                        src={svgs.truckIcon || '/placeholder.svg'}
                        alt="COD"
                        width={24}
                        height={24}
                        className="h-6 w-6"
                     />
                  </div>
                  <div className="font-medium select-none font-SFProText">
                     Cash on Delivery
                  </div>
                  <div className="text-xs text-gray-500 text-center mt-1 select-none">
                     Pay when you receive
                  </div>
               </label>
               <Check className="absolute top-4 right-4 h-5 w-5 text-gray-900 opacity-0 peer-checked:opacity-100" />
            </div>

            {/* VNPAY Payment Option */}
            <div className="relative">
               <input
                  type="radio"
                  id="payment-vnpay"
                  value={EPaymentType.VNPAY.toString()}
                  {...register('payment_method')}
                  className="peer sr-only"
               />
               <label
                  htmlFor="payment-vnpay"
                  className="flex flex-col items-center justify-between rounded-md border-2 border-gray-200 bg-white p-4 cursor-pointer hover:border-gray-300 peer-checked:border-gray-900 peer-checked:bg-gray-50 transition-all duration-200 ease-in-out"
               >
                  <div className="mb-2 rounded-full bg-gray-100 p-2">
                     <Image
                        src={svgs.vnpayLogo || '/placeholder.svg'}
                        alt="VNPAY"
                        width={24}
                        height={24}
                        className="h-6 w-6"
                     />
                  </div>
                  <div className="font-medium select-none font-SFProText">
                     VNPAY
                  </div>
                  <div className="text-xs text-gray-500 text-center mt-1 select-none">
                     Online payment
                  </div>
               </label>
               <Check className="absolute top-4 right-4 h-5 w-5 text-gray-900 opacity-0 peer-checked:opacity-100" />
            </div>

            {/* MOMO Payment Option */}
            <div className="relative opacity-50">
               <input
                  type="radio"
                  id="payment-momo"
                  value={EPaymentType.MOMO.toString()}
                  disabled={true}
                  {...register('payment_method')}
                  className="peer sr-only"
               />
               <label
                  htmlFor="payment-momo"
                  className="flex flex-col items-center justify-between rounded-md border-2 border-gray-200 bg-white p-4 cursor-not-allowed hover:border-gray-300 peer-checked:border-gray-900 peer-checked:bg-gray-50 duration-200 ease-in-out"
               >
                  <div className="mb-2 rounded-full bg-gray-100 p-2">
                     <Image
                        src={svgs.momoLogo || '/placeholder.svg'}
                        alt="MOMO"
                        width={24}
                        height={24}
                        className="h-6 w-6"
                     />
                  </div>
                  <div className="font-medium select-none font-SFProText">
                     MOMO
                  </div>
                  <div className="text-xs text-gray-500 text-center mt-1 select-none">
                     Mobile wallet
                  </div>
               </label>
               <Check className="absolute top-4 right-4 h-5 w-5 text-gray-900 opacity-0 peer-checked:opacity-100" />
            </div>
         </div>

         {/* Conditional Credit Card Fields for VNPAY */}
      </div>
   );
}
