/* eslint-disable react/react-in-jsx-scope */
'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { useEffect, useMemo, useState } from 'react';
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
import { ShippingAddressSelector } from '~/app/(pages)/(privates)/cart/_components/shipping-address-selector';
import { FiEdit3 } from 'react-icons/fi';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { useSearchParams } from 'next/navigation';
import { useForm } from 'react-hook-form';
import withAuth from '@components/HoCs/with-auth.hoc';
import useIdentityService from '@components/hooks/api/use-identity-service';
import useBasketService from '@components/hooks/api/use-basket-service';
import CheckoutItem from './_components/checkout-item';
import { ICartItem } from '~/domain/interfaces/baskets/basket.interface';

const fakeData = [
   {
      id: 'e0511627-9287-40a9-95bb-c6e67e5e64ce',
      label: 'Default',
      contact_name: 'USER USER',
      contact_phone_number: '0333284890',
      address_line: '',
      district: '',
      province: '',
      country: '',
      is_default: true,
   },
];

const fakeCheckoutCart = {
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

const fakeCheckoutResponse = {
   payment_redirect_url:
      'https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?vnp_Amount=2500000000&vnp_Command=pay&vnp_CreateDate=20251028225140&vnp_CurrCode=VND&vnp_IpAddr=127.0.0.1&vnp_Locale=vn&vnp_OrderInfo=ORDER_ID%3De824f6fa-92e4-4911-8b9e-98bd2ea4b82d&vnp_OrderType=VNPAY_CHECKOUT&vnp_ReturnUrl=http%3A%2F%2Flocalhost%3A3000%2Fcheckout%2Fpayment-callback&vnp_TmnCode=SB1TO3BK&vnp_TxnRef=638972887005197901&vnp_Version=2.1.0&vnp_SecureHash=695ee3da6712e246efa37fa73e51b7e3fd7a227940341b5f792a29b36d5e7bce457ebcf953cb810aacd501b251a5c5e4bdac62914c9da24e53f7a7556dc3fa7f',
   order_details_redirect_url: null,
};

export type TCheckoutResponse = typeof fakeCheckoutResponse;

export type TAddressItem = (typeof fakeData)[number];
export type TCheckoutCart = typeof fakeCheckoutCart;
export type TCheckoutItem = (typeof fakeCheckoutCart.cart_items)[number];

const CheckoutPage = () => {
   const searchParams = useSearchParams();
   //  const [isCheckoutLoading, setIsCheckoutLoading] = useState(false);
   const [editable, setEditable] = useState(false);
   const [selectedAddress, setSelectedAddress] = useState<TAddressItem | null>(
      null,
   );

   const appliedCouponFromQuery = searchParams.get('_couponCode');

   const {
      getAddressesAsync,
      getAddressesState,
      isLoading: isLoadingAddresses,
   } = useIdentityService();

   const {
      getCheckoutItemsAsync,
      checkoutBasketAsync,
      getCheckoutItemsState,
      isLoading: isLoadingBasket,
   } = useBasketService();

   useEffect(() => {
      const getAddresses = async () => {
         await getAddressesAsync();
      };

      getAddresses();
   }, [getAddressesAsync]);

   useEffect(() => {
      const getCheckoutItems = async () => {
         await getCheckoutItemsAsync({ _couponCode: appliedCouponFromQuery });
      };

      getCheckoutItems();
   }, [getCheckoutItemsAsync, appliedCouponFromQuery]);

   const defaultAddress = useMemo(() => {
      if (getAddressesState.data) {
         const getDefaultAddress = (
            getAddressesState.data as unknown as TAddressItem[]
         ).find((address) => address.is_default);

         if (getDefaultAddress) {
            return getDefaultAddress;
         }
      }

      return null;
   }, [getAddressesState]);

   const addressesList = useMemo(() => {
      if (getAddressesState.data) {
         return getAddressesState.data as unknown as TAddressItem[];
      }

      return [];
   }, [getAddressesState]);

   const form = useForm<CheckoutFormType>({
      resolver: CheckoutResolver,
      defaultValues: {
         shipping_address: {
            contact_name: defaultAddress?.contact_name || '',
            contact_phone_number: defaultAddress?.contact_phone_number || '',
            address_line: defaultAddress?.address_line || '',
            district: defaultAddress?.district || '',
            province: defaultAddress?.province || '',
            country: defaultAddress?.country || '',
         },
         payment_method: 'VNPAY',
         discount_code: null,
      },
   });

   const handleSelectAddress = (address: TAddressItem) => {
      form.setValue('shipping_address.contact_name', address.contact_name);
      form.setValue(
         'shipping_address.contact_phone_number',
         address.contact_phone_number,
      );
      form.setValue('shipping_address.address_line', address.address_line);
      form.setValue('shipping_address.district', address.district);
      form.setValue('shipping_address.province', address.province);
      form.setValue('shipping_address.country', address.country);

      setSelectedAddress(address);
   };

   useEffect(() => {
      if (defaultAddress) {
         setSelectedAddress(defaultAddress);
      }
   }, [defaultAddress]);

   const handleSubmit = async (data: CheckoutFormType) => {
      console.log('data', data);

      const result = await checkoutBasketAsync(data);

      console.log('result', result);

      if (result.isSuccess) {
         window.location.href = (
            result.data as unknown as TCheckoutResponse
         ).payment_redirect_url;
      }
   };

   //  useEffect(() => {
   //     if (isSuccessCheckoutBasket && dataCheckoutBasket) {
   //        setTimeout(() => {
   //           setIsLoading(false);
   //        }, 1000);

   //        dispatch(deleteCart());

   //        if (
   //           typeof dataCheckoutBasket.payment_redirect_url === 'string' &&
   //           dataCheckoutBasket.payment_redirect_url
   //        ) {
   //           window.location.href = dataCheckoutBasket.payment_redirect_url; // Redirect to the VNPAY payment URL
   //        } else if (
   //           typeof dataCheckoutBasket.order_details_redirect_url === 'string' &&
   //           dataCheckoutBasket.order_details_redirect_url
   //        ) {
   //           window.location.href =
   //              dataCheckoutBasket.order_details_redirect_url; // Redirect to the order details page
   //        }
   //     }
   //  }, [isSuccessCheckoutBasket]);

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col items-center justify-start bg-white',
         )}
      >
         <LoadingOverlay isLoading={false} fullScreen />
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
                                 selectedAddress={selectedAddress}
                                 onSelectAddress={handleSelectAddress}
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
                                       'bg-slate-200/50 hover:bg-blue-500 hover:text-white':
                                          !editable,
                                       'bg-blue-500 hover:bg-blue-500/80 text-white':
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
                                 disabled={!editable}
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
                                 disabled={!editable}
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
                                       'bg-slate-200/50 hover:bg-blue-500 hover:text-white':
                                          !editable,
                                       'bg-blue-500 hover:bg-blue-500/80 text-white':
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
                                 disabled={!editable}
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
                                 disabled={!editable}
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
                                 disabled={!editable}
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
                                 disabled={!editable}
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
                        You have {0} items in your cart
                     </div>
                     <div className="w-full flex flex-col justify-start items-center">
                        {getCheckoutItemsState.data &&
                           getCheckoutItemsState.data.cart_items.length > 0 &&
                           getCheckoutItemsState.data.cart_items.map(
                              (item: ICartItem, index) => {
                                 return (
                                    <CheckoutItem
                                       key={index}
                                       item={item as unknown as TCheckoutItem}
                                    />
                                 );
                              },
                           )}
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
                              $
                              {getCheckoutItemsState.data &&
                                 getCheckoutItemsState.data.cart_items.length >
                                    0 &&
                                 getCheckoutItemsState.data.cart_items
                                    .reduce(
                                       (acc, item) =>
                                          acc + item.unit_price * item.quantity,
                                       0,
                                    )
                                    .toFixed(2)}
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
                                       - $
                                       {getCheckoutItemsState.data &&
                                          getCheckoutItemsState.data.cart_items
                                             .length > 0 &&
                                          getCheckoutItemsState.data.cart_items
                                             .reduce(
                                                (acc, item) =>
                                                   acc +
                                                   ((
                                                      item as unknown as TCheckoutItem
                                                   ).promotion
                                                      ?.discount_amount ?? 0) *
                                                      (
                                                         item as unknown as TCheckoutItem
                                                      ).quantity,
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
                           {(getCheckoutItemsState.data &&
                              getCheckoutItemsState.data.cart_items.length >
                                 0 &&
                              getCheckoutItemsState.data.total_amount.toFixed(
                                 2,
                              )) ??
                              '0.00'}
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
