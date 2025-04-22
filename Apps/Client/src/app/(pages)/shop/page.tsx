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
import IphoneModelItem from './_components/iphone-model-item';
import { useGetModelsAsyncQuery } from '~/infrastructure/services/catalog.service';
import { IIphoneModelResponse } from '~/domain/interfaces/catalogs/iPhone-model.inteface';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { Skeleton } from '@components/ui/skeleton';
import { Separator } from '@components/ui/separator';
import FilterSection from './_components/_layout/filter-section';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useRouter, useSearchParams } from 'next/navigation';
import {
   clearAppFilters,
   FiltersType,
} from '~/infrastructure/redux/features/app.slice';
import { useDispatch } from 'react-redux';

const ShopPage = () => {
   const [isLoading, setIsLoading] = useState(true);
   const [models, setModels] = useState<IIphoneModelResponse[]>([]);
   const router = useRouter();

   const searchParams = useSearchParams();
   const _productColors = searchParams.getAll('_productColors');
   const _productModels = searchParams.getAll('_productModels');
   const _productStorages = searchParams.getAll('_productStorages');

   const dispatch = useDispatch();

   const filters = useAppSelector((state) => state.app.value.filters);

   const buildQueryParams = (
      _productColors: string[] = [],
      _productModels: string[] = [],
      _productStorages: string[] = [],
   ) => {
      let params = '';

      if (_productColors.length > 0) {
         const colors = _productColors.map((item) => `_productColors=${item}`);

         params += `${colors.join('&')}&`;
      }

      if (_productModels.length > 0) {
         const models = _productModels.map((item) => `_productModels=${item}`);

         params += `${models.join('&')}&`;
      }

      if (_productStorages.length > 0) {
         const storages = _productStorages.map(
            (item) => `_productStorages=${item}`,
         );

         params += `${storages.join('&')}&`;
      }

      return params;
   };

   const params = buildQueryParams(
      _productColors,
      _productModels,
      _productStorages,
   );

   const {
      data: modelsDataAsync,
      isSuccess: modelsDataIsSuccess,
      isLoading: modelsDataIsLoading,
      isFetching: modelsDataIsFetching,
   } = useGetModelsAsyncQuery(params);

   useEffect(() => {
      if (modelsDataIsSuccess) {
         setModels(modelsDataAsync.items);
      }
   }, [modelsDataAsync]);

   useEffect(() => {
      if (modelsDataIsLoading || modelsDataIsFetching) {
         setIsLoading(true);
      } else {
         setTimeout(() => {
            setIsLoading(!isLoading);
         }, 500);
      }
   }, [modelsDataIsLoading, modelsDataIsFetching]);

   useEffect(() => {
      console.log('filters in store', filters);

      let queryParams = '';

      for (const key in filters) {
         if (filters[key as keyof FiltersType].length > 0) {
            switch (key) {
               case 'colors':
                  const colorValues = filters[key].map(
                     (item) => `_productColors=${item.name}`,
                  );
                  queryParams += `${colorValues.join('&')}&`;
                  break;
               case 'models':
                  const modelValues = filters[key].map(
                     (item) => `_productModels=${item.value}`,
                  );
                  queryParams += `${modelValues.join('&')}&`;
                  break;
               case 'storages':
                  const storageValues = filters[key].map(
                     (item) => `_productStorages=${item.value}`,
                  );
                  queryParams += `${storageValues.join('&')}&`;
                  break;
               default:
                  break;
            }
         }
      }

      router.push(`/shop?${queryParams}`);
   }, [filters]);

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
                     <Button
                        onClick={() => {
                           dispatch(clearAppFilters());
                        }}
                        className="h-[22.5px] p-0 text-[15px] font-semibold border-b border-[#000] hover:text-blue-600 rounded-none bg-transparent text-black hover:bg-transparent hover:border-b-blue-500/50"
                     >
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
            <FilterSection />

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
