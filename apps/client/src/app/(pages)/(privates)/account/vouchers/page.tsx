'use client';

import { VouchersList } from './_components/voucher-list';
import { motion } from 'framer-motion';
import CardWrapper from '~/app/(pages)/(privates)/checkout/_components/card-wrapper';

const VoucherPage = () => {
   return (
      <CardWrapper>
         <main className="mx-auto max-w-7xl px-4 py-8">
            <motion.div
               initial={{ opacity: 0, y: 20 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3 }}
            >
               <h1 className="text-3xl font-medium text-gray-900">Voucher</h1>
               <p className="mt-1 text-sm text-gray-500">
                  Here you can find all your vouchers. You can use them to get
                  discounts on your next purchase.
               </p>
            </motion.div>

            <div className="mt-5">
               <VouchersList />
            </div>
         </main>
      </CardWrapper>
   );
};

export default VoucherPage;
