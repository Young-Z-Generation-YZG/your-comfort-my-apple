'use client';

import CardWrapper from '~/app/(pages)/(privates)/checkout/_components/card-wrapper';
import { PaymentMethodsList } from './_components/payment-method-list';
import { motion } from 'framer-motion';

const PaymentMethodPage = () => {
   return (
      <CardWrapper>
         <main className="mx-auto max-w-7xl px-4 py-8">
            <motion.div
               initial={{ opacity: 0, y: 20 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3 }}
            >
               <h1 className="text-3xl font-medium text-gray-900">
                  Payment methods
               </h1>
               <p className="mt-1 text-sm text-gray-500">
                  Manage your payment methods and billing information for a
                  seamless checkout experience.
               </p>
            </motion.div>

            <div className="mt-5">
               <PaymentMethodsList />
            </div>
         </main>
      </CardWrapper>
   );
};

export default PaymentMethodPage;
