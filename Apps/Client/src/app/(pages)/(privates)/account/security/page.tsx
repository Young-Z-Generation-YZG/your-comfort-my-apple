'use client';

import { useState } from 'react';
import CardWrapper from '~/app/(pages)/checkout/_components/card-wrapper';
import { ChangePassword } from './change-password';
import { TwoFactorAuth } from './two-factor-auth';
import { LoginHistory } from './login-history';
import { SecuritySettings } from './security-settings';
import { motion } from 'framer-motion';

const SecurityPage = () => {
   const [activeSection, setActiveSection] = useState<string>('password');

   return (
      <CardWrapper>
         <main className="mx-auto max-w-7xl px-4 py-8">
            <motion.div
               initial={{ opacity: 0, y: 20 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3 }}
            >
               <h1 className="text-3xl font-medium text-gray-900">Security</h1>
               <p className="mt-1 text-sm text-gray-500">
                  Increase the security of your account by managing your
                  password, two-factor authentication, and login history.
               </p>
            </motion.div>

            <div className="mt-8">
               <div className="space-y-6">
                  <SecuritySettings
                     activeSection={activeSection}
                     setActiveSection={setActiveSection}
                  />

                  {activeSection === 'password' && <ChangePassword />}
                  {activeSection === 'two-factor' && <TwoFactorAuth />}
                  {activeSection === 'activity' && <LoginHistory />}
               </div>
            </div>
         </main>
      </CardWrapper>
   );
};

export default SecurityPage;
