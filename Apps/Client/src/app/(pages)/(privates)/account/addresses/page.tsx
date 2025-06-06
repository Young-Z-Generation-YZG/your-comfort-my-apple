'use client';

import { Button } from '@components/ui/button';
import { CardContext, DefaultActionContent } from '../_components/card-content';
import Badge from '../_components/badge';
import { FiEdit3 } from 'react-icons/fi';
import { useEffect, useState } from 'react';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '@components/ui/dialog';
import { AddressResolver } from '~/domain/schemas/address.schema';
import { AddressForm } from './_components/address-form';
import { motion } from 'framer-motion';
import { Plus } from 'lucide-react';
import {
   useGetAddressesAsyncQuery,
   useSetDefaultAddressAsyncMutation,
} from '~/infrastructure/services/identity.service';
import {
   IAddressPayload,
   IAddressResponse,
} from '~/domain/interfaces/identity/address';
import { Skeleton } from '@components/ui/skeleton';
import isServerErrorResponse from '~/infrastructure/utils/http/is-server-error';
import { useToast } from '~/hooks/use-toast';
import { toast as sonnerToast } from 'sonner';

const AddressesPage = () => {
   const [isLoading, setIsLoading] = useState(true);
   const [addressesList, setAddressesList] = useState<IAddressResponse[]>([]);
   const [open, setOpen] = useState(false);
   const [editingAddress, setEditingAddress] = useState<{
      id: string;
      payload: IAddressPayload;
   } | null>(null);

   const { toast } = useToast();

   const {
      data: addressesAsync,
      isLoading: isLoadingAddresses,
      isSuccess: isSuccessAddressesAsync,
      refetch: refetchAddresses,
   } = useGetAddressesAsyncQuery();

   const [
      setDefaultAddressAsync,
      {
         isLoading: isLoadingSetDefaultAddress,
         isSuccess: isSuccessSetDefaultAddress,
         isError: isErrorSetDefaultAddress,
         error: errorSetDefaultAddress,
      },
   ] = useSetDefaultAddressAsyncMutation();

   const handleSetDefaultAddress = async (id: string) => {
      await setDefaultAddressAsync(id).unwrap();
   };

   const renderAddresses = () => {
      const defaultAddress = addressesList.find((item) => item.is_default);

      if (!defaultAddress) return addressesList;

      const otherAddresses = addressesList.filter((item) => !item.is_default);

      return [defaultAddress, ...otherAddresses] as IAddressResponse[];
   };

   useEffect(() => {
      if (isSuccessAddressesAsync && addressesAsync) {
         setAddressesList(addressesAsync);
      }
   }, [isSuccessAddressesAsync, addressesAsync]);

   useEffect(() => {
      if (isSuccessSetDefaultAddress) {
         sonnerToast.success('Set Default Successfully', {
            style: {
               backgroundColor: '#4CAF50', // Custom green background color
               color: '#FFFFFF', // White text color
            },
         });
      }

      if (isErrorSetDefaultAddress && errorSetDefaultAddress) {
         if (isServerErrorResponse(errorSetDefaultAddress)) {
            toast({
               variant: 'destructive',
               title: `${errorSetDefaultAddress.data.error.message ?? 'Server busy, please try again later'}`,
            });
         }
      }
   }, [
      isSuccessSetDefaultAddress,
      isErrorSetDefaultAddress,
      errorSetDefaultAddress,
   ]);

   useEffect(() => {
      setIsLoading(isLoadingAddresses || isLoadingSetDefaultAddress);
   }, [isLoadingAddresses, isLoadingSetDefaultAddress]);

   return (
      <CardContext>
         <div className="flex flex-col">
            <div className="flex justify-between">
               <h3 className="text-xl font-medium">Personal Information</h3>

               {/* <Addresses /> */}
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
                        onSubmit={() => {
                           setOpen(false);
                           refetchAddresses();
                        }}
                        isEditing={!!editingAddress}
                        initialAddress={
                           editingAddress
                              ? {
                                   id: editingAddress.id,
                                   payload: editingAddress.payload,
                                }
                              : null
                        }
                     />
                  </DialogContent>
               </Dialog>
            </div>

            {isLoading ? (
               <div>
                  {Array(addressesList.length > 0 ? addressesList.length : 3)
                     .fill(0)
                     .map((_, index) => (
                        <DefaultActionContent
                           className="w-full mt-2 flex flex-col gap-2"
                           key={index}
                        >
                           <Skeleton className="h-[30px] w-[100px] rounded-xl" />
                           <Skeleton className="h-[24px] w-[200px] rounded-xl" />
                           <Skeleton className="h-[24px] w-[200px] rounded-xl" />
                           <Skeleton className="h-[24px] w-[200px] rounded-xl" />
                        </DefaultActionContent>
                     ))}
               </div>
            ) : addressesList.length > 0 ? (
               renderAddresses().map((item) => {
                  return (
                     <DefaultActionContent
                        className="w-full mt-2"
                        key={item.id}
                     >
                        <div className="flex w-full flex-col gap-1 text-slate-500 font-SFProText text-sm font-light">
                           <div className="flex justify-between items-center">
                              <div className="flex gap-2 items-center">
                                 <h3 className="text-xl font-medium text-black/80 font-SFProDisplay">
                                    {item.label}
                                 </h3>
                                 {item.is_default && (
                                    <Badge variants="default" />
                                 )}
                              </div>

                              <Button
                                 variant="outline"
                                 className="select-none rounded-full h-[22px] px-2 py-1 font-SFProText text-sm font-medium"
                                 onClick={() => {
                                    setEditingAddress({
                                       id: item.id,
                                       payload: item,
                                    });
                                    setOpen(true);
                                 }}
                              >
                                 <span>edit</span>
                                 <span>
                                    <FiEdit3 />
                                 </span>
                              </Button>
                           </div>
                           <p>
                              {item.contact_name} | +84{' '}
                              {item.contact_phone_number}
                           </p>
                           <p>{item.address_line}</p>
                           <p>
                              {item.district}
                              {!(item.district.length > 0) || ','}{' '}
                              {item.province}
                              {!(item.province.length > 0) || ','}{' '}
                              {item.country}
                           </p>

                           {!item.is_default && (
                              <p
                                 className="font-medium w-fit text-blue-600 mt-2 select-none cursor-pointer hover:underline"
                                 onClick={() => {
                                    handleSetDefaultAddress(item.id);
                                 }}
                              >
                                 Set as Default
                              </p>
                           )}
                        </div>
                     </DefaultActionContent>
                  );
               })
            ) : null}
         </div>
      </CardContext>
   );
};

export default AddressesPage;
