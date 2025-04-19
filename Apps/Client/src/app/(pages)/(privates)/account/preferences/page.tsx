'use client';

import CardWrapper from '~/app/(pages)/(privates)/checkout/_components/card-wrapper';
import { PreferencesForm } from './_components/form/preference-form';
import { motion } from 'framer-motion';

const PreferencePage = () => {
   return (
      <CardWrapper>
         <main className="mx-auto max-w-7xl px-4 py-8">
            <motion.div
               initial={{ opacity: 0, y: 20 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3 }}
            >
               <h1 className="text-3xl font-medium text-gray-900">
                  Preferences
               </h1>
               <p className="mt-1 text-sm text-gray-500">
                  Manage your preferences and settings for a better experience.
               </p>
            </motion.div>

            <div className="mt-5">
               <PreferencesForm />
            </div>
         </main>
      </CardWrapper>
   );
};

export default PreferencePage;
