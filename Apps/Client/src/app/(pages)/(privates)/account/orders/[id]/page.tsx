'use client';

import { useState, useEffect } from 'react';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import { Separator } from '@components/ui/separator';
import {
   ArrowLeft,
   Package,
   Truck,
   CheckCircle,
   AlertCircle,
   Check,
   X,
} from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import {
   useCancelOrderAsyncMutation,
   useConfirmOrderAsyncMutation,
   useGetOrderDetailsAsyncQuery,
} from '~/infrastructure/services/order.service';
import { useParams, useRouter } from 'next/navigation';
import {
   OrderDetailsResponse,
   OrderItemResponse,
} from '~/domain/interfaces/orders/order.interface';
import { cn } from '~/infrastructure/lib/utils';
import { toast } from 'sonner';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
} from '@components/ui/dialog';

import { ReviewModal } from '../_components/review-model';
import { useToast } from '~/hooks/use-toast';

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

export default function OrderDetails() {
   const [order, setOrder] = useState<OrderDetailsResponse | null>(null);
   const [isLoading, setIsLoading] = useState(true);
   const [reviewItem, setReviewItem] = useState<OrderItemResponse | null>(null);
   const [reviewModalOpen, setReviewModalOpen] = useState(false);
   const [timeLeft, setTimeLeft] = useState(30 * 60); // 30 minutes in seconds
   const { toast } = useToast();
   const router = useRouter();
   const params = useParams<{ id: string }>();

   const {
      data: orderDetailsData,
      isLoading: orderDetailsLoading,
      isError: orderDetailsError,
      error: orderDetailsErrorResponse,
      isSuccess: orderDetailsSuccess,
      isFetching: orderDetailsFetching,
      refetch: orderDetailsRefetch,
   } = useGetOrderDetailsAsyncQuery(params.id);

   const [confirmOrder, { isLoading: isConfirmingOrder }] =
      useConfirmOrderAsyncMutation();
   const [cancelOrder, { isLoading: isCancellingOrder }] =
      useCancelOrderAsyncMutation();

   useEffect(() => {
      if (orderDetailsSuccess) {
         setOrder(orderDetailsData);

         const orderCreatedAt = new Date(orderDetailsData.order_created_at);
         const currentTime = new Date();
         const timeDifference =
            currentTime.getTime() - orderCreatedAt.getTime();
         const minutesDifference = Math.floor(timeDifference / (1000 * 60));

         setTimeLeft((30 - minutesDifference) * 60);

         setTimeout(() => {
            setIsLoading(false);
         }, 500);
      }
   }, [orderDetailsData]);

   // Get status icon
   const getStatusIcon = (status: string) => {
      switch (status) {
         case 'PENDING':
            return <Package className="h-5 w-5 text-yellow-500" />;
         case 'PAID':
            return <CheckCircle className="h-5 w-5 text-blue-500" />;
         case 'DELIVERED':
            return <Truck className="h-5 w-5 text-green-500" />;
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

   const CountdownTimer = () => {
      useEffect(() => {
         if (timeLeft <= 0) return;

         const intervalId = setInterval(() => {
            setTimeLeft((prevTime) => prevTime - 1);
         }, 1000);

         return () => clearInterval(intervalId);
      }, [timeLeft]);

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

   const renderButton = () => {
      if (order?.order_status === 'PENDING') {
         return (
            <div className="flex items-center gap-2">
               <CountdownTimer />

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
         );
      }

      if (order?.order_status === 'CONFIRMED') {
         return (
            <div className="flex items-center gap-2">
               <Button
                  className="bg-red-600 hover:bg-red-700 text-white"
                  onClick={() => {
                     handleCancelOrder();
                  }}
               >
                  <X className="mr-2 h-4 w-4" />
                  Cancel Order
               </Button>
            </div>
         );
      }

      return null;
   };

   const handleConfirmOrder = async () => {
      if (order?.order_id) {
         await confirmOrder(order.order_id).unwrap();

         window.location.reload();
      }
   };

   const handleCancelOrder = async () => {
      if (order?.order_id) {
         await cancelOrder(order.order_id).unwrap();

         window.location.reload();
      }
   };

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
                  window.history.back();
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

         {isLoading ? (
            <div className="px-6 py-8 text-center">
               <div className="inline-block h-8 w-8 animate-spin rounded-full border-4 border-solid border-blue-600 border-r-transparent align-[-0.125em] motion-reduce:animate-[spin_1.5s_linear_infinite]"></div>
               <p className="mt-4 text-sm text-gray-500">
                  Loading order details...
               </p>
            </div>
         ) : order ? (
            <div className="p-6">
               <motion.div
                  className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6"
                  variants={itemVariants}
               >
                  <div>
                     <h3 className="text-sm font-medium text-gray-900 mb-2">
                        Order Information
                     </h3>
                     <div className="bg-gray-50 rounded-lg p-4 space-y-3">
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">
                              Order Number
                           </span>
                           <span className="text-sm font-medium">
                              {order.order_id}
                           </span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">
                              Date Placed
                           </span>
                           <span className="text-sm font-medium">
                              {new Date(
                                 order.order_created_at,
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
                                 'capitalize flex items-center gap-1',
                                 getStatusColor(order.order_status),
                              )}
                           >
                              {getStatusIcon(order.order_status)}
                              {order.order_status}
                           </Badge>
                        </div>
                        {true && (
                           <div className="flex justify-between">
                              <span className="text-sm text-gray-500">
                                 Tracking Number
                              </span>
                              <span className="text-sm font-medium text-blue-600 hover:underline cursor-pointer">
                                 {order.order_code}
                              </span>
                           </div>
                        )}
                        {true && (
                           <div className="flex justify-between">
                              <span className="text-sm text-gray-500">
                                 Delivery
                              </span>
                              <span className="text-sm font-medium">
                                 Expected to ship in 1-2 business days
                              </span>
                           </div>
                        )}
                     </div>
                  </div>

                  <div>
                     <h3 className="text-sm font-medium text-gray-900 mb-2">
                        Shipping Address
                     </h3>
                     <div className="bg-gray-50 rounded-lg p-4">
                        <p className="text-sm font-medium">
                           {order.order_shipping_address.contact_name}
                        </p>
                        <p className="text-sm text-gray-500">
                           {order.order_shipping_address.contact_address_line}
                        </p>
                        <p className="text-sm text-gray-500">
                           {order.order_shipping_address.contact_district},{' '}
                           {order.order_shipping_address.contact_province}{' '}
                        </p>
                        <p className="text-sm text-gray-500">
                           {order.order_shipping_address.contact_country}
                        </p>
                     </div>

                     <h3 className="text-sm font-medium text-gray-900 mt-4 mb-2">
                        Payment Method
                     </h3>
                     <div className="bg-gray-50 rounded-lg p-4">
                        <p className="text-sm font-medium">
                           {order.order_payment_method}
                        </p>
                     </div>
                  </div>
               </motion.div>

               <motion.div variants={itemVariants} className="mb-6">
                  <h3 className="text-sm font-medium text-gray-900 mb-2">
                     Order Items
                  </h3>
                  <div className="border rounded-lg overflow-hidden">
                     <div className="divide-y divide-gray-200">
                        <AnimatePresence>
                           {order.order_items.map((item, index) => (
                              <motion.div
                                 key={index}
                                 className="p-4 flex items-center"
                                 variants={itemVariants}
                                 initial="hidden"
                                 animate="visible"
                                 custom={index}
                              >
                                 <div className="flex-shrink-0 w-20 h-20 bg-gray-100 rounded-md overflow-hidden">
                                    <img
                                       src={
                                          item.product_image ||
                                          '/placeholder.svg'
                                       }
                                       alt={item.product_name}
                                       className="w-full h-full object-center object-cover"
                                    />
                                 </div>
                                 <div className="ml-4 flex-1">
                                    <h4 className="text-sm font-medium text-gray-900">
                                       {item.product_name}
                                    </h4>
                                    <p className="mt-1 text-xs text-gray-500">
                                       {item.product_color_name}
                                    </p>
                                    <div className="mt-1 flex justify-between">
                                       <p className="text-sm text-gray-500">
                                          Qty {item.quantity}
                                       </p>
                                       <p className="text-sm font-medium text-gray-900">
                                          $
                                          {item.promotion?.promotion_discount_unit_price.toFixed(
                                             2,
                                          ) ||
                                             item.product_unit_price.toFixed(2)}
                                       </p>
                                    </div>
                                    {true && !item.is_reviewed && (
                                       <div className="mt-2 flex justify-end">
                                          <Button
                                             variant="ghost"
                                             size="sm"
                                             className="text-blue-600 hover:text-blue-800 text-xs"
                                             onClick={(e) => {
                                                e.stopPropagation();
                                                setReviewItem(item);
                                                setReviewModalOpen(true);
                                             }}
                                          >
                                             Write a Review
                                          </Button>
                                       </div>
                                    )}
                                 </div>
                              </motion.div>
                           ))}
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
                           ${order.order_sub_total_amount.toFixed(2)}
                        </span>
                     </div>
                     <div className="flex justify-between">
                        <span className="text-sm text-gray-500">Shipping</span>
                        <span className="text-sm font-medium">$0.00</span>
                     </div>
                     <div className="flex justify-between">
                        <span className="text-sm text-gray-500">Discount</span>
                        <span className="text-sm font-medium">
                           ${order.order_discount_amount.toFixed(2)}
                        </span>
                     </div>
                     <Separator />
                     <div className="flex justify-between">
                        <span className="text-sm font-medium">Total</span>
                        <span className="text-sm font-medium">
                           ${order.order_total_amount.toFixed(2)}
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
                  {renderButton()}
               </motion.div>
            </div>
         ) : (
            <motion.div
               className="px-6 py-8 text-center"
               variants={itemVariants}
            >
               <AlertCircle className="mx-auto h-12 w-12 text-red-500" />
               <h3 className="mt-2 text-sm font-medium text-gray-900">
                  Order Not Found
               </h3>
               <p className="mt-1 text-sm text-gray-500">
                  We couldn't find the order you're looking for.
               </p>
               <div className="mt-6">
                  <Button>Back to Orders</Button>
               </div>
            </motion.div>
         )}
         {/* Review Modal */}
         <Dialog open={reviewModalOpen} onOpenChange={setReviewModalOpen}>
            <DialogContent className="sm:max-w-[500px]">
               <DialogHeader>
                  <DialogTitle>Write a Review</DialogTitle>
               </DialogHeader>
               {reviewItem && (
                  <ReviewModal
                     item={{
                        product_id: reviewItem.product_id,
                        model_id: reviewItem.model_id,
                        order_item_id: reviewItem.order_item_id,
                        name: reviewItem.product_name,
                        image: reviewItem.product_image,
                        options: reviewItem.product_color_name,
                     }}
                     onClose={() => setReviewModalOpen(false)}
                     onSubmit={() => {
                        orderDetailsRefetch();
                     }}
                  />
               )}
            </DialogContent>
         </Dialog>
      </motion.div>
   );
}
