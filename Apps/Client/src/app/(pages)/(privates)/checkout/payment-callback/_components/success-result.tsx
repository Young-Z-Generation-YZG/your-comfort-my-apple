'use client';

import Link from 'next/link';
import Image from 'next/image';
import { ConfettiAnimation } from '~/components/client/confetti-animation';
import { motion } from 'framer-motion';
import { CheckCircle, ChevronRight, Package, ShoppingBag } from 'lucide-react';
import { RedirectCountdown } from '~/components/client/countdown-redirect';
import { useState } from 'react';
import { OrderDetailsResponse } from '~/domain/interfaces/orders/order.interface';
import { useRouter } from 'next/navigation';

// Animation variants
const containerVariants = {
   hidden: { opacity: 0 },
   visible: {
      opacity: 1,
      transition: {
         when: 'beforeChildren',
         staggerChildren: 0.1,
         delayChildren: 0.3,
      },
   },
};

const itemVariants = {
   hidden: { opacity: 0, y: 20 },
   visible: {
      opacity: 1,
      y: 0,
      transition: { type: 'spring', stiffness: 300, damping: 25 },
   },
};

const checkmarkVariants = {
   hidden: { scale: 0, opacity: 0 },
   visible: {
      scale: [0, 1.2, 1],
      opacity: 1,
      transition: {
         type: 'spring',
         stiffness: 300,
         damping: 20,
         delay: 0.5,
      },
   },
};

const cardVariants = {
   hidden: { opacity: 0, y: 30 },
   visible: {
      opacity: 1,
      y: 0,
      transition: {
         type: 'spring',
         stiffness: 300,
         damping: 25,
      },
   },
   hover: {
      y: -5,
      boxShadow:
         '0 10px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)',
      transition: { type: 'spring', stiffness: 300, damping: 20 },
   },
};

const buttonVariants = {
   hidden: { opacity: 0, y: 20 },
   visible: {
      opacity: 1,
      y: 0,
      transition: { type: 'spring', stiffness: 300, damping: 25 },
   },
   hover: {
      scale: 1.03,
      transition: { type: 'spring', stiffness: 400, damping: 10 },
   },
   tap: {
      scale: 0.97,
      transition: { type: 'spring', stiffness: 400, damping: 10 },
   },
};

const imageVariants = {
   hidden: { opacity: 0, scale: 0.8 },
   visible: {
      opacity: 1,
      scale: 1,
      transition: { type: 'spring', stiffness: 300, damping: 25 },
   },
   hover: {
      scale: 1.1,
      rotate: [0, -5, 5, 0],
      transition: { type: 'spring', stiffness: 300, damping: 10 },
   },
};

