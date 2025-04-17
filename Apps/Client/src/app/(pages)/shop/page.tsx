/* eslint-disable react/react-in-jsx-scope */
'use client';
import '/globals.css';
import { useEffect, useState } from 'react';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { cn } from '~/infrastructure/lib/utils';
import { FaFilter } from 'react-icons/fa6';
import { Button } from '~/components/ui/button';
import { IoChatboxEllipsesOutline } from 'react-icons/io5';
import { GoMultiSelect } from 'react-icons/go';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '~/components/ui/select';
import {
   Accordion,
   AccordionContent,
   AccordionItem,
   AccordionTrigger,
} from '~/components/ui/accordion';
import { DualRangeSlider } from '~/components/ui/dualRangeSlider';
import IphoneModelItem from './_components/iphone-model-item';
import { useGetModelsAsyncQuery } from '~/infrastructure/services/catalog.service';
import { IIphoneModelResponse } from '~/domain/interfaces/catalogs/iPhone-model.inteface';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { Skeleton } from '@components/ui/skeleton';
import { Separator } from '@components/ui/separator';

const modelData = [
   {
      model_id: '67346f7549189f7314e4ef0c',
      model_name: 'iPhone 16',
      model_items: [
         {
            model_name: 'iPhone 16',
            model_order: 0,
         },
         {
            model_name: 'iPhone 16 Plus',
            model_order: 1,
         },
      ],
      color_items: [
         {
            color_name: 'ultramarine',
            color_hex: '#9AADF6',
            color_image:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-ultramarine-202409',
            color_order: 0,
         },
         {
            color_name: 'teal',
            color_hex: '#B0D4D2',
            color_image:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-teal-202409',
            color_order: 1,
         },
         {
            color_name: 'pink',
            color_hex: '#F2ADDA',
            color_image:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-pink-202409',
            color_order: 2,
         },
         {
            color_name: 'white',
            color_hex: '#FAFAFA',
            color_image:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-white-202409',
            color_order: 3,
         },
         {
            color_name: 'black',
            color_hex: '#3C4042',
            color_image:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202409',
            color_order: 4,
         },
      ],
      storage_items: [
         {
            storage_name: '128GB',
            storage_value: 128,
         },
         {
            storage_name: '256GB',
            storage_value: 256,
         },
         {
            storage_name: '512GB',
            storage_value: 512,
         },
         {
            storage_name: '1TB',
            storage_value: 1024,
         },
      ],
      general_model: 'iphone-16',
      model_description: '',
      minimun_unit_price: 699,
      maximun_unit_price: 1299,
      overall_sold: 0,
      average_rating: {
         rating_average_value: 0,
         rating_count: 0,
      },
      rating_stars: [
         {
            star: 1,
            count: 0,
         },
         {
            star: 2,
            count: 0,
         },
         {
            star: 3,
            count: 0,
         },
         {
            star: 4,
            count: 0,
         },
         {
            star: 5,
            count: 0,
         },
      ],
      model_images: [
         {
            image_id: 'image_id_1',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 0,
         },
         {
            image_id: 'image_id_2',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-2-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 1,
         },
         {
            image_id: 'image_id_3',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-3-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 2,
         },
         {
            image_id: 'image_id_4',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-4-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 3,
         },
         {
            image_id: 'image_id_5',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-5-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 4,
         },
         {
            image_id: 'image_id_6',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-6-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 5,
         },
      ],
      model_promotion: {
         minimum_promotion_price: 594.15,
         maximum_promotion_price: 1234.05,
         minimum_discount_percentage: 0.05,
         maximum_discount_percentage: 0.15,
      },
      model_slug: 'iphone-16',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      is_deleted: false,
      deleted_by: null,
      created_at: '2025-04-16T09:37:33.1957166Z',
      updated_at: '2025-04-16T09:37:33.1957364Z',
      deleted_at: null,
   },
   {
      model_id: '664439317954a1ae3c52364c',
      model_name: 'iPhone 16e',
      model_items: [
         {
            model_name: 'iPhone 16e',
            model_order: 0,
         },
      ],
      color_items: [
         {
            color_name: 'white',
            color_hex: '#FAFAFA',
            color_image:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-white-202409',
            color_order: 3,
         },
         {
            color_name: 'black',
            color_hex: '#3C4042',
            color_image:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202409',
            color_order: 4,
         },
      ],
      storage_items: [
         {
            storage_name: '128GB',
            storage_value: 128,
         },
         {
            storage_name: '256GB',
            storage_value: 256,
         },
         {
            storage_name: '512GB',
            storage_value: 512,
         },
         {
            storage_name: '1TB',
            storage_value: 1024,
         },
      ],
      general_model: 'iphone-16e',
      model_description: '',
      minimun_unit_price: 799,
      maximun_unit_price: 1299,
      overall_sold: 0,
      average_rating: {
         rating_average_value: 0,
         rating_count: 0,
      },
      rating_stars: [
         {
            star: 1,
            count: 0,
         },
         {
            star: 2,
            count: 0,
         },
         {
            star: 3,
            count: 0,
         },
         {
            star: 4,
            count: 0,
         },
         {
            star: 5,
            count: 0,
         },
      ],
      model_images: [
         {
            image_id: 'image_id_1',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 0,
         },
         {
            image_id: 'image_id_2',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-2-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 1,
         },
         {
            image_id: 'image_id_3',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-3-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 2,
         },
         {
            image_id: 'image_id_4',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-4-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 3,
         },
         {
            image_id: 'image_id_5',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-5-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 4,
         },
         {
            image_id: 'image_id_6',
            image_url:
               'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-6-202409',
            image_name: '6ip',
            image_description: '6 ip in an images',
            image_width: 0,
            image_height: 0,
            image_bytes: 0,
            image_order: 5,
         },
      ],
      model_promotion: {
         minimum_promotion_price: 759.05,
         maximum_promotion_price: 1234.05,
         minimum_discount_percentage: 0.05,
         maximum_discount_percentage: 0.15,
      },
      model_slug: 'iphone-16e',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      is_deleted: false,
      deleted_by: null,
      created_at: '2025-04-16T09:37:33.1958447Z',
      updated_at: '2025-04-16T09:37:33.1958447Z',
      deleted_at: null,
   },
];

