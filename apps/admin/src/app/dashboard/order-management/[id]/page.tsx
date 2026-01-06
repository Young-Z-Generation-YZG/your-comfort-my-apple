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
   Tooltip,
   TooltipContent,
   TooltipProvider,
   TooltipTrigger,
} from '@components/ui/tooltip';
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
   Copy,
   Check,
} from 'lucide-react';
import { useParams } from 'next/navigation';
import { Fragment, useEffect, useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import Image from 'next/image';
import { cn } from '~/src/infrastructure/lib/utils';
import useOrderingService from '~/src/hooks/api/use-ordering-service';
import { truncateAddress } from '~/src/infrastructure/utils/truncate-address';
import { toast } from 'sonner';
import { useForm } from 'react-hook-form';
import {
   UpdateOrderStatusFormType,
   UpdateOrderStatusResolver,
} from '~/src/domain/schemas/order.schema';
import { LoadingOverlay } from '@components/loading-overlay';
import { TOrderDetails } from '~/src/domain/types/ordering.type';

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
   const [order, setOrder] = useState<TOrderDetails | null>(null);

   const params = useParams<{ id: string }>();

   const {
      getOrderDetailsAsync,
      getOrderDetailsState,
      updateOnlineOrderStatusAsync,
      isLoading,
   } = useOrderingService();

   const form = useForm({
      resolver: UpdateOrderStatusResolver,
   });

   const AvailableUpdateStatus = () => {
      if (order?.status === 'CONFIRMED') {
         return <SelectItem value="PREPARING">Preparing</SelectItem>;
      }

      if (order?.status === 'PAID') {
         return <SelectItem value="PREPARING">Preparing</SelectItem>;
      }

      if (order?.status === 'PREPARING') {
         return <SelectItem value="DELIVERING">Delivering</SelectItem>;
      }

      if (order?.status === 'DELIVERING') {
         return <SelectItem value="DELIVERED">Delivered</SelectItem>;
      }

      return null;
   };

   const handleUpdateStatus = async (data: UpdateOrderStatusFormType) => {
      if (
         form.formState.errors.update_status &&
         form.formState.errors.order_id
      ) {
         return;
      }

      const result = await updateOnlineOrderStatusAsync(data.order_id, {
         update_status: data.update_status,
      });

      if (result.isSuccess) {
         // Refetch order details to get updated status
         if (params.id) {
            await getOrderDetailsAsync(params.id);
         }
      }
   };

   useEffect(() => {
      if (params.id) {
         getOrderDetailsAsync(params.id);
      }
   }, [params.id, getOrderDetailsAsync]);

   useEffect(() => {
      if (getOrderDetailsState.isSuccess && getOrderDetailsState.data) {
         setOrder(getOrderDetailsState.data);
         form.setValue('order_id', getOrderDetailsState.data.order_id);
      }
   }, [getOrderDetailsState.data, getOrderDetailsState.isSuccess, form]);

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
                                 <Fragment>
                                    {order &&
                                       order.order_items.map((item, index) => (
                                          <motion.div
                                             key={index}
                                             className="p-4 flex items-center"
                                             variants={itemVariants}
                                             initial="hidden"
                                             animate="visible"
                                             custom={index}
                                          >
                                             <div className="flex-shrink-0 w-20 h-20 bg-gray-100 rounded-md overflow-hidden relative">
                                                <Image
                                                   src={
                                                      item.display_image_url ||
                                                      '/placeholder.svg'
                                                   }
                                                   alt={item.model_name}
                                                   fill
                                                   className="object-center object-cover"
                                                />
                                             </div>
                                             <div className="ml-4 flex-1">
                                                <h4 className="text-sm font-medium text-gray-900">
                                                   {item.model_name}
                                                </h4>
                                                <p className="mt-1 text-xs text-gray-500">
                                                   {item.color_name}
                                                </p>
                                                <div className="mt-1 flex justify-between">
                                                   <p className="text-sm text-gray-500">
                                                      Qty {item.quantity}
                                                   </p>
                                                   <p className="font-semibold text-gray-900 text-base">
                                                      $
                                                      {item.unit_price.toFixed(
                                                         2,
                                                      )}
                                                   </p>
                                                </div>
                                             </div>
                                          </motion.div>
                                       ))}
                                 </Fragment>
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
                                          {order?.shipping_address
                                             .contact_address_line +
                                             ', ' +
                                             order?.shipping_address
                                                .contact_district +
                                             ' ' +
                                             order?.shipping_address
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
                                          {order?.payment_method}
                                       </div>
                                    </div>
                                 </div>

                                 {order?.customer_public_key && (
                                    <CopyableAddressRow
                                       label="Customer Public Key"
                                       value={order.customer_public_key}
                                       displayValue={truncateAddress(
                                          order.customer_public_key,
                                          5,
                                          5,
                                       )}
                                    />
                                 )}

                                 {order?.tx && (
                                    <CopyableAddressRow
                                       label="Transaction Hash"
                                       value={order.tx}
                                       displayValue={truncateAddress(
                                          order.tx,
                                          10,
                                          10,
                                       )}
                                    />
                                 )}

                                 <div className="mt-4 pt-4 border-t border-slate-200 dark:border-slate-700">
                                    <div className="flex justify-between text-sm mb-2">
                                       <span className="text-muted-foreground">
                                          Subtotal
                                       </span>
                                       <span>
                                          $
                                          {order
                                             ? order.order_items
                                                  .reduce(
                                                     (sum, item) =>
                                                        sum +
                                                        item.unit_price *
                                                           item.quantity,
                                                     0,
                                                  )
                                                  .toFixed(2)
                                             : '0.00'}
                                       </span>
                                    </div>
                                    <div className="flex justify-between text-sm mb-2">
                                       <span className="text-muted-foreground">
                                          Discount
                                       </span>
                                       <span>
                                          $
                                          {(order?.discount_amount ?? 0) > 0
                                             ? (
                                                  order?.discount_amount ?? 0
                                               ).toFixed(2)
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
                                          {order?.total_amount?.toFixed(2) ||
                                             '0.00'}
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
                                          {order?.shipping_address.contact_name}
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
                                             order?.shipping_address
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
                                             order?.shipping_address
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
                                             `${statusConfig[order?.status as keyof typeof statusConfig]?.color}`,
                                          )}
                                       >
                                          {
                                             statusConfig[
                                                order?.status as keyof typeof statusConfig
                                             ].icon
                                          }
                                          {order?.status}
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

const CopyableAddressRow = ({
   label,
   value,
   displayValue,
}: {
   label: string;
   value: string;
   displayValue: string;
}) => {
   const [copied, setCopied] = useState(false);

   const handleCopy = async () => {
      try {
         await navigator.clipboard.writeText(value);
         setCopied(true);
         toast.success(`${label} copied to clipboard`);
         setTimeout(() => setCopied(false), 2000);
      } catch (err) {
         toast.error('Failed to copy to clipboard');
      }
   };

   return (
      <div className="flex items-start gap-3 mb-4">
         <div className="h-9 w-9 rounded-full bg-slate-100 dark:bg-slate-700 flex items-center justify-center flex-shrink-0">
            <CreditCard className="h-5 w-5 text-slate-500" />
         </div>
         <div className="flex-1">
            <div className="font-medium">{label}</div>
            <div className="flex items-center gap-2 mt-1">
               <TooltipProvider>
                  <Tooltip>
                     <TooltipTrigger asChild>
                        <Badge
                           variant="outline"
                           className="font-mono text-base"
                        >
                           {displayValue}
                        </Badge>
                     </TooltipTrigger>
                     <TooltipContent className="max-w-none whitespace-nowrap">
                        <p>{value}</p>
                     </TooltipContent>
                  </Tooltip>
               </TooltipProvider>
               <Button
                  variant="ghost"
                  size="icon"
                  className="h-6 w-6"
                  onClick={handleCopy}
                  title={`Copy ${label}`}
               >
                  {copied ? (
                     <Check className="h-3 w-3 text-green-600" />
                  ) : (
                     <Copy className="h-3 w-3" />
                  )}
               </Button>
            </div>
         </div>
      </div>
   );
};

export default OrderDetails;
