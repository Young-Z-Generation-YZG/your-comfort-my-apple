/* eslint-disable react/react-in-jsx-scope */
'use client';
import '/globals.css';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { useEffect, useState } from 'react';
import Image from 'next/image';
import images from '@components/client/images';
import { Button } from '@components/ui/button';
import {
   Accordion,
   AccordionItem,
   AccordionTrigger,
   AccordionContent,
} from '@components/ui/accordion';
import {
   CheckoutFormType,
   CheckoutResolver,
} from '~/domain/schemas/basket.schema';
import { FieldInput } from '@components/client/forms/field-input';
import CardWrapper from './_components/card-wrapper';
import { Separator } from '@components/ui/separator';
import { PaymentMethodSelector } from '@components/client/forms/payment-method-selector';
import { motion } from 'framer-motion';
import { ShippingAddressSelector } from '@components/client/forms/shipping-address-selector';
import { FiEdit3 } from 'react-icons/fi';
import {
   useCheckoutBasketAsyncMutation,
   useGetBasketAsyncQuery,
} from '~/infrastructure/services/basket.service';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { ICartItemResponse } from '~/domain/interfaces/baskets/basket.interface';
import CartItem from './_components/cart-item';
import { useSearchParams } from 'next/navigation';
import { useGetAddressesAsyncQuery } from '~/infrastructure/services/identity.service';
import { IAddressResponse } from '~/domain/interfaces/identity/address';
import { useForm } from 'react-hook-form';
import withAuth from '@components/HoCs/with-auth.hoc';

const shippingAddresses = [
   {
      id: '1',
      isDefault: true,
      contactName: 'Foo Bar',
      contactPhoneNumber: '+84 123456789',
      addressLine: '106* Kha Van Can',
      district: 'Thu Duc',
      province: 'Ho Chi Minh City',
      country: 'Vietnam',
   },
];

