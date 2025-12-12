'use client';

import { useMemo, useState, useEffect, useRef } from 'react';
import useTenantService from '~/src/hooks/api/use-tenant-service';
import { TTenant } from '~/src/domain/types/catalog.type';
import { useForm, type SubmitHandler } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import {
   Form,
   FormControl,
   FormField,
   FormItem,
   FormLabel,
   FormMessage,
} from '@components/ui/form';
import { Button } from '@components/ui/button';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '@components/ui/dialog';
import { Input } from '@components/ui/input';
import { Label } from '@components/ui/label';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '@components/ui/popover';
import { Calendar } from '@components/ui/calendar';
import { CalendarIcon, Plus } from 'lucide-react';
import { format } from 'date-fns';
import { cn } from '~/src/infrastructure/lib/utils';
import useAuthService from '~/src/hooks/api/use-auth-service';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { IAddNewStaffPayload } from '~/src/domain/types/identity.type';
import { ERole } from '~/src/domain/enums/role.enum';

/// Add New Staff Schema
const AddNewStaffSchema = z.object({
   email: z
      .string()
      .min(1, { message: 'Email is required' })
      .email({ message: 'Invalid email format' }),
   password: z
      .string()
      .min(1, { message: 'Password is required' })
      .min(6, { message: 'Password must be at least 6 characters' }),
   birth_day: z.date({
      required_error: 'Birth day is required.',
   }),
   first_name: z.string().min(1, { message: 'First name is required' }),
   last_name: z.string().min(1, { message: 'Last name is required' }),
   phone_number: z
      .string()
      .min(1, { message: 'Phone number is required' })
      .regex(/^\d+$/, { message: 'Phone number must contain digits only' }),
   role_name: z.string().min(1, { message: 'Role is required' }),
   tenant_id: z.string().optional(),
   branch_id: z.string().optional(),
} satisfies Record<keyof any, any>);

// Input types (before transform) are used for react-hook-form typing
type TAddNewStaffForm = z.input<typeof AddNewStaffSchema>;
const addNewStaffResolver = zodResolver(AddNewStaffSchema);

/// Add New Staff Schema with Super Admin validation
const AddNewStaffSuperAdminSchema = AddNewStaffSchema.refine(
   (data) => data.tenant_id !== undefined && data.tenant_id !== '',
   {
      message: 'Tenant ID is required',
      path: ['tenant_id'],
   },
).refine((data) => data.branch_id !== undefined && data.branch_id !== '', {
   message: 'Branch ID is required',
   path: ['branch_id'],
});

const addNewStaffSuperAdminResolver = zodResolver(AddNewStaffSuperAdminSchema);

interface AddNewStaffDialogProps {
   onSuccess?: () => void;
   roles?: Array<{ id: string; name: string }>;
}

