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
import usePagination from '~/src/hooks/use-pagination';
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

const fakeData = {
   total_records: 6,
   total_pages: 1,
   page_size: 10,
   current_page: 1,
   items: [
      {
         id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'user@gmail.com',
         normalized_user_name: 'USER@GMAIL.COM',
         email: 'user@gmail.com',
         normalized_email: 'USER@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: '422452ca-6ac1-4fac-8d37-8f59c5305d63',
            user_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
            first_name: 'USER',
            last_name: 'USER',
            birth_day: '2006-11-19T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:04.828081Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: 'e79d0b6f-af5a-4162-a6fd-8194d5a5f616',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'staff@gmail.com',
         normalized_user_name: 'STAFF@GMAIL.COM',
         email: 'staff@gmail.com',
         normalized_email: 'STAFF@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: '573ff10d-f412-4291-b6d8-89594fc7c0fd',
            user_id: 'e79d0b6f-af5a-4162-a6fd-8194d5a5f616',
            first_name: 'STAFF',
            last_name: 'USER',
            birth_day: '2005-10-18T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:05.487937Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: '65dad719-7368-4d9f-b623-f308299e9575',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'admin@gmail.com',
         normalized_user_name: 'ADMIN@GMAIL.COM',
         email: 'admin@gmail.com',
         normalized_email: 'ADMIN@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: 'e5676437-47bd-47ad-b5f9-21f7278c061f',
            user_id: '65dad719-7368-4d9f-b623-f308299e9575',
            first_name: 'ADMIN',
            last_name: 'USER',
            birth_day: '2004-09-17T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:05.834715Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: 'be0cd669-237a-484d-b3cf-793e0ad1b0ea',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'adminsuper@gmail.com',
         normalized_user_name: 'ADMINSUPER@GMAIL.COM',
         email: 'adminsuper@gmail.com',
         normalized_email: 'ADMINSUPER@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: '79ce1c51-a7f5-47df-9c51-d3797dbc4818',
            user_id: 'be0cd669-237a-484d-b3cf-793e0ad1b0ea',
            first_name: 'ADMIN SUPER',
            last_name: 'USER',
            birth_day: '2003-08-16T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:06.084494Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: '8d8059c4-38b8-4f62-a776-4527e059b14a',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'foobar@gmail.com',
         normalized_user_name: 'FOOBAR@GMAIL.COM',
         email: 'foobar@gmail.com',
         normalized_email: 'FOOBAR@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: 'd46461c7-e1f0-4784-b729-f421bc17eb04',
            user_id: '8d8059c4-38b8-4f62-a776-4527e059b14a',
            first_name: 'FOO',
            last_name: 'BAR',
            birth_day: '2007-12-20T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:06.271631Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'lov3rinve146@gmail.com',
         normalized_user_name: 'LOV3RINVE146@GMAIL.COM',
         email: 'lov3rinve146@gmail.com',
         normalized_email: 'LOV3RINVE146@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0123456789',
         profile: {
            id: '943788f8-298e-4fba-9c6e-1b322719671b',
            user_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
            first_name: 'Bach',
            last_name: 'Le',
            birth_day: '2003-08-16T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T14:34:18.202777Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_page=1&_limit=10',
      prev: null,
      next: null,
      last: '?_page=1&_limit=10',
   },
};

export type TUser = (typeof fakeData.items)[number];

type TUserFilter = {
   _page?: number | null;
   _limit?: number | null;
   _email?: string | null;
   _firstName?: string | null;
   _lastName?: string | null;
   _phoneNumber?: string | null;
};

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
   const { getUsersByAdminAsync, getUsersByAdminState, isLoading } =
      useIdentityService();

   const { filters, setFilters } = useFilters<TUserFilter>({
      _page: 'number',
      _limit: 'number',
      _email: 'string',
      _firstName: 'string',
      _lastName: 'string',
      _phoneNumber: 'string',
   });

   const [sorting, setSorting] = useState<SortingState>([]);
   const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
      {},
   );
   const [rowSelection, setRowSelection] = useState<RowSelectionState>({});

   //    App state
   const { tenantId } = useAppSelector((state) => state.tenant);
   const { impersonatedUser } = useAppSelector((state) => state.auth);

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
      getUsersByAdminState.isSuccess &&
         getUsersByAdminState.data &&
         getUsersByAdminState.data.items.length > 0
         ? getUsersByAdminState.data
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

   // Setup table
   const table = useReactTable({
      data: paginationItems,
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
      pageCount: totalPages,
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
               {totalPages >= 1 && (
                  <div className="flex items-center justify-between px-4 py-4 border-t">
                     <div className="flex items-center gap-2">
                        <Select
                           value={filters._limit?.toString() || '10'}
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
                           <span className="font-medium">
                              {paginationItems.length > 0
                                 ? (currentPage - 1) * pageSize + 1
                                 : 0}
                           </span>{' '}
                           to{' '}
                           <span className="font-medium">
                              {Math.min(currentPage * pageSize, totalRecords)}
                           </span>{' '}
                           of{' '}
                           <span className="font-medium">{totalRecords}</span>{' '}
                           users
                        </div>
                     </div>

                     <div className="flex items-center gap-2">
                        {/* First Page */}
                        <Button
                           variant="outline"
                           size="icon"
                           className="h-9 w-9"
                           onClick={() => {
                              setFilters({ _page: 1 });
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
                                 setFilters({ _page: currentPage - 1 });
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
                                       setFilters({ _page: page as number });
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
                                 setFilters({ _page: currentPage + 1 });
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
                              setFilters({ _page: totalPages });
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
   );
};

export default HRMPage;
