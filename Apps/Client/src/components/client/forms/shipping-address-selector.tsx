'use client';

import { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { Check, ChevronDown, Plus } from 'lucide-react';
import type { UseFormSetValue } from 'react-hook-form';
import { CheckoutFormType } from '~/domain/schemas/basket.schema';

interface ShippingAddress {
   id: string;
   isDefault: boolean;
   contactName: string;
   contactPhoneNumber: string;
   addressLine: string;
   district: string;
   province: string;
   country: string;
}

interface ShippingAddressSelectorProps {
   addresses: ShippingAddress[];
   selectedAddress: ShippingAddress;
   setSelectedAddress: (address: ShippingAddress) => void;
   setValue: UseFormSetValue<CheckoutFormType>;
}

export function ShippingAddressSelector({
   addresses,
   selectedAddress,
   setSelectedAddress,
   setValue,
}: ShippingAddressSelectorProps) {
   const [showAddressSelector, setShowAddressSelector] = useState(false);

   // Handle address selection
   const handleSelectAddress = (address: ShippingAddress) => {
      setSelectedAddress(address);

      // Update form values
      setValue('shipping_address.contact_name', address.contactName);
      setValue(
         'shipping_address.contact_phone_number',
         address.contactPhoneNumber,
      );
      setValue('shipping_address.address_line', address.addressLine);
      setValue('shipping_address.district', address.district);
      setValue('shipping_address.province', address.province);
      setValue('shipping_address.country', address.country);

      setShowAddressSelector(false);
   };

   // Animation variants
   const dropdownVariants = {
      hidden: {
         opacity: 0,
         height: 0,
         transition: {
            when: 'afterChildren',
            staggerChildren: 0.05,
            staggerDirection: -1,
         },
      },
      visible: {
         opacity: 1,
         height: 'auto',
         transition: {
            when: 'beforeChildren',
            staggerChildren: 0.05,
         },
      },
   };

   const itemVariants = {
      hidden: { opacity: 0, y: -10 },
      visible: { opacity: 1, y: 0 },
   };

   const checkVariants = {
      hidden: { scale: 0, opacity: 0 },
      visible: {
         scale: 1,
         opacity: 1,
         transition: { type: 'spring', stiffness: 500, damping: 25 },
      },
   };

   return (
      <div className="bg-white rounded-lg shadow-sm p-6 border border-gray-200">
         <div className="flex justify-between items-center mb-4">
            <h2 className="text-base font-normal font-SFProText">
               Choose Shipping Address
            </h2>
            <motion.button
               type="button"
               className="text-sm text-gray-900 font-medium flex items-center"
               onClick={() => setShowAddressSelector(!showAddressSelector)}
               whileTap={{ scale: 0.97 }}
            >
               Change
               <motion.div
                  animate={{ rotate: showAddressSelector ? 180 : 0 }}
                  transition={{ duration: 0.3, ease: 'easeInOut' }}
               >
                  <ChevronDown className="h-4 w-4 ml-1" />
               </motion.div>
            </motion.button>
         </div>

         {/* Selected Address Summary */}
         <motion.div
            className="flex items-start mb-4"
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
         >
            <div className="flex-1">
               <p className="font-medium">
                  {selectedAddress.contactName} |{' '}
                  {selectedAddress.contactPhoneNumber}
               </p>
               <p className="text-sm text-gray-500">
                  {selectedAddress.addressLine}
               </p>
               <p className="text-sm text-gray-500">
                  {selectedAddress.district}, {selectedAddress.province},{' '}
                  {selectedAddress.country}
               </p>
            </div>
            {true && (
               <motion.span
                  className="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-gray-100 text-gray-800"
                  initial={{ opacity: 0, scale: 0.8 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ duration: 0.2 }}
               >
                  Default
               </motion.span>
            )}
         </motion.div>

         {/* Address Selector Dropdown */}
         <AnimatePresence>
            {showAddressSelector && (
               <motion.div
                  className="mt-4 border border-gray-200 rounded-md overflow-hidden"
                  variants={dropdownVariants}
                  initial="hidden"
                  animate="visible"
                  exit="hidden"
               >
                  {/* {addresses.map((address) => (
                     <motion.div
                        key={address.id}
                        className={`p-4 flex items-start hover:bg-gray-50 cursor-pointer ${
                           selectedAddress.id === address.id ? 'bg-gray-50' : ''
                        }`}
                        onClick={() => handleSelectAddress(address)}
                        variants={itemVariants}
                        whileHover={{
                           backgroundColor: 'rgba(243, 244, 246, 1)',
                        }}
                        whileTap={{ backgroundColor: 'rgba(229, 231, 235, 1)' }}
                     >
                        <div className="flex-1">
                           <div className="flex items-center">
                              <p className="font-medium">
                                 {address.firstName} {address.lastName}
                              </p>
                              {address.isDefault && (
                                 <motion.span
                                    className="ml-2 inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-gray-100 text-gray-800"
                                    initial={{ opacity: 0 }}
                                    animate={{ opacity: 1 }}
                                    transition={{ duration: 0.2 }}
                                 >
                                    Default
                                 </motion.span>
                              )}
                           </div>
                           <p className="text-sm text-gray-500">
                              {address.address}
                           </p>
                           <p className="text-sm text-gray-500">
                              {address.city}, {address.state} {address.zipCode}
                           </p>
                        </div>
                        {selectedAddress.id === address.id && (
                           <motion.div
                              variants={checkVariants}
                              initial="hidden"
                              animate="visible"
                           >
                              <Check className="h-5 w-5 text-gray-900" />
                           </motion.div>
                        )}
                     </motion.div>
                  ))} */}

                  {/* Add New Address Option */}
                  <motion.div
                     className="p-4 flex items-center text-gray-900 hover:bg-gray-50 cursor-pointer border-t border-gray-200"
                     variants={itemVariants}
                     whileHover={{ backgroundColor: 'rgba(243, 244, 246, 1)' }}
                     whileTap={{ backgroundColor: 'rgba(229, 231, 235, 1)' }}
                  >
                     <motion.div
                        whileHover={{ rotate: 90 }}
                        transition={{ duration: 0.2 }}
                        className="mr-2"
                     >
                        <Plus className="h-5 w-5" />
                     </motion.div>
                     <span className="font-medium">
                        Add a new shipping address
                     </span>
                  </motion.div>
               </motion.div>
            )}
         </AnimatePresence>
      </div>
   );
}
