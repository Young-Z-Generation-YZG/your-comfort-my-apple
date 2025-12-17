'use client';

import type { ReactNode } from 'react';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import Image from 'next/image';
import { ArrowLeft } from 'lucide-react';

import {
   Card,
   CardContent,
   CardDescription,
   CardHeader,
   CardTitle,
} from '@components/ui/card';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import { Separator } from '@components/ui/separator';
import { LoadingOverlay } from '@components/loading-overlay';

import { useToast } from '~/src/hooks/use-toast';
import useOrderingService from '~/src/hooks/api/use-ordering-service';
import { EOrderStatus } from '~/src/domain/enums/order-status.enum';
import type { TOrder, TOrderItem } from '~/src/domain/types/ordering.type';
import { cn } from '~/src/infrastructure/lib/utils';

const statusColorMap: Record<EOrderStatus, string> = {
   [EOrderStatus.PENDING]: 'bg-yellow-100 text-yellow-800 border-yellow-200',
   [EOrderStatus.CONFIRMED]: 'bg-blue-100 text-blue-800 border-blue-200',
   [EOrderStatus.PREPARING]: 'bg-purple-100 text-purple-800 border-purple-200',
   [EOrderStatus.DELIVERING]: 'bg-sky-100 text-sky-800 border-sky-200',
   [EOrderStatus.DELIVERED]:
      'bg-emerald-100 text-emerald-800 border-emerald-200',
   [EOrderStatus.CANCELLED]: 'bg-red-100 text-red-800 border-red-200',
   [EOrderStatus.PAID]: 'bg-green-100 text-green-800 border-green-200',
};

const currencyFormatter = new Intl.NumberFormat('en-US', {
   style: 'currency',
   currency: 'USD',
});

const dateTimeFormatter = new Intl.DateTimeFormat('vi-VN', {
   dateStyle: 'medium',
   timeStyle: 'short',
});

const availableUpdateStatuses: Record<EOrderStatus, EOrderStatus[]> = {
   [EOrderStatus.PENDING]: [EOrderStatus.CONFIRMED],
   [EOrderStatus.CONFIRMED]: [EOrderStatus.PREPARING],
   [EOrderStatus.PAID]: [EOrderStatus.PREPARING],
   [EOrderStatus.PREPARING]: [EOrderStatus.DELIVERING],
   [EOrderStatus.DELIVERING]: [EOrderStatus.DELIVERED],
   [EOrderStatus.DELIVERED]: [],
   [EOrderStatus.CANCELLED]: [],
};

