/* eslint-disable react/no-unescaped-entities */
'use client';

import { useState, useEffect, useMemo, useCallback } from 'react';
import { Button } from '~/components/ui/button';
import { Badge } from '~/components/ui/badge';
import { Separator } from '~/components/ui/separator';
import {
   ArrowLeft,
   Package,
   PackageCheck,
   Truck,
   CheckCircle,
   AlertCircle,
   Check,
   X,
} from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useParams, useRouter } from 'next/navigation';
import { cn } from '~/infrastructure/lib/utils';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
} from '~/components/ui/dialog';
import { ReviewModal } from '../_components/review-model';
import { Skeleton } from '~/components/ui/skeleton';
import { EOrderStatus } from '~/domain/enums/order-status.enum';
import NextImage from 'next/image';
import useOrderingService from '~/hooks/api/use-ordering-service';
import { LoadingOverlay } from '~/components/client/loading-overlay';
import useReviewService from '~/hooks/api/use-review-service';
import RatingStar from '~/components/ui/rating-star';
import { TOrderItem } from '~/domain/types/ordering.type';
import { TReviewInOrderItem } from '~/domain/types/catalog.type';

const containerVariants = {
   hidden: { opacity: 0 },
   visible: {
      opacity: 1,
      transition: {
         staggerChildren: 0.05,
      },
   },
};

const itemVariants = {
   hidden: { opacity: 0, y: 20 },
   visible: {
      opacity: 1,
      y: 0,
      transition: {
         type: 'spring',
         stiffness: 300,
         damping: 24,
      },
   },
};

// Get status icon
const getStatusIcon = (status: string) => {
   switch (status) {
      case 'PENDING':
         return <Package className="h-5 w-5 text-yellow-500" />;
      case 'PENDING_ASSIGNMENT':
         return <Package className="h-5 w-5 text-yellow-500" />;
      case 'CONFIRMED':
         return <PackageCheck className="h-5 w-5 text-green-600" />;
      case 'PAID':
         return <CheckCircle className="h-5 w-5 text-blue-500" />;
      case 'DELIVERED':
         return <Truck className="h-5 w-5 text-green-800" />;
      case 'CANCELLED':
         return <AlertCircle className="h-5 w-5 text-red-500" />;
      default:
         return <AlertCircle className="h-5 w-5 text-red-500" />;
   }
};

// Get status color
const getStatusColor = (status: string) => {
   switch (status) {
      case 'PENDING':
         return 'bg-yellow-100 text-yellow-800';
      case 'PENDING_ASSIGNMENT':
         return 'bg-yellow-100 text-yellow-800';
      case 'CONFIRMED':
         return 'bg-green-100 text-green-600';
      case 'PAID':
         return 'bg-blue-100 text-blue-800';
      case 'DELIVERED':
         return 'bg-green-100 text-green-800';
      case 'CANCELLED':
         return 'bg-red-100 text-red-800';
      default:
         return 'bg-gray-100 text-gray-800';
   }
};

const getHoverStatusColor = (status: string) => {
   switch (status) {
      case 'PENDING':
         return 'bg-yellow-100 text-yellow-800 hover:bg-yellow-200 hover:text-yellow-900';
      case 'PENDING_ASSIGNMENT':
         return 'bg-yellow-100 text-yellow-800 hover:bg-yellow-200 hover:text-yellow-900';
      case 'CONFIRMED':
         return 'bg-green-100 text-green-800 hover:bg-green-200 hover:text-green-900';
      case 'PAID':
         return 'bg-blue-100 text-blue-800 hover:bg-blue-200 hover:text-blue-900';
      case 'DELIVERED':
         return 'bg-green-100 text-green-800 hover:bg-green-200 hover:text-green-900';
      case 'CANCELLED':
         return 'bg-red-100 text-red-800 hover:bg-red-200 hover:text-red-900';
      default:
         return 'bg-gray-100 text-gray-800 hover:bg-gray-200 hover:text-gray-900';
   }
};

