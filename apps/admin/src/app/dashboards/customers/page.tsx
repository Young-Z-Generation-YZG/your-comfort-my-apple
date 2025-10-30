import { LoadingOverlay } from '@components/loading-overlay';
import useIdentityService from '~/src/hooks/api/use-identity-service';
import {
   ColumnDef,
   ColumnFiltersState,
   flexRender,
   getCoreRowModel,
   getFilteredRowModel,
   getPaginationRowModel,
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
import useFilter from '~/src/hooks/use-filter';
import { useEffect, useMemo } from 'react';
import usePagination from '~/src/hooks/use-pagination';

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

type IUserFilter = {
   _page?: number | null;
   _limit?: number | null;
   _email?: string | null;
   _firstName?: string | null;
   _lastName?: string | null;
   _phoneNumber?: string | null;
};

const columns: ColumnDef<TUser>[] = [];

const UsersPage = () => {
   const { getUsersByAdminAsync, getUsersByAdminState, isLoading } =
      useIdentityService();

   const { filters, setFilters, clearFilters, activeFilterCount } =
      useFilter<IUserFilter>();

   const queryParams = useMemo(() => {
      return {
         _page: filters._page || 1,
         _limit: filters._limit || 10,
         _email: filters._email || null,
         _firstName: filters._firstName || null,
         _lastName: filters._lastName || null,
         _phoneNumber: filters._phoneNumber || null,
      };
   }, [filters]);

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
      getUsersByAdminState.data && getUsersByAdminState.data.items.length > 0
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

   useEffect(() => {
      const fetchUsers = async () => {
         await getUsersByAdminAsync(queryParams);
      };

      fetchUsers();
   }, [queryParams, getUsersByAdminAsync]);

   return (
      <div className="p-5">
         <LoadingOverlay isLoading={isLoading}>
            <div className="flex items-center py-4">
               <div></div>

               {/* <DropdownMenu>
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
               </DropdownMenu> */}
            </div>

            <div className="overflow-hidden rounded-md border">
               {/* <Table className="">
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
                              No results.
                           </TableCell>
                        </TableRow>
                     )}
                  </TableBody>
               </Table> */}
            </div>
         </LoadingOverlay>
      </div>
   );
};

export default UsersPage;
