'use client';

import { Button } from '@components/ui/button';
import { CardContext, DefaultActionContent } from '../_components/card-content';
import Badge from '../_components/badge';
import { useEffect, useMemo, useState } from 'react';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '@components/ui/dialog';
import { AddressForm } from './_components/address-form';
import { motion } from 'framer-motion';
import { Plus } from 'lucide-react';
import { IAddressPayload } from '~/domain/interfaces/identity/address';
import { Skeleton } from '@components/ui/skeleton';
import { FiEdit3 } from 'react-icons/fi';
import useIdentityService from '@components/hooks/api/use-identity-service';

const AddressesPage = () => {
   const [open, setOpen] = useState(false);
   const [editingAddress, setEditingAddress] = useState<{
      id: string;
      payload: IAddressPayload;
   } | null>(null);

   const {
      setDefaultAddressAsync,
      addAddressAsync,
      updateAddressAsync,
      deleteAddressAsync,
      getAddressesAsync,
      getAddressesState,
      isLoading,
   } = useIdentityService();

   const displayAddresses = useMemo(() => {
      if (getAddressesState.isSuccess && getAddressesState.data) {
         const defaultAddress = getAddressesState.data.find(
            (item) => item.is_default,
         );
         const otherAddresses = getAddressesState.data.filter(
            (item) => !item.is_default,
         );

         return [defaultAddress, ...otherAddresses];
      }
      return null;
   }, [getAddressesState.isSuccess, getAddressesState.data]);

   useEffect(() => {
      const fetchAddresses = async () => {
         const result = await getAddressesAsync();
         if (result.isSuccess) {
            console.log(result.data);
         }
      };
      fetchAddresses();
   }, [getAddressesAsync]);

   return (
      <CardContext>
         <div className="flex flex-col">
            <div className="flex justify-between md:flex-row flex-col">
               <h3 className="text-xl font-medium">Addresses Management</h3>

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
                  <DialogContent className="w-full max-w-[90vw] sm:max-w-[425px] p-4 sm:p-6 rounded-xl">
                     <DialogHeader>
                        <DialogTitle>
                           {editingAddress ? 'Edit Address' : 'Add New Address'}
                        </DialogTitle>
                     </DialogHeader>
                     <AddressForm
                        onAdd={async (data) => {
                           await addAddressAsync(data);
                        }}
                        onUpdate={async (id, data) => {
                           await updateAddressAsync({ id, payload: data });
                        }}
                        onDelete={async (id) => {
                           await deleteAddressAsync(id);
                        }}
                        onClose={() => {
                           setOpen(false);
                           setEditingAddress(null);
                        }}
                        isLoading={isLoading}
                        isEditing={!!editingAddress}
                        initialAddress={editingAddress}
                     />
                  </DialogContent>
               </Dialog>
            </div>

            {isLoading ? (
               <div>
                  {Array(
                     (displayAddresses?.length ?? 0 > 0)
                        ? (displayAddresses?.length ?? 0)
                        : 4,
                  )
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
            ) : (displayAddresses?.length ?? 0 > 0) ? (
               displayAddresses?.map((item, index) => {
                  return (
                     <DefaultActionContent
                        className="w-full mt-2"
                        key={item?.id ?? index}
                     >
                        <div
                           className={` flex w-full flex-col gap-1 text-slate-500 font-SFProText text-sm font-light md:cursor-default cursor-pointer md:hover:bg-transparent hover:bg-gray-50 md:rounded-none rounded-xl md:p-0 p-3 md:border-0 border border-gray-100 `}
                           onClick={() => {
                              if (window.innerWidth < 1024 && item) {
                                 setEditingAddress({
                                    id: item.id,
                                    payload: item,
                                 });
                                 setOpen(true);
                              }
                           }}
                        >
                           <div className="flex justify-between items-center flex-wrap">
                              <div className="flex gap-2 items-center">
                                 <h3 className="text-base md:text-xl font-medium text-black/80 font-SFProDisplay">
                                    {item?.label ?? ''}
                                 </h3>
                                 {item?.is_default && (
                                    <Badge variants="default" />
                                 )}
                              </div>
                              <div className="hidden md:block">
                                 <Button
                                    variant="outline"
                                    className="select-none rounded-full h-[22px] px-2 py-1 font-SFProText text-sm font-medium gap-1"
                                    onClick={() => {
                                       if (item) {
                                          setEditingAddress({
                                             id: item.id,
                                             payload: item,
                                          });
                                          setOpen(true);
                                       }
                                    }}
                                 >
                                    <span>Edit</span>
                                    <FiEdit3 className="h-3 w-3" />
                                 </Button>
                              </div>
                           </div>

                           <p>
                              {item?.contact_name ?? ''} | +84{' '}
                              {item?.contact_phone_number ?? ''}
                           </p>
                           <p>{item?.address_line ?? ''}</p>
                           <p>
                              {item?.district ?? ''}
                              {(item?.district?.length ?? 0) > 0 && ','}{' '}
                              {item?.province ?? ''}
                              {(item?.province?.length ?? 0) > 0 && ','}{' '}
                              {item?.country ?? ''}
                           </p>

                           {!item?.is_default && (
                              <div className="mt-2">
                                 <button
                                    className=" text-blue-600 font-medium text-sm md:text-[13px] rounded-lg border border-blue-200 px-3 py-1.5 hover:bg-blue-50 active:bg-blue-100 transition-all select-none"
                                    onClick={async (e) => {
                                       e.stopPropagation();
                                       if (item?.id) {
                                          await setDefaultAddressAsync(item.id);
                                       }
                                    }}
                                 >
                                    Set as Default
                                 </button>
                              </div>
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