const ShopPage = () => {
   const [isLoading, setIsLoading] = useState(true);
   const [models, setModels] = useState<IIphoneModelResponse[]>([]);
   const [priceFilter, setPriceFilter] = useState([0, 1000]);
   const storageFilter = ['64 GB', '128 GB', '256 GB', '512 GB', '1 TB'];

   const {
      data: modelsDataAsync,
      error,
      isLoading: modelsDataIsLoading,
      isFetching,
      isSuccess,
      refetch,
   } = useGetModelsAsyncQuery();

   useEffect(() => {
      if (modelsDataAsync) {
         setModels(modelsDataAsync.items);
      }
   }, [modelsDataAsync]);

   useEffect(() => {
      if (modelsDataIsLoading) {
         setIsLoading(true);
      } else {
         setTimeout(() => {
            setIsLoading(!isLoading);
         }, 500);
      }
   }, [modelsDataIsLoading]);

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col items-start justify-center bg-white',
         )}
      >
         <LoadingOverlay isLoading={isLoading} fullScreen />
         {/* FILTERS INFO */}
         <div className="w-full border-y border-[#ccc] text-[15px] font-semibold">
            <div className="h-[4.514vw] w-full max-w-[1440px] mx-auto px-5 flex flex-row justify-start items-center">
               <div className="flex flex-row mr-auto">
                  <div className="text-[#218bff] flex flex-row items-center gap-2">
                     <FaFilter />
                     <div>Filters</div>
                  </div>
                  <div className="mx-3 px-[18px] border-x-[1px] border-[#ccc] flex flex-row items-center">
                     <div>10 Results</div>
                  </div>
                  <div className="flex flex-row items-center ">
                     <Button className="h-[22.5px] p-0 text-[15px] font-semibold border-b-2 border-[#000] rounded-none bg-transparent text-black hover:bg-transparent">
                        Clear Filters
                     </Button>
                  </div>
               </div>
               <div className="flex flex-row gap-[50px]">
                  <div className="flex flex-row items-center gap-1">
                     <IoChatboxEllipsesOutline />
                     <div>Chat with an expert</div>
                  </div>
                  <Select>
                     <SelectTrigger className="w-fit flex items-center justify-center border-none">
                        <GoMultiSelect className="mr-2" />
                        <SelectValue placeholder="Recommended" />
                     </SelectTrigger>
                     <SelectContent className="bg-[#f7f7f7]">
                        <SelectGroup>
                           <SelectItem value="newest">Newest</SelectItem>
                           <SelectItem value="most-clicked">
                              Most Clicked
                           </SelectItem>
                           <SelectItem value="highest-rated">
                              Highest Rated
                           </SelectItem>
                           <SelectItem value="recommended">
                              Recommended
                           </SelectItem>
                           <SelectItem value="price-high-low">
                              Price: High to Low
                           </SelectItem>
                           <SelectItem value="price-low-high">
                              Price: Low to High
                           </SelectItem>
                           <SelectItem value="online-availability">
                              Online Availability
                           </SelectItem>
                           <SelectItem value="most-reviewed">
                              Most Reviewed
                           </SelectItem>
                        </SelectGroup>
                     </SelectContent>
                  </Select>
               </div>
            </div>
         </div>

         {/* SHOP INFO */}
         <div className="w-full h-fit flex flex-row justify-center items-start">
            {/* SIDEBAR */}
            <div className="w-[354px] h-[4336px] relative">
               <div className="w-full h-fit sticky top-0">
                  <Accordion type="multiple" className="w-full h-full p-6">
                     {/* STORAGE FILTER */}
                     <AccordionItem value="item-1">
                        <AccordionTrigger className="hover:no-underline text-[15px] font-semibold pb-0 uppercase ">
                           Storage
                        </AccordionTrigger>
                        <AccordionContent className="pt-4">
                           <div className="grid grid-cols-3 gap-2">
                              {storageFilter.map((item, index) => {
                                 return (
                                    <Button
                                       key={index}
                                       className="h-fit py-1 rounded-full text-[12px] font-normal 
                                    border border-[#000] opacity-50 hover:opacity-100 bg-white text-black hover:bg-black hover:text-white"
                                    >
                                       {item}
                                    </Button>
                                 );
                              })}
                           </div>
                        </AccordionContent>
                     </AccordionItem>

                     {/* PRICE FILTER */}
                     <AccordionItem value="item-2">
                        <AccordionTrigger className="hover:no-underline text-[15px] font-semibold pb-0 uppercase ">
                           Price
                        </AccordionTrigger>
                        <AccordionContent className="pt-4">
                           <div className="w-full flex flex-col justify-center">
                              <div className="w-full flex flex-row justify-between">
                                 <div>From: {priceFilter[0]}$</div>
                                 <div>To: {priceFilter[1]}$</div>
                              </div>
                              <DualRangeSlider
                                 className="mt-3 bg-gray-200 rounded-lg"
                                 value={priceFilter}
                                 onValueChange={setPriceFilter}
                                 min={0}
                                 max={1000}
                                 step={100}
                              />
                           </div>
                        </AccordionContent>
                     </AccordionItem>

                     {/* CAMERA FILTER */}
                     <AccordionItem value="item-3">
                        <AccordionTrigger className="hover:no-underline text-[15px] font-semibold pb-0 uppercase ">
                           Camera
                        </AccordionTrigger>
                        <AccordionContent className="pt-4">
                           <div className="grid grid-cols-3 gap-2">
                              {Array.from({ length: 4 }).map((_, index) => {
                                 return (
                                    <Button
                                       key={index}
                                       className="h-fit py-1 rounded-full text-[12px] font-normal 
                                    border border-[#000] opacity-50 hover:opacity-100 bg-white text-black hover:bg-black hover:text-white"
                                    >
                                       12 MP
                                    </Button>
                                 );
                              })}
                           </div>
                        </AccordionContent>
                     </AccordionItem>
                  </Accordion>
               </div>
            </div>

            {/* PRODUCTS */}
            <div className="w-[1086px] h-fit p-6 flex flex-col gap-6 bg-[#f7f7f7]">
               {isLoading
                  ? Array(5)
                       .fill(0)
                       .map((_, index) => (
                          <div className="flex flex-row bg-white px-5 py-5 rounded-md">
                             {/* image */}
                             <div className="image flex basis-[23%] items-center justify-center h-[300px] rounded-lg overflow-hidden">
                                <Skeleton className="h-full w-full rounded-lg" />
                             </div>

                             {/* content */}
                             <div className="flex flex-col relative basis-[40%] lg:basis-[30%] px-7">
                                <div className="content flex flex-col gap-2">
                                   <h2 className="font-SFProText font-normal text-xl">
                                      <Skeleton className="h-5 w-[350px]" />
                                   </h2>

                                   <span className="flex flex-row gap-2">
                                      <p className="first-letter:uppercase text-sm">
                                         colors:
                                      </p>
                                      <p className="first-letter:uppercase text-sm">
                                         {/* ultramarine */}
                                      </p>
                                   </span>

                                   {/* Colors */}
                                   <div className="flex flex-row gap-2">
                                      <Skeleton className="h-5 w-[350px]" />
                                   </div>

                                   <span className="flex flex-row gap-2 mt-2">
                                      <p className="first-letter:uppercase text-sm">
                                         Storage:
                                      </p>
                                   </span>

                                   {/* Storage */}
                                   <div className="flex flex-row gap-2">
                                      <Skeleton className="h-5 w-[350px]" />
                                   </div>

                                   <Separator className="mt-2" />

                                   <span className="flex flex-row gap-2 mt-2 items-center">
                                      <Skeleton className="h-5 w-[350px]" />
                                   </span>

                                   <span className="gap-2 mt-2 text-right">
                                      <Skeleton className="h-5 w-[350px]" />
                                   </span>
                                </div>
                             </div>

                             {/* feature */}
                             <div className="flex flex-col justify-between flex-1 border-l-2 border-[#E5E7EB] px-4">
                                <div className="flex flex-col gap-2">
                                   <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                                      <Skeleton className="h-10 w-full" />
                                   </div>

                                   <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                                      <Skeleton className="h-10 w-full" />
                                   </div>

                                   <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                                      <Skeleton className="h-10 w-full" />
                                   </div>

                                   <div className="flex flex-row gap-4 items-center">
                                      <Skeleton className="h-10 w-full" />
                                   </div>
                                </div>

                                <Skeleton className="h-12 w-full" />
                             </div>
                          </div>
                       ))
                  : models.map((item, index) => {
                       return (
                          <IphoneModelItem
                             modelItem={item}
                             title={item.model_items.map(
                                (item) => item.model_name,
                             )}
                             colorsHex={item.color_items.map((item) => {
                                return {
                                   name: item.color_name,
                                   hex: item.color_hex,
                                };
                             })}
                             storage={item.storage_items.map((item) => {
                                return item.storage_name;
                             })}
                             averageRating={{
                                rating:
                                   item.average_rating.rating_average_value,
                                count: item.average_rating.rating_count,
                             }}
                             unitPriceRange={[
                                item.minimun_unit_price,
                                item.maximun_unit_price,
                             ]}
                             promotion={item.model_promotion}
                             modelSlug={item.model_slug}
                          />
                       );
                    })}
            </div>
         </div>
      </div>
   );
};

export default ShopPage;
