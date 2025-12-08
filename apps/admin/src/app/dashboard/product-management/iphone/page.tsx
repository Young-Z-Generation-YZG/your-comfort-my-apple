'use client';

import { useEffect, useState } from 'react';
import useProductService from '~/src/hooks/api/use-product-service';
import usePaginationV2 from '~/src/hooks/use-pagination';
import IphoneModel from './_components/iphone-model';
import { Button } from '@components/ui/button';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   ChevronsLeft,
   ChevronsRight,
   ChevronLeft,
   ChevronRight,
   Ellipsis,
   Smartphone,
} from 'lucide-react';
import { cn } from '~/src/infrastructure/lib/utils';
import { LoadingOverlay } from '@components/loading-overlay';

const ProductManagementIphonePage = () => {
   const {
      getProductModelsByCategorySlugAsync,
      getProductModelsByCategorySlugState,
      isLoading,
   } = useProductService();

   const [filters, setFilters] = useState<Record<string, any>>({
      _page: 1,
      _limit: 10,
      product_classification: 'IPHONE',
   });

   useEffect(() => {
      getProductModelsByCategorySlugAsync('iphone', filters);
   }, [filters, getProductModelsByCategorySlugAsync]);

   const {
      getPaginationItems,
      currentPage,
      totalRecords,
      totalPages,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
   } = usePaginationV2(
      getProductModelsByCategorySlugState.data ?? {
         total_records: 0,
         total_pages: 0,
         page_size: 0,
         current_page: 0,
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

   const productItems = getProductModelsByCategorySlugState.data?.items ?? [];
   const paginationNavItems = getPaginationItems();

   const handlePageChange = (page: number) => {
      setFilters((prev) => ({ ...prev, _page: page }));
   };

   const handlePageSizeChange = (value: string) => {
      setFilters({
         _limit: Number(value),
         _page: 1,
      });
   };

   return (
      <div className="flex flex-col h-full">
         <LoadingOverlay isLoading={isLoading} />

         {/* Page Header */}
         <div className="flex flex-col gap-2 p-6 border-b bg-white">
            <div className="flex flex-row items-center justify-between">
               <div className="flex flex-col gap-1">
                  <h1 className="text-3xl font-bold tracking-tight">
                     iPhone Product Management
                  </h1>
                  <p className="text-muted-foreground">
                     Manage and view all iPhone product models
                  </p>
               </div>
               <div className="flex flex-row items-center gap-4">
                  <div className="flex flex-col items-end">
                     <span className="text-sm text-muted-foreground">
                        Total Products
                     </span>
                     <span className="text-2xl font-bold">{totalRecords}</span>
                  </div>
               </div>
            </div>
         </div>

         {/* Main Content */}
         <div className="flex-1 overflow-auto p-6 bg-muted/30">
            {/* Products List */}
            {getProductModelsByCategorySlugState.isError ? (
               <div className="flex flex-col items-center justify-center py-16 bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700">
                  <div className="flex flex-col items-center gap-4">
                     <div className="h-16 w-16 rounded-full bg-destructive/10 flex items-center justify-center">
                        <Smartphone className="h-8 w-8 text-destructive" />
                     </div>
                     <div className="text-center">
                        <p className="font-medium text-lg text-destructive mb-1">
                           Error loading products
                        </p>
                        <p className="text-sm text-muted-foreground">
                           Please try again later or contact support if the
                           problem persists.
                        </p>
                     </div>
                  </div>
               </div>
            ) : getProductModelsByCategorySlugState.isSuccess &&
              getProductModelsByCategorySlugState.data &&
              productItems.length > 0 ? (
               <div className="flex flex-col gap-4">
                  {productItems.map((productModel) => (
                     <IphoneModel
                        key={productModel.id}
                        productModel={productModel}
                     />
                  ))}
               </div>
            ) : getProductModelsByCategorySlugState.isSuccess &&
              getProductModelsByCategorySlugState.data &&
              productItems.length === 0 ? (
               <div className="flex flex-col items-center justify-center py-16 bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700">
                  <div className="flex flex-col items-center gap-4">
                     <div className="h-16 w-16 rounded-full bg-muted flex items-center justify-center">
                        <Smartphone className="h-8 w-8 text-muted-foreground" />
                     </div>
                     <div className="text-center">
                        <p className="font-medium text-lg mb-1">
                           No iPhone products found
                        </p>
                        <p className="text-sm text-muted-foreground">
                           Try adjusting your filters or check back later.
                        </p>
                     </div>
                  </div>
               </div>
            ) : null}
         </div>

         {/* Pagination Footer */}
         {totalPages > 0 && (
            <div className="border-t bg-white p-4">
               <div className="flex items-center justify-between">
                  <div className="flex items-center gap-2">
                     <Select
                        value={limitSelectValue}
                        onValueChange={handlePageSizeChange}
                     >
                        <SelectTrigger className="w-[100px] h-9">
                           <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                           <SelectGroup>
                              <SelectItem value="5">5 / page</SelectItem>
                              <SelectItem value="10">10 / page</SelectItem>
                              <SelectItem value="20">20 / page</SelectItem>
                           </SelectGroup>
                        </SelectContent>
                     </Select>

                     <div className="text-sm text-muted-foreground">
                        Showing{' '}
                        <span className="font-medium">{firstItemIndex}</span> to{' '}
                        <span className="font-medium">{lastItemIndex}</span> of{' '}
                        <span className="font-medium">{totalRecords}</span>{' '}
                        products
                     </div>
                  </div>

                  <div className="flex items-center gap-2">
                     {paginationNavItems.map((item, index) => {
                        if (item.type === 'ellipsis') {
                           return (
                              <span
                                 key={`ellipsis-${index}`}
                                 className="px-2 text-gray-400"
                              >
                                 <Ellipsis className="h-4 w-4" />
                              </span>
                           );
                        }

                        if (item.type === 'nav') {
                           const Icon =
                              item.label === '<<'
                                 ? ChevronsLeft
                                 : item.label === '<'
                                   ? ChevronLeft
                                   : item.label === '>'
                                     ? ChevronRight
                                     : item.label === '>>'
                                       ? ChevronsRight
                                       : null;

                           if (!Icon) return null;

                           return (
                              <Button
                                 key={index}
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() =>
                                    item.value && handlePageChange(item.value)
                                 }
                                 disabled={item.disabled}
                                 title={
                                    item.label === '<<'
                                       ? 'First page'
                                       : item.label === '<'
                                         ? 'Previous page'
                                         : item.label === '>'
                                           ? 'Next page'
                                           : 'Last page'
                                 }
                              >
                                 <Icon className="h-4 w-4" />
                              </Button>
                           );
                        }

                        if (item.type === 'page') {
                           return (
                              <Button
                                 key={index}
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
                                    item.value && handlePageChange(item.value)
                                 }
                              >
                                 {item.label}
                              </Button>
                           );
                        }
                        return null;
                     })}
                  </div>
               </div>
            </div>
         )}
      </div>
   );
};

export default ProductManagementIphonePage;
