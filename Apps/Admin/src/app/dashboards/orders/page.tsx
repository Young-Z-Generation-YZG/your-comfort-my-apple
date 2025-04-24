'use client';

import { Button } from '@components/ui/button';
import ContentWrapper from '@components/ui/content-wrapper';
import { Input } from '@components/ui/input';
import { AnimatedTabsContent, Tabs, TabsTrigger } from '@components/ui/tabs';
import { TabsList } from '@radix-ui/react-tabs';
import { Search } from 'lucide-react';
import { Fragment, useState } from 'react';
import { CreditCard, Package, Truck } from 'lucide-react';
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

// Mock data for orders
const initialOrders = [
   {
      id: 'ORD-1234',
      customer: 'John Smith',
      date: '2023-04-24',
      total: 1299.0,
      status: 'pending',
      paymentType: 'Credit Card',
      items: [
         {
            name: 'iPhone 16 Pro',
            color: 'Black',
            storage: '256GB',
            price: 1299.0,
         },
      ],
      address: '123 Main St, New York, NY 10001',
      email: 'john.smith@example.com',
      phone: '+1 (555) 123-4567',
   },
   {
      id: 'ORD-1235',
      customer: 'Sarah Johnson',
      date: '2023-04-23',
      total: 594.15,
      status: 'processing',
      paymentType: 'PayPal',
      items: [
         { name: 'iPhone 16', color: 'Pink', storage: '128GB', price: 594.15 },
      ],
      address: '456 Oak Ave, San Francisco, CA 94102',
      email: 'sarah.j@example.com',
      phone: '+1 (555) 987-6543',
   },
   {
      id: 'ORD-1236',
      customer: 'Michael Brown',
      date: '2023-04-22',
      total: 2468.1,
      status: 'shipped',
      paymentType: 'Apple Pay',
      items: [
         {
            name: 'iPhone 16 Pro',
            color: 'Silver',
            storage: '512GB',
            price: 1399.0,
         },
         { name: 'iPhone 16', color: 'Blue', storage: '256GB', price: 1069.1 },
      ],
      address: '789 Pine St, Chicago, IL 60601',
      email: 'mbrown@example.com',
      phone: '+1 (555) 456-7890',
   },
   {
      id: 'ORD-1237',
      customer: 'Emily Davis',
      date: '2023-04-21',
      total: 1234.05,
      status: 'delivered',
      paymentType: 'Credit Card',
      items: [
         {
            name: 'iPhone 16 Plus',
            color: 'Ultramarine',
            storage: '256GB',
            price: 1234.05,
         },
      ],
      address: '321 Elm St, Boston, MA 02108',
      email: 'emily.d@example.com',
      phone: '+1 (555) 234-5678',
   },
   {
      id: 'ORD-1238',
      customer: 'David Wilson',
      date: '2023-04-20',
      total: 594.15,
      status: 'cancelled',
      paymentType: 'Bank Transfer',
      items: [
         { name: 'iPhone 16', color: 'Pink', storage: '128GB', price: 594.15 },
      ],
      address: '654 Maple Ave, Seattle, WA 98101',
      email: 'dwilson@example.com',
      phone: '+1 (555) 876-5432',
   },
];

// Status badge colors and icons
const statusConfig = {
   pending: {
      color: 'bg-amber-50 text-amber-700 border-amber-200',
      icon: <Package className="h-4 w-4" />,
   },
   processing: {
      color: 'bg-blue-50 text-blue-700 border-blue-200',
      icon: <Package className="h-4 w-4" />,
   },
   shipped: {
      color: 'bg-indigo-50 text-indigo-700 border-indigo-200',
      icon: <Truck className="h-4 w-4" />,
   },
   delivered: {
      color: 'bg-emerald-50 text-emerald-700 border-emerald-200',
      icon: <Package className="h-4 w-4" />,
   },
   cancelled: {
      color: 'bg-rose-50 text-rose-700 border-rose-200',
      icon: <Package className="h-4 w-4" />,
   },
};

const OrdersList = () => {
   const [orders] = useState(initialOrders);
   const [statusFilter, setStatusFilter] = useState('all');

   // Filter orders based on status
   const filteredOrders =
      statusFilter === 'all'
         ? orders
         : orders.filter((order) => order.status === statusFilter);

   return (
      <Fragment>
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
                                       value="pending"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Pending
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="processing"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Processing
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="shipped"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Shipped
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="delivered"
                                       className="px-4 py-2 data-[state=active]:bg-white dark:data-[state=active]:bg-slate-800"
                                    >
                                       Delivered
                                    </TabsTrigger>
                                    <TabsTrigger
                                       value="cancelled"
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
                                          Order ID
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
                                       <TableHead className="font-medium">
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
                                          key={order.id}
                                          className="transition-all hover:bg-slate-50 dark:hover:bg-slate-800/50 animate-fadeIn border-b border-slate-200 dark:border-slate-700"
                                       >
                                          <TableCell className="font-medium">
                                             {order.id}
                                          </TableCell>
                                          <TableCell>
                                             {order.customer}
                                          </TableCell>
                                          <TableCell>
                                             {new Date(
                                                order.date,
                                             ).toLocaleDateString()}
                                          </TableCell>
                                          <TableCell className="font-medium">
                                             ${order.total.toFixed(2)}
                                          </TableCell>
                                          <TableCell>
                                             <div
                                                className={`flex items-center gap-1.5 ${statusConfig[order.status as keyof typeof statusConfig]?.color}`}
                                             >
                                                <CreditCard className="h-3.5 w-3.5 text-slate-500" />
                                                {order.paymentType}
                                             </div>
                                          </TableCell>
                                          <TableCell>
                                             <Badge
                                                className={`$${statusConfig[order.status as keyof typeof statusConfig]?.color} border px-2 py-0.5 text-xs font-medium flex items-center gap-1.5`}
                                             >
                                                {
                                                   statusConfig[
                                                      order.status as keyof typeof statusConfig
                                                   ].icon
                                                }
                                                {order.status
                                                   .charAt(0)
                                                   .toUpperCase() +
                                                   order.status.slice(1)}
                                             </Badge>
                                          </TableCell>
                                          <TableCell className="text-right">
                                             <Link
                                                href={`/dashboards/orders/${order.id}`}
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
