'use client';

import { useEffect, useMemo, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { Button } from '@components/ui/button';
import {
   ArrowLeft,
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
   MessageSquare,
   Smartphone,
} from 'lucide-react';
import { LoadingOverlay } from '@components/loading-overlay';
import Image from 'next/image';
import RatingStar from '@components/ui/rating-star';
import { Badge } from '@components/ui/badge';
import { Separator } from '@components/ui/separator';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { cn } from '~/src/infrastructure/lib/utils';
import {
   TIphoneModelDetails,
   TReviewItem,
} from '~/src/domain/types/catalog.type';
import useProductService from '~/src/hooks/api/use-product-service';
import useReviewService from '~/src/hooks/api/use-review-service';
import usePaginationV2 from '~/src/hooks/use-pagination';

const IphoneModelDetailPage = () => {
   const params = useParams();
   const router = useRouter();
   const slug = params?.slug as string;
   const [reviewPage, setReviewPage] = useState(1);
   const [reviewPageSize, setReviewPageSize] = useState(10);

   const {
      isLoading: productLoading,
      getProductModelBySlugAsync,
      getProductModelBySlugState,
   } = useProductService();

   const {
      isLoading: reviewsLoading,
      getReviewsByProductModelIdAsync,
      getReviewsState,
   } = useReviewService();

   useEffect(() => {
      if (slug) {
         getProductModelBySlugAsync(slug);
      }
   }, [slug, getProductModelBySlugAsync]);

   useEffect(() => {
      const model =
         getProductModelBySlugState.data as TIphoneModelDetails | null;

      if (model?.id) {
         getReviewsByProductModelIdAsync(model.id, {
            _page: reviewPage,
            _limit: reviewPageSize,
            _sortOrder: 'DESC',
         });
      }
   }, [
      getProductModelBySlugState.data,
      getReviewsByProductModelIdAsync,
      reviewPage,
      reviewPageSize,
   ]);

   const detail: TIphoneModelDetails | null = useMemo(() => {
      if (getProductModelBySlugState.data) {
         return getProductModelBySlugState.data as TIphoneModelDetails;
      }
      return null;
   }, [getProductModelBySlugState.data]);

   const reviews: TReviewItem[] = useMemo(() => {
      if (getReviewsState.data) {
         return getReviewsState.data.items as TReviewItem[];
      }
      return [];
   }, [getReviewsState.data]);

   const {
      getPaginationItems: getReviewPaginationItems,
      currentPage: reviewCurrentPage,
      totalRecords: reviewTotalRecords,
      totalPages: reviewTotalPages,
      firstItemIndex: reviewFirstItemIndex,
      lastItemIndex: reviewLastItemIndex,
      limitSelectValue: reviewLimitSelectValue,
   } = usePaginationV2(
      getReviewsState.data ?? {
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
         pageSizeOverride: reviewPageSize,
         currentPageOverride: reviewPage,
         fallbackPageSize: 10,
      },
   );

   const renderPriceRange = (model: TIphoneModelDetails) => {
      const prices = model.sku_prices.map((p) => p.unit_price);
      if (!prices.length) return null;
      const min = Math.min(...prices);
      const max = Math.max(...prices);
      return min === max ? (
         <span className="text-2xl font-semibold text-gray-900">
            ${min.toFixed(0)}
         </span>
      ) : (
         <span className="text-2xl font-semibold text-gray-900">
            ${min.toFixed(0)} - ${max.toFixed(0)}
         </span>
      );
   };

   const renderContent = () => {
      if (getProductModelBySlugState.isError) {
         return (
            <div className="flex h-full items-center justify-center rounded-xl border bg-white p-10 text-center">
               <div className="flex flex-col items-center gap-3">
                  <div className="flex h-16 w-16 items-center justify-center rounded-full bg-destructive/10">
                     <Smartphone className="h-8 w-8 text-destructive" />
                  </div>
                  <p className="text-lg font-semibold text-destructive">
                     Failed to load product details
                  </p>
                  <p className="text-sm text-muted-foreground">
                     Please try again later.
                  </p>
               </div>
            </div>
         );
      }

      if (!detail) return null;

      return (
         <div className="flex flex-col gap-6">
            <div className="flex flex-col gap-4 rounded-xl border bg-white p-6 shadow-sm md:flex-row md:gap-8">
               {detail.showcase_images?.[0] && (
                  <div className="relative h-[320px] w-full max-w-md overflow-hidden rounded-lg bg-muted">
                     <Image
                        src={detail.showcase_images[0].image_url}
                        alt={detail.showcase_images[0].image_name}
                        fill
                        className="object-cover"
                        unoptimized
                     />
                  </div>
               )}

               <div className="flex flex-1 flex-col gap-4">
                  <div className="flex items-start justify-between gap-3">
                     <div className="flex flex-col gap-2">
                        <h1 className="text-3xl font-bold leading-tight">
                           {detail.name}
                        </h1>
                        <div className="flex flex-wrap items-center gap-2">
                           <Badge variant="outline">
                              {detail.category?.name ?? 'iPhone'}
                           </Badge>
                           <Badge variant="secondary">
                              {detail.average_rating.rating_average_value.toFixed(
                                 1,
                              )}{' '}
                              ({detail.average_rating.rating_count} reviews)
                           </Badge>
                           <Badge variant="outline">
                              {detail.overall_sold.toLocaleString()} sold
                           </Badge>
                        </div>
                     </div>
                     <div className="text-right">
                        {renderPriceRange(detail)}
                     </div>
                  </div>

                  <Separator />

                  <div className="flex flex-col gap-3">
                     <h3 className="text-lg font-semibold">Colors</h3>
                     <div className="flex flex-wrap gap-2">
                        {detail.color_items.map((color, idx) => (
                           <div
                              key={idx}
                              className={cn(
                                 'h-9 w-9 rounded-full border-2 shadow-color-selector',
                              )}
                              style={{ backgroundColor: color.hex_code }}
                              title={color.name}
                           />
                        ))}
                     </div>
                  </div>

                  <div className="flex flex-col gap-3">
                     <h3 className="text-lg font-semibold">Storages</h3>
                     <div className="flex flex-wrap gap-2">
                        {detail.storage_items.map((storage, idx) => (
                           <span
                              key={idx}
                              className="min-w-[72px] rounded-full border px-3 py-1 text-center text-xs font-medium uppercase"
                           >
                              {storage.name}
                           </span>
                        ))}
                     </div>
                  </div>

                  <div className="flex flex-col gap-3">
                     <h3 className="text-lg font-semibold">Description</h3>
                     <p className="text-sm text-muted-foreground">
                        {detail.description || 'No description provided.'}
                     </p>
                  </div>
               </div>
            </div>

            <div className="rounded-xl border bg-white p-6 shadow-sm">
               <div className="flex items-center gap-3">
                  <RatingStar
                     rating={detail.average_rating.rating_average_value}
                     size={24}
                  />
                  <div className="flex flex-col">
                     <span className="text-lg font-semibold">
                        {detail.average_rating.rating_average_value.toFixed(1)}{' '}
                        / 5
                     </span>
                     <span className="text-sm text-muted-foreground">
                        {detail.average_rating.rating_count} reviews
                     </span>
                  </div>
               </div>

               {detail.rating_stars?.length ? (
                  <div className="mt-4 flex flex-col gap-2">
                     {detail.rating_stars
                        .slice()
                        .sort((a, b) => b.star - a.star)
                        .map((star, idx) => {
                           const total =
                              detail.average_rating.rating_count || 1;
                           const percent = Math.round(
                              (star.count / total) * 100,
                           );
                           return (
                              <div
                                 key={idx}
                                 className="flex items-center gap-3 text-sm"
                              >
                                 <span className="w-6">{star.star}★</span>
                                 <div className="h-2 flex-1 overflow-hidden rounded-full bg-muted">
                                    <div
                                       className="h-full rounded-full bg-yellow-400"
                                       style={{ width: `${percent}%` }}
                                    />
                                 </div>
                                 <span className="min-w-[40px] text-right">
                                    {star.count}
                                 </span>
                              </div>
                           );
                        })}
                  </div>
               ) : null}
            </div>

            <div className="rounded-xl border bg-white p-6 shadow-sm">
               <div className="mb-4 flex items-center justify-between">
                  <div className="flex items-center gap-2">
                     <MessageSquare className="h-5 w-5 text-primary" />
                     <div>
                        <h3 className="text-lg font-semibold">
                           Customer Reviews
                        </h3>
                        <p className="text-xs text-muted-foreground">
                           Insights from recent customer feedback
                        </p>
                     </div>
                  </div>
                  <span className="text-sm text-muted-foreground">
                     {reviewTotalRecords} reviews (page {reviewCurrentPage} of{' '}
                     {reviewTotalPages || 1})
                  </span>
               </div>

               {getReviewsState.isError ? (
                  <p className="rounded-md bg-destructive/5 px-3 py-2 text-sm text-destructive">
                     Failed to load reviews. Please try again later.
                  </p>
               ) : reviews.length === 0 ? (
                  <div className="flex flex-col items-center justify-center rounded-md border border-dashed px-6 py-10 text-center">
                     <p className="text-sm font-medium">
                        No reviews for this model yet.
                     </p>
                     <p className="mt-1 text-xs text-muted-foreground">
                        Reviews from customers will appear here.
                     </p>
                  </div>
               ) : (
                  <>
                     <div className="mb-4 flex flex-wrap items-center justify-between gap-2 text-sm text-muted-foreground">
                        <div className="flex items-center gap-2">
                           <Select
                              value={reviewLimitSelectValue}
                              onValueChange={(value) => {
                                 setReviewPageSize(Number(value));
                                 setReviewPage(1);
                              }}
                           >
                              <SelectTrigger className="h-8 w-auto">
                                 <SelectValue />
                              </SelectTrigger>
                              <SelectContent>
                                 <SelectGroup>
                                    <SelectItem value="5">5 / page</SelectItem>
                                    <SelectItem value="10">
                                       10 / page
                                    </SelectItem>
                                    <SelectItem value="20">
                                       20 / page
                                    </SelectItem>
                                 </SelectGroup>
                              </SelectContent>
                           </Select>
                           <span>
                              Showing{' '}
                              <span className="font-medium">
                                 {reviewFirstItemIndex}
                              </span>{' '}
                              to{' '}
                              <span className="font-medium">
                                 {reviewLastItemIndex}
                              </span>{' '}
                              of{' '}
                              <span className="font-medium">
                                 {reviewTotalRecords}
                              </span>{' '}
                              reviews
                           </span>
                        </div>

                        {reviewTotalPages > 0 && (
                           <div className="flex items-center gap-2">
                              {getReviewPaginationItems().map((item, index) => {
                                 if (item.type === 'ellipsis') {
                                    return (
                                       <span
                                          key={`ellipsis-${index}`}
                                          className="flex items-center px-2 text-gray-400"
                                       >
                                          <Ellipsis className="h-4 w-4" />
                                       </span>
                                    );
                                 }

                                 const isCurrentPage =
                                    item.type === 'page' &&
                                    item.value === reviewCurrentPage;

                                 return (
                                    <Button
                                       key={`${item.type}-${item.label}-${index}`}
                                       variant={
                                          isCurrentPage ? 'default' : 'outline'
                                       }
                                       size="icon"
                                       className={cn(
                                          'h-8 w-8',
                                          isCurrentPage &&
                                             'bg-black text-white hover:bg-black/90',
                                       )}
                                       disabled={
                                          item.disabled || item.value === null
                                       }
                                       onClick={() => {
                                          if (
                                             item.value !== null &&
                                             !item.disabled
                                          ) {
                                             setReviewPage(item.value);
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
                        )}
                     </div>

                     <div className="grid gap-4 md:grid-cols-2">
                        {reviews.map((review) => (
                           <div
                              key={review.id}
                              className="group relative overflow-hidden rounded-xl border bg-white p-4 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md"
                           >
                              <div className="flex items-start gap-3">
                                 <div className="flex h-10 w-10 flex-shrink-0 items-center justify-center rounded-full bg-primary/10 text-xs font-semibold uppercase text-primary">
                                    {review.customer_review_info.name
                                       .charAt(0)
                                       .toUpperCase()}
                                 </div>
                                 <div className="flex flex-1 flex-col gap-2">
                                    <div className="flex items-start justify-between gap-2">
                                       <div className="flex flex-col">
                                          <span className="text-sm font-semibold">
                                             {review.customer_review_info.name}
                                          </span>
                                          <span className="text-[11px] text-muted-foreground">
                                             Order #{review.order_info.order_id}
                                          </span>
                                       </div>
                                       <div className="flex flex-col items-end gap-1">
                                          <div className="flex items-center gap-1">
                                             <RatingStar
                                                rating={review.rating}
                                                size={14}
                                             />
                                             <span className="rounded-full bg-amber-100 px-2 py-[2px] text-[11px] font-medium text-amber-700">
                                                {review.rating.toFixed(1)} / 5
                                             </span>
                                          </div>
                                       </div>
                                    </div>

                                    <p className="mt-1 text-sm leading-relaxed text-muted-foreground line-clamp-4 group-hover:line-clamp-none">
                                       {review.content}
                                    </p>
                                 </div>
                              </div>

                              <div className="pointer-events-none absolute inset-x-0 bottom-0 h-1 bg-gradient-to-r from-primary/60 via-amber-400 to-primary/60 opacity-0 transition-opacity duration-200 group-hover:opacity-100" />
                           </div>
                        ))}
                     </div>
                  </>
               )}
            </div>
         </div>
      );
   };

   return (
      <div className="flex h-full flex-col gap-4 p-6">
         <LoadingOverlay isLoading={productLoading || reviewsLoading} />
         <div className="flex items-center justify-between">
            <Button
               variant="outline"
               className="rounded-full"
               onClick={() => router.back()}
            >
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>
         </div>

         {renderContent()}
      </div>
   );
};

export default IphoneModelDetailPage;
