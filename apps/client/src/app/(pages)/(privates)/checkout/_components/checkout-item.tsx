'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import NextImage from 'next/image';
import { TCheckoutBasketItem } from '~/domain/types/basket.type';

interface CheckoutItemProps {
   item: TCheckoutBasketItem;
}

const CheckoutItem = ({ item }: CheckoutItemProps) => {
   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col py-6 border-b border-[#ccc] ',
         )}
      >
         <div className="w-full flex flex-row">
            {/* image */}
            <div className="w-[60px] overflow-hidden relative h-[60px] rounded-lg">
               <NextImage
                  src={item.display_image_url}
                  alt={item.product_name}
                  width={Math.round((1000 * 16) / 9)}
                  height={1000}
                  className="absolute top-0 left-0 w-full h-full object-cover"
               />
            </div>
            <div className="flex-1 flex flex-col justify-start items-start ml-2 text-[13px] tracking-[0.03px]">
               <div className="font-semibold ">{item.product_name}</div>
               <div className="font-light">
                  <span>{item.color.name}</span>
               </div>

               {!item.promotion && (
                  <div className="font-semibold mt-1">
                     ${item.unit_price.toFixed(2)} x {item.quantity}
                  </div>
               )}

               {item.promotion && (
                  <div className="">
                     <span className="font-semibold mt-1 text-sm inline-block text-red-500">
                        ${item.total_amount.toFixed(2)} x {item.quantity}
                     </span>
                     <span className="font-semibold mt-1 text-xs inline-block text-[#A0A0A0] line-through ml-2">
                        ${item.unit_price.toFixed(2)}
                     </span>
                  </div>
               )}
            </div>
         </div>
      </div>
   );
};
export default CheckoutItem;