const OnlineOrderDetailsPage = () => {
   const params = useParams<{ id: string }>();
   const router = useRouter();
   const { toast } = useToast();

   const {
      getOrderDetailsAsync,
      updateOnlineOrderStatusAsync,
      updateOnlineOrderStatusState,
      isLoading,
   } = useOrderingService();

   const [order, setOrder] = useState<TOrder | null>(null);
   const [selectedStatus, setSelectedStatus] = useState<EOrderStatus | ''>('');

   const orderId = useMemo(() => {
      const id = params?.id;
      if (Array.isArray(id)) {
         return id[0];
      }
      return id ?? '';
   }, [params]);

   const fetchOrderDetails = useCallback(async () => {
      if (!orderId) {
         return;
      }

      const result = await getOrderDetailsAsync(orderId);

      if (result.isSuccess && result.data) {
         setOrder(result.data as TOrder);
      } else {
         setOrder(null);
         toast({
            variant: 'destructive',
            title: 'Unable to find this order',
            description: 'Please verify the order ID or try again later.',
         });
      }
   }, [orderId, getOrderDetailsAsync, toast]);

   useEffect(() => {
      fetchOrderDetails();
   }, [fetchOrderDetails]);

   useEffect(() => {
      if (order?.status) {
         setSelectedStatus(order.status as EOrderStatus);
      }
   }, [order?.status]);

   const formattedCreatedAt = order?.created_at
      ? dateTimeFormatter.format(new Date(order.created_at))
      : '—';

   const handleUpdateStatus = useCallback(async () => {
      if (!order || !selectedStatus || selectedStatus === order.status) {
         return;
      }

      const result = await updateOnlineOrderStatusAsync(order.order_id, {
         update_status: selectedStatus,
      });

      if (result.isSuccess) {
         setOrder((prev) =>
            prev ? { ...prev, status: selectedStatus } : prev,
         );
         toast({
            title: 'Order status updated',
            description: `Status changed to ${formatStatusLabel(selectedStatus)}.`,
         });
      } else {
         toast({
            variant: 'destructive',
            title: 'Failed to update status',
            description: 'Please try again in a moment.',
         });
      }
   }, [order, selectedStatus, updateOnlineOrderStatusAsync, toast]);

   const canUpdateStatus =
      Boolean(order) &&
      Boolean(selectedStatus) &&
      selectedStatus !== order?.status;

   const availableStatuses = useMemo(() => {
      if (!order?.status) {
         return [];
      }

      const currentStatus = order.status as EOrderStatus;
      const nextStatuses = availableUpdateStatuses[currentStatus] ?? [];

      // Include current status as the first option
      return [currentStatus, ...nextStatuses];
   }, [order?.status]);

   return (
      <LoadingOverlay
         isLoading={isLoading}
         fullScreen
         text="Loading order details..."
      >
         {order ? (
            <div className="mx-auto flex max-w-6xl flex-col gap-6 py-6">
               <div className="flex flex-col gap-4">
                  <Button
                     variant="ghost"
                     className="w-fit"
                     onClick={() => router.back()}
                  >
                     <ArrowLeft className="mr-2 h-4 w-4" />
                     Back
                  </Button>

                  <div className="flex flex-col gap-4 md:flex-row md:items-start md:justify-between">
                     <div className="space-y-2">
                        <p className="text-sm text-muted-foreground">
                           Order Code
                        </p>
                        <h1 className="text-3xl font-semibold">
                           {order.order_code}
                        </h1>
                        <p className="text-sm text-muted-foreground">
                           Placed on {formattedCreatedAt}
                        </p>
                     </div>
                     <div className="flex flex-col gap-3 md:items-end">
                        <Badge
                           className={cn(
                              'w-fit border',
                              statusColorMap[order.status as EOrderStatus] ??
                                 'bg-muted text-foreground',
                           )}
                        >
                           {formatStatusLabel(order.status)}
                        </Badge>

                        <div className="flex flex-col gap-3 md:flex-row md:items-center">
                           <Select
                              value={selectedStatus}
                              onValueChange={(value) =>
                                 setSelectedStatus(value as EOrderStatus)
                              }
                              disabled={updateOnlineOrderStatusState.isLoading}
                           >
                              <SelectTrigger className="w-[220px]">
                                 <SelectValue placeholder="Select status" />
                              </SelectTrigger>
                              <SelectContent>
                                 {availableStatuses.map(
                                    (status: EOrderStatus) => {
                                       const isCurrentStatus =
                                          status === order?.status;
                                       return (
                                          <SelectItem
                                             key={status}
                                             value={status}
                                             disabled={isCurrentStatus}
                                             className={
                                                isCurrentStatus
                                                   ? 'opacity-50 cursor-not-allowed'
                                                   : ''
                                             }
                                          >
                                             {formatStatusLabel(status)}
                                             {isCurrentStatus && ' (Current)'}
                                          </SelectItem>
                                       );
                                    },
                                 )}
                              </SelectContent>
                           </Select>
                           <Button
                              onClick={handleUpdateStatus}
                              disabled={
                                 !canUpdateStatus ||
                                 updateOnlineOrderStatusState.isLoading
                              }
                           >
                              {updateOnlineOrderStatusState.isLoading
                                 ? 'Updating...'
                                 : 'Update status'}
                           </Button>
                        </div>
                     </div>
                  </div>
               </div>

               <div className="grid gap-6 lg:grid-cols-2">
                  <Card>
                     <CardHeader>
                        <CardTitle>Order Information</CardTitle>
                        <CardDescription>
                           Overview of payment and customer details
                        </CardDescription>
                     </CardHeader>
                     <CardContent className="space-y-4">
                        <InfoRow
                           label="Customer ID"
                           value={order.customer_id}
                        />
                        <InfoRow
                           label="Customer email"
                           value={order.customer_email}
                        />
                        <InfoRow
                           label="Payment method"
                           value={order.payment_method}
                        />
                        <InfoRow
                           label="Total amount"
                           value={currencyFormatter.format(order.total_amount)}
                        />
                     </CardContent>
                  </Card>

                  <Card>
                     <CardHeader>
                        <CardTitle>Shipping Information</CardTitle>
                        <CardDescription>
                           Destination and contact details
                        </CardDescription>
                     </CardHeader>
                     <CardContent className="space-y-4">
                        <InfoRow
                           label="Recipient"
                           value={order.shipping_address?.contact_name ?? '—'}
                        />
                        <InfoRow
                           label="Phone number"
                           value={
                              order.shipping_address?.contact_phone_number ??
                              '—'
                           }
                        />
                        <InfoRow
                           label="Email"
                           value={order.shipping_address?.contact_email ?? '—'}
                        />
                        <InfoRow
                           label="Address"
                           value={formatShippingAddress(order.shipping_address)}
                        />
                     </CardContent>
                  </Card>
               </div>

               <Card>
                  <CardHeader>
                     <CardTitle>
                        Order Items ({order.order_items?.length ?? 0})
                     </CardTitle>
                     <CardDescription>
                        Items included in this order
                     </CardDescription>
                  </CardHeader>
                  <CardContent>
                     {order.order_items?.length ? (
                        <Table>
                           <TableHeader>
                              <TableRow>
                                 <TableHead>Item</TableHead>
                                 <TableHead>Variant</TableHead>
                                 <TableHead className="text-right">
                                    Unit price
                                 </TableHead>
                                 <TableHead className="text-right">
                                    Quantity
                                 </TableHead>
                                 <TableHead className="text-right">
                                    Subtotal
                                 </TableHead>
                              </TableRow>
                           </TableHeader>
                           <TableBody>
                              {order.order_items.map((item) => (
                                 <OrderItemRow
                                    key={item.order_item_id}
                                    item={item}
                                 />
                              ))}
                           </TableBody>
                        </Table>
                     ) : (
                        <p className="text-sm text-muted-foreground">
                           No items found for this order.
                        </p>
                     )}
                  </CardContent>
               </Card>
            </div>
         ) : (
            <div className="flex flex-col items-center justify-center gap-4 py-16 text-center">
               <p className="text-lg font-semibold">Order not found</p>
               <p className="text-sm text-muted-foreground">
                  We could not locate the requested order. Please return to the
                  orders list and try again.
               </p>
               <Button onClick={() => router.push('/dashboard/online/orders')}>
                  Back to orders
               </Button>
            </div>
         )}
      </LoadingOverlay>
   );
};

