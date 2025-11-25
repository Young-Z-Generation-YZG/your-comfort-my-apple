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
import { Input } from '@components/ui/input';
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
import { useEffect, useMemo, useRef, useState } from 'react';
import { useRouter } from 'next/navigation';
import usePaginationV2 from '~/src/hooks/use-pagination';
import {
   ArrowUpDown,
   ChevronDown,
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
   MoreHorizontal,
   X,
} from 'lucide-react';
import { cn } from '~/src/infrastructure/lib/utils';
import { Gender } from '~/src/domain/enums/gender.enum';
import useFilters from '~/src/hooks/use-filter';
import { TUser } from '~/src/infrastructure/services/identity.service';
import { useDebounce } from '~/src/hooks/use-debounce';

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
   _email?: string | null;
   _firstName?: string | null;
   _lastName?: string | null;
   _phoneNumber?: string | null;
   _gender?: string[] | null;
   _emailVerified?: boolean | null;
};

const CustomersPage = () => {
   const router = useRouter();
   const { getUsersAsync, getUsersState, isLoading } = useIdentityService();

   const columns: ColumnDef<TUser>[] = useMemo(
      () => [
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
                           onClick={() =>
                              navigator.clipboard.writeText(user.id)
                           }
                        >
                           Copy user ID
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem
                           onClick={() =>
                              router.push(
                                 `/dashboard/customer-management/${user.id}`,
                              )
                           }
                        >
                           View profile
                        </DropdownMenuItem>
                     </DropdownMenuContent>
                  </DropdownMenu>
               );
            },
         },
      ],
      [router],
   );

   const { filters, setFilters } = useFilters<TUserFilter>({
      _page: 'number',
      _limit: 'number',
      _email: 'string',
      _firstName: 'string',
      _lastName: 'string',
      _phoneNumber: 'string',
      _gender: { array: 'string' },
      _emailVerified: 'boolean',
   });

   const [sorting, setSorting] = useState<SortingState>([]);
   const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
      {},
   );
   const [rowSelection, setRowSelection] = useState<RowSelectionState>({});

   // Local state for input values (not debounced)
   const [emailInput, setEmailInput] = useState<string>(filters._email ?? '');
   const [firstNameInput, setFirstNameInput] = useState<string>(
      filters._firstName ?? '',
   );
   const [lastNameInput, setLastNameInput] = useState<string>(
      filters._lastName ?? '',
   );
   const [phoneNumberInput, setPhoneNumberInput] = useState<string>(
      filters._phoneNumber ?? '',
   );

   // Debounce the input values
   const debouncedEmail = useDebounce<string>(emailInput, 500);
   const debouncedFirstName = useDebounce<string>(firstNameInput, 500);
   const debouncedLastName = useDebounce<string>(lastNameInput, 500);
   const debouncedPhoneNumber = useDebounce<string>(phoneNumberInput, 500);

   // Update filters when debounced values change
   useEffect(() => {
      if (filters._email !== debouncedEmail) {
         setFilters((prev) => {
            return {
               ...prev,
               _email: debouncedEmail || null,
            };
         });
      }
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [debouncedEmail]);

   useEffect(() => {
      if (filters._firstName !== debouncedFirstName) {
         setFilters((prev) => {
            return {
               ...prev,
               _firstName: debouncedFirstName || null,
            };
         });
      }
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [debouncedFirstName]);

   useEffect(() => {
      if (filters._lastName !== debouncedLastName) {
         setFilters((prev) => {
            return {
               ...prev,
               _lastName: debouncedLastName || null,
            };
         });
      }
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [debouncedLastName]);

   useEffect(() => {
      if (filters._phoneNumber !== debouncedPhoneNumber) {
         setFilters((prev) => {
            return {
               ...prev,
               _phoneNumber: debouncedPhoneNumber || null,
            };
         });
      }
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [debouncedPhoneNumber]);

   // Get users data from state or use empty array
   const usersData = useMemo(() => {
      return getUsersState.data?.items || [];
   }, [getUsersState.data]);

   const {
      currentPage,
      totalPages,
      totalRecords,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
      getPaginationItems,
   } = usePaginationV2(
      getUsersState.data ?? {
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
         fallbackPageSize: 10,
      },
   );

   const paginationItems = getPaginationItems();

   // Track previous API call params to prevent duplicate calls
   const prevApiParamsRef = useRef<string>('');

   // Setup table
   const table = useReactTable({
      data: usersData,
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
      const apiParams = JSON.stringify({
         _page: filters._page ?? undefined,
         _limit: filters._limit ?? undefined,
         _email: filters._email ?? undefined,
         _firstName: filters._firstName ?? undefined,
         _lastName: filters._lastName ?? undefined,
         _phoneNumber: filters._phoneNumber ?? undefined,
         _gender: filters._gender ?? undefined,
         _emailVerified: filters._emailVerified ?? undefined,
      });

      // Only call API if params actually changed
      if (prevApiParamsRef.current !== apiParams) {
         prevApiParamsRef.current = apiParams;
         getUsersAsync({
            _page: filters._page ?? undefined,
            _limit: filters._limit ?? undefined,
            _email: filters._email ?? undefined,
            _firstName: filters._firstName ?? undefined,
            _lastName: filters._lastName ?? undefined,
            _phoneNumber: filters._phoneNumber ?? undefined,
            _gender: filters._gender ?? undefined,
            _emailVerified: filters._emailVerified ?? undefined,
         });
      }
   }, [filters, getUsersAsync]);

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
            {/* Filter section */}
            <div className="rounded-lg border bg-card shadow-sm mb-4">
               <div className="p-6 space-y-4">
                  {/* Search Inputs Row */}
                  <div className="grid grid-cols-2 gap-4">
                     <div className="space-y-2">
                        <label className="text-sm font-medium text-foreground">
                           Email
                        </label>
                        <Input
                           placeholder="Search by email..."
                           value={emailInput}
                           onChange={(event) => {
                              setEmailInput(event.target.value);
                           }}
                           className="w-full"
                           type="email"
                        />
                     </div>

                     <div className="space-y-2">
                        <label className="text-sm font-medium text-foreground">
                           First Name
                        </label>
                        <Input
                           placeholder="Search by first name..."
                           value={firstNameInput}
                           onChange={(event) => {
                              setFirstNameInput(event.target.value);
                           }}
                           className="w-full"
                        />
                     </div>

                     <div className="space-y-2">
                        <label className="text-sm font-medium text-foreground">
                           Last Name
                        </label>
                        <Input
                           placeholder="Search by last name..."
                           value={lastNameInput}
                           onChange={(event) => {
                              setLastNameInput(event.target.value);
                           }}
                           className="w-full"
                        />
                     </div>

                     <div className="space-y-2">
                        <label className="text-sm font-medium text-foreground">
                           Phone Number
                        </label>
                        <Input
                           placeholder="Search by phone number..."
                           value={phoneNumberInput}
                           onChange={(event) => {
                              setPhoneNumberInput(event.target.value);
                           }}
                           className="w-full"
                           type="tel"
                        />
                     </div>
                  </div>

                  {/* Filter Dropdowns Row */}
                  <div className="flex items-center gap-3">
                     <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                           <Button variant="outline" className="h-10 gap-2">
                              <span className="font-medium">Gender</span>
                              <div className="flex items-center gap-2">
                                 {(() => {
                                    const selectedGenders =
                                       filters._gender ?? [];
                                    const genderCount = selectedGenders.length;

                                    if (genderCount === 0) {
                                       return null;
                                    }

                                    if (genderCount > 2) {
                                       return (
                                          <>
                                             {selectedGenders
                                                .slice(0, 2)
                                                .map((gender) => (
                                                   <Badge
                                                      key={gender}
                                                      variant="outline"
                                                      className={cn(
                                                         getGenderStyle(gender),
                                                      )}
                                                   >
                                                      {gender}
                                                   </Badge>
                                                ))}
                                             <Badge
                                                variant="outline"
                                                className="bg-gray-100 text-gray-800 border-gray-300"
                                             >
                                                +{genderCount - 2}
                                             </Badge>
                                          </>
                                       );
                                    }

                                    return selectedGenders.map((gender) => (
                                       <Badge
                                          key={gender}
                                          variant="outline"
                                          className={cn(getGenderStyle(gender))}
                                       >
                                          {gender}
                                       </Badge>
                                    ));
                                 })()}
                                 <ChevronDown />
                              </div>
                           </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent
                           align="start"
                           side="bottom"
                           sideOffset={4}
                           className="w-56"
                        >
                           <DropdownMenuLabel>Gender</DropdownMenuLabel>
                           <DropdownMenuSeparator />
                           {Object.values(Gender).map((gender) => {
                              const isChecked =
                                 filters._gender?.includes(gender) ?? false;

                              return (
                                 <DropdownMenuCheckboxItem
                                    key={gender}
                                    onSelect={(e) => e.preventDefault()}
                                    checked={isChecked}
                                    onCheckedChange={() => {
                                       setFilters((prev) => {
                                          const currentGenders =
                                             prev._gender ?? [];
                                          const isGenderSelected =
                                             currentGenders.includes(gender);

                                          return {
                                             ...prev,
                                             _gender: isGenderSelected
                                                ? currentGenders.filter(
                                                     (g) => g !== gender,
                                                  )
                                                : [...currentGenders, gender],
                                          };
                                       });
                                    }}
                                 >
                                    <div className="flex items-center gap-2">
                                       <Badge
                                          variant="outline"
                                          className={cn(getGenderStyle(gender))}
                                       >
                                          {gender}
                                       </Badge>
                                    </div>
                                 </DropdownMenuCheckboxItem>
                              );
                           })}
                           <DropdownMenuSeparator />
                           <div className="p-2">
                              <Button
                                 variant="outline"
                                 size="sm"
                                 className="w-full"
                                 onClick={(e) => {
                                    e.stopPropagation();
                                    setFilters((prev) => ({
                                       ...prev,
                                       _gender: [],
                                    }));
                                 }}
                                 disabled={(filters._gender?.length ?? 0) === 0}
                              >
                                 Clear All
                              </Button>
                           </div>
                        </DropdownMenuContent>
                     </DropdownMenu>

                     <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                           <Button variant="outline" className="h-10 gap-2">
                              <span className="font-medium">
                                 Email Verified
                              </span>
                              <div className="flex items-center gap-2">
                                 {filters._emailVerified !== null &&
                                    filters._emailVerified !== undefined && (
                                       <Badge
                                          variant="outline"
                                          className={
                                             filters._emailVerified
                                                ? 'bg-green-100 text-green-800 border-green-300'
                                                : 'bg-red-100 text-red-800 border-red-300'
                                          }
                                       >
                                          {filters._emailVerified
                                             ? 'Verified'
                                             : 'Not Verified'}
                                       </Badge>
                                    )}
                                 <ChevronDown />
                              </div>
                           </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent
                           align="start"
                           side="bottom"
                           sideOffset={4}
                           className="w-56"
                        >
                           <DropdownMenuLabel>Email Verified</DropdownMenuLabel>
                           <DropdownMenuSeparator />
                           <DropdownMenuCheckboxItem
                              onSelect={(e) => e.preventDefault()}
                              checked={filters._emailVerified === true}
                              onCheckedChange={(checked) => {
                                 setFilters((prev) => ({
                                    ...prev,
                                    _emailVerified: checked ? true : null,
                                 }));
                              }}
                           >
                              <div className="flex items-center gap-2">
                                 <Badge
                                    variant="outline"
                                    className="bg-green-100 text-green-800 border-green-300"
                                 >
                                    Verified
                                 </Badge>
                              </div>
                           </DropdownMenuCheckboxItem>
                           <DropdownMenuCheckboxItem
                              onSelect={(e) => e.preventDefault()}
                              checked={filters._emailVerified === false}
                              onCheckedChange={(checked) => {
                                 setFilters((prev) => ({
                                    ...prev,
                                    _emailVerified: checked ? false : null,
                                 }));
                              }}
                           >
                              <div className="flex items-center gap-2">
                                 <Badge
                                    variant="outline"
                                    className="bg-red-100 text-red-800 border-red-300"
                                 >
                                    Not Verified
                                 </Badge>
                              </div>
                           </DropdownMenuCheckboxItem>
                           <DropdownMenuSeparator />
                           <div className="p-2">
                              <Button
                                 variant="outline"
                                 size="sm"
                                 className="w-full"
                                 onClick={(e) => {
                                    e.stopPropagation();
                                    setFilters((prev) => ({
                                       ...prev,
                                       _emailVerified: null,
                                    }));
                                 }}
                                 disabled={
                                    filters._emailVerified === null ||
                                    filters._emailVerified === undefined
                                 }
                              >
                                 Clear All
                              </Button>
                           </div>
                        </DropdownMenuContent>
                     </DropdownMenu>

                     <Button
                        variant="outline"
                        onClick={() => {
                           setFilters({
                              _email: null,
                              _firstName: null,
                              _lastName: null,
                              _phoneNumber: null,
                              _gender: [],
                              _emailVerified: null,
                              _page: 1,
                           });
                           setEmailInput('');
                           setFirstNameInput('');
                           setLastNameInput('');
                           setPhoneNumberInput('');
                        }}
                        className={cn(
                           'h-10 px-4 gap-2 whitespace-nowrap',
                           (filters._email ||
                              filters._firstName ||
                              filters._lastName ||
                              filters._phoneNumber ||
                              (filters._gender?.length ?? 0) > 0 ||
                              filters._emailVerified !== null) &&
                              'border-destructive text-destructive bg-destructive/10 hover:bg-destructive/20',
                        )}
                        disabled={
                           !filters._email &&
                           !filters._firstName &&
                           !filters._lastName &&
                           !filters._phoneNumber &&
                           (filters._gender?.length ?? 0) === 0 &&
                           (filters._emailVerified === null ||
                              filters._emailVerified === undefined)
                        }
                     >
                        <X className="h-4 w-4" />
                        Clear Filters
                     </Button>
                  </div>
               </div>
            </div>

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

                     {getUsersState.data &&
                        getUsersState.data.total_pages > 0 && (
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
               )}
            </div>
         </LoadingOverlay>
      </div>
   );
};

export default CustomersPage;
