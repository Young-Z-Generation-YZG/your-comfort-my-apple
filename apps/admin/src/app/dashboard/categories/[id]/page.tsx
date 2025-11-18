'use client';

import { useEffect, useMemo, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useCategoryService } from '~/src/hooks/api/use-category-service';
import { LoadingOverlay } from '@components/loading-overlay';
import { Button } from '@components/ui/button';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import {
   ColumnDef,
   flexRender,
   getCoreRowModel,
   getSortedRowModel,
   SortingState,
   useReactTable,
} from '@tanstack/react-table';
import { ArrowUpDown } from 'lucide-react';
import { cn } from '~/src/infrastructure/lib/utils';
import { Badge } from '@components/ui/badge';
import { ProductClassification } from '~/src/domain/enums/product-classification.enum';

type TCategoryDetails = {
   id: string;
   name: string;
   slug: string;
   order: number;
   parent_category: { id: string; name: string } | null;
   sub_categories: Array<{ id: string; name: string; slug: string }> | null;
   product_models: Array<TCategoryModel> | null;
};

type TCategoryModel = {
   id: string;
   name: string;
   slug: string;
   showcase_images?: Array<{ image_url: string }>;
   product_classification?: ProductClassification | string;
};
const CategoryDetailPage = () => {
   const params = useParams<{ id: string }>();
   const router = useRouter();
   const { isLoading, getCategoryDetailsAsync } = useCategoryService();
   const [category, setCategory] = useState<TCategoryDetails | null>(null);
   const [error, setError] = useState<string | null>(null);

   useEffect(() => {
      const fetchDetails = async () => {
         if (!params?.id) return;
         const res = await getCategoryDetailsAsync(params.id as string);
         if (res.isSuccess && res.data) {
            setCategory(res.data as TCategoryDetails);
            setError(null);
         } else {
            setError('Failed to load category details');
         }
      };
      fetchDetails();
   }, [params?.id, getCategoryDetailsAsync]);

   const meta = useMemo(() => {
      if (!category) return [];
      return [
         { label: 'Name', value: category.name },
         { label: 'Slug', value: category.slug },
         { label: 'Order', value: category.order },
         { label: 'Parent', value: category.parent_category?.name ?? '-' },
         {
            label: 'Sub Categories',
            value: category.sub_categories?.length ?? 0,
         },
         {
            label: 'Product Models',
            value: category.product_models?.length ?? 0,
         },
      ];
   }, [category]);

   // Product models data table definitions
   type TProductModelRow = {
      id: string;
      name: string;
      imageUrl: string | null;
      productClassification: ProductClassification | string | null;
      slug: string;
   };
   const productModelColumns: ColumnDef<TProductModelRow>[] = [
      {
         accessorKey: 'name',
         header: ({ column }) => (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Name
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         ),
         cell: ({ row }) => (
            <div className="font-medium">{row.getValue('name')}</div>
         ),
      },
      {
         accessorKey: 'imageUrl',
         header: 'Image',
         cell: ({ row }) => {
            const src = (row.getValue('imageUrl') as string) || '';
            return src ? (
               // eslint-disable-next-line @next/next/no-img-element
               <img
                  src={src}
                  alt={row.original.name}
                  className="h-10 w-10 rounded-md object-cover"
               />
            ) : (
               <div className="h-10 w-10 rounded-md bg-gray-100" />
            );
         },
      },
      {
         accessorKey: 'productClassification',
         header: 'Classification',
         cell: ({ row }) => {
            const cls =
               (row.getValue('productClassification') as string) || '-';
            return (
               <Badge variant="outline" className="uppercase">
                  {cls}
               </Badge>
            );
         },
      },
      {
         accessorKey: 'slug',
         header: 'Slug',
         cell: ({ row }) => (
            <div className="text-muted-foreground">{row.getValue('slug')}</div>
         ),
      },
   ];
   const productModelsData: TProductModelRow[] = useMemo(() => {
      return (category?.product_models || []).map((pm) => ({
         id: pm.id,
         name: pm.name,
         imageUrl: pm.showcase_images?.[0]?.image_url ?? null,
         productClassification: pm.product_classification ?? null,
         slug: pm.slug,
      }));
   }, [category?.product_models]);
   const [pmSorting, setPmSorting] = useState<SortingState>([]);
   const productModelsTable = useReactTable({
      data: productModelsData,
      columns: productModelColumns,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      onSortingChange: setPmSorting,
      state: { sorting: pmSorting },
   });

   return (
      <div className="p-5">
         <div className="mb-4 flex items-center justify-between">
            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Category details
               </h1>
               <p className="text-muted-foreground">
                  View category metadata and relations
               </p>
            </div>
            <div className="flex gap-2">
               <Button variant="outline" onClick={() => router.back()}>
                  Back
               </Button>
            </div>
         </div>

         {error && <div className="mb-3 text-sm text-red-600">{error}</div>}

         <LoadingOverlay isLoading={isLoading}>
            {!category ? (
               <div className="rounded-md border bg-card p-6 text-sm text-muted-foreground">
                  {isLoading ? 'Loading...' : 'No category found.'}
               </div>
            ) : (
               <div className="space-y-8">
                  <div className="rounded-lg border bg-card">
                     <div className="border-b p-4">
                        <h2 className="text-lg font-semibold">Metadata</h2>
                     </div>
                     <div className="grid grid-cols-1 gap-4 p-4 md:grid-cols-2 lg:grid-cols-3">
                        {meta.map((m) => (
                           <div key={m.label} className="space-y-1">
                              <div className="text-xs uppercase text-muted-foreground">
                                 {m.label}
                              </div>
                              <div className="text-sm font-medium">
                                 {String(m.value)}
                              </div>
                           </div>
                        ))}
                     </div>
                  </div>

                  <div className="rounded-lg border bg-card">
                     <div className="border-b p-4">
                        <h2 className="text-lg font-semibold">
                           Sub Categories
                        </h2>
                     </div>
                     <div className="overflow-auto">
                        <Table>
                           <TableHeader>
                              <TableRow>
                                 <TableHead>Name</TableHead>
                                 <TableHead>Slug</TableHead>
                              </TableRow>
                           </TableHeader>
                           <TableBody>
                              {category.sub_categories?.length ? (
                                 category.sub_categories.map((sc) => (
                                    <TableRow key={sc.id}>
                                       <TableCell className="font-medium">
                                          {sc.name}
                                       </TableCell>
                                       <TableCell className="text-muted-foreground">
                                          {sc.slug}
                                       </TableCell>
                                    </TableRow>
                                 ))
                              ) : (
                                 <TableRow>
                                    <TableCell
                                       colSpan={2}
                                       className="h-20 text-center"
                                    >
                                       No sub categories
                                    </TableCell>
                                 </TableRow>
                              )}
                           </TableBody>
                        </Table>
                     </div>
                  </div>

                  <div className="rounded-lg border bg-card">
                     <div className="border-b p-4">
                        <h2 className="text-lg font-semibold">
                           Product Models
                        </h2>
                     </div>
                     <div className="overflow-auto">
                        <Table>
                           <TableHeader>
                              {productModelsTable
                                 .getHeaderGroups()
                                 .map((hg) => (
                                    <TableRow key={hg.id}>
                                       {hg.headers.map((header) => (
                                          <TableHead key={header.id}>
                                             {header.isPlaceholder
                                                ? null
                                                : flexRender(
                                                     header.column.columnDef
                                                        .header,
                                                     header.getContext(),
                                                  )}
                                          </TableHead>
                                       ))}
                                    </TableRow>
                                 ))}
                           </TableHeader>
                           <TableBody>
                              {productModelsTable.getRowModel().rows?.length ? (
                                 productModelsTable
                                    .getRowModel()
                                    .rows.map((row, idx) => (
                                       <TableRow
                                          key={row.id}
                                          className={cn(
                                             'transition-colors',
                                             idx % 2 === 0
                                                ? 'bg-white hover:bg-slate-300/50'
                                                : 'bg-slate-300/30 hover:bg-slate-300/50',
                                          )}
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
                                       colSpan={productModelColumns.length}
                                       className="h-20 text-center"
                                    >
                                       No product models
                                    </TableCell>
                                 </TableRow>
                              )}
                           </TableBody>
                        </Table>
                     </div>
                  </div>
               </div>
            )}
         </LoadingOverlay>
      </div>
   );
};

export default CategoryDetailPage;
