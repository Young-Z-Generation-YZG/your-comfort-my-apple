'use client';

import useOrderingService from '~/src/hooks/api/use-ordering-service';
import usePagination from '~/src/hooks/use-pagination';
import { useEffect, useState } from 'react';
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
import { Checkbox } from '@components/ui/checkbox';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import { Input } from '@components/ui/input';
import {
   ArrowUpDown,
   MoreHorizontal,
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
   ChevronDown,
   Search,
} from 'lucide-react';
import {
   DropdownMenu,
   DropdownMenuCheckboxItem,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuLabel,
   DropdownMenuSeparator,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import { EPaymentMethod } from '~/src/domain/enums/payment-method.enum';
import { EOrderStatus } from '~/src/domain/enums/order-status.enum';
import { EPromotionType } from '~/src/domain/enums/promotion-type.enum';
import { cn } from '~/src/infrastructure/lib/utils';
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
import { useRouter } from 'next/navigation';
import { TOrder } from '~/src/infrastructure/services/order.service';
import usePaginationV2 from '~/src/hooks/use-pagination-v2';

const getStatusStyle = (status: string) => {
   switch (status) {
      case EOrderStatus.PENDING:
         return 'bg-yellow-100 text-yellow-800 border-yellow-300';
      case EOrderStatus.CONFIRMED:
         return 'bg-blue-100 text-blue-800 border-blue-300';
      case EOrderStatus.PREPARING:
         return 'bg-purple-100 text-purple-800 border-purple-300';
      case EOrderStatus.DELIVERING:
         return 'bg-indigo-100 text-indigo-800 border-indigo-300';
      case EOrderStatus.DELIVERED:
         return 'bg-green-100 text-green-800 border-green-300';
      case EOrderStatus.CANCELLED:
         return 'bg-red-100 text-red-800 border-red-300';
      case EOrderStatus.PAID:
         return 'bg-green-100 text-green-800 border-green-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const getPaymentMethodStyle = (method: string) => {
   switch (method) {
      case EPaymentMethod.VNPAY:
         return 'bg-blue-100 text-blue-800 border-blue-300';
      case EPaymentMethod.MOMO:
         return 'bg-pink-100 text-pink-800 border-pink-300';
      case EPaymentMethod.COD:
         return 'bg-green-100 text-green-800 border-green-300';
      case EPaymentMethod.BLOCKCHAIN:
         return 'bg-purple-100 text-purple-800 border-purple-300';
      case EPaymentMethod.UNKNOWN:
         return 'bg-gray-100 text-gray-800 border-gray-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const getPromotionTypeStyle = (type: string) => {
   switch (type) {
      case EPromotionType.COUPON:
         return 'bg-orange-100 text-orange-800 border-orange-300';
      case EPromotionType.EVENT:
         return 'bg-cyan-100 text-cyan-800 border-cyan-300';
      case EPromotionType.EVENT_ITEM:
         return 'bg-teal-100 text-teal-800 border-teal-300';
      case EPromotionType.UNKNOWN:
         return 'bg-gray-100 text-gray-800 border-gray-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const columns: ColumnDef<TOrder>[] = [
   {
      id: 'select',
      header: ({ table }) => (
         <Checkbox
            checked={
               table.getIsAllPageRowsSelected() ||
               (table.getIsSomePageRowsSelected() && 'indeterminate')
            }
            onCheckedChange={(value) =>
               table.toggleAllPageRowsSelected(!!value)
            }
            aria-label="Select all"
         />
      ),
      cell: ({ row }) => (
         <Checkbox
            checked={row.getIsSelected()}
            onCheckedChange={(value) => row.toggleSelected(!!value)}
            aria-label="Select row"
         />
      ),
      enableSorting: false,
      enableHiding: false,
   },
   {
      accessorKey: 'order_code',
      header: ({ column, table: tableInstance }) => {
         // Access the parent component's state through table meta
         const showSearch = (tableInstance.options.meta as any)
            ?.showOrderCodeSearch;
         const setShowSearch = (tableInstance.options.meta as any)
            ?.setShowOrderCodeSearch;

         return (
            <div className="relative">
               <div className="flex items-center justify-between">
                  <span className="font-medium">Order Code</span>
                  <Button
                     variant="ghost"
                     size="icon"
                     onClick={() => setShowSearch?.(!showSearch)}
                     className="h-8 w-8"
                  >
                     <Search className="h-4 w-4" />
                  </Button>
               </div>
               {showSearch && (
                  <div className="absolute top-full left-0 z-50 mt-2 w-72 rounded-md border bg-white p-3 shadow-lg">
                     <div className="relative">
                        <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
                        <Input
                           placeholder="Search order code..."
                           value={(column.getFilterValue() as string) ?? ''}
                           onChange={(event) =>
                              column.setFilterValue(event.target.value)
                           }
                           className="h-9 w-full pl-8"
                        />
                     </div>
                  </div>
               )}
            </div>
         );
      },
      cell: ({ row }) => (
         <div className="font-medium">{row.getValue('order_code')}</div>
      ),
   },
   {
      accessorKey: 'customer_email',
      header: ({ column, table: tableInstance }) => {
         // Access the parent component's state through table meta
         const showSearch = (tableInstance.options.meta as any)
            ?.showCustomerEmailSearch;
         const setShowSearch = (tableInstance.options.meta as any)
            ?.setShowCustomerEmailSearch;

         return (
            <div className="relative">
               <div className="flex items-center justify-between">
                  <span className="font-medium">Customer Email</span>
                  <Button
                     variant="ghost"
                     size="icon"
                     onClick={() => setShowSearch?.(!showSearch)}
                     className="h-8 w-8"
                  >
                     <Search className="h-4 w-4" />
                  </Button>
               </div>
               {showSearch && (
                  <div className="absolute top-full left-0 z-50 mt-2 w-72 rounded-md border bg-white p-3 shadow-lg">
                     <div className="relative">
                        <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
                        <Input
                           placeholder="Search customer email..."
                           value={(column.getFilterValue() as string) ?? ''}
                           onChange={(event) =>
                              column.setFilterValue(event.target.value)
                           }
                           className="h-9 w-full pl-8"
                        />
                     </div>
                  </div>
               )}
            </div>
         );
      },
      cell: ({ row }) => (
         <div className="lowercase">{row.getValue('customer_email')}</div>
      ),
   },
   {
      accessorKey: 'status',
      header: 'Status',
      cell: ({ row }) => {
         const status = row.getValue('status') as string;

         return (
            <Badge variant="outline" className={getStatusStyle(status)}>
               {status}
            </Badge>
         );
      },
   },
   {
      accessorKey: 'payment_method',
      header: 'Payment Method',
      cell: ({ row }) => {
         const method = row.getValue('payment_method') as string;

         return (
            <Badge variant="outline" className={getPaymentMethodStyle(method)}>
               {method}
            </Badge>
         );
      },
   },
   {
      accessorKey: 'total_amount',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Total Amount
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => {
         const amount = parseFloat(row.getValue('total_amount'));
         const formatted = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
         }).format(amount);
         return <div className="font-medium">{formatted}</div>;
      },
   },
   {
      id: 'items_count',
      header: 'Items',
      cell: ({ row }) => {
         const itemsCount = row.original.order_items.length;
         return <div className="text-center">{itemsCount}</div>;
      },
   },
   {
      id: 'discount_info',
      header: 'Discount',
      cell: ({ row }) => {
         const discountAmount = row.original.discount_amount;
         const promotionType = row.original.promotion_type;

         if (!discountAmount || !promotionType) {
            return <div className="text-muted-foreground">-</div>;
         }

         const formatted = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
         }).format(discountAmount);

         return (
            <div>
               <div className="font-medium">{formatted}</div>
               <Badge
                  variant="outline"
                  className={cn(
                     'text-xs',
                     getPromotionTypeStyle(promotionType),
                  )}
               >
                  {promotionType}
               </Badge>
            </div>
         );
      },
   },
   {
      id: 'shipping_address',
      header: 'Shipping Address',
      cell: ({ row }) => {
         const address = row.original.shipping_address;
         return (
            <div className="max-w-[200px]">
               <div className="font-medium">{address.contact_name}</div>
               <div className="text-sm text-muted-foreground">
                  {address.contact_phone_number}
               </div>
               <div className="text-xs text-muted-foreground truncate">
                  {address.contact_address_line}, {address.contact_district},{' '}
                  {address.contact_province}
               </div>
            </div>
         );
      },
   },
   {
      accessorKey: 'created_at',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Created At
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => {
         const date = new Date(row.getValue('created_at'));
         return (
            <div className="text-sm">
               {date.toLocaleDateString('en-US', {
                  year: 'numeric',
                  month: 'short',
                  day: 'numeric',
               })}
               <div className="text-xs text-muted-foreground">
                  {date.toLocaleTimeString('en-US', {
                     hour: '2-digit',
                     minute: '2-digit',
                  })}
               </div>
            </div>
         );
      },
   },
   {
      id: 'actions',
      enableHiding: false,
      cell: ({ row, table }) => {
         const order = row.original;
         const onViewDetails = (table.options.meta as any)?.onViewDetails;

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
                        navigator.clipboard.writeText(order.order_id)
                     }
                  >
                     Copy order ID
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem
                     onClick={() => onViewDetails?.(order.order_id)}
                  >
                     View details
                  </DropdownMenuItem>
                  <DropdownMenuItem>View customer</DropdownMenuItem>
                  <DropdownMenuItem>Update status</DropdownMenuItem>
               </DropdownMenuContent>
            </DropdownMenu>
         );
      },
   },
];

