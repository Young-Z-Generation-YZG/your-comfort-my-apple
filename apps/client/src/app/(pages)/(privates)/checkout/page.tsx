/* eslint-disable react/react-in-jsx-scope */
'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { useEffect, useMemo, useState } from 'react';
import Image from 'next/image';
import images from '~/components/client/images';
import { Button } from '~/components/ui/button';
import {
   Accordion,
   AccordionItem,
   AccordionTrigger,
   AccordionContent,
} from '~/components/ui/accordion';
import {
   CheckoutFormType,
   CheckoutResolver,
} from '~/domain/schemas/basket.schema';
import { FieldInput } from '~/components/client/forms/field-input';
import CardWrapper from './_components/card-wrapper';
import { Separator } from '~/components/ui/separator';
import { PaymentMethodSelector } from '~/components/client/forms/payment-method-selector';
import { motion } from 'framer-motion';
import { ShippingAddressSelector } from '~/app/(pages)/(privates)/cart/_components/shipping-address-selector';
import { FiEdit3 } from 'react-icons/fi';
import { LoadingOverlay } from '~/components/client/loading-overlay';
import { useRouter, useSearchParams } from 'next/navigation';
import { useForm } from 'react-hook-form';
import withAuth from '~/components/HoCs/with-auth.hoc';
import useIdentityService from '~/hooks/api/use-identity-service';
import useBasketService from '~/hooks/api/use-basket-service';
import CheckoutItem from './_components/checkout-item';
import { Check } from 'lucide-react';
import svgs from '@assets/svgs';
import { WalletConnectButton } from '~/components/client/wallet-connect-button';
import BlockchainPaymentModel from './_components/blockchain-payment-model';
import { EPaymentType } from '~/domain/enums/payment-type.enum';
import { useSolana } from '~/components/providers/solana-provider';
import { toast } from 'sonner';
import { CleanSelectedItems } from '~/infrastructure/redux/features/cart.slice';
import { useDispatch } from 'react-redux';
import { TCheckoutBasketItem } from '~/domain/types/basket.type';

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

