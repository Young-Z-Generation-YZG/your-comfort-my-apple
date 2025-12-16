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
import { AppDispatch, useAppSelector } from '~/src/infrastructure/redux/store';
import { ERole } from '~/src/domain/enums/role.enum';
import { TTenant } from '~/src/domain/types/catalog.type';
import { orderingApi } from '~/src/infrastructure/services/ordering.service';
import { inventoryApi } from '~/src/infrastructure/services/inventory.service';
import { productApi } from '~/src/infrastructure/services/product.service';
import { categoryApi } from '~/src/infrastructure/services/category.service';
import { notificationApi } from '~/src/infrastructure/services/notification.service';
import { userApi } from '~/src/infrastructure/services/user.service';
import { tenantApi } from '~/src/infrastructure/services/tenant.service';
import { identityApi } from '~/src/infrastructure/services/identity.service';

export function TenantSwitcher({
   tenants,
   currentTenant,
}: {
   tenants: TTenant[];
   currentTenant: TTenant | null;
}) {
   const [selectedTenant, setSelectedTenant] = useState<TTenant | null>(null);
   const [searchQuery, setSearchQuery] = useState('');

   const { currentUser, impersonatedUser } = useAppSelector(
      (state) => state.auth,
   );
   const { tenantId: currentTenantId } = useAppSelector(
      (state) => state.tenant,
   );

   const dispatch = useDispatch<AppDispatch>();

   const roles = useMemo(
      () => currentUser?.roles || impersonatedUser?.roles || [],
      [currentUser, impersonatedUser],
   );

   const DEFAULT_TENANT_ID = useMemo(() => {
      return currentUser?.tenantId || null;
   }, [currentUser?.tenantId]);

   //    // Check if user has permission to switch tenants
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

   const handleSelectTenant = (tenant: TTenant) => {
      setSelectedTenant(tenant);
      setSearchQuery('');

      const isDefaultTenant = tenant.id === DEFAULT_TENANT_ID;

      const tenantPayload = {
         tenantId: isDefaultTenant ? null : tenant.id,
         branchId: isDefaultTenant
            ? null
            : (tenant.embedded_branch?.id ?? null),
         tenantSubDomain: isDefaultTenant ? null : tenant.sub_domain,
      };

      dispatch(setTenant(tenantPayload));
      // Invalidate tenant-scoped caches so queries refetch with new tenant
      dispatch(orderingApi.util.invalidateTags(['Orders']));
      dispatch(inventoryApi.util.invalidateTags(['Inventory']));
      dispatch(productApi.util.invalidateTags(['Products']));
      dispatch(categoryApi.util.invalidateTags(['Categories']));
      dispatch(notificationApi.util.invalidateTags(['OrderNotifications']));
      dispatch(userApi.util.invalidateTags(['Users', 'UserRoles']));
      dispatch(tenantApi.util.invalidateTags(['Tenants']));
      // Do not invalidate identityApi 'UserSwitcher' cache so user-switcher list stays global
      dispatch(identityApi.util.invalidateTags(['Users']));
   };

   // Default selected tenant
   useEffect(() => {
      if (tenants.length === 0) return;

      if (currentTenantId) {
         const currentTenant = tenants.find((t) => t.id === currentTenantId);
         if (currentTenant) {
            setSelectedTenant(currentTenant);
         }

         return;
      }

      const defaultTenant =
         tenants.find((t) => t.id === currentUser?.tenantId) || null;

      if (defaultTenant) {
         setSelectedTenant(defaultTenant);
      }
   }, [currentUser?.tenantId, tenants, currentTenantId]);

   if (!roles.includes(ERole.ADMIN_SUPER) && currentTenant) {
      return (
         <SidebarMenuButton size="lg" className="cursor-default">
            <div className="bg-sidebar-primary text-sidebar-primary-foreground flex aspect-square size-8 items-center justify-center rounded-lg">
               <GalleryVerticalEnd className="size-4" />
            </div>
            <div className="flex flex-col gap-0.5 leading-none">
               <span className="font-medium">{currentTenant.name}</span>
               <span className="text-xs text-muted-foreground">
                  {currentTenant.sub_domain}
               </span>
            </div>
         </SidebarMenuButton>
      );
   }

   if (
      !selectedTenant ||
      tenants.length === 0 ||
      (!currentTenantId && impersonatedUser)
   ) {
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
                           filteredTenants.map((tenant) => {
                              const isSelected =
                                 tenant.id === selectedTenant?.id;

                              return (
                                 <DropdownMenuItem
                                    key={tenant.id}
                                    onSelect={() => handleSelectTenant(tenant)}
                                    className="flex items-center gap-2"
                                    disabled={isSelected || !!impersonatedUser}
                                 >
                                    <div className="flex flex-col flex-1">
                                       <span className="font-medium">
                                          {tenant.name}
                                       </span>
                                       {tenant.embedded_branch?.address && (
                                          <span className="text-xs text-muted-foreground">
                                             {tenant.embedded_branch.address}
                                          </span>
                                       )}
                                    </div>
                                    {isSelected && (
                                       <Check className="ml-auto" />
                                    )}
                                 </DropdownMenuItem>
                              );
                           })
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
                     <span className="font-medium">{currentTenant?.name}</span>
                     <span className="text-xs text-muted-foreground">
                        {currentTenant?.sub_domain}
                     </span>
                  </div>
               </SidebarMenuButton>
            )}
         </SidebarMenuItem>
      </SidebarMenu>
   );
}
