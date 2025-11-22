'use client';

import { LoadingOverlay } from '@components/loading-overlay';
import useIdentityService from '~/src/hooks/api/use-identity-service';
import {
   ColumnDef,
   flexRender,
   getCoreRowModel,
   getSortedRowModel,
   SortingState,
   useReactTable,
   VisibilityState,
   RowSelectionState,
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
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   DropdownMenu,
   DropdownMenuCheckboxItem,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuLabel,
   DropdownMenuSeparator,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import { useEffect, useState } from 'react';
import {
   ArrowUpDown,
   ChevronDown,
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
   MoreHorizontal,
} from 'lucide-react';
import { cn } from '~/src/infrastructure/lib/utils';
import { Gender } from '~/src/domain/enums/gender.enum';
import useFilters from '~/src/hooks/use-filter';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { TUser } from '~/src/infrastructure/services/identity.service';
import usePaginationV2 from '~/src/hooks/use-pagination-v2';

// Helper function to get gender badge styles
const getGenderStyle = (gender: string) => {
   switch (gender) {
      case Gender.MALE:
         return 'bg-blue-100 text-blue-800 border-blue-300';
      case Gender.FEMALE:
         return 'bg-pink-100 text-pink-800 border-pink-300';
      case Gender.OTHER:
         return 'bg-purple-100 text-purple-800 border-purple-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

type TUserFilter = {
   _page?: number | null;
   _limit?: number | null;
};

const PAGE_LIMIT_OPTIONS = [10, 20, 50];

const columns: ColumnDef<TUser>[] = [
   {
      accessorKey: 'email',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Email
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => (
         <div className="font-medium">{row.getValue('email')}</div>
      ),
   },
   {
      accessorKey: 'profile.first_name',
      header: 'First Name',
      cell: ({ row }) => (
         <div className="capitalize">
            {row.original.profile?.first_name || '-'}
         </div>
      ),
   },
   {
      accessorKey: 'profile.last_name',
      header: 'Last Name',
      cell: ({ row }) => (
         <div className="capitalize">
            {row.original.profile?.last_name || '-'}
         </div>
      ),
   },
   {
      accessorKey: 'phone_number',
      header: 'Phone Number',
      cell: ({ row }) => <div>{row.getValue('phone_number') || '-'}</div>,
   },
   {
      accessorKey: 'profile.gender',
      header: 'Gender',
      cell: ({ row }) => {
         const gender = row.original.profile?.gender || 'OTHER';
         return (
            <Badge
               variant="outline"
               className={cn('capitalize', getGenderStyle(gender))}
            >
               {gender.toLowerCase()}
            </Badge>
         );
      },
   },
   {
      accessorKey: 'profile.birth_day',
      header: 'Birth Date',
      cell: ({ row }) => {
         const birthDay = row.original.profile?.birth_day;
         if (!birthDay || birthDay === '0001-01-01T00:00:00') return '-';
         return new Date(birthDay).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
         });
      },
   },
   {
      accessorKey: 'email_confirmed',
      header: 'Email Verified',
      cell: ({ row }) => {
         const isConfirmed = row.getValue('email_confirmed');
         return (
            <Badge variant={isConfirmed ? 'default' : 'destructive'}>
               {isConfirmed ? 'Verified' : 'Not Verified'}
            </Badge>
         );
      },
   },
   {
      id: 'actions',
      enableHiding: false,
      cell: ({ row }) => {
         const user = row.original;

         return (
            <DropdownMenu>
               <DropdownMenuTrigger asChild>
                  <Button variant="ghost" className="h-8 w-8 p-0">
                     <span className="sr-only">Open menu</span>
                     <MoreHorizontal className="h-4 w-4" />
                  </Button>
               </DropdownMenuTrigger>
               <DropdownMenuContent align="end">
                  <DropdownMenuLabel>Actions</DropdownMenuLabel>
                  <DropdownMenuItem
                     onClick={() => navigator.clipboard.writeText(user.id)}
                  >
                     Copy user ID
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem>View profile</DropdownMenuItem>
                  <DropdownMenuItem>Edit user</DropdownMenuItem>
                  <DropdownMenuItem>Reset password</DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem className="text-destructive">
                     Delete user
                  </DropdownMenuItem>
               </DropdownMenuContent>
            </DropdownMenu>
         );
      },
   },
];

const HRMPage = () => {
   const [sorting, setSorting] = useState<SortingState>([]);
   const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
      {},
   );
   const [rowSelection, setRowSelection] = useState<RowSelectionState>({});

   // API
   const { getUsersByAdminAsync, getUsersByAdminState, isLoading } =
      useIdentityService();

   //    App state
   const { tenantId } = useAppSelector((state) => state.tenant);
   const { impersonatedUser } = useAppSelector((state) => state.auth);

   const { filters, setFilters } = useFilters<TUserFilter>({
      _page: 'number',
      _limit: 'number',
   });

   const {
      getPaginationItems,
      totalPages,
      totalRecords,
      currentPage,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
   } = usePaginationV2(
      getUsersByAdminState.data ?? {
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
      {
         pageSizeOverride: filters._limit ?? null,
         currentPageOverride: filters._page ?? null,
         fallbackPageSize: PAGE_LIMIT_OPTIONS[0],
      },
   );

   // Setup table
   const table = useReactTable({
      data: getUsersByAdminState.data?.items ?? [],
      columns,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      onSortingChange: setSorting,
      onColumnVisibilityChange: setColumnVisibility,
      onRowSelectionChange: setRowSelection,
      state: {
         sorting,
         columnVisibility,
         rowSelection,
      },
      manualPagination: true,
      pageCount: getUsersByAdminState.data?.total_pages ?? 0,
   });

   useEffect(() => {
      const fetchUsers = async () => {
         await getUsersByAdminAsync(filters);
      };

      fetchUsers();
   }, [filters, getUsersByAdminAsync, tenantId, impersonatedUser]);

   return (
      <div className="p-5">
         <div className="flex items-center justify-between">
            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  HRM - User Management
               </h1>
               <p className="text-muted-foreground">
                  Manage and view all users in the system
               </p>
            </div>
         </div>

         <LoadingOverlay isLoading={isLoading}>
            {/* Column Visibility Toggle */}
            <div className="flex items-center justify-end py-4">
               <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                     <Button variant="outline" className="ml-auto">
                        Columns <ChevronDown />
                     </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                     {table
                        .getAllColumns()
                        .filter((column) => column.getCanHide())
                        .map((column) => {
                           return (
                              <DropdownMenuCheckboxItem
                                 key={column.id}
                                 className="capitalize"
                                 checked={column.getIsVisible()}
                                 onCheckedChange={(value) =>
                                    column.toggleVisibility(!!value)
                                 }
                              >
                                 {column.id}
                              </DropdownMenuCheckboxItem>
                           );
                        })}
                  </DropdownMenuContent>
               </DropdownMenu>
            </div>

            {/* Data Table */}
            <div className="rounded-lg border bg-card">
               <div className="overflow-auto">
                  <Table>
                     <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                           <TableRow key={headerGroup.id}>
                              {headerGroup.headers.map((header) => {
                                 return (
                                    <TableHead key={header.id}>
                                       {header.isPlaceholder
                                          ? null
                                          : flexRender(
                                               header.column.columnDef.header,
                                               header.getContext(),
                                            )}
                                    </TableHead>
                                 );
                              })}
                           </TableRow>
                        ))}
                     </TableHeader>
                     <TableBody>
                        {table.getRowModel().rows?.length ? (
                           table.getRowModel().rows.map((row, index) => (
                              <TableRow
                                 key={row.id}
                                 data-state={row.getIsSelected() && 'selected'}
                                 className={`cursor-pointer transition-colors ${
                                    row.getIsSelected()
                                       ? '!bg-blue-400/20 hover:bg-blue-200'
                                       : `hover:bg-slate-300/50 ${
                                            index % 2 === 0
                                               ? 'bg-white'
                                               : 'bg-slate-300/30'
                                         }`
                                 }`}
                                 onClick={() => row.toggleSelected()}
                              >
                                 {row.getVisibleCells().map((cell) => (
                                    <TableCell key={cell.id}>
                                       {flexRender(
                                          cell.column.columnDef.cell,
                                          cell.getContext(),
                                       )}
                                    </TableCell>
                                 ))}
                              </TableRow>
                           ))
                        ) : (
                           <TableRow>
                              <TableCell
                                 colSpan={columns.length}
                                 className="h-24 text-center"
                              >
                                 No users found.
                              </TableCell>
                           </TableRow>
                        )}
                     </TableBody>
                  </Table>
               </div>

               {/* Pagination */}
               {(totalPages ?? 0) > 0 && (
                  <div className="flex items-center justify-between px-4 py-4 border-t">
                     <div className="flex items-center gap-2">
                        <Select
                           value={limitSelectValue}
                           onValueChange={(value) => {
                              setFilters({ _limit: Number(value), _page: 1 });
                           }}
                        >
                           <SelectTrigger className="w-auto h-9">
                              <SelectValue />
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
                           <span className="font-medium">{firstItemIndex}</span>{' '}
                           to{' '}
                           <span className="font-medium">{lastItemIndex}</span>{' '}
                           of{' '}
                           <span className="font-medium">{totalRecords}</span>{' '}
                           users
                        </div>
                     </div>

                     {getUsersByAdminState.data &&
                        getUsersByAdminState.data.total_pages > 0 && (
                           <div className="flex items-center gap-2">
                              {getPaginationItems().map((item, index) => {
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
               )}
            </div>
         </LoadingOverlay>
      </div>
   );
};

export default HRMPage;
