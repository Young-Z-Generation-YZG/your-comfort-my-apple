'use client';

import { useEffect, useMemo } from 'react';
import { useParams } from 'next/navigation';
import {
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
} from 'lucide-react';
import useReviewService from '@components/hooks/api/use-review-service';
import usePagination from '@components/hooks/use-pagination';
import useFilter from '../../../../shop/_hooks/use-filter';
import ReviewItem from './review-item';
import { Button } from '@components/ui/button';
import { Separator } from '@components/ui/separator';
import { cn } from '~/infrastructure/lib/utils';
import RatingStar from '@components/ui/rating-star';

type ReviewFilters = {
   _page: number;
};

const ReviewsSection = () => {
   const { slug } = useParams();
   const { filters, setFilters, removeFilter } = useFilter<ReviewFilters>();
   const {
      getReviewByProductModelSlugAsync,
      getReviewByProductModelSlugState,
      isLoading,
   } = useReviewService();

   const page = filters._page || 1;
   const limit = 5;

   useEffect(() => {
      if (slug && page) {
         getReviewByProductModelSlugAsync(slug as string, {
            page,
            limit,
            sortOrder: 'desc',
         });
      }
   }, [slug, page, limit, getReviewByProductModelSlugAsync]);

   const {
      currentPage,
      totalPages,
      pageSize,
      totalRecords,
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,
      paginationItems,
      getPageNumbers,
   } = usePagination(
      getReviewByProductModelSlugState.data &&
         getReviewByProductModelSlugState.data.items.length > 0
         ? getReviewByProductModelSlugState.data
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

   const averageRating = useMemo(() => {
      if (
         !getReviewByProductModelSlugState.data ||
         getReviewByProductModelSlugState.data.items.length === 0
      ) {
         return 0;
      }
      const sum = getReviewByProductModelSlugState.data.items.reduce(
         (acc, review) => acc + review.rating,
         0,
      );
      return sum / getReviewByProductModelSlugState.data.items.length;
   }, [getReviewByProductModelSlugState.data]);

   const handlePageChange = (page: number) => {
      if (page !== currentPage && page >= 1 && page <= totalPages) {
         // Only add _page to URL if it's not page 1, remove it if it's page 1
         if (page === 1) {
            removeFilter('_page'); // Remove _page from URL when going to page 1
         } else {
            setFilters({ _page: page });
         }
         window.scrollTo({ top: 0, behavior: 'smooth' });
      }
   };

   const hasReviews = paginationItems.length > 0;

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
                  {paginationItems.map((item) => {
                     return <ReviewItem key={item.id} review={item} />;
                  })}
               </div>

               {/* Pagination */}
               {totalPages > 0 && (
                  <div className="mt-8 flex flex-col sm:flex-row items-center justify-between gap-4 pb-6">
                     {/* Page Info */}
                     <div className="flex items-center gap-4">
                        <div className="text-sm text-gray-600">
                           Showing{' '}
                           <span className="font-semibold">
                              {(currentPage - 1) * pageSize + 1}
                           </span>{' '}
                           to{' '}
                           <span className="font-semibold">
                              {Math.min(currentPage * pageSize, totalRecords)}
                           </span>{' '}
                           of{' '}
                           <span className="font-semibold">{totalRecords}</span>{' '}
                           results
                        </div>
                     </div>

                     {/* Pagination Controls */}
                     <div className="flex items-center gap-2">
                        {/* First Page */}
                        <Button
                           variant="outline"
                           size="icon"
                           className="h-9 w-9"
                           onClick={() => handlePageChange(1)}
                           disabled={isFirstPage}
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
                        >
                           <ChevronLeft className="h-4 w-4" />
                        </Button>

                        {/* Page Numbers */}
                        <div className="flex items-center gap-1">
                           {getPageNumbers().map((page, index) => {
                              const pageNum = page as number | string;
                              if (pageNum === '...') {
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
                                    key={`page-${pageNum}`}
                                    variant={
                                       currentPage === pageNum
                                          ? 'default'
                                          : 'outline'
                                    }
                                    size="icon"
                                    className={cn(
                                       'h-9 w-9',
                                       currentPage === pageNum &&
                                          'bg-black text-white hover:bg-black/90',
                                    )}
                                    onClick={() =>
                                       handlePageChange(pageNum as number)
                                    }
                                 >
                                    {pageNum}
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
                        >
                           <ChevronsRight className="h-4 w-4" />
                        </Button>
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