export default function OrderDetails() {
   const [reviewsInOrder, setReviewsInOrder] = useState<TReviewInOrderItem[]>(
      [],
   );
   const [reviewItem, setReviewItem] = useState<TOrderItem | null>(null);
   const [reviewModalOpen, setReviewModalOpen] = useState(false);
   const [loadingScreen, setLoadingScreen] = useState(false);
   const [timeLeft, setTimeLeft] = useState(0);
   const router = useRouter();
   const params = useParams<{ id: string }>();

   const { getReviewByOrderIdAsync, isLoading: isLoadingReview } =
      useReviewService();

   const {
      getOrderDetailsAsync,
      confirmOrderAsync,
      cancelOrderAsync,
      getOrderDetailsState,
      isLoading: isLoadingOrdering,
   } = useOrderingService();

   const isLoading = useMemo(() => {
      return isLoadingOrdering || isLoadingReview;
   }, [isLoadingOrdering, isLoadingReview]);

   useEffect(() => {
      const fetchOrderDetails = async () => {
         await getOrderDetailsAsync(params.id);
      };

      const fetchReviews = async () => {
         const result = await getReviewByOrderIdAsync(params.id);
         if (result.isSuccess && result.data) {
            setReviewsInOrder(result.data);
         }
      };

      if (params.id) {
         fetchOrderDetails();
         fetchReviews();
      }
   }, [params.id, getOrderDetailsAsync, getReviewByOrderIdAsync]);

   // Helper function to get review for an order item
   const getReviewForItem = useCallback(
      (orderItemId: string): TReviewInOrderItem | undefined => {
         return reviewsInOrder.find(
            (review) => review.order_info.order_item_id === orderItemId,
         );
      },
      [reviewsInOrder],
   );

   // Calculate initial time left based on created_at
   useEffect(() => {
      if (
         getOrderDetailsState.isSuccess &&
         getOrderDetailsState.data?.created_at
      ) {
         const AUTO_CONFIRM_DURATION = 30 * 60; // 30 minutes in seconds
         const createdAt = new Date(getOrderDetailsState.data.created_at);
         const now = new Date();
         const elapsedSeconds = Math.floor(
            (now.getTime() - createdAt.getTime()) / 1000,
         );
         const remainingSeconds = Math.max(
            0,
            AUTO_CONFIRM_DURATION - elapsedSeconds,
         );
         setTimeLeft(remainingSeconds);
      }
   }, [getOrderDetailsState]);

   const CountdownTimer = () => {
      useEffect(() => {
         const intervalId = setInterval(() => {
            setTimeLeft((prevTime) => {
               const newTime = Math.max(0, prevTime - 1);
               if (
                  newTime === 0 &&
                  getOrderDetailsState.isSuccess &&
                  getOrderDetailsState.data?.status === EOrderStatus.PENDING
               ) {
                  confirmOrderAsync(getOrderDetailsState.data.order_id);
                  window.location.reload();
               }
               return newTime;
            });
         }, 1000);

         return () => clearInterval(intervalId);
         // eslint-disable-next-line react-hooks/exhaustive-deps
      }, []);

      const minutes = Math.floor(timeLeft / 60);
      const seconds = timeLeft % 60;

      return (
         <p className="text-sm text-gray-500">
            Order will be confirmed in{' '}
            <span className="font-medium">
               {minutes}:{seconds < 10 ? `0${seconds}` : seconds}
            </span>
         </p>
      );
   };

   const handleCancelOrder = async () => {
      if (
         getOrderDetailsState.isSuccess &&
         getOrderDetailsState.data?.order_id
      ) {
         await cancelOrderAsync(getOrderDetailsState.data.order_id);
         setLoadingScreen(true);
         setTimeout(() => {
            window.location.reload();
         }, 300);
      }
   };

   const handleConfirmOrder = async () => {
      if (
         getOrderDetailsState.isSuccess &&
         getOrderDetailsState.data?.order_id
      ) {
         await confirmOrderAsync(getOrderDetailsState.data.order_id);
         setLoadingScreen(true);
         setTimeout(() => {
            window.location.reload();
         }, 300);
      }
   };

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         variants={containerVariants}
         initial="hidden"
         animate="visible"
      >
         <LoadingOverlay isLoading={loadingScreen} fullScreen={true} />
         <div className="flex items-center px-6 py-4 border-b border-gray-200">
            <motion.button
               onClick={() => {
                  router.push('/account/orders');
                  router.refresh();
               }}
               className="mr-3 text-gray-500 hover:text-gray-700 transition-colors"
               variants={itemVariants}
               whileHover={{ scale: 1.05 }}
               whileTap={{ scale: 0.95 }}
            >
               <ArrowLeft className="h-5 w-5" />
            </motion.button>
            <motion.h2
               className="text-lg font-medium text-gray-900 flex-1"
               variants={itemVariants}
            >
               Order Details
            </motion.h2>
            <motion.div variants={itemVariants}>
               <Button
                  variant="outline"
                  size="sm"
                  className="text-blue-600 hover:text-blue-800"
               ></Button>
            </motion.div>
         </div>

         {isLoading && (
            <div className="px-6 py-8 flex flex-col gap-4">
               <div className="flex gap-4">
                  <div className="w-[60%]">
                     <p className="text-sm font-medium text-gray-900 mb-2">
                        Order Information
                     </p>

                     <Skeleton className="h-[200px] w-full" />
                  </div>

                  <div className="flex flex-col gap-4 w-[40%]">
                     <div>
                        <p className="text-sm font-medium text-gray-900 mb-2">
                           Shipping Address
                        </p>

                        <Skeleton className="h-[50px] w-full" />
                     </div>

                     <div>
                        <p className="text-sm font-medium text-gray-900 mb-2">
                           Payment Method
                        </p>

                        <Skeleton className="h-[50px] w-full" />
                     </div>
                  </div>
               </div>

               <div className="flex flex-col gap-2">
                  <p className="text-sm font-medium text-gray-900 mb-2 mt-5">
                     Order items
                  </p>

                  <Skeleton className="h-[40px] w-full" />
                  <Skeleton className="h-[40px] w-full" />
                  <Skeleton className="h-[40px] w-full" />
                  <Skeleton className="h-[40px] w-full" />
               </div>

               <div>
                  <p className="text-sm font-medium text-gray-900 mb-2 mt-5">
                     Order summary
                  </p>

                  <Skeleton className="h-[200px] w-full" />
               </div>
            </div>
         )}

         {!isLoading &&
            getOrderDetailsState.isSuccess &&
            getOrderDetailsState.data && (
               <motion.div
                  className="px-6 py-8 flex flex-col gap-4"
                  variants={itemVariants}
               >
                  <div className="flex flex-row gap-10">
                     <div className="w-[60%]">
                        <h3 className="text-sm font-medium text-gray-900 mb-2">
                           Order Information
                        </h3>
                        <div className="bg-gray-50 rounded-lg p-4 space-y-3">
                           <div className="flex justify-between">
                              <span className="text-sm text-gray-500">
                                 Tracking Number
                              </span>
                              <span className="text-sm font-medium text-blue-600 hover:underline cursor-pointer">
                                 {getOrderDetailsState.data.order_code}
                              </span>
                           </div>
                           <div className="flex justify-between">
                              <span className="text-sm text-gray-500">
                                 Date Placed
                              </span>
                              <span className="text-sm font-medium">
                                 {new Date(
                                    getOrderDetailsState.data.created_at,
                                 ).toLocaleDateString('en-US', {
                                    year: 'numeric',
                                    month: 'long',
                                    day: 'numeric',
                                 })}
                              </span>
                           </div>
                           <div className="flex justify-between">
                              <span className="text-sm text-gray-500">
                                 Status
                              </span>
                              <Badge
                                 className={cn(
                                    'capitalize flex items-center select-none gap-1',
                                    getStatusColor(
                                       getOrderDetailsState.data.status ===
                                          (EOrderStatus.PENDING_ASSIGNMENT ||
                                             EOrderStatus.CONFIRMED)
                                          ? EOrderStatus.CONFIRMED
                                          : getOrderDetailsState.data.status,
                                    ),
                                    getHoverStatusColor(
                                       getOrderDetailsState.data.status ===
                                          (EOrderStatus.PENDING_ASSIGNMENT ||
                                             EOrderStatus.CONFIRMED)
                                          ? EOrderStatus.CONFIRMED
                                          : getOrderDetailsState.data.status,
                                    ),
                                 )}
                              >
                                 {getStatusIcon(
                                    getOrderDetailsState.data.status ===
                                       (EOrderStatus.PENDING_ASSIGNMENT ||
                                          EOrderStatus.CONFIRMED)
                                       ? EOrderStatus.CONFIRMED
                                       : getOrderDetailsState.data.status,
                                 )}
                                 {getOrderDetailsState.data.status ===
                                 (EOrderStatus.PENDING_ASSIGNMENT ||
                                    EOrderStatus.CONFIRMED)
                                    ? EOrderStatus.CONFIRMED
                                    : getOrderDetailsState.data.status}
                              </Badge>
                           </div>

                           <div className="flex justify-between">
                              <span className="text-sm text-gray-500">
                                 Delivery
                              </span>
                              <span className="text-sm font-medium">
                                 Expected to ship in 2-3 business days
                              </span>
                           </div>
                        </div>
                     </div>

                     <div className="w-[40%]">
                        <h3 className="text-sm font-medium text-gray-900 mb-2">
                           Shipping Address
                        </h3>
                        <div className="bg-gray-50 rounded-lg p-4 flex flex-col gap-2">
                           <p className="text-sm font-medium">
                              {
                                 getOrderDetailsState.data.shipping_address
                                    .contact_name
                              }
                           </p>
                           <p className="text-sm text-gray-500">
                              {
                                 getOrderDetailsState.data.shipping_address
                                    .contact_address_line
                              }
                           </p>
                           <p className="text-sm text-gray-500">
                              {
                                 getOrderDetailsState.data.shipping_address
                                    .contact_district
                              }
                              ,{' '}
                              {
                                 getOrderDetailsState.data.shipping_address
                                    .contact_province
                              }{' '}
                           </p>
                           <p className="text-sm text-gray-500">
                              {
                                 getOrderDetailsState.data.shipping_address
                                    .contact_country
                              }
                           </p>
                        </div>

                        <h3 className="text-sm font-medium text-gray-900 mt-4 mb-2">
                           Payment Method
                        </h3>
                        <div className="bg-gray-50 rounded-lg p-4">
                           <p className="text-sm font-medium">
                              {getOrderDetailsState.data.payment_method}
                           </p>
                        </div>
                     </div>
                  </div>

                  <motion.div variants={itemVariants} className="mb-6">
                     <h3 className="text-sm font-medium text-gray-900 mb-2">
                        Order Items
                     </h3>
                     <div className="border rounded-lg overflow-hidden">
                        <div className="divide-y divide-gray-200">
                           <AnimatePresence>
                              {getOrderDetailsState.data.order_items.map(
                                 (item, index) => (
                                    <motion.div
                                       key={index}
                                       className="p-4 flex items-center"
                                       variants={itemVariants}
                                       initial="hidden"
                                       animate="visible"
                                       custom={index}
                                    >
                                       {/* image */}
                                       <div className="w-[100px] overflow-hidden relative h-[100px] rounded-lg">
                                          <NextImage
                                             src={
                                                item.display_image_url ||
                                                '/placeholder.svg'
                                             }
                                             alt={
                                                item.model_name +
                                                ' ' +
                                                item.color_name +
                                                ' ' +
                                                item.storage_name
                                             }
                                             width={Math.round((500 * 16) / 9)}
                                             height={500}
                                             className="absolute top-0 left-0 w-full h-full object-cover"
                                          />
                                       </div>
                                       <div className="ml-4 flex-1">
                                          <h4 className="text-sm font-medium text-gray-900">
                                             {item.model_name +
                                                ' ' +
                                                item.color_name +
                                                ' ' +
                                                item.storage_name}
                                          </h4>
                                          <p className="mt-1 text-xs text-gray-500">
                                             {item.color_name}
                                          </p>
                                          <div className="mt-1 flex justify-between">
                                             <p className="text-sm text-gray-500">
                                                Qty x{item.quantity}
                                             </p>
                                             <p className="text-sm font-medium text-gray-900">
                                                $
                                                {(
                                                   item.sub_total_amount ?? 0
                                                ).toFixed(2)}
                                             </p>
                                          </div>

                                          {/* Review Display */}
                                          {item.is_reviewed && (
                                             <div className="mt-3 pt-3 border-t border-gray-200">
                                                {(() => {
                                                   const review =
                                                      getReviewForItem(
                                                         item.order_item_id,
                                                      );
                                                   if (!review) return null;
                                                   return (
                                                      <div className="space-y-2">
                                                         <div className="flex items-center gap-2">
                                                            <RatingStar
                                                               rating={
                                                                  review.rating
                                                               }
                                                               size="sm"
                                                            />
                                                            <span className="text-xs text-gray-600">
                                                               {review.rating}.0
                                                            </span>
                                                         </div>
                                                         {review.content && (
                                                            <p className="text-xs text-gray-600 line-clamp-2">
                                                               {review.content}
                                                            </p>
                                                         )}
                                                      </div>
                                                   );
                                                })()}
                                             </div>
                                          )}

                                          {/* Action Buttons */}
                                          {getOrderDetailsState.data.status ===
                                             EOrderStatus.DELIVERED && (
                                             <div className="mt-2 flex justify-end">
                                                {!item.is_reviewed ? (
                                                   <Button
                                                      variant="ghost"
                                                      size="sm"
                                                      className="text-blue-600 hover:text-blue-800 text-xs"
                                                      onClick={(e) => {
                                                         e.stopPropagation();
                                                         setReviewItem(item);
                                                         setReviewModalOpen(
                                                            true,
                                                         );
                                                      }}
                                                   >
                                                      Write review
                                                   </Button>
                                                ) : (
                                                   <Button
                                                      variant="ghost"
                                                      size="sm"
                                                      className="text-blue-600 hover:text-blue-800 text-xs"
                                                      onClick={(e) => {
                                                         e.stopPropagation();
                                                         setReviewItem(item);
                                                         setReviewModalOpen(
                                                            true,
                                                         );
                                                      }}
                                                   >
                                                      Update review
                                                   </Button>
                                                )}
                                             </div>
                                          )}
                                       </div>
                                    </motion.div>
                                 ),
                              )}
                           </AnimatePresence>
                        </div>
                     </div>
                  </motion.div>

                  <motion.div variants={itemVariants} className="mb-6">
                     <h3 className="text-sm font-medium text-gray-900 mb-2">
                        Order Summary
                     </h3>
                     <div className="bg-gray-50 rounded-lg p-4 space-y-3">
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">
                              Subtotal
                           </span>
                           <span className="text-sm font-medium">
                              $
                              {getOrderDetailsState.data.total_amount.toFixed(
                                 2,
                              )}
                           </span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">
                              Shipping
                           </span>
                           <span className="text-sm font-medium">$0.00</span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">
                              Discount
                           </span>
                           <span className="text-sm font-medium">
                              $
                              {(
                                 getOrderDetailsState.data?.discount_amount ?? 0
                              ).toFixed(2)}
                           </span>
                        </div>
                        <Separator />
                        <div className="flex justify-between">
                           <span className="text-sm font-medium">Total</span>
                           <span className="text-sm font-medium">
                              $
                              {getOrderDetailsState.data.total_amount.toFixed(
                                 2,
                              )}
                           </span>
                        </div>
                     </div>
                  </motion.div>

                  <motion.div
                     variants={itemVariants}
                     className="flex justify-between items-center pt-4 border-t border-gray-200"
                  >
                     <Button
                        variant="outline"
                        className="text-gray-600"
                        onClick={() => {
                           window.history.back();
                        }}
                     >
                        <ArrowLeft className="mr-2 h-4 w-4" />
                        Back to Orders
                     </Button>
                     {getOrderDetailsState.data.status ===
                        EOrderStatus.PENDING && (
                        <div className="flex items-center gap-3">
                           <CountdownTimer />

                           <Button
                              className="bg-red-600 hover:bg-red-700 text-white"
                              onClick={() => {
                                 handleCancelOrder();
                              }}
                           >
                              <X className="mr-2 h-4 w-4" />
                              Cancel Order
                           </Button>

                           <Button
                              className="bg-blue-600 hover:bg-blue-700 text-white"
                              onClick={() => {
                                 handleConfirmOrder();
                              }}
                           >
                              <Check className="mr-2 h-4 w-4" />
                              Confirm Order
                           </Button>
                        </div>
                     )}
                  </motion.div>
               </motion.div>
            )}

         {/* Review Modal */}
         <Dialog open={reviewModalOpen} onOpenChange={setReviewModalOpen}>
            <DialogContent className="sm:max-w-[500px]">
               <DialogHeader>
                  <DialogTitle>
                     {reviewItem?.is_reviewed
                        ? 'Update review'
                        : 'Write review'}
                  </DialogTitle>
               </DialogHeader>
               {reviewItem && (
                  <ReviewModal
                     item={{
                        product_id: reviewItem.model_id, // Using model_id as product_id
                        model_id: reviewItem.model_id,
                        order_id: getOrderDetailsState.data?.order_id || '',
                        order_item_id: reviewItem.order_item_id,
                        sku_id: reviewItem.sku_id || null,
                        name:
                           reviewItem.model_name +
                           ' ' +
                           reviewItem.color_name +
                           ' ' +
                           reviewItem.storage_name,
                        image:
                           reviewItem.display_image_url || '/placeholder.svg',
                        options:
                           reviewItem.color_name +
                           ', ' +
                           reviewItem.storage_name,
                        isReviewed: reviewItem.is_reviewed,
                     }}
                     reviewedData={{
                        reviewId: reviewsInOrder.find(
                           (review) =>
                              review.order_info.order_item_id ===
                              reviewItem.order_item_id,
                        )?.id,
                        rating: reviewsInOrder.find(
                           (review) =>
                              review.order_info.order_item_id ===
                              reviewItem.order_item_id,
                        )?.rating,
                        content: reviewsInOrder.find(
                           (review) =>
                              review.order_info.order_item_id ===
                              reviewItem.order_item_id,
                        )?.content,
                     }}
                     onClose={() => {
                        setReviewModalOpen(false);
                        setReviewItem(null);
                     }}
                     onSubmit={async () => {
                        // Refresh reviews after create/update/delete
                        if (params.id) {
                           const result = await getReviewByOrderIdAsync(
                              params.id,
                           );
                           if (result.isSuccess && result.data) {
                              setReviewsInOrder(result.data);
                           }
                        }
                        // Refresh order details to update is_reviewed status
                        if (params.id) {
                           await getOrderDetailsAsync(params.id);
                        }
                     }}
                  />
               )}
            </DialogContent>
         </Dialog>
      </motion.div>
   );
}
