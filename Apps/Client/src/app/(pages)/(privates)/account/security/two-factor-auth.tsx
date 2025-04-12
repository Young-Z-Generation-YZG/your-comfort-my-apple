'use client';

import { useState } from 'react';
import { Button } from '@components/ui/button';
import { Switch } from '@components/ui/switch';
import { Label } from '@components/ui/label';
import { motion } from 'framer-motion';
import { Smartphone, Mail, Shield } from 'lucide-react';

export function TwoFactorAuth() {
   const [twoFactorEnabled, setTwoFactorEnabled] = useState(false);
   const [preferredMethod, setPreferredMethod] = useState<string>('sms');

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         initial={{ opacity: 0, y: 20 }}
         animate={{ opacity: 1, y: 0 }}
         transition={{ duration: 0.3, delay: 0.1 }}
      >
         <div className="px-6 py-4 border-b border-gray-200">
            <h2 className="text-lg font-medium text-gray-900">
               Two-Factor Authentication
            </h2>
         </div>

         <div className="p-6 space-y-6">
            <div className="flex items-center justify-between">
               <div className="flex items-start space-x-3">
                  <div className="flex-shrink-0 p-1.5 rounded-full bg-blue-100 text-blue-600">
                     <Shield className="h-5 w-5" />
                  </div>
                  <div>
                     <h3 className="text-sm font-medium text-gray-900">
                        Two-Factor Authentication
                     </h3>
                     <p className="mt-1 text-sm text-gray-500">
                        Add an extra layer of security to your account by
                        requiring a verification code in addition to your
                        password.
                     </p>
                  </div>
               </div>
               <Switch
                  checked={twoFactorEnabled}
                  onCheckedChange={setTwoFactorEnabled}
               />
            </div>

            {twoFactorEnabled && (
               <motion.div
                  className="mt-6 space-y-4 bg-gray-50 p-4 rounded-lg"
                  initial={{ opacity: 0, height: 0 }}
                  animate={{ opacity: 1, height: 'auto' }}
                  transition={{ duration: 0.3 }}
               >
                  <h4 className="text-sm font-medium text-gray-900">
                     Verification Method
                  </h4>
                  <p className="text-sm text-gray-500">
                     Choose how you want to receive verification codes.
                  </p>

                  <div className="space-y-3 mt-4">
                     <div className="flex items-center space-x-3">
                        <input
                           type="radio"
                           id="sms"
                           name="verification-method"
                           value="sms"
                           checked={preferredMethod === 'sms'}
                           onChange={() => setPreferredMethod('sms')}
                           className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300"
                        />
                        <div className="flex items-center">
                           <Smartphone className="h-5 w-5 text-gray-400 mr-2" />
                           <Label
                              htmlFor="sms"
                              className="text-sm text-gray-900"
                           >
                              Text Message (SMS)
                           </Label>
                        </div>
                     </div>

                     <div className="flex items-center space-x-3">
                        <input
                           type="radio"
                           id="email"
                           name="verification-method"
                           value="email"
                           checked={preferredMethod === 'email'}
                           onChange={() => setPreferredMethod('email')}
                           className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300"
                        />
                        <div className="flex items-center">
                           <Mail className="h-5 w-5 text-gray-400 mr-2" />
                           <Label
                              htmlFor="email"
                              className="text-sm text-gray-900"
                           >
                              Email
                           </Label>
                        </div>
                     </div>
                  </div>

                  <div className="pt-4 border-t border-gray-200 mt-4">
                     <Button>Set Up Two-Factor Authentication</Button>
                  </div>
               </motion.div>
            )}

            <div className="bg-yellow-50 border border-yellow-100 rounded-lg p-4 mt-6">
               <h4 className="text-sm font-medium text-yellow-800">
                  Important
               </h4>
               <p className="mt-1 text-sm text-yellow-700">
                  If you lose access to your two-factor authentication method,
                  you may lose access to your account. Make sure to set up
                  recovery options.
               </p>
            </div>
         </div>
      </motion.div>
   );
}
