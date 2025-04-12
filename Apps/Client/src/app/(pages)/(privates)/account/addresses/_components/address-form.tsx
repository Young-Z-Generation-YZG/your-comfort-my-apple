'use client';

import type React from 'react';

import { useState, useRef, useEffect } from 'react';
import { Button } from '@components/ui/button';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '@components/ui/dialog';
import { Label } from '@components/ui/label';
import { Input } from '@components/ui/input';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   ChevronRight,
   GripVertical,
   Plus,
   Search,
   X,
   Check,
} from 'lucide-react';
import { motion, AnimatePresence, useAnimation } from 'framer-motion';

type Address = {
   id: string;
   name: string;
   label: string;
   street: string;
   city: string;
   state: string;
   zip: string;
   country: string;
   isDefault: boolean;
};
import {
   vietnamProvinces,
   type Province,
   type District,
} from '../_lib/vietnam-provinces';

const staggerDuration = 0.05;
const staggerDelay = 0.03;

export function Addresses() {
   const controls = useAnimation();
   const [addresses, setAddresses] = useState<Address[]>([
      {
         id: '1',
         name: 'John Doe',
         label: 'Home',
         street: '123 Apple Street',
         city: 'Cupertino',
         state: 'CA',
         zip: '95014',
         country: 'United States',
         isDefault: true,
      },
      {
         id: '2',
         name: 'John Doe',
         label: 'Work',
         street: '456 Tech Avenue',
         city: 'San Francisco',
         state: 'CA',
         zip: '94105',
         country: 'United States',
         isDefault: false,
      },
   ]);

   const [open, setOpen] = useState(false);
   const [editingAddress, setEditingAddress] = useState<Address | null>(null);
   const [searchQuery, setSearchQuery] = useState('');
   const [draggedItem, setDraggedItem] = useState<string | null>(null);
   const [recentlySetDefault, setRecentlySetDefault] = useState<string | null>(
      null,
   );
   const [isAddingNew, setIsAddingNew] = useState(false);

   const filteredAddresses = addresses.filter(
      (address) =>
         address.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
         address.street.toLowerCase().includes(searchQuery.toLowerCase()) ||
         address.city.toLowerCase().includes(searchQuery.toLowerCase()) ||
         address.label.toLowerCase().includes(searchQuery.toLowerCase()),
   );

   const handleSetDefault = (id: string) => {
      setRecentlySetDefault(id);

      // Animate the address card
      controls.start('highlight');

      setTimeout(() => {
         setAddresses(
            addresses.map((address) => ({
               ...address,
               isDefault: address.id === id,
            })),
         );

         // Reset the highlight after animation completes
         setTimeout(() => setRecentlySetDefault(null), 1500);
      }, 300);
   };

   const handleAddAddress = (newAddress: Omit<Address, 'id' | 'isDefault'>) => {
      setIsAddingNew(true);

      setTimeout(() => {
         const id = Math.random().toString(36).substring(2, 9);
         setAddresses([
            ...addresses,
            {
               ...newAddress,
               id,
               isDefault: addresses.length === 0, // Make default if it's the first address
            },
         ]);
         setOpen(false);
         setIsAddingNew(false);
      }, 500);
   };

   const handleEditAddress = (updatedAddress: Address) => {
      setAddresses(
         addresses.map((address) =>
            address.id === updatedAddress.id ? updatedAddress : address,
         ),
      );
      setEditingAddress(null);
      setOpen(false);
   };

   const handleDeleteAddress = (id: string) => {
      const addressToDelete = addresses.find((address) => address.id === id);
      const wasDefault = addressToDelete?.isDefault || false;

      const filteredAddresses = addresses.filter(
         (address) => address.id !== id,
      );

      // If we deleted the default address and there are other addresses, make the first one default
      if (wasDefault && filteredAddresses.length > 0) {
         filteredAddresses[0].isDefault = true;
      }

      setAddresses(filteredAddresses);
      setEditingAddress(null);
      setOpen(false);
   };

   const handleDragStart = (id: string) => {
      setDraggedItem(id);
   };

   const handleDragOver = (e: React.DragEvent, id: string) => {
      e.preventDefault();
      if (draggedItem === null || draggedItem === id) return;

      const draggedIndex = addresses.findIndex(
         (address) => address.id === draggedItem,
      );
      const hoverIndex = addresses.findIndex((address) => address.id === id);

      if (draggedIndex === hoverIndex) return;

      const newAddresses = [...addresses];
      const draggedAddress = newAddresses[draggedIndex];

      // Remove the dragged item
      newAddresses.splice(draggedIndex, 1);
      // Insert it at the new position
      newAddresses.splice(hoverIndex, 0, draggedAddress);

      setAddresses(newAddresses);
   };

   const handleDragEnd = () => {
      setDraggedItem(null);
   };

   // Animation variants
   const containerVariants = {
      hidden: { opacity: 0 },
      visible: {
         opacity: 1,
         transition: {
            staggerChildren: staggerDelay,
            delayChildren: 0.1,
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

   return (
      <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
         <motion.div
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
            className="flex items-center justify-between px-6 py-4 border-b border-gray-200"
         >
            <h2 className="text-lg font-medium text-gray-900">
               Shipping Addresses
            </h2>
            <div className="flex items-center">
               <Dialog open={open} onOpenChange={setOpen}>
                  <DialogTrigger asChild>
                     <motion.div
                        whileHover={{ scale: 1.05 }}
                        whileTap={{ scale: 0.95 }}
                     >
                        <Button
                           variant="outline"
                           className="text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors duration-200"
                           onClick={() => setEditingAddress(null)}
                        >
                           <Plus className="h-4 w-4 mr-1" />
                           Add Address
                        </Button>
                     </motion.div>
                  </DialogTrigger>
                  <DialogContent className="sm:max-w-[425px]">
                     <DialogHeader>
                        <DialogTitle>
                           {editingAddress ? 'Edit Address' : 'Add New Address'}
                        </DialogTitle>
                     </DialogHeader>
                     <AddressForm
                        initialAddress={editingAddress}
                        onSubmit={
                           editingAddress ? handleEditAddress : handleAddAddress
                        }
                        onDelete={
                           editingAddress
                              ? () => handleDeleteAddress(editingAddress.id)
                              : undefined
                        }
                        isLoading={isAddingNew}
                     />
                  </DialogContent>
               </Dialog>
               <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
               >
                  <Button
                     variant="ghost"
                     className="ml-2 text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors duration-200"
                  >
                     Manage
                     <ChevronRight className="h-4 w-4 ml-1" />
                  </Button>
               </motion.div>
            </div>
         </motion.div>

         {addresses.length > 1 && (
            <motion.div
               initial={{ opacity: 0, height: 0 }}
               animate={{ opacity: 1, height: 'auto' }}
               transition={{ duration: 0.3 }}
               className="px-6 py-3 bg-gray-50 border-b border-gray-200"
            >
               <div className="flex items-center">
                  <div className="relative flex-1 max-w-sm">
                     <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                     <Input
                        placeholder="Search addresses..."
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        className="pl-9 h-9"
                     />
                     <AnimatePresence>
                        {searchQuery && (
                           <motion.button
                              initial={{ opacity: 0, scale: 0.8 }}
                              animate={{ opacity: 1, scale: 1 }}
                              exit={{ opacity: 0, scale: 0.8 }}
                              onClick={() => setSearchQuery('')}
                              className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600"
                           >
                              <X className="h-4 w-4" />
                           </motion.button>
                        )}
                     </AnimatePresence>
                  </div>
                  <motion.p
                     initial={{ opacity: 0 }}
                     animate={{ opacity: 1 }}
                     transition={{ delay: 0.2 }}
                     className="ml-4 text-xs text-gray-500"
                  >
                     Drag to reorder your addresses
                  </motion.p>
               </div>
            </motion.div>
         )}

         <motion.div
            variants={containerVariants}
            initial="hidden"
            animate="visible"
            className="divide-y divide-gray-200"
         >
            <AnimatePresence>
               {filteredAddresses.map((address, index) => (
                  <motion.div
                     key={address.id}
                     custom={index}
                     variants={itemVariants}
                     initial="hidden"
                     animate={
                        address.id === recentlySetDefault
                           ? 'highlight'
                           : 'visible'
                     }
                     exit="exit"
                     className={`px-6 py-4 ${draggedItem === address.id ? 'bg-gray-50' : ''}`}
                     draggable
                     onDragStart={() => handleDragStart(address.id)}
                     onDragOver={(e) => handleDragOver(e, address.id)}
                     onDragEnd={handleDragEnd}
                     style={{ position: 'relative' }}
                  >
                     <div className="flex items-center justify-between">
                        <div className="flex items-center">
                           <motion.div
                              whileHover={{ scale: 1.1, rotate: 5 }}
                              whileTap={{ scale: 0.9 }}
                           >
                              <GripVertical className="h-5 w-5 mr-2 text-gray-300 cursor-grab active:cursor-grabbing" />
                           </motion.div>
                           <h3 className="text-sm font-medium text-gray-900">
                              {address.label}
                           </h3>
                           <AnimatePresence>
                              {address.isDefault && (
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
                        <Dialog>
                           <DialogTrigger asChild>
                              <motion.div
                                 whileHover={{ scale: 1.05 }}
                                 whileTap={{ scale: 0.95 }}
                              >
                                 <Button
                                    variant="ghost"
                                    className="text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors duration-200"
                                    onClick={() => setEditingAddress(address)}
                                 >
                                    Edit
                                 </Button>
                              </motion.div>
                           </DialogTrigger>
                           <DialogContent className="sm:max-w-[425px]">
                              <DialogHeader>
                                 <DialogTitle>Edit Address</DialogTitle>
                              </DialogHeader>
                              <AddressForm
                                 initialAddress={address}
                                 onSubmit={handleEditAddress}
                                 onDelete={() =>
                                    handleDeleteAddress(address.id)
                                 }
                              />
                           </DialogContent>
                        </Dialog>
                     </div>
                     <motion.div
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.1 * (index + 1) }}
                        className="mt-1 text-sm text-gray-500 pl-7"
                     >
                        <p>{address.name}</p>
                        <p>{address.street}</p>
                        <p>
                           {address.city}, {address.state} {address.zip}
                        </p>
                        <p>{address.country}</p>
                     </motion.div>
                     {!address.isDefault && (
                        <motion.div
                           whileHover={{ scale: 1.02 }}
                           whileTap={{ scale: 0.98 }}
                           initial={{ opacity: 0, y: 10 }}
                           animate={{ opacity: 1, y: 0 }}
                           transition={{ delay: 0.2 * (index + 1) }}
                        >
                           <Button
                              variant="link"
                              className="mt-2 p-0 h-auto text-sm font-medium text-blue-600 hover:text-blue-800 ml-7 transition-colors duration-200"
                              onClick={() => handleSetDefault(address.id)}
                           >
                              Set as Default
                           </Button>
                        </motion.div>
                     )}
                  </motion.div>
               ))}
            </AnimatePresence>

            {addresses.length === 0 && (
               <motion.div
                  initial={{ opacity: 0 }}
                  animate={{ opacity: 1 }}
                  transition={{ delay: 0.3 }}
                  className="px-6 py-8 text-center"
               >
                  <p className="text-sm text-gray-500">
                     You don't have any addresses yet.
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
                              Add Your First Address
                           </Button>
                        </motion.div>
                     </DialogTrigger>
                     <DialogContent className="sm:max-w-[425px]">
                        <DialogHeader>
                           <DialogTitle>Add New Address</DialogTitle>
                        </DialogHeader>
                        <AddressForm onSubmit={handleAddAddress} />
                     </DialogContent>
                  </Dialog>
               </motion.div>
            )}

            {addresses.length > 0 && filteredAddresses.length === 0 && (
               <motion.div
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ duration: 0.3 }}
                  className="px-6 py-8 text-center"
               >
                  <p className="text-sm text-gray-500">
                     No addresses match your search.
                  </p>
                  <motion.div whileHover={{ scale: 1.05 }}>
                     <Button
                        variant="link"
                        className="mt-2 text-sm font-medium text-blue-600"
                        onClick={() => setSearchQuery('')}
                     >
                        Clear search
                     </Button>
                  </motion.div>
               </motion.div>
            )}
         </motion.div>
      </div>
   );
}

type AddressFormProps = {
   initialAddress?: Address | null;
   onSubmit: (address: any) => void;
   onDelete?: () => void;
   isLoading?: boolean;
};

export function AddressForm({
   initialAddress,
   onSubmit,
   onDelete,
   isLoading = false,
}: AddressFormProps) {
   const [formData, setFormData] = useState<any>({
      id: initialAddress?.id || '',
      name: initialAddress?.name || '',
      label: initialAddress?.label || 'Home',
      street: initialAddress?.street || '',
      city: initialAddress?.city || '',
      state: initialAddress?.state || '',
      zip: initialAddress?.zip || '',
      country: initialAddress?.country || 'United States',
      isDefault: initialAddress?.isDefault || false,
   });

   const [selectedProvince, setSelectedProvince] = useState<Province | null>(
      null,
   );
   const [districts, setDistricts] = useState<District[]>([]);
   const [formSubmitted, setFormSubmitted] = useState(false);
   const formRef = useRef<HTMLFormElement>(null);

   // Find the province and set districts when component mounts or province changes
   useEffect(() => {
      if (formData.state) {
         const province = vietnamProvinces.find(
            (p) => p.name === formData.state,
         );
         if (province) {
            setSelectedProvince(province);
            setDistricts(province.districts);
         }
      }
   }, [formData.state]);

   const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const { name, value } = e.target;
      setFormData({ ...formData, [name]: value });
   };

   const handleSelectChange = (name: string, value: string) => {
      setFormData({ ...formData, [name]: value });
   };

   const handleSubmit = (e: React.FormEvent) => {
      e.preventDefault();
      setFormSubmitted(true);

      // Simulate form submission with animation
      setTimeout(() => {
         onSubmit(formData);
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
            delay: custom * staggerDuration,
            duration: 0.3,
            ease: 'easeOut',
         },
      }),
   };

   return (
      <form ref={formRef} onSubmit={handleSubmit} className="space-y-4 mt-4">
         <div className="grid grid-cols-2 gap-4">
            <motion.div
               className="col-span-2"
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={0}
            >
               <Label htmlFor="name">Full Name</Label>
               <Input
                  id="name"
                  name="name"
                  value={formData.name}
                  onChange={handleChange}
                  required
                  className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
               />
            </motion.div>

            <motion.div
               className="col-span-2"
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={1}
            >
               <Label htmlFor="label">Address Label</Label>
               <Select
                  value={formData.label}
                  onValueChange={(value) => handleSelectChange('label', value)}
               >
                  <SelectTrigger className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50">
                     <SelectValue placeholder="Select a label" />
                  </SelectTrigger>
                  <SelectContent className="font-SFProText">
                     <SelectItem value="Home">Home</SelectItem>
                     <SelectItem value="Work">Work</SelectItem>
                     <SelectItem value="Other">Other</SelectItem>
                  </SelectContent>
               </Select>
            </motion.div>

            <motion.div
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={3}
               className="col-span-2"
            >
               <Label htmlFor="state">Province</Label>
               <Select
                  value={formData.state}
                  onValueChange={(value) => handleSelectChange('state', value)}
               >
                  <SelectTrigger className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50">
                     <SelectValue placeholder="Select a province" />
                  </SelectTrigger>
                  <SelectContent className="max-h-[300px]">
                     {vietnamProvinces.map((province) => (
                        <SelectItem key={province.code} value={province.name}>
                           {province.name}
                        </SelectItem>
                     ))}
                  </SelectContent>
               </Select>
            </motion.div>

            <motion.div
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={3.5}
               className="col-span-2"
            >
               <Label htmlFor="city">District</Label>
               <Select
                  value={formData.city}
                  onValueChange={(value) => handleSelectChange('city', value)}
                  disabled={!selectedProvince}
               >
                  <SelectTrigger className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50">
                     <SelectValue
                        placeholder={
                           selectedProvince
                              ? 'Select a district'
                              : 'Select province first'
                        }
                     />
                  </SelectTrigger>
                  <SelectContent className="max-h-[300px]">
                     {districts.map((district) => (
                        <SelectItem key={district.code} value={district.name}>
                           {district.name}
                        </SelectItem>
                     ))}
                  </SelectContent>
               </Select>
            </motion.div>

            <motion.div
               className="col-span-2"
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={2}
            >
               <Label htmlFor="street">Street Address</Label>
               <Input
                  id="street"
                  name="street"
                  value={formData.street}
                  onChange={handleChange}
                  required
                  className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
               />
            </motion.div>

            <motion.div
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={3}
            >
               <Label htmlFor="city">City</Label>
               <Input
                  id="city"
                  name="city"
                  value={formData.city}
                  onChange={handleChange}
                  required
                  className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
               />
            </motion.div>

            <motion.div
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={3.5}
            >
               <Label htmlFor="state">State</Label>
               <Input
                  id="state"
                  name="state"
                  value={formData.state}
                  onChange={handleChange}
                  required
                  className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
               />
            </motion.div>

            <motion.div
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={4}
            >
               <Label htmlFor="zip">ZIP Code</Label>
               <Input
                  id="zip"
                  name="zip"
                  value={formData.zip}
                  onChange={handleChange}
                  required
                  className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
               />
            </motion.div>

            <motion.div
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={4.5}
            >
               <Label htmlFor="country">Country</Label>
               <Select
                  value={formData.country}
                  onValueChange={(value) =>
                     handleSelectChange('country', value)
                  }
               >
                  <SelectTrigger className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50">
                     <SelectValue placeholder="Select a country" />
                  </SelectTrigger>
                  <SelectContent>
                     <SelectItem value="United States">
                        United States
                     </SelectItem>
                     <SelectItem value="Canada">Canada</SelectItem>
                     <SelectItem value="United Kingdom">
                        United Kingdom
                     </SelectItem>
                     <SelectItem value="Australia">Australia</SelectItem>
                     <SelectItem value="Japan">Japan</SelectItem>
                  </SelectContent>
               </Select>
            </motion.div>
         </div>

         <motion.div
            variants={formFieldVariants}
            initial="hidden"
            animate="visible"
            custom={5}
            className="flex justify-between pt-4"
         >
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
                     Delete
                  </Button>
               </motion.div>
            )}
            <div className="flex gap-2 ml-auto">
               <DialogTrigger asChild>
                  <motion.div
                     whileHover={{ scale: 1.05 }}
                     whileTap={{ scale: 0.95 }}
                  >
                     <Button
                        type="button"
                        variant="outline"
                        className="transition-colors duration-200"
                     >
                        Cancel
                     </Button>
                  </motion.div>
               </DialogTrigger>
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
                     className="transition-colors duration-200 relative"
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
                     ) : initialAddress ? (
                        'Save Changes'
                     ) : (
                        'Add Address'
                     )}
                  </Button>
               </motion.div>
            </div>
         </motion.div>
      </form>
   );
}
