'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { Button } from '@components/ui/button';
import { FaRegTrashAlt } from 'react-icons/fa';
import { Input } from '@components/ui/input';
import { useState } from 'react';
import { ICartItemResponse } from '~/domain/interfaces/baskets/basket.interface';
import Link from 'next/link';
import { useDispatch } from 'react-redux';
import {
   removeCartItem,
   updateCartQuantity,
} from '~/infrastructure/redux/features/cart.slice';
import { CldImage } from 'next-cloudinary';

interface CartItemProps {
   item: ICartItemResponse;
}

const CartItem = ({ item }: CartItemProps) => {
   const dispatch = useDispatch();

   const handleRemoveItem = () => {
      dispatch(removeCartItem(item.product_id));
   };

   const handleQuantityChange = (quantity: number) => {
      if (quantity < 1) {
         handleRemoveItem();
      } else {
         dispatch(
            updateCartQuantity({
               product_id: item.product_id,
               quantity: quantity,
            }),
         );
      }
   };

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay line-cart w-full flex flex-col py-6 border-b border-[#ccc] ',
         )}
         key={item.product_id}
      >
         <div className="product-item w-full flex flex-row justify-start">
            <div className="flex items-center justify-center overflow-hidden rounded-lg w-[200px]">
               <CldImage
                  src={`${item.product_image}`}
                  alt={item.product_name}
                  width={1000}
                  height={1000}
                  className="h-[180%] w-[60%] object-cover"
               />
            </div>
            <div className="flex-1 flex flex-col justify-start items-start pl-[16px]">
               <div className="w-full h-[60px] flex flex-row">
                  <div className="flex-1 h-full text-[22px] font-medium font-SFProText">
                     <Link href={`#`}>{item.product_name}</Link>
                  </div>
                  {item.promotion && (
                     <div className="h-full flex flex-col text-[16px] font-normal text-end">
                        <div className="w-full font-medium text-red-500">
                           $
                           {(
                              item.promotion.promotion_final_price *
                              item.quantity
                           ).toFixed(2)}
                        </div>
                        <div className="w-full line-through text-[14px] font-light ">
                           $
                           {(item.product_unit_price * item.quantity).toFixed(
                              2,
                           )}
                        </div>
                        <div className="w-full text-[14px] font-light ">
                           x {item.quantity}
                        </div>
                     </div>
                  )}

                  {!item.promotion && (
                     <div className="h-full flex flex-col text-[16px] font-normal text-end">
                        <div className="w-full font-medium">
                           $
                           {(item.product_unit_price * item.quantity).toFixed(
                              2,
                           )}
                        </div>
                        <div className="w-full text-[14px] font-light ">
                           x {item.quantity}
                        </div>
                     </div>
                  )}
               </div>
               <div className="w-full h-[60px] flex flex-row">
                  <div className="w-full h-full">
                     <div className="text-[15px] font-light tracking-[0.2px] flex flex-col gap-2">
                        <span className="font-SFProText font-normal text-[14px] flex items-center gap-2">
                           <span>Color: </span>
                           <p className="px-3 py-1 rounded-full bg-[#f5f5f7] w-fit first-letter:uppercase">
                              {item.product_color_name}
                           </p>
                        </span>
                        <span className="font-SFProText font-medium text-[16px]">
                           ${item.product_unit_price.toFixed(2)}
                        </span>
                     </div>
                  </div>
                  <span
                     className="hover:text-sky-500 cursor-pointer p-2 transition-all duration-200 ease-in-out"
                     onClick={handleRemoveItem}
                  >
                     <FaRegTrashAlt className="w-4 h-4 mt-3" />
                  </span>
               </div>
               <div className="w-full flex flex-row justify-end">
                  <Button
                     onClick={() => {
                        handleQuantityChange(item.quantity - 1);
                     }}
                     className="relative w-8 h-6 border border-[#ebebeb] bg-sky-400 hover:bg-sky-500 rounded-l-full flex p-0 z-0"
                  >
                     <p className="absolute h-1 top-0 bottom-0 right-3">-</p>
                  </Button>
                  <Input
                     type="text"
                     className="w-8 h-6 p-0 border-x-0 border-[#ebebeb] text-center rounded-none text-xs"
                     value={item.quantity}
                     onChange={() => {}}
                  />
                  <Button
                     onClick={() => {
                        handleQuantityChange(item.quantity + 1);
                     }}
                     className="relative w-8 h-6 border border-[#ebebeb] bg-sky-400 hover:bg-sky-500 rounded-r-full flex p-0"
                  >
                     <p className="absolute h-1 top-0 bottom-0 right-3">+</p>
                  </Button>
               </div>
            </div>
         </div>
      </div>
   );
};
export default CartItem;