const SuccessResult = ({ order }: { order: OrderDetailsResponse | null }) => {
   const router = useRouter();

   const [showRedirect, setShowRedirect] = useState(false);

   // Handle redirect completion
   const handleRedirectComplete = () => {
      router.push('/');
   };

   // Handle redirect cancellation
   const handleRedirectCancel = () => {
      setShowRedirect(false);
   };

   return (
      <div>
         <ConfettiAnimation />
         <main className="container mx-auto px-4 py-8 md:py-12">
            <motion.div
               className="max-w-2xl mx-auto"
               variants={containerVariants}
               initial="hidden"
               animate="visible"
            >
               {/* Success Message */}
               <motion.div
                  className="bg-white rounded-lg shadow-sm p-8 border border-gray-200 text-center mb-8"
                  variants={cardVariants}
                  whileHover="hover"
               >
                  <motion.div
                     className="mb-6 mx-auto"
                     variants={checkmarkVariants}
                  >
                     <motion.div
                        animate={{
                           boxShadow: [
                              '0 0 0 0 rgba(34, 197, 94, 0)',
                              '0 0 0 20px rgba(34, 197, 94, 0.2)',
                              '0 0 0 40px rgba(34, 197, 94, 0)',
                           ],
                        }}
                        transition={{
                           repeat: 3,
                           duration: 2,
                           ease: 'easeInOut',
                           delay: 1,
                        }}
                        className="rounded-full inline-flex"
                     >
                        <CheckCircle className="h-20 w-20 text-green-500 mx-auto" />
                     </motion.div>
                  </motion.div>
                  <motion.h1
                     className="text-3xl font-medium mb-2 font-SFProText"
                     variants={itemVariants}
                     initial={{ opacity: 0, y: 20 }}
                     animate={{
                        opacity: 1,
                        y: 0,
                        transition: {
                           delay: 0.7,
                           type: 'spring',
                           stiffness: 300,
                           damping: 25,
                        },
                     }}
                  >
                     Thank You for Your Order!
                  </motion.h1>
                  <motion.p
                     className="text-gray-500 mb-6 font-SFProText"
                     initial={{ opacity: 0 }}
                     animate={{
                        opacity: 1,
                        transition: { delay: 0.9, duration: 0.5 },
                     }}
                  >
                     Your payment was successful and your order has been placed.
                  </motion.p>
                  <motion.div
                     className="text-lg font-medium mb-1 font-SFProText"
                     initial={{ opacity: 0, scale: 0.9 }}
                     animate={{
                        opacity: 1,
                        scale: 1,
                        transition: {
                           delay: 1.1,
                           type: 'spring',
                           stiffness: 300,
                           damping: 25,
                        },
                     }}
                  >
                     Order Number: #710153
                  </motion.div>
                  <motion.div
                     className="text-gray-500"
                     initial={{ opacity: 0 }}
                     animate={{
                        opacity: 1,
                        transition: { delay: 1.2, duration: 0.5 },
                     }}
                  >
                     {new Date().toLocaleDateString('en-US', {
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric',
                     })}
                  </motion.div>
               </motion.div>

               {/* Order Details */}
               {order && (
                  <motion.div
                     className="bg-white rounded-lg shadow-sm p-6 border border-gray-200 mb-8"
                     variants={cardVariants}
                     whileHover="hover"
                  >
                     <h2 className="text-xl font-medium mb-4">Order Details</h2>

                     {/* Order Items */}

                     <div className="border-b border-gray-200 pb-4 mb-4">
                        {order.order_items.map((item, index) => (
                           <motion.div
                              key={item.product_id}
                              className="flex items-start mb-4"
                              initial={{ opacity: 0, x: -20 }}
                              animate={{
                                 opacity: 1,
                                 x: 0,
                                 transition: {
                                    delay: 1.3 + index * 0.1,
                                    type: 'spring',
                                    stiffness: 300,
                                    damping: 25,
                                 },
                              }}
                           >
                              <motion.div
                                 className="h-16 w-16 flex-shrink-0 overflow-hidden rounded-md border border-gray-200 bg-gray-100"
                                 variants={imageVariants}
                                 whileHover="hover"
                              >
                                 <Image
                                    src={
                                       item.product_image || '/placeholder.svg'
                                    }
                                    alt={item.product_name}
                                    width={64}
                                    height={64}
                                    className="h-full w-full object-contain object-center"
                                 />
                              </motion.div>
                              <div className="ml-4 flex-1">
                                 <h3 className="text-sm font-medium">
                                    {item.product_name} x {item.quantity}
                                 </h3>
                                 <p className="mt-1 text-xs text-gray-500">
                                    {item.product_color_name}
                                 </p>
                              </div>
                              {item?.promotion ? (
                                 <div className="flex gap-2 items-end">
                                    <p className="text-xs font-medium line-through mr-2">
                                       ${item.product_unit_price.toFixed(2)}
                                    </p>
                                    <p className="text-sm font-medium text-red-500 mr-2">
                                       $
                                       {item.promotion.promotion_final_price.toFixed(
                                          2,
                                       )}
                                    </p>
                                 </div>
                              ) : (
                                 <p className="text-sm font-medium">
                                    ${item.product_unit_price.toFixed(2)}
                                 </p>
                              )}
                           </motion.div>
                        ))}
                     </div>

                     {/* Order Summary */}
                     <motion.div
                        className="space-y-2 text-sm"
                        initial={{ opacity: 0 }}
                        animate={{
                           opacity: 1,
                           transition: { delay: 1.5, duration: 0.5 },
                        }}
                     >
                        <div className="flex justify-between">
                           <p className="text-gray-500">Subtotal</p>
                           <p className="font-medium">
                              ${order?.order_sub_total_amount.toFixed(2)}
                           </p>
                        </div>
                        <div className="flex justify-between">
                           <p className="text-gray-500">Shipping</p>
                           <p className="font-medium">Free</p>
                        </div>
                        <div className="flex justify-between">
                           <p className="text-gray-500">Discount</p>
                           <p className="font-medium">
                              ${order?.order_discount_amount.toFixed(2)}
                           </p>
                        </div>
                        <div className="border-t border-gray-200 pt-2 mt-2">
                           <motion.div
                              className="flex justify-between font-medium"
                              initial={{ opacity: 0, scale: 0.95 }}
                              animate={{
                                 opacity: 1,
                                 scale: 1,
                                 transition: {
                                    delay: 1.7,
                                    type: 'spring',
                                    stiffness: 300,
                                    damping: 25,
                                 },
                              }}
                           >
                              <p>Total</p>
                              <p>${order?.order_total_amount.toFixed(2)}</p>
                           </motion.div>
                        </div>
                     </motion.div>
                  </motion.div>
               )}

               {/* Shipping Information */}
               {order && (
                  <motion.div
                     className="bg-white rounded-lg shadow-sm p-6 border border-gray-200 mb-8"
                     variants={cardVariants}
                     whileHover="hover"
                     initial={{ opacity: 0, y: 30 }}
                     animate={{
                        opacity: 1,
                        y: 0,
                        transition: {
                           delay: 1.8,
                           type: 'spring',
                           stiffness: 300,
                           damping: 25,
                        },
                     }}
                  >
                     <div className="flex items-start">
                        <motion.div
                           initial={{ scale: 0, opacity: 0 }}
                           animate={{
                              scale: 1,
                              opacity: 1,
                              transition: {
                                 delay: 2,
                                 type: 'spring',
                                 stiffness: 300,
                                 damping: 25,
                              },
                           }}
                        >
                           <Package className="h-5 w-5 text-gray-400 mt-0.5 mr-3" />
                        </motion.div>
                        <div>
                           <h2 className="text-lg font-medium mb-2">
                              Shipping Information
                           </h2>
                           <p className="mb-1 text-lg font-SFProText">
                              {/* {order.order_shipping_address.contact_name} | {order.order_shipping_address.contact_phone_number} */}
                              {order.order_shipping_address.contact_name} |{' '}
                              {
                                 order.order_shipping_address
                                    .contact_phone_number
                              }
                           </p>
                           <motion.p
                              className="text-base font-SFProText text-gray-700"
                              initial={{ opacity: 0 }}
                              animate={{
                                 opacity: 1,
                                 transition: { delay: 2.2, duration: 0.5 },
                              }}
                           >
                              {
                                 order.order_shipping_address
                                    .contact_address_line
                              }
                              , {order.order_shipping_address.contact_district},{' '}
                              {order.order_shipping_address.contact_province},{' '}
                              {order.order_shipping_address.contact_country}
                              <br />
                              <span className="mt-3 flex">
                                 Delivered in{' '}
                                 <p className="ml-1 mr-1"> 4 days</p>
                                 after payment (
                                 <p className="font-semibold">
                                    {new Date(
                                       Date.now() + 4 * 24 * 60 * 60 * 1000,
                                    ).toLocaleDateString('en-US', {
                                       month: 'long',
                                       day: 'numeric',
                                    })}
                                 </p>
                                 )
                              </span>
                           </motion.p>
                        </div>
                     </div>
                  </motion.div>
               )}

               {/* Action Buttons */}
               <motion.div className="flex flex-col sm:flex-row gap-4">
                  <motion.div
                     className="flex-1"
                     variants={buttonVariants}
                     initial="hidden"
                     animate="visible"
                     whileHover="hover"
                     whileTap="tap"
                     transition={{ delay: 2.3 }}
                  >
                     <Link
                        href="/orders"
                        className="w-full inline-flex justify-center items-center px-6 py-3 border border-gray-300 shadow-sm text-base font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                     >
                        View Order
                        <motion.div
                           animate={{ x: [0, 5, 0] }}
                           transition={{
                              repeat: Number.POSITIVE_INFINITY,
                              repeatDelay: 3,
                              duration: 0.5,
                           }}
                        >
                           <ChevronRight className="ml-2 h-4 w-4" />
                        </motion.div>
                     </Link>
                  </motion.div>
                  <motion.div
                     className="flex-1"
                     variants={buttonVariants}
                     initial="hidden"
                     animate="visible"
                     whileHover="hover"
                     whileTap="tap"
                     transition={{ delay: 2.4 }}
                  >
                     <Link
                        href="/"
                        className="w-full inline-flex justify-center items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-gray-900 hover:bg-gray-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                     >
                        Continue Shopping
                        <motion.div
                           animate={{
                              rotate: [0, 10, -10, 0],
                           }}
                           transition={{
                              repeat: Number.POSITIVE_INFINITY,
                              repeatDelay: 4,
                              duration: 0.6,
                           }}
                        >
                           <ShoppingBag className="ml-2 h-4 w-4" />
                        </motion.div>
                     </Link>
                  </motion.div>
               </motion.div>
            </motion.div>
         </main>

         {showRedirect && (
            <RedirectCountdown
               seconds={10}
               onComplete={handleRedirectComplete}
               onCancel={handleRedirectCancel}
            />
         )}
      </div>
   );
};

export default SuccessResult;
