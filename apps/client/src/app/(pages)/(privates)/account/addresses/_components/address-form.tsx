'use client';

import type React from 'react';

import { useState, useRef, useEffect } from 'react';
import { Button } from '~/components/ui/button';
import { DialogTrigger } from '~/components/ui/dialog';
import { Label } from '~/components/ui/label';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '~/components/ui/select';

import { motion } from 'framer-motion';

import {
   vietnamProvinces,
   type Province,
   type District,
} from '../_data/vietnam-provinces';
import {
   AddressFormType,
   AddressResolver,
} from '~/domain/schemas/address.schema';
import FieldInputSecond from '~/components/client/forms/field-input-second';
import { Form, FormField, FormItem, FormMessage } from '~/components/ui/form';
import { IAddressPayload } from '~/domain/interfaces/identity.interface';
import { useForm } from 'react-hook-form';
import isDifferentValue from '~/infrastructure/utils/is-different-value';

const staggerDuration = 0.05;

type AddressFormProps = {
   initialAddress?: {
      addressId: string;
      payload: IAddressPayload;
   } | null;
   isEditing?: boolean;
   onAdd: (data: AddressFormType) => Promise<void>;
   onUpdate: (addressId: string, data: AddressFormType) => Promise<void>;
   onDelete: (addressId: string) => Promise<void>;
   onClose: () => void;
   isLoading?: boolean;
};

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

