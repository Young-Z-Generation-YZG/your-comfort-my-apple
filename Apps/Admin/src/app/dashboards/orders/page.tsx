'use client';

import { Button } from '@components/ui/button';
import ContentWrapper from '@components/ui/content-wrapper';
import { Input } from '@components/ui/input';
import { AnimatedTabsContent, Tabs, TabsTrigger } from '@components/ui/tabs';
import { TabsList } from '@radix-ui/react-tabs';
import { Search, X, Loader } from 'lucide-react';
import { Fragment, useEffect, useState } from 'react';
import { CreditCard, Package, Truck, PackageSearch } from 'lucide-react';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import Link from 'next/link';
import { Badge } from '@components/ui/badge';
import { cn } from '~/src/infrastructure/lib/utils';
import { sampleData } from './_data';
import { useGetOrdersAsyncQuery } from '~/src/infrastructure/services/order.service';
import { OrderResponse } from '~/src/domain/interfaces/orders/order.interface';
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

const OrdersList = () => {
   const [isLoading, setIsLoading] = useState(true);
   const [orders, setOrders] = useState<OrderResponse[]>([]);
   const [statusFilter, setStatusFilter] = useState('all');

   const {
      data: orderDataAsync,
      isLoading: loadingOrders,
      isFetching: fetchingOrders,
      isError: errorOrders,
      error: errorOrdersMessage,
      isSuccess: successOrders,
      refetch: refetchOrders,
   } = useGetOrdersAsyncQuery();

   // Filter orders based on status
   const filteredOrders =
      statusFilter === 'all'
         ? orders
         : orders.filter((order) => order.order_status === statusFilter);

   useEffect(() => {
      if (successOrders) {
         setOrders(orderDataAsync.items);

         setTimeout(() => {
            setIsLoading(false);
         }, 500);
      }
   }, [orderDataAsync]);

   return (
      <Fragment>
         <LoadingOverlay isLoading={isLoading} fullScreen />
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center justify-between">
                     <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                           Orders
                        </h1>
                        <p className="text-muted-foreground">
                           View and manage customer orders.
                        </p>
                     </div>
                  </div>

                  <div className="p-8">
                     <div className="mb-8 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
                        <div className="relative w-full max-w-md">
                           <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                           <Input
                              placeholder="Search orders by ID, customer, or product..."
                              className="pl-10 bg-white dark:bg-slate-800 border-slate-200 dark:border-slate-700 h-10"
                           />
                        </div>
                        <div className="flex items-center gap-2">
                           <Button
                              variant="outline"
                              size="sm"
                              className="h-10 bg-white dark:bg-slate-800 border-slate-200 dark:border-slate-700"
                           >
                              <svg
                                 xmlns="http://www.w3.org/2000/svg"
                                 width="16"
                                 height="16"
                                 viewBox="0 0 24 24"
                                 fill="none"
                                 stroke="currentColor"
                                 strokeWidth="2"
                                 strokeLinecap="round"
                                 strokeLinejoin="round"
                                 className="mr-2"
                              >
                                 <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4" />
                                 <polyline points="7 10 12 15 17 10" />
                                 <line x1="12" x2="12" y1="15" y2="3" />
                              </svg>
                              Export
                           </Button>
                           <Button
                              variant="outline"
                              size="sm"
                              className="h-10 bg-white dark:bg-slate-800 border-slate-200 dark:border-slate-700"
                           >
                              <svg
                                 xmlns="http://www.w3.org/2000/svg"
                                 width="16"
                                 height="16"
                                 viewBox="0 0 24 24"
                                 fill="none"
                                 stroke="currentColor"
                                 strokeWidth="2"
                                 strokeLinecap="round"
                                 strokeLinejoin="round"
                                 className="mr-2"
                              >
                                 <rect
                                    width="18"
                                    height="18"
                                    x="3"
                                    y="3"
                                    rx="2"
                                 />
                                 <path d="M3 9h18" />
                                 <path d="M3 15h18" />
                              </svg>
                              Columns
                           </Button>
                        </div>
                     </div>

                     <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700 shadow-sm overflow-hidden">
                        <div>
                           <div className="px-6 py-4 border-b border-slate-200 dark:border-slate-700">
                              <Tabs
                                 defaultValue="all"
                                 className="w-full"
                                 onValueChange={setStatusFilter}
                              >
                                 <TabsList className="bg-slate-100 dark:bg-slate-700/50 p-1 h-auto">
                                    <TabsTrigger
                                       value="all"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       All Orders
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="PENDING"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Pending
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="PREPARING"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Preparing
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="DELIVERING"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Delivering
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="DELIVERED"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Delivered
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="CANCELLED"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Cancelled
                                    </TabsTrigger>
                                 </TabsList>
                              </Tabs>
                           </div>

                           <div>
                              <Table>
                                 <TableHeader>
                                    <TableRow className="bg-slate-50 dark:bg-slate-800/50 hover:bg-slate-50 dark:hover:bg-slate-800/50">
                                       <TableHead className="font-medium">
                                          Order Code
                                       </TableHead>
                                       <TableHead className="font-medium">
                                          Customer
                                       </TableHead>
                                       <TableHead className="font-medium">
                                          Date
                                       </TableHead>
                                       <TableHead className="font-medium">
                                          Total
                                       </TableHead>
                                       <TableHead className="font-medium w-[150px]">
                                          Payment Type
                                       </TableHead>
                                       <TableHead className="font-medium">
                                          Status
                                       </TableHead>
                                       <TableHead className="text-right font-medium">
                                          Actions
                                       </TableHead>
                                    </TableRow>
                                 </TableHeader>
                                 <TableBody>
                                    {filteredOrders.map((order) => (
                                       <TableRow
                                          key={order.order_code}
                                          className="transition-all hover:bg-slate-50 dark:hover:bg-slate-800/50 animate-fadeIn border-b border-slate-200 dark:border-slate-700"
                                       >
                                          <TableCell className="order-id font-medium">
                                             {order.order_code}
                                          </TableCell>
                                          <TableCell className="customer-name">
                                             {
                                                order.order_shipping_address
                                                   .contact_name
                                             }
                                          </TableCell>
                                          <TableCell className="date">
                                             {new Date(
                                                order.order_created_at,
                                             ).toLocaleDateString()}
                                          </TableCell>
                                          <TableCell className="total-amount font-medium">
                                             $
                                             {order.order_total_amount.toFixed(
                                                2,
                                             )}
                                          </TableCell>
                                          <TableCell>
                                             <Badge
                                                className={cn(
                                                   'payment-type select-none flex items-center gap-1.5 font-medium py-1 px-2 mr-5 rounded-lg hover:bg-white',
                                                   `${statusConfig[order.order_status as keyof typeof statusConfig]?.color}`,
                                                )}
                                             >
                                                <CreditCard className="h-3.5 w-3.5 text-slate-500" />
                                                {order.order_payment_method}
                                             </Badge>
                                          </TableCell>
                                          <TableCell>
                                             <Badge
                                                className={cn(
                                                   'border select-none text-xs font-medium flex items-center gap-1.5 py-1 px-2 mr-5 rounded-lg uppercase hover:bg-white',
                                                   `${statusConfig[order.order_status as keyof typeof statusConfig]?.color}`,
                                                )}
                                             >
                                                {
                                                   statusConfig[
                                                      order.order_status as keyof typeof statusConfig
                                                   ].icon
                                                }
                                                {order.order_status}
                                             </Badge>
                                          </TableCell>
                                          <TableCell className="text-right">
                                             <Link
                                                href={`/dashboards/orders/${order.order_id}`}
                                             >
                                                <Button
                                                   variant="ghost"
                                                   size="sm"
                                                   className="h-8 px-3 text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-50"
                                                >
                                                   View Details
                                                </Button>
                                             </Link>
                                          </TableCell>
                                       </TableRow>
                                    ))}

                                    {}
                                 </TableBody>
                              </Table>
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

export default OrdersList;
