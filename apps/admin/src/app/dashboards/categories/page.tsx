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
import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuLabel,
   DropdownMenuSeparator,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import { ArrowUpDown, MoreHorizontal } from 'lucide-react';
import { LoadingOverlay } from '@components/loading-overlay';
import { cn } from '~/src/infrastructure/lib/utils';
import { useEffect, useState } from 'react';
import { useCategoryService } from '~/src/hooks/api/use-category-service';

type TCategoryRow = {
   id: string;
   name: string;
   parentCategory: string | null;
   subCategories: number;
   productModels: number;
   slug: string;
   order: number;
};

const columns: ColumnDef<TCategoryRow>[] = [
   {
      accessorKey: 'name',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Name
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => (
         <div className="font-medium">{row.getValue('name')}</div>
      ),
   },
   {
      accessorKey: 'parentCategory',
      header: 'Parent',
      cell: ({ row }) => (
         <div>{(row.getValue('parentCategory') as string) ?? '-'}</div>
      ),
   },
   {
      accessorKey: 'subCategories',
      header: 'Sub Categories',
      cell: ({ row }) => (
         <div>{(row.getValue('subCategories') as number) ?? 0}</div>
      ),
   },
   {
      accessorKey: 'productModels',
      header: 'Product Models',
      cell: ({ row }) => (
         <div>{(row.getValue('productModels') as number) ?? 0}</div>
      ),
   },
   {
      accessorKey: 'order',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Order
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => <div>{row.getValue('order') as number}</div>,
   },
   {
      accessorKey: 'slug',
      header: 'Slug',
      cell: ({ row }) => (
         <div className="text-muted-foreground">{row.getValue('slug')}</div>
      ),
   },
   {
      id: 'actions',
      enableHiding: false,
      cell: ({ row }) => {
         const item = row.original;
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
                     onClick={() => navigator.clipboard.writeText(item.id)}
                  >
                     Copy Category ID
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem>View details</DropdownMenuItem>
                  <DropdownMenuItem>Edit</DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem className="text-destructive">
                     Delete
                  </DropdownMenuItem>
               </DropdownMenuContent>
            </DropdownMenu>
         );
      },
   },
];

const CategoriesPage = () => {
   const { isLoading, getCategoriesAsync } = useCategoryService();
   const [data, setData] = useState<TCategoryRow[]>([]);
   const [error, setError] = useState<string | null>(null);

   const [sorting, setSorting] = useState<SortingState>([]);
   const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
      {},
   );
   const [rowSelection, setRowSelection] = useState<RowSelectionState>({});

   useEffect(() => {
      const fetchCategories = async () => {
         const res = await getCategoriesAsync();
         if (res.isSuccess && res.data) {
            const mapped = (res.data as any[]).map((c) => ({
               id: c.id,
               name: c.name,
               parentCategory: c.parent_category
                  ? c.parent_category.name
                  : null,
               subCategories: Array.isArray(c.sub_categories)
                  ? c.sub_categories.length
                  : 0,
               productModels: Array.isArray(c.product_models)
                  ? c.product_models.length
                  : 0,
               slug: c.slug,
               order: c.order,
            })) as TCategoryRow[];
            setData(mapped);
            setError(null);
         } else {
            setError('Failed to load categories');
         }
      };
      fetchCategories();
   }, [getCategoriesAsync]);

   const table = useReactTable({
      data,
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
   });

   return (
      <div className="p-5">
         <div className="flex items-center justify-between">
            <div>
               <h1 className="text-3xl font-bold tracking-tight">Categories</h1>
               <p className="text-muted-foreground">
                  Manage product categories
               </p>
            </div>
         </div>
         {error && <div className="my-3 text-sm text-red-600">{error}</div>}
         <LoadingOverlay isLoading={isLoading}>
            <div className="rounded-lg border bg-card">
               <div className="overflow-auto">
                  <Table>
                     <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                           <TableRow key={headerGroup.id}>
                              {headerGroup.headers.map((header) => (
                                 <TableHead key={header.id}>
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
                           table.getRowModel().rows.map((row, index) => (
                              <TableRow
                                 key={row.id}
                                 data-state={row.getIsSelected() && 'selected'}
                                 className={cn(
                                    'cursor-pointer transition-colors',
                                    row.getIsSelected()
                                       ? '!bg-blue-400/20 hover:bg-blue-200'
                                       : index % 2 === 0
                                         ? 'bg-white hover:bg-slate-300/50'
                                         : 'bg-slate-300/30 hover:bg-slate-300/50',
                                 )}
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
                                 {isLoading
                                    ? 'Loading...'
                                    : 'No categories found.'}
                              </TableCell>
                           </TableRow>
                        )}
                     </TableBody>
                  </Table>
               </div>
            </div>
         </LoadingOverlay>
      </div>
   );
};

export default CategoriesPage;