const formatStatusLabel = (status: string) =>
   status
      ?.toLowerCase()
      .split('_')
      .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');

const formatShippingAddress = (address?: TOrder['shipping_address']) => {
   if (!address) {
      return '—';
   }

   const formatted = [
      address.contact_address_line,
      address.contact_district,
      address.contact_province,
      address.contact_country,
   ]
      .filter(Boolean)
      .join(', ');

   return formatted || '—';
};

const InfoRow = ({ label, value }: { label: string; value?: ReactNode }) => (
   <div>
      <p className="text-sm text-muted-foreground">{label}</p>
      <p className="font-medium text-foreground">{value ?? '—'}</p>
      <Separator className="my-3" />
   </div>
);

const OrderItemRow = ({ item }: { item: TOrderItem }) => {
   const subtotal = currencyFormatter.format(item.unit_price * item.quantity);

   return (
      <TableRow>
         <TableCell>
            <div className="flex items-center gap-3">
               <div className="h-14 w-14 overflow-hidden rounded-md border bg-muted">
                  {item.display_image_url ? (
                     <div className="relative h-full w-full">
                        <Image
                           src={item.display_image_url}
                           alt={item.model_name}
                           fill
                           sizes="56px"
                           className="object-cover"
                        />
                     </div>
                  ) : (
                     <div className="flex h-full w-full items-center justify-center text-xs text-muted-foreground">
                        No image
                     </div>
                  )}
               </div>
               <div>
                  <p className="font-medium">{item.model_name}</p>
                  <p className="text-xs text-muted-foreground">
                     SKU ID: {item.sku_id ?? '—'}
                  </p>
               </div>
            </div>
         </TableCell>
         <TableCell>
            <div className="text-sm text-muted-foreground">
               {item.color_name} / {item.storage_name}
            </div>
         </TableCell>
         <TableCell className="text-right">
            {currencyFormatter.format(item.unit_price)}
         </TableCell>
         <TableCell className="text-right">{item.quantity}</TableCell>
         <TableCell className="text-right font-semibold">{subtotal}</TableCell>
      </TableRow>
   );
};

export default OnlineOrderDetailsPage;
