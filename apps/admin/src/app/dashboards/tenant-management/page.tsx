'use client';

import { Button } from '@components/ui/button';
import ContentWrapper from '@components/ui/content-wrapper';
import { Input } from '@components/ui/input';
import {
   Search,
   Plus,
   Building2,
   MapPin,
   CheckCircle2,
   XCircle,
} from 'lucide-react';
import { Fragment, useState } from 'react';

import TenantCard from './_components/tenant-card';

// Mock data for tenants
const mockTenants = [
   {
      tenant_id: '664355f845e56534956be32b',
      code: 'WARE_HOUSE',
      name: 'Ware house',
      description: 'Main warehouse facility',
      tenant_type: 'WARE_HOUSE',
      tenant_state: 'ACTIVE',
      created_at: '2024-05-14T10:30:00Z',
      updated_at: '2024-10-20T15:45:00Z',
   },
   {
      tenant_id: '664355f845e56534956be32c',
      code: 'HCM_BRANCH_01',
      name: 'Ho Chi Minh Branch 01',
      description: 'District 1 retail store',
      tenant_type: 'BRANCH',
      tenant_state: 'ACTIVE',
      created_at: '2024-06-01T08:00:00Z',
      updated_at: '2024-10-15T12:00:00Z',
   },
   {
      tenant_id: '664355f845e56534956be32d',
      code: 'HN_BRANCH_01',
      name: 'Ha Noi Branch 01',
      description: 'Hoan Kiem retail store',
      tenant_type: 'BRANCH',
      tenant_state: 'ACTIVE',
      created_at: '2024-06-15T09:00:00Z',
      updated_at: '2024-10-25T14:30:00Z',
   },
   {
      tenant_id: '664355f845e56534956be32e',
      code: 'DN_BRANCH_01',
      name: 'Da Nang Branch 01',
      description: 'Son Tra district store',
      tenant_type: 'BRANCH',
      tenant_state: 'INACTIVE',
      created_at: '2024-07-01T10:00:00Z',
      updated_at: '2024-10-10T11:00:00Z',
   },
   {
      tenant_id: '664355f845e56534956be32f',
      code: 'HCM_BRANCH_02',
      name: 'Ho Chi Minh Branch 02',
      description: 'District 3 retail store',
      tenant_type: 'BRANCH',
      tenant_state: 'ACTIVE',
      created_at: '2024-08-01T08:30:00Z',
      updated_at: '2024-10-28T16:00:00Z',
   },
];

export type TTenant = (typeof mockTenants)[number];

// Status badge colors and icons
const statusConfig = {
   ACTIVE: {
      color: 'bg-green-50 text-green-700 border-green-200',
      icon: <CheckCircle2 className="h-4 w-4" />,
   },
   INACTIVE: {
      color: 'bg-gray-50 text-gray-700 border-gray-200',
      icon: <XCircle className="h-4 w-4" />,
   },
};

const typeConfig = {
   WARE_HOUSE: {
      color: 'bg-purple-50 text-purple-700 border-purple-200',
      icon: <Building2 className="h-3.5 w-3.5" />,
   },
   BRANCH: {
      color: 'bg-blue-50 text-blue-700 border-blue-200',
      icon: <MapPin className="h-3.5 w-3.5" />,
   },
};

