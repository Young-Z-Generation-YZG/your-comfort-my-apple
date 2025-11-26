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
   Search,
   X,
} from 'lucide-react';
import { cn } from '~/src/infrastructure/lib/utils';
import { LoadingOverlay } from '@components/loading-overlay';
import useInventoryService from '~/src/hooks/api/use-inventory-service';
import { useEffect, useMemo, useRef, useState } from 'react';
import { Input } from '@components/ui/input';
import usePaginationV2 from '~/src/hooks/use-pagination-v2';
import Image from 'next/image';
import useFilters from '~/src/hooks/use-filter';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { TSku } from '~/src/domain/types/catalog';
import { Cylinder } from 'lucide-react';

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

// Available filter options
const models = [
   {
      name: 'iPhone 15',
      normalized_name: 'IPHONE_15',
      icon: Cylinder,
      order: 0,
   },
   {
      name: 'iPhone 15 Plus',
      normalized_name: 'IPHONE_15_PLUS',
      icon: Cylinder,
      order: 1,
   },
   {
      name: 'iPhone 16',
      normalized_name: 'IPHONE_16',
      icon: Cylinder,
      order: 2,
   },
   {
      name: 'iPhone 16 Plus',
      normalized_name: 'IPHONE_16_PLUS',
      icon: Cylinder,
      order: 3,
   },
   {
      name: 'iPhone 16e',
      normalized_name: 'IPHONE_16E',
      icon: Cylinder,
      order: 5,
   },
   {
      name: 'iPhone 17',
      normalized_name: 'IPHONE_17',
      icon: Cylinder,
      order: 6,
   },
   {
      name: 'iPhone 17 Pro',
      normalized_name: 'IPHONE_17_PRO',
      icon: Cylinder,
      order: 7,
   },
   {
      name: 'iPhone 17 Pro Max',
      normalized_name: 'IPHONE_17_PRO_MAX',
      icon: Cylinder,
      order: 8,
   },
   {
      name: 'iPhone 17 Air',
      normalized_name: 'IPHONE_17_AIR',
      icon: Cylinder,
      order: 9,
   },
];

const colors = [
   {
      name: 'ultramarine',
      normalized_name: 'ULTRAMARINE',
      hex: '#9AADF6',
      icon: Cylinder,
      order: 0,
   },
   {
      name: 'green',
      normalized_name: 'GREEN',
      hex: '#D0D9CA',
      icon: Cylinder,
      order: 1,
   },
   {
      name: 'teal',
      normalized_name: 'TEAL',
      hex: '#B0D4D2',
      icon: Cylinder,
      order: 2,
   },
   {
      name: 'blue',
      normalized_name: 'BLUE',
      hex: '#D5DDDF',
      icon: Cylinder,
      order: 3,
   },
   {
      name: 'yellow',
      normalized_name: 'YELLOW',
      hex: '#EDE6C8',
      icon: Cylinder,
      order: 4,
   },
   {
      name: 'pink',
      normalized_name: 'PINK',
      hex: '#F2ADDA',
      icon: Cylinder,
      order: 5,
   },
   {
      name: 'white',
      normalized_name: 'WHITE',
      hex: '#FAFAFA',
      icon: Cylinder,
      order: 6,
   },
   {
      name: 'black',
      normalized_name: 'BLACK',
      hex: '#3C4042',
      icon: Cylinder,
      order: 7,
   },
   {
      name: 'Lavender',
      normalized_name: 'LAVENDER',
      hex: '#E7D9F2',
      icon: Cylinder,
      order: 8,
   },
   {
      name: 'Sage',
      normalized_name: 'SAGE',
      hex: '#BBC89E',
      icon: Cylinder,
      order: 9,
   },
   {
      name: 'Mist Blue',
      normalized_name: 'MIST_BLUE',
      hex: '#A7BDDE',
      icon: Cylinder,
      order: 10,
   },
   {
      name: 'Silver',
      normalized_name: 'SILVER',
      hex: '#E6E6E6',
      icon: Cylinder,
      order: 11,
   },
   {
      name: 'Cosmic Orange',
      normalized_name: 'COSMIC_ORANGE',
      hex: '#F67E36',
      icon: Cylinder,
      order: 12,
   },
   {
      name: 'Deep Blue',
      normalized_name: 'DEEP_BLUE',
      hex: '#3F4C77',
      icon: Cylinder,
      order: 13,
   },
   {
      name: 'Sky Blue',
      normalized_name: 'SKY_BLUE',
      hex: '#E5F2FA',
      icon: Cylinder,
      order: 14,
   },
   {
      name: 'Light Gold',
      normalized_name: 'LIGHT_GOLD',
      hex: '#FAF3E6',
      icon: Cylinder,
      order: 15,
   },
   {
      name: 'Cloud White',
      normalized_name: 'CLOUD_WHITE',
      hex: '#FCFCFC',
      icon: Cylinder,
      order: 16,
   },
   {
      name: 'Space Black',
      normalized_name: 'SPACE_BLACK',
      hex: '#121212',
      icon: Cylinder,
      order: 17,
   },
];

