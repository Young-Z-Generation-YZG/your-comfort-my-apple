'use client';

import type React from 'react';

import { useState } from 'react';
import { Button } from '@components/ui/button';
import { Input } from '@components/ui/input';
import { Label } from '@components/ui/label';
import { Progress } from '@components/ui/progress';
import { motion, AnimatePresence } from 'framer-motion';
import { Check, Eye, EyeOff, AlertCircle } from 'lucide-react';
import useIdentityService from '../../../../../components/hooks/api/use-identity-service';

// Password strength calculation
const calculatePasswordStrength = (password: string): number => {
   if (!password) return 0;

   let strength = 0;
   // Length check
   if (password.length >= 6) strength += 50;
   // Contains lowercase
   if (/[a-z]/.test(password)) strength += 50;
   // Contains uppercase
   //  if (/[A-Z]/.test(password)) strength += 25;
   // Contains number or special char
   //  if (/[0-9!@#$%^&*(),.?":{}|<>]/.test(password)) strength += 25;

   return strength;
};

const getPasswordStrengthLabel = (strength: number): string => {
   if (strength === 0) return 'None';
   if (strength <= 25) return 'Weak';
   if (strength <= 50) return 'Fair';
   if (strength <= 75) return 'Good';
   return 'Strong';
};

const getPasswordStrengthColor = (strength: number): string => {
   if (strength === 0) return 'bg-gray-200';
   if (strength <= 25) return 'bg-red-500';
   if (strength <= 50) return 'bg-yellow-500';
   if (strength <= 75) return 'bg-blue-500';
   return 'bg-green-500';
};