const TenantsPage = () => {
   const [tenants] = useState(mockTenants);
   const [searchQuery, setSearchQuery] = useState('');
   const [currentPage, setCurrentPage] = useState(1);
   const pageSize = 10;

   // Filter tenants based on search query
   const filteredTenants = tenants.filter(
      (tenant) =>
         tenant.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
         tenant.code.toLowerCase().includes(searchQuery.toLowerCase()) ||
         tenant.description?.toLowerCase().includes(searchQuery.toLowerCase()),
   );

   const totalPages = Math.ceil(filteredTenants.length / pageSize);
   const paginatedTenants = filteredTenants.slice(
      (currentPage - 1) * pageSize,
      currentPage * pageSize,
   );

   return (
      <Fragment>
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center justify-between">
                     <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                           Tenant Management
                        </h1>
                        <p className="text-muted-foreground">
                           Manage warehouses and branch locations
                        </p>
                     </div>
                     <Button className="h-10 gap-2">
                        <Plus className="h-4 w-4" />
                        Add New Tenant
                     </Button>
                  </div>

                  <div className="p-8">
                     <div className="mb-8 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
                        <div className="relative w-full max-w-md">
                           <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                           <Input
                              placeholder="Search tenants by name, code, or description..."
                              className="pl-10 bg-white dark:bg-slate-800 border-slate-200 dark:border-slate-700 h-10"
                              value={searchQuery}
                              onChange={(e) => setSearchQuery(e.target.value)}
                           />
                        </div>
                        <div className="flex items-center gap-2">
                           <Button
                              variant="outline"
                              size="sm"
                              className="h-10 bg-white dark:bg-slate-800 border-slate-200 dark:border-slate-700"
                           >
                              <svg
                                 xmlns="http://www.w3.org/2000/svg"
                                 width="16"
                                 height="16"
                                 viewBox="0 0 24 24"
                                 fill="none"
                                 stroke="currentColor"
                                 strokeWidth="2"
                                 strokeLinecap="round"
                                 strokeLinejoin="round"
                                 className="mr-2"
                              >
                                 <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4" />
                                 <polyline points="7 10 12 15 17 10" />
                                 <line x1="12" x2="12" y1="15" y2="3" />
                              </svg>
                              Export
                           </Button>
                           <Button
                              variant="outline"
                              size="sm"
                              className="h-10 bg-white dark:bg-slate-800 border-slate-200 dark:border-slate-700"
                           >
                              <svg
                                 xmlns="http://www.w3.org/2000/svg"
                                 width="16"
                                 height="16"
                                 viewBox="0 0 24 24"
                                 fill="none"
                                 stroke="currentColor"
                                 strokeWidth="2"
                                 strokeLinecap="round"
                                 strokeLinejoin="round"
                                 className="mr-2"
                              >
                                 <rect
                                    width="18"
                                    height="18"
                                    x="3"
                                    y="3"
                                    rx="2"
                                 />
                                 <path d="M3 9h18" />
                                 <path d="M3 15h18" />
                              </svg>
                              Filters
                           </Button>
                        </div>
                     </div>

                     {/* Cards Grid */}
                     {paginatedTenants.length > 0 ? (
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                           {paginatedTenants.map((tenant, key) => (
                              <div key={key}>
                                 <TenantCard key={key} tenant={tenant} />
                              </div>
                           ))}
                        </div>
                     ) : (
                        <div className="flex flex-col items-center justify-center py-16 bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700">
                           <Building2 className="h-16 w-16 text-muted-foreground/50 mb-4" />
                           <p className="font-medium text-lg mb-1">
                              No tenants found
                           </p>
                           <p className="text-sm text-muted-foreground">
                              Try adjusting your search query
                           </p>
                        </div>
                     )}

                     {/* Pagination Controls */}
                     {paginatedTenants.length > 0 && (
                        <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-6">
                           <div className="text-sm text-muted-foreground">
                              Showing {(currentPage - 1) * pageSize + 1} to{' '}
                              {Math.min(
                                 currentPage * pageSize,
                                 filteredTenants.length,
                              )}{' '}
                              of {filteredTenants.length} tenants
                           </div>
                           <div className="flex items-center gap-2">
                              <Button
                                 variant="outline"
                                 size="sm"
                                 disabled={currentPage === 1}
                                 onClick={() => setCurrentPage(currentPage - 1)}
                              >
                                 Previous
                              </Button>
                              {Array.from(
                                 { length: totalPages },
                                 (_, i) => i + 1,
                              ).map((page) => (
                                 <Button
                                    key={page}
                                    variant={
                                       currentPage === page
                                          ? 'default'
                                          : 'outline'
                                    }
                                    size="sm"
                                    onClick={() => setCurrentPage(page)}
                                 >
                                    {page}
                                 </Button>
                              ))}
                              <Button
                                 variant="outline"
                                 size="sm"
                                 disabled={currentPage === totalPages}
                                 onClick={() => setCurrentPage(currentPage + 1)}
                              >
                                 Next
                              </Button>
                           </div>
                        </div>
                     )}
                  </div>
               </div>
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default TenantsPage;