export default function AddNewStaffDialog({
   onSuccess,
   roles = [
      { id: 'STAFF', name: 'Staff' },
      //   { id: 'MANAGER', name: 'Manager' },
      { id: 'ADMIN', name: 'Admin' },
   ],
}: AddNewStaffDialogProps) {
   const [open, setOpen] = useState(false);
   const dialogContentRef = useRef<HTMLDivElement | null>(null);
   const [popoverContainer, setPopoverContainer] = useState<HTMLElement | null>(
      null,
   );

   const { addNewStaffAsync, addNewStaffState } = useAuthService();
   const { getListTenantsAsync, isLoading: isLoadingTenants } =
      useTenantService();
   const [tenants, setTenants] = useState<TTenant[]>([]);
   const isLoading = addNewStaffState.isLoading || isLoadingTenants;
   const { tenantId, branchId } = useAppSelector((state) => state.tenant);
   const { currentUser, impersonatedUser, defaultTenantId, defaultBranchId } =
      useAppSelector((state) => state.auth);

   const isSuperAdmin = useMemo(() => {
      const activeRoles = impersonatedUser?.roles ?? currentUser?.roles ?? [];
      return Array.isArray(activeRoles)
         ? activeRoles.includes(ERole.ADMIN_SUPER)
         : false;
   }, [currentUser?.roles, impersonatedUser?.roles]);

   const isAdmin = useMemo(() => {
      const activeRoles = impersonatedUser?.roles ?? currentUser?.roles ?? [];
      return Array.isArray(activeRoles)
         ? activeRoles.includes(ERole.ADMIN)
         : false;
   }, [currentUser?.roles, impersonatedUser?.roles]);

   // Use appropriate schema based on user role
   const resolver = isSuperAdmin
      ? addNewStaffSuperAdminResolver
      : addNewStaffResolver;

   const defaultTenant = tenantId ?? defaultTenantId ?? '';
   const defaultBranch = branchId ?? defaultBranchId ?? '';

   const showTenantBranch = isSuperAdmin || isAdmin;
   const disableTenantBranch = !isSuperAdmin && showTenantBranch;

   const form = useForm<TAddNewStaffForm>({
      resolver,
      defaultValues: {
         email: '',
         password: '',
         first_name: '',
         last_name: '',
         phone_number: '',
         role_name: 'STAFF',
         tenant_id: isSuperAdmin ? '' : defaultTenant,
         branch_id: isSuperAdmin ? '' : defaultBranch,
      },
   });
   const {
      formState: { errors },
   } = form;

   useEffect(() => {
      if (open && dialogContentRef.current) {
         setPopoverContainer(dialogContentRef.current);
      } else {
         setPopoverContainer(null);
      }
   }, [open]);

   // Fetch tenants when dialog opens
   useEffect(() => {
      if (!showTenantBranch || !open) return;
      const fetchTenants = async () => {
         const res = await getListTenantsAsync();
         if (res.isSuccess && res.data) {
            setTenants(res.data);
         }
      };
      fetchTenants();
   }, [getListTenantsAsync, open, showTenantBranch]);

   const selectedTenantId = form.watch('tenant_id');
   const selectedTenant = tenants.find((t) => t.id === selectedTenantId);
   const selectedBranchName = selectedTenant?.embedded_branch.name ?? '';

   // Keep branch_id in sync with selected tenant's embedded branch
   useEffect(() => {
      if (selectedTenant) {
         form.setValue('branch_id', selectedTenant.embedded_branch.id);
      } else {
         form.setValue('branch_id', '');
      }
   }, [selectedTenant, form]);

   const onSubmit: SubmitHandler<TAddNewStaffForm> = async (data) => {
      const submitData: IAddNewStaffPayload = {
         ...data,
         birth_day: data.birth_day.toISOString(),
         tenant_id: isSuperAdmin
            ? data.tenant_id || ''
            : tenantId || data.tenant_id || '',
         branch_id: isSuperAdmin
            ? data.branch_id || ''
            : branchId || data.branch_id || '',
      };

      const result = await addNewStaffAsync(submitData);

      if (result.isSuccess) {
         setOpen(false);
         form.reset();
         onSuccess?.();
      }
   };

   const handleOpenChange = (newOpen: boolean) => {
      setOpen(newOpen);
      if (!newOpen) {
         form.reset();
      }
   };

   return (
      <Dialog open={open} onOpenChange={handleOpenChange}>
         <DialogTrigger asChild>
            <Button className="gap-2">
               <Plus className="h-4 w-4" />
               Add New Staff
            </Button>
         </DialogTrigger>
         <DialogContent
            ref={dialogContentRef}
            className="sm:max-w-[500px] rounded-md"
         >
            <DialogHeader>
               <DialogTitle>Add New Staff Member</DialogTitle>
            </DialogHeader>

            <Form {...form}>
               <form
                  onSubmit={form.handleSubmit(onSubmit)}
                  className="space-y-4"
               >
                  <FormField
                     control={form.control}
                     name="email"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Email *</FormLabel>
                           <FormControl>
                              <Input
                                 type="email"
                                 placeholder="staff@example.com"
                                 {...field}
                                 disabled={isLoading}
                              />
                           </FormControl>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="password"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Password *</FormLabel>
                           <FormControl>
                              <Input
                                 type="password"
                                 placeholder="Enter password"
                                 {...field}
                                 disabled={isLoading}
                              />
                           </FormControl>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="first_name"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>First Name *</FormLabel>
                           <FormControl>
                              <Input
                                 placeholder="John"
                                 {...field}
                                 disabled={isLoading}
                              />
                           </FormControl>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="last_name"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Last Name *</FormLabel>
                           <FormControl>
                              <Input
                                 placeholder="Doe"
                                 {...field}
                                 disabled={isLoading}
                              />
                           </FormControl>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="birth_day"
                     render={({ field }) => (
                        <FormItem className="flex flex-col">
                           <FormLabel>Birth Day *</FormLabel>
                           <Popover modal={false}>
                              <PopoverTrigger asChild>
                                 <FormControl>
                                    <Button
                                       variant="outline"
                                       className={cn(
                                          'w-full pl-3 text-left font-normal',
                                          !field.value &&
                                             'text-muted-foreground',
                                          isLoading &&
                                             'opacity-50 cursor-not-allowed',
                                       )}
                                       disabled={isLoading}
                                    >
                                       {(() => {
                                          const dateValue = field.value
                                             ? new Date(
                                                  field.value as unknown as
                                                     | string
                                                     | number
                                                     | Date,
                                               )
                                             : undefined;
                                          return dateValue ? (
                                             format(dateValue, 'PPP')
                                          ) : (
                                             <span>Pick a birth date</span>
                                          );
                                       })()}
                                       <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                    </Button>
                                 </FormControl>
                              </PopoverTrigger>
                              <PopoverContent
                                 className="w-auto p-0"
                                 align="start"
                                 onInteractOutside={(e) => {
                                    // Prevent closing when clicking inside the dialog
                                    e.preventDefault();
                                 }}
                                 container={popoverContainer ?? undefined}
                              >
                                 {(() => {
                                    const dateValue = field.value
                                       ? new Date(
                                            field.value as unknown as
                                               | string
                                               | number
                                               | Date,
                                         )
                                       : undefined;
                                    return (
                                       <Calendar
                                          mode="single"
                                          selected={dateValue}
                                          onSelect={(date) =>
                                             field.onChange(date)
                                          }
                                          disabled={(date) =>
                                             date > new Date() || isLoading
                                          }
                                          initialFocus
                                       />
                                    );
                                 })()}
                              </PopoverContent>
                           </Popover>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="phone_number"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Phone Number *</FormLabel>
                           <FormControl>
                              <Input
                                 type="tel"
                                 placeholder="0987654321"
                                 {...field}
                                 disabled={isLoading}
                              />
                           </FormControl>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="role_name"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Role *</FormLabel>
                           <Select
                              value={field.value}
                              onValueChange={field.onChange}
                           >
                              <FormControl>
                                 <SelectTrigger>
                                    <SelectValue placeholder="Select a role" />
                                 </SelectTrigger>
                              </FormControl>
                              <SelectContent>
                                 {roles.map((role) => (
                                    <SelectItem key={role.id} value={role.id}>
                                       {role.name}
                                    </SelectItem>
                                 ))}
                              </SelectContent>
                           </Select>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  {showTenantBranch && (
                     <div className="grid grid-cols-1 gap-4">
                        <FormField
                           control={form.control}
                           name="tenant_id"
                           render={({ field }) => (
                              <FormItem>
                                 <FormLabel>
                                    Tenant ID {isSuperAdmin ? '*' : ''}
                                 </FormLabel>
                                 <Select
                                    value={field.value ?? ''}
                                    onValueChange={field.onChange}
                                    disabled={disableTenantBranch}
                                 >
                                    <FormControl>
                                       <SelectTrigger
                                          className={
                                             errors.tenant_id && isSuperAdmin
                                                ? 'border-red-500'
                                                : ''
                                          }
                                       >
                                          <SelectValue placeholder="Select tenant" />
                                       </SelectTrigger>
                                    </FormControl>
                                    <SelectContent>
                                       {tenants.map((tenant) => (
                                          <SelectItem
                                             key={tenant.id}
                                             value={tenant.id}
                                          >
                                             {tenant.name}
                                          </SelectItem>
                                       ))}
                                    </SelectContent>
                                 </Select>
                                 <FormMessage />
                              </FormItem>
                           )}
                        />
                        <div className="space-y-2">
                           <Label htmlFor="branch_name">
                              Branch Name (from tenant)
                           </Label>
                           <Input
                              id="branch_name"
                              value={selectedBranchName}
                              readOnly
                              disabled
                              placeholder="Select tenant to see branch name"
                           />
                           <Label htmlFor="branch_address">
                              Branch Address
                           </Label>
                           <Input
                              id="branch_address"
                              value={
                                 selectedTenant?.embedded_branch.address ?? ''
                              }
                              readOnly
                              disabled
                              placeholder="Select tenant to see branch address"
                           />
                        </div>
                     </div>
                  )}

                  <div className="flex gap-2 justify-end pt-4">
                     <Button
                        type="button"
                        variant="outline"
                        onClick={() => handleOpenChange(false)}
                        disabled={isLoading}
                     >
                        Cancel
                     </Button>
                     <Button type="submit" disabled={isLoading}>
                        {isLoading ? 'Adding...' : 'Add Staff'}
                     </Button>
                  </div>
               </form>
            </Form>
         </DialogContent>
      </Dialog>
   );
}
