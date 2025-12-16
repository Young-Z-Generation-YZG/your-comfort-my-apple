'use client';

import { useState } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { AlertCircle, ArrowLeft, HelpCircle } from 'lucide-react';
import { RedirectCountdown } from '~/components/client/countdown-redirect';

// Animation variants
const containerVariants = {
   hidden: { opacity: 0 },
   visible: {
      opacity: 1,
      transition: {
         when: 'beforeChildren',
         staggerChildren: 0.1,
         delayChildren: 0.2,
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

const errorIconVariants = {
   hidden: { scale: 0, opacity: 0 },
   visible: {
      scale: [0, 1.2, 1],
      opacity: 1,
      transition: {
         type: 'spring',
         stiffness: 300,
         damping: 20,
         delay: 0.2,
      },
   },
   pulse: {
      scale: [1, 1.05, 1],
      transition: {
         repeat: Number.POSITIVE_INFINITY,
         repeatType: 'reverse' as const,
         duration: 2,
      },
   },
   shake: {
      x: [0, -10, 10, -10, 10, -5, 5, -2, 2, 0],
      transition: {
         duration: 0.8,
         delay: 0.5,
         repeat: 2,
         repeatDelay: 5,
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

const listItemVariants = {
   hidden: { opacity: 0, x: -20 },
   visible: (custom: number) => ({
      opacity: 1,
      x: 0,
      transition: {
         delay: 0.5 + custom * 0.1,
         type: 'spring',
         stiffness: 300,
         damping: 25,
      },
   }),
   hover: {
      x: 5,
      transition: { type: 'spring', stiffness: 300, damping: 20 },
   },
};

const FailureResult = () => {
   const router = useRouter();
   const [showRedirect, setShowRedirect] = useState(false);

   // Simulate error details
   const errorDetails = {
      errorCode: 'ERR_PAYMENT_DECLINED',
      errorMessage: 'Your payment could not be processed at this time.',
      orderNumber: 'W' + Math.floor(10000000 + Math.random() * 90000000),
   };

   // Handle redirect completion
   const handleRedirectComplete = () => {
      router.push('/checkout');
   };

   // Handle redirect cancellation
   const handleRedirectCancel = () => {
      setShowRedirect(false);
   };

   return (
      <div className="min-h-screen bg-gray-50 text-gray-900 font-sans">
         <main className="container mx-auto px-4 py-8 md:py-12">
            <motion.div
               className="max-w-2xl mx-auto"
               variants={containerVariants}
               initial="hidden"
               animate="visible"
            >
               {/* Error Message */}
               <motion.div
                  className="bg-white rounded-lg shadow-sm p-8 border border-gray-200 text-center mb-8"
                  variants={cardVariants}
                  whileHover="hover"
               >
                  <motion.div
                     className="mb-6 mx-auto"
                     variants={errorIconVariants}
                     initial="hidden"
                     animate={['visible', 'pulse', 'shake']}
                  >
                     <motion.div
                        animate={{
                           boxShadow: [
                              '0 0 0 0 rgba(239, 68, 68, 0)',
                              '0 0 0 20px rgba(239, 68, 68, 0.2)',
                              '0 0 0 40px rgba(239, 68, 68, 0)',
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
                        <AlertCircle className="h-20 w-20 text-red-500 mx-auto" />
                     </motion.div>
                  </motion.div>
                  <motion.h1
                     className="text-3xl font-medium mb-2"
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
                     Payment Unsuccessful
                  </motion.h1>
                  <motion.p
                     className="text-gray-500 mb-2"
                     initial={{ opacity: 0 }}
                     animate={{
                        opacity: 1,
                        transition: { delay: 0.9, duration: 0.5 },
                     }}
                  >
                     {errorDetails.errorMessage}
                  </motion.p>
                  <motion.p
                     className="text-sm text-gray-400 mb-6"
                     initial={{ opacity: 0 }}
                     animate={{
                        opacity: 1,
                        transition: { delay: 1.1, duration: 0.5 },
                     }}
                  >
                     Error Code: {errorDetails.errorCode}
                  </motion.p>
                  <motion.div
                     className="text-sm text-gray-500"
                     initial={{ opacity: 0, scale: 0.9 }}
                     animate={{
                        opacity: 1,
                        scale: 1,
                        transition: {
                           delay: 1.3,
                           type: 'spring',
                           stiffness: 300,
                           damping: 25,
                        },
                     }}
                  >
                     Reference: {errorDetails.orderNumber}
                  </motion.div>
               </motion.div>

               {/* Possible Reasons */}
               <motion.div
                  className="bg-white rounded-lg shadow-sm p-6 border border-gray-200 mb-8"
                  variants={cardVariants}
                  whileHover="hover"
                  initial={{ opacity: 0, y: 30 }}
                  animate={{
                     opacity: 1,
                     y: 0,
                     transition: {
                        delay: 1.5,
                        type: 'spring',
                        stiffness: 300,
                        damping: 25,
                     },
                  }}
               >
                  <h2 className="text-xl font-medium mb-4">Possible Reasons</h2>
                  <ul className="space-y-3 text-gray-600">
                     {[
                        'Your card may have insufficient funds',
                        'The card details entered may be incorrect',
                        'Your bank may have declined the transaction',
                        'There might be a temporary issue with our payment system',
                     ].map((reason, index) => (
                        <motion.li
                           key={index}
                           className="flex items-start"
                           custom={index}
                           variants={listItemVariants}
                           initial="hidden"
                           animate="visible"
                           whileHover="hover"
                        >
                           <motion.span
                              className="flex-shrink-0 h-5 w-5 rounded-full bg-red-100 flex items-center justify-center mr-3"
                              whileHover={{ scale: 1.2 }}
                              transition={{
                                 type: 'spring',
                                 stiffness: 300,
                                 damping: 20,
                              }}
                           >
                              <motion.span
                                 className="h-2 w-2 rounded-full bg-red-500"
                                 animate={{ scale: [1, 1.5, 1] }}
                                 transition={{
                                    repeat: Number.POSITIVE_INFINITY,
                                    repeatDelay: 2 + index,
                                    duration: 0.5,
                                 }}
                              ></motion.span>
                           </motion.span>
                           {reason}
                        </motion.li>
                     ))}
                  </ul>
               </motion.div>

               {/* What to do next */}
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
                  <h2 className="text-xl font-medium mb-4">What to Do Next</h2>
                  <ul className="space-y-3 text-gray-600">
                     {[
                        'Check your card details and try again',
                        'Try a different payment method',
                        'Contact your bank to ensure there are no restrictions on your card',
                        'If the problem persists, contact our support team for assistance',
                     ].map((step, index) => (
                        <motion.li
                           key={index}
                           className="flex items-start"
                           custom={index}
                           variants={listItemVariants}
                           initial="hidden"
                           animate="visible"
                           whileHover="hover"
                        >
                           <motion.span
                              className="flex-shrink-0 h-5 w-5 rounded-full bg-gray-100 flex items-center justify-center mr-3"
                              whileHover={{
                                 backgroundColor: '#f3f4f6',
                                 scale: 1.2,
                              }}
                              transition={{
                                 type: 'spring',
                                 stiffness: 300,
                                 damping: 20,
                              }}
                           >
                              <span className="text-gray-500 text-xs">
                                 {index + 1}
                              </span>
                           </motion.span>
                           {step}
                        </motion.li>
                     ))}
                  </ul>
               </motion.div>

               {/* Action Buttons */}
               <motion.div className="flex flex-col sm:flex-row gap-4">
                  <motion.div
                     className="flex-1"
                     variants={buttonVariants}
                     initial="hidden"
                     animate="visible"
                     whileHover="hover"
                     whileTap="tap"
                     transition={{ delay: 2.1 }}
                  >
                     <Link
                        href="/checkout"
                        className="w-full inline-flex justify-center items-center px-6 py-3 border border-gray-300 shadow-sm text-base font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                     >
                        <motion.div
                           animate={{ x: [0, -5, 0] }}
                           transition={{
                              repeat: Number.POSITIVE_INFINITY,
                              repeatDelay: 3,
                              duration: 0.5,
                           }}
                        >
                           <ArrowLeft className="mr-2 h-4 w-4" />
                        </motion.div>
                        Return to Checkout
                     </Link>
                  </motion.div>
                  <motion.div
                     className="flex-1"
                     variants={buttonVariants}
                     initial="hidden"
                     animate="visible"
                     whileHover="hover"
                     whileTap="tap"
                     transition={{ delay: 2.2 }}
                  >
                     <Link
                        href="/support"
                        className="w-full inline-flex justify-center items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-gray-900 hover:bg-gray-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                     >
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
                           <HelpCircle className="mr-2 h-4 w-4" />
                        </motion.div>
                        Contact Support
                     </Link>
                  </motion.div>
               </motion.div>
            </motion.div>
         </main>

         {/* Auto-redirect countdown */}
         {showRedirect && (
            <RedirectCountdown
               seconds={5}
               onComplete={handleRedirectComplete}
               onCancel={handleRedirectCancel}
            />
         )}
      </div>
   );
};

export default FailureResult;
