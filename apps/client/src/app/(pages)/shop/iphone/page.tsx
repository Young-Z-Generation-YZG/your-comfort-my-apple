'use client';

import { useEffect, useMemo, useState } from 'react';
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
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '~/components/ui/dialog';
import FilterSection from './_components/_layout/filter-section';
import IphoneModel from './[slug]/_components/iphone-model';
import { Skeleton } from '~/components/ui/skeleton';
import { Separator } from '~/components/ui/separator';
import {
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
} from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';
import SuggestionProducts from './_components/suggestion-products';
import useFilters from '~/hooks/use-filter';
import useProductService from '~/hooks/api/use-product-service';
import { IGetProductModelsByCategorySlugQueryParams } from '~/domain/interfaces/catalog.interface';
import usePaginationV2 from '~/hooks/use-pagination';
import { TProductModel } from '~/domain/types/catalog.type';

const IphoneShopPage = () => {
   const [isFilterDialogOpen, setIsFilterDialogOpen] = useState(false);

   const { filters, setFilters } =
      useFilters<IGetProductModelsByCategorySlugQueryParams>({
         _page: 'number',
         _limit: 'number',
         _colors: { array: 'string' },
         _storages: { array: 'string' },
         _models: { array: 'string' },
         _minPrice: 'number',
         _maxPrice: 'number',
         _priceSort: 'string',
      });

   const {
      getProductModelsByCategorySlugAsync,
      getProductModelsByCategorySlugState,
      isLoading,
   } = useProductService();

   useEffect(() => {
      getProductModelsByCategorySlugAsync('iphone', filters);
   }, [filters, getProductModelsByCategorySlugAsync]);

   const {
      currentPage,
      totalPages,
      totalRecords,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
      getPaginationItems,
   } = usePaginationV2(
      getProductModelsByCategorySlugState.data ?? {
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
      },
      {
         pageSizeOverride: filters._limit ?? null,
         currentPageOverride: filters._page ?? null,
         fallbackPageSize: 10,
      },
   );

   const paginationItems = getPaginationItems();

   const handleSortChange = (value: string) => {
      // Map sort value to priceSort
      let priceSort: 'ASC' | 'DESC' | null = null;

      if (value === 'price-low-high') {
         priceSort = 'ASC';
      } else if (value === 'price-high-low') {
         priceSort = 'DESC';
      }

      setFilters({ _priceSort: priceSort });
   };

   const handlePageChange = (page: number) => {
      setFilters({ _page: page });
      window.scrollTo({ top: 0, behavior: 'smooth' });
   };

   const handlePageSizeChange = (size: string) => {
      setFilters({ _limit: Number(size), _page: 1 });
   };

   return (
      <div>
         {/* FILTERS INFO */}
         <div className="w-full border-y border-[#ccc] text-sm md:text-[15px] font-semibold">
            <div className="min-h-[60px] md:h-[4.514vw] w-full max-w-[1440px] mx-auto px-3 md:px-5 flex flex-col md:flex-row justify-between md:justify-start items-start md:items-center gap-3 md:gap-0 py-3 md:py-0">
               <div className="flex flex-row flex-wrap items-center gap-2 md:gap-0 md:mr-auto w-full md:w-auto">
                  {/* Mobile & Tablet Filter Button */}
                  <Dialog
                     open={isFilterDialogOpen}
                     onOpenChange={setIsFilterDialogOpen}
                  >
                     <DialogTrigger asChild>
                        <Button
                           variant="outline"
                           className="lg:hidden flex items-center gap-2 border-[#ccc]"
                        >
                           <FaFilter />
                           <span>
                              Filters
                              {Object.keys(filters).length > 0 && (
                                 <span className="ml-1">
                                    ({Object.keys(filters).length})
                                 </span>
                              )}
                           </span>
                        </Button>
                     </DialogTrigger>
                     <DialogContent className="max-w-[90vw] max-h-[90vh] overflow-y-auto">
                        <DialogHeader>
                           <DialogTitle>Filters</DialogTitle>
                        </DialogHeader>
                        <div className="w-full">
                           <FilterSection />
                        </div>
                     </DialogContent>
                  </Dialog>

                  {/* Desktop Filter Label */}
                  <div className="hidden lg:flex text-[#218bff] flex-row items-center gap-2">
                     <FaFilter />
                     <div>
                        Filters
                        {Object.keys(filters).length > 0 && (
                           <span className="ml-1">
                              ({Object.keys(filters).length})
                           </span>
                        )}
                     </div>
                  </div>

                  <div className="hidden md:flex mx-3 px-[18px] border-x-[1px] border-[#ccc] flex-row items-center">
                     <div>
                        {totalRecords}{' '}
                        {totalRecords === 1 ? 'Result' : 'Results'}
                     </div>
                  </div>

                  {/* Mobile Results Count */}
                  <div className="md:hidden text-gray-600">
                     {totalRecords} {totalRecords === 1 ? 'Result' : 'Results'}
                  </div>

                  {Object.keys(filters).length > 0 && (
                     <div className="flex flex-row items-center">
                        <Button
                           onClick={() => {
                              setFilters({
                                 _page: null,
                                 _limit: null,
                                 _colors: null,
                                 _storages: null,
                                 _models: null,
                                 _minPrice: null,
                                 _maxPrice: null,
                                 _priceSort: null,
                              });
                           }}
                           className="h-[22.5px] p-0 text-xs md:text-[15px] font-semibold border-b border-[#000] hover:text-blue-600 rounded-none bg-transparent text-black hover:bg-transparent hover:border-b-blue-500/50 transition-colors"
                        >
                           Clear Filters
                        </Button>
                     </div>
                  )}
               </div>
               <div className="flex flex-row gap-3 md:gap-[50px] items-center w-full md:w-auto justify-between md:justify-start">
                  <div className="hidden md:flex flex-row items-center gap-1">
                     <IoChatboxEllipsesOutline />
                     <div>Chat with an expert</div>
                  </div>
                  <Select
                     value={
                        filters._priceSort === 'ASC'
                           ? 'price-low-high'
                           : filters._priceSort === 'DESC'
                             ? 'price-high-low'
                             : 'recommended'
                     }
                     onValueChange={handleSortChange}
                  >
                     <SelectTrigger className="w-fit flex items-center justify-center border-none focus:ring-0 text-xs md:text-sm">
                        <GoMultiSelect className="mr-2 h-4 w-4" />
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

         {/* MAIN CONTENT: FILTERS + PRODUCTS */}
         <div className="w-full max-w-[1440px] mx-auto px-3 md:px-5">
            <div className="flex flex-col lg:flex-row gap-6 py-4 md:py-6">
               {/* Left: Filter Section - Hidden on mobile, shown in dialog */}
               <div className="hidden lg:block">
                  <FilterSection />
               </div>

               {/* Right: Products Grid */}
               <div className="flex-1 w-full">
                  <div className="w-full">
                     {/* Products will be displayed here */}
                     <div className="grid grid-cols-1 gap-4 md:gap-6">
                        {isLoading
                           ? Array(5)
                                .fill(0)
                                .map((_, index) => (
                                   <div
                                      key={index}
                                      className="flex flex-col md:flex-row bg-white px-3 md:px-5 py-3 md:py-5 rounded-md"
                                   >
                                      {/* image */}
                                      <div className="image flex basis-[23%] items-center justify-center h-[200px] md:h-[300px] rounded-lg overflow-hidden mb-4 md:mb-0">
                                         <Skeleton className="h-full w-full rounded-lg" />
                                      </div>

                                      {/* content */}
                                      <div className="flex flex-col relative basis-[40%] lg:basis-[30%] px-0 md:px-7">
                                         <div className="content flex flex-col gap-2">
                                            <h2 className="font-SFProText font-normal text-lg md:text-xl">
                                               <Skeleton className="h-5 w-full md:w-[350px]" />
                                            </h2>

                                            <span className="flex flex-row gap-2">
                                               <p className="first-letter:uppercase text-xs md:text-sm">
                                                  colors:
                                               </p>
                                            </span>

                                            {/* Colors */}
                                            <div className="flex flex-row gap-2">
                                               <Skeleton className="h-5 w-full md:w-[350px]" />
                                            </div>

                                            <span className="flex flex-row gap-2 mt-2">
                                               <p className="first-letter:uppercase text-xs md:text-sm">
                                                  Storage:
                                               </p>
                                            </span>

                                            {/* Storage */}
                                            <div className="flex flex-row gap-2">
                                               <Skeleton className="h-5 w-full md:w-[350px]" />
                                            </div>

                                            <Separator className="mt-2" />

                                            <span className="flex flex-row gap-2 mt-2 items-center">
                                               <Skeleton className="h-5 w-full md:w-[350px]" />
                                            </span>

                                            <span className="gap-2 mt-2 text-right">
                                               <Skeleton className="h-5 w-full md:w-[350px]" />
                                            </span>
                                         </div>
                                      </div>

                                      {/* feature */}
                                      <div className="flex flex-col justify-between flex-1 border-t-2 md:border-l-2 md:border-t-0 border-[#E5E7EB] pt-4 md:pt-0 px-0 md:px-4 mt-4 md:mt-0">
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

                                         <Skeleton className="h-12 w-full mt-4 md:mt-5" />
                                      </div>
                                   </div>
                                ))
                           : getProductModelsByCategorySlugState.isSuccess &&
                               getProductModelsByCategorySlugState.data?.items
                                  .length > 0
                             ? getProductModelsByCategorySlugState.data.items.map(
                                  (item: TProductModel) => (
                                     <div
                                        key={item.id}
                                        className="mb-4 md:mb-10 hover:shadow-lg transition-all duration-300 ease-in-out rounded-md"
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
                                  ),
                               )
                             : !isLoading && (
                                  <div className="col-span-full text-center py-12 text-gray-500">
                                     No products found
                                  </div>
                               )}
                     </div>

                     {/* Pagination */}
                     {totalPages > 0 && (
                        <div className="mt-6 md:mt-8 flex flex-col sm:flex-row items-center justify-between gap-4 pb-6">
                           {/* Page Info & Size Selector */}
                           <div className="flex flex-col sm:flex-row items-center gap-3 sm:gap-4 w-full sm:w-auto">
                              <Select
                                 value={limitSelectValue}
                                 onValueChange={handlePageSizeChange}
                              >
                                 <SelectTrigger className="w-full sm:w-[100px] h-9">
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

                              <div className="text-xs sm:text-sm text-gray-600 text-center sm:text-left">
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
                           </div>

                           {/* Pagination Controls */}
                           <div className="flex items-center gap-1 sm:gap-2 flex-wrap justify-center">
                              {paginationItems.map(
                                 (
                                    item: {
                                       type: 'nav' | 'page' | 'ellipsis';
                                       label: string;
                                       value: number | null;
                                       disabled?: boolean;
                                    },
                                    index: number,
                                 ) => {
                                    if (item.type === 'ellipsis') {
                                       return (
                                          <span
                                             key={`ellipsis-${index}`}
                                             className="px-2 text-gray-400 flex items-center"
                                          >
                                             <Ellipsis className="h-4 w-4" />
                                          </span>
                                       );
                                    }

                                    const isCurrentPage =
                                       item.type === 'page' &&
                                       item.value === currentPage;

                                    return (
                                       <Button
                                          key={`${item.type}-${item.label}-${index}`}
                                          variant={
                                             isCurrentPage
                                                ? 'default'
                                                : 'outline'
                                          }
                                          size="icon"
                                          className={cn(
                                             'h-9 w-9',
                                             isCurrentPage &&
                                                'bg-black text-white hover:bg-black/90',
                                          )}
                                          disabled={
                                             item.disabled ||
                                             item.value === null
                                          }
                                          onClick={() => {
                                             if (
                                                item.value !== null &&
                                                !item.disabled
                                             ) {
                                                handlePageChange(item.value);
                                             }
                                          }}
                                       >
                                          {item.type === 'nav' ? (
                                             item.label === '<<' ? (
                                                <ChevronsLeft className="h-4 w-4" />
                                             ) : item.label === '>>' ? (
                                                <ChevronsRight className="h-4 w-4" />
                                             ) : item.label === '<' ? (
                                                <ChevronLeft className="h-4 w-4" />
                                             ) : (
                                                <ChevronRight className="h-4 w-4" />
                                             )
                                          ) : (
                                             item.label
                                          )}
                                       </Button>
                                    );
                                 },
                              )}
                           </div>
                        </div>
                     )}
                  </div>
               </div>
            </div>
         </div>

         {/* SUGGESTION PRODUCTS */}
         <SuggestionProducts />
      </div>
   );
};

export default IphoneShopPage;
