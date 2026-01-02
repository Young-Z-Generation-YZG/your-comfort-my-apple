'use client';

import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import Image from 'next/image';
import { FormProvider } from 'react-hook-form';

import images from '~/components/client/images';
import { BsExclamationCircle, BsCheckCircle } from 'react-icons/bs';
import { FaRegTrashAlt } from 'react-icons/fa';
import { Button } from '~/components/ui/button';
import { Input } from '~/components/ui/input';
import {
   Accordion,
   AccordionItem,
   AccordionTrigger,
   AccordionContent,
} from '~/components/ui/accordion';
import { LoadingOverlay } from '~/components/client/loading-overlay';
import { usePromoCode } from './_hooks/usePromoCode';
import CartItem from './_components/cart-item';
import { useRouter } from 'next/navigation';
import CheckboxField from '~/components/client/forms/checkbox-field';
import useBasketService from '~/hooks/api/use-basket-service';
import { useEffect, useMemo } from 'react';
import { TCartItem } from '~/domain/types/basket.type';
import { TReduxCartState } from '~/infrastructure/redux/features/cart.slice';
import { useAppSelector } from '~/infrastructure/redux/store';
import useCartSync from '~/hooks/use-cart-sync';
import useCartForm from './_hooks/useCartForm';
import { useDispatch } from 'react-redux';
import {
   clearCart,
   UpdateSelection,
   removeCartItem,
} from '~/infrastructure/redux/features/cart.slice';
import PopularProducts from '~/app/_components/popular-product-section';

