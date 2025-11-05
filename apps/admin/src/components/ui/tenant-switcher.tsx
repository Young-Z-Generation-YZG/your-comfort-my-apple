'use client';

import { useEffect, useMemo, useState } from 'react';
import {
   Check,
   ChevronsUpDown,
   GalleryVerticalEnd,
   Search,
} from 'lucide-react';

import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import {
   SidebarMenu,
   SidebarMenuButton,
   SidebarMenuItem,
} from '@components/ui/sidebar';
import { Input } from '@components/ui/input';
import { useDispatch } from 'react-redux';
import { setTenant } from '~/src/infrastructure/redux/features/tenant.slice';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { TTenantItem } from '@components/layouts/sidebar-layout';
import { ERole } from '~/src/domain/enums/role.enum';
import useAuthService from '~/src/hooks/api/use-auth-service';
import { setRoles } from '~/src/infrastructure/redux/features/auth.slice';

export function TenantSwitcher({ tenants }: { tenants: TTenantItem[] }) {
   const { tenantId } = useAppSelector((state) => state.tenant);
   const [selectedTenant, setSelectedTenant] = useState<TTenantItem | null>(
      null,
   );
   const [searchQuery, setSearchQuery] = useState('');

   const { currentUser, impersonatedUser } = useAppSelector(
      (state) => state.auth,
   );

   const dispatch = useDispatch();

   const roles = useMemo(
      () => currentUser?.roles || impersonatedUser?.roles || [],
      [currentUser, impersonatedUser],
   );

   // Check if user has permission to switch tenants
   const canSwitchTenants = useMemo(
      () => roles.includes(ERole.ADMIN_SUPER),
      [roles],
   );

   const filteredTenants = useMemo(() => {
      if (!searchQuery) return tenants;

      return tenants.filter((tenant) =>
         tenant.name.toLowerCase().includes(searchQuery.toLowerCase()),
      );
   }, [tenants, searchQuery]);

   const { getIdentityAsync } = useAuthService();

   // Sync selected tenant when Redux tenantId changes or tenants load
   //    useEffect(() => {
   //       if (tenants.length === 0) return;

   //       if (tenantId) {
   //          const tenant = tenants.find((t) => t.id === tenantId);
   //          if (tenant && tenant.id !== selectedTenant?.id) {
   //             setSelectedTenant(tenant);
   //          }
   //       } else if (!selectedTenant) {
   //          // If no tenantId in Redux, set first tenant
   //          setSelectedTenant(tenants[0]);
   //          dispatch(
   //             setTenant({
   //                tenantId: tenants[0].id,
   //                tenantCode: tenants[0].code,
   //                tenantName: tenants[0].name,
   //             }),
   //          );
   //       }
   //    }, [tenantId, tenants, selectedTenant, dispatch]);

   const handleSelectTenant = (tenant: TTenantItem) => {
      setSelectedTenant(tenant);
      setSearchQuery('');
      dispatch(
         setTenant({
            tenantId: tenant.id,
            tenantSubDomain: tenant.sub_domain,
            tenantName: tenant.name,
         }),
      );
   };

   useEffect(() => {
      const fetchIdentity = async () => {
         const identityResult = await getIdentityAsync();

         if (identityResult.isSuccess && identityResult.data) {
            dispatch(
               setRoles({
                  currentUser: {
                     roles: null,
                  },
                  impersonatedUser: {
                     roles: identityResult.data.roles,
                  },
               }),
            );

            dispatch(
               setTenant({
                  tenantId: identityResult.data.tenant_id,
               }),
            );
         }
      };

      if (impersonatedUser) {
         fetchIdentity();
      }
   }, [impersonatedUser, getIdentityAsync, dispatch]);

   useEffect(() => {
      if (tenants.length === 0) return;

      if (tenantId) {
         setSelectedTenant(tenants.find((t) => t.id === tenantId) || null);
      } else {
         setSelectedTenant(() => {
            return tenants.find((t) => t.sub_domain === 'admin') || null;
         });
      }
   }, [tenants, tenantId, setSelectedTenant]);

   // Don't render if no tenants available
   if (!selectedTenant || tenants.length === 0) {
      return null;
   }

   return (
      <SidebarMenu>
         <SidebarMenuItem>
            {canSwitchTenants ? (
               <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                     <SidebarMenuButton
                        size="lg"
                        className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground"
                     >
                        <div className="bg-sidebar-primary text-sidebar-primary-foreground flex aspect-square size-8 items-center justify-center rounded-lg">
                           <GalleryVerticalEnd className="size-4" />
                        </div>
                        <div className="flex flex-col gap-0.5 leading-none">
                           <span className="font-medium">
                              {selectedTenant.name}
                           </span>
                           <span className="">{selectedTenant.sub_domain}</span>
                        </div>
                        <ChevronsUpDown className="ml-auto" />
                     </SidebarMenuButton>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent
                     className="w-[--radix-dropdown-menu-trigger-width] p-0"
                     side="right"
                     align="start"
                  >
                     <div
                        className="flex items-center border-b px-3 py-2"
                        onKeyDown={(e) => e.stopPropagation()}
                     >
                        <Search className="mr-2 h-4 w-4 shrink-0 opacity-50" />
                        <Input
                           placeholder="Search tenant..."
                           value={searchQuery}
                           onChange={(e) => setSearchQuery(e.target.value)}
                           className="h-8 border-0 focus-visible:ring-0 focus-visible:ring-offset-0"
                           autoFocus
                        />
                     </div>
                     <div className="max-h-[300px] overflow-y-auto p-1">
                        {filteredTenants.length === 0 ? (
                           <div className="py-6 text-center text-sm text-muted-foreground">
                              No tenant found.
                           </div>
                        ) : (
                           filteredTenants.map((tenant) => (
                              <DropdownMenuItem
                                 key={tenant.id}
                                 onSelect={() => handleSelectTenant(tenant)}
                                 className="flex items-center gap-2"
                                 disabled={
                                    tenant.id === tenantId || tenantId === null
                                 }
                              >
                                 <div className="flex flex-col flex-1">
                                    <span className="font-medium">
                                       {tenant.name}
                                    </span>
                                    {tenant.embedded_branch.address && (
                                       <span className="text-xs text-muted-foreground">
                                          {tenant.embedded_branch.address}
                                       </span>
                                    )}
                                 </div>
                                 {tenant.id === selectedTenant.id && (
                                    <Check className="ml-auto" />
                                 )}
                              </DropdownMenuItem>
                           ))
                        )}
                     </div>
                  </DropdownMenuContent>
               </DropdownMenu>
            ) : (
               <SidebarMenuButton size="lg" className="cursor-default">
                  <div className="bg-sidebar-primary text-sidebar-primary-foreground flex aspect-square size-8 items-center justify-center rounded-lg">
                     <GalleryVerticalEnd className="size-4" />
                  </div>
                  <div className="flex flex-col gap-0.5 leading-none">
                     <span className="font-medium">{selectedTenant.name}</span>
                     <span className="text-xs text-muted-foreground">
                        {selectedTenant.sub_domain}
                     </span>
                  </div>
               </SidebarMenuButton>
            )}
         </SidebarMenuItem>
      </SidebarMenu>
   );
}
