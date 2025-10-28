'use client';
import '/globals.css';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import Image from 'next/image';
import { FormProvider } from 'react-hook-form';

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
import { useCartForm } from './_hooks/useCartForm';
import { usePromoCode } from './_hooks/usePromoCode';
import CartItem, { TCartItem } from './_components/cart-item';
import { ICartItem } from '~/domain/interfaces/baskets/basket.interface';
import { useRouter } from 'next/navigation';
import CheckboxField from '@components/client/forms/checkbox-field';
import useBasketService from '@components/hooks/api/use-basket-service';
import { useEffect } from 'react';

const fakeData = {
   user_email: 'user@gmail.com',
   cart_items: [
      {
         is_selected: true,
         model_id: '664351e90087aa09993f5ae7',
         product_name: 'iPhone 15 128GB BLUE',
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '',
            showcase_image_id: '',
            order: 0,
         },
         model: {
            name: 'iPhone 15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1000,
         quantity: 2,
         sub_total_amount: 1800.0,
         promotion: {
            promotion_id: '550e8400-e29b-41d4-a716-446655440000',
            promotion_type: 'COUPON',
            product_unit_price: 1000,
            discount_type: 'PERCENTAGE',
            discount_value: 0.1,
            discount_amount: 100.0,
            final_price: 900.0,
         },
         index: 1,
      },
   ],
   total_amount: 1800.0,
};

const CartPage = () => {
   const router = useRouter();

   const {
      getBasketAsync,
      getBasketState,
      storeBasketAsync,
      deleteBasketAsync,
      proceedToCheckoutAsync,
      isLoading,
   } = useBasketService();

   // Promo code management
   const {
      promoCode,
      urlCouponCode,
      handleApplyPromoCode,
      handlePromoCodeChange,
      handleRemovePromoCode,
   } = usePromoCode();

   useEffect(() => {
      const fetchBasket = async () => {
         const result = await getBasketAsync({
            _couponCode: urlCouponCode,
         });
         if (result.isSuccess) {
            console.log(result.data);
         }
      };
      fetchBasket();
   }, [getBasketAsync, urlCouponCode]);

   // Cart form management with auto-save
   const { form, selectedItems, handleQuantityChange, handleRemoveItem } =
      useCartForm({
         basketData: getBasketState.data,
         storeBasket: storeBasketAsync,
         deleteBasket: deleteBasketAsync,
      });

   // Validate and apply promo code (only when items are selected)
   const handleValidatedApplyPromoCode = () => {
      if (selectedItems.length === 0) {
         return;
      }
      handleApplyPromoCode();
   };

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
                  <LoadingOverlay isLoading={isLoading}>
                     <FormProvider {...form}>
                        <form>
                           {getBasketState.data?.cart_items.map(
                              (item: ICartItem, index: number) => (
                                 <div
                                    key={`${item.model_id}-${index}`}
                                    className="w-full flex flex-row justify-start items-center gap-3"
                                 >
                                    <CheckboxField
                                       name={`cart_items.${index}.is_selected`}
                                       form={form}
                                       checkboxClassName="h-5 w-5"
                                    />
                                    <CartItem
                                       item={item as unknown as TCartItem}
                                       index={index}
                                       onQuantityChange={handleQuantityChange}
                                       onRemoveItem={handleRemoveItem}
                                    />
                                 </div>
                              ),
                           )}
                        </form>
                     </FormProvider>
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
                        value={promoCode}
                        onChange={handlePromoCodeChange}
                        disabled={isLoading}
                     />
                     <Button
                        className="w-[80px] h-fit rounded-full text-[14px] font-medium tracking-[0.2px] transition-all duration-200 ease-in-out"
                        onClick={handleValidatedApplyPromoCode}
                        disabled={
                           isLoading ||
                           !promoCode.trim() ||
                           selectedItems.length === 0
                        }
                     >
                        Apply
                     </Button>
                  </div>
                  {urlCouponCode ? (
                     <div className="pt-1 flex items-center gap-2">
                        <p className="text-sm font-medium text-green-600">
                           Applied: {urlCouponCode}
                        </p>
                        <Button
                           variant="ghost"
                           size="sm"
                           className="h-6 px-2 text-xs text-red-500 hover:text-red-700"
                           onClick={handleRemovePromoCode}
                        >
                           Remove
                        </Button>
                     </div>
                  ) : (
                     <p className="text-sm font-light tracking-[0.2px] text-[#999999] pt-1">
                        {selectedItems.length === 0
                           ? 'Please select items to apply a promo code'
                           : ''}
                     </p>
                  )}
               </div>
               <div className="summary w-full flex flex-col justify-start items-start py-6 border-b border-[#dddddd]">
                  <div className="w-full pb-3 text-[22px] text-black font-bold tracking-[0.8px]">
                     Summary
                  </div>
                  <div className="w-full flex flex-col justify-between">
                     <div className="w-full flex flex-row justify-between items-center text-[14px] tracking-[0.2px]">
                        <div className="font-light">Subtotal</div>
                        <div className="font-semibold">
                           $
                           {getBasketState.data
                              ? getBasketState.data.total_amount.toFixed(2)
                              : '0.00'}
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
                                    - $
                                    {getBasketState.data?.cart_items
                                       .reduce(
                                          (acc, item) =>
                                             acc +
                                             ((item as unknown as TCartItem)
                                                .promotion?.discount_amount ??
                                                0) *
                                                (item as unknown as TCartItem)
                                                   .quantity,
                                          0,
                                       )
                                       .toFixed(2)}
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
                     <div className="">
                        $
                        {getBasketState.data
                           ? getBasketState.data.total_amount.toFixed(2)
                           : '0.00'}
                     </div>
                  </div>
                  <Button
                     className="w-full h-fit border rounded-full text-[14px] font-medium tracking-[0.2px] mt-5"
                     disabled={isLoading || selectedItems.length === 0}
                     onClick={async () => {
                        const result = await proceedToCheckoutAsync();

                        if (result.data === true) {
                           const checkoutUrl = urlCouponCode
                              ? `/checkout?_couponCode=${urlCouponCode}`
                              : '/checkout';
                           router.push(checkoutUrl);
                        }
                     }}
                  >
                     Checkout{' '}
                     {selectedItems.length > 0 && `(${selectedItems.length})`}
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