const CartPage = () => {
   const router = useRouter();
   const dispatch = useDispatch();

   const { isAuthenticated } = useAppSelector((state) => state.auth);
   const cartAppState = useAppSelector((state) => state.cart);

   useCartSync({ autoSync: true });

   const {
      getBasketAsync,
      getBasketQueryState,
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

   // Fetch basket when authenticated, automatically applying coupon code from URL if present
   // This handles the case where user visits with ?_couponCode=XXX before logging in
   useEffect(() => {
      const fetchBasket = async () => {
         await getBasketAsync({
            _couponCode: urlCouponCode || null,
         });
      };

      if (isAuthenticated) {
         fetchBasket();
      }
   }, [getBasketAsync, urlCouponCode, isAuthenticated]);

   // Auto-clear coupon code from URL when server rejects coupon due to no selected items
   useEffect(() => {
      if (!isAuthenticated) return;
      if (!urlCouponCode) return;

      const err = getBasketQueryState.error as any;

      console.log('err', err);

      const errorCode = err?.data?.error?.code || '';

      const isNoSelectedItemsError = errorCode === 'Basket.NotSelectedItems';

      console.log('isNoSelectedItemsError', isNoSelectedItemsError);

      if (isNoSelectedItemsError) {
         handleRemovePromoCode();
      }
   }, [
      isAuthenticated,
      urlCouponCode,
      getBasketQueryState.error,
      handleRemovePromoCode,
   ]);

   const basketData = useMemo(() => {
      if (getBasketQueryState.isSuccess && getBasketQueryState.data)
         return getBasketQueryState.data as TReduxCartState;

      return {
         user_email: '',
         cart_items: cartAppState.cart_items,
         total_amount: 0,
         coupon_applied: null,
      } as TReduxCartState;
   }, [getBasketQueryState, cartAppState]);

   const { form, cartItems, handleQuantityChange, handleRemoveItem } =
      useCartForm({
         basketData: basketData,
         storeBasketAsync: storeBasketAsync,
         deleteBasketAsync: deleteBasketAsync,
      });

   useEffect(() => {
      form.reset({
         cart_items: basketData.cart_items.map((item) => ({
            is_selected: item.is_selected,
            model_id: item.model_id,
            sku_id: item.sku_id,
            color: item.color,
            model: item.model,
            storage: item.storage,
            quantity: item.quantity,
         })),
      });
   }, [basketData, form]);

   const handleItemSelectionChange = (item: TCartItem, checked: boolean) => {
      if (!item) {
         return;
      }

      dispatch(
         UpdateSelection({
            ...item,
            is_selected: checked,
         }),
      );
   };

   const selectedItems = form
      .getValues('cart_items')
      .filter((item) => item.is_selected);

   // If a coupon code is present but no items are selected, remove the coupon (do not auto-select items)
   // useEffect(() => {
   //    if (!isAuthenticated) return;
   //    if (!urlCouponCode) return;

   //    const items = form.getValues('cart_items');
   //    const hasAnySelected = items.some((i) => i.is_selected);
   //    if (!hasAnySelected) {
   //       handleRemovePromoCode();
   //    }
   // }, [isAuthenticated, urlCouponCode, form, handleRemovePromoCode]);

   // Validate and apply promo code (only when items are selected)
   const handleValidatedApplyPromoCode = () => {
      if (!isAuthenticated) {
         return;
      }

      if (!promoCode.trim() || selectedItems.length === 0) {
         return;
      }

      handleApplyPromoCode();
   };

   console.log('isAuthenticated', isAuthenticated);
   console.log('isLoading', isLoading);

   // Helper function to extract clean error message from potentially raw gRPC error format
   const getCleanErrorMessage = (errorMessage: string | null): string => {
      if (!errorMessage) return '';

      // If error contains gRPC Status format, extract the Detail
      const detailMatch = errorMessage.match(/Detail="([^"]+)"/);
      if (detailMatch && detailMatch[1]) {
         return detailMatch[1];
      }

      // If error contains StatusCode, try to extract a cleaner message
      if (errorMessage.includes('Status(')) {
         // Try to get text after Detail=
         const afterDetail = errorMessage.split('Detail=')[1];
         if (afterDetail) {
            const cleanMessage = afterDetail.replace(/[")]/g, '').trim();
            if (cleanMessage) return cleanMessage;
         }
      }

      // Return original message if no parsing needed
      return errorMessage;
   };

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col items-center justify-start bg-white',
         )}
      >
         <div className="max-w-[1156px] w-full grid grid-cols-12 mb-[200px]">
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
                           {basketData.cart_items.length > 0 &&
                              basketData.cart_items.map(
                                 (item: TCartItem, index: number) => (
                                    <div
                                       key={`${item.model_id}-${index}`}
                                       className="w-full flex flex-row justify-start items-center gap-3"
                                    >
                                       {isAuthenticated ? (
                                          <CheckboxField
                                             name={`cart_items.${index}.is_selected`}
                                             form={form}
                                             checkboxClassName="h-5 w-5"
                                             onCheckedChange={(checked) =>
                                                handleItemSelectionChange(
                                                   cartItems[index] || item,
                                                   checked,
                                                )
                                             }
                                          />
                                       ) : null}
                                       <CartItem
                                          item={item as unknown as TCartItem}
                                          currentCartItem={cartItems[index]}
                                          index={index}
                                          onQuantityChange={
                                             handleQuantityChange
                                          }
                                          onRemoveItem={() => {
                                             if (isAuthenticated) {
                                                handleRemoveItem(index);
                                             } else {
                                                dispatch(removeCartItem(index));
                                             }
                                          }}
                                       />
                                    </div>
                                 ),
                              )}
                        </form>
                     </FormProvider>
                  </LoadingOverlay>
                  {basketData.cart_items.length !== 0 && (
                     <Button
                        variant="outline"
                        className="w-full h-fit border-2 border-red-200 text-red-600 hover:bg-red-50 hover:border-red-300 
                                   rounded-full text-[14px] font-medium tracking-[0.2px] mt-3 
                                   transition-all duration-200 ease-in-out flex items-center justify-center gap-2
                                   disabled:opacity-50 disabled:cursor-not-allowed"
                        disabled={isLoading}
                        onClick={async () => {
                           if (isAuthenticated) {
                              await deleteBasketAsync();
                              await getBasketAsync({
                                 _couponCode: null,
                              });
                           } else {
                              dispatch(clearCart());
                           }
                        }}
                     >
                        <FaRegTrashAlt className="w-4 h-4" />
                        Clear Cart
                     </Button>
                  )}
               </div>
            </div>
            <div className="col-span-4 h-full relative">
               <div className="promotion w-full flex flex-col justify-start items-start pb-6 border-b border-[#dddddd]">
                  <div className="text-[15px] text-[#999999] font-normal tracking-[0.2px]">
                     Promo Code
                  </div>
                  <div className="w-full flex flex-row justify-between items-end ">
                     <div className="w-full flex flex-col justify-start items-start">
                        <Input
                           className="w-[200px] h-fit p-0 border-[#999999] border-t-0 border-l-0 border-r-0 border-b-1 rounded-none 
                        focus-visible:ring-0 focus-visible:ring-offset-0 text-[18px] font-light tracking-[0.2px]"
                           placeholder="Enter promo code"
                           value={promoCode}
                           onChange={handlePromoCodeChange}
                           disabled={!isAuthenticated || isLoading}
                        />
                        {!isAuthenticated ? (
                           <p className="text-sm font-light tracking-[0.2px] text-[#999999] pt-1">
                              Please login to apply a promo code
                           </p>
                        ) : null}
                     </div>
                     <Button
                        className="w-[80px] h-fit rounded-full text-[14px] font-medium tracking-[0.2px] transition-all duration-200 ease-in-out"
                        onClick={handleValidatedApplyPromoCode}
                        disabled={
                           !isAuthenticated ||
                           isLoading ||
                           !promoCode.trim() ||
                           selectedItems.length === 0
                        }
                     >
                        Apply
                     </Button>
                  </div>
                  {urlCouponCode ? (
                     isAuthenticated ? (
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
                        <div className="pt-1 flex items-center gap-2">
                           <p className="text-sm font-medium text-amber-600">
                              Coupon code will be applied after login:{' '}
                              {urlCouponCode}
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
                     )
                  ) : (
                     <p className="text-sm font-light tracking-[0.2px] text-[#999999] pt-1">
                        {selectedItems.length === 0 && isAuthenticated
                           ? 'Please select items to apply a promo code'
                           : ''}
                     </p>
                  )}
                  {basketData.coupon_applied && isAuthenticated && (
                     <div className="w-full pt-1 flex flex-col gap-1">
                        {basketData.coupon_applied.error_message && (
                           <div className="w-full flex flex-row items-start gap-2 p-3 bg-red-50 border border-red-200 rounded-lg mt-2">
                              <BsExclamationCircle className="h-5 w-5 text-red-600 flex-shrink-0 mt-0.5" />
                              <p className="text-sm font-medium tracking-[0.2px] text-red-700 flex-1 break-words">
                                 {getCleanErrorMessage(
                                    basketData.coupon_applied.error_message,
                                 )}
                              </p>
                           </div>
                        )}
                        {!basketData.coupon_applied.error_message && (
                           <div className="w-full mt-2 p-4 bg-green-50 border border-green-200 rounded-lg">
                              <div className="flex flex-row items-start gap-3 mb-3">
                                 <BsCheckCircle className="h-5 w-5 text-green-600 flex-shrink-0 mt-0.5" />
                                 <div className="flex-1">
                                    {basketData.coupon_applied.title && (
                                       <h4 className="text-base font-semibold tracking-[0.2px] text-green-800 mb-1">
                                          {basketData.coupon_applied.title}
                                       </h4>
                                    )}
                                    {basketData.coupon_applied.description && (
                                       <p className="text-sm font-normal tracking-[0.2px] text-green-700 mb-3">
                                          {
                                             basketData.coupon_applied
                                                .description
                                          }
                                       </p>
                                    )}
                                 </div>
                              </div>
                              <div className="flex flex-col gap-2 pl-8">
                                 {basketData.coupon_applied.discount_type && (
                                    <div className="flex flex-row items-center gap-2">
                                       <span className="text-xs font-medium text-green-600 min-w-[100px]">
                                          Discount:
                                       </span>
                                       <span className="text-sm font-semibold text-green-800">
                                          {
                                             basketData.coupon_applied
                                                .discount_value
                                          }
                                          {basketData.coupon_applied
                                             .discount_type === 'PERCENTAGE'
                                             ? '%'
                                             : ''}
                                       </span>
                                    </div>
                                 )}
                                 {basketData.coupon_applied
                                    .max_discount_amount !== null && (
                                    <div className="flex flex-row items-center gap-2">
                                       <span className="text-xs font-medium text-green-600 min-w-[100px]">
                                          Max Discount:
                                       </span>
                                       <span className="text-sm font-semibold text-green-800">
                                          $
                                          {basketData.coupon_applied.max_discount_amount.toFixed(
                                             2,
                                          )}
                                       </span>
                                    </div>
                                 )}
                                 {basketData.coupon_applied.expired_date && (
                                    <div className="flex flex-row items-center gap-2">
                                       <span className="text-xs font-medium text-green-600 min-w-[100px]">
                                          Expires:
                                       </span>
                                       <span className="text-sm font-normal text-gray-700">
                                          {new Date(
                                             basketData.coupon_applied.expired_date,
                                          ).toLocaleDateString('en-US', {
                                             year: 'numeric',
                                             month: 'short',
                                             day: 'numeric',
                                          })}
                                       </span>
                                    </div>
                                 )}
                              </div>
                           </div>
                        )}
                     </div>
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
                           {getBasketQueryState.data
                              ? getBasketQueryState.data.sub_total_amount.toFixed(
                                   2,
                                )
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
                                    {(
                                       getBasketQueryState.data as unknown as TReduxCartState
                                    )?.cart_items
                                       .reduce(
                                          (acc: number, item: TCartItem) =>
                                             acc + (item.discount_amount ?? 0),
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
                        {getBasketQueryState.data
                           ? (
                                getBasketQueryState.data as unknown as TReduxCartState
                             ).total_amount.toFixed(2)
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

         <div className="w-full px-4 py-8">
            {/* POPULAR PRODUCTS */}
            <PopularProducts />
         </div>
      </div>
   );
};

export default CartPage;
