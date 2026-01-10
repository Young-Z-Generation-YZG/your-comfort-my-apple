'use client';

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import {
   ColumnDef,
   ColumnFiltersState,
   flexRender,
   getCoreRowModel,
   getFilteredRowModel,
   getSortedRowModel,
   SortingState,
   useReactTable,
   VisibilityState,
} from '@tanstack/react-table';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import {
   ArrowUpDown,
   MoreHorizontal,
   Ellipsis,
   ChevronDown,
   Search,
   CheckCircle2,
   XCircle,
   Clock,
   ArrowRightLeft,
   RefreshCw,
   LayoutList,
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   X,
   Warehouse,
   Building2,
   Filter,
} from 'lucide-react';
import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuLabel,
   DropdownMenuSeparator,
   DropdownMenuTrigger,
   DropdownMenuCheckboxItem,
} from '@components/ui/dropdown-menu';
import {
   SelectGroup,
   SelectContent,
   Select,
   SelectTrigger,
   SelectValue,
   SelectItem,
} from '@components/ui/select';
import { LoadingOverlay } from '@components/loading-overlay';
import useFilters from '~/src/hooks/use-filter';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { useRouter, useSearchParams } from 'next/navigation';
import { TSkuRequest } from '~/src/domain/types/catalog.type';
import usePaginationV2 from '~/src/hooks/use-pagination';
import useRequestService from '~/src/hooks/api/use-request-service';
import { ESkuRequestState } from '~/src/domain/enums/sku-request-state.enum';
import { cn } from '~/src/infrastructure/lib/utils';
import Image from 'next/image';
import useTenantService from '~/src/hooks/api/use-tenant-service';

const getStatusStyle = (status: string) => {
   switch (status) {
      case ESkuRequestState.PENDING:
         return 'bg-amber-50 text-amber-700 border-amber-200 dark:bg-amber-900/30 dark:text-amber-400 dark:border-amber-800';
      case ESkuRequestState.APPROVED:
         return 'bg-emerald-50 text-emerald-700 border-emerald-200 dark:bg-emerald-900/30 dark:text-emerald-400 dark:border-emerald-800';
      case ESkuRequestState.REJECTED:
         return 'bg-rose-50 text-rose-700 border-rose-200 dark:bg-rose-900/30 dark:text-rose-400 dark:border-rose-800';
      default:
         return 'bg-slate-50 text-slate-700 border-slate-200 dark:bg-slate-900/30 dark:text-slate-400 dark:border-slate-800';
   }
};

const getStatusIcon = (status: string) => {
   switch (status) {
      case ESkuRequestState.PENDING:
         return <Clock className="h-3.5 w-3.5 mr-1.5" />;
      case ESkuRequestState.APPROVED:
         return <CheckCircle2 className="h-3.5 w-3.5 mr-1.5" />;
      case ESkuRequestState.REJECTED:
         return <XCircle className="h-3.5 w-3.5 mr-1.5" />;
      default:
         return null;
   }
};

const PAGE_LIMIT_OPTIONS = [10, 20, 50];

