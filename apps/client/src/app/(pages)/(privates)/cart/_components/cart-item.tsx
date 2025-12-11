'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { Button } from '@components/ui/button';
import { FaRegTrashAlt } from 'react-icons/fa';
import { Input } from '@components/ui/input';
import Link from 'next/link';
import NextImage from 'next/image';
import { TCartItem, IStoreBasketItemPayload } from '~/domain/types/basket.type';

// Constants
const MIN_QUANTITY = 1;
const MAX_QUANTITY = 99;

interface CartItemProps {
   item: TCartItem;
   currentCartItem: IStoreBasketItemPayload;
   index: number;
   onQuantityChange: (index: number, newQuantity: number) => void;
   onRemoveItem: (index: number) => void;
}

const CartItem = ({
   item,
   currentCartItem,
   index,
   onQuantityChange,
   onRemoveItem,
}: CartItemProps) => {
   const displayQuantity =
      (currentCartItem && currentCartItem.quantity) ||
      item.quantity ||
      MIN_QUANTITY;

   const maxAllowed = Math.min(MAX_QUANTITY, item.quantity_remain);

   const handleIncrement = () => {
      if (displayQuantity < maxAllowed) {
         onQuantityChange(index, displayQuantity + 1);
      }
   };

   const handleDecrement = () => {
      if (displayQuantity > MIN_QUANTITY) {
         onQuantityChange(index, displayQuantity - 1);
      }
   };

   // Prevent manual typing or wheel changes
   const handleKeyDown: React.KeyboardEventHandler<HTMLInputElement> = (e) => {
      e.preventDefault();
   };

   const handleWheel: React.WheelEventHandler<HTMLInputElement> = (e) => {
      (e.target as HTMLInputElement).blur();
      e.preventDefault();
   };

   if (!currentCartItem && !item) return null;

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay line-cart w-full flex flex-col py-6 border-b border-[#ccc] ',
         )}
      >
         <div className="product-item w-full flex flex-row justify-start">
            <div className="flex items-center justify-center overflow-hidden rounded-lg w-[200px]">
               {/* image */}
               <div className="w-[100px] overflow-hidden relative h-[200px] rounded-lg">
                  <NextImage
                     src={item.display_image_url}
                     alt={`test`}
                     width={Math.round((1000 * 16) / 9)}
                     height={1000}
                     className="absolute top-0 left-0 w-full h-full object-cover"
                  />
               </div>
            </div>
            <div className="flex-1 flex flex-col justify-start items-start pl-[16px]">
               <div className="w-full h-[60px] flex flex-row">
                  <div className="flex-1 h-full text-[22px] font-medium font-SFProText">
                     <Link href={`#`}>
                        {item.model.name} {item.storage.name} {item.color.name}
                     </Link>
                  </div>
                  {(() => {
                     const originalTotal = item.unit_price * displayQuantity;
                     const discountAmount = item.discount_amount ?? 0;
                     const hasDiscount = discountAmount > 0;
                     const discountedTotal = Math.max(
                        0,
                        originalTotal - discountAmount,
                     );

                     if (hasDiscount) {
                        return (
                           <div className="h-full flex flex-col text-[16px] font-normal text-end">
                              <div className="w-full font-medium text-red-500">
                                 ${discountedTotal.toFixed(2)}
                              </div>
                              <div className="w-full line-through text-[14px] font-light ">
                                 ${originalTotal.toFixed(2)}
                              </div>
                              <div className="w-full text-[14px] font-light ">
                                 x {displayQuantity}
                              </div>
                           </div>
                        );
                     }

                     return (
                        <div className="h-full flex flex-col text-[16px] font-normal text-end">
                           <div className="w-full font-medium">
                              ${originalTotal.toFixed(2)}
                           </div>
                           <div className="w-full text-[14px] font-light ">
                              x {displayQuantity}
                           </div>
                        </div>
                     );
                  })()}
               </div>
               <div className="w-full h-[60px] flex flex-row">
                  <div className="w-full h-full">
                     <div className="text-[15px] font-light tracking-[0.2px] flex flex-col gap-2">
                        <span className="font-SFProText font-normal text-[14px] flex items-center gap-2">
                           <span>Color: </span>
                           <p className="px-3 py-1 rounded-full bg-[#f5f5f7] w-fit first-letter:uppercase">
                              {item.color.name}
                           </p>
                        </span>
                        <span className="font-SFProText font-medium text-[16px]">
                           ${item.unit_price.toFixed(2)}
                        </span>
                     </div>
                  </div>
                  <span
                     onClick={() => {
                        onRemoveItem(index);
                     }}
                     className="hover:text-sky-500 cursor-pointer p-2 transition-all duration-200 ease-in-out"
                  >
                     <FaRegTrashAlt className="w-4 h-4 mt-3" />
                  </span>
               </div>
               <div className="w-full flex flex-row justify-end">
                  <Button
                     type="button"
                     className="relative w-8 h-6 border border-[#ebebeb] bg-sky-400 hover:bg-sky-500 disabled:bg-gray-300 disabled:cursor-not-allowed rounded-l-full flex p-0 z-0"
                     onClick={handleDecrement}
                     disabled={displayQuantity <= MIN_QUANTITY}
                     aria-label="Decrease quantity"
                  >
                     <p className="absolute h-1 top-0 bottom-0 right-3">-</p>
                  </Button>
                  <Input
                     type="text"
                     inputMode="none"
                     readOnly
                     onKeyDown={handleKeyDown}
                     onWheel={handleWheel}
                     className="w-8 h-6 p-0 border-x-0 border-[#ebebeb] text-center rounded-none text-xs cursor-default select-none"
                     value={displayQuantity}
                     aria-label="Quantity"
                     tabIndex={-1}
                  />
                  <Button
                     type="button"
                     className="relative w-8 h-6 border border-[#ebebeb] bg-sky-400 hover:bg-sky-500 disabled:bg-gray-300 disabled:cursor-not-allowed rounded-r-full flex p-0"
                     onClick={handleIncrement}
                     disabled={displayQuantity >= maxAllowed}
                     aria-label="Increase quantity"
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
