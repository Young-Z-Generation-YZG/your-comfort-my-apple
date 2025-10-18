/* eslint-disable react/no-unescaped-entities */
'use client';

import { useState, useEffect, useMemo } from 'react';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import { Separator } from '@components/ui/separator';
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
import { useGetOrderDetailsQuery } from '~/infrastructure/services/order.service';
import { useParams, useRouter } from 'next/navigation';
import {
   OrderDetailsResponse,
   OrderItemResponse,
} from '~/domain/interfaces/orders/order.interface';
import { cn } from '~/infrastructure/lib/utils';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
} from '@components/ui/dialog';

import { ReviewModal } from '../_components/review-model';
import { useGetReviewByOrderIdAsyncQuery } from '~/infrastructure/services/review.service';
import { IReviewByOrderResponse } from '~/domain/interfaces/catalogs/review.interface';
import { ORDER_STATUS_TYPE_ENUM } from '~/domain/enums/order-type.enum';
import { Skeleton } from '@components/ui/skeleton';
import { EOrderStatus } from '~/domain/enums/order-status.enum';
import NextImage from 'next/image';
import useOrderApi from '../_hooks/use-order-api';

const fakeOrderDetails = {
   tenant_id: null,
   branch_id: null,
   order_id: 'f7dbe073-accb-4b87-9ba7-868bde5adf5d',
   customer_id: 'e79d0b6f-af5a-4162-a6fd-8194d5a5f616',
   customer_email: 'staff@gmail.com',
   order_code: '#441259',
   status: 'PENDING_ASSIGNMENT',
   payment_method: 'VNPAY',
   shipping_address: {
      contact_name: 'Foo Bar',
      contact_email: 'staff@gmail.com',
      contact_phone_number: '0333284890',
      contact_address_line: '123 Street',
      contact_district: 'Thu Duc',
      contact_province: 'Ho Chi Minh',
      contact_country: 'Vietnam',
   },
   order_items: [
      {
         order_id: 'f7dbe073-accb-4b87-9ba7-868bde5adf5d',
         order_item_id: '2ac7d0dd-f874-4e44-9fd2-98a643cdf786',
         sku_id: null,
         model_id: '68e403d5617b27ad030bf28f',
         model_name: 'IPHONE_15',
         color_name: 'BLUE',
         storage_name: '128GB',
         unit_price: 1000,
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         model_slug: 'iphone-15',
         quantity: 7,
         promotion: {
            promotion_id_or_code: '175a2367-04c0-4eb5-a3dd-5a509e4bedc8',
            promotion_type: 'COUPON',
            product_unit_price: 1000,
            discount_type: 'PERCENTAGE',
            discount_value: 0.1,
            discount_amount: 0.1,
            final_price: 900,
         },
         is_reviewed: false,
         created_at: '2025-10-15T14:27:01.065529Z',
         updated_at: '2025-10-15T14:27:01.065529Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   total_amount: 900,
   created_at: '2025-10-15T14:27:01.063217Z',
   updated_at: '2025-10-15T14:27:01.063217Z',
   updated_by: null,
   is_deleted: false,
   deleted_at: null,
   deleted_by: null,
};

type TOrderDetailsResponse = typeof fakeOrderDetails;
type TOrderItemResponse = (typeof fakeOrderDetails.order_items)[0];

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
   //  const [reviewsInOrder, setReviewInOrder] = useState<
   //     IReviewByOrderResponse[]
   //  >([]);
   //  const [reviewItem, setReviewItem] = useState<OrderItemResponse | null>(null);
   //  const [reviewModalOpen, setReviewModalOpen] = useState(false);
   const [timeLeft, setTimeLeft] = useState(30 * 60); // 30 minutes in seconds
   const router = useRouter();
   const params = useParams<{ id: string }>();

   const {
      getOrderDetailsAsync,
      confirmOrderAsync,
      cancelOrderAsync,
      orderDetailsState,
      isLoading,
   } = useOrderApi();

   useEffect(() => {
      if (params.id) {
         getOrderDetailsAsync(params.id);
      }
   }, [params.id, getOrderDetailsAsync]);

   const displayOrderDetailsData = useMemo(() => {
      if (orderDetailsState.isSuccess && orderDetailsState.data) {
         return orderDetailsState.data as unknown as TOrderDetailsResponse;
      }
      return null;
   }, [orderDetailsState]);

   //  const {
   //     data: reviewsInOrderDataAsync,
   //     isLoading: isReviewsInOrderLoading,
   //     isError: isReviewsInOrderError,
   //     error: reviewsInOrderError,
   //     isSuccess: isReviewsInOrderSuccess,
   //  } = useGetReviewByOrderIdAsyncQuery(params.id);

   //  const [
   //     confirmOrder,
   //     { isLoading: isConfirmingOrder, isSuccess: isSuccessConfirmOrder },
   //  ] = useConfirmOrderAsyncMutation();
   //  const [
   //     cancelOrder,
   //     { isLoading: isCancellingOrder, isSuccess: isSuccessCancelOrder },
   //  ] = useCancelOrderAsyncMutation();

   const CountdownTimer = () => {
      useEffect(() => {
         if (timeLeft <= 0) {
            window.location.reload();

            return;
         }

         const intervalId = setInterval(() => {
            setTimeLeft((prevTime) => prevTime - 1);
         }, 1000);

         return () => clearInterval(intervalId);
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
      if (displayOrderDetailsData?.order_id) {
         await cancelOrderAsync(displayOrderDetailsData.order_id);
      }
   };

   const handleConfirmOrder = async () => {
      if (displayOrderDetailsData?.order_id) {
         await confirmOrderAsync(displayOrderDetailsData.order_id);
      }
   };

   //  useEffect(() => {
   //     if (isOrderDetailsDataSuccess && orderDetailsData) {
   //        setOrder(orderDetailsData);

   //        const orderCreatedAt = new Date(orderDetailsData.order_created_at);
   //        const currentTime = new Date();
   //        const timeDifference =
   //           currentTime.getTime() - orderCreatedAt.getTime();
   //        const minutesDifference = Math.floor(timeDifference / (1000 * 60));

   //        setTimeLeft((30 - minutesDifference) * 60);
   //     }

   //     if (isReviewsInOrderSuccess && reviewsInOrderDataAsync) {
   //        setReviewInOrder(reviewsInOrderDataAsync);
   //     }

   //     if (isOrderDetailsError && orderDetailsError) {
   //     }

   //     if (isReviewsInOrderError && reviewsInOrderError) {
   //     }
   //  }, [
   //     isOrderDetailsDataSuccess,
   //     isOrderDetailsError,
   //     isReviewsInOrderSuccess,
   //     isReviewsInOrderError,
   //  ]);

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         variants={containerVariants}
         initial="hidden"
         animate="visible"
      >
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

         {!isLoading && displayOrderDetailsData && (
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
                              {displayOrderDetailsData.order_code}
                           </span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">
                              Date Placed
                           </span>
                           <span className="text-sm font-medium">
                              {new Date(
                                 displayOrderDetailsData.created_at,
                              ).toLocaleDateString('en-US', {
                                 year: 'numeric',
                                 month: 'long',
                                 day: 'numeric',
                              })}
                           </span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">Status</span>
                           <Badge
                              className={cn(
                                 'capitalize flex items-center select-none gap-1',
                                 getStatusColor(
                                    displayOrderDetailsData.status ===
                                       (EOrderStatus.PENDING_ASSIGNMENT ||
                                          EOrderStatus.CONFIRMED)
                                       ? EOrderStatus.CONFIRMED
                                       : displayOrderDetailsData.status,
                                 ),
                                 getHoverStatusColor(
                                    displayOrderDetailsData.status ===
                                       (EOrderStatus.PENDING_ASSIGNMENT ||
                                          EOrderStatus.CONFIRMED)
                                       ? EOrderStatus.CONFIRMED
                                       : displayOrderDetailsData.status,
                                 ),
                              )}
                           >
                              {getStatusIcon(
                                 displayOrderDetailsData.status ===
                                    (EOrderStatus.PENDING_ASSIGNMENT ||
                                       EOrderStatus.CONFIRMED)
                                    ? EOrderStatus.CONFIRMED
                                    : displayOrderDetailsData.status,
                              )}
                              {displayOrderDetailsData.status ===
                              (EOrderStatus.PENDING_ASSIGNMENT ||
                                 EOrderStatus.CONFIRMED)
                                 ? EOrderStatus.CONFIRMED
                                 : displayOrderDetailsData.status}
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
                              displayOrderDetailsData.shipping_address
                                 .contact_name
                           }
                        </p>
                        <p className="text-sm text-gray-500">
                           {
                              displayOrderDetailsData.shipping_address
                                 .contact_address_line
                           }
                        </p>
                        <p className="text-sm text-gray-500">
                           {
                              displayOrderDetailsData.shipping_address
                                 .contact_district
                           }
                           ,{' '}
                           {
                              displayOrderDetailsData.shipping_address
                                 .contact_province
                           }{' '}
                        </p>
                        <p className="text-sm text-gray-500">
                           {
                              displayOrderDetailsData.shipping_address
                                 .contact_country
                           }
                        </p>
                     </div>

                     <h3 className="text-sm font-medium text-gray-900 mt-4 mb-2">
                        Payment Method
                     </h3>
                     <div className="bg-gray-50 rounded-lg p-4">
                        <p className="text-sm font-medium">
                           {displayOrderDetailsData.payment_method}
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
                           {displayOrderDetailsData.order_items.map(
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
                                             {item.promotion?.final_price.toFixed(
                                                2,
                                             ) || item.unit_price.toFixed(2)}
                                          </p>
                                       </div>
                                       {displayOrderDetailsData.status ===
                                          ORDER_STATUS_TYPE_ENUM.DELIVERED &&
                                          !item.is_reviewed && (
                                             <div className="mt-2 flex justify-end">
                                                <Button
                                                   variant="ghost"
                                                   size="sm"
                                                   className="text-blue-600 hover:text-blue-800 text-xs"
                                                   onClick={(e) => {
                                                      e.stopPropagation();
                                                      //  setReviewItem(item);
                                                      //  setReviewModalOpen(true);
                                                   }}
                                                >
                                                   Write a Review
                                                </Button>
                                             </div>
                                          )}

                                       {item.is_reviewed && (
                                          <div className="mt-2 flex justify-end">
                                             <Button
                                                variant="ghost"
                                                size="sm"
                                                className="text-blue-600 hover:text-blue-800 text-xs"
                                                onClick={(e) => {
                                                   e.stopPropagation();
                                                   // setReviewItem(item);
                                                   // setReviewModalOpen(true);
                                                }}
                                             >
                                                View review
                                             </Button>
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
                        <span className="text-sm text-gray-500">Subtotal</span>
                        <span className="text-sm font-medium">
                           ${displayOrderDetailsData.total_amount.toFixed(2)}
                        </span>
                     </div>
                     <div className="flex justify-between">
                        <span className="text-sm text-gray-500">Shipping</span>
                        <span className="text-sm font-medium">$0.00</span>
                     </div>
                     <div className="flex justify-between">
                        <span className="text-sm text-gray-500">Discount</span>
                        <span className="text-sm font-medium">
                           ${(0.12).toFixed(2)}
                        </span>
                     </div>
                     <Separator />
                     <div className="flex justify-between">
                        <span className="text-sm font-medium">Total</span>
                        <span className="text-sm font-medium">
                           ${displayOrderDetailsData.total_amount.toFixed(2)}
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
                  {displayOrderDetailsData.status === EOrderStatus.PENDING && (
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
         {/* <Dialog open={reviewModalOpen} onOpenChange={setReviewModalOpen}>
            <DialogContent className="sm:max-w-[500px]">
               <DialogHeader>
                  <DialogTitle>Write a Review</DialogTitle>
               </DialogHeader>
               {reviewItem && (
                  <ReviewModal
                     order={order}
                     item={{
                        product_id: reviewItem.product_id,
                        model_id: reviewItem.model_id,
                        order_id: reviewItem.order_id,
                        order_item_id: reviewItem.order_item_id,
                        name: reviewItem.product_name,
                        image: reviewItem.product_image,
                        options: reviewItem.product_color_name,
                        isReviewed: reviewItem.is_reviewed,
                     }}
                     reviewedData={{
                        reviewId: reviewsInOrder.find(
                           (review) =>
                              review.order_item_id === reviewItem.order_item_id,
                        )?.review_id,
                        rating: reviewsInOrder.find(
                           (review) =>
                              review.order_item_id === reviewItem.order_item_id,
                        )?.rating,
                        content: reviewsInOrder.find(
                           (review) =>
                              review.order_item_id === reviewItem.order_item_id,
                        )?.content,
                     }}
                     onClose={() => setReviewModalOpen(false)}
                     onSubmit={() => {
                        window.location.reload();
                     }}
                  />
               )}
            </DialogContent>
         </Dialog> */}
      </motion.div>
   );
}