const storages = [
   {
      name: '128GB',
      normalized_name: '128GB',
      icon: Cylinder,
      order: 0,
   },
   {
      name: '256GB',
      normalized_name: '256GB',
      icon: Cylinder,
      order: 1,
   },
   {
      name: '512GB',
      normalized_name: '512GB',
      icon: Cylinder,
      order: 2,
   },
   {
      name: '1TB',
      normalized_name: '1TB',
      icon: Cylinder,
      order: 3,
   },
] as const;

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

const WarehousesPage = () => {
   const { getWarehousesAsync, getWarehousesState, isLoading } =
      useInventoryService();

   const { tenantId } = useAppSelector((state) => state.tenant);

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

   // Search states for filter dropdowns
   const [modelSearchQuery, setModelSearchQuery] = useState('');
   const [colorSearchQuery, setColorSearchQuery] = useState('');
   const [storageSearchQuery, setStorageSearchQuery] = useState('');

   // Filtered results
   const filteredModels = useMemo(() => {
      if (!modelSearchQuery) return models;
      return models.filter((model) =>
         model.name.toLowerCase().includes(modelSearchQuery.toLowerCase()),
      );
   }, [modelSearchQuery]);

   const filteredColors = useMemo(() => {
      if (!colorSearchQuery) return colors;
      return colors.filter((color) =>
         color.name.toLowerCase().includes(colorSearchQuery.toLowerCase()),
      );
   }, [colorSearchQuery]);

   const filteredStorages = useMemo(() => {
      if (!storageSearchQuery) return storages;
      return storages.filter((storage) =>
         storage.name.toLowerCase().includes(storageSearchQuery.toLowerCase()),
      );
   }, [storageSearchQuery]);

   const {
      currentPage,
      totalPages,
      totalRecords,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
      getPaginationItems,
   } = usePaginationV2(
      getWarehousesState.data ?? {
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

   const table = useReactTable({
      data: getWarehousesState.data?.items ?? [],
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
         _models: filters._models ?? undefined,
         _colors: filters._colors ?? undefined,
         _storages: filters._storages ?? undefined,
      });

      // Only call API if params actually changed
      if (prevApiParamsRef.current !== apiParams) {
         prevApiParamsRef.current = apiParams;
      }
      getWarehousesAsync(filters);
   }, [filters, tenantId, getWarehousesAsync]);

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
            {/* Filter section */}
            <div className="rounded-lg border bg-card shadow-sm mb-4">
               <div className="p-6 space-y-4">
                  {/* Filter Dropdowns Row */}
                  <div className="flex items-center gap-3">
                     <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                           <Button variant="outline" className="h-10 gap-2">
                              <Cylinder className="h-4 w-4" />
                              <span className="font-medium">Model</span>
                              <div className="flex items-center gap-2">
                                 {(() => {
                                    const selectedModels =
                                       filters._models ?? [];
                                    const modelCount = selectedModels.length;

                                    if (modelCount === 0) {
                                       return null;
                                    }

                                    if (modelCount > 2) {
                                       return (
                                          <>
                                             {selectedModels
                                                .slice(0, 2)
                                                .map((modelNormalizedName) => {
                                                   const model = models.find(
                                                      (m) =>
                                                         m.normalized_name ===
                                                         modelNormalizedName,
                                                   );
                                                   return (
                                                      <Badge
                                                         key={
                                                            modelNormalizedName
                                                         }
                                                         variant="outline"
                                                         className="bg-gray-100 text-gray-800 border-gray-300"
                                                      >
                                                         {model?.name ??
                                                            modelNormalizedName.replace(
                                                               /_/g,
                                                               ' ',
                                                            )}
                                                      </Badge>
                                                   );
                                                })}
                                             <Badge
                                                variant="outline"
                                                className="bg-gray-100 text-gray-800 border-gray-300"
                                             >
                                                +{modelCount - 2}
                                             </Badge>
                                          </>
                                       );
                                    }

                                    return selectedModels.map(
                                       (modelNormalizedName) => {
                                          const model = models.find(
                                             (m) =>
                                                m.normalized_name ===
                                                modelNormalizedName,
                                          );
                                          return (
                                             <Badge
                                                key={modelNormalizedName}
                                                variant="outline"
                                                className="bg-gray-100 text-gray-800 border-gray-300"
                                             >
                                                {model?.name ??
                                                   modelNormalizedName.replace(
                                                      /_/g,
                                                      ' ',
                                                   )}
                                             </Badge>
                                          );
                                       },
                                    );
                                 })()}
                                 <ChevronDown />
                              </div>
                           </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent
                           align="start"
                           side="bottom"
                           sideOffset={4}
                           className="w-56 p-0"
                        >
                           <div
                              className="flex items-center border-b px-3 py-2"
                              onKeyDown={(e) => e.stopPropagation()}
                           >
                              <Search className="mr-2 h-4 w-4 shrink-0 opacity-50" />
                              <Input
                                 placeholder="Search model..."
                                 value={modelSearchQuery}
                                 onChange={(e) =>
                                    setModelSearchQuery(e.target.value)
                                 }
                                 className="h-8 border-0 focus-visible:ring-0 focus-visible:ring-offset-0"
                                 autoFocus
                              />
                           </div>
                           <div className="max-h-[300px] overflow-y-auto p-1">
                              {filteredModels.length === 0 ? (
                                 <div className="py-6 text-center text-sm text-muted-foreground">
                                    No model found.
                                 </div>
                              ) : (
                                 <>
                                    {filteredModels.map((model) => {
                                       const ModelIcon = model.icon;
                                       const isChecked =
                                          filters._models?.includes(
                                             model.normalized_name,
                                          ) ?? false;

                                       return (
                                          <DropdownMenuCheckboxItem
                                             key={model.normalized_name}
                                             onSelect={(e) =>
                                                e.preventDefault()
                                             }
                                             checked={isChecked}
                                             onCheckedChange={() => {
                                                setFilters((prev) => {
                                                   const currentModels =
                                                      prev._models ?? [];
                                                   const isModelSelected =
                                                      currentModels.includes(
                                                         model.normalized_name,
                                                      );

                                                   return {
                                                      ...prev,
                                                      _models: isModelSelected
                                                         ? currentModels.filter(
                                                              (m) =>
                                                                 m !==
                                                                 model.normalized_name,
                                                           )
                                                         : [
                                                              ...currentModels,
                                                              model.normalized_name,
                                                           ],
                                                   };
                                                });
                                             }}
                                             className="flex items-center gap-2"
                                          >
                                             <ModelIcon className="h-4 w-4" />
                                             {model.name}
                                          </DropdownMenuCheckboxItem>
                                       );
                                    })}
                                 </>
                              )}
                           </div>
                           <div className="border-t p-2">
                              <Button
                                 variant="outline"
                                 size="sm"
                                 className="w-full"
                                 onClick={(e) => {
                                    e.stopPropagation();
                                    setFilters((prev) => ({
                                       ...prev,
                                       _models: [],
                                    }));
                                    setModelSearchQuery('');
                                 }}
                                 disabled={(filters._models?.length ?? 0) === 0}
                              >
                                 Clear All
                              </Button>
                           </div>
                        </DropdownMenuContent>
                     </DropdownMenu>

                     <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                           <Button variant="outline" className="h-10 gap-2">
                              <Cylinder className="h-4 w-4" />
                              <span className="font-medium">Color</span>
                              <div className="flex items-center gap-2">
                                 {(() => {
                                    const selectedColors =
                                       filters._colors ?? [];
                                    const colorCount = selectedColors.length;

                                    if (colorCount === 0) {
                                       return null;
                                    }

                                    if (colorCount > 2) {
                                       return (
                                          <>
                                             {selectedColors
                                                .slice(0, 2)
                                                .map((colorNormalizedName) => {
                                                   const color = colors.find(
                                                      (c) =>
                                                         c.normalized_name ===
                                                         colorNormalizedName,
                                                   );
                                                   return (
                                                      <Badge
                                                         key={
                                                            colorNormalizedName
                                                         }
                                                         variant="outline"
                                                         className="bg-gray-100 text-gray-800 border-gray-300"
                                                      >
                                                         {color?.name ??
                                                            colorNormalizedName}
                                                      </Badge>
                                                   );
                                                })}
                                             <Badge
                                                variant="outline"
                                                className="bg-gray-100 text-gray-800 border-gray-300"
                                             >
                                                +{colorCount - 2}
                                             </Badge>
                                          </>
                                       );
                                    }

                                    return selectedColors.map(
                                       (colorNormalizedName) => {
                                          const color = colors.find(
                                             (c) =>
                                                c.normalized_name ===
                                                colorNormalizedName,
                                          );
                                          return (
                                             <Badge
                                                key={colorNormalizedName}
                                                variant="outline"
                                                className="bg-gray-100 text-gray-800 border-gray-300"
                                             >
                                                {color?.name ??
                                                   colorNormalizedName}
                                             </Badge>
                                          );
                                       },
                                    );
                                 })()}
                                 <ChevronDown />
                              </div>
                           </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent
                           align="start"
                           side="bottom"
                           sideOffset={4}
                           className="w-56 p-0"
                        >
                           <div
                              className="flex items-center border-b px-3 py-2"
                              onKeyDown={(e) => e.stopPropagation()}
                           >
                              <Search className="mr-2 h-4 w-4 shrink-0 opacity-50" />
                              <Input
                                 placeholder="Search color..."
                                 value={colorSearchQuery}
                                 onChange={(e) =>
                                    setColorSearchQuery(e.target.value)
                                 }
                                 className="h-8 border-0 focus-visible:ring-0 focus-visible:ring-offset-0"
                                 autoFocus
                              />
                           </div>
                           <div className="max-h-[300px] overflow-y-auto p-1">
                              {filteredColors.length === 0 ? (
                                 <div className="py-6 text-center text-sm text-muted-foreground">
                                    No color found.
                                 </div>
                              ) : (
                                 <>
                                    {filteredColors.map((color) => {
                                       const ColorIcon = color.icon;
                                       const isChecked =
                                          filters._colors?.includes(
                                             color.normalized_name,
                                          ) ?? false;

                                       return (
                                          <DropdownMenuCheckboxItem
                                             key={color.normalized_name}
                                             onSelect={(e) =>
                                                e.preventDefault()
                                             }
                                             checked={isChecked}
                                             onCheckedChange={() => {
                                                setFilters((prev) => {
                                                   const currentColors =
                                                      prev._colors ?? [];
                                                   const isColorSelected =
                                                      currentColors.includes(
                                                         color.normalized_name,
                                                      );

                                                   return {
                                                      ...prev,
                                                      _colors: isColorSelected
                                                         ? currentColors.filter(
                                                              (c) =>
                                                                 c !==
                                                                 color.normalized_name,
                                                           )
                                                         : [
                                                              ...currentColors,
                                                              color.normalized_name,
                                                           ],
                                                   };
                                                });
                                             }}
                                             className="flex items-center gap-2"
                                          >
                                             <div className="flex items-center gap-2">
                                                {color.hex && (
                                                   <div
                                                      className="h-4 w-4 rounded-full border border-gray-300"
                                                      style={{
                                                         backgroundColor:
                                                            color.hex,
                                                      }}
                                                   />
                                                )}
                                                <ColorIcon className="h-4 w-4" />
                                             </div>
                                             <span className="capitalize">
                                                {color.name}
                                             </span>
                                          </DropdownMenuCheckboxItem>
                                       );
                                    })}
                                 </>
                              )}
                           </div>
                           <div className="border-t p-2">
                              <Button
                                 variant="outline"
                                 size="sm"
                                 className="w-full"
                                 onClick={(e) => {
                                    e.stopPropagation();
                                    setFilters((prev) => ({
                                       ...prev,
                                       _colors: [],
                                    }));
                                    setColorSearchQuery('');
                                 }}
                                 disabled={(filters._colors?.length ?? 0) === 0}
                              >
                                 Clear All
                              </Button>
                           </div>
                        </DropdownMenuContent>
                     </DropdownMenu>

                     <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                           <Button variant="outline" className="h-10 gap-2">
                              <Cylinder className="h-4 w-4" />
                              <span className="font-medium">Storage</span>
                              <div className="flex items-center gap-2">
                                 {(() => {
                                    const selectedStorages =
                                       filters._storages ?? [];
                                    const storageCount =
                                       selectedStorages.length;

                                    if (storageCount === 0) {
                                       return null;
                                    }

                                    if (storageCount > 2) {
                                       return (
                                          <>
                                             {selectedStorages
                                                .slice(0, 2)
                                                .map(
                                                   (storageNormalizedName) => {
                                                      const storage =
                                                         storages.find(
                                                            (s) =>
                                                               s.normalized_name ===
                                                               storageNormalizedName,
                                                         );
                                                      return (
                                                         <Badge
                                                            key={
                                                               storageNormalizedName
                                                            }
                                                            variant="outline"
                                                            className="bg-gray-100 text-gray-800 border-gray-300"
                                                         >
                                                            {storage?.name ??
                                                               storageNormalizedName}
                                                         </Badge>
                                                      );
                                                   },
                                                )}
                                             <Badge
                                                variant="outline"
                                                className="bg-gray-100 text-gray-800 border-gray-300"
                                             >
                                                +{storageCount - 2}
                                             </Badge>
                                          </>
                                       );
                                    }

                                    return selectedStorages.map(
                                       (storageNormalizedName) => {
                                          const storage = storages.find(
                                             (s) =>
                                                s.normalized_name ===
                                                storageNormalizedName,
                                          );
                                          return (
                                             <Badge
                                                key={storageNormalizedName}
                                                variant="outline"
                                                className="bg-gray-100 text-gray-800 border-gray-300"
                                             >
                                                {storage?.name ??
                                                   storageNormalizedName}
                                             </Badge>
                                          );
                                       },
                                    );
                                 })()}
                                 <ChevronDown />
                              </div>
                           </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent
                           align="start"
                           side="bottom"
                           sideOffset={4}
                           className="w-56 p-0"
                        >
                           <div
                              className="flex items-center border-b px-3 py-2"
                              onKeyDown={(e) => e.stopPropagation()}
                           >
                              <Search className="mr-2 h-4 w-4 shrink-0 opacity-50" />
                              <Input
                                 placeholder="Search storage..."
                                 value={storageSearchQuery}
                                 onChange={(e) =>
                                    setStorageSearchQuery(e.target.value)
                                 }
                                 className="h-8 border-0 focus-visible:ring-0 focus-visible:ring-offset-0"
                                 autoFocus
                              />
                           </div>
                           <div className="max-h-[300px] overflow-y-auto p-1">
                              {filteredStorages.length === 0 ? (
                                 <div className="py-6 text-center text-sm text-muted-foreground">
                                    No storage found.
                                 </div>
                              ) : (
                                 <>
                                    {filteredStorages.map((storage) => {
                                       const StorageIcon = storage.icon;
                                       const isChecked =
                                          filters._storages?.includes(
                                             storage.normalized_name,
                                          ) ?? false;

                                       return (
                                          <DropdownMenuCheckboxItem
                                             key={storage.normalized_name}
                                             onSelect={(e) =>
                                                e.preventDefault()
                                             }
                                             checked={isChecked}
                                             onCheckedChange={() => {
                                                setFilters((prev) => {
                                                   const currentStorages =
                                                      prev._storages ?? [];
                                                   const isStorageSelected =
                                                      currentStorages.includes(
                                                         storage.normalized_name,
                                                      );

                                                   return {
                                                      ...prev,
                                                      _storages:
                                                         isStorageSelected
                                                            ? currentStorages.filter(
                                                                 (s) =>
                                                                    s !==
                                                                    storage.normalized_name,
                                                              )
                                                            : [
                                                                 ...currentStorages,
                                                                 storage.normalized_name,
                                                              ],
                                                   };
                                                });
                                             }}
                                             className="flex items-center gap-2"
                                          >
                                             <StorageIcon className="h-4 w-4" />
                                             {storage.name}
                                          </DropdownMenuCheckboxItem>
                                       );
                                    })}
                                 </>
                              )}
                           </div>
                           <div className="border-t p-2">
                              <Button
                                 variant="outline"
                                 size="sm"
                                 className="w-full"
                                 onClick={(e) => {
                                    e.stopPropagation();
                                    setFilters((prev) => ({
                                       ...prev,
                                       _storages: [],
                                    }));
                                    setStorageSearchQuery('');
                                 }}
                                 disabled={
                                    (filters._storages?.length ?? 0) === 0
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
                              _models: [],
                              _colors: [],
                              _storages: [],
                              _page: 1,
                           });
                        }}
                        className={cn(
                           'h-10 px-4 gap-2 whitespace-nowrap',
                           ((filters._models?.length ?? 0) > 0 ||
                              (filters._colors?.length ?? 0) > 0 ||
                              (filters._storages?.length ?? 0) > 0) &&
                              'border-[#ef4444] text-[#ef4444] bg-[#ef4444]/10 hover:bg-[#ef4444]/20',
                        )}
                        disabled={
                           (filters._models?.length ?? 0) === 0 &&
                           (filters._colors?.length ?? 0) === 0 &&
                           (filters._storages?.length ?? 0) === 0
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
                                 No SKU items found.
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
                           items
                        </div>
                     </div>

                     {getWarehousesState.data &&
                        getWarehousesState.data.total_pages > 0 && (
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

export default WarehousesPage;
