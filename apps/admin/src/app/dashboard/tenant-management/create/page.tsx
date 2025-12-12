'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { Button } from '@components/ui/button';
import {
   Form,
   FormControl,
   FormDescription,
   FormField,
   FormItem,
   FormLabel,
   FormMessage,
} from '@components/ui/form';
import { Input } from '@components/ui/input';
import { Textarea } from '@components/ui/textarea';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { LoadingOverlay } from '@components/loading-overlay';
import useTenantService from '~/src/hooks/api/use-tenant-service';
import { toast } from '~/src/hooks/use-toast';
import { ArrowLeft } from 'lucide-react';
import { ETenantType } from '~/src/domain/enums/tenant-type.enum';

const createTenantSchema = z.object({
   name: z
      .string()
      .min(1, {
         message: 'Name is required.',
      })
      .min(3, {
         message: 'Name must be at least 3 characters.',
      }),
   sub_domain: z
      .string()
      .min(1, {
         message: 'Sub domain is required.',
      })
      .min(3, {
         message: 'Sub domain must be at least 3 characters.',
      })
      .regex(/^[a-z0-9-]+$/, {
         message:
            'Sub domain can only contain lowercase letters, numbers, and hyphens.',
      }),
   branch_address: z
      .string()
      .min(1, {
         message: 'Branch address is required.',
      })
      .min(5, {
         message: 'Branch address must be at least 5 characters.',
      }),
   tenant_type: z.nativeEnum(ETenantType, {
      required_error: 'Tenant type is required.',
   }),
   tenant_description: z.string().optional(),
   branch_description: z.string().optional(),
});

type CreateTenantFormValues = z.infer<typeof createTenantSchema>;

const CreateTenantPage = () => {
   const router = useRouter();
   const { createTenantAsync, isLoading } = useTenantService();
   const [isSubmitting, setIsSubmitting] = useState(false);

   const form = useForm<CreateTenantFormValues>({
      resolver: zodResolver(createTenantSchema),
      defaultValues: {
         name: '',
         sub_domain: '',
         branch_address: '',
         tenant_type: ETenantType.BRANCH,
         tenant_description: '',
         branch_description: '',
      },
   });

   const onSubmit = async (data: CreateTenantFormValues) => {
      setIsSubmitting(true);

      try {
         const payload = {
            name: data.name,
            sub_domain: data.sub_domain,
            branch_address: data.branch_address,
            tenant_type: data.tenant_type,
            tenant_description: data.tenant_description || '',
            branch_description: data.branch_description || '',
         };

         const result = await createTenantAsync(payload);

         if (result.isSuccess) {
            toast({
               title: 'Success',
               description: 'Tenant created successfully!',
            });

            // Navigate back to tenants list after a short delay
            setTimeout(() => {
               router.push('/dashboard/tenant-management');
            }, 1000);
         } else {
            toast({
               variant: 'destructive',
               title: 'Error',
               description: result.error
                  ? (result.error as any)?.data?.message ||
                    'Failed to create tenant'
                  : 'Failed to create tenant. Please try again.',
            });
         }
      } catch (error) {
         console.error('Error creating tenant:', error);
         toast({
            variant: 'destructive',
            title: 'Error',
            description: 'An unexpected error occurred. Please try again.',
         });
      } finally {
         setIsSubmitting(false);
      }
   };

   return (
      <div className="p-5">
         <LoadingOverlay isLoading={isLoading || isSubmitting} />

         <div className="mb-6">
            <Button
               variant="ghost"
               onClick={() => router.back()}
               className="mb-4"
            >
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>

            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Create New Tenant
               </h1>
               <p className="text-muted-foreground mt-1">
                  Create a new tenant (warehouse or branch) with name, sub
                  domain, and address
               </p>
            </div>
         </div>

         <div className="rounded-lg border bg-card p-6 w-auto">
            <Form {...form}>
               <form
                  onSubmit={form.handleSubmit(onSubmit)}
                  className="space-y-6"
               >
                  <FormField
                     control={form.control}
                     name="name"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Tenant Name</FormLabel>
                           <FormControl>
                              <Input
                                 placeholder="e.g., Branch Name"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Enter the name of the tenant (warehouse or
                              branch).
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="sub_domain"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Sub Domain</FormLabel>
                           <FormControl>
                              <Input
                                 placeholder="e.g., main-warehouse"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Enter a unique sub domain (lowercase letters,
                              numbers, and hyphens only).
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="tenant_type"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Tenant Type</FormLabel>
                           <Select
                              onValueChange={field.onChange}
                              defaultValue={field.value}
                              disabled={isSubmitting}
                           >
                              <FormControl>
                                 <SelectTrigger>
                                    <SelectValue placeholder="Select tenant type" />
                                 </SelectTrigger>
                              </FormControl>
                              <SelectContent>
                                 <SelectItem
                                    value={ETenantType.WAREHOUSE}
                                    disabled={true}
                                 >
                                    Warehouse
                                 </SelectItem>
                                 <SelectItem value={ETenantType.BRANCH}>
                                    Branch
                                 </SelectItem>
                              </SelectContent>
                           </Select>
                           <FormDescription>
                              Select whether this is a warehouse or branch.
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="branch_address"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Branch Address</FormLabel>
                           <FormControl>
                              <Input
                                 placeholder="e.g., 123 Main Street, City, State"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Enter the physical address of the branch or
                              warehouse.
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="tenant_description"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Tenant Description</FormLabel>
                           <FormControl>
                              <Textarea
                                 placeholder="e.g., Branch tenant description..."
                                 className="min-h-[100px]"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Provide a description for the tenant (optional).
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="branch_description"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Branch Description</FormLabel>
                           <FormControl>
                              <Textarea
                                 placeholder="e.g., Branch location details..."
                                 className="min-h-[100px]"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Provide a description for the branch (optional).
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <div className="flex justify-end gap-4">
                     <Button
                        type="button"
                        variant="outline"
                        onClick={() => router.back()}
                        disabled={isSubmitting}
                     >
                        Cancel
                     </Button>
                     <Button type="submit" disabled={isSubmitting}>
                        {isSubmitting ? 'Creating...' : 'Create Tenant'}
                     </Button>
                  </div>
               </form>
            </Form>
         </div>
      </div>
   );
};

export default CreateTenantPage;
