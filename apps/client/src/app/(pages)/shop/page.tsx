'use client';

import useProductService from '~/hooks/api/use-product-service';
import usePaginationV2 from '~/hooks/use-pagination';
import { useRouter, useSearchParams } from 'next/navigation';
import { useEffect, useMemo } from 'react';
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
import IphoneModel from '../shop/iphone/_components/iphone-model';
import { Skeleton } from '~/components/ui/skeleton';
import { Separator } from '~/components/ui/separator';
import {
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
} from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';
import { TProductModel } from '~/domain/types/catalog.type';

const ShopPage = () => {
   const router = useRouter();
   const queryParams = useSearchParams();
   const { getProductModelsAsync, getProductModelsState, isLoading } =
      useProductService();

   const productModelsPaginationData = useMemo(() => {
      return (
         getProductModelsState.data ?? {
            total_records: 0,
            total_pages: 0,
            page_size: 10,
            current_page: 1,
            items: [],
            links: {
               first: null,
               last: null,
               prev: null,
               next: null,
            },
         }
      );
   }, [getProductModelsState.data]);

   const {
      getPaginationItems,
      currentPage,
      totalPages,
      totalRecords,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
   } = usePaginationV2(productModelsPaginationData, {});

   const textSearch = queryParams.get('_textSearch');
   const pageParam = queryParams.get('_page');
   const limitParam = queryParams.get('_limit');
   const priceSortParam = queryParams.get('_priceSort');

   useEffect(() => {
      const params: {
         _textSearch?: string | null;
         _page?: number;
         _limit?: number;
         _priceSort?: 'ASC' | 'DESC' | null;
      } = {};

      if (textSearch) {
         params._textSearch = textSearch;
      }
      if (pageParam) {
         params._page = Number(pageParam);
      }
      if (limitParam) {
         params._limit = Number(limitParam);
      }
      if (priceSortParam === 'ASC' || priceSortParam === 'DESC') {
         params._priceSort = priceSortParam;
      }

      getProductModelsAsync(params);
   }, [
      getProductModelsAsync,
      textSearch,
      pageParam,
      limitParam,
      priceSortParam,
   ]);

   const handleSortChange = (value: string) => {
      const params = new URLSearchParams(queryParams.toString());

      if (value === 'price-low-high') {
         params.set('_priceSort', 'ASC');
      } else if (value === 'price-high-low') {
         params.set('_priceSort', 'DESC');
      } else {
         params.delete('_priceSort');
      }

      params.set('_page', '1'); // Reset to first page when sorting changes
      router.push(`/shop?${params.toString()}`);
   };

   const handlePageChange = (page: number) => {
      const params = new URLSearchParams(queryParams.toString());
      params.set('_page', page.toString());
      router.push(`/shop?${params.toString()}`);
      window.scrollTo({ top: 0, behavior: 'smooth' });
   };

   const handlePageSizeChange = (size: string) => {
      const params = new URLSearchParams(queryParams.toString());
      params.set('_limit', size);
      params.set('_page', '1'); // Reset to first page when page size changes
      router.push(`/shop?${params.toString()}`);
   };

   const paginationItems = getPaginationItems();
   const productItems = productModelsPaginationData.items as TProductModel[];

   return (
      <div>
         {/* FILTERS INFO */}
         <div className="w-full border-y border-[#ccc] text-[15px] font-semibold">
            <div className="h-[4.514vw] w-full max-w-[1440px] mx-auto px-5 flex flex-row justify-start items-center">
               <div className="flex flex-row mr-auto">
                  <div className="mx-3 px-[18px] border-x-[1px] border-[#ccc] flex flex-row items-center">
                     <div>
                        {totalRecords}{' '}
                        {totalRecords === 1 ? 'Result' : 'Results'}
                     </div>
                  </div>
               </div>
               <div className="flex flex-row gap-[50px]">
                  <div className="flex flex-row items-center gap-1">
                     <IoChatboxEllipsesOutline />
                     <div>Chat with an expert</div>
                  </div>
                  <Select
                     value={
                        priceSortParam === 'ASC'
                           ? 'price-low-high'
                           : priceSortParam === 'DESC'
                             ? 'price-high-low'
                             : 'recommended'
                     }
                     onValueChange={handleSortChange}
                  >
                     <SelectTrigger className="w-fit flex items-center justify-center border-none focus:ring-0">
                        <GoMultiSelect className="mr-2" />
                        <SelectValue placeholder="Recommended" />
                     </SelectTrigger>
                     <SelectContent className="bg-[#f7f7f7]">
                        <SelectGroup>
                           <SelectItem value="price-low-high">
                              Price: Low to High
                           </SelectItem>
                           <SelectItem value="price-high-low">
                              Price: High to Low
                           </SelectItem>
                        </SelectGroup>
                     </SelectContent>
                  </Select>
               </div>
            </div>
         </div>

         {/* MAIN CONTENT: PRODUCTS */}
         <div className="w-full max-w-[1440px] mx-auto px-5">
            {/* Search Results Header */}
            {textSearch && (
               <div className="py-6 border-b border-[#e5e5e5]">
                  <div className="flex items-center gap-2">
                     <p className="text-[15px] text-gray-600 font-SFProText">
                        Search results for:
                     </p>
                     <p className="text-[15px] text-gray-900 font-SFProText font-semibold">
                        &quot;{textSearch}&quot;
                     </p>
                  </div>
               </div>
            )}

            <div className="flex flex-row gap-6 py-6">
               {/* Products Grid */}
               <div className="flex-1 w-full">
                  <div className="w-full">
                     {/* Products will be displayed here */}
                     <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        <div className="col-span-full text-center py-12 text-gray-500">
                           {isLoading
                              ? Array(5)
                                   .fill(0)
                                   .map((_, index) => (
                                      <div
                                         key={index}
                                         className="flex flex-row bg-white px-5 py-5 rounded-md"
                                      >
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
                              : productItems.length > 0
                                ? productItems.map((item) => (
                                     <div
                                        key={item.id}
                                        className="mb-10 hover:shadow-lg transition-all duration-300 ease-in-out rounded-md"
                                     >
                                        <IphoneModel
                                           models={item.model_items}
                                           colors={item.color_items}
                                           storages={item.storage_items}
                                           displayImageUrl={
                                              item.showcase_images[0]?.image_url
                                           }
                                           averageRating={item.average_rating}
                                           skuPrices={item.sku_prices}
                                           modelSlug={item.slug}
                                        />
                                     </div>
                                  ))
                                : !isLoading && (
                                     <div className="col-span-full text-center py-12 text-gray-500">
                                        No products found
                                     </div>
                                  )}
                        </div>
                     </div>

                     {/* Pagination */}
                     {totalPages > 0 && (
                        <div className="mt-8 flex flex-col sm:flex-row items-center justify-between gap-4 pb-6">
                           {/* Page Info & Size Selector */}
                           <div className="flex items-center gap-4">
                              <div className="text-sm text-gray-600">
                                 Showing{' '}
                                 <span className="font-semibold">
                                    {firstItemIndex}
                                 </span>{' '}
                                 to{' '}
                                 <span className="font-semibold">
                                    {lastItemIndex}
                                 </span>{' '}
                                 of{' '}
                                 <span className="font-semibold">
                                    {totalRecords}
                                 </span>{' '}
                                 results
                              </div>

                              <Select
                                 value={limitSelectValue}
                                 onValueChange={handlePageSizeChange}
                              >
                                 <SelectTrigger className="w-[100px] h-9">
                                    <SelectValue />
                                 </SelectTrigger>
                                 <SelectContent>
                                    <SelectGroup>
                                       <SelectItem value="5">
                                          5 / page
                                       </SelectItem>
                                       <SelectItem value="10">
                                          10 / page
                                       </SelectItem>
                                       <SelectItem value="20">
                                          20 / page
                                       </SelectItem>
                                    </SelectGroup>
                                 </SelectContent>
                              </Select>
                           </div>

                           {/* Pagination Controls */}
                           <div className="flex items-center gap-2">
                              {paginationItems.map((item, index) => {
                                 if (item.type === 'ellipsis') {
                                    return (
                                       <span
                                          key={`ellipsis-${index}`}
                                          className="px-2 text-gray-400"
                                       >
                                          {item.label}
                                       </span>
                                    );
                                 }

                                 if (item.type === 'nav') {
                                    const Icon =
                                       item.label === '<<'
                                          ? ChevronsLeft
                                          : item.label === '>>'
                                            ? ChevronsRight
                                            : item.label === '<'
                                              ? ChevronLeft
                                              : ChevronRight;

                                    return (
                                       <Button
                                          key={`nav-${index}`}
                                          variant="outline"
                                          size="icon"
                                          className="h-9 w-9"
                                          onClick={() =>
                                             item.value !== null &&
                                             handlePageChange(item.value)
                                          }
                                          disabled={
                                             item.disabled ||
                                             item.value === null
                                          }
                                       >
                                          <Icon className="h-4 w-4" />
                                       </Button>
                                    );
                                 }

                                 // Page number button
                                 return (
                                    <Button
                                       key={`page-${item.value}`}
                                       variant={
                                          currentPage === item.value
                                             ? 'default'
                                             : 'outline'
                                       }
                                       size="icon"
                                       className={cn(
                                          'h-9 w-9',
                                          currentPage === item.value &&
                                             'bg-black text-white hover:bg-black/90',
                                       )}
                                       onClick={() =>
                                          item.value !== null &&
                                          handlePageChange(item.value)
                                       }
                                    >
                                       {item.label}
                                    </Button>
                                 );
                              })}
                           </div>
                        </div>
                     )}
                  </div>
               </div>
            </div>
         </div>
      </div>
   );
};

export default ShopPage;