const CheckoutPage = () => {
   const searchParams = useSearchParams();
   const [isLoading, setIsLoading] = useState(false);
   const [editable, setEditable] = useState(false);
   const [addressesList, setAddressesList] = useState<IAddressResponse[]>([]);
   const [selectedAddress, setSelectedAddress] =
      useState<IAddressResponse | null>(null);
   const [cartItems, setCartItems] = useState<ICartItemResponse[]>([]);
   const [totalDiscount, setTotalDiscount] = useState(0);
   const [subtotal, setSubtotal] = useState(0);

   const appliedCouponFromQuery = searchParams.get('_couponCode');

   const form = useForm<CheckoutFormType>({
      resolver: CheckoutResolver,
      defaultValues: {
         shipping_address: {
            contact_name: selectedAddress?.contact_name,
            contact_phone_number: selectedAddress?.contact_phone_number,
            address_line: selectedAddress?.address_line || '',
            district: selectedAddress?.district || '',
            province: selectedAddress?.province || '',
            country: selectedAddress?.country || '',
         },
         payment_method: 'VNPAY',
         discount_code: null,
         sub_total_amount: 0,
         discount_amount: 0,
         total_amount: 0,
      },
   });

   const {
      data: addressesDataAsync,
      isLoading: isLoadingAddresses,
      isFetching: isFetchingAddresses,
      isSuccess: isSuccessAddresses,
      error: errorAddresses,
      refetch: refetchAddresses,
   } = useGetAddressesAsyncQuery();

   const {
      data: basketData,
      isLoading: isBasketLoading,
      isError: isErrorBasket,
      error: errorBasket,
      refetch: refetchBasket,
   } = useGetBasketAsyncQuery({
      _couponCode: appliedCouponFromQuery,
   });

   const [
      checkoutBasket,
      {
         data: dataCheckoutBasket,
         isSuccess: isSuccessCheckoutBasket,
         isLoading: isLoadingCheckoutBasket,
         isError: isErrorCheckoutBasket,
         error: errorCheckoutBasket,
      },
   ] = useCheckoutBasketAsyncMutation();

   const handleSubmit = async (data: CheckoutFormType) => {
      console.log('data', data);

      setIsLoading(true);

      await checkoutBasket(data).unwrap();
   };

   useEffect(() => {
      if (isSuccessCheckoutBasket && dataCheckoutBasket) {
         setTimeout(() => {
            setIsLoading(false);
         }, 1000);

         if (
            typeof dataCheckoutBasket.payment_redirect_url === 'string' &&
            dataCheckoutBasket.payment_redirect_url.startsWith('https://')
         ) {
            window.location.href = dataCheckoutBasket.payment_redirect_url; // Redirect to the VNPAY payment URL
         } else if (
            typeof dataCheckoutBasket.payment_redirect_url === 'string' &&
            dataCheckoutBasket.order_details_redirect_url
         ) {
            window.location.href =
               dataCheckoutBasket.order_details_redirect_url; // Redirect to the order details page
         }
      }
   }, [isSuccessCheckoutBasket]);

   useEffect(() => {
      if (isSuccessAddresses) {
         setAddressesList(addressesDataAsync);

         console.log('addressesDataAsync', addressesDataAsync);

         var defaultValue = addressesDataAsync.find((addr) => addr.is_default);

         if (defaultValue) {
            setSelectedAddress(defaultValue);

            form.setValue(
               'shipping_address.contact_name',
               defaultValue.contact_name,
            );
            form.setValue(
               'shipping_address.contact_phone_number',
               defaultValue.contact_phone_number,
            );
            form.setValue(
               'shipping_address.address_line',
               defaultValue.address_line,
            );
            form.setValue('shipping_address.district', defaultValue.district);
            form.setValue('shipping_address.province', defaultValue.province);
            form.setValue('shipping_address.country', defaultValue.country);
         } else {
            if (addressesList.length > 0) {
               setSelectedAddress(addressesList[0]);

               form.setValue(
                  'shipping_address.contact_name',
                  addressesList[0].contact_name,
               );
               form.setValue(
                  'shipping_address.contact_phone_number',
                  addressesList[0].contact_phone_number,
               );
               form.setValue(
                  'shipping_address.address_line',
                  addressesList[0].address_line,
               );
               form.setValue(
                  'shipping_address.district',
                  addressesList[0].district,
               );
               form.setValue(
                  'shipping_address.province',
                  addressesList[0].province,
               );
               form.setValue(
                  'shipping_address.country',
                  addressesList[0].country,
               );
            }
         }
      }
   }, [addressesDataAsync]);

   useEffect(() => {
      if (basketData) {
         setCartItems(basketData.cart_items);

         if (basketData.cart_items.length > 0) {
            const subtotal = basketData.cart_items.reduce(
               (acc, item) => acc + item.product_unit_price * item.quantity,
               0,
            );

            let dcTotal = 0;

            basketData.cart_items.forEach((item) => {
               if (item.promotion) {
                  dcTotal +=
                     (item.product_unit_price -
                        item.promotion.promotion_discount_unit_price) *
                     item.quantity;
               }
            });

            setSubtotal(subtotal);
            setTotalDiscount(dcTotal);

            form.setValue('sub_total_amount', Number(subtotal.toFixed(2)));
            form.setValue('discount_amount', Number(dcTotal.toFixed(2)));
            form.setValue(
               'total_amount',
               Number((subtotal - dcTotal).toFixed(2)),
            );
         }
      }
   }, [basketData]);

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col items-center justify-start bg-white',
         )}
      >
         <LoadingOverlay
            isLoading={isLoadingCheckoutBasket || isLoading}
            fullScreen
         />
         <div className="max-w-[1156px] w-full grid grid-cols-12 mt-10">
            <div className="col-span-8 pr-[64px]">
               <div className="">
                  <CardWrapper className="mt-6">
                     <div className="px-3 py-4 bg-[#F9F9F9] rounded-md">
                        <div>
                           <span className="font-SFProText text-lg font-medium">
                              Shipping Address
                           </span>
                        </div>

                        <div className="mt-3">
                           <motion.div
                              className="lg:col-span-2 space-y-8"
                              initial={{ opacity: 0, y: 20 }}
                              animate={{ opacity: 1, y: 0 }}
                              transition={{ duration: 0.5 }}
                           >
                              <ShippingAddressSelector
                                 addresses={addressesList}
                                 selectedAddress={
                                    selectedAddress ||
                                    addressesList[0] ||
                                    shippingAddresses[0]
                                 }
                                 setSelectedAddress={setSelectedAddress}
                                 setValue={form.setValue}
                              />
                           </motion.div>
                        </div>
                     </div>

                     <Separator className="mt-3 bg-slate-200" />
                  </CardWrapper>

                  <form
                     onSubmit={form.handleSubmit(handleSubmit)}
                     className="mt-5"
                  >
                     <CardWrapper>
                        <div className="px-3 py-4 bg-[#F9F9F9] rounded-md">
                           <div className="flex justify-between">
                              <span className="font-SFProText text-lg font-medium">
                                 Shipping Information
                              </span>

                              <div
                                 onClick={() => {
                                    setEditable(!editable);
                                 }}
                                 className={cn(
                                    'flex flex-row items-center gap-2 font-SFProText text-sm font-medium cursor-pointer bg-slate-200/50 transition-all duration-200 ease-in-out active:bg-slate-200/50 select-none rounded-full px-2 py-1',
                                    {
                                       'bg-slate-200/50 hover:bg-slate-200':
                                          !editable,
                                       'bg-blue-500 hover:bg-blue-500/50 text-white':
                                          editable,
                                    },
                                 )}
                              >
                                 <span>edit</span>
                                 <span>
                                    <FiEdit3 />
                                 </span>
                              </div>
                           </div>
                           <div className="w-full mt-3">
                              <FieldInput<CheckoutFormType>
                                 form={form}
                                 name="shipping_address.contact_name"
                                 label="Contact Name"
                                 className="rounded-xl w-full"
                                 type="text"
                                 disabled={!editable || isLoading}
                                 errorTextClassName="pb-1"
                              />
                           </div>

                           <div className="w-full mt-3">
                              <FieldInput
                                 form={form}
                                 name="shipping_address.contact_phone_number"
                                 label="Phone Number"
                                 className="rounded-xl w-full"
                                 type="text"
                                 disabled={!editable || isLoading}
                                 errorTextClassName="pb-1"
                              />
                           </div>
                        </div>

                        <Separator className="mt-3 bg-slate-200" />

                        <div className="px-3 py-4 bg-[#F9F9F9] rounded-md mt-3">
                           <div className="flex justify-between">
                              <span className="font-SFProText text-lg font-medium">
                                 Shipping Address
                              </span>

                              <div
                                 onClick={() => {
                                    setEditable(!editable);
                                 }}
                                 className={cn(
                                    'flex flex-row items-center gap-2 font-SFProText text-sm font-medium cursor-pointer bg-slate-200/50 transition-all duration-200 ease-in-out active:bg-slate-200/50 select-none rounded-full px-2 py-1',
                                    {
                                       'bg-slate-200/50 hover:bg-slate-200':
                                          !editable,
                                       'bg-blue-500 hover:bg-blue-500/50 text-white':
                                          editable,
                                    },
                                 )}
                              >
                                 <span>edit</span>
                                 <span>
                                    <FiEdit3 />
                                 </span>
                              </div>
                           </div>
                           <div className="w-full mt-3">
                              <FieldInput
                                 form={form}
                                 name="shipping_address.address_line"
                                 label="Address Line"
                                 className="rounded-xl w-full"
                                 type="text"
                                 disabled={!editable || isLoading}
                                 errorTextClassName="pb-1"
                              />
                           </div>

                           <div className="w-full mt-3">
                              <FieldInput
                                 form={form}
                                 name="shipping_address.district"
                                 label="District"
                                 className="rounded-xl w-full"
                                 type="text"
                                 disabled={!editable || isLoading}
                                 errorTextClassName="pb-1"
                              />
                           </div>

                           <div className="w-full mt-3">
                              <FieldInput
                                 form={form}
                                 name="shipping_address.province"
                                 label="Province/City"
                                 className="rounded-xl w-full"
                                 type="text"
                                 disabled={!editable || isLoading}
                                 errorTextClassName="pb-1"
                              />
                           </div>

                           <div className="w-full mt-3">
                              <FieldInput
                                 form={form}
                                 name="shipping_address.country"
                                 label="Country"
                                 className="rounded-xl w-full"
                                 type="text"
                                 disabled={!editable || isLoading}
                                 errorTextClassName="pb-1"
                              />
                           </div>
                        </div>
                        <Separator className="mt-3 bg-slate-200" />
                     </CardWrapper>

                     <CardWrapper className="mt-6">
                        <div className="px-3 py-4 bg-[#F9F9F9] rounded-md">
                           <span className="font-SFProText text-lg font-medium">
                              Payment Method
                           </span>

                           <div>
                              <PaymentMethodSelector
                                 register={form.register}
                                 errors={form.formState.errors}
                                 watch={form.watch}
                              />
                           </div>
                        </div>

                        <Separator className="mt-3 bg-slate-200" />
                     </CardWrapper>
                  </form>
               </div>
            </div>

            {/* ORDER SUMMARY */}
            <div className="col-span-4 h-full">
               <div className="w-full h-fit flex flex-col justify-start items-start border border-[#dddddd] rounded-[10px] p-[30px]">
                  <div className="order-summary w-full flex flex-col justify-start items-start">
                     <div className="w-full pb-3 text-[22px] text-black font-bold tracking-[0.8px] font-SFProText">
                        Order Summary
                     </div>
                     <div className="text-[16px] font-light tracking-[0.2px] font-SFProText">
                        You have {cartItems.length} items in your cart
                     </div>
                     <div className="w-full flex flex-col justify-start items-center">
                        {cartItems.map((item) => {
                           return (
                              <CartItem key={item.product_id} item={item} />
                           );
                        })}
                     </div>
                  </div>
                  <div className="summary w-full flex flex-col justify-start items-start py-6 border-b border-[#dddddd]">
                     <div className="w-full pb-3 text-[22px] text-black font-bold tracking-[0.8px]">
                        Summary
                     </div>
                     <div className="w-full flex flex-col justify-between">
                        <div className="w-full flex flex-row justify-between items-center text-[14px] tracking-[0.2px]">
                           <div className="font-light">Subtotal</div>
                           <div className="font-semibold">
                              ${subtotal.toFixed(2)}
                           </div>
                        </div>
                        <Accordion type="multiple" className="w-full h-full ">
                           <AccordionItem
                              value="item-1"
                              className="border-none"
                           >
                              <AccordionTrigger className="hover:no-underline text-[14px] font-semibold tracking-[0.2px] pb-0 pt-0">
                                 Total Savings
                              </AccordionTrigger>
                              <AccordionContent>
                                 <div className="pt-1 pl-1 w-full flex flex-row justify-between items-center text-[14px] tracking-[0.2px]">
                                    <div className="font-light">
                                       Other Discount
                                    </div>
                                    <div className="font-semibold">
                                       - ${totalDiscount.toFixed(2)}
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
                           ${basketData?.total_amount.toFixed(2) ?? '0.00'}
                        </div>
                     </div>
                     <Button
                        className="w-full h-fit bg-[#0070f0] text-white rounded-full text-[14px] font-medium tracking-[0.2px] mt-5"
                        onClick={() => {
                           form.handleSubmit(handleSubmit)();
                        }}
                     >
                        Place Order
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
         </div>
      </div>
   );
};

export default withAuth(CheckoutPage);
