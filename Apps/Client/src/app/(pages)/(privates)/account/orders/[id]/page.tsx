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
   RefreshCw,
} from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';

// Order type definition
type OrderItem = {
   id: string;
   name: string;
   image: string;
   price: string;
   quantity: number;
   options?: string;
};

type OrderDetails = {
   id: string;
   orderNumber: string;
   date: string;
   status: 'processing' | 'shipped' | 'delivered' | 'canceled';
   total: string;
   subtotal: string;
   tax: string;
   shipping: string;
   items: OrderItem[];
   shippingAddress: {
      name: string;
      street: string;
      city: string;
      state: string;
      zip: string;
      country: string;
   };
   paymentMethod: string;
   trackingNumber?: string;
   estimatedDelivery?: string;
};

// Sample order details
const sampleOrderDetails: Record<string, OrderDetails> = {
   '1': {
      id: '1',
      orderNumber: 'W12345678',
      date: 'Apr 12, 2023',
      status: 'delivered',
      total: '$2,399.00',
      subtotal: '$2,199.00',
      tax: '$175.92',
      shipping: '$24.08',
      items: [
         {
            id: '1',
            name: 'MacBook Air 13-inch',
            image: '/placeholder.svg?height=80&width=80',
            price: '$1,199.00',
            quantity: 1,
            options: 'M2 chip, 8GB RAM, 256GB SSD, Space Gray',
         },
         {
            id: '2',
            name: 'Apple AirPods Pro',
            image: '/placeholder.svg?height=80&width=80',
            price: '$249.00',
            quantity: 1,
         },
         {
            id: '3',
            name: 'AppleCare+ for MacBook Air',
            image: '/placeholder.svg?height=80&width=80',
            price: '$199.00',
            quantity: 1,
         },
      ],
      shippingAddress: {
         name: 'John Doe',
         street: '123 Apple Street',
         city: 'Cupertino',
         state: 'CA',
         zip: '95014',
         country: 'United States',
      },
      paymentMethod: 'Visa ending in 4242',
      trackingNumber: '1Z999AA10123456784',
      estimatedDelivery: 'Delivered on Apr 15, 2023',
   },
   '2': {
      id: '2',
      orderNumber: 'W12345679',
      date: 'Mar 28, 2023',
      status: 'shipped',
      total: '$129.00',
      subtotal: '$119.00',
      tax: '$9.52',
      shipping: '$0.48',
      items: [
         {
            id: '1',
            name: 'Apple Pencil (2nd Generation)',
            image: '/placeholder.svg?height=80&width=80',
            price: '$129.00',
            quantity: 1,
         },
      ],
      shippingAddress: {
         name: 'John Doe',
         street: '123 Apple Street',
         city: 'Cupertino',
         state: 'CA',
         zip: '95014',
         country: 'United States',
      },
      paymentMethod: 'Visa ending in 4242',
      trackingNumber: '1Z999AA10123456785',
      estimatedDelivery: 'Expected delivery on Apr 18, 2023',
   },
   '8': {
      id: '8',
      orderNumber: 'W12345685',
      date: 'Apr 15, 2023',
      status: 'processing',
      total: '$1,599.00',
      subtotal: '$1,499.00',
      tax: '$119.92',
      shipping: '$0.00',
      items: [
         {
            id: '1',
            name: 'iPhone 14 Pro',
            image: '/placeholder.svg?height=80&width=80',
            price: '$999.00',
            quantity: 1,
            options: '128GB, Deep Purple',
         },
         {
            id: '2',
            name: 'iPhone 14 Pro Leather Case',
            image: '/placeholder.svg?height=80&width=80',
            price: '$59.00',
            quantity: 1,
            options: 'Deep Purple',
         },
         {
            id: '3',
            name: 'AppleCare+ for iPhone 14 Pro',
            image: '/placeholder.svg?height=80&width=80',
            price: '$199.00',
            quantity: 1,
         },
      ],
      shippingAddress: {
         name: 'John Doe',
         street: '123 Apple Street',
         city: 'Cupertino',
         state: 'CA',
         zip: '95014',
         country: 'United States',
      },
      paymentMethod: 'Visa ending in 4242',
      estimatedDelivery: 'Expected to ship in 1-2 business days',
   },
};

type OrderDetailsProps = {
   orderId: string;
   onBack: () => void;
};

