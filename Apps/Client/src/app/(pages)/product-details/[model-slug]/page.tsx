/* eslint-disable react/react-in-jsx-scope */
'use client';
import '/globals.css';
import Image from 'next/image';
import { useEffect, useState } from 'react';
import { cn } from '~/infrastructure/lib/utils';

import {
   Carousel,
   CarouselApi,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '~/components/ui/carousel';

import images from '~/components/client/images';
import ModelItem from '~/app/(pages)/product-details/_components/ModelItem';
import HelpItem from '~/app/(pages)/product-details/_components/HelpItem';
import CompareItem, {
   CompareItemType,
} from '~/app/(pages)/product-details/_components/CompareItem';
import ColorItem from '~/app/(pages)/product-details/_components/ColorItem';
import StorageItem from '~/app/(pages)/product-details/_components/StorageItem';
import { FaRegStar, FaStar } from 'react-icons/fa6';
import ItemReview from '~/app/(pages)/product-details/_components/ReviewItem';
import { useForm } from 'react-hook-form';
import {
   StoreBasketFormType,
   StoreBasketResolver,
} from '~/domain/schemas/basket.schema';
import { Form, FormControl, FormField, FormItem } from '@components/ui/form';
import { RadioGroup, RadioGroupItem } from '@components/ui/radio-group';
// import { productModelsData } from '../data';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { motion } from 'framer-motion';
import { Button } from '@components/ui/button';
import { IIphoneModelResponse } from '~/domain/interfaces/catalogs/iPhone-model.inteface';
import { useStoreBasketAsyncMutation } from '~/infrastructure/services/basket.service';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useDispatch } from 'react-redux';
import {
   IBasketItem,
   IStoreBasketPayload,
} from '~/domain/interfaces/baskets/basket.interface';
import { addCartItem } from '~/infrastructure/redux/features/cart.slice';
import { Badge } from '@components/ui/badge';
import {
   selectorSchema,
   SelectorFormType,
   SelectorResolver,
} from '~/app/(pages)/product-details/_schemas/selector-schema';
import { IIphoneResponse } from '~/domain/interfaces/catalogs/iPhone.interface';
import {
   useGetIPhonesByModelAsyncQuery,
   useGetModelBySlugAsyncQuery,
} from '~/infrastructure/services/catalog.service';
import { useParams } from 'next/navigation';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { Skeleton } from '@components/ui/skeleton';

