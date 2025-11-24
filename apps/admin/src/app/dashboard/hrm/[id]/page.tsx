/* eslint-disable react/no-unescaped-entities */
'use client';

import { useCallback, useEffect, useMemo, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import {
   ArrowLeft,
   Mail,
   Phone,
   Calendar,
   User,
   Building2,
   Edit,
   Save,
   X,
} from 'lucide-react';
import Image from 'next/image';

import {
   Card,
   CardContent,
   CardDescription,
   CardHeader,
   CardTitle,
} from '@components/ui/card';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import { Separator } from '@components/ui/separator';
import {
   Form,
   FormControl,
   FormField,
   FormItem,
   FormLabel,
   FormMessage,
} from '@components/ui/form';
import { Input } from '@components/ui/input';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { LoadingOverlay } from '@components/loading-overlay';
import useUserService from '~/src/hooks/api/use-user-service';
import { TUser } from '~/src/domain/types/identity';
import { Gender } from '~/src/domain/enums/gender.enum';
import { cn } from '~/src/infrastructure/lib/utils';
import type { IUpdateProfileByIdPayload } from '~/src/infrastructure/services/user.service';

const updateProfileSchema = z.object({
   first_name: z
      .string()
      .min(1, 'First name must be at least 1 character')
      .nullable()
      .optional(),
   last_name: z
      .string()
      .min(1, 'Last name must be at least 1 character')
      .nullable()
      .optional(),
   phone_number: z
      .string()
      .refine(
         (val) =>
            !val ||
            val === '' ||
            /^[+]?[(]?[0-9]{1,4}[)]?[-\s.]?[(]?[0-9]{1,4}[)]?[-\s.]?[0-9]{1,9}$/.test(
               val,
            ),
         {
            message: 'Invalid phone number format',
         },
      )
      .nullable()
      .optional(),
   birthday: z.string().nullable().optional(),
   gender: z
      .enum([Gender.MALE, Gender.FEMALE, Gender.OTHER])
      .nullable()
      .optional(),
});

type UpdateProfileFormValues = z.infer<typeof updateProfileSchema>;

const dateFormatter = new Intl.DateTimeFormat('en-US', {
   dateStyle: 'medium',
   timeStyle: 'short',
});

const dateOnlyFormatter = new Intl.DateTimeFormat('en-US', {
   dateStyle: 'medium',
});

// Helper function to get gender badge style
const getGenderStyle = (gender: string) => {
   switch (gender?.toUpperCase()) {
      case 'MALE':
         return 'bg-blue-100 text-blue-800 border-blue-300';
      case 'FEMALE':
         return 'bg-pink-100 text-pink-800 border-pink-300';
      case 'OTHER':
         return 'bg-gray-100 text-gray-800 border-gray-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const StaffDetailPage = () => {
   const params = useParams<{ id: string }>();
   const router = useRouter();

   const {
      getUserByUserIdAsync,
      updateProfileByUserIdAsync,
      getUserByUserIdQueryState,
      isLoading,
   } = useUserService();

   // Use query state data directly instead of local state for automatic refetching
   const user = getUserByUserIdQueryState.data as TUser | undefined;
   const [isEditMode, setIsEditMode] = useState(false);

   const form = useForm<UpdateProfileFormValues>({
      resolver: zodResolver(updateProfileSchema),
      defaultValues: {
         first_name: null,
         last_name: null,
         phone_number: null,
         birthday: null,
         gender: null,
      },
   });

   const userId = useMemo(() => {
      const id = params?.id;
      if (Array.isArray(id)) {
         return id[0];
      }
      return id ?? '';
   }, [params]);

   const fetchUserDetails = useCallback(async () => {
      if (!userId) {
         return;
      }

      // Trigger the query - RTK Query will handle caching and automatic refetching
      // The data will be available via getUserByUserIdQueryState.data
      await getUserByUserIdAsync(userId);
   }, [userId, getUserByUserIdAsync]);

   useEffect(() => {
      fetchUserDetails();
   }, [fetchUserDetails]);

   // Initialize form when user data is available
   useEffect(() => {
      if (user) {
         form.reset({
            first_name: user.profile?.first_name || null,
            last_name: user.profile?.last_name || null,
            phone_number: user.profile?.phone_number || null,
            birthday: user.profile?.birth_day
               ? user.profile.birth_day.split('T')[0]
               : null,
            gender: (user.profile?.gender as Gender) || null,
         });
      }
   }, [user, form]);

   const onSubmit = useCallback(
      async (data: UpdateProfileFormValues) => {
         if (!userId || !user) {
            return;
         }

         const payload: IUpdateProfileByIdPayload = {
            first_name: data.first_name || null,
            last_name: data.last_name || null,
            phone_number: data.phone_number || null,
            birthday: data.birthday || null,
            gender: data.gender || null,
         };

         const result = await updateProfileByUserIdAsync(userId, payload);

         if (result.isSuccess) {
            setIsEditMode(false);
            // No need to manually refetch - RTK Query will automatically refetch
            // when invalidatesTags: ['Users'] triggers
         }
      },
      [userId, updateProfileByUserIdAsync, user],
   );

   const handleCancel = useCallback(() => {
      if (user) {
         // Reset form data to current user data
         form.reset({
            first_name: user.profile?.first_name || null,
            last_name: user.profile?.last_name || null,
            phone_number: user.profile?.phone_number || null,
            birthday: user.profile?.birth_day
               ? user.profile.birth_day.split('T')[0]
               : null,
            gender: (user.profile?.gender as Gender) || null,
         });
      }
      setIsEditMode(false);
   }, [user, form]);

   if (!user && !isLoading && getUserByUserIdQueryState.isError) {
      return (
         <div className="p-5">
            <Button
               variant="ghost"
               onClick={() => router.back()}
               className="mb-4"
            >
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>
            <Card>
               <CardHeader>
                  <CardTitle>Staff Not Found</CardTitle>
                  <CardDescription>
                     The staff member you're looking for doesn't exist or has
                     been deleted.
                  </CardDescription>
               </CardHeader>
            </Card>
         </div>
      );
   }

   return (
      <div className="p-5">
         <div className="mb-4 flex items-center justify-between">
            <Button variant="ghost" onClick={() => router.back()}>
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>
            {user && !isEditMode && (
               <Button onClick={() => setIsEditMode(true)}>
                  <Edit className="mr-2 h-4 w-4" />
                  Edit Profile
               </Button>
            )}
         </div>

         <LoadingOverlay isLoading={isLoading}>
            {user && (
               <div className="space-y-6">
                  {/* Header Card */}
                  <Card>
                     <CardHeader>
                        <div className="flex items-start justify-between">
                           <div className="flex items-center gap-4">
                              {user.profile?.image_url ? (
                                 <div className="relative h-24 w-24 rounded-full overflow-hidden border-2 border-gray-200">
                                    <Image
                                       src={user.profile.image_url}
                                       alt={
                                          user.profile.full_name ||
                                          'Profile picture'
                                       }
                                       fill
                                       className="object-cover"
                                    />
                                 </div>
                              ) : (
                                 <div className="flex h-24 w-24 items-center justify-center rounded-full bg-gray-100 border-2 border-gray-200">
                                    <User className="h-12 w-12 text-gray-400" />
                                 </div>
                              )}
                              <div>
                                 <CardTitle className="text-2xl">
                                    {user.profile?.full_name || 'N/A'}
                                 </CardTitle>
                                 <CardDescription className="mt-1">
                                    {user.email}
                                 </CardDescription>
                                 {user.profile?.gender && (
                                    <div className="mt-2">
                                       <Badge
                                          variant="outline"
                                          className={cn(
                                             'capitalize',
                                             getGenderStyle(
                                                user.profile.gender,
                                             ),
                                          )}
                                       >
                                          {user.profile.gender.toLowerCase()}
                                       </Badge>
                                    </div>
                                 )}
                              </div>
                           </div>
                        </div>
                     </CardHeader>
                  </Card>

                  <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                     {/* Profile Information */}
                     <Card>
                        <CardHeader>
                           <CardTitle>Profile Information</CardTitle>
                           <CardDescription>
                              Personal details and contact information
                           </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                           {!isEditMode ? (
                              <>
                                 <div className="space-y-2">
                                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                       <User className="h-4 w-4" />
                                       <span className="font-medium">
                                          Full Name
                                       </span>
                                    </div>
                                    <p className="text-base font-medium capitalize">
                                       {user.profile?.full_name || 'N/A'}
                                    </p>
                                 </div>

                                 <Separator />

                                 <div className="space-y-2">
                                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                       <User className="h-4 w-4" />
                                       <span className="font-medium">
                                          First Name
                                       </span>
                                    </div>
                                    <p className="text-base font-medium capitalize">
                                       {user.profile?.first_name || 'N/A'}
                                    </p>
                                 </div>

                                 <Separator />

                                 <div className="space-y-2">
                                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                       <User className="h-4 w-4" />
                                       <span className="font-medium">
                                          Last Name
                                       </span>
                                    </div>
                                    <p className="text-base font-medium capitalize">
                                       {user.profile?.last_name || 'N/A'}
                                    </p>
                                 </div>

                                 <Separator />

                                 <div className="space-y-2">
                                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                       <Phone className="h-4 w-4" />
                                       <span className="font-medium">
                                          Phone Number
                                       </span>
                                    </div>
                                    <p className="text-base font-medium">
                                       {user.profile?.phone_number ||
                                          user.phone_number ||
                                          'N/A'}
                                    </p>
                                 </div>

                                 <Separator />

                                 <div className="space-y-2">
                                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                       <Calendar className="h-4 w-4" />
                                       <span className="font-medium">
                                          Birthday
                                       </span>
                                    </div>
                                    <p className="text-base font-medium">
                                       {user.profile?.birth_day
                                          ? dateOnlyFormatter.format(
                                               new Date(user.profile.birth_day),
                                            )
                                          : 'N/A'}
                                    </p>
                                 </div>

                                 <Separator />

                                 <div className="space-y-2">
                                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                       <User className="h-4 w-4" />
                                       <span className="font-medium">
                                          Gender
                                       </span>
                                    </div>
                                    {user.profile?.gender ? (
                                       <Badge
                                          variant="outline"
                                          className={cn(
                                             'capitalize',
                                             getGenderStyle(
                                                user.profile.gender,
                                             ),
                                          )}
                                       >
                                          {user.profile.gender.toLowerCase()}
                                       </Badge>
                                    ) : (
                                       <p className="text-base font-medium">
                                          N/A
                                       </p>
                                    )}
                                 </div>
                              </>
                           ) : (
                              <Form {...form}>
                                 <form
                                    onSubmit={form.handleSubmit(onSubmit)}
                                    className="space-y-4"
                                 >
                                    <FormField
                                       control={form.control}
                                       name="first_name"
                                       render={({ field }) => (
                                          <FormItem>
                                             <FormLabel>
                                                <div className="flex items-center gap-2">
                                                   <User className="h-4 w-4" />
                                                   <span>First Name</span>
                                                </div>
                                             </FormLabel>
                                             <FormControl>
                                                <Input
                                                   placeholder="Enter first name"
                                                   {...field}
                                                   value={field.value || ''}
                                                />
                                             </FormControl>
                                             <FormMessage />
                                          </FormItem>
                                       )}
                                    />

                                    <Separator />

                                    <FormField
                                       control={form.control}
                                       name="last_name"
                                       render={({ field }) => (
                                          <FormItem>
                                             <FormLabel>
                                                <div className="flex items-center gap-2">
                                                   <User className="h-4 w-4" />
                                                   <span>Last Name</span>
                                                </div>
                                             </FormLabel>
                                             <FormControl>
                                                <Input
                                                   placeholder="Enter last name"
                                                   {...field}
                                                   value={field.value || ''}
                                                />
                                             </FormControl>
                                             <FormMessage />
                                          </FormItem>
                                       )}
                                    />

                                    <Separator />

                                    <FormField
                                       control={form.control}
                                       name="phone_number"
                                       render={({ field }) => (
                                          <FormItem>
                                             <FormLabel>
                                                <div className="flex items-center gap-2">
                                                   <Phone className="h-4 w-4" />
                                                   <span>Phone Number</span>
                                                </div>
                                             </FormLabel>
                                             <FormControl>
                                                <Input
                                                   type="tel"
                                                   placeholder="Enter phone number"
                                                   {...field}
                                                   value={field.value || ''}
                                                />
                                             </FormControl>
                                             <FormMessage />
                                          </FormItem>
                                       )}
                                    />

                                    <Separator />

                                    <FormField
                                       control={form.control}
                                       name="birthday"
                                       render={({ field }) => (
                                          <FormItem>
                                             <FormLabel>
                                                <div className="flex items-center gap-2">
                                                   <Calendar className="h-4 w-4" />
                                                   <span>Birthday</span>
                                                </div>
                                             </FormLabel>
                                             <FormControl>
                                                <Input
                                                   type="date"
                                                   {...field}
                                                   value={field.value || ''}
                                                />
                                             </FormControl>
                                             <FormMessage />
                                          </FormItem>
                                       )}
                                    />

                                    <Separator />

                                    <FormField
                                       control={form.control}
                                       name="gender"
                                       render={({ field }) => (
                                          <FormItem>
                                             <FormLabel>
                                                <div className="flex items-center gap-2">
                                                   <User className="h-4 w-4" />
                                                   <span>Gender</span>
                                                </div>
                                             </FormLabel>
                                             <Select
                                                onValueChange={field.onChange}
                                                value={field.value || ''}
                                             >
                                                <FormControl>
                                                   <SelectTrigger>
                                                      <SelectValue placeholder="Select gender" />
                                                   </SelectTrigger>
                                                </FormControl>
                                                <SelectContent>
                                                   <SelectItem
                                                      value={Gender.MALE}
                                                   >
                                                      Male
                                                   </SelectItem>
                                                   <SelectItem
                                                      value={Gender.FEMALE}
                                                   >
                                                      Female
                                                   </SelectItem>
                                                   <SelectItem
                                                      value={Gender.OTHER}
                                                   >
                                                      Other
                                                   </SelectItem>
                                                </SelectContent>
                                             </Select>
                                             <FormMessage />
                                          </FormItem>
                                       )}
                                    />

                                    <Separator />

                                    <div className="flex items-center gap-2 pt-2">
                                       <Button type="submit" className="flex-1">
                                          <Save className="mr-2 h-4 w-4" />
                                          Save Changes
                                       </Button>
                                       <Button
                                          type="button"
                                          variant="outline"
                                          onClick={handleCancel}
                                          className="flex-1"
                                       >
                                          <X className="mr-2 h-4 w-4" />
                                          Cancel
                                       </Button>
                                    </div>
                                 </form>
                              </Form>
                           )}
                        </CardContent>
                     </Card>

                     {/* Account Information */}
                     <Card>
                        <CardHeader>
                           <CardTitle>Account Information</CardTitle>
                           <CardDescription>
                              Account details and system information
                           </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Mail className="h-4 w-4" />
                                 <span className="font-medium">Email</span>
                              </div>
                              <p className="text-base font-medium">
                                 {user.email || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <User className="h-4 w-4" />
                                 <span className="font-medium">Username</span>
                              </div>
                              <p className="text-base font-medium">
                                 {user.user_name || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Mail className="h-4 w-4" />
                                 <span className="font-medium">
                                    Email Verified
                                 </span>
                              </div>
                              <Badge
                                 variant="outline"
                                 className={
                                    user.email_confirmed
                                       ? 'bg-green-100 text-green-800 border-green-300'
                                       : 'bg-red-100 text-red-800 border-red-300'
                                 }
                              >
                                 {user.email_confirmed
                                    ? 'Verified'
                                    : 'Not Verified'}
                              </Badge>
                           </div>

                           <Separator />

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Phone className="h-4 w-4" />
                                 <span className="font-medium">
                                    Phone Number
                                 </span>
                              </div>
                              <p className="text-base font-medium">
                                 {user.phone_number || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Building2 className="h-4 w-4" />
                                 <span className="font-medium">Tenant ID</span>
                              </div>
                              <p className="text-sm font-medium font-mono">
                                 {user.tenant_id || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Building2 className="h-4 w-4" />
                                 <span className="font-medium">Branch ID</span>
                              </div>
                              <p className="text-sm font-medium font-mono">
                                 {user.branch_id || 'N/A'}
                              </p>
                           </div>
                        </CardContent>
                     </Card>
                  </div>

                  {/* System Information */}
                  <Card>
                     <CardHeader>
                        <CardTitle>System Information</CardTitle>
                        <CardDescription>
                           Timestamps and system metadata
                        </CardDescription>
                     </CardHeader>
                     <CardContent className="space-y-4">
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Calendar className="h-4 w-4" />
                                 <span className="font-medium">Created At</span>
                              </div>
                              <p className="text-base font-medium">
                                 {user.created_at
                                    ? dateFormatter.format(
                                         new Date(user.created_at),
                                      )
                                    : 'N/A'}
                              </p>
                           </div>

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Calendar className="h-4 w-4" />
                                 <span className="font-medium">Updated At</span>
                              </div>
                              <p className="text-base font-medium">
                                 {user.updated_at
                                    ? dateFormatter.format(
                                         new Date(user.updated_at),
                                      )
                                    : 'N/A'}
                              </p>
                           </div>

                           {user.updated_by && (
                              <div className="space-y-2">
                                 <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                    <User className="h-4 w-4" />
                                    <span className="font-medium">
                                       Updated By
                                    </span>
                                 </div>
                                 <p className="text-sm font-medium font-mono">
                                    {user.updated_by}
                                 </p>
                              </div>
                           )}

                           {user.is_deleted && (
                              <div className="space-y-2">
                                 <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                    <Calendar className="h-4 w-4" />
                                    <span className="font-medium">
                                       Deleted At
                                    </span>
                                 </div>
                                 <p className="text-base font-medium">
                                    {user.deleted_at
                                       ? dateFormatter.format(
                                            new Date(user.deleted_at),
                                         )
                                       : 'N/A'}
                                 </p>
                              </div>
                           )}

                           {user.is_deleted && user.deleted_by && (
                              <div className="space-y-2">
                                 <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                    <User className="h-4 w-4" />
                                    <span className="font-medium">
                                       Deleted By
                                    </span>
                                 </div>
                                 <p className="text-sm font-medium font-mono">
                                    {user.deleted_by}
                                 </p>
                              </div>
                           )}
                        </div>

                        {user.is_deleted && (
                           <div className="mt-4">
                              <Badge
                                 variant="outline"
                                 className="bg-red-100 text-red-800 border-red-300"
                              >
                                 Deleted
                              </Badge>
                           </div>
                        )}
                     </CardContent>
                  </Card>
               </div>
            )}
         </LoadingOverlay>
      </div>
   );
};

export default StaffDetailPage;
