'use client';

import { useMemo } from 'react';
import { FaFilter } from 'react-icons/fa6';
import { Button } from '@components/ui/button';
import { IoChatboxEllipsesOutline } from 'react-icons/io5';
import { GoMultiSelect } from 'react-icons/go';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import FilterSection from './_components/_layout/filter-section';
import IphoneModel from './_components/iphone-model';
import { Skeleton } from '@components/ui/skeleton';
import { Separator } from '@components/ui/separator';
import { iphoneFakeData } from './_data/fake-data';
import { useGetModelsAsyncQuery } from '~/infrastructure/services/catalog.service';
import {
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
} from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';
import { useShopFilters } from './_hooks/useShopFilters';
import { useApiErrorHandler } from './_hooks/useApiErrorHandler';

const IphoneShopPage = () => {
   // Use custom hook instead of Redux
   const {
      filters,
      updateFilters,
      clearFilters,
      queryString,
      activeFiltersCount,
   } = useShopFilters();

   const {
      data: modelsDataAsync,
      isSuccess: modelsDataIsSuccess,
      isLoading: modelsDataIsLoading,
      isFetching: modelsDataIsFetching,
      isError: modelsDataIsError,
      error: modelsDataError,
   } = useGetModelsAsyncQuery(queryString);

   // Handle API errors with custom hook
   useApiErrorHandler(modelsDataIsError, modelsDataError, {
      title: 'Failed to Load Products',
      description: 'Unable to fetch iPhone models. Showing cached data.',
   });

   // Use API data if available, otherwise fallback to fake data
   const displayData = useMemo(() => {
      if (modelsDataIsSuccess && modelsDataAsync?.items?.length) {
         return modelsDataAsync.items;
      }
      return iphoneFakeData.items;
   }, [modelsDataAsync, modelsDataIsSuccess]);

   // Pagination data
   const paginationData = useMemo(() => {
      if (modelsDataIsSuccess && modelsDataAsync) {
         return {
            totalRecords: modelsDataAsync.total_records,
            totalPages: modelsDataAsync.total_pages,
            currentPage: modelsDataAsync.current_page,
            pageSize: modelsDataAsync.page_size,
         };
      }
      return null;
   }, [modelsDataAsync, modelsDataIsSuccess]);

   // Results count from actual data
   const resultsCount = useMemo(() => {
      if (paginationData) {
         return paginationData.totalRecords;
      }
      return displayData.length;
   }, [paginationData, displayData]);

   const handleSortChange = (value: string) => {
      // Map sort value to priceSort
      let priceSort: 'ASC' | 'DESC' | null = null;

      if (value === 'price-low-high') {
         priceSort = 'ASC';
      } else if (value === 'price-high-low') {
         priceSort = 'DESC';
      }

      updateFilters({ priceSort });
   };

   const handlePageChange = (page: number) => {
      updateFilters({ page });
      window.scrollTo({ top: 0, behavior: 'smooth' });
   };

   const handlePageSizeChange = (size: string) => {
      updateFilters({ pageSize: Number(size), page: 1 });
   };

   // Generate page numbers to display
   const getPageNumbers = () => {
      if (!paginationData) return [];

      const { currentPage, totalPages } = paginationData;
      const pages: (number | string)[] = [];
      const maxPagesToShow = 7;

      if (totalPages <= maxPagesToShow) {
         for (let i = 1; i <= totalPages; i++) {
            pages.push(i);
         }
      } else {
         if (currentPage <= 4) {
            for (let i = 1; i <= 5; i++) pages.push(i);
            pages.push('...');
            pages.push(totalPages);
         } else if (currentPage >= totalPages - 3) {
            pages.push(1);
            pages.push('...');
            for (let i = totalPages - 4; i <= totalPages; i++) pages.push(i);
         } else {
            pages.push(1);
            pages.push('...');
            for (let i = currentPage - 1; i <= currentPage + 1; i++)
               pages.push(i);
            pages.push('...');
            pages.push(totalPages);
         }
      }

      return pages;
   };

   return (
      <div>
         {/* FILTERS INFO */}
         <div className="w-full border-y border-[#ccc] text-[15px] font-semibold">
            <div className="h-[4.514vw] w-full max-w-[1440px] mx-auto px-5 flex flex-row justify-start items-center">
               <div className="flex flex-row mr-auto">
                  <div className="text-[#218bff] flex flex-row items-center gap-2">
                     <FaFilter />
                     <div>
                        Filters
                        {activeFiltersCount > 0 && (
                           <span className="ml-1">({activeFiltersCount})</span>
                        )}
                     </div>
                  </div>
                  <div className="mx-3 px-[18px] border-x-[1px] border-[#ccc] flex flex-row items-center">
                     <div>
                        {resultsCount}{' '}
                        {resultsCount === 1 ? 'Result' : 'Results'}
                     </div>
                  </div>
                  {activeFiltersCount > 0 && (
                     <div className="flex flex-row items-center">
                        <Button
                           onClick={clearFilters}
                           className="h-[22.5px] p-0 text-[15px] font-semibold border-b border-[#000] hover:text-blue-600 rounded-none bg-transparent text-black hover:bg-transparent hover:border-b-blue-500/50 transition-colors"
                        >
                           Clear Filters
                        </Button>
                     </div>
                  )}
               </div>
               <div className="flex flex-row gap-[50px]">
                  <div className="flex flex-row items-center gap-1">
                     <IoChatboxEllipsesOutline />
                     <div>Chat with an expert</div>
                  </div>
                  <Select
                     value={
                        filters.priceSort === 'ASC'
                           ? 'price-low-high'
                           : filters.priceSort === 'DESC'
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
                           <SelectItem value="recommended">
                              Recommended
                           </SelectItem>
                           <SelectItem value="newest">Newest</SelectItem>
                           <SelectItem value="price-low-high">
                              Price: Low to High
                           </SelectItem>
                           <SelectItem value="price-high-low">
                              Price: High to Low
                           </SelectItem>
                           <SelectItem value="most-clicked">
                              Most Clicked
                           </SelectItem>
                           <SelectItem value="highest-rated">
                              Highest Rated
                           </SelectItem>
                           <SelectItem value="most-reviewed">
                              Most Reviewed
                           </SelectItem>
                           <SelectItem value="online-availability">
                              Online Availability
                           </SelectItem>
                        </SelectGroup>
                     </SelectContent>
                  </Select>
               </div>
            </div>
         </div>

         {/* MAIN CONTENT: FILTERS + PRODUCTS */}
         <div className="w-full max-w-[1440px] mx-auto px-5">
            <div className="flex flex-row gap-6 py-6">
               {/* Left: Filter Section */}
               <FilterSection />

               {/* Right: Products Grid */}
               <div className="flex-1">
                  <div className="w-full">
                     {/* Products will be displayed here */}
                     <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {/* TODO: Map products here when API is integrated */}
                        <div className="col-span-full text-center py-12 text-gray-500">
                           {modelsDataIsLoading || modelsDataIsFetching
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
                              : displayData.map((item) => (
                                   <div key={item.id}>
                                      <IphoneModel
                                         models={item.model_items}
                                         colors={item.color_items}
                                         storages={item.storage_items}
                                         averageRating={item.average_rating}
                                         skuPrices={item.sku_prices}
                                         modelSlug={item.slug}
                                      />
                                   </div>
                                ))}
                        </div>
                     </div>

                     {/* Pagination */}
                     {paginationData && paginationData.totalPages > 0 && (
                        <div className="mt-8 flex flex-col sm:flex-row items-center justify-between gap-4 pb-6">
                           {/* Page Info & Size Selector */}
                           <div className="flex items-center gap-4">
                              <div className="text-sm text-gray-600">
                                 Showing{' '}
                                 <span className="font-semibold">
                                    {(paginationData.currentPage - 1) *
                                       paginationData.pageSize +
                                       1}
                                 </span>{' '}
                                 to{' '}
                                 <span className="font-semibold">
                                    {Math.min(
                                       paginationData.currentPage *
                                          paginationData.pageSize,
                                       paginationData.totalRecords,
                                    )}
                                 </span>{' '}
                                 of{' '}
                                 <span className="font-semibold">
                                    {paginationData.totalRecords}
                                 </span>{' '}
                                 results
                              </div>

                              <Select
                                 value={filters.pageSize.toString()}
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
                                       <SelectItem value="50">
                                          50 / page
                                       </SelectItem>
                                    </SelectGroup>
                                 </SelectContent>
                              </Select>
                           </div>

                           {/* Pagination Controls */}
                           <div className="flex items-center gap-2">
                              {/* First Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() => handlePageChange(1)}
                                 disabled={paginationData.currentPage === 1}
                              >
                                 <ChevronsLeft className="h-4 w-4" />
                              </Button>

                              {/* Previous Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() =>
                                    handlePageChange(
                                       paginationData.currentPage - 1,
                                    )
                                 }
                                 disabled={paginationData.currentPage === 1}
                              >
                                 <ChevronLeft className="h-4 w-4" />
                              </Button>

                              {/* Page Numbers */}
                              <div className="flex items-center gap-1">
                                 {getPageNumbers().map((page, index) => {
                                    if (page === '...') {
                                       return (
                                          <span
                                             key={`ellipsis-${index}`}
                                             className="px-2 text-gray-400"
                                          >
                                             ...
                                          </span>
                                       );
                                    }

                                    return (
                                       <Button
                                          key={page}
                                          variant={
                                             paginationData.currentPage === page
                                                ? 'default'
                                                : 'outline'
                                          }
                                          size="icon"
                                          className={cn(
                                             'h-9 w-9',
                                             paginationData.currentPage ===
                                                page &&
                                                'bg-black text-white hover:bg-black/90',
                                          )}
                                          onClick={() =>
                                             handlePageChange(page as number)
                                          }
                                       >
                                          {page}
                                       </Button>
                                    );
                                 })}
                              </div>

                              {/* Next Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() =>
                                    handlePageChange(
                                       paginationData.currentPage + 1,
                                    )
                                 }
                                 disabled={
                                    paginationData.currentPage ===
                                    paginationData.totalPages
                                 }
                              >
                                 <ChevronRight className="h-4 w-4" />
                              </Button>

                              {/* Last Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() =>
                                    handlePageChange(paginationData.totalPages)
                                 }
                                 disabled={
                                    paginationData.currentPage ===
                                    paginationData.totalPages
                                 }
                              >
                                 <ChevronsRight className="h-4 w-4" />
                              </Button>
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

export default IphoneShopPage;
