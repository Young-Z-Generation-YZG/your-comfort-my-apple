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
   CreditCard,
   Download,
   Mail,
   MapPin,
   Package,
   Phone,
   Printer,
   Send,
   Truck,
   User,
} from 'lucide-react';
import { useRouter } from 'next/navigation';
import { Fragment, useEffect, useState } from 'react';

// Status badge colors and icons
const statusConfig = {
   pending: {
      color: 'bg-amber-50 text-amber-700 border-amber-200',
      icon: <Package className="h-4 w-4" />,
      progress: 20,
   },
   processing: {
      color: 'bg-blue-50 text-blue-700 border-blue-200',
      icon: <Package className="h-4 w-4" />,
      progress: 40,
   },
   shipped: {
      color: 'bg-indigo-50 text-indigo-700 border-indigo-200',
      icon: <Truck className="h-4 w-4" />,
      progress: 60,
   },
   delivered: {
      color: 'bg-emerald-50 text-emerald-700 border-emerald-200',
      icon: <Package className="h-4 w-4" />,
      progress: 100,
   },
   cancelled: {
      color: 'bg-rose-50 text-rose-700 border-rose-200',
      icon: <Package className="h-4 w-4" />,
      progress: 100,
   },
};

const OrderDetails = () => {
   const [loading, setLoading] = useState(true);
   const [order, setOrder] = useState(null);
   const router = useRouter();

   const statusInfo = statusConfig.delivered || statusConfig.pending;

   return (
      <Fragment>
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center justify-between">
                     <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                           Order Details
                        </h1>
                        <p className="text-muted-foreground">
                           Order ID: <span className="font-bold">123456</span>
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
                           <div className="divide-y divide-slate-200 dark:divide-slate-700">
                              <div className="p-6 flex items-start gap-4">
                                 <div className="h-16 w-16 bg-slate-100 dark:bg-slate-700 rounded-lg flex items-center justify-center flex-shrink-0">
                                    <svg
                                       xmlns="http://www.w3.org/2000/svg"
                                       width="24"
                                       height="24"
                                       viewBox="0 0 24 24"
                                       fill="none"
                                       stroke="currentColor"
                                       strokeWidth="2"
                                       strokeLinecap="round"
                                       strokeLinejoin="round"
                                       className="text-slate-500"
                                    >
                                       <rect
                                          width="14"
                                          height="20"
                                          x="5"
                                          y="2"
                                          rx="2"
                                          ry="2"
                                       />
                                       <path d="M12 18h.01" />
                                    </svg>
                                 </div>
                                 <div className="flex-1 min-w-0">
                                    <div className="font-medium text-lg">
                                       {'iphone 15-128gb'}
                                    </div>
                                    <div className="text-sm text-muted-foreground mt-1">
                                       {'blue'}
                                    </div>
                                 </div>
                                 <div className="font-medium text-lg">
                                    ${Number(799).toFixed(2)}
                                 </div>
                              </div>
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
                                       <div className="text-sm text-muted-foreground mt-1">
                                          {'123 Main St, New York, NY 10001'}
                                       </div>
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
                                          {'VNPAY'}
                                       </div>
                                    </div>
                                 </div>

                                 <div className="mt-4 pt-4 border-t border-slate-200 dark:border-slate-700">
                                    <div className="flex justify-between text-sm mb-2">
                                       <span className="text-muted-foreground">
                                          Subtotal
                                       </span>
                                       <span>${Number(1290).toFixed(2)}</span>
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
                                       <span>${Number(1290).toFixed(2)}</span>
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
                                          {'John Smith'}
                                       </div>
                                       <div className="text-sm text-muted-foreground mt-1">
                                          Customer since 2022
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
                                          {'john.smith@example.com'}
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
                                          {'+1 (555) 123-4567'}
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
                                    <Badge
                                       className={`${statusInfo.color} border px-2.5 py-1 text-sm font-medium flex items-center gap-1.5`}
                                    >
                                       {statusInfo.icon}
                                       {/* {order.status.charAt(0).toUpperCase() +
                                          order.status.slice(1)} */}
                                    </Badge>
                                 </div>

                                 <div className="mb-6">
                                    <div className="font-medium mb-2">
                                       Update Status
                                    </div>
                                    <Select
                                       defaultValue={'delivered'}
                                       onValueChange={() => {}}
                                    >
                                       <SelectTrigger className="w-full">
                                          <SelectValue placeholder="Select status" />
                                       </SelectTrigger>
                                       <SelectContent>
                                          <SelectItem value="pending">
                                             Pending
                                          </SelectItem>
                                          <SelectItem value="processing">
                                             Processing
                                          </SelectItem>
                                          <SelectItem value="shipped">
                                             Shipped
                                          </SelectItem>
                                          <SelectItem value="delivered">
                                             Delivered
                                          </SelectItem>
                                          <SelectItem value="cancelled">
                                             Cancelled
                                          </SelectItem>
                                       </SelectContent>
                                    </Select>
                                 </div>

                                 <Button className="w-full">
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
