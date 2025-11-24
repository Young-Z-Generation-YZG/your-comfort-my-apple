/* eslint-disable react/no-unescaped-entities */
'use client';

import { useCallback, useEffect, useMemo, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import {
   ArrowLeft,
   Mail,
   Phone,
   Calendar,
   User,
   Building2,
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
import { LoadingOverlay } from '@components/loading-overlay';
import { useToast } from '~/src/hooks/use-toast';
import useUserService from '~/src/hooks/api/use-user-service';
import { TUser } from '~/src/domain/types/identity';
import { cn } from '~/src/infrastructure/lib/utils';

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
   const { toast } = useToast();

   const { getUserByUserIdAsync, isLoading } = useUserService();

   const [user, setUser] = useState<TUser | null>(null);

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

      const result = await getUserByUserIdAsync(userId);

      if (result.isSuccess && result.data) {
         setUser(result.data as TUser);
      } else {
         setUser(null);
         toast({
            title: 'Error',
            description: 'Failed to load staff details. Please try again.',
            variant: 'destructive',
         });
      }
   }, [userId, getUserByUserIdAsync, toast]);

   useEffect(() => {
      fetchUserDetails();
   }, [fetchUserDetails]);

   if (!user && !isLoading) {
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
         <div className="mb-4">
            <Button variant="ghost" onClick={() => router.back()}>
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>
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
                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <User className="h-4 w-4" />
                                 <span className="font-medium">Full Name</span>
                              </div>
                              <p className="text-base font-medium capitalize">
                                 {user.profile?.full_name || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <User className="h-4 w-4" />
                                 <span className="font-medium">First Name</span>
                              </div>
                              <p className="text-base font-medium capitalize">
                                 {user.profile?.first_name || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <User className="h-4 w-4" />
                                 <span className="font-medium">Last Name</span>
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
                                 <span className="font-medium">Birthday</span>
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
                                 <span className="font-medium">Gender</span>
                              </div>
                              {user.profile?.gender ? (
                                 <Badge
                                    variant="outline"
                                    className={cn(
                                       'capitalize',
                                       getGenderStyle(user.profile.gender),
                                    )}
                                 >
                                    {user.profile.gender.toLowerCase()}
                                 </Badge>
                              ) : (
                                 <p className="text-base font-medium">N/A</p>
                              )}
                           </div>
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
                              <p className="text-base font-medium font-mono text-sm">
                                 {user.tenant_id || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-2">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Building2 className="h-4 w-4" />
                                 <span className="font-medium">Branch ID</span>
                              </div>
                              <p className="text-base font-medium font-mono text-sm">
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
                                 <p className="text-base font-medium font-mono text-sm">
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
                                 <p className="text-base font-medium font-mono text-sm">
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
