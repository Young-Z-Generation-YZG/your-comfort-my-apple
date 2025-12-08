'use client';

import { Building2, Plus } from 'lucide-react';
import { Fragment, useEffect, useMemo } from 'react';
import { useRouter } from 'next/navigation';
import { Button } from '@components/ui/button';

import TenantCard from './_components/tenant-card';
import useTenantService from '~/src/hooks/api/use-tenant-service';
import { LoadingOverlay } from '@components/loading-overlay';
import { TTenant } from '~/src/domain/types/catalog.type';

const TenantsPage = () => {
   const router = useRouter();
   const { getListTenantsAsync, getListTenantsState, isLoading } =
      useTenantService();

   // Get tenants from API response
   const tenants = useMemo(() => {
      if (!getListTenantsState.isSuccess || !getListTenantsState.data) {
         return [];
      }
      return getListTenantsState.data as TTenant[];
   }, [getListTenantsState.isSuccess, getListTenantsState.data]);

   useEffect(() => {
      getListTenantsAsync();
   }, [getListTenantsAsync]);

   return (
      <Fragment>
         <div className="p-4">
            <div className="flex flex-col gap-6 p-6">
               <div className="flex items-center justify-between">
                  <div>
                     <h1 className="text-3xl font-bold tracking-tight">
                        Tenant Management
                     </h1>
                     <p className="text-muted-foreground">
                        Manage warehouses and branch locations
                     </p>
                  </div>
                  <Button
                     onClick={() =>
                        router.push('/dashboard/tenant-management/create')
                     }
                  >
                     <Plus className="mr-2 h-4 w-4" />
                     Create Tenant
                  </Button>
               </div>

               <LoadingOverlay isLoading={isLoading}>
                  <div className="p-8">
                     {/* Cards Grid */}
                     {tenants.length > 0 ? (
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                           {tenants.map((tenant) => (
                              <TenantCard key={tenant.id} tenant={tenant} />
                           ))}
                        </div>
                     ) : (
                        <div className="flex flex-col items-center justify-center py-16 bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700">
                           <Building2 className="h-16 w-16 text-muted-foreground/50 mb-4" />
                           <p className="font-medium text-lg mb-1">
                              {isLoading
                                 ? 'Loading tenants...'
                                 : 'No tenants found'}
                           </p>
                           <p className="text-sm text-muted-foreground">
                              {isLoading
                                 ? 'Please wait'
                                 : 'Try adjusting your search query'}
                           </p>
                        </div>
                     )}

                     {/* Total Count */}
                     {tenants.length > 0 && (
                        <div className="flex items-center justify-between mt-4">
                           <div className="text-muted-foreground text-sm">
                              Showing {tenants.length} total tenants
                           </div>
                        </div>
                     )}
                  </div>
               </LoadingOverlay>
            </div>
         </div>
      </Fragment>
   );
};

export default TenantsPage;
