'use client';

import * as React from 'react';
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

export function TenantSwitcher({
   tenants,
}: {
   tenants: {
      tenantId: string;
      tenantCode: string;
      tenantName: string;
      tenantAddress?: string;
   }[];
}) {
   const [selectedTenant, setSelectedTenant] = React.useState(tenants[0]);
   const [searchQuery, setSearchQuery] = React.useState('');

   const filteredTenants = React.useMemo(() => {
      if (!searchQuery) return tenants;

      return tenants.filter((tenant) =>
         tenant.tenantName.toLowerCase().includes(searchQuery.toLowerCase()),
      );
   }, [tenants, searchQuery]);

   const handleSelectTenant = (tenant: {
      tenantId: string;
      tenantCode: string;
      tenantName: string;
      tenantAddress?: string;
   }) => {
      setSelectedTenant(tenant);
      setSearchQuery('');
   };

   return (
      <SidebarMenu>
         <SidebarMenuItem>
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
                           {selectedTenant.tenantName}
                        </span>
                        <span className="">{selectedTenant.tenantCode}</span>
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
                              key={tenant.tenantId}
                              onSelect={() => handleSelectTenant(tenant)}
                              className="flex items-center gap-2"
                           >
                              <div className="flex flex-col flex-1">
                                 <span className="font-medium">
                                    {tenant.tenantName}
                                 </span>
                                 {tenant.tenantAddress && (
                                    <span className="text-xs text-muted-foreground">
                                       {tenant.tenantAddress}
                                    </span>
                                 )}
                              </div>
                              {tenant.tenantId === selectedTenant.tenantId && (
                                 <Check className="ml-auto" />
                              )}
                           </DropdownMenuItem>
                        ))
                     )}
                  </div>
               </DropdownMenuContent>
            </DropdownMenu>
         </SidebarMenuItem>
      </SidebarMenu>
   );
}
