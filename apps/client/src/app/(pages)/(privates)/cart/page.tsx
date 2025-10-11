'use client';
import '/globals.css';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import Image from 'next/image';

import images from '@components/client/images';
import { BsExclamationCircle } from 'react-icons/bs';
import { Button } from '@components/ui/button';
import { Input } from '@components/ui/input';
import {
   Accordion,
   AccordionItem,
   AccordionTrigger,
   AccordionContent,
} from '@components/ui/accordion';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { useCart } from './_hooks/useCart';
import { shoppingCartData } from './_data/fake-data';
import CartItem from './_components/cart-item';
import { useApiErrorHandler } from '../../shop/iphone/_hooks/useApiErrorHandler';
import { ICartItem } from '~/domain/interfaces/baskets/basket.interface';
import { useRouter } from 'next/navigation';

const CartPage = () => {
   const { isLoading, basketData, cartCalculations, isFallbackMode } = useCart({
      fallbackData: shoppingCartData,
   });

   const router = useRouter();

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col items-center justify-start bg-white',
         )}
      >
         <div className="max-w-[1156px] w-full grid grid-cols-12">
            <div className="col-span-12 h-fit flex flex-row justify-start items-center gap-3 p-6 bg-[#f5f5f7] mb-8">
               <BsExclamationCircle className="h-6 w-6" />
               <div className="text-black text-[16px] font-medium">
                  Promotional gift fully redeemed.
               </div>
            </div>
            <div className="col-span-8 ">
               <div className="w-full h-full pr-[64px] flex flex-col justify-start">
                  <div className="text-[16px] font-light tracking-[0.2px]">
                     You have {0} items in your cart
                  </div>
                  <LoadingOverlay isLoading={isLoading}>
                     {basketData?.cart_items.map(
                        (item: ICartItem, index: number) => (
                           <div key={index}>
                              <CartItem item={item} />
                           </div>
                        ),
                     )}
                  </LoadingOverlay>
               </div>
            </div>
            <div className="col-span-4 h-full relative">
               <div className="promotion w-full flex flex-col justify-start items-start pb-6 border-b border-[#dddddd]">
                  <div className="text-[15px] text-[#999999] font-normal tracking-[0.2px]">
                     Promo Code
                  </div>
                  <div className="w-full flex flex-row justify-between items-end ">
                     <Input
                        className="w-[200px] h-fit p-0 border-[#999999] border-t-0 border-l-0 border-r-0 border-b-1 rounded-none 
                        focus-visible:ring-0 focus-visible:ring-offset-0 text-[18px] font-light tracking-[0.2px]"
                        placeholder="Enter promo code"
                        value={''}
                        onChange={() => {}}
                        disabled={isLoading}
                     />
                     <Button
                        className="w-[80px] h-fit rounded-full text-[14px] font-medium tracking-[0.2px] transition-all duration-200 ease-in-out"
                        onClick={() => {}}
                        disabled={isLoading}
                     >
                        Apply
                     </Button>
                  </div>
                  {
                     <p className="text-sm font-light tracking-[0.2px] text-[#999999] pt-1">
                        Sign in to apply promo code
                     </p>
                  }
               </div>
               <div className="summary w-full flex flex-col justify-start items-start py-6 border-b border-[#dddddd]">
                  <div className="w-full pb-3 text-[22px] text-black font-bold tracking-[0.8px]">
                     Summary
                  </div>
                  <div className="w-full flex flex-col justify-between">
                     <div className="w-full flex flex-row justify-between items-center text-[14px] tracking-[0.2px]">
                        <div className="font-light">Subtotal</div>
                        <div className="font-semibold">
                           ${(100.2).toFixed(2)}
                        </div>
                     </div>
                     <Accordion type="multiple" className="w-full h-full ">
                        <AccordionItem value="item-1" className="border-none">
                           <AccordionTrigger className="hover:no-underline text-[14px] font-semibold tracking-[0.2px] pb-0 pt-0">
                              Total Savings
                           </AccordionTrigger>
                           <AccordionContent className="">
                              <div className="pt-1 pl-1 w-full flex flex-row justify-between items-center text-[14px] tracking-[0.2px]">
                                 <div className="font-light">
                                    Other Discount
                                 </div>
                                 <div className="font-semibold">
                                    - ${(1.2).toFixed(2)}
                                 </div>
                              </div>
                           </AccordionContent>
                        </AccordionItem>
                     </Accordion>
                  </div>
               </div>
               <div className="total w-full flex flex-col justify-start items-start pt-6">
                  <div className="w-full flex flex-row justify-between items-center text-[24px] font-semibold tracking-[0.2px]">
                     <div className="">Total</div>
                     <div className="">${(100.2 - 1.2).toFixed(2)}</div>
                  </div>
                  <Button
                     className="w-full h-fit border rounded-full text-[14px] font-medium tracking-[0.2px] mt-5"
                     disabled={isLoading}
                     onClick={() => router.push('/checkout')}
                  >
                     Checkout
                  </Button>
                  <div className="w-full mt-5 flex flex-col gap-3 text-[12px] font-semibold tracking-[0.2px]">
                     <div className="w-full flex flex-row items-center">
                        <Image
                           src={images.iconFreeShip}
                           alt="icon-deal"
                           width={1000}
                           height={1000}
                           quality={100}
                           className="w-[20px] h-[20px]"
                        />
                        <div className="pl-2">Free delivery</div>
                     </div>
                     <div className="w-full flex flex-row items-center">
                        <Image
                           src={images.iconDeal}
                           alt="icon-deal"
                           width={1000}
                           height={1000}
                           quality={100}
                           className="w-[20px] h-[20px]"
                        />
                        <div className="pl-2">0% Instalment Plans</div>
                     </div>
                     <div className="w-full flex flex-row items-center">
                        <Image
                           src={images.iconWarranty}
                           alt="icon-deal"
                           width={1000}
                           height={1000}
                           quality={100}
                           className="w-[20px] h-[20px]"
                        />
                        <div className="pl-2">Warranty</div>
                     </div>
                  </div>
               </div>
            </div>
         </div>
         <div className="w-full h-fit">
            <div className="w-full h-fit pt-[100px] pb-[50px]">
               <div className="w-fit mx-auto text-[38px] font-semibold tracking-[0.2px]">
                  You may also like
               </div>
            </div>
            {/* <div className="w-full h-fit">
               <Carousel
                  opts={{
                     align: 'start',
                  }}
                  className="w-full max-w-[1066px] mx-auto"
               >
                  <CarouselContent>
                     {Array.from({ length: 5 }).map((_, index) => (
                        <CarouselItem
                           key={index}
                           className="md:basis-1/2 lg:basis-1/3"
                        >
                           <div className="w-full h-[654px] p-6 bg-[#f5f5f7] rounded-[10px] flex flex-col justify-start items-center relative">
                              <div className="absolute top-5 left-5 w-fit h-fit bg-[#0070f0] text-white text-[12px] font-medium rounded-full px-2 py-[2px]">
                                 New
                              </div>
                              <Image
                                 src={images.ip16Pro}
                                 alt="product"
                                 width={1000}
                                 height={1000}
                                 quality={100}
                                 className="w-[240px] h-auto"
                              />
                              <div className="w-full h-[44px] mt-6 mb-4">
                                 <div className="text-[20px] font-semibold tracking-[0.2px] text-center">
                                    iPhone 16 Pro
                                 </div>
                              </div>
                              <div className="w-full h-fit mb-6 flex flex-row justify-center items-center gap-2">
                                 <div className="h-5 w-5 bg-[#0070f0] rounded-full"></div>
                                 <div className="h-5 w-5 bg-[#0070f0] rounded-full"></div>
                                 <div className="h-5 w-5 bg-[#0070f0] rounded-full"></div>
                              </div>
                              <div className="w-full h-fit mb-5 flex flex-row justify-center items-center gap-2">
                                 <Button className="w-[64px] h-fit px-2 py-1 border border-[#000] rounded-full text-[14px] font-medium  ">
                                    128 GB
                                 </Button>
                                 <Button className="w-[64px] h-fit px-2 py-1 border border-[#000] rounded-full text-[14px] font-medium  ">
                                    256 GB
                                 </Button>
                                 <Button className="w-[64px] h-fit px-2 py-1 border border-[#000] rounded-full text-[14px] font-medium  ">
                                    512 GB
                                 </Button>
                                 <Button className="w-[64px] h-fit px-2 py-1 border border-[#000] rounded-full text-[14px] font-medium  ">
                                    1 TB
                                 </Button>
                              </div>
                              <div className="w-full h-fit mb-4">
                                 <div className="text-[18px] font-semibold tracking-[0.2px] text-center">
                                    $1,928.00
                                 </div>
                              </div>
                              <Button className="w-full h-fit bg-[#000] hover:bg-[#333] text-white rounded-full text-[14px] font-medium tracking-[0.2px]">
                                 Add to Cart
                              </Button>
                              <Button className="w-full h-fit bg-white border border-[#000] text-black hover:bg-[#f5f5f7] rounded-full text-[14px] font-medium tracking-[0.2px] mt-auto">
                                 View Details
                              </Button>
                           </div>
                        </CarouselItem>
                     ))}
                  </CarouselContent>
                  <CarouselPrevious />
                  <CarouselNext />
               </Carousel>
            </div> */}
         </div>
      </div>
   );
};

export default CartPage;