type TOrderFilter = {
   _page?: number | null;
   _limit?: number | null;
   _orderStatus?: string[] | null;
   _customerEmail?: string | null;
};

const OnlineOrdersPage = () => {
   const [sorting, setSorting] = useState<SortingState>([]);
   const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
   const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
      {},
   );
   const [rowSelection, setRowSelection] = useState({});
   const [showOrderCodeSearch, setShowOrderCodeSearch] = useState(false);
   const [showCustomerEmailSearch, setShowCustomerEmailSearch] =
      useState(false);

   //    App state
   const { tenantId } = useAppSelector((state) => state.tenant);
   const { impersonatedUser } = useAppSelector((state) => state.auth);

   const router = useRouter();

   const { getOrdersByAdminAsync, getOrdersByAdminState, isLoading } =
      useOrderingService();

   const { filters, setFilters } = useFilters<TOrderFilter>({
      _page: 'number',
      _limit: 'number',
      _orderStatus: { array: 'string' },
      _customerEmail: 'string',
   });
   const { getPaginationItems } = usePaginationV2(
      getOrdersByAdminState.data ?? {
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
      getOrdersByAdminAsync({
         _page: filters._page ?? undefined,
         _limit: filters._limit ?? undefined,
         _orderStatus: filters._orderStatus ?? undefined,
      });
   }, [filters, getOrdersByAdminAsync, tenantId, impersonatedUser]);

   const table = useReactTable({
      data: getOrdersByAdminState.data?.items ?? [],
      columns: columns,
      onSortingChange: setSorting,
      onColumnFiltersChange: setColumnFilters,
      getCoreRowModel: getCoreRowModel(),
      getPaginationRowModel: getPaginationRowModel(),
      getSortedRowModel: getSortedRowModel(),
      getFilteredRowModel: getFilteredRowModel(),
      onColumnVisibilityChange: setColumnVisibility,
      onRowSelectionChange: setRowSelection,
      // Prevent auto resets that can trigger nested state updates during render
      autoResetPageIndex: false,
      autoResetExpanded: false,
      state: {
         sorting: sorting,
         columnFilters: columnFilters,
         columnVisibility: columnVisibility,
         rowSelection: rowSelection,
      },
      meta: {
         showOrderCodeSearch,
         setShowOrderCodeSearch,
         showCustomerEmailSearch,
         setShowCustomerEmailSearch,
         onViewDetails: (orderId: string) =>
            router.push(`/dashboard/online/orders/${orderId}/details`),
      },
   });

   return (
      <div className="p-5">
         <div className="flex items-center justify-between">
            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Online Orders Management
               </h1>
               <p className="text-muted-foreground">
                  Manage and view all online orders in the system
               </p>
            </div>
         </div>

         <LoadingOverlay isLoading={isLoading}>
            <div className="flex items-center py-4">
               <div className="flex items-center gap-2">
                  <Input
                     placeholder="Filter emails..."
                     value={
                        (table
                           .getColumn('customer_email')
                           ?.getFilterValue() as string) ?? ''
                     }
                     onChange={(event) => {
                        setFilters((prev) => {
                           return {
                              ...prev,
                              _customerEmail: event.target.value,
                           };
                        });
                     }}
                     className="max-w-sm"
                  />
                  <DropdownMenu>
                     <DropdownMenuTrigger asChild>
                        <Button variant="outline" className="ml-auto">
                           Status Filter
                           <div className="flex items-center gap-2">
                              {filters._orderStatus &&
                              filters._orderStatus?.length &&
                              filters._orderStatus?.length > 2 ? (
                                 <>
                                    {filters._orderStatus
                                       .slice(0, 2)
                                       .map((status) => (
                                          <Badge
                                             key={status}
                                             variant="outline"
                                             className={cn(
                                                getStatusStyle(status),
                                             )}
                                          >
                                             {status}
                                          </Badge>
                                       ))}
                                    <Badge
                                       variant="outline"
                                       className="bg-gray-100 text-gray-800 border-gray-300"
                                    >
                                       +
                                       {(filters._orderStatus?.length ?? 0) - 2}
                                    </Badge>
                                 </>
                              ) : filters._orderStatus &&
                                filters._orderStatus.length > 0 ? (
                                 filters._orderStatus.map((status) => (
                                    <Badge
                                       key={status}
                                       variant="outline"
                                       className={cn(getStatusStyle(status))}
                                    >
                                       {status}
                                    </Badge>
                                 ))
                              ) : null}
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
                        <DropdownMenuLabel>Order Status</DropdownMenuLabel>
                        <DropdownMenuSeparator />
                        <DropdownMenuCheckboxItem
                           onSelect={(e) => e.preventDefault()}
                           checked={
                              filters._orderStatus?.includes('PENDING') ?? false
                           }
                           onCheckedChange={() => {
                              setFilters((prev) => {
                                 if (prev._orderStatus?.includes('PENDING')) {
                                    return {
                                       ...prev,
                                       _orderStatus: prev._orderStatus?.filter(
                                          (s) => s !== 'PENDING',
                                       ),
                                    };
                                 } else {
                                    return {
                                       ...prev,
                                       _orderStatus: [
                                          ...(prev._orderStatus ?? []),
                                          'PENDING',
                                       ],
                                    };
                                 }
                              });
                           }}
                        >
                           <div className="flex items-center gap-2">
                              <Badge
                                 variant="outline"
                                 className="bg-yellow-100 text-yellow-800 border-yellow-300"
                              >
                                 {EOrderStatus.PENDING}
                              </Badge>
                           </div>
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           onSelect={(e) => e.preventDefault()}
                           checked={
                              filters._orderStatus?.includes('CONFIRMED') ??
                              false
                           }
                           onCheckedChange={() => {
                              setFilters((prev) => {
                                 if (prev._orderStatus?.includes('CONFIRMED')) {
                                    return {
                                       ...prev,
                                       _orderStatus: prev._orderStatus?.filter(
                                          (s) => s !== 'CONFIRMED',
                                       ),
                                    };
                                 } else {
                                    return {
                                       ...prev,
                                       _orderStatus: [
                                          ...(prev._orderStatus ?? []),
                                          'CONFIRMED',
                                       ],
                                    };
                                 }
                              });
                           }}
                        >
                           <div className="flex items-center gap-2">
                              <Badge
                                 variant="outline"
                                 className="bg-blue-100 text-blue-800 border-blue-300"
                              >
                                 {EOrderStatus.CONFIRMED}
                              </Badge>
                           </div>
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           onSelect={(e) => e.preventDefault()}
                           checked={
                              filters._orderStatus?.includes('PAID') ?? false
                           }
                           onCheckedChange={() => {
                              setFilters((prev) => {
                                 if (prev._orderStatus?.includes('PAID')) {
                                    return {
                                       ...prev,
                                       _orderStatus: prev._orderStatus?.filter(
                                          (s) => s !== 'PAID',
                                       ),
                                    };
                                 } else {
                                    return {
                                       ...prev,
                                       _orderStatus: [
                                          ...(prev._orderStatus ?? []),
                                          'PAID',
                                       ],
                                    };
                                 }
                              });
                           }}
                        >
                           <div className="flex items-center gap-2">
                              <Badge
                                 variant="outline"
                                 className="bg-green-100 text-green-800 border-green-300"
                              >
                                 PAID
                              </Badge>
                           </div>
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           onSelect={(e) => e.preventDefault()}
                           checked={
                              filters._orderStatus?.includes('PREPARING') ??
                              false
                           }
                           onCheckedChange={() => {
                              setFilters((prev) => {
                                 if (prev._orderStatus?.includes('PREPARING')) {
                                    return {
                                       ...prev,
                                       _orderStatus: prev._orderStatus?.filter(
                                          (s) => s !== 'PREPARING',
                                       ),
                                    };
                                 } else {
                                    return {
                                       ...prev,
                                       _orderStatus: [
                                          ...(prev._orderStatus ?? []),
                                          'PREPARING',
                                       ],
                                    };
                                 }
                              });
                           }}
                        >
                           <div className="flex items-center gap-2">
                              <Badge
                                 variant="outline"
                                 className="bg-purple-100 text-purple-800 border-purple-300"
                              >
                                 {EOrderStatus.PREPARING}
                              </Badge>
                           </div>
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           onSelect={(e) => e.preventDefault()}
                           checked={
                              filters._orderStatus?.includes('DELIVERING') ??
                              false
                           }
                           onCheckedChange={() => {
                              setFilters((prev) => {
                                 if (
                                    prev._orderStatus?.includes('DELIVERING')
                                 ) {
                                    return {
                                       ...prev,
                                       _orderStatus: prev._orderStatus?.filter(
                                          (s) => s !== 'DELIVERING',
                                       ),
                                    };
                                 } else {
                                    return {
                                       ...prev,
                                       _orderStatus: [
                                          ...(prev._orderStatus ?? []),
                                          'DELIVERING',
                                       ],
                                    };
                                 }
                              });
                           }}
                        >
                           <div className="flex items-center gap-2">
                              <Badge
                                 variant="outline"
                                 className="bg-indigo-100 text-indigo-800 border-indigo-300"
                              >
                                 {EOrderStatus.DELIVERING}
                              </Badge>
                           </div>
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           onSelect={(e) => e.preventDefault()}
                           checked={
                              filters._orderStatus?.includes('DELIVERED') ??
                              false
                           }
                           onCheckedChange={() => {
                              setFilters((prev) => {
                                 if (prev._orderStatus?.includes('DELIVERED')) {
                                    return {
                                       ...prev,
                                       _orderStatus: prev._orderStatus?.filter(
                                          (s) => s !== 'DELIVERED',
                                       ),
                                    };
                                 } else {
                                    return {
                                       ...prev,
                                       _orderStatus: [
                                          ...(prev._orderStatus ?? []),
                                          'DELIVERED',
                                       ],
                                    };
                                 }
                              });
                           }}
                        >
                           <div className="flex items-center gap-2">
                              <Badge
                                 variant="outline"
                                 className="bg-green-100 text-green-800 border-green-300"
                              >
                                 {EOrderStatus.DELIVERED}
                              </Badge>
                           </div>
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           onSelect={(e) => e.preventDefault()}
                           checked={
                              filters._orderStatus?.includes('CANCELLED') ??
                              false
                           }
                           onCheckedChange={() => {
                              setFilters((prev) => {
                                 if (prev._orderStatus?.includes('CANCELLED')) {
                                    return {
                                       ...prev,
                                       _orderStatus: prev._orderStatus?.filter(
                                          (s) => s !== 'CANCELLED',
                                       ),
                                    };
                                 } else {
                                    return {
                                       ...prev,
                                       _orderStatus: [
                                          ...(prev._orderStatus ?? []),
                                          'CANCELLED',
                                       ],
                                    };
                                 }
                              });
                           }}
                        >
                           <div className="flex items-center gap-2">
                              <Badge
                                 variant="outline"
                                 className="bg-red-100 text-red-800 border-red-300"
                              >
                                 {EOrderStatus.CANCELLED}
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
                                 setFilters((prev) => {
                                    return {
                                       ...prev,
                                       _orderStatus: [],
                                    };
                                 });
                              }}
                              disabled={filters._orderStatus?.length === 0}
                           >
                              Clear All
                           </Button>
                        </div>
                     </DropdownMenuContent>
                  </DropdownMenu>
               </div>

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

            <div className="overflow-hidden rounded-md border">
               <Table className="">
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
               </Table>
            </div>

            {/* Pagination Controls */}
            {/* Pagination Controls */}
            {getOrdersByAdminState.data &&
               getOrdersByAdminState.data.total_pages > 0 && (
                  <div className="flex items-center gap-2 justify-end mr-5 py-5">
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
                           item.value ===
                              (getOrdersByAdminState.data?.current_page || 1);

                        return (
                           <Button
                              key={`${item.type}-${item.label}-${index}`}
                              variant={isCurrentPage ? 'default' : 'outline'}
                              size="sm"
                              disabled={item.disabled || item.value === null}
                              onClick={() => {
                                 if (item.value !== null && !item.disabled) {
                                    setFilters((prev) => ({
                                       ...prev,
                                       _page: item.value!,
                                    }));
                                 }
                              }}
                              className={cn(
                                 isCurrentPage &&
                                    'bg-black text-white hover:bg-black/90',
                              )}
                           >
                              {item.label}
                           </Button>
                        );
                     })}
                  </div>
               )}
         </LoadingOverlay>
      </div>
   );
};

export default OnlineOrdersPage;
