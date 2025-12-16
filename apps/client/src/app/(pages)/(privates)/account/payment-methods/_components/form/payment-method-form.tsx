'use client';

import type React from 'react';

import { useState } from 'react';
import { Button } from '~/components/ui/button';
import { Input } from '~/components/ui/input';
import { Label } from '~/components/ui/label';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '~/components/ui/select';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '~/components/ui/tabs';
import { motion, AnimatePresence } from 'framer-motion';
import { CreditCard, Trash2, AlertCircle } from 'lucide-react';

type PaymentMethod = {
   id: string;
   type: 'credit' | 'debit' | 'paypal' | 'applepay';
   name: string;
   last4: string;
   expiry: string;
   isDefault: boolean;
   cardType?: 'visa' | 'mastercard' | 'amex' | 'discover';
   billingAddress?: {
      line1: string;
      line2?: string;
      city: string;
      state: string;
      zip: string;
      country: string;
   };
};

type PaymentMethodFormProps = {
   initialMethod?: PaymentMethod | null;
   onSubmit: (method: any) => void;
   onDelete?: () => void;
   isLoading?: boolean;
   setOpen: (open: boolean) => void;
};

export function PaymentMethodForm({
   initialMethod,
   onSubmit,
   onDelete,
   isLoading = false,
   setOpen,
}: PaymentMethodFormProps) {
   const [activeTab, setActiveTab] = useState<string>(
      initialMethod?.type || 'credit',
   );
   const [formData, setFormData] = useState({
      // Card details
      type: initialMethod?.type || 'credit',
      name: initialMethod?.name || '',
      cardNumber: initialMethod?.last4
         ? `•••• •••• •••• ${initialMethod.last4}`
         : '',
      expiry: initialMethod?.expiry || '',
      cvc: '',
      cardType: initialMethod?.cardType || 'visa',

      // PayPal
      email: initialMethod?.type === 'paypal' ? initialMethod.name : '',

      // Billing Address
      useSameAddress: true,
      line1: initialMethod?.billingAddress?.line1 || '',
      line2: initialMethod?.billingAddress?.line2 || '',
      city: initialMethod?.billingAddress?.city || '',
      state: initialMethod?.billingAddress?.state || '',
      zip: initialMethod?.billingAddress?.zip || '',
      country: initialMethod?.billingAddress?.country || 'United States',
   });

   const [errors, setErrors] = useState<Record<string, string>>({});
   const [formSubmitted, setFormSubmitted] = useState(false);

   const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const { name, value } = e.target;

      // Format card number with spaces
      if (name === 'cardNumber' && !initialMethod) {
         const digitsOnly = value.replace(/\D/g, '');
         const formattedValue = digitsOnly
            .replace(/(\d{4})/g, '$1 ')
            .trim()
            .slice(0, 19);

         setFormData({ ...formData, [name]: formattedValue });
      } else if (name === 'expiry') {
         // Format expiry date as MM/YY
         const digitsOnly = value.replace(/\D/g, '');
         let formattedValue = digitsOnly;

         if (digitsOnly.length > 2) {
            formattedValue = `${digitsOnly.slice(0, 2)}/${digitsOnly.slice(2, 4)}`;
         }

         setFormData({ ...formData, [name]: formattedValue });
      } else {
         setFormData({ ...formData, [name]: value });
      }

      // Clear error when field is edited
      if (errors[name]) {
         setErrors((prev) => {
            const newErrors = { ...prev };
            delete newErrors[name];
            return newErrors;
         });
      }
   };

   const handleSelectChange = (name: string, value: string) => {
      setFormData({ ...formData, [name]: value });
   };

   const handleTabChange = (value: string) => {
      setActiveTab(value);
      setFormData({
         ...formData,
         type: value as 'credit' | 'debit' | 'paypal' | 'applepay',
      });
   };

   const validateForm = (): boolean => {
      const newErrors: Record<string, string> = {};

      if (activeTab === 'credit' || activeTab === 'debit') {
         if (!formData.name.trim()) {
            newErrors.name = 'Name on card is required';
         }

         if (!initialMethod) {
            if (!formData.cardNumber.trim()) {
               newErrors.cardNumber = 'Card number is required';
            } else if (formData.cardNumber.replace(/\D/g, '').length < 16) {
               newErrors.cardNumber = 'Card number must be 16 digits';
            }

            if (!formData.cvc.trim()) {
               newErrors.cvc = 'Security code is required';
            } else if (formData.cvc.length < 3) {
               newErrors.cvc = 'Security code must be 3 or 4 digits';
            }
         }

         if (!formData.expiry.trim()) {
            newErrors.expiry = 'Expiration date is required';
         } else {
            const [month, year] = formData.expiry.split('/');
            const currentYear = new Date().getFullYear() % 100;
            const currentMonth = new Date().getMonth() + 1;

            if (Number.parseInt(month) < 1 || Number.parseInt(month) > 12) {
               newErrors.expiry = 'Invalid month';
            } else if (
               Number.parseInt(year) < currentYear ||
               (Number.parseInt(year) === currentYear &&
                  Number.parseInt(month) < currentMonth)
            ) {
               newErrors.expiry = 'Card has expired';
            }
         }

         // Validate billing address
         if (!formData.useSameAddress) {
            if (!formData.line1.trim()) {
               newErrors.line1 = 'Address is required';
            }

            if (!formData.city.trim()) {
               newErrors.city = 'City is required';
            }

            if (!formData.state.trim()) {
               newErrors.state = 'State is required';
            }

            if (!formData.zip.trim()) {
               newErrors.zip = 'ZIP code is required';
            }
         }
      } else if (activeTab === 'paypal') {
         if (!formData.email.trim()) {
            newErrors.email = 'Email is required';
         } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
            newErrors.email = 'Email is invalid';
         }
      }

      setErrors(newErrors);
      return Object.keys(newErrors).length === 0;
   };

   const handleSubmit = (e: React.FormEvent) => {
      e.preventDefault();

      if (!validateForm()) {
         return;
      }

      setFormSubmitted(true);

      // Prepare data for submission
      const submitData: any = {
         type: formData.type,
         isDefault: initialMethod?.isDefault || false,
      };

      if (formData.type === 'credit' || formData.type === 'debit') {
         submitData.name = formData.name;
         submitData.cardType = formData.cardType;

         // Extract last 4 digits from card number if it's a new card
         if (!initialMethod) {
            submitData.last4 = formData.cardNumber.replace(/\D/g, '').slice(-4);
         } else {
            submitData.last4 = initialMethod.last4;
         }

         submitData.expiry = formData.expiry;

         // Add billing address if not using same address
         if (!formData.useSameAddress) {
            submitData.billingAddress = {
               line1: formData.line1,
               line2: formData.line2 || undefined,
               city: formData.city,
               state: formData.state,
               zip: formData.zip,
               country: formData.country,
            };
         }
      } else if (formData.type === 'paypal') {
         submitData.name = formData.email;
         submitData.last4 = '';
         submitData.expiry = '';
      } else if (formData.type === 'applepay') {
         submitData.name = 'Apple Pay';
         submitData.last4 = '';
         submitData.expiry = '';
      }

      // If it's an edit, add the ID
      if (initialMethod) {
         submitData.id = initialMethod.id;
      }

      // Simulate form submission with animation
      setTimeout(() => {
         onSubmit(submitData);
         setFormSubmitted(false);
      }, 600);
   };

   // Animation variants for form fields
   const formFieldVariants = {
      hidden: { opacity: 0, y: 20 },
      visible: (custom: number) => ({
         opacity: 1,
         y: 0,
         transition: {
            delay: custom * 0.05,
            duration: 0.3,
            ease: 'easeOut',
         },
      }),
   };

   return (
      <form onSubmit={handleSubmit} className="space-y-4 mt-4">
         <Tabs
            value={activeTab}
            onValueChange={handleTabChange}
            className="w-full"
         >
            <TabsList className="grid grid-cols-3 mb-4">
               <TabsTrigger value="credit" disabled={!!initialMethod}>
                  <CreditCard className="h-4 w-4 mr-2" />
                  Credit Card
               </TabsTrigger>
               <TabsTrigger value="paypal" disabled={!!initialMethod}>
                  <span className="font-bold text-blue-600 mr-2">Pay</span>
                  PayPal
               </TabsTrigger>
               <TabsTrigger value="applepay" disabled={!!initialMethod}>
                  <svg
                     className="h-4 w-4 mr-2"
                     viewBox="0 0 24 24"
                     fill="none"
                     xmlns="http://www.w3.org/2000/svg"
                  >
                     <path
                        d="M18.71 19.5C17.88 20.74 17 21.95 15.66 21.97C14.32 22 13.89 21.18 12.37 21.18C10.84 21.18 10.37 21.95 9.09998 22C7.78998 22.05 6.79998 20.68 5.95998 19.47C4.24998 17 2.93998 12.45 4.69998 9.39C5.56998 7.87 7.12998 6.91 8.81998 6.88C10.1 6.86 11.32 7.75 12.11 7.75C12.89 7.75 14.37 6.68 15.92 6.84C16.57 6.87 18.39 7.1 19.56 8.82C19.47 8.88 17.39 10.1 17.41 12.63C17.44 15.65 20.06 16.66 20.09 16.67C20.06 16.74 19.67 18.11 18.71 19.5ZM13 3.5C13.73 2.67 14.94 2.04 15.94 2C16.07 3.17 15.6 4.35 14.9 5.19C14.21 6.04 13.09 6.7 11.95 6.61C11.8 5.46 12.36 4.26 13 3.5Z"
                        fill="black"
                     />
                  </svg>
                  Apple Pay
               </TabsTrigger>
            </TabsList>

            <TabsContent value="credit" className="space-y-4">
               <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <motion.div
                     className="col-span-2"
                     variants={formFieldVariants}
                     initial="hidden"
                     animate="visible"
                     custom={0}
                  >
                     <Label htmlFor="name">Name on Card</Label>
                     <Input
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        className={`transition-all duration-200 ${errors.name ? 'border-red-300 focus:ring-red-500' : ''}`}
                     />
                     <AnimatePresence>
                        {errors.name && (
                           <motion.p
                              initial={{ opacity: 0, height: 0 }}
                              animate={{ opacity: 1, height: 'auto' }}
                              exit={{ opacity: 0, height: 0 }}
                              className="text-sm text-red-600 flex items-center mt-1"
                           >
                              <AlertCircle className="h-3 w-3 mr-1" />
                              {errors.name}
                           </motion.p>
                        )}
                     </AnimatePresence>
                  </motion.div>

                  <motion.div
                     className="col-span-2"
                     variants={formFieldVariants}
                     initial="hidden"
                     animate="visible"
                     custom={1}
                  >
                     <Label htmlFor="cardNumber">Card Number</Label>
                     <Input
                        id="cardNumber"
                        name="cardNumber"
                        value={formData.cardNumber}
                        onChange={handleChange}
                        placeholder="•••• •••• •••• ••••"
                        disabled={!!initialMethod}
                        className={`transition-all duration-200 ${errors.cardNumber ? 'border-red-300 focus:ring-red-500' : ''}`}
                     />
                     <AnimatePresence>
                        {errors.cardNumber && (
                           <motion.p
                              initial={{ opacity: 0, height: 0 }}
                              animate={{ opacity: 1, height: 'auto' }}
                              exit={{ opacity: 0, height: 0 }}
                              className="text-sm text-red-600 flex items-center mt-1"
                           >
                              <AlertCircle className="h-3 w-3 mr-1" />
                              {errors.cardNumber}
                           </motion.p>
                        )}
                     </AnimatePresence>
                  </motion.div>

                  <motion.div
                     variants={formFieldVariants}
                     initial="hidden"
                     animate="visible"
                     custom={2}
                  >
                     <Label htmlFor="expiry">Expiration Date</Label>
                     <Input
                        id="expiry"
                        name="expiry"
                        value={formData.expiry}
                        onChange={handleChange}
                        placeholder="MM/YY"
                        maxLength={5}
                        className={`transition-all duration-200 ${errors.expiry ? 'border-red-300 focus:ring-red-500' : ''}`}
                     />
                     <AnimatePresence>
                        {errors.expiry && (
                           <motion.p
                              initial={{ opacity: 0, height: 0 }}
                              animate={{ opacity: 1, height: 'auto' }}
                              exit={{ opacity: 0, height: 0 }}
                              className="text-sm text-red-600 flex items-center mt-1"
                           >
                              <AlertCircle className="h-3 w-3 mr-1" />
                              {errors.expiry}
                           </motion.p>
                        )}
                     </AnimatePresence>
                  </motion.div>

                  <motion.div
                     variants={formFieldVariants}
                     initial="hidden"
                     animate="visible"
                     custom={3}
                  >
                     <Label htmlFor="cvc">Security Code</Label>
                     <Input
                        id="cvc"
                        name="cvc"
                        value={formData.cvc}
                        onChange={handleChange}
                        placeholder="CVC"
                        maxLength={4}
                        disabled={!!initialMethod}
                        className={`transition-all duration-200 ${errors.cvc ? 'border-red-300 focus:ring-red-500' : ''}`}
                     />
                     <AnimatePresence>
                        {errors.cvc && (
                           <motion.p
                              initial={{ opacity: 0, height: 0 }}
                              animate={{ opacity: 1, height: 'auto' }}
                              exit={{ opacity: 0, height: 0 }}
                              className="text-sm text-red-600 flex items-center mt-1"
                           >
                              <AlertCircle className="h-3 w-3 mr-1" />
                              {errors.cvc}
                           </motion.p>
                        )}
                     </AnimatePresence>
                  </motion.div>

                  <motion.div
                     variants={formFieldVariants}
                     initial="hidden"
                     animate="visible"
                     custom={4}
                     className="col-span-2"
                  >
                     <Label htmlFor="cardType">Card Type</Label>
                     <Select
                        value={formData.cardType}
                        onValueChange={(value) =>
                           handleSelectChange('cardType', value)
                        }
                     >
                        <SelectTrigger className="transition-all duration-200">
                           <SelectValue placeholder="Select card type" />
                        </SelectTrigger>
                        <SelectContent>
                           <SelectItem value="visa">Visa</SelectItem>
                           <SelectItem value="mastercard">
                              Mastercard
                           </SelectItem>
                           <SelectItem value="amex">
                              American Express
                           </SelectItem>
                           <SelectItem value="discover">Discover</SelectItem>
                        </SelectContent>
                     </Select>
                  </motion.div>
               </div>

               <motion.div
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={5}
                  className="pt-4"
               >
                  <h3 className="text-md font-medium text-gray-900 mb-2">
                     Billing Address
                  </h3>
                  <div className="flex items-center space-x-2 mb-4">
                     <input
                        type="checkbox"
                        id="useSameAddress"
                        checked={formData.useSameAddress}
                        onChange={(e) =>
                           setFormData({
                              ...formData,
                              useSameAddress: e.target.checked,
                           })
                        }
                        className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                     />
                     <Label
                        htmlFor="useSameAddress"
                        className="text-sm text-gray-700"
                     >
                        Use same address as shipping address
                     </Label>
                  </div>

                  <AnimatePresence>
                     {!formData.useSameAddress && (
                        <motion.div
                           initial={{ opacity: 0, height: 0 }}
                           animate={{ opacity: 1, height: 'auto' }}
                           exit={{ opacity: 0, height: 0 }}
                           className="space-y-4"
                        >
                           <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                              <div className="col-span-2">
                                 <Label htmlFor="line1">Address Line 1</Label>
                                 <Input
                                    id="line1"
                                    name="line1"
                                    value={formData.line1}
                                    onChange={handleChange}
                                    className={`transition-all duration-200 ${errors.line1 ? 'border-red-300 focus:ring-red-500' : ''}`}
                                 />
                                 {errors.line1 && (
                                    <p className="text-sm text-red-600 flex items-center mt-1">
                                       <AlertCircle className="h-3 w-3 mr-1" />
                                       {errors.line1}
                                    </p>
                                 )}
                              </div>

                              <div className="col-span-2">
                                 <Label htmlFor="line2">
                                    Address Line 2 (Optional)
                                 </Label>
                                 <Input
                                    id="line2"
                                    name="line2"
                                    value={formData.line2}
                                    onChange={handleChange}
                                    className="transition-all duration-200"
                                 />
                              </div>

                              <div>
                                 <Label htmlFor="city">City</Label>
                                 <Input
                                    id="city"
                                    name="city"
                                    value={formData.city}
                                    onChange={handleChange}
                                    className={`transition-all duration-200 ${errors.city ? 'border-red-300 focus:ring-red-500' : ''}`}
                                 />
                                 {errors.city && (
                                    <p className="text-sm text-red-600 flex items-center mt-1">
                                       <AlertCircle className="h-3 w-3 mr-1" />
                                       {errors.city}
                                    </p>
                                 )}
                              </div>

                              <div>
                                 <Label htmlFor="state">State / Province</Label>
                                 <Input
                                    id="state"
                                    name="state"
                                    value={formData.state}
                                    onChange={handleChange}
                                    className={`transition-all duration-200 ${errors.state ? 'border-red-300 focus:ring-red-500' : ''}`}
                                 />
                                 {errors.state && (
                                    <p className="text-sm text-red-600 flex items-center mt-1">
                                       <AlertCircle className="h-3 w-3 mr-1" />
                                       {errors.state}
                                    </p>
                                 )}
                              </div>

                              <div>
                                 <Label htmlFor="zip">ZIP / Postal Code</Label>
                                 <Input
                                    id="zip"
                                    name="zip"
                                    value={formData.zip}
                                    onChange={handleChange}
                                    className={`transition-all duration-200 ${errors.zip ? 'border-red-300 focus:ring-red-500' : ''}`}
                                 />
                                 {errors.zip && (
                                    <p className="text-sm text-red-600 flex items-center mt-1">
                                       <AlertCircle className="h-3 w-3 mr-1" />
                                       {errors.zip}
                                    </p>
                                 )}
                              </div>

                              <div>
                                 <Label htmlFor="country">Country</Label>
                                 <Select
                                    value={formData.country}
                                    onValueChange={(value) =>
                                       handleSelectChange('country', value)
                                    }
                                 >
                                    <SelectTrigger className="transition-all duration-200">
                                       <SelectValue placeholder="Select country" />
                                    </SelectTrigger>
                                    <SelectContent>
                                       <SelectItem value="United States">
                                          United States
                                       </SelectItem>
                                       <SelectItem value="Canada">
                                          Canada
                                       </SelectItem>
                                       <SelectItem value="United Kingdom">
                                          United Kingdom
                                       </SelectItem>
                                       <SelectItem value="Australia">
                                          Australia
                                       </SelectItem>
                                       <SelectItem value="Japan">
                                          Japan
                                       </SelectItem>
                                       <SelectItem value="Việt Nam">
                                          Việt Nam
                                       </SelectItem>
                                    </SelectContent>
                                 </Select>
                              </div>
                           </div>
                        </motion.div>
                     )}
                  </AnimatePresence>
               </motion.div>
            </TabsContent>

            <TabsContent value="paypal" className="space-y-4">
               <motion.div
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={0}
               >
                  <Label htmlFor="email">PayPal Email</Label>
                  <Input
                     id="email"
                     name="email"
                     type="email"
                     value={formData.email}
                     onChange={handleChange}
                     className={`transition-all duration-200 ${errors.email ? 'border-red-300 focus:ring-red-500' : ''}`}
                  />
                  <AnimatePresence>
                     {errors.email && (
                        <motion.p
                           initial={{ opacity: 0, height: 0 }}
                           animate={{ opacity: 1, height: 'auto' }}
                           exit={{ opacity: 0, height: 0 }}
                           className="text-sm text-red-600 flex items-center mt-1"
                        >
                           <AlertCircle className="h-3 w-3 mr-1" />
                           {errors.email}
                        </motion.p>
                     )}
                  </AnimatePresence>
                  <p className="text-sm text-gray-500 mt-2">
                     You will be redirected to PayPal to complete the setup
                     after submitting this form.
                  </p>
               </motion.div>
            </TabsContent>

            <TabsContent value="applepay" className="space-y-4">
               <div className="text-center py-6">
                  <svg
                     className="h-12 w-12 mx-auto mb-4"
                     viewBox="0 0 24 24"
                     fill="none"
                     xmlns="http://www.w3.org/2000/svg"
                  >
                     <path
                        d="M18.71 19.5C17.88 20.74 17 21.95 15.66 21.97C14.32 22 13.89 21.18 12.37 21.18C10.84 21.18 10.37 21.95 9.09998 22C7.78998 22.05 6.79998 20.68 5.95998 19.47C4.24998 17 2.93998 12.45 4.69998 9.39C5.56998 7.87 7.12998 6.91 8.81998 6.88C10.1 6.86 11.32 7.75 12.11 7.75C12.89 7.75 14.37 6.68 15.92 6.84C16.57 6.87 18.39 7.1 19.56 8.82C19.47 8.88 17.39 10.1 17.41 12.63C17.44 15.65 20.06 16.66 20.09 16.67C20.06 16.74 19.67 18.11 18.71 19.5ZM13 3.5C13.73 2.67 14.94 2.04 15.94 2C16.07 3.17 15.6 4.35 14.9 5.19C14.21 6.04 13.09 6.7 11.95 6.61C11.8 5.46 12.36 4.26 13 3.5Z"
                        fill="black"
                     />
                  </svg>
                  <h3 className="text-lg font-medium text-gray-900">
                     Apple Pay
                  </h3>
                  <p className="text-sm text-gray-500 mt-2">
                     You will be prompted to authenticate with Apple Pay after
                     submitting this form.
                  </p>
               </div>
            </TabsContent>
         </Tabs>

         <div className="flex justify-between pt-4 border-t border-gray-200">
            {onDelete && (
               <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
               >
                  <Button
                     type="button"
                     variant="outline"
                     className="text-red-600 hover:text-red-800 border-red-200 hover:border-red-300 transition-colors duration-200"
                     onClick={onDelete}
                  >
                     <Trash2 className="h-4 w-4 mr-2" />
                     Delete
                  </Button>
               </motion.div>
            )}
            <div className="flex gap-2 ml-auto">
               <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
               >
                  <Button
                     type="button"
                     variant="outline"
                     className="transition-colors duration-200"
                     onClick={() => setOpen(false)}
                  >
                     Cancel
                  </Button>
               </motion.div>
               <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                  animate={
                     formSubmitted || isLoading
                        ? { scale: [1, 0.95, 1.05, 1] }
                        : {}
                  }
                  transition={{ duration: 0.4 }}
               >
                  <Button
                     type="submit"
                     className="bg-blue-600 hover:bg-blue-700 text-white transition-colors duration-200"
                     disabled={formSubmitted || isLoading}
                  >
                     {formSubmitted || isLoading ? (
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
                           Processing...
                        </span>
                     ) : initialMethod ? (
                        'Save Changes'
                     ) : (
                        'Add Payment Method'
                     )}
                  </Button>
               </motion.div>
            </div>
         </div>
      </form>
   );
}
