'use client';

import type React from 'react';

import { useState, useRef, useEffect } from 'react';
import { Button } from '@components/ui/button';
import { DialogTrigger } from '@components/ui/dialog';
import { Label } from '@components/ui/label';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';

import { motion } from 'framer-motion';

import {
   vietnamProvinces,
   type Province,
   type District,
} from '../_lib/vietnam-provinces';
import {
   AddressFormType,
   AddressResolver,
} from '~/domain/schemas/address.schema';
import FieldInputSecond from '@components/client/forms/field-input-second';
import { Form, FormField, FormItem, FormMessage } from '@components/ui/form';
import {
   useAddAddressAsyncMutation,
   useDeleteAddressAsyncMutation,
   useUpdateAddressAsyncMutation,
} from '~/infrastructure/services/identity.service';
import { IAddressPayload } from '~/domain/interfaces/identity/address';
import { useForm } from 'react-hook-form';
import isServerErrorResponse from '~/infrastructure/utils/http/is-server-error';
import { toast as sonnerToast } from 'sonner';
import { useToast } from '~/hooks/use-toast';
import isDifferentValue from '~/infrastructure/utils/is-different-value';

const staggerDuration = 0.05;

type AddressFormProps = {
   initialAddress?: {
      id: string;
      payload: IAddressPayload;
   } | null;
   isEditing?: boolean;
   onSubmit: () => void;
   onDelete?: () => void;
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
   onSubmit,
   onDelete,
   isLoading = false,
}: AddressFormProps) {
   const [loading, setLoading] = useState(false);
   const [selectedProvince, setSelectedProvince] = useState<Province | null>(
      null,
   );
   const [districts, setDistricts] = useState<District[]>([]);

   const { toast } = useToast();

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
         country: 'Vietnam',
      } as AddressFormType,
   });

   const [
      addAddressAsync,
      {
         isLoading: isAddAddressLoading,
         isSuccess: isAddAddressSuccess,
         isError: isAddAddressError,
         error: addAddressError,
      },
   ] = useAddAddressAsyncMutation();

   const [
      updateAddressAsync,
      {
         isLoading: isUpdateAddressLoading,
         isSuccess: isUpdateAddressSuccess,
         isError: isUpdateAddressError,
         error: updateAddressError,
      },
   ] = useUpdateAddressAsyncMutation();

   const [
      deleteAddressAsync,
      {
         isLoading: isDeleteAddressLoading,
         isSuccess: isDeleteAddressSuccess,
         isError: isDeleteAddressError,
         error: deleteAddressError,
      },
   ] = useDeleteAddressAsyncMutation();

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
      console.log('Form submitted:', data);

      if (isEditing) {
         if (
            (initialAddress?.payload.label !== data.label ||
               initialAddress?.payload.contact_name !== data.contact_name ||
               initialAddress?.payload.contact_phone_number !==
                  data.contact_phone_number ||
               initialAddress?.payload.address_line !== data.address_line ||
               initialAddress?.payload.district !== data.district ||
               initialAddress?.payload.province !== data.province) &&
            initialAddress?.id
         ) {
            await updateAddressAsync({
               id: initialAddress.id,
               payload: data,
            }).unwrap();
         }
         onSubmit();

         return;
      }

      await addAddressAsync(data).unwrap();

      onSubmit();
   };

   const handleDelete = async () => {
      if (initialAddress?.id) {
         await deleteAddressAsync(initialAddress.id).unwrap();

         onSubmit();
      }
   };

   useEffect(() => {
      if (isAddAddressError && isServerErrorResponse(addAddressError)) {
         toast({
            variant: 'destructive',
            title: `${addAddressError.data.error.message ?? 'Server busy, please try again later'}`,
         });
      }

      if (isAddAddressSuccess) {
         sonnerToast.success('Add Address Successfully', {
            style: {
               backgroundColor: '#4CAF50', // Custom green background color
               color: '#FFFFFF', // White text color
            },
         });
      }
   }, [isAddAddressError, addAddressError, isAddAddressSuccess]);

   useEffect(() => {
      if (isUpdateAddressError && isServerErrorResponse(updateAddressError)) {
         toast({
            variant: 'destructive',
            title: `${updateAddressError.data.error.message ?? 'Server busy, please try again later'}`,
         });
      }

      if (isUpdateAddressSuccess) {
         sonnerToast.success('Update Address Successfully', {
            style: {
               backgroundColor: '#4CAF50', // Custom green background color
               color: '#FFFFFF', // White text color
            },
         });
      }
   }, [isUpdateAddressError, updateAddressError, isUpdateAddressSuccess]);

   useEffect(() => {
      if (isDeleteAddressError && isServerErrorResponse(deleteAddressError)) {
         toast({
            variant: 'destructive',
            title: `${deleteAddressError.data.error.message ?? 'Server busy, please try again later'}`,
         });
      }

      if (isDeleteAddressSuccess) {
         sonnerToast.success('Delete Address Successfully', {
            style: {
               backgroundColor: '#4CAF50', // Custom green background color
               color: '#FFFFFF', // White text color
            },
         });
      }
   }, [isDeleteAddressError, deleteAddressError, isDeleteAddressSuccess]);

   isAddAddressSuccess;
   isUpdateAddressSuccess;
   isDeleteAddressSuccess;

   useEffect(() => {
      setLoading(
         isAddAddressLoading ||
            isUpdateAddressLoading ||
            isDeleteAddressLoading,
      );
   }, [isAddAddressLoading, isUpdateAddressLoading, isDeleteAddressLoading]);

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
                     disabled={loading}
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
                     disabled={loading}
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
                     type="number"
                     disabled={loading}
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
                     disabled={loading}
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
                              disabled={loading}
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
                              disabled={!selectedProvince || loading}
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
                              disabled={loading}
                           >
                              <SelectTrigger className="transition-all duration-200 focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50">
                                 <SelectValue placeholder="Select a country" />
                              </SelectTrigger>
                              <SelectContent>
                                 <SelectItem value="Viet Nam">
                                    Viet Nam
                                 </SelectItem>
                                 <SelectItem value="United States">
                                    United States
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
                     animate={loading ? { scale: [1, 0.95, 1.05, 1] } : {}}
                     transition={{ duration: 0.4 }}
                  >
                     <Button
                        type="submit"
                        className="transition-colors duration-200 relative"
                        disabled={
                           loading ||
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
                        {loading ? (
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
