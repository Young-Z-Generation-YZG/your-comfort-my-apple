'use client';

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
import {
   ArrowUpDown,
   ChevronDown,
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
   MoreHorizontal,
   Package,
} from 'lucide-react';
import { cn } from '~/src/infrastructure/lib/utils';
import { LoadingOverlay } from '@components/loading-overlay';
import useInventoryService from '~/src/hooks/api/use-inventory-service';
import { useEffect, useMemo, useState } from 'react';
import usePagination from '~/src/hooks/use-pagination';
import Image from 'next/image';
import useFilters from '~/src/hooks/use-filter';
import { TSku } from '~/src/domain/types/catalog.type';

type TSkuFilter = {
   _page?: number | null;
   _limit?: number | null;
   _colors?: string[] | null;
   _storages?: string[] | null;
   _models?: string[] | null;
};

// Helper function to get state badge styles
const getStateStyle = (state: string) => {
   switch (state) {
      case 'ACTIVE':
         return 'bg-green-100 text-green-800 border-green-300';
      case 'INACTIVE':
         return 'bg-gray-100 text-gray-800 border-gray-300';
      case 'OUT_OF_STOCK':
         return 'bg-red-100 text-red-800 border-red-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const columns: ColumnDef<TSku>[] = [
   {
      accessorKey: 'display_image_url',
      header: 'Image',
      cell: ({ row }) => {
         const imageUrl = row.getValue('display_image_url') as string;
         return (
            <div className="flex items-center justify-center">
               {imageUrl ? (
                  <Image
                     src={imageUrl}
                     alt={row.original.code}
                     width={48}
                     height={48}
                     className="rounded-md object-cover"
                  />
               ) : (
                  <div className="flex h-12 w-12 items-center justify-center rounded-md bg-gray-100">
                     <Package className="h-6 w-6 text-gray-400" />
                  </div>
               )}
            </div>
         );
      },
   },
   {
      accessorKey: 'code',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               SKU Code
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => (
         <div className="font-mono text-sm font-medium">
            {row.getValue('code')}
         </div>
      ),
   },
   {
      accessorKey: 'model.name',
      header: 'Model',
      cell: ({ row }) => (
         <div className="font-medium">
            {row.original.model?.name?.replace(/_/g, ' ') || '-'}
         </div>
      ),
   },
   {
      accessorKey: 'color.name',
      header: 'Color',
      cell: ({ row }) => {
         const colorName = row.original.color?.name || 'N/A';
         const hexCode = row.original.color?.hex_code;
         return (
            <div className="flex items-center gap-2">
               {hexCode && (
                  <div
                     className="h-6 w-6 rounded-full border-2 border-gray-300"
                     style={{ backgroundColor: hexCode }}
                  />
               )}
               <span className="capitalize">{colorName.toLowerCase()}</span>
            </div>
         );
      },
   },
   {
      accessorKey: 'storage.name',
      header: 'Storage',
      cell: ({ row }) => (
         <Badge variant="outline" className="font-semibold">
            {row.original.storage?.name || '-'}
         </Badge>
      ),
   },
   {
      accessorKey: 'unit_price',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Unit Price
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => {
         const price = parseFloat(row.getValue('unit_price'));
         const formatted = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
         }).format(price);
         return <div className="font-medium">{formatted}</div>;
      },
   },
   {
      accessorKey: 'available_in_stock',
      header: 'In Stock',
      cell: ({ row }) => {
         const stock = row.getValue('available_in_stock') as number;
         return (
            <div
               className={cn(
                  'text-center font-semibold',
                  stock === 0
                     ? 'text-red-600'
                     : stock < 10
                       ? 'text-yellow-600'
                       : 'text-green-600',
               )}
            >
               {stock}
            </div>
         );
      },
   },
   {
      accessorKey: 'total_sold',
      header: 'Total Sold',
      cell: ({ row }) => (
         <div className="text-center">{row.getValue('total_sold')}</div>
      ),
   },
   {
      id: 'reserved_quantity',
      header: 'Reserved',
      cell: ({ row }) => {
         const reservedEvent = row.original.reserved_for_event;
         if (!reservedEvent) {
            return <div className="text-center text-gray-400">-</div>;
         }
         return (
            <div className="text-center">
               <Badge
                  variant="outline"
                  className="bg-orange-100 text-orange-800 border-orange-300"
               >
                  {reservedEvent.reserved_quantity}
               </Badge>
            </div>
         );
      },
   },
   {
      accessorKey: 'state',
      header: 'Status',
      cell: ({ row }) => {
         const state = row.getValue('state') as string;
         return (
            <Badge
               variant="outline"
               className={cn('capitalize', getStateStyle(state))}
            >
               {state.toLowerCase().replace(/_/g, ' ')}
            </Badge>
         );
      },
   },
   {
      id: 'actions',
      enableHiding: false,
      cell: ({ row }) => {
         const sku = row.original;

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
                     onClick={() => navigator.clipboard.writeText(sku.id)}
                  >
                     Copy SKU ID
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem>View details</DropdownMenuItem>
                  <DropdownMenuItem>Edit SKU</DropdownMenuItem>
                  <DropdownMenuItem>Adjust stock</DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem className="text-destructive">
                     Delete SKU
                  </DropdownMenuItem>
               </DropdownMenuContent>
            </DropdownMenu>
         );
      },
   },
];

const SkusInTenantPage = () => {
   const { getWarehousesAsync, getWarehousesState, isLoading } =
      useInventoryService();

   const { filters, setFilters } = useFilters<TSkuFilter>({
      _page: 'number',
      _limit: 'number',
      _colors: { array: 'string' },
      _storages: { array: 'string' },
      _models: { array: 'string' },
   });

   const [sorting, setSorting] = useState<SortingState>([]);
   const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
      {},
   );
   const [rowSelection, setRowSelection] = useState<RowSelectionState>({});

   // Get warehouse data from state or use empty array
   const warehouseData = useMemo(() => {
      return getWarehousesState.data?.items || [];
   }, [getWarehousesState.data]);

   const {
      currentPage,
      totalPages,
      pageSize,
      totalRecords,
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,
      getPageNumbers,
   } = usePagination(
      getWarehousesState.data && getWarehousesState.data.items.length > 0
         ? getWarehousesState.data
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
      data: warehouseData,
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
      const fetchWarehouses = async () => {
         await getWarehousesAsync(filters);
      };

      fetchWarehouses();
   }, [filters, getWarehousesAsync]);

   return (
      <div className="p-5">
         <div className="flex items-center justify-between">
            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Warehouse Management
               </h1>
               <p className="text-muted-foreground">
                  Manage inventory and SKU items across all warehouses
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
                                 No SKU items found.
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
                              {warehouseData.length > 0
                                 ? (currentPage - 1) * pageSize + 1
                                 : 0}
                           </span>{' '}
                           to{' '}
                           <span className="font-medium">
                              {Math.min(currentPage * pageSize, totalRecords)}
                           </span>{' '}
                           of{' '}
                           <span className="font-medium">{totalRecords}</span>{' '}
                           items
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

export default SkusInTenantPage;