const fakeCheckoutResponse = {
   order_id: 'b384e952-bb28-4f97-8e04-f2f055481d77',
   cart_items: [
      {
         is_selected: true,
         model_id: '664351e90087aa09993f5ae7',
         product_name: 'iPhone 15 128GB Blue',
         color: {
            name: 'Blue',
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
         quantity: 1,
         sub_total_amount: 1000,
         promotion: null,
         index: 1,
      },
   ],
   payment_redirect_url: 'string',
   order_details_redirect_url:
      '/account/orders/b384e952-bb28-4f97-8e04-f2f055481d77',
};

export type TCheckoutResponse = typeof fakeCheckoutResponse;

export type TAddressItem = (typeof fakeData)[number];

const CheckoutPage = () => {
   const router = useRouter();
   const searchParams = useSearchParams();
   const [editable, setEditable] = useState(false);
   const [selectedAddress, setSelectedAddress] = useState<TAddressItem | null>(
      null,
   );
   const [displayPaymentBlockchainModel, setDisplayPaymentBlockchainModel] =
      useState(false);

   const { isConnected } = useSolana();

   const dispatch = useDispatch();

   const handleClosePaymentModal = () => {
      setDisplayPaymentBlockchainModel(false);
   };

   const appliedCouponFromQuery = searchParams.get('_couponCode');

   const {
      getAddressesAsync,
      getAddressesState,
      isLoading: isLoadingAddresses,
   } = useIdentityService();

   const {
      getCheckoutItemsAsync,
      checkoutBasketAsync,
      getCheckoutItemsQueryState,
      isLoading: isLoadingBasket,
   } = useBasketService();

   const totalCartQuantity = useMemo(() => {
      if (getCheckoutItemsQueryState.data) {
         return getCheckoutItemsQueryState.data.cart_items.reduce(
            (acc, item) => acc + (item.quantity ?? 0),
            0,
         );
      }

      return 0;
   }, [getCheckoutItemsQueryState.data]);

   const isLoading = useMemo(() => {
      return isLoadingAddresses || isLoadingBasket;
   }, [isLoadingAddresses, isLoadingBasket]);

   useEffect(() => {
      getAddressesAsync();
   }, [getAddressesAsync]);

   useEffect(() => {
      getCheckoutItemsAsync({ _couponCode: appliedCouponFromQuery });
   }, [getCheckoutItemsAsync, appliedCouponFromQuery]);

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
            contact_name: '',
            contact_phone_number: '',
            address_line: '',
            district: '',
            province: '',
            country: '',
         },
         payment_method: undefined,
         discount_code: appliedCouponFromQuery || null,
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
      if (getAddressesState.isSuccess && getAddressesState.data) {
         const defaultAddress = getAddressesState.data.find(
            (address) => address.is_default,
         );

         if (defaultAddress) {
            setSelectedAddress(defaultAddress);

            form.setValue(
               'shipping_address.contact_name',
               defaultAddress.contact_name,
            );
            form.setValue(
               'shipping_address.contact_phone_number',
               defaultAddress.contact_phone_number,
            );
            form.setValue(
               'shipping_address.address_line',
               defaultAddress.address_line,
            );
            form.setValue('shipping_address.district', defaultAddress.district);
            form.setValue('shipping_address.province', defaultAddress.province);
            form.setValue('shipping_address.country', defaultAddress.country);
         }
      }
   }, [getAddressesState, form]);

   const handleSubmit = async (data: CheckoutFormType) => {
      const result = await checkoutBasketAsync(data);

      if (result.isSuccess) {
         dispatch(CleanSelectedItems());

         if (
            (result.data as unknown as TCheckoutResponse).payment_redirect_url
         ) {
            window.location.href = (
               result.data as unknown as TCheckoutResponse
            ).payment_redirect_url;
         } else if (
            (result.data as unknown as TCheckoutResponse)
               .order_details_redirect_url
         ) {
            router.replace(
               (result.data as unknown as TCheckoutResponse)
                  .order_details_redirect_url,
            );
         }
      }
   };

   useEffect(() => {
      if (appliedCouponFromQuery) {
         form.setValue('discount_code', appliedCouponFromQuery);
      }
   }, [appliedCouponFromQuery, form]);

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col items-center justify-start bg-white',
         )}
      >
         <LoadingOverlay isLoading={isLoading} fullScreen />
         {displayPaymentBlockchainModel && (
            <BlockchainPaymentModel
               form={form}
               isOpen={displayPaymentBlockchainModel}
               onClose={handleClosePaymentModal}
               amount={
                  getCheckoutItemsQueryState.data?.total_amount?.toFixed(2) ||
                  '0.00'
               }
            />
         )}
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
                                 className={cn('rounded-xl w-full', {
                                    'border-gray-500 bg-gray-300/50 text-gray-500':
                                       !editable,
                                 })}
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
                                 className={cn('rounded-xl w-full', {
                                    'border-gray-500 bg-gray-300/50 text-gray-500':
                                       !editable,
                                 })}
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
                                 className={cn('rounded-xl w-full', {
                                    'border-gray-500 bg-gray-300/50 text-gray-500':
                                       !editable,
                                 })}
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
                                 className={cn('rounded-xl w-full', {
                                    'border-gray-500 bg-gray-300/50 text-gray-500':
                                       !editable,
                                 })}
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
                                 className={cn('rounded-xl w-full', {
                                    'border-gray-500 bg-gray-300/50 text-gray-500':
                                       !editable,
                                 })}
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
                                 className={cn('rounded-xl w-full', {
                                    'border-gray-500 bg-gray-300/50 text-gray-500':
                                       !editable,
                                 })}
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
                              <div className="relative mt-5">
                                 <input
                                    type="radio"
                                    {...form.register('payment_method')}
                                    id="payment-solana"
                                    value={EPaymentType.SOLANA.toString()}
                                    className="peer sr-only"
                                 />
                                 <label
                                    htmlFor="payment-solana"
                                    className="flex flex-col items-center justify-between rounded-md border-2 border-gray-200 bg-white p-4 cursor-pointer hover:border-gray-300 peer-checked:border-gray-900 peer-checked:bg-gray-50 transition-all duration-200 ease-in-out"
                                 >
                                    <div className="mb-2 rounded-full bg-gray-100 p-2">
                                       <Image
                                          src={
                                             svgs.solanaLogo ||
                                             '/placeholder.svg'
                                          }
                                          alt="SOLANA"
                                          width={24}
                                          height={24}
                                          className="h-6 w-6"
                                       />
                                    </div>
                                    <div className="font-medium select-none font-SFProText">
                                       Solana
                                    </div>
                                    <div className="text-xs text-gray-500 text-center mt-1 select-none">
                                       Blockchain payment
                                    </div>
                                    <div className="mt-5">
                                       <WalletConnectButton />
                                    </div>
                                 </label>
                                 <Check className="absolute top-4 right-4 h-5 w-5 text-gray-900 opacity-0 peer-checked:opacity-100" />
                              </div>
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
                        You have {totalCartQuantity} items in your cart
                     </div>
                     <div className="w-full flex flex-col justify-start items-center">
                        {getCheckoutItemsQueryState.data &&
                        getCheckoutItemsQueryState.data.cart_items.length >
                           0 ? (
                           getCheckoutItemsQueryState.data.cart_items.map(
                              (item: TCheckoutBasketItem, index: number) => {
                                 return (
                                    <CheckoutItem key={index} item={item} />
                                 );
                              },
                           )
                        ) : (
                           <div className="w-full flex flex-col justify-start items-center">
                              <div className="text-[14px] font-light tracking-[0.2px] font-SFProText">
                                 No items in your cart
                              </div>
                           </div>
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
                              {getCheckoutItemsQueryState.data &&
                              getCheckoutItemsQueryState.data.cart_items
                                 .length > 0
                                 ? getCheckoutItemsQueryState.data.cart_items
                                      .reduce(
                                         (
                                            acc: number,
                                            item: TCheckoutBasketItem,
                                         ) =>
                                            acc +
                                            item.unit_price * item.quantity,
                                         0,
                                      )
                                      .toFixed(2)
                                 : '0.00'}
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
                                       {getCheckoutItemsQueryState.data &&
                                       getCheckoutItemsQueryState.data
                                          .cart_items.length > 0
                                          ? getCheckoutItemsQueryState.data.cart_items
                                               .reduce(
                                                  (
                                                     acc: number,
                                                     item: TCheckoutBasketItem,
                                                  ) =>
                                                     acc +
                                                     (item.discount_amount ??
                                                        0) *
                                                        item.quantity,
                                                  0,
                                               )
                                               .toFixed(2)
                                          : '0.00'}
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
                           {getCheckoutItemsQueryState.data &&
                           getCheckoutItemsQueryState.data.cart_items.length > 0
                              ? getCheckoutItemsQueryState.data.total_amount.toFixed(
                                   2,
                                )
                              : '0.00'}
                        </div>
                     </div>
                     {form.getValues('payment_method') !==
                        EPaymentType.SOLANA.toString() && (
                        <Button
                           className="w-full h-fit bg-[#0070f0] text-white rounded-full text-[14px] font-medium tracking-[0.2px] mt-5"
                           onClick={() => {
                              form.handleSubmit(handleSubmit)();
                           }}
                        >
                           Place Order
                        </Button>
                     )}
                     {form.getValues('payment_method') ===
                        EPaymentType.SOLANA.toString() && (
                        <Button
                           className="w-full h-fit bg-[#0070f0] text-white rounded-full text-[14px] font-medium tracking-[0.2px] mt-5"
                           onClick={() => {
                              form.trigger();

                              if (!isConnected) {
                                 toast.error(
                                    'Please connect your wallet first',
                                    {
                                       style: {
                                          backgroundColor: '#FEE2E2',
                                          color: '#991B1B',
                                          border: '1px solid #FCA5A5',
                                       },
                                       duration: 1000,
                                    },
                                 );
                              }
                              if (form.formState.isValid && isConnected) {
                                 setDisplayPaymentBlockchainModel(true);
                              }
                           }}
                        >
                           Pay with Solana
                        </Button>
                     )}
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
