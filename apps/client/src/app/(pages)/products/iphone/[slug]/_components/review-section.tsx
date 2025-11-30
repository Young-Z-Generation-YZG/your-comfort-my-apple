'use client';

import { useEffect, useMemo, useState } from 'react';
import { useParams } from 'next/navigation';
import {
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
} from 'lucide-react';
import useReviewService from '@components/hooks/api/use-review-service';
import ReviewItem from './review-item';
import { Button } from '@components/ui/button';
import { Separator } from '@components/ui/separator';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { cn } from '~/infrastructure/lib/utils';
import RatingStar from '@components/ui/rating-star';
import usePaginationV2 from '@components/hooks/use-pagination';

const ReviewsSection = () => {
   const { slug } = useParams();

   const [_page, setPage] = useState<number | null>(null);
   const [_limit, setLimit] = useState<number | null>(null);

   const {
      getReviewByProductModelSlugAsync,
      getReviewByProductModelSlugState,
      isLoading,
   } = useReviewService();

   // Default pagination data
   const defaultPaginationData = useMemo(
      () => ({
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
      }),
      [],
   );

   const paginationData = useMemo(() => {
      return getReviewByProductModelSlugState.data ?? defaultPaginationData;
   }, [getReviewByProductModelSlugState.data, defaultPaginationData]);

   const {
      currentPage,
      totalPages,
      totalRecords,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
      getPaginationItems,
   } = usePaginationV2(paginationData, {
      pageSizeOverride: _limit ?? null,
      currentPageOverride: _page ?? null,
      fallbackPageSize: 5,
   });

   const paginationItems = getPaginationItems();
   const reviewItems = paginationData.items;

   useEffect(() => {
      if (slug) {
         getReviewByProductModelSlugAsync(slug as string, {
            _page: _page ?? 1,
            _limit: _limit ?? 5,
            _sortOrder: 'DESC',
         });
      }
   }, [slug, _page, _limit, getReviewByProductModelSlugAsync]);

   const averageRating = useMemo(() => {
      if (!reviewItems || reviewItems.length === 0) {
         return 0;
      }
      const sum = reviewItems.reduce((acc, review) => acc + review.rating, 0);
      return sum / reviewItems.length;
   }, [reviewItems]);

   const handlePageChange = (page: number) => {
      if (page !== currentPage && page >= 1 && page <= totalPages) {
         if (page === 1) {
            setPage(null);
         } else {
            setPage(page);
         }
         //  window.scrollTo({ top: 0, behavior: 'smooth' });
      }
   };

   const handlePageSizeChange = (size: string) => {
      setLimit(Number(size));
      setPage(1);
   };

   const hasReviews = reviewItems.length > 0;

   return (
      <div className="mt-16">
         {/* Header Section */}
         <div className="mb-8">
            <h2 className="text-3xl font-bold tracking-tight text-gray-900 dark:text-gray-100 mb-4">
               Customer Reviews
            </h2>

            {hasReviews && (
               <div className="flex items-center gap-4">
                  <div className="flex items-center gap-2">
                     <RatingStar rating={averageRating} size="lg" />
                     <span className="text-2xl font-semibold">
                        {averageRating.toFixed(1)}
                     </span>
                  </div>
                  <Separator orientation="vertical" className="h-6" />
                  <span className="text-sm text-gray-600 dark:text-gray-400">
                     {totalRecords} {totalRecords === 1 ? 'review' : 'reviews'}
                  </span>
               </div>
            )}
         </div>

         {/* Reviews List */}
         {isLoading && !hasReviews ? (
            <div className="space-y-6">
               {[1, 2, 3].map((i) => (
                  <div
                     key={i}
                     className="animate-pulse bg-gray-100 dark:bg-gray-800 rounded-lg p-6 h-32"
                  />
               ))}
            </div>
         ) : hasReviews ? (
            <>
               <div className="space-y-6 mb-8">
                  {reviewItems.map((item) => {
                     return <ReviewItem key={item.id} review={item} />;
                  })}
               </div>

               {/* Pagination */}
               {totalPages > 0 && (
                  <div className="mt-8 flex flex-col sm:flex-row items-center justify-between gap-4 pb-6">
                     {/* Page Info & Size Selector */}
                     <div className="flex items-center gap-4">
                        <Select
                           value={limitSelectValue}
                           onValueChange={handlePageSizeChange}
                        >
                           <SelectTrigger className="w-auto h-9">
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
                           <span className="font-semibold">{totalRecords}</span>{' '}
                           reviews
                        </div>
                     </div>

                     {/* Pagination Controls */}
                     <div className="flex items-center gap-2">
                        {paginationItems.map((item, index) => {
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
                                 variant={isCurrentPage ? 'default' : 'outline'}
                                 size="icon"
                                 className={cn(
                                    'h-9 w-9',
                                    isCurrentPage &&
                                       'bg-black text-white hover:bg-black/90',
                                 )}
                                 disabled={item.disabled || item.value === null}
                                 onClick={() => {
                                    if (item.value !== null && !item.disabled) {
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
                        })}
                     </div>
                  </div>
               )}
            </>
         ) : (
            <div className="text-center py-12 border border-dashed border-gray-300 dark:border-gray-700 rounded-lg">
               <p className="text-gray-500 dark:text-gray-400 text-lg">
                  No reviews yet. Be the first to review this product!
               </p>
            </div>
         )}
      </div>
   );
};

export default ReviewsSection;
