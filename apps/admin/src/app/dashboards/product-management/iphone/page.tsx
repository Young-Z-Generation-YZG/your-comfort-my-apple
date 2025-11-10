'use client';

import { useEffect, useState } from 'react';
import useProductService from '~/src/hooks/api/use-product-service';
import usePagination from '~/src/hooks/use-pagination';
import IphoneModel from './_components/iphone-model';
import { Button } from '@components/ui/button';
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
   const { getProductsAsync, getProductsState, isLoading } =
      useProductService();

   const [filters, setFilters] = useState<Record<string, any>>({
      _page: 1,
      _limit: 10,
      product_classification: 'IPHONE',
   });

   useEffect(() => {
      getProductsAsync(filters);
   }, [getProductsAsync, filters]);

   const {
      currentPage,
      totalPages,
      totalRecords,
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,
      paginationItems,
      getPageNumbers,
   } = usePagination(
      getProductsState.isSuccess &&
         getProductsState.data &&
         getProductsState.data.items.length > 0
         ? getProductsState.data
         : {
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
   );

   const handlePageChange = (page: number) => {
      setFilters((prev) => ({ ...prev, _page: page }));
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
            {getProductsState.isError ? (
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
            ) : getProductsState.isSuccess &&
              getProductsState.data &&
              paginationItems.length > 0 ? (
               <div className="flex flex-col gap-4">
                  {paginationItems.map((productModel) => (
                     <IphoneModel
                        key={productModel.id}
                        productModel={productModel}
                     />
                  ))}
               </div>
            ) : getProductsState.isSuccess &&
              getProductsState.data &&
              paginationItems.length === 0 ? (
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
               <div className="flex flex-row items-center justify-between">
                  {/* Page Info */}
                  <div className="flex flex-row items-center gap-2">
                     <span className="text-sm text-muted-foreground">
                        Showing page{' '}
                        <span className="font-medium">{currentPage}</span> of{' '}
                        <span className="font-medium">{totalPages}</span>
                     </span>
                  </div>

                  {/* Pagination Buttons */}
                  <div className="flex flex-row items-center gap-2">
                     {/* First Page */}
                     <Button
                        variant="outline"
                        size="icon"
                        className="h-9 w-9"
                        onClick={() => handlePageChange(1)}
                        disabled={isFirstPage}
                        title="First page"
                     >
                        <ChevronsLeft className="h-4 w-4" />
                     </Button>

                     {/* Previous Page */}
                     <Button
                        variant="outline"
                        size="icon"
                        className="h-9 w-9"
                        onClick={() => handlePageChange(currentPage - 1)}
                        disabled={!isPrevPage}
                        title="Previous page"
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
                                    <Ellipsis className="h-4 w-4" />
                                 </span>
                              );
                           }

                           return (
                              <Button
                                 key={index}
                                 variant={
                                    currentPage === page ? 'default' : 'outline'
                                 }
                                 size="icon"
                                 className={cn(
                                    'h-9 w-9',
                                    currentPage === page &&
                                       'bg-black text-white hover:bg-black/90',
                                 )}
                                 onClick={() =>
                                    handlePageChange(page as number)
                                 }
                              >
                                 {page as number}
                              </Button>
                           );
                        })}
                     </div>

                     {/* Next Page */}
                     <Button
                        variant="outline"
                        size="icon"
                        className="h-9 w-9"
                        onClick={() => handlePageChange(currentPage + 1)}
                        disabled={!isNextPage}
                        title="Next page"
                     >
                        <ChevronRight className="h-4 w-4" />
                     </Button>

                     {/* Last Page */}
                     <Button
                        variant="outline"
                        size="icon"
                        className="h-9 w-9"
                        onClick={() => handlePageChange(totalPages)}
                        disabled={isLastPage}
                        title="Last page"
                     >
                        <ChevronsRight className="h-4 w-4" />
                     </Button>
                  </div>
               </div>
            </div>
         )}
      </div>
   );
};

export default ProductManagementIphonePage;
