'use client';

import { useState } from 'react';
import { Button } from '~/components/ui/button';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '~/components/ui/dialog';
import { motion, AnimatePresence } from 'framer-motion';
import { useToast } from '~/hooks/use-toast';
import {
   Plus,
   CreditCard,
   Trash2,
   Check,
   Edit,
   AlertCircle,
} from 'lucide-react';
import { PaymentMethodForm } from './form/payment-method-form';

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

export function PaymentMethodsList() {
   const { toast } = useToast();
   const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([
      {
         id: '1',
         type: 'credit',
         name: 'John Doe',
         last4: '4242',
         expiry: '12/25',
         isDefault: true,
         cardType: 'visa',
         billingAddress: {
            line1: '123 Apple Street',
            city: 'Cupertino',
            state: 'CA',
            zip: '95014',
            country: 'United States',
         },
      },
      {
         id: '2',
         type: 'credit',
         name: 'John Doe',
         last4: '5555',
         expiry: '10/24',
         isDefault: false,
         cardType: 'mastercard',
         billingAddress: {
            line1: '456 Tech Avenue',
            city: 'San Francisco',
            state: 'CA',
            zip: '94105',
            country: 'United States',
         },
      },
      {
         id: '3',
         type: 'paypal',
         name: 'john.doe@example.com',
         last4: '',
         expiry: '',
         isDefault: false,
      },
   ]);

   const [open, setOpen] = useState(false);
   const [editingPaymentMethod, setEditingPaymentMethod] =
      useState<PaymentMethod | null>(null);
   const [recentlySetDefault, setRecentlySetDefault] = useState<string | null>(
      null,
   );
   const [isAddingNew, setIsAddingNew] = useState(false);
   const [confirmDeleteId, setConfirmDeleteId] = useState<string | null>(null);

   const handleSetDefault = (id: string) => {
      setRecentlySetDefault(id);

      setTimeout(() => {
         setPaymentMethods(
            paymentMethods.map((method) => ({
               ...method,
               isDefault: method.id === id,
            })),
         );

         toast({
            title: 'Default payment method updated',
            description: 'Your default payment method has been updated.',
            duration: 3000,
         });

         // Reset the highlight after animation completes
         setTimeout(() => setRecentlySetDefault(null), 1500);
      }, 300);
   };

   const handleAddPaymentMethod = (
      newMethod: Omit<PaymentMethod, 'id' | 'isDefault'>,
   ) => {
      setIsAddingNew(true);

      setTimeout(() => {
         const id = Math.random().toString(36).substring(2, 9);
         setPaymentMethods([
            ...paymentMethods,
            {
               ...newMethod,
               id,
               isDefault: paymentMethods.length === 0, // Make default if it's the first one
            },
         ]);
         setOpen(false);
         setIsAddingNew(false);

         toast({
            title: 'Payment method added',
            description: 'Your new payment method has been added.',
            duration: 3000,
         });
      }, 500);
   };

   const handleEditPaymentMethod = (updatedMethod: PaymentMethod) => {
      setPaymentMethods(
         paymentMethods.map((method) =>
            method.id === updatedMethod.id ? updatedMethod : method,
         ),
      );
      setEditingPaymentMethod(null);
      setOpen(false);

      toast({
         title: 'Payment method updated',
         description: 'Your payment method has been updated.',
         duration: 3000,
      });
   };

   const handleDeletePaymentMethod = (id: string) => {
      const methodToDelete = paymentMethods.find((method) => method.id === id);
      const wasDefault = methodToDelete?.isDefault || false;

      const filteredMethods = paymentMethods.filter(
         (method) => method.id !== id,
      );

      // If we deleted the default method and there are other methods, make the first one default
      if (wasDefault && filteredMethods.length > 0) {
         filteredMethods[0].isDefault = true;
      }

      setPaymentMethods(filteredMethods);
      setEditingPaymentMethod(null);
      setOpen(false);
      setConfirmDeleteId(null);

      toast({
         title: 'Payment method deleted',
         description: 'Your payment method has been removed.',
         duration: 3000,
      });
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
      exit: {
         opacity: 0,
         x: -300,
         transition: {
            duration: 0.3,
            ease: 'easeInOut',
         },
      },
      highlight: {
         backgroundColor: [
            'rgba(59, 130, 246, 0)',
            'rgba(59, 130, 246, 0.1)',
            'rgba(59, 130, 246, 0)',
         ],
         transition: { duration: 1.5, times: [0, 0.2, 1] },
      },
   };

   const getCardIcon = (cardType?: string) => {
      switch (cardType) {
         case 'visa':
            return (
               <div className="h-8 w-12 bg-blue-600 rounded-md flex items-center justify-center text-white font-bold text-xs">
                  VISA
               </div>
            );
         case 'mastercard':
            return (
               <div className="h-8 w-12 bg-red-500 rounded-md flex items-center justify-center text-white font-bold text-xs">
                  MC
               </div>
            );
         case 'amex':
            return (
               <div className="h-8 w-12 bg-blue-400 rounded-md flex items-center justify-center text-white font-bold text-xs">
                  AMEX
               </div>
            );
         case 'discover':
            return (
               <div className="h-8 w-12 bg-orange-500 rounded-md flex items-center justify-center text-white font-bold text-xs">
                  DISC
               </div>
            );
         default:
            return <CreditCard className="h-8 w-8 text-gray-400" />;
      }
   };

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         variants={containerVariants}
         initial="hidden"
         animate="visible"
      >
         <div className="flex items-center justify-between px-6 py-4 border-b border-gray-200">
            <h2 className="text-lg font-medium text-gray-900">
               Payment Methods
            </h2>
            <Dialog open={open} onOpenChange={setOpen}>
               <DialogTrigger asChild>
                  <motion.div
                     whileHover={{ scale: 1.05 }}
                     whileTap={{ scale: 0.95 }}
                  >
                     <Button
                        variant="outline"
                        className="text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors duration-200"
                        onClick={() => setEditingPaymentMethod(null)}
                     >
                        <Plus className="h-4 w-4 mr-1" />
                        Add Payment Method
                     </Button>
                  </motion.div>
               </DialogTrigger>
               <DialogContent className="sm:max-w-[500px]">
                  <DialogHeader>
                     <DialogTitle>
                        {editingPaymentMethod
                           ? 'Edit Payment Method'
                           : 'Add Payment Method'}
                     </DialogTitle>
                  </DialogHeader>
                  <PaymentMethodForm
                     initialMethod={editingPaymentMethod}
                     setOpen={setOpen}
                     onSubmit={
                        editingPaymentMethod
                           ? handleEditPaymentMethod
                           : handleAddPaymentMethod
                     }
                     onDelete={
                        editingPaymentMethod
                           ? () => setConfirmDeleteId(editingPaymentMethod.id)
                           : undefined
                     }
                     isLoading={isAddingNew}
                  />
               </DialogContent>
            </Dialog>
         </div>

         <motion.div
            variants={containerVariants}
            className="divide-y divide-gray-200"
         >
            <AnimatePresence>
               {paymentMethods.map((method) => (
                  <motion.div
                     key={method.id}
                     variants={itemVariants}
                     initial="hidden"
                     animate={
                        method.id === recentlySetDefault
                           ? 'highlight'
                           : 'visible'
                     }
                     exit="exit"
                     className="px-6 py-4"
                  >
                     <div className="flex items-center justify-between">
                        <div className="flex items-center">
                           {method.type === 'paypal' ? (
                              <div className="h-8 w-12 bg-blue-700 rounded-md flex items-center justify-center text-white font-bold text-xs">
                                 PayPal
                              </div>
                           ) : method.type === 'applepay' ? (
                              <div className="h-8 w-12 bg-black rounded-md flex items-center justify-center text-white font-bold text-xs">
                                 Apple
                              </div>
                           ) : (
                              getCardIcon(method.cardType)
                           )}
                           <div className="ml-4">
                              <div className="flex items-center">
                                 <h3 className="text-sm font-medium text-gray-900">
                                    {method.type === 'paypal'
                                       ? `PayPal (${method.name})`
                                       : method.type === 'applepay'
                                         ? 'Apple Pay'
                                         : `${method.cardType?.toUpperCase() || 'Card'} •••• ${method.last4}`}
                                 </h3>
                                 <AnimatePresence>
                                    {method.isDefault && (
                                       <motion.span
                                          initial={{ scale: 0.8, opacity: 0 }}
                                          animate={{ scale: 1, opacity: 1 }}
                                          exit={{ scale: 0.8, opacity: 0 }}
                                          className="ml-2 inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800"
                                       >
                                          <motion.span
                                             initial={{ opacity: 0, scale: 0 }}
                                             animate={{ opacity: 1, scale: 1 }}
                                             transition={{ delay: 0.2 }}
                                          >
                                             <Check className="h-3 w-3 mr-1" />
                                          </motion.span>
                                          Default
                                       </motion.span>
                                    )}
                                 </AnimatePresence>
                              </div>
                              {method.type !== 'paypal' &&
                                 method.type !== 'applepay' && (
                                    <p className="text-sm text-gray-500">
                                       Expires {method.expiry}
                                    </p>
                                 )}
                           </div>
                        </div>
                        <div className="flex space-x-2">
                           {!method.isDefault && (
                              <motion.div
                                 whileHover={{ scale: 1.05 }}
                                 whileTap={{ scale: 0.95 }}
                              >
                                 <Button
                                    variant="ghost"
                                    size="sm"
                                    className="text-blue-600 hover:text-blue-800 hover:bg-blue-50"
                                    onClick={() => handleSetDefault(method.id)}
                                 >
                                    Set as Default
                                 </Button>
                              </motion.div>
                           )}
                           <motion.div
                              whileHover={{ scale: 1.05 }}
                              whileTap={{ scale: 0.95 }}
                           >
                              <Button
                                 variant="ghost"
                                 size="sm"
                                 className="text-gray-600 hover:text-gray-800 hover:bg-gray-50"
                                 onClick={() => {
                                    setEditingPaymentMethod(method);
                                    setOpen(true);
                                 }}
                              >
                                 <Edit className="h-4 w-4 mr-1" />
                                 Edit
                              </Button>
                           </motion.div>
                           <motion.div
                              whileHover={{ scale: 1.05 }}
                              whileTap={{ scale: 0.95 }}
                           >
                              <Button
                                 variant="ghost"
                                 size="sm"
                                 className="text-red-600 hover:text-red-800 hover:bg-red-50"
                                 onClick={() => setConfirmDeleteId(method.id)}
                                 disabled={
                                    method.isDefault &&
                                    paymentMethods.length > 1
                                 }
                              >
                                 <Trash2 className="h-4 w-4 mr-1" />
                                 Delete
                              </Button>
                           </motion.div>
                        </div>
                     </div>

                     {method.billingAddress && (
                        <motion.div
                           initial={{ opacity: 0, height: 0 }}
                           animate={{ opacity: 1, height: 'auto' }}
                           transition={{ duration: 0.3 }}
                           className="mt-2 ml-16 text-sm text-gray-500"
                        >
                           <p className="font-medium text-gray-700">
                              Billing Address:
                           </p>
                           <p>{method.billingAddress.line1}</p>
                           {method.billingAddress.line2 && (
                              <p>{method.billingAddress.line2}</p>
                           )}
                           <p>
                              {method.billingAddress.city},{' '}
                              {method.billingAddress.state}{' '}
                              {method.billingAddress.zip}
                           </p>
                           <p>{method.billingAddress.country}</p>
                        </motion.div>
                     )}
                  </motion.div>
               ))}
            </AnimatePresence>

            {paymentMethods.length === 0 && (
               <motion.div
                  initial={{ opacity: 0 }}
                  animate={{ opacity: 1 }}
                  transition={{ delay: 0.3 }}
                  className="px-6 py-8 text-center"
               >
                  <p className="text-sm text-gray-500">
                     You don't have any payment methods yet.
                  </p>
                  <Dialog>
                     <DialogTrigger asChild>
                        <motion.div
                           whileHover={{ scale: 1.05 }}
                           whileTap={{ scale: 0.95 }}
                           initial={{ opacity: 0, y: 20 }}
                           animate={{ opacity: 1, y: 0 }}
                           transition={{ delay: 0.5 }}
                        >
                           <Button
                              variant="outline"
                              className="mt-4 text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors duration-200"
                           >
                              Add Your First Payment Method
                           </Button>
                        </motion.div>
                     </DialogTrigger>
                     <DialogContent className="sm:max-w-[500px]">
                        <DialogHeader>
                           <DialogTitle>Add Payment Method</DialogTitle>
                        </DialogHeader>
                        <PaymentMethodForm
                           onSubmit={handleAddPaymentMethod}
                           setOpen={setOpen}
                        />
                     </DialogContent>
                  </Dialog>
               </motion.div>
            )}
         </motion.div>

         {/* Confirmation Dialog for Delete */}
         <Dialog
            open={confirmDeleteId !== null}
            onOpenChange={(open) => {
               if (!open) setConfirmDeleteId(null);
            }}
         >
            <DialogContent className="sm:max-w-[425px]">
               <DialogHeader>
                  <DialogTitle>Delete Payment Method</DialogTitle>
               </DialogHeader>
               <div className="py-4">
                  <div className="flex items-center space-x-2 text-amber-600 mb-4">
                     <AlertCircle className="h-5 w-5" />
                     <p className="font-medium">
                        Are you sure you want to delete this payment method?
                     </p>
                  </div>
                  <p className="text-sm text-gray-500">
                     This action cannot be undone. This will permanently remove
                     the payment method from your account.
                  </p>
               </div>
               <div className="flex justify-end space-x-2">
                  <Button
                     variant="outline"
                     onClick={() => setConfirmDeleteId(null)}
                  >
                     Cancel
                  </Button>
                  <Button
                     variant="destructive"
                     onClick={() => {
                        if (confirmDeleteId)
                           handleDeletePaymentMethod(confirmDeleteId);
                     }}
                  >
                     Delete
                  </Button>
               </div>
            </DialogContent>
         </Dialog>
      </motion.div>
   );
}