export function ChangePassword() {
   const { changePasswordAsync, isLoading } = useIdentityService();

   const [currentPassword, setCurrentPassword] = useState('');
   const [newPassword, setNewPassword] = useState('');
   const [confirmPassword, setConfirmPassword] = useState('');
   const [showCurrentPassword, setShowCurrentPassword] = useState(false);
   const [showNewPassword, setShowNewPassword] = useState(false);
   const [showConfirmPassword, setShowConfirmPassword] = useState(false);
   const [formErrors, setFormErrors] = useState<{ [key: string]: string }>({});

   const passwordStrength = calculatePasswordStrength(newPassword);
   const passwordStrengthLabel = getPasswordStrengthLabel(passwordStrength);
   const passwordStrengthColor = getPasswordStrengthColor(passwordStrength);

   // Password requirements
   const requirements = [
      {
         id: 'length',
         label: 'At least 6 characters',
         met: newPassword.length >= 6,
      },
      {
         id: 'lowercase',
         label: 'At least one lowercase letter',
         met: /[a-z]/.test(newPassword),
      },
      // {
      //    id: 'uppercase',
      //    label: 'At least one uppercase letter',
      //    met: /[A-Z]/.test(newPassword),
      // },
      // {
      //    id: 'number-special',
      //    label: 'At least one number or special character',
      //    met: /[0-9!@#$%^&*(),.?":{}|<>]/.test(newPassword),
      // },
   ];

   const handleSubmit = async (e: React.FormEvent) => {
      e.preventDefault();

      // Reset errors
      setFormErrors({});

      // Validate form
      const errors: { [key: string]: string } = {};

      if (!currentPassword) {
         errors.currentPassword = 'Current password is required';
      }

      if (!newPassword) {
         errors.newPassword = 'New password is required';
      } else if (passwordStrength < 75) {
         errors.newPassword = "Password doesn't meet the requirements";
      }

      if (!confirmPassword) {
         errors.confirmPassword = 'Please confirm your new password';
      } else if (newPassword !== confirmPassword) {
         errors.confirmPassword = "Passwords don't match";
      }

      if (Object.keys(errors).length > 0) {
         setFormErrors(errors);
         return;
      }

      const result = await changePasswordAsync({
         old_password: currentPassword,
         new_password: confirmPassword,
      });

      if (result.isSuccess) {
         // Reset form on success
         setCurrentPassword('');
         setNewPassword('');
         setConfirmPassword('');
         setFormErrors({});
      }
   };

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         initial={{ opacity: 0, y: 20 }}
         animate={{ opacity: 1, y: 0 }}
         transition={{ duration: 0.3, delay: 0.1 }}
      >
         <div className="px-6 py-4 border-b border-gray-200">
            <h2 className="text-lg font-medium text-gray-900">
               Change Password
            </h2>
         </div>

         <form onSubmit={handleSubmit} className="p-6 space-y-6">
            <motion.div
               className="space-y-2"
               initial={{ opacity: 0, y: 10 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3 }}
            >
               <Label htmlFor="current-password">Current Password</Label>
               <div className="relative">
                  <Input
                     id="current-password"
                     type={showCurrentPassword ? 'text' : 'password'}
                     value={currentPassword}
                     onChange={(e) => setCurrentPassword(e.target.value)}
                     className={`pr-10 ${formErrors.currentPassword ? 'border-red-300 focus:ring-red-500' : ''}`}
                  />
                  <button
                     type="button"
                     className="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600"
                     onClick={() =>
                        setShowCurrentPassword(!showCurrentPassword)
                     }
                  >
                     {showCurrentPassword ? (
                        <EyeOff className="h-4 w-4" />
                     ) : (
                        <Eye className="h-4 w-4" />
                     )}
                  </button>
               </div>
               <AnimatePresence>
                  {formErrors.currentPassword && (
                     <motion.p
                        initial={{ opacity: 0, height: 0 }}
                        animate={{ opacity: 1, height: 'auto' }}
                        exit={{ opacity: 0, height: 0 }}
                        className="text-sm text-red-600 flex items-center mt-1"
                     >
                        <AlertCircle className="h-3 w-3 mr-1" />
                        {formErrors.currentPassword}
                     </motion.p>
                  )}
               </AnimatePresence>
            </motion.div>

            <motion.div
               className="space-y-2"
               initial={{ opacity: 0, y: 10 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3, delay: 0.1 }}
            >
               <Label htmlFor="new-password">New Password</Label>
               <div className="relative">
                  <Input
                     id="new-password"
                     type={showNewPassword ? 'text' : 'password'}
                     value={newPassword}
                     onChange={(e) => setNewPassword(e.target.value)}
                     className={`pr-10 ${formErrors.newPassword ? 'border-red-300 focus:ring-red-500' : ''}`}
                  />
                  <button
                     type="button"
                     className="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600"
                     onClick={() => setShowNewPassword(!showNewPassword)}
                  >
                     {showNewPassword ? (
                        <EyeOff className="h-4 w-4" />
                     ) : (
                        <Eye className="h-4 w-4" />
                     )}
                  </button>
               </div>
               <AnimatePresence>
                  {formErrors.newPassword && (
                     <motion.p
                        initial={{ opacity: 0, height: 0 }}
                        animate={{ opacity: 1, height: 'auto' }}
                        exit={{ opacity: 0, height: 0 }}
                        className="text-sm text-red-600 flex items-center mt-1"
                     >
                        <AlertCircle className="h-3 w-3 mr-1" />
                        {formErrors.newPassword}
                     </motion.p>
                  )}
               </AnimatePresence>
            </motion.div>

            <motion.div
               className="space-y-2"
               initial={{ opacity: 0, y: 10 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3, delay: 0.2 }}
            >
               <div className="flex justify-between items-center">
                  <Label>Password Strength</Label>
                  <span
                     className="text-xs font-medium"
                     style={{ color: passwordStrengthColor }}
                  >
                     {passwordStrengthLabel}
                  </span>
               </div>
               <Progress value={passwordStrength} className="h-1.5" />

               <div className="mt-4 space-y-2">
                  <p className="text-xs text-gray-500">
                     Your password must include:
                  </p>
                  <ul className="space-y-1">
                     {requirements.map((req) => (
                        <motion.li
                           key={req.id}
                           className="flex items-center text-xs"
                           initial={false}
                           animate={{ color: req.met ? '#10B981' : '#6B7280' }}
                        >
                           <motion.span
                              initial={false}
                              animate={{
                                 backgroundColor: req.met
                                    ? '#10B981'
                                    : '#E5E7EB',
                                 borderColor: req.met ? '#10B981' : '#E5E7EB',
                              }}
                              className="flex-shrink-0 h-4 w-4 mr-2 rounded-full flex items-center justify-center border"
                           >
                              {req.met && (
                                 <Check className="h-3 w-3 text-white" />
                              )}
                           </motion.span>
                           {req.label}
                        </motion.li>
                     ))}
                  </ul>
               </div>
            </motion.div>

            <motion.div
               className="space-y-2"
               initial={{ opacity: 0, y: 10 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3, delay: 0.3 }}
            >
               <Label htmlFor="confirm-password">Confirm New Password</Label>
               <div className="relative">
                  <Input
                     id="confirm-password"
                     type={showConfirmPassword ? 'text' : 'password'}
                     value={confirmPassword}
                     onChange={(e) => setConfirmPassword(e.target.value)}
                     className={`pr-10 ${formErrors.confirmPassword ? 'border-red-300 focus:ring-red-500' : ''}`}
                  />
                  <button
                     type="button"
                     className="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600"
                     onClick={() =>
                        setShowConfirmPassword(!showConfirmPassword)
                     }
                  >
                     {showConfirmPassword ? (
                        <EyeOff className="h-4 w-4" />
                     ) : (
                        <Eye className="h-4 w-4" />
                     )}
                  </button>
               </div>
               <AnimatePresence>
                  {formErrors.confirmPassword && (
                     <motion.p
                        initial={{ opacity: 0, height: 0 }}
                        animate={{ opacity: 1, height: 'auto' }}
                        exit={{ opacity: 0, height: 0 }}
                        className="text-sm text-red-600 flex items-center mt-1"
                     >
                        <AlertCircle className="h-3 w-3 mr-1" />
                        {formErrors.confirmPassword}
                     </motion.p>
                  )}
               </AnimatePresence>

               <AnimatePresence>
                  {confirmPassword && newPassword === confirmPassword && (
                     <motion.p
                        initial={{ opacity: 0, height: 0 }}
                        animate={{ opacity: 1, height: 'auto' }}
                        exit={{ opacity: 0, height: 0 }}
                        className="text-sm text-green-600 flex items-center mt-1"
                     >
                        <Check className="h-3 w-3 mr-1" />
                        Passwords match
                     </motion.p>
                  )}
               </AnimatePresence>
            </motion.div>

            <motion.div
               className="flex justify-end space-x-3 pt-4 border-t border-gray-200"
               initial={{ opacity: 0, y: 10 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3, delay: 0.4 }}
            >
               <Button
                  type="button"
                  variant="outline"
                  onClick={() => {
                     setCurrentPassword('');
                     setNewPassword('');
                     setConfirmPassword('');
                     setFormErrors({});
                  }}
                  disabled={isLoading}
               >
                  Cancel
               </Button>
               <Button
                  type="submit"
                  disabled={
                     isLoading ||
                     !confirmPassword ||
                     newPassword.trim() !== confirmPassword.trim()
                  }
                  className="relative"
               >
                  {isLoading ? (
                     <span className="flex items-center">
                        <svg
                           className="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                           xmlns="http://www.w3.org/2000/svg"
                           fill="none"
                           viewBox="0 0 24 24"
                        >
                           <circle
                              className="opacity-25"
                              cx="12"
                              cy="12"
                              r="10"
                              stroke="currentColor"
                              strokeWidth="4"
                           ></circle>
                           <path
                              className="opacity-75"
                              fill="currentColor"
                              d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                           ></path>
                        </svg>
                        Updating...
                     </span>
                  ) : (
                     'Update Password'
                  )}
               </Button>
            </motion.div>
         </form>
      </motion.div>
   );
}
