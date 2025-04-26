'use client';

import { Badge } from '@components/ui/badge';
import { Button } from '@components/ui/button';
import ContentWrapper from '@components/ui/content-wrapper';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { Separator } from '@components/ui/separator';
import {
   Download,
   Mail,
   MapPin,
   Phone,
   Printer,
   Send,
   Truck,
   User,
   CreditCard,
   Package,
   PackageSearch,
   Loader,
   X,
} from 'lucide-react';
import { useParams, useRouter } from 'next/navigation';
import { Fragment, useEffect, useState } from 'react';
import { sampleData } from './_data';
import { motion, AnimatePresence } from 'framer-motion';
import { cn } from '~/src/infrastructure/lib/utils';
import {
   useGetOrderDetailsAsyncQuery,
   useUpdateOrderAsyncMutation,
} from '~/src/infrastructure/services/order.service';
import { useForm } from 'react-hook-form';
import {
   UpdateOrderStatusFormType,
   UpdateOrderStatusResolver,
} from '~/src/domain/schemas/order.schema';
import { OrderDetailsResponse } from '~/src/domain/interfaces/orders/order.interface';
import { LoadingOverlay } from '@components/loading-overlay';

// Status badge colors and icons
const statusConfig = {
   PENDING: {
      color: 'bg-amber-50 text-amber-700 border-amber-200',
      icon: <Loader className="h-4 w-4" />,
   },
   CONFIRMED: {
      color: 'bg-green-50 text-green-700 border-green-200',
      icon: <Package className="h-4 w-4" />,
   },
   PAID: {
      color: 'bg-green-50 text-green-700 border-green-200',
      icon: <CreditCard className="h-4 w-4" />,
   },
   PREPARING: {
      color: 'bg-blue-50 text-blue-700 border-blue-200',
      icon: <PackageSearch className="h-4 w-4" />,
   },
   DELIVERING: {
      color: 'bg-indigo-50 text-indigo-700 border-indigo-200',
      icon: <Truck className="h-4 w-4" />,
   },
   DELIVERED: {
      color: 'bg-emerald-50 text-emerald-700 border-emerald-200',
      icon: <Package className="h-4 w-4" />,
   },
   CANCELLED: {
      color: 'bg-rose-50 text-rose-700 border-rose-200',
      icon: <X className="h-4 w-4" />,
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

const OrderDetails = () => {
   const [isLoading, setIsLoading] = useState(true);
   const [order, setOrder] = useState<OrderDetailsResponse | null>(null);
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

   const form = useForm({
      resolver: UpdateOrderStatusResolver,
   });

   const [
      updateOrderAsyncMutation,
      { isLoading: isUpdating, isSuccess: isUpdated },
   ] = useUpdateOrderAsyncMutation();

   const AvailableUpdateStatus = () => {
      if (order?.order_status === 'CONFIRMED') {
         return <SelectItem value="PREPARING">Preparing</SelectItem>;
      }

      if (order?.order_status === 'PAID') {
         return <SelectItem value="PREPARING">Preparing</SelectItem>;
      }

      if (order?.order_status === 'PREPARING') {
         return <SelectItem value="DELIVERING">Delivering</SelectItem>;
      }

      if (order?.order_status === 'DELIVERING') {
         return <SelectItem value="DELIVERED">Delivered</SelectItem>;
      }

      return null;
   };

   const handleUpdateStatus = async (data: UpdateOrderStatusFormType) => {
      console.log('data', data);

      if (
         form.formState.errors.update_status &&
         form.formState.errors.order_id
      ) {
         return;
      }

      await updateOrderAsyncMutation({
         order_id: data.order_id,
         update_status: data.update_status,
      });

      window.location.reload();
   };

   useEffect(() => {
      if (orderDetailsSuccess) {
         setOrder(orderDetailsData);

         form.setValue('order_id', orderDetailsData.order_id);

         setTimeout(() => {
            setIsLoading(false);
         }, 500);
      }
   }, [orderDetailsData]);

   return (
      <Fragment>
         <LoadingOverlay isLoading={isLoading} fullScreen />
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center justify-between">
                     <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                           Order {order?.order_code}
                        </h1>
                        <p className="text-muted-foreground mt-2">
                           Order ID:{' '}
                           <span className="font-bold">{order?.order_id}</span>
                        </p>
                     </div>
                  </div>
               </div>

               <div className="flex flex-row gap-10">
                  <div className="basis-2/3 flex flex-col gap-5">
                     <div className="bg-white px-5 py-5 rounded-md">
                        <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700 shadow-sm overflow-hidden">
                           <div className="px-6 py-5 border-b border-slate-200 dark:border-slate-700">
                              <h2 className="font-semibold text-lg">
                                 Order Items
                              </h2>
                           </div>
                           <div className="divide-y divide-gray-200">
                              <AnimatePresence>
                                 {order?.order_items.map((item, index) => (
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
                                             {item.promotion && (
                                                <div className="">
                                                   <p className="text-sm text-gray-500 inline-block mr-2">
                                                      <span className="text-sm text-gray-500">
                                                         Promotion:
                                                      </span>{' '}
                                                      {
                                                         item.promotion
                                                            .promotion_title
                                                      }
                                                   </p>
                                                   <p className="text-sm text-gray-500 inline-block mr-2">
                                                      Discount: %
                                                      {item.promotion
                                                         .promotion_discount_value *
                                                         100}
                                                   </p>
                                                   <p className="font-semibold text-red-500 text-base inline-block mr-2">
                                                      $
                                                      {item.promotion.promotion_discount_unit_price.toFixed(
                                                         2,
                                                      )}
                                                   </p>
                                                   <p className="text-sm text-gray-500 line-through inline-block">
                                                      $
                                                      {item.product_unit_price.toFixed(
                                                         2,
                                                      )}
                                                   </p>
                                                </div>
                                             )}

                                             {!item.promotion && (
                                                <p className="font-semibold text-gray-900 text-base">
                                                   $
                                                   {item.product_unit_price.toFixed(
                                                      2,
                                                   )}
                                                </p>
                                             )}
                                          </div>
                                       </div>
                                    </motion.div>
                                 ))}
                              </AnimatePresence>
                           </div>
                        </div>
                     </div>
                     <div className="flex flex-row gap-10">
                        <div className="bg-white flex-1 px-5 py-5 rounded-md">
                           <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700 shadow-sm overflow-hidden">
                              <div className="px-6 py-5 border-b border-slate-200 dark:border-slate-700">
                                 <h2 className="font-semibold text-lg">
                                    Shipping Information
                                 </h2>
                              </div>
                              <div className="p-6">
                                 <div className="flex items-start gap-3 mb-4">
                                    <div className="h-9 w-9 rounded-full bg-slate-100 dark:bg-slate-700 flex items-center justify-center flex-shrink-0">
                                       <MapPin className="h-5 w-5 text-slate-500" />
                                    </div>
                                    <div>
                                       <div className="font-medium">
                                          Shipping Address
                                       </div>
                                       <p className="text-sm text-muted-foreground mt-1">
                                          {order?.order_shipping_address
                                             .contact_address_line +
                                             ', ' +
                                             order?.order_shipping_address
                                                .contact_district +
                                             ' ' +
                                             order?.order_shipping_address
                                                .contact_province}
                                       </p>
                                    </div>
                                 </div>

                                 <div className="flex items-start gap-3">
                                    <div className="h-9 w-9 rounded-full bg-slate-100 dark:bg-slate-700 flex items-center justify-center flex-shrink-0">
                                       <Truck className="h-5 w-5 text-slate-500" />
                                    </div>
                                    <div>
                                       <div className="font-medium">
                                          Shipping Method
                                       </div>
                                       <div className="text-sm text-muted-foreground mt-1">
                                          Standard Shipping (3-5 business days)
                                       </div>
                                    </div>
                                 </div>
                              </div>
                           </div>
                        </div>
                        <div className="bg-white flex-1 px-5 py-5 rounded-md">
                           <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700 shadow-sm overflow-hidden">
                              <div className="px-6 py-5 border-b border-slate-200 dark:border-slate-700">
                                 <h2 className="font-semibold text-lg">
                                    Payment Information
                                 </h2>
                              </div>
                              <div className="p-6">
                                 <div className="flex items-start gap-3 mb-4">
                                    <div className="h-9 w-9 rounded-full bg-slate-100 dark:bg-slate-700 flex items-center justify-center flex-shrink-0">
                                       <CreditCard className="h-5 w-5 text-slate-500" />
                                    </div>
                                    <div>
                                       <div className="font-medium">
                                          Payment Method
                                       </div>
                                       <div className="text-sm text-muted-foreground mt-1">
                                          {order?.order_payment_method}
                                       </div>
                                    </div>
                                 </div>

                                 <div className="mt-4 pt-4 border-t border-slate-200 dark:border-slate-700">
                                    <div className="flex justify-between text-sm mb-2">
                                       <span className="text-muted-foreground">
                                          Subtotal
                                       </span>
                                       <span>
                                          $
                                          {order?.order_sub_total_amount.toFixed(
                                             2,
                                          )}
                                       </span>
                                    </div>
                                    <div className="flex justify-between text-sm mb-2">
                                       <span className="text-muted-foreground">
                                          Discount
                                       </span>
                                       <span>
                                          $
                                          {(order?.order_discount_amount ?? 0) >
                                          0
                                             ? order?.order_discount_amount.toFixed(
                                                  2,
                                               )
                                             : '0.00'}
                                       </span>
                                    </div>
                                    <div className="flex justify-between text-sm mb-2">
                                       <span className="text-muted-foreground">
                                          Shipping
                                       </span>
                                       <span>$0.00</span>
                                    </div>
                                    <div className="flex justify-between text-sm mb-4">
                                       <span className="text-muted-foreground">
                                          Tax
                                       </span>
                                       <span>Included</span>
                                    </div>
                                    <Separator className="my-2" />
                                    <div className="flex justify-between font-medium text-lg mt-2">
                                       <span>Total</span>
                                       <span>
                                          $
                                          {order?.order_total_amount?.toFixed(
                                             2,
                                          ) || '0.00'}
                                       </span>
                                    </div>
                                 </div>
                              </div>
                           </div>
                        </div>
                     </div>
                  </div>

                  <div className="basis-1/3">
                     <div className="flex flex-col gap-10">
                        <div className="bg-white flex-1 px-5 py-5 rounded-md">
                           <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700 shadow-sm overflow-hidden">
                              <div className="px-6 py-5 border-b border-slate-200 dark:border-slate-700">
                                 <h2 className="font-semibold text-lg">
                                    Customer Information
                                 </h2>
                              </div>
                              <div className="p-6">
                                 <div className="flex items-start gap-3 mb-4">
                                    <div className="h-9 w-9 rounded-full bg-slate-100 dark:bg-slate-700 flex items-center justify-center flex-shrink-0">
                                       <User className="h-5 w-5 text-slate-500" />
                                    </div>
                                    <div>
                                       <div className="font-medium">
                                          Customer Name
                                       </div>
                                       <div className="text-sm text-muted-foreground mt-1">
                                          {
                                             order?.order_shipping_address
                                                .contact_name
                                          }
                                       </div>
                                    </div>
                                 </div>

                                 <div className="flex items-start gap-3 mb-4">
                                    <div className="h-9 w-9 rounded-full bg-slate-100 dark:bg-slate-700 flex items-center justify-center flex-shrink-0">
                                       <Mail className="h-5 w-5 text-slate-500" />
                                    </div>
                                    <div>
                                       <div className="font-medium">
                                          Email Address
                                       </div>
                                       <div className="text-sm text-muted-foreground mt-1">
                                          {
                                             order?.order_shipping_address
                                                .contact_email
                                          }
                                       </div>
                                    </div>
                                 </div>

                                 <div className="flex items-start gap-3">
                                    <div className="h-9 w-9 rounded-full bg-slate-100 dark:bg-slate-700 flex items-center justify-center flex-shrink-0">
                                       <Phone className="h-5 w-5 text-slate-500" />
                                    </div>
                                    <div>
                                       <div className="font-medium">
                                          Phone Number
                                       </div>
                                       <div className="text-sm text-muted-foreground mt-1">
                                          {
                                             order?.order_shipping_address
                                                .contact_phone_number
                                          }
                                       </div>
                                    </div>
                                 </div>
                              </div>
                           </div>
                        </div>

                        <div className="bg-white flex-1 px-5 py-5 rounded-md">
                           <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700 shadow-sm overflow-hidden">
                              <div className="px-6 py-5 border-b border-slate-200 dark:border-slate-700">
                                 <h2 className="font-semibold text-lg">
                                    Order Status
                                 </h2>
                              </div>
                              <div className="p-6">
                                 <div className="mb-4">
                                    <div className="font-medium mb-2">
                                       Current Status
                                    </div>
                                    {order ? (
                                       <Badge
                                          className={cn(
                                             'border select-none text-sm font-medium flex items-center gap-1.5 py-2 px-2 mr-5 rounded-lg uppercase hover:bg-white',
                                             `${statusConfig[order?.order_status as keyof typeof statusConfig]?.color}`,
                                          )}
                                       >
                                          {
                                             statusConfig[
                                                order?.order_status as keyof typeof statusConfig
                                             ].icon
                                          }
                                          {order?.order_status}
                                       </Badge>
                                    ) : null}
                                 </div>

                                 <div className="mb-6">
                                    <div className="font-medium mb-2">
                                       Update Status
                                    </div>
                                    <Select
                                       defaultValue={'delivered'}
                                       onValueChange={(value) => {
                                          form.setValue(
                                             'update_status',
                                             value as any,
                                          );
                                       }}
                                       disabled={
                                          AvailableUpdateStatus() === null
                                       }
                                    >
                                       <SelectTrigger className="w-full">
                                          <SelectValue placeholder="Select status" />
                                       </SelectTrigger>
                                       <SelectContent className="w-full">
                                          {AvailableUpdateStatus()}
                                       </SelectContent>
                                    </Select>
                                 </div>

                                 <Button
                                    className="w-full"
                                    disabled={AvailableUpdateStatus() === null}
                                    onClick={() => {
                                       console.log(
                                          'form',
                                          form.getValues('order_id'),
                                       );

                                       console.log(
                                          'form',
                                          form.getValues('update_status'),
                                       );

                                       form.handleSubmit(handleUpdateStatus)();
                                    }}
                                 >
                                    Update Status
                                 </Button>
                              </div>
                           </div>
                        </div>
                        <div className="bg-white flex-1 px-5 py-5 rounded-md">
                           <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700 shadow-sm overflow-hidden">
                              <div className="px-6 py-5 border-b border-slate-200 dark:border-slate-700">
                                 <h2 className="font-semibold text-lg">
                                    Actions
                                 </h2>
                              </div>
                              <div className="p-6 space-y-3">
                                 <Button
                                    variant="outline"
                                    className="w-full justify-start"
                                 >
                                    <Printer className="h-4 w-4 mr-2" />
                                    Print Invoice
                                 </Button>
                                 <Button
                                    variant="outline"
                                    className="w-full justify-start"
                                 >
                                    <Download className="h-4 w-4 mr-2" />
                                    Download Invoice
                                 </Button>
                                 <Button
                                    variant="outline"
                                    className="w-full justify-start"
                                 >
                                    <Send className="h-4 w-4 mr-2" />
                                    Email Customer
                                 </Button>
                              </div>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default OrderDetails;