export default function OrderDetails({ orderId, onBack }: OrderDetailsProps) {
   const [order, setOrder] = useState<OrderDetails | null>(null);
   const [isLoading, setIsLoading] = useState(true);

   // Simulate loading data
   useEffect(() => {
      const timer = setTimeout(() => {
         setOrder(sampleOrderDetails[orderId] || null);
         setIsLoading(false);
      }, 800);

      return () => clearTimeout(timer);
   }, [orderId]);

   // Get status icon
   const getStatusIcon = (status: OrderDetails['status']) => {
      switch (status) {
         case 'processing':
            return <Package className="h-5 w-5 text-yellow-500" />;
         case 'shipped':
            return <Truck className="h-5 w-5 text-blue-500" />;
         case 'delivered':
            return <CheckCircle className="h-5 w-5 text-green-500" />;
         case 'canceled':
            return <AlertCircle className="h-5 w-5 text-red-500" />;
         default:
            return null;
      }
   };

   // Get status color
   const getStatusColor = (status: OrderDetails['status']) => {
      switch (status) {
         case 'processing':
            return 'bg-yellow-100 text-yellow-800';
         case 'shipped':
            return 'bg-blue-100 text-blue-800';
         case 'delivered':
            return 'bg-green-100 text-green-800';
         case 'canceled':
            return 'bg-red-100 text-red-800';
         default:
            return 'bg-gray-100 text-gray-800';
      }
   };

   // Animation variants
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

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         variants={containerVariants}
         initial="hidden"
         animate="visible"
      >
         <div className="flex items-center px-6 py-4 border-b border-gray-200">
            <motion.button
               onClick={onBack}
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
               >
                  <RefreshCw className="mr-2 h-4 w-4" />
                  Reorder
               </Button>
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
                              {order.orderNumber}
                           </span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">
                              Date Placed
                           </span>
                           <span className="text-sm font-medium">
                              {order.date}
                           </span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-sm text-gray-500">Status</span>
                           <Badge
                              className={`${getStatusColor(order.status)} capitalize flex items-center gap-1`}
                           >
                              {getStatusIcon(order.status)}
                              {order.status}
                           </Badge>
                        </div>
                        {order.trackingNumber && (
                           <div className="flex justify-between">
                              <span className="text-sm text-gray-500">
                                 Tracking Number
                              </span>
                              <span className="text-sm font-medium text-blue-600 hover:underline cursor-pointer">
                                 {order.trackingNumber}
                              </span>
                           </div>
                        )}
                        {order.estimatedDelivery && (
                           <div className="flex justify-between">
                              <span className="text-sm text-gray-500">
                                 Delivery
                              </span>
                              <span className="text-sm font-medium">
                                 {order.estimatedDelivery}
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
                           {order.shippingAddress.name}
                        </p>
                        <p className="text-sm text-gray-500">
                           {order.shippingAddress.street}
                        </p>
                        <p className="text-sm text-gray-500">
                           {order.shippingAddress.city},{' '}
                           {order.shippingAddress.state}{' '}
                           {order.shippingAddress.zip}
                        </p>
                        <p className="text-sm text-gray-500">
                           {order.shippingAddress.country}
                        </p>
                     </div>

                     <h3 className="text-sm font-medium text-gray-900 mt-4 mb-2">
                        Payment Method
                     </h3>
                     <div className="bg-gray-50 rounded-lg p-4">
                        <p className="text-sm font-medium">
                           {order.paymentMethod}
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
                           {order.items.map((item, index) => (
                              <motion.div
                                 key={item.id}
                                 className="p-4 flex items-center"
                                 variants={itemVariants}
                                 initial="hidden"
                                 animate="visible"
                                 custom={index}
                              >
                                 <div className="flex-shrink-0 w-20 h-20 bg-gray-100 rounded-md overflow-hidden">
                                    <img
                                       src={item.image || '/placeholder.svg'}
                                       alt={item.name}
                                       className="w-full h-full object-center object-cover"
                                    />
                                 </div>
                                 <div className="ml-4 flex-1">
                                    <h4 className="text-sm font-medium text-gray-900">
                                       {item.name}
                                    </h4>
                                    {item.options && (
                                       <p className="mt-1 text-xs text-gray-500">
                                          {item.options}
                                       </p>
                                    )}
                                    <div className="mt-1 flex justify-between">
                                       <p className="text-sm text-gray-500">
                                          Qty {item.quantity}
                                       </p>
                                       <p className="text-sm font-medium text-gray-900">
                                          {item.price}
                                       </p>
                                    </div>
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
                           {order.subtotal}
                        </span>
                     </div>
                     <div className="flex justify-between">
                        <span className="text-sm text-gray-500">Shipping</span>
                        <span className="text-sm font-medium">
                           {order.shipping}
                        </span>
                     </div>
                     <div className="flex justify-between">
                        <span className="text-sm text-gray-500">Tax</span>
                        <span className="text-sm font-medium">{order.tax}</span>
                     </div>
                     <Separator />
                     <div className="flex justify-between">
                        <span className="text-sm font-medium">Total</span>
                        <span className="text-sm font-medium">
                           {order.total}
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
                     onClick={onBack}
                     className="text-gray-600"
                  >
                     <ArrowLeft className="mr-2 h-4 w-4" />
                     Back to Orders
                  </Button>
                  <div className="space-x-2">
                     <Button
                        variant="outline"
                        className="text-blue-600 hover:text-blue-800"
                     >
                        Get Help
                     </Button>
                     <Button className="bg-blue-600 hover:bg-blue-700 text-white">
                        <RefreshCw className="mr-2 h-4 w-4" />
                        Reorder
                     </Button>
                  </div>
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
                  <Button onClick={onBack}>Back to Orders</Button>
               </div>
            </motion.div>
         )}
      </motion.div>
   );
}