const DetailProductPage = () => {
   const params = useParams<{ 'model-slug': string }>();

   const [isLoading, setIsLoading] = useState(true);
   const [selectedColor, setSelectedColor] = useState<string | null>(null);
   const [selectedModel, setSelectedModel] = useState<string | null>(null);
   const [selectedStorage, setSelectedStorage] = useState<string | null>(null);
   const [productDetails, setProductDetails] = useState<{
      product_id: string;
      product_unit_price: number;
      promotion: {
         promotion_final_price: number;
      } | null;
   } | null>(null);
   const [visibleColor, setVisibleColor] = useState({
      visible: false,
      order: 2,
   });
   const [visibleStorage, setVisibleStorage] = useState({
      visible: false,
      order: 3,
   });

   const [model, setModel] = useState<IIphoneModelResponse | null>(null);
   const [iPhones, setIPhones] = useState<IIphoneResponse[] | null>(null);

   const dispatch = useDispatch();
   const { items, currentOrder } = useAppSelector((state) => state.cart.value);

   const [
      storeBasket,
      {
         isLoading: isLoadingStoreBasket,
         isError: isErrorStoreBasket,
         error: errorStoreBasket,
      },
   ] = useStoreBasketAsyncMutation();

   const {
      data: modelDataAsync,
      error: errorModelData,
      isLoading: isLoadingModelData,
      isFetching: isFetchingModelData,
      isSuccess: isSuccessModelData,
      refetch: refetchModelData,
   } = useGetModelBySlugAsyncQuery(params['model-slug']);

   const {
      data: iPhonesWithModelAsync,
      error: errorIPhonesWithModelAsync,
      isLoading: isLoadingIPhonesWithModelAsync,
      isFetching: isFetchingIPhonesWithModelAsync,
      isSuccess: isSuccessIPhonesWithModelAsync,
      refetch,
   } = useGetIPhonesByModelAsyncQuery(params['model-slug']);

   const form = useForm<SelectorFormType>({
      resolver: SelectorResolver,
      defaultValues: {
         model: '',
         color: '',
         storage: '',
      },
   });

   const handleVisible = (order: number) => {
      if (order === 1) {
         setVisibleColor({ ...visibleColor, visible: true });
      }

      if (order === 2) {
         setVisibleStorage({ ...visibleStorage, visible: true });
      }
   };

   const handleSubmit = async (data: SelectorFormType) => {
      if (iPhones) {
         const validProduct = iPhones.find((item) => {
            return (
               item.product_model.toLowerCase().replace(' ', '-') ===
                  data.model.toLowerCase().replace(' ', '-') &&
               item.product_color.color_name === data.color &&
               item.product_storage.storage_name === data.storage
            );
         });

         if (validProduct) {
            const onPromotion = iPhones.find((item) => {
               if (
                  item.promotion &&
                  item.promotion.promotion_product_id ===
                     validProduct.product_id
               ) {
                  return true;
               }
            });

            const promotion = onPromotion ? onPromotion.promotion : null;

            console.log('promotion', promotion);

            const basketItem = {
               product_id: validProduct.product_id,
               product_name: validProduct.product_slug.replace('-', ' '),
               product_color_name: selectedColor,
               product_unit_price: validProduct.product_unit_price,
               product_name_tag: validProduct.product_name_tag,
               product_image: validProduct.product_images[0].image_url,
               product_slug: validProduct.product_slug,
               category_id: validProduct.category_id,
               quantity: 1,
               promotion: {
                  promotion_id_or_code: promotion?.promotion_id,
                  promotion_event_type: promotion?.promotion_event_type,
               },
               order: 0,
            } as IBasketItem;

            dispatch(addCartItem(basketItem));

            await storeBasket({
               cart_items: [
                  ...items,
                  {
                     ...basketItem,
                     order: currentOrder,
                  },
               ],
            });
         }
      }
   };

   useEffect(() => {
      if (
         selectedModel &&
         selectedColor &&
         selectedStorage &&
         iPhones &&
         model
      ) {
         const validProduct = iPhones.find((item) => {
            return (
               item.product_model.toLowerCase().replace(' ', '-') ===
                  selectedModel.toLowerCase().replace(' ', '-') &&
               item.product_color.color_name === selectedColor &&
               item.product_storage.storage_name === selectedStorage
            );
         });

         if (validProduct) {
            const onPromotion = iPhones.find((item) => {
               if (
                  item.promotion &&
                  item.promotion.promotion_product_id ===
                     validProduct.product_id
               ) {
                  return true;
               }
            });

            const promotion = onPromotion ? onPromotion.promotion : null;

            if (promotion) {
               setProductDetails({
                  ...validProduct,
                  promotion: {
                     ...promotion,
                  },
               });
            }
         }
      }
   }, [selectedModel, selectedColor, selectedStorage]);

   useEffect(() => {
      if (modelDataAsync) {
         setModel(modelDataAsync);
      }
   }, [modelDataAsync]);

   useEffect(() => {
      if (iPhonesWithModelAsync) {
         setIPhones(iPhonesWithModelAsync);
      }
   }, [iPhonesWithModelAsync]);

   useEffect(() => {
      if (isLoadingModelData) {
         setIsLoading(true);
      } else {
         setTimeout(() => {
            setIsLoading(!isLoading);
         }, 500);
      }

      if (isLoadingIPhonesWithModelAsync) {
         setIsLoading(true);
      } else {
         setTimeout(() => {
            setIsLoading(!isLoading);
         }, 500);
      }

      if (isLoadingStoreBasket) {
         setIsLoading(true);
      } else {
         setTimeout(() => {
            setIsLoading(!isLoading);
         }, 500);
      }
   }, [
      isLoadingStoreBasket,
      isLoadingModelData,
      isLoadingIPhonesWithModelAsync,
   ]);

   const renderCheckoutHeader = () => {
      return (
         <div className="fixed bottom-0 left-0 w-full h-fit z-[99] backdrop-blur-sm transition-all opacity-100 flex justify-end">
            <div className="flex flex-row gap-4 items-center justify-between px-10 py-2">
               <span className="text-[15px] font-semibold leading-[20px] flex flex-col gap-1 items-end">
                  {productDetails?.promotion ? (
                     <div>
                        <span className="flex flex-row gap-2">
                           <Badge>Black Friday</Badge>
                           <p className=" text-xl font-semibold text-destructive">
                              $
                              {productDetails.promotion.promotion_final_price.toFixed(
                                 2,
                              )}
                           </p>
                        </span>
                        <p className=" line-through text-sm text-right">
                           ${productDetails.product_unit_price.toFixed(2)}
                        </p>
                     </div>
                  ) : (
                     <p className="text-base font-semibold">
                        ${productDetails?.product_unit_price.toFixed(2)}
                     </p>
                  )}
               </span>

               <Button
                  variant={
                     selectedModel && selectedStorage && selectedColor
                        ? 'default'
                        : 'destructive'
                  }
                  disabled={
                     !selectedModel || !selectedStorage || !selectedColor
                  }
                  className="rounded-lg"
                  onClick={() => {
                     form.handleSubmit(handleSubmit)();
                  }}
               >
                  Add to Basket
               </Button>
            </div>
         </div>
      );
   };

   return (
      <div
         className={cn(
            'w-full flex flex-col items-start justify-center bg-[#fff]',
         )}
      >
         <LoadingOverlay isLoading={isLoading} fullScreen />

         {renderCheckoutHeader()}
         {/* CARRIER */}
         <div className="w-full pt-[14px] relative bg-transparent">
            <div className="w-[980px] py-[14px] h-[66px] rounded-[5px] flex flex-row justify-center items-center text-[12px] font-normal bg-[#f5f5f7] mx-auto">
               <div className="h-full pl-[20px] w-[176px] flex flex-col">
                  <div className="leading-[18px] tracking-[0.4px] text-[15px] font-medium">
                     Carrier Deals at Apple
                  </div>
                  <div className="leading-[20px] tracking-[0.4px] text-[15px] font-light text-blue-500">
                     <a href="#" target="_blank">
                        See all deals
                     </a>
                  </div>
               </div>
               <div className="h-full pl-[20px] w-[190px] flex flex-row items-center">
                  <div className="basis-1/6 w-[24px]">
                     <Image
                        src={images.icAtt}
                        className="h-[24px] w-auto mx-auto"
                        alt="apple-logo"
                        width={1000}
                        height={1000}
                        quality={100}
                     />
                  </div>
                  <div className="basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light">
                     Save up to $1000 after trade-in.
                  </div>
               </div>
               <div className="h-full pl-[20px] w-[190px] flex flex-row items-center">
                  <div className="basis-1/6 w-[24px]">
                     <Image
                        src={images.icLightYear}
                        className="h-[24px] w-auto mx-auto"
                        alt="apple-logo"
                        width={1000}
                        height={1000}
                        quality={100}
                     />
                  </div>
                  <div className="basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light">
                     Save up to $1000. No trade-in needed.
                  </div>
               </div>
               <div className="h-full pl-[20px] w-[190px] flex flex-row items-center">
                  <div className="basis-1/6 w-[24px]">
                     <Image
                        src={images.icTmobile}
                        className="h-[24px] w-auto mx-auto"
                        alt="apple-logo"
                        width={1000}
                        height={1000}
                        quality={100}
                     />
                  </div>
                  <div className="basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light">
                     Save up to $1000 after trade-in.
                  </div>
               </div>
               <div className="h-full pl-[20px] w-[190px] flex flex-row items-center">
                  <div className="basis-1/6 w-[24px]">
                     <Image
                        src={images.icVerizon}
                        className="h-[24px] w-auto mx-auto"
                        alt="apple-logo"
                        width={1000}
                        height={1000}
                        quality={100}
                     />
                  </div>
                  <div className="basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light">
                     Save up to $1000 after trade-in.
                  </div>
               </div>
            </div>
         </div>

         {/* DETAIL PRODUCT */}
         <div className="min-w-[980px] max-w-[2632px] w-[87.5%] mx-auto relative bg-transparent">
            {/* Product Title */}
            <div className="w-full bg-transparent flex flex-row pt-[52px] pb-[22px]">
               <div className="basis-[70%] bg-transparent">
                  <span className="text-[18px] text-[#b64400] font-semibold leading-[20px]">
                     New
                  </span>
                  <h1 className="text-[48px] font-semibold leading-[52px] pb-2 mb-[13px]">
                     Buy iPhone 16 Pro{' '}
                  </h1>
                  <div className="text-[15px] font-light leading-[20px]">
                     From $999 or $41.62/mo. for 24 mo.
                  </div>
               </div>
               <div className="basis-[30%] bg-transparent">
                  <div className="h-full flex flex-col mt-1">
                     <div className="w-full basis-1/2 flex flex-row justify-end">
                        <div className="w-fit text-[13px] font-light leading-[16px] tracking-[0.7px] px-[16px] py-[12px] my-[6px] flex items-center bg-[#f5f5f7] rounded-full">
                           Get $40–$650 for your trade-in.
                        </div>
                     </div>
                     <div className="w-full basis-1/2 flex flex-row justify-end">
                        <div className="w-fit text-[13px] font-light leading-[16px] tracking-[0.7px] px-[16px] py-[12px] my-[6px] flex items-center bg-[#f5f5f7] rounded-full">
                           Get 3% Daily Cash back with Apple Card.
                        </div>
                     </div>
                  </div>
               </div>
            </div>

            {/* Product Detail */}
            <div className="w-full bg-transparent flex flex-row relative pb-[30px]">
               <div className="basis-4/5 h-fit pr-[60px] sticky top-[100px]">
                  <div className="w-full flex items-center justify-center pt-10">
                     <Carousel className="w-full z-10">
                        <CarouselContent className="min-h-[460px] h-[calc(100vh-189px)]">
                           {model
                              ? model.model_images.map((img, index) => {
                                   return (
                                      <CarouselItem key={index}>
                                         <Image
                                            src={img.image_url || ''}
                                            className="h-full w-full rounded-[20px]"
                                            width={2000}
                                            height={2000}
                                            quality={100}
                                            style={{
                                               objectFit: 'cover',
                                            }}
                                            alt="iphone-16-pro-model-unselect-gallery-1-202409"
                                         />
                                      </CarouselItem>
                                   );
                                })
                              : null}
                        </CarouselContent>
                        <CarouselPrevious className="left-[1rem]" />
                        <CarouselNext className="right-[1rem]" />

                        {/* <div className="absolute bottom-2 left-0 w-full z-50 flex flex-row items-center justify-center gap-2">
                           {/* Slide {current} of {count} */}
                        {/* {Array.from({ length: count }).map((_, index) => (
                              <div
                                 className="w-[10px] h-[10px] rounded-full"
                                 style={{
                                    backgroundColor:
                                       current === index + 1
                                          ? '#6b7280'
                                          : '#d1d5db',
                                 }}
                                 key={index}
                              />
                           ))}
                        </div> */}
                     </Carousel>
                  </div>
               </div>
               <Form {...form}>
                  <form onSubmit={form.handleSubmit(handleSubmit)}>
                     <div className="basis-1/4 h-fit min-w-[328px]">
                        {/* models */}
                        <div className="type-model w-full mt-[100px]">
                           <div className="text-[24px] font-semibold leading-[28px] pb-[13px]">
                              <span className="text-[#1D1D1F]">Model. </span>
                              <span className="text-[#86868B]">
                                 Which is best for you?
                              </span>
                           </div>
                           <FormField
                              control={form.control}
                              name="model"
                              render={({ field }) => {
                                 return (
                                    <FormItem>
                                       <FormControl>
                                          <RadioGroup
                                             onValueChange={(value) => {
                                                field.onChange(value);
                                                setSelectedModel(value);
                                                handleVisible(1);
                                             }}
                                          >
                                             {isLoading &&
                                                Array(2)
                                                   .fill(0)
                                                   .map((_, index) => (
                                                      <Skeleton
                                                         key={index}
                                                         className="h-[60px] w-full rounded-lg"
                                                      />
                                                   ))}

                                             {!isLoading && model
                                                ? model.model_items.map(
                                                     (model, index) => (
                                                        <motion.div key={index}>
                                                           <RadioGroupItem
                                                              key={index}
                                                              value={
                                                                 model.model_name
                                                              }
                                                              id={`model-${model.model_name}`}
                                                              className="sr-only"
                                                           />
                                                           <motion.label
                                                              htmlFor={`model-${model.model_name}`}
                                                              className={cn(
                                                                 'block cursor-pointer rounded-lg border-2',
                                                                 field.value ===
                                                                    model.model_name
                                                                    ? 'border-blue-500'
                                                                    : 'border-gray-200',
                                                              )}
                                                              transition={{
                                                                 type: 'spring',
                                                                 stiffness: 400,
                                                                 damping: 17,
                                                              }}
                                                           >
                                                              <ModelItem
                                                                 modelName={
                                                                    model.model_name
                                                                 }
                                                              />
                                                           </motion.label>
                                                        </motion.div>
                                                     ),
                                                  )
                                                : null}
                                          </RadioGroup>
                                       </FormControl>
                                    </FormItem>
                                 );
                              }}
                           />
                           <div className="mt-[20px] mb-[6px]">
                              <HelpItem
                                 title="Need help choosing a model?"
                                 subTitle="Explore the differences in screen size and battery life."
                              />
                           </div>
                        </div>

                        {/* color */}
                        <div className="type-color w-full mt-[100px]">
                           <div className="text-[24px] font-semibold leading-[28px] pb-[13px]">
                              <span className="text-[#1D1D1F]">Finish. </span>
                              <span className="text-[#868a8b]">
                                 Which is best for you?
                              </span>
                           </div>
                           <div className="pt-5 text-[17px] font-semibold leading-[25px] tracking-[0.5px]">
                              Color
                           </div>
                           <div
                              className={cn(
                                 'flex flex-row gap-3 pt-[17px] pb-[13px]',
                              )}
                           >
                              <FormField
                                 control={form.control}
                                 name="color"
                                 render={({ field }) => {
                                    return (
                                       <FormItem>
                                          <FormControl>
                                             <RadioGroup
                                                disabled={!visibleColor.visible}
                                                onValueChange={(value) => {
                                                   field.onChange(value);
                                                   setSelectedColor(value);
                                                   handleVisible(2);
                                                }}
                                             >
                                                <div className="flex flex-row gap-3">
                                                   {model
                                                      ? model.color_items.map(
                                                           (color, index) => {
                                                              return (
                                                                 <div
                                                                    key={index}
                                                                 >
                                                                    <RadioGroupItem
                                                                       key={
                                                                          index
                                                                       }
                                                                       value={
                                                                          color.color_name
                                                                       }
                                                                       id={`color-${color.color_name}`}
                                                                       className="sr-only"
                                                                    />

                                                                    <motion.label
                                                                       htmlFor={`color-${color.color_name}`}
                                                                       className={cn(
                                                                          {
                                                                             'opacity-50':
                                                                                !visibleColor.visible,
                                                                          },
                                                                       )}
                                                                    >
                                                                       <div
                                                                          key={
                                                                             index
                                                                          }
                                                                          className={cn(
                                                                             'h-[30px] w-[30px] rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                                                                             selectedColor ===
                                                                                color.color_name
                                                                                ? 'ring-2 ring-[#2563EB] ring-offset-2 ring-offset-white'
                                                                                : 'hover:ring-2 hover:ring-[#2563EB] hover:ring-offset-2 hover:ring-offset-white',
                                                                             {
                                                                                'cursor-not-allowed':
                                                                                   !visibleColor.visible,
                                                                             },
                                                                             {
                                                                                'cursor-pointer':
                                                                                   visibleColor.visible,
                                                                             },
                                                                          )}
                                                                          style={{
                                                                             backgroundColor:
                                                                                color.color_hex,
                                                                          }}
                                                                       />
                                                                    </motion.label>
                                                                 </div>
                                                              );
                                                           },
                                                        )
                                                      : null}
                                                </div>
                                             </RadioGroup>
                                          </FormControl>
                                       </FormItem>
                                    );
                                 }}
                              />
                           </div>
                        </div>

                        {/* storages */}
                        <div className="type-storage w-full mt-[200px]">
                           <div className="text-[24px] font-semibold leading-[28px] pb-[13px]">
                              <span className="text-[#1D1D1F]">Storage. </span>
                              <span className="text-[#86868B]">
                                 How much space do you need?
                              </span>
                           </div>
                           <div
                              className="flex flex-col gap-[14px] my-[14px]"
                              style={{
                                 opacity: true ? 1 : 0.5,
                              }}
                           >
                              <FormField
                                 control={form.control}
                                 name="storage"
                                 render={({ field }) => {
                                    return (
                                       <FormItem>
                                          <FormControl>
                                             <RadioGroup
                                                disabled={
                                                   !visibleStorage.visible
                                                }
                                                onValueChange={(value) => {
                                                   field.onChange(value);
                                                   setSelectedStorage(value);
                                                }}
                                             >
                                                {isLoading &&
                                                   Array(4)
                                                      .fill(0)
                                                      .map((_, index) => (
                                                         <Skeleton
                                                            key={index}
                                                            className="h-[60px] w-full rounded-lg"
                                                         />
                                                      ))}

                                                {!isLoading &&
                                                   (model
                                                      ? model.storage_items.map(
                                                           (storage, index) => {
                                                              return (
                                                                 <div>
                                                                    <RadioGroupItem
                                                                       key={
                                                                          index
                                                                       }
                                                                       value={
                                                                          storage.storage_name
                                                                       }
                                                                       id={`storage-${storage.storage_name}`}
                                                                       className="sr-only"
                                                                    />
                                                                    <motion.label
                                                                       htmlFor={`storage-${storage.storage_name}`}
                                                                       className={cn(
                                                                          'block cursor-pointer rounded-lg border-2 border-blue-500',
                                                                          field.value ===
                                                                             storage.storage_name
                                                                             ? 'border-blue-500'
                                                                             : 'border-gray-200',
                                                                          {
                                                                             'border-gray-200':
                                                                                !visibleStorage.visible,
                                                                          },
                                                                          {
                                                                             'opacity-50 cursor-not-allowed':
                                                                                !visibleStorage.visible,
                                                                          },
                                                                       )}
                                                                    >
                                                                       <StorageItem
                                                                          storageName={
                                                                             storage.storage_name
                                                                          }
                                                                       />
                                                                    </motion.label>
                                                                 </div>
                                                              );
                                                           },
                                                        )
                                                      : null)}
                                             </RadioGroup>
                                          </FormControl>
                                       </FormItem>
                                    );
                                 }}
                              />

                              <HelpItem
                                 image={images.helpStorage}
                                 title="Not sure how much storage to get?"
                                 subTitle="Get a better understanding of how much space you’ll need."
                              />
                           </div>
                        </div>
                     </div>
                  </form>
               </Form>
            </div>
         </div>
      </div>
   );
};

export default DetailProductPage;
