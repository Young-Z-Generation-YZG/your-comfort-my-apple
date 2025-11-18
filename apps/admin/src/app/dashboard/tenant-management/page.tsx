'use client';

import ContentWrapper from '@components/ui/content-wrapper';
import { Button } from '@components/ui/button';
import {
   Building2,
   ChevronsLeft,
   ChevronsRight,
   ChevronLeft,
   ChevronRight,
   Ellipsis,
} from 'lucide-react';
import { Fragment, useEffect } from 'react';
import { cn } from '~/src/infrastructure/lib/utils';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';

import TenantCard from './_components/tenant-card';
import useTenantService from '~/src/hooks/api/use-tenant-service';
import usePagination from '~/src/hooks/use-pagination';
import { LoadingOverlay } from '@components/loading-overlay';

// Fake data for tenants
const fakeData = {
   total_records: 1,
   total_pages: 1,
   page_size: 10,
   current_page: 1,
   items: [
      {
         id: '664355f845e56534956be32b',
         code: 'WARE_HOUSE',
         name: 'Ware house',
         description: '',
         tenantType: 'WARE_HOUSE',
         tenantState: 'ACTIVE',
         createdAt: '2025-10-21T05:33:50.425Z',
         updatedAt: '2025-10-21T05:33:50.425Z',
         updatedBy: '',
         isDeleted: false,
         deletedAt: null,
         deletedBy: '',
      },
   ],
   links: {
      first: '?_page=1&_limit=10',
      prev: null,
      next: null,
      last: '?_page=1&_limit=10',
   },
};

export type TTenantItem = (typeof fakeData.items)[number];

const TenantsPage = () => {
   const { getTenantsAsync, getTenantsState, isLoading } = useTenantService();

   const {
      currentPage,
      totalPages,
      pageSize,
      totalRecords,
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,
      paginationItems,
      getPageNumbers,
   } = usePagination(
      getTenantsState.isSuccess &&
         getTenantsState.data &&
         getTenantsState.data.items.length > 0
         ? getTenantsState.data
         : {
              total_records: 0,
              total_pages: 0,
              page_size: 0,
              current_page: 0,
              items: [],
              links: {
                 first: null,
                 last: null,
                 prev: null,
                 next: null,
              },
           },
   );

   useEffect(() => {
      const fetchTenants = async () => {
         await getTenantsAsync('');
      };
      fetchTenants();
   }, [getTenantsAsync]);

   return (
      <Fragment>
         <div className="p-4">
            <div className="flex flex-col gap-6 p-6">
               <div>
                  <h1 className="text-3xl font-bold tracking-tight">
                     Tenant Management
                  </h1>
                  <p className="text-muted-foreground">
                     Manage warehouses and branch locations
                  </p>
               </div>

               <LoadingOverlay isLoading={isLoading}>
                  <div className="p-8">
                     {/* Cards Grid */}
                     {paginationItems.length > 0 ? (
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                           {paginationItems.map((tenant) => (
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

                     {/* Pagination Controls */}
                     {totalPages >= 1 && (
                        <div className="flex items-center justify-between mt-4">
                           <div className="flex items-center gap-2">
                              <Select
                                 value={pageSize.toString()}
                                 onValueChange={(value) => {
                                    // setFilters({ _limit: Number(value) });
                                 }}
                              >
                                 <SelectTrigger className="w-auto h-9">
                                    <SelectValue />
                                 </SelectTrigger>
                                 <SelectContent>
                                    <SelectGroup>
                                       <SelectItem value="10">
                                          10 / page
                                       </SelectItem>
                                       <SelectItem value="20">
                                          20 / page
                                       </SelectItem>
                                       <SelectItem value="30">
                                          30 / page
                                       </SelectItem>
                                    </SelectGroup>
                                 </SelectContent>
                              </Select>

                              <div className="text-muted-foreground flex-1 text-sm">
                                 Showing {totalRecords} total tenants
                              </div>
                           </div>

                           <div className="flex items-center gap-2 justify-end py-5">
                              {/* First Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() => {
                                    // setFilters({ _page: 1 });
                                 }}
                                 disabled={isFirstPage}
                              >
                                 <ChevronsLeft className="h-4 w-4" />
                              </Button>

                              {/* Previous Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() => {
                                    if (currentPage > 1) {
                                       // setFilters({ _page: currentPage - 1 });
                                    }
                                 }}
                                 disabled={!isPrevPage}
                              >
                                 <ChevronLeft className="h-4 w-4" />
                              </Button>

                              {/* Page Numbers */}
                              <div className="flex items-center gap-1">
                                 {getPageNumbers().map((page, index) => {
                                    if (page === '...') {
                                       return (
                                          <span
                                             key={`ellipsis-${index}`}
                                             className="px-2 text-gray-400"
                                          >
                                             <Ellipsis className="h-4 w-4" />
                                          </span>
                                       );
                                    }

                                    return (
                                       <Button
                                          key={index}
                                          variant={
                                             currentPage === page
                                                ? 'default'
                                                : 'outline'
                                          }
                                          size="icon"
                                          className={cn(
                                             'h-9 w-9',
                                             currentPage === page &&
                                                'bg-black text-white hover:bg-black/90',
                                          )}
                                          onClick={() => {
                                             // setFilters({ _page: page as number });
                                          }}
                                       >
                                          {page as number}
                                       </Button>
                                    );
                                 })}
                              </div>

                              {/* Next Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() => {
                                    if (currentPage < totalPages) {
                                       // setFilters({ _page: currentPage + 1 });
                                    }
                                 }}
                                 disabled={!isNextPage}
                              >
                                 <ChevronRight className="h-4 w-4" />
                              </Button>

                              {/* Last Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() => {
                                    // setFilters({ _page: totalPages });
                                 }}
                                 disabled={isLastPage}
                              >
                                 <ChevronsRight className="h-4 w-4" />
                              </Button>
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
