'use client';

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import {
   Dialog,
   DialogContent,
   DialogDescription,
   DialogFooter,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '@components/ui/dialog';
import {
   Form,
   FormControl,
   FormField,
   FormItem,
   FormLabel,
   FormMessage,
} from '@components/ui/form';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { Input } from '@components/ui/input';
import { Button } from '@components/ui/button';
import { TSku } from '~/src/domain/types/catalog.type';
import useRequestService from '~/src/hooks/api/use-request-service';
import useTenantService from '~/src/hooks/api/use-tenant-service';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { Loader2, Send, Package } from 'lucide-react';

const formSchema = z.object({
   to_branch_id: z.string().min(1, 'Target branch is required'),
   request_quantity: z.coerce.number().min(1, 'Quantity must be at least 1'),
});

interface RequestSkuModalProps {
   sku: TSku;
   trigger?: React.ReactNode;
}

export function RequestSkuModal({ sku, trigger }: RequestSkuModalProps) {
   const [open, setOpen] = useState(false);
   const { createSkuRequestAsync, createSkuRequestState } = useRequestService();
   const { getListTenantsAsync, getListTenantsState } = useTenantService();
   const { currentUser } = useAppSelector((state) => state.auth);

   const tenants = getListTenantsState.data || [];

   useEffect(() => {
      if (open) {
         getListTenantsAsync();
      }
   }, [open, getListTenantsAsync]);

   const form = useForm<z.infer<typeof formSchema>>({
      resolver: zodResolver(formSchema),
      defaultValues: {
         to_branch_id: '',
         request_quantity: 1,
      },
   });

   const onSubmit = async (values: z.infer<typeof formSchema>) => {
      if (!currentUser?.userId) return;

      const result = await createSkuRequestAsync({
         sender_user_id: currentUser.userId,
         from_branch_id: sku.branch_id,
         to_branch_id: values.to_branch_id,
         sku_id: sku.id,
         request_quantity: values.request_quantity,
      });

      if (result.isSuccess) {
         setOpen(false);
         form.reset();
      }
   };

   return (
      <Dialog open={open} onOpenChange={setOpen}>
         <DialogTrigger asChild>
            {trigger || (
               <Button variant="default" className="bg-indigo-600 hover:bg-indigo-700 text-white shadow-md transition-all active:scale-95">
                  <Send className="mr-2 h-4 w-4" />
                  Request SKU Application
               </Button>
            )}
         </DialogTrigger>
         <DialogContent className="sm:max-w-[425px] border-none shadow-2xl bg-white/95 backdrop-blur-md">
            <DialogHeader>
               <DialogTitle className="text-xl font-bold text-gray-800">Request SKU Application</DialogTitle>
               <DialogDescription className="text-gray-500">
                  Submit a formal request to transfer <strong>{sku.model.name}</strong> stock.
               </DialogDescription>
            </DialogHeader>

            <Form {...form}>
               <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6 py-4">
                  <div className="space-y-4">
                     <div className="flex items-center gap-3 p-3 rounded-xl bg-indigo-50 border border-indigo-100">
                        <div className="p-2 bg-indigo-600 rounded-lg">
                           <Package className="h-5 w-5 text-white" />
                        </div>
                        <div>
                           <p className="text-[10px] font-bold text-indigo-600 uppercase tracking-tight">Source SKU</p>
                           <p className="text-sm font-bold text-indigo-900">{sku.code}</p>
                        </div>
                     </div>

                     <FormField
                        control={form.control}
                        name="to_branch_id"
                        render={({ field }) => (
                           <FormItem className="space-y-1.5">
                              <FormLabel className="text-sm font-semibold text-gray-700">Requesting Branch (Target)</FormLabel>
                              <Select
                                 onValueChange={field.onChange}
                                 defaultValue={field.value}
                                 disabled={getListTenantsState.isLoading}
                              >
                                 <FormControl>
                                    <SelectTrigger className="bg-gray-50 border-gray-200 focus:ring-indigo-500">
                                       <SelectValue placeholder="Select target branch" />
                                    </SelectTrigger>
                                 </FormControl>
                                 <SelectContent>
                                    {tenants
                                       .filter(t => t.embedded_branch.id !== sku.branch_id)
                                       .map((tenant) => (
                                          <SelectItem 
                                             key={tenant.embedded_branch.id} 
                                             value={tenant.embedded_branch.id}
                                             className="py-2.5"
                                          >
                                             <div className="flex flex-col">
                                                <span className="font-medium">{tenant.name}</span>
                                                <span className="text-[10px] text-gray-400 capitalize">{tenant.embedded_branch.name}</span>
                                             </div>
                                          </SelectItem>
                                       ))}
                                 </SelectContent>
                              </Select>
                              <FormMessage className="text-xs" />
                           </FormItem>
                        )}
                     />

                     <FormField
                        control={form.control}
                        name="request_quantity"
                        render={({ field }) => (
                           <FormItem className="space-y-1.5">
                              <FormLabel className="text-sm font-semibold text-gray-700">Desired Quantity</FormLabel>
                              <FormControl>
                                 <div className="relative">
                                    <Input 
                                       type="number" 
                                       className="bg-gray-50 border-gray-200 focus:ring-indigo-500 pr-12"
                                       {...field} 
                                       min={1} 
                                       max={sku.available_in_stock} 
                                    />
                                    <div className="absolute right-3 top-1/2 -translate-y-1/2 text-[10px] font-bold text-gray-400 uppercase">
                                       Units
                                    </div>
                                 </div>
                              </FormControl>
                              <div className="flex justify-between items-center px-1">
                                 <p className="text-[10px] text-gray-400 italic">Available in source: {sku.available_in_stock}</p>
                              </div>
                              <FormMessage className="text-xs" />
                           </FormItem>
                        )}
                     />
                  </div>

                  <DialogFooter className="pt-2">
                     <Button 
                        type="submit" 
                        className="w-full h-11 bg-indigo-600 hover:bg-indigo-700 text-white font-bold rounded-xl shadow-lg shadow-indigo-200 transition-all hover:shadow-indigo-300 active:scale-[0.98]" 
                        disabled={createSkuRequestState.isLoading}
                     >
                        {createSkuRequestState.isLoading ? (
                           <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                        ) : (
                           <Send className="mr-2 h-4 w-4" />
                        )}
                        Send Application Request
                     </Button>
                  </DialogFooter>
               </form>
            </Form>
         </DialogContent>
      </Dialog>
   );
}