const ApplicationRequestsPage = () => {
   const [sorting, setSorting] = useState<SortingState>([]);
   const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
   const [columnVisibility, setColumnVisibility] = useState<VisibilityState>({});
   const [rowSelection, setRowSelection] = useState({});

   const { tenantId } = useAppSelector((state) => state.tenant);
   const { currentUser } = useAppSelector((state) => state.auth);
   const defaultBranchId = currentUser?.branchId;
   const router = useRouter();

   const {
      getSkuRequestsAsync,
      getSkuRequestsState,
      updateSkuRequestAsync,
      isLoading: isRequestsLoading,
   } = useRequestService();

   const {
         getListTenantsAsync,
         getListTenantsState,
         isLoading: isTenantLoading,
      } = useTenantService();

   const isLoading = isRequestsLoading || isTenantLoading;

   const { filters, setFilters } = useFilters<{
      _page?: number | null;
      _limit?: number | null;
      _requestState?: string | null;
      _transferType?: string | null;
      _branchId?: string | null;
   }>({
      _page: 'number',
      _limit: 'number',
      _requestState: 'string',
      _transferType: 'string',
      _branchId: 'string',
   });

   const searchParams = useSearchParams();
   const isInitialMount = useRef(true);

   useEffect(() => {
      if (isInitialMount.current && defaultBranchId) {
         if (!searchParams.get('_branchId')) {
            setFilters({ _branchId: defaultBranchId });
         }
         isInitialMount.current = false;
      }
   }, [defaultBranchId, searchParams, setFilters]);

   const tenantItems = useMemo(() => {
         return getListTenantsState.isSuccess && getListTenantsState.data
            ? getListTenantsState.data
            : [];
      }, [getListTenantsState.isSuccess, getListTenantsState.data]); 

   const branchItems = useMemo(() => {
      return tenantItems.map((tenant) => {
         return {
            branch_id: tenant.embedded_branch.id,
            branch_name: tenant.embedded_branch.name,
         };
      });
   }, [tenantItems]);

   const {
      getPaginationItems,
      currentPage,
      totalRecords,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
   } = usePaginationV2(
      getSkuRequestsState.data ?? {
         total_records: 0,
         total_pages: 0,
         page_size: 0,
         current_page: 0,
         items: [],
         links: {},
      },
      {
         pageSizeOverride: filters._limit ?? null,
         currentPageOverride: filters._page ?? null,
         fallbackPageSize: PAGE_LIMIT_OPTIONS[0],
      },
   );

   const paginationItems = getPaginationItems();

   const fetchData = useCallback(() => {
      getSkuRequestsAsync({
         _page: filters._page ?? undefined,
         _limit: filters._limit ?? undefined,
         _requestState: filters._requestState ?? undefined,
         _transferType: filters._transferType ?? undefined,
         _branchId: filters._branchId ?? undefined,
      });
   }, [
      filters._page,
      filters._limit,
      filters._requestState,
      filters._transferType,
      filters._branchId,
      getSkuRequestsAsync,
   ]);

   const fetchTenants = useCallback(() => {
      getListTenantsAsync();
   }, [getListTenantsAsync]);

   useEffect(() => {
      fetchData();
   }, [fetchData]);

   useEffect(() => {
      fetchTenants();
   }, [fetchTenants]);

   const handleUpdateStatus = async (id: string, newState: ESkuRequestState) => {
      const result = await updateSkuRequestAsync(id, { state: newState });
      if (result.isSuccess) {
         fetchData();
      }
   };

   const columns: ColumnDef<TSkuRequest>[] = [
      {
         accessorKey: 'id',
         header: 'ID',
         cell: ({ row }) => (
            <div className="font-mono text-[10px] text-muted-foreground uppercase">
               #{row.getValue<string>('id').slice(-8)}
            </div>
         ),
      },
      {
         id: 'flow',
         header: 'Branch Transfer',
         cell: ({ row }) => {
            const from = row.original.from_branch;
            const to = row.original.to_branch;
            return (
               <div className="flex items-center space-x-3">
                  <div className="flex flex-col">
                     <span className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">From</span>
                     <span className="text-sm font-medium">{from.branch_name}</span>
                  </div>
                  <ArrowRightLeft className="h-4 w-4 text-slate-300 mx-1" />
                  <div className="flex flex-col">
                     <span className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">To</span>
                     <span className="text-sm font-medium">{to.branch_name}</span>
                  </div>
               </div>
            );
         },
      },
      {
         accessorKey: 'sku',
         header: 'SKU Details',
         cell: ({ row }) => {
            const sku = row.original.sku;
            return (
               <div className="flex items-center gap-3">
                  <div className="h-12 w-12 rounded-lg border bg-slate-50 p-1 dark:bg-slate-900 overflow-hidden relative">
                     <Image
                        src={sku.image_url}
                        alt={sku.model_normalized_name}
                        fill
                        className="object-contain"
                     />
                  </div>
                  <div className="flex flex-col">
                     <span className="text-sm font-semibold">{sku.model_normalized_name}</span>
                     <div className="flex items-center gap-2">
                        <Badge variant="secondary" className="px-1.5 h-4 text-[10px] font-medium uppercase tracking-tighter">
                           {sku.color_normalized_name}
                        </Badge>
                        <Badge variant="secondary" className="px-1.5 h-4 text-[10px] font-medium uppercase tracking-tighter">
                           {sku.storage_normalized_name}
                        </Badge>
                     </div>
                  </div>
               </div>
            );
         },
      },
      {
         accessorKey: 'request_quantity',
         header: ({ column }) => (
            <div className="text-center">
               <Button
                  variant="ghost"
                  onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
                  className="h-8 hover:bg-transparent"
               >
                  Quantity
                  <ArrowUpDown className="ml-2 h-3 w-3" />
               </Button>
            </div>
         ),
         cell: ({ row }) => (
            <div className="text-center">
               <span className="inline-flex items-center justify-center h-7 w-7 rounded-full bg-slate-100 text-slate-900 font-bold text-xs border dark:bg-slate-800 dark:text-slate-100">
                  {row.getValue('request_quantity')}
               </span>
            </div>
         ),
      },
      {
         accessorKey: 'state',
         header: 'Current State',
         cell: ({ row }) => {
            const state = row.getValue('state') as string;
            return (
               <Badge
                  variant="outline"
                  className={cn(
                     'px-2.5 py-0.5 rounded-full font-medium transition-colors shadow-sm',
                     getStatusStyle(state)
                  )}
               >
                  {getStatusIcon(state)}
                  {state}
               </Badge>
            );
         },
      },
      {
         accessorKey: 'created_at',
         header: 'Timeline',
         cell: ({ row }) => {
            const date = new Date(row.getValue('created_at'));
            return (
               <div className="flex flex-col leading-tight">
                  <span className="text-sm font-medium">{date.toLocaleDateString('en-GB')}</span>
                  <span className="text-[10px] text-muted-foreground uppercase">{date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' })}</span>
               </div>
            );
         },
      },
      {
         id: 'actions',
         cell: ({ row }) => {
            const request = row.original;
            return (
               <div className="text-right">
                  <DropdownMenu>
                     <DropdownMenuTrigger asChild>
                        <Button variant="ghost" className="h-8 w-8 p-0 rounded-full hover:bg-slate-100 dark:hover:bg-slate-800">
                           <MoreHorizontal className="h-4 w-4" />
                        </Button>
                     </DropdownMenuTrigger>
                     <DropdownMenuContent align="end" className="w-48">
                        <DropdownMenuLabel className="text-xs text-muted-foreground">Request Actions</DropdownMenuLabel>
                        <DropdownMenuItem className="text-xs" onClick={() => navigator.clipboard.writeText(request.id)}>
                           Copy Reference ID
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        {request.state === ESkuRequestState.PENDING ? (
                           <>
                              <DropdownMenuItem
                                 onClick={() => handleUpdateStatus(request.id, ESkuRequestState.APPROVED)}
                                 className="text-emerald-600 focus:text-emerald-700 focus:bg-emerald-50 dark:focus:bg-emerald-950/30"
                              >
                                 <CheckCircle2 className="h-4 w-4 mr-2" />
                                 Approve Request
                              </DropdownMenuItem>
                              <DropdownMenuItem
                                 onClick={() => handleUpdateStatus(request.id, ESkuRequestState.REJECTED)}
                                 className="text-rose-600 focus:text-rose-700 focus:bg-rose-50 dark:focus:bg-rose-950/30"
                              >
                                 <XCircle className="h-4 w-4 mr-2" />
                                 Reject Request
                              </DropdownMenuItem>
                           </>
                        ) : (
                           <DropdownMenuItem className="text-xs italic text-muted-foreground" disabled>
                              Status: {request.state}
                           </DropdownMenuItem>
                        )}
                        <DropdownMenuSeparator />
                        <DropdownMenuItem className="text-xs" disabled>
                           View History
                        </DropdownMenuItem>
                     </DropdownMenuContent>
                  </DropdownMenu>
               </div>
            );
         },
      },
   ];

   const table = useReactTable({
      data: getSkuRequestsState.data?.items ?? [],
      columns,
      onSortingChange: setSorting,
      onColumnFiltersChange: setColumnFilters,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      getFilteredRowModel: getFilteredRowModel(),
      onColumnVisibilityChange: setColumnVisibility,
      onRowSelectionChange: setRowSelection,
      state: {
         sorting,
         columnFilters,
         columnVisibility,
         rowSelection,
      },
   });

   return (
      <div className="min-h-screen bg-slate-50/50 dark:bg-slate-950/50">
         <div className="p-4 lg:p-8 space-y-6 max-w-7xl mx-auto">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
               <div className="space-y-1">
                  <div className="flex items-center gap-2">
                     <div className="p-2 bg-slate-900 text-white rounded-lg dark:bg-white dark:text-slate-900 shadow-lg">
                        <LayoutList className="h-5 w-5" />
                     </div>
                     <h1 className="text-2xl font-extrabold tracking-tight text-slate-900 dark:text-slate-50">
                        Transfer Management
                     </h1>
                  </div>
                  <p className="text-sm text-muted-foreground max-w-lg">
                     Manage stock transfer requests between inventory branches. Track status and fulfill SKU requirements.
                  </p>
               </div>
               <div className="flex items-center gap-2">
                  <Button
                     variant="outline"
                     size="sm"
                     className="bg-white dark:bg-slate-900 h-9"
                     onClick={fetchData}
                     disabled={isLoading}
                  >
                     <RefreshCw className={cn('h-4 w-4 mr-2', isLoading && 'animate-spin')} />
                     Refresh
                  </Button>
               </div>
            </div>

            <LoadingOverlay isLoading={isLoading}>
               {/* Filter Section - Matching Warehouse Pattern Exactly */}
               <div className="rounded-xl border bg-card shadow-sm mb-6">
                  <div className="p-6">
                     <div className="flex flex-wrap items-center gap-3">
                        {/* Transfer Flow Filter */}
                        <Select
                           value={filters._transferType ?? 'ALL'}
                           onValueChange={(value) =>
                              setFilters({
                                 _transferType: value === 'ALL' ? null : value,
                                 _page: 1,
                              })
                           }
                        >
                           <SelectTrigger className="h-10 w-auto min-w-[220px] bg-white dark:bg-slate-900 border-slate-200 dark:border-slate-800">
                              <div className="flex items-center gap-2 overflow-hidden">
                                 <ArrowRightLeft className="h-4 w-4 shrink-0 text-muted-foreground" />
                                 <span className="font-medium shrink-0">Type</span>
                                 <div className="truncate text-left">
                                    <SelectValue placeholder="Transfer Type" />
                                 </div>
                              </div>
                           </SelectTrigger>
                           <SelectContent>
                              <SelectItem value="ALL">All Flows</SelectItem>
                              <SelectItem value="SENT_TO">Sent To (Incoming)</SelectItem>
                              <SelectItem value="RECEIVE_FROM">Received From (Outgoing)</SelectItem>
                           </SelectContent>
                        </Select>

                        {/* Branch Filter */}
                        <Select
                           value={filters._branchId ?? 'ALL'}
                           onValueChange={(value) =>
                              setFilters({
                                 _branchId: value === 'ALL' ? null : value,
                                 _page: 1,
                              })
                           }
                        >
                           <SelectTrigger className="h-10 w-auto min-w-[260px] bg-white dark:bg-slate-900 border-slate-200 dark:border-slate-800">
                              <div className="flex items-center gap-2 overflow-hidden">
                                 <Building2 className="h-4 w-4 shrink-0 text-muted-foreground" />
                                 <span className="font-medium shrink-0">Branch</span>
                                 <div className="truncate text-left">
                                    <SelectValue placeholder="Select Branch" />
                                 </div>
                              </div>
                           </SelectTrigger>
                           <SelectContent>
                              <SelectItem value="ALL">All Branches</SelectItem>
                              {branchItems.map((branch) => (
                                 <SelectItem key={branch.branch_id} value={branch.branch_id}>
                                    {branch.branch_name}
                                 </SelectItem>
                              ))}
                           </SelectContent>
                        </Select>

                        {/* Status Filter */}
                        <DropdownMenu>
                           <DropdownMenuTrigger asChild>
                              <Button variant="outline" className="h-10 gap-2 bg-white dark:bg-slate-900 border-slate-200 dark:border-slate-800">
                                 <Clock className="h-4 w-4 text-muted-foreground" />
                                 <span className="font-medium">Status</span>
                                 <div className="flex items-center gap-2">
                                    {filters._requestState ? (
                                       <Badge variant="outline" className="bg-slate-100 text-slate-800 border-slate-300 dark:bg-slate-800 dark:text-slate-200 dark:border-slate-700">
                                          {filters._requestState}
                                       </Badge>
                                    ) : (
                                       <span className="text-muted-foreground font-normal">All States</span>
                                    )}
                                 </div>
                                 <ChevronDown className="h-4 w-4 opacity-50" />
                              </Button>
                           </DropdownMenuTrigger>
                           <DropdownMenuContent align="start" className="w-48 p-1">
                              <DropdownMenuLabel className="text-[10px] uppercase font-bold tracking-widest text-muted-foreground px-2 py-1.5">Filter by Status</DropdownMenuLabel>
                              <DropdownMenuSeparator />
                              <DropdownMenuCheckboxItem
                                 checked={!filters._requestState}
                                 onCheckedChange={() => setFilters({ _requestState: null, _page: 1 })}
                                 className="text-xs py-2"
                              >
                                 All Statuses
                              </DropdownMenuCheckboxItem>
                              {Object.values(ESkuRequestState).map((state) => (
                                 <DropdownMenuCheckboxItem
                                    key={state}
                                    checked={filters._requestState === state}
                                    onCheckedChange={(checked) => {
                                       setFilters({
                                          _requestState: checked ? state : null,
                                          _page: 1,
                                       });
                                    }}
                                    className="text-xs py-2"
                                 >
                                    <div className="flex items-center grow">
                                       <span className={cn('h-2 w-2 rounded-full mr-2 shadow-sm', 
                                          state === ESkuRequestState.PENDING ? 'bg-amber-400' :
                                          state === ESkuRequestState.APPROVED ? 'bg-emerald-400' : 'bg-rose-400'
                                       )} />
                                       {state}
                                    </div>
                                 </DropdownMenuCheckboxItem>
                              ))}
                           </DropdownMenuContent>
                        </DropdownMenu>

                        {/* Clear Filters Button */}
                        {(filters._requestState || filters._transferType || filters._branchId) && (
                           <Button 
                              variant="outline"
                              onClick={() => setFilters({ _requestState: null, _transferType: null, _branchId: null, _page: 1 })}
                              className="h-10 px-4 gap-2 whitespace-nowrap border-[#ef4444] text-[#ef4444] bg-[#ef4444]/10 hover:bg-[#ef4444]/20 dark:bg-[#ef4444]/20 dark:border-[#ef4444]"
                           >
                              <X className="h-4 w-4" />
                              Clear Filters
                           </Button>
                        )}
                     </div>
                  </div>
               </div>

               <div className="rounded-xl border bg-white shadow-sm overflow-hidden dark:bg-slate-950">
                  <div className="overflow-x-auto">
                     <Table>
                        <TableHeader className="bg-slate-50/50 dark:bg-slate-900/50">
                           {table.getHeaderGroups().map((headerGroup) => (
                              <TableRow key={headerGroup.id} className="hover:bg-transparent border-b">
                                 {headerGroup.headers.map((header) => (
                                    <TableHead key={header.id} className="h-12 py-0 align-middle font-semibold text-slate-900 dark:text-slate-100">
                                       {header.isPlaceholder
                                          ? null
                                          : flexRender(
                                               header.column.columnDef.header,
                                               header.getContext(),
                                            )}
                                    </TableHead>
                                 ))}
                              </TableRow>
                           ))}
                        </TableHeader>
                        <TableBody>
                           {table.getRowModel().rows?.length ? (
                              table.getRowModel().rows.map((row) => (
                                 <TableRow key={row.id} className="group transition-colors hover:bg-slate-50 dark:hover:bg-slate-900/50 border-b last:border-0 font-medium">
                                    {row.getVisibleCells().map((cell) => (
                                       <TableCell key={cell.id} className="py-3 align-middle">
                                          {flexRender(
                                             cell.column.columnDef.cell,
                                             cell.getContext(),
                                          )}
                                       </TableCell>
                                    ))}
                                 </TableRow>
                              ))
                           ) : (
                              <TableRow className="hover:bg-transparent">
                                 <TableCell colSpan={columns.length} className="h-48 text-center bg-white dark:bg-slate-950">
                                    <div className="flex flex-col items-center justify-center space-y-2 opacity-40">
                                       <LayoutList className="h-10 w-10" />
                                       <p className="text-sm font-medium">No transfer requests pending.</p>
                                    </div>
                                 </TableCell>
                              </TableRow>
                           )}
                        </TableBody>
                     </Table>
                  </div>

                <div className="flex flex-wrap items-center justify-between gap-4 px-4 py-4 border-t">
                   <div className="flex items-center gap-4">
                      <Select
                         value={limitSelectValue}
                         onValueChange={(value) =>
                            setFilters({ _limit: Number(value), _page: 1 })
                         }
                      >
                         <SelectTrigger className="w-auto h-9">
                            <SelectValue placeholder="Select limit" />
                         </SelectTrigger>
                         <SelectContent>
                            <SelectGroup>
                               <SelectItem value="10">10 / page</SelectItem>
                               <SelectItem value="20">20 / page</SelectItem>
                               <SelectItem value="50">50 / page</SelectItem>
                            </SelectGroup>
                         </SelectContent>
                      </Select>

                      <div className="text-muted-foreground text-sm">
                         Showing{' '}
                         <span className="font-medium">{firstItemIndex}</span> to{' '}
                         <span className="font-medium">{lastItemIndex}</span> of{' '}
                         <span className="font-medium">{totalRecords}</span>{' '}
                         requests
                      </div>
                   </div>

                   {getSkuRequestsState.data &&
                      getSkuRequestsState.data.total_pages > 0 && (
                         <div className="flex items-center gap-2">
                            {paginationItems.map((item, index) => {
                               if (item.type === 'ellipsis') {
                                  return (
                                     <span
                                        key={`ellipsis-${index}`}
                                        className="px-2 text-gray-400 flex items-center"
                                     >
                                        <Ellipsis className="h-4 w-4" />
                                     </span>
                                  );
                               }

                               const isCurrentPage =
                                  item.type === 'page' &&
                                  item.value === currentPage;

                               return (
                                  <Button
                                     key={`${item.type}-${item.label}-${index}`}
                                     variant={
                                        isCurrentPage ? 'default' : 'outline'
                                     }
                                     size="icon"
                                     className={cn(
                                        'h-9 w-9',
                                        isCurrentPage &&
                                           'bg-black text-white hover:bg-black/90',
                                     )}
                                     disabled={
                                        item.disabled || item.value === null
                                     }
                                     onClick={() => {
                                        if (
                                           item.value !== null &&
                                           !item.disabled
                                        ) {
                                          setFilters({ _page: item.value });
                                        }
                                     }}
                                  >
                                     {item.type === 'nav' ? (
                                        item.label === '<<' ? (
                                           <ChevronsLeft className="h-4 w-4" />
                                        ) : item.label === '>>' ? (
                                           <ChevronsRight className="h-4 w-4" />
                                        ) : item.label === '<' ? (
                                           <ChevronLeft className="h-4 w-4" />
                                        ) : (
                                           <ChevronRight className="h-4 w-4" />
                                        )
                                     ) : (
                                        item.label
                                     )}
                                  </Button>
                               );
                            })}
                         </div>
                      )}
                </div>
               </div>
            </LoadingOverlay>
         </div>
      </div>
   );
};

export default ApplicationRequestsPage;