export function AddressForm({
   initialAddress,
   isEditing = false,
   onAdd,
   onUpdate,
   onDelete,
   onClose,
   isLoading = false,
}: AddressFormProps) {
   const [selectedProvince, setSelectedProvince] = useState<Province | null>(
      null,
   );
   const [districts, setDistricts] = useState<District[]>([]);

   const form = useForm<AddressFormType>({
      resolver: AddressResolver,
      defaultValues: {
         label: initialAddress?.payload.label || '',
         contact_name: initialAddress?.payload.contact_name || '',
         contact_phone_number:
            initialAddress?.payload.contact_phone_number || '',
         address_line: initialAddress?.payload.address_line || '',
         district: initialAddress?.payload.district || '',
         province: initialAddress?.payload.province || '',
         country: 'Việt Nam',
      } as AddressFormType,
   });

   useEffect(() => {
      if (form.getValues('province')) {
         const province = vietnamProvinces.find(
            (p) => p.name === form.getValues('province'),
         );
         if (province) {
            setSelectedProvince(province);
            setDistricts(province.districts);
         }
      }
   }, [form.getValues('province')]);

   const handleSelectChange = (
      name: 'province' | 'district' | 'country',
      value: string,
   ) => {
      form.setValue(name, value);
      if (name === 'province') {
         const province = vietnamProvinces.find((p) => p.name === value);
         if (province) {
            setSelectedProvince(province);
            setDistricts(province.districts);
         } else {
            setSelectedProvince(null);
            setDistricts([]);
         }
      }
   };

   const handleSubmit = async (data: AddressFormType) => {
      try {
         if (isEditing && initialAddress?.addressId) {
            await onUpdate(initialAddress.addressId, data);
         } else {
            await onAdd(data);
         }
         onClose();
      } catch (error) {
         console.error('Form submission error:', error);
      }
   };

   const handleDelete = async () => {
      if (initialAddress?.addressId) {
         try {
            await onDelete(initialAddress.addressId);
            onClose();
         } catch (error) {
            console.error('Delete error:', error);
         }
      }
   };

   return (
      <Form {...form}>
         <form
            onSubmit={form.handleSubmit(handleSubmit)}
            className="space-y-4 mt-4"
         >
            <div className="grid grid-cols-2 gap-4">
               <motion.div
                  className="col-span-2"
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={2}
               >
                  <FieldInputSecond
                     form={form}
                     name="label"
                     label="Label"
                     placeholder="e.g. Home, Office"
                     disabled={isLoading}
                  />
               </motion.div>

               <motion.div
                  className="col-span-2"
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={0}
               >
                  <FieldInputSecond
                     form={form}
                     name="contact_name"
                     label="Full Name"
                     placeholder="Enter full name"
                     disabled={isLoading}
                  />
               </motion.div>

               <motion.div
                  className="col-span-2"
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={0}
               >
                  <FieldInputSecond
                     form={form}
                     name="contact_phone_number"
                     label="Phone Number"
                     placeholder="Enter phone number"
                     type="number"
                     disabled={isLoading}
                  />
               </motion.div>

               <motion.div
                  className="col-span-2"
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={2}
               >
                  <FieldInputSecond
                     form={form}
                     name="address_line"
                     label="Address Street"
                     placeholder="Street, house number, etc."
                     disabled={isLoading}
                  />
               </motion.div>

               <motion.div
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={3}
                  className="col-span-2"
               >
                  <Label htmlFor="state">Province</Label>
                  <FormField
                     control={form.control}
                     name="province"
                     render={({ field }) => (
                        <FormItem>
                           <Select
                              value={form.getValues('province')}
                              onValueChange={(value) =>
                                 handleSelectChange('province', value)
                              }
                              disabled={isLoading}
                           >
                              <SelectTrigger className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50">
                                 <SelectValue placeholder="Select a province" />
                              </SelectTrigger>
                              <SelectContent className="max-h-[300px]">
                                 {vietnamProvinces.map((province) => (
                                    <SelectItem
                                       key={province.code}
                                       value={province.name}
                                    >
                                       {province.name}
                                    </SelectItem>
                                 ))}
                              </SelectContent>
                           </Select>
                           <FormMessage />
                        </FormItem>
                     )}
                  />
               </motion.div>

               <motion.div
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={3.5}
                  className="col-span-2"
               >
                  <Label htmlFor="city">District</Label>
                  <FormField
                     control={form.control}
                     name="district"
                     render={({ field }) => (
                        <FormItem>
                           <Select
                              value={form.getValues('district')}
                              onValueChange={(value) =>
                                 handleSelectChange('district', value)
                              }
                              disabled={!selectedProvince || isLoading}
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
                                    <SelectItem
                                       key={district.code}
                                       value={district.name}
                                    >
                                       {district.name}
                                    </SelectItem>
                                 ))}
                              </SelectContent>
                           </Select>
                           <FormMessage />
                        </FormItem>
                     )}
                  />
               </motion.div>

               <motion.div
                  variants={formFieldVariants}
                  initial="hidden"
                  animate="visible"
                  custom={4.5}
                  className="col-span-2"
               >
                  <Label htmlFor="country">Country</Label>
                  <FormField
                     control={form.control}
                     name="country"
                     render={({ field }) => (
                        <FormItem>
                           <Select
                              value={form.getValues('country')}
                              onValueChange={(value) =>
                                 handleSelectChange('country', value)
                              }
                              disabled={isLoading}
                           >
                              <SelectTrigger className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50">
                                 <SelectValue placeholder="Select a country" />
                              </SelectTrigger>
                              <SelectContent>
                                 <SelectItem value="Việt Nam">
                                    Việt Nam
                                 </SelectItem>
                              </SelectContent>
                           </Select>
                           <FormMessage />
                        </FormItem>
                     )}
                  />
               </motion.div>
            </div>

            <motion.div
               variants={formFieldVariants}
               initial="hidden"
               animate="visible"
               custom={5}
               className="flex justify-between pt-4"
            >
               <div className="flex gap-2 ml-auto">
                  {isEditing && (
                     <motion.div
                        whileHover={{ scale: 1.05 }}
                        whileTap={{ scale: 0.95 }}
                     >
                        <Button
                           type="button"
                           variant="destructive"
                           className="transition-colors duration-200"
                           onClick={handleDelete}
                        >
                           Delete
                        </Button>
                     </motion.div>
                  )}

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
                     animate={isLoading ? { scale: [1, 0.95, 1.05, 1] } : {}}
                     transition={{ duration: 0.4 }}
                  >
                     <Button
                        type="submit"
                        className="transition-colors duration-200 relative"
                        disabled={
                           isLoading ||
                           !isDifferentValue(
                              {
                                 label: initialAddress?.payload.label,
                                 contact_name:
                                    initialAddress?.payload.contact_name,
                                 contact_phone_number:
                                    initialAddress?.payload
                                       .contact_phone_number,
                                 address_line:
                                    initialAddress?.payload.address_line,
                                 district: initialAddress?.payload.district,
                                 province: initialAddress?.payload.province,
                              },
                              form.getValues(),
                           )
                        }
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
      </Form>
   );
}
