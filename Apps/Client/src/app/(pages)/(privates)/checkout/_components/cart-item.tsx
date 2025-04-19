'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import Image from 'next/image';

import { ICartItemResponse } from '~/domain/interfaces/baskets/basket.interface';
import { CldImage } from 'next-cloudinary';

interface CartItemProps {
   item: ICartItemResponse;
}

const CartItem = ({ item }: CartItemProps) => {
   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col py-6 border-b border-[#ccc] ',
         )}
      >
         <div className="w-full flex flex-row">
            <div className="w-[60px] h-[35px]">
               <CldImage
                  src={`${item.product_image}`}
                  alt={item.product_name}
                  width={500}
                  height={500}
                  className="h-[180%] w-full object-cover"
               />
            </div>
            <div className="flex-1 flex flex-col justify-start items-start ml-2 text-[13px] tracking-[0.03px]">
               <div className="font-semibold ">{item.product_name}</div>
               <div className="font-light">
                  <span>{item.product_color_name}</span>
               </div>
               <div className="font-semibold mt-1">
                  ${item.product_unit_price.toFixed(2)} x {item.quantity}
               </div>
            </div>
         </div>
      </div>
   );
};
export default CartItem;
