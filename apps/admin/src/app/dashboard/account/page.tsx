'use client';

import { useCallback, useEffect, useMemo } from 'react';
import {
   Calendar,
   Mail,
   Phone,
   ShieldCheck,
   User,
   XCircle,
} from 'lucide-react';

import {
   Card,
   CardContent,
   CardDescription,
   CardHeader,
   CardTitle,
} from '@components/ui/card';
import { Badge } from '@components/ui/badge';
import { Button } from '@components/ui/button';
import { Separator } from '@components/ui/separator';
import { LoadingOverlay } from '@components/loading-overlay';
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

const getGenderStyle = (gender?: string | null) => {
   switch (gender?.toUpperCase()) {
      case 'MALE':
         return 'bg-blue-100 text-blue-800 border-blue-300';
      case 'FEMALE':
         return 'bg-pink-100 text-pink-800 border-pink-300';
      case 'OTHER':
         return 'bg-purple-100 text-purple-800 border-purple-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const AccountPage = () => {
   const { getAccountDetailsAsync, getAccountDetailsQueryState, isLoading } =
      useUserService();

   const account = getAccountDetailsQueryState.data as TUser | undefined;

   const fetchAccountDetails = useCallback(async () => {
      await getAccountDetailsAsync();
   }, [getAccountDetailsAsync]);

   useEffect(() => {
      fetchAccountDetails();
   }, [fetchAccountDetails]);

   const profile = useMemo(() => account?.profile, [account]);

   if (!account && !isLoading && getAccountDetailsQueryState.isError) {
      return (
         <div className="p-5">
            <Card>
               <CardHeader>
                  <CardTitle>Unable to load account</CardTitle>
                  <CardDescription>
                     Something went wrong while fetching your account details.
                     Please try again.
                  </CardDescription>
               </CardHeader>
               <CardContent>
                  <Button onClick={fetchAccountDetails}>Retry</Button>
               </CardContent>
            </Card>
         </div>
      );
   }

   return (
      <div className="p-5">
         <div className="flex items-center justify-between mb-6">
            <div>
               <h1 className="text-3xl font-bold tracking-tight">My Account</h1>
               <p className="text-muted-foreground">
                  View your personal profile and account status
               </p>
            </div>
            <Button variant="outline" onClick={fetchAccountDetails}>
               Refresh
            </Button>
         </div>

         <LoadingOverlay isLoading={isLoading}>
            {account ? (
               <div className="space-y-6">
                  <Card className="overflow-hidden">
                     <div className="bg-gradient-to-r from-slate-50 to-slate-100 p-6">
                        <div className="flex flex-col gap-4 md:flex-row md:items-center md:gap-6">
                           <div className="flex h-24 w-24 items-center justify-center rounded-full bg-white shadow-md">
                              <User className="h-12 w-12 text-gray-400" />
                           </div>
                           <div className="flex-1 space-y-2">
                              <div className="flex flex-wrap items-center gap-3">
                                 <h2 className="text-2xl font-semibold text-gray-900">
                                    {profile?.full_name ||
                                       `${profile?.first_name ?? ''} ${
                                          profile?.last_name ?? ''
                                       }`.trim() ||
                                       account.email}
                                 </h2>
                                 {account.email_confirmed ? (
                                    <Badge
                                       variant="outline"
                                       className="bg-green-100 text-green-800 border-green-300"
                                    >
                                       <ShieldCheck className="mr-1 h-3 w-3" />
                                       Verified
                                    </Badge>
                                 ) : (
                                    <Badge
                                       variant="outline"
                                       className="bg-red-100 text-red-800 border-red-300"
                                    >
                                       <XCircle className="mr-1 h-3 w-3" />
                                       Not Verified
                                    </Badge>
                                 )}
                              </div>
                              <p className="text-muted-foreground">
                                 {account.email}
                              </p>
                              <div className="flex flex-wrap items-center gap-4 text-sm text-gray-600">
                                 {profile?.gender && (
                                    <Badge
                                       variant="outline"
                                       className={cn(
                                          'capitalize',
                                          getGenderStyle(profile.gender),
                                       )}
                                    >
                                       {profile.gender.toLowerCase()}
                                    </Badge>
                                 )}
                                 {profile?.birth_day && (
                                    <span className="flex items-center gap-2">
                                       <Calendar className="h-4 w-4" />
                                       {dateOnlyFormatter.format(
                                          new Date(profile.birth_day),
                                       )}
                                    </span>
                                 )}
                                 {profile?.phone_number && (
                                    <span className="flex items-center gap-2">
                                       <Phone className="h-4 w-4" />
                                       {profile.phone_number}
                                    </span>
                                 )}
                              </div>
                           </div>
                        </div>
                     </div>
                  </Card>

                  <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
                     <Card>
                        <CardHeader>
                           <CardTitle>Profile Information</CardTitle>
                           <CardDescription>
                              Personal details associated with your account
                           </CardDescription>
                        </CardHeader>
                        <CardContent>
                           <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    First Name
                                 </p>
                                 <p className="text-base font-semibold capitalize">
                                    {profile?.first_name || 'N/A'}
                                 </p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Last Name
                                 </p>
                                 <p className="text-base font-semibold capitalize">
                                    {profile?.last_name || 'N/A'}
                                 </p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Username
                                 </p>
                                 <p className="text-base font-semibold">
                                    {account.user_name || 'N/A'}
                                 </p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Phone Number
                                 </p>
                                 <p className="text-base font-semibold">
                                    {profile?.phone_number ||
                                       account.phone_number ||
                                       'N/A'}
                                 </p>
                              </div>
                           </div>
                        </CardContent>
                     </Card>

                     <Card>
                        <CardHeader>
                           <CardTitle>Contact</CardTitle>
                           <CardDescription>
                              Ways we can reach out to you
                           </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-6">
                           <div className="flex items-start gap-3">
                              <div className="rounded-full bg-slate-100 p-2">
                                 <Mail className="h-4 w-4 text-slate-600" />
                              </div>
                              <div>
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Email Address
                                 </p>
                                 <p className="font-semibold break-all">
                                    {account.email || 'N/A'}
                                 </p>
                              </div>
                           </div>
                           <Separator />
                           <div className="flex items-start gap-3">
                              <div className="rounded-full bg-slate-100 p-2">
                                 <Phone className="h-4 w-4 text-slate-600" />
                              </div>
                              <div>
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Phone Number
                                 </p>
                                 <p className="font-semibold">
                                    {profile?.phone_number ||
                                       account.phone_number ||
                                       'N/A'}
                                 </p>
                              </div>
                           </div>
                        </CardContent>
                     </Card>
                  </div>

                  <Card>
                     <CardHeader>
                        <CardTitle>System Information</CardTitle>
                        <CardDescription>
                           Metadata about your account lifecycle
                        </CardDescription>
                     </CardHeader>
                     <CardContent>
                        <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
                           <div className="space-y-1">
                              <p className="text-xs font-medium uppercase tracking-wide text-muted-foreground">
                                 Tenant ID
                              </p>
                              <p className="font-mono text-sm font-semibold">
                                 {account.tenant_id || 'N/A'}
                              </p>
                           </div>
                           <div className="space-y-1">
                              <p className="text-xs font-medium uppercase tracking-wide text-muted-foreground">
                                 Created At
                              </p>
                              <p className="text-sm font-semibold">
                                 {account.created_at
                                    ? dateFormatter.format(
                                         new Date(account.created_at),
                                      )
                                    : 'N/A'}
                              </p>
                           </div>
                           <div className="space-y-1">
                              <p className="text-xs font-medium uppercase tracking-wide text-muted-foreground">
                                 Updated At
                              </p>
                              <p className="text-sm font-semibold">
                                 {account.updated_at
                                    ? dateFormatter.format(
                                         new Date(account.updated_at),
                                      )
                                    : 'N/A'}
                              </p>
                           </div>
                        </div>
                     </CardContent>
                  </Card>
               </div>
            ) : (
               <Card>
                  <CardHeader>
                     <CardTitle>Loading account...</CardTitle>
                     <CardDescription>
                        Please wait while we fetch your details.
                     </CardDescription>
                  </CardHeader>
               </Card>
            )}
         </LoadingOverlay>
      </div>
   );
};

export default AccountPage;
