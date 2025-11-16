'use client';

import { useEffect, useMemo, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { usePromotionService } from '~/src/hooks/api/use-promotion-service';
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
import type {
   TEvent,
   TEventItem,
} from '~/src/infrastructure/services/promotion.service';

const formatDateTime = (value: string) => {
   if (!value) return '-';
   const d = new Date(value);
   if (Number.isNaN(d.getTime())) return value;
   return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
   }).format(d);
};

type TEventItemRow = {
   id: string;
   model: string;
   color: string;
   storage: string;
   classification: string;
   originalPrice: number;
   discountType: string;
   discountValue: number;
   stock: number;
   sold: number;
   imageUrl: string;
};

const itemColumns: ColumnDef<TEventItemRow>[] = [
   {
      accessorKey: 'imageUrl',
      header: 'Image',
      cell: ({ row }) => {
         const src = (row.getValue('imageUrl') as string) || '';
         return src ? (
            // eslint-disable-next-line @next/next/no-img-element
            <img
               src={src}
               alt={row.original.model}
               className="h-10 w-10 rounded-md object-cover"
            />
         ) : (
            <div className="h-10 w-10 rounded-md bg-gray-100" />
         );
      },
   },
   {
      accessorKey: 'model',
      header: ({ column }) => (
         <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
         >
            Model
            <ArrowUpDown className="ml-2 h-4 w-4" />
         </Button>
      ),
      cell: ({ row }) => (
         <div className="font-medium">{row.getValue('model')}</div>
      ),
   },
   { accessorKey: 'color', header: 'Color' },
   { accessorKey: 'storage', header: 'Storage' },
   { accessorKey: 'classification', header: 'Classification' },
   {
      accessorKey: 'originalPrice',
      header: ({ column }) => (
         <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
         >
            Original Price
            <ArrowUpDown className="ml-2 h-4 w-4" />
         </Button>
      ),
      cell: ({ row }) => {
         const price = Number(row.getValue('originalPrice'));
         const formatted = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
         }).format(Number.isFinite(price) ? price : 0);
         return <div>{formatted}</div>;
      },
   },
   { accessorKey: 'discountType', header: 'Discount Type' },
   { accessorKey: 'discountValue', header: 'Discount Value' },
   { accessorKey: 'stock', header: 'Stock' },
   { accessorKey: 'sold', header: 'Sold' },
];

const PromotionEventDetailPage = () => {
   const params = useParams<{ id: string }>();
   const router = useRouter();
   const { isLoading, getEventDetailsAsync } = usePromotionService();
   const [event, setEvent] = useState<TEvent | null>(null);
   const [error, setError] = useState<string | null>(null);

   useEffect(() => {
      const fetchDetails = async () => {
         if (!params?.id) return;
         const res = await getEventDetailsAsync(params.id as string);
         if (res.isSuccess && res.data) {
            setEvent(res.data as TEvent);
            setError(null);
         } else {
            setError('Failed to load event details');
         }
      };
      fetchDetails();
   }, [params?.id, getEventDetailsAsync]);

   const meta = useMemo(() => {
      if (!event) return [];
      return [
         { label: 'Title', value: event.title },
         { label: 'Description', value: event.description },
         { label: 'Start', value: formatDateTime(event.start_date) },
         { label: 'End', value: formatDateTime(event.end_date) },
         { label: 'Items', value: event.event_items?.length ?? 0 },
      ];
   }, [event]);

   const itemRows: TEventItemRow[] = useMemo(() => {
      return (event?.event_items || []).map((it: TEventItem) => ({
         id: it.id,
         model: it.model_name,
         color: it.color_name,
         storage: it.storage_name,
         classification: it.product_classification,
         originalPrice: it.original_price,
         discountType: it.discount_type,
         discountValue: it.discount_value,
         stock: it.stock,
         sold: it.sold,
         imageUrl: it.image_url,
      }));
   }, [event?.event_items]);

   const [sorting, setSorting] = useState<SortingState>([]);
   const itemsTable = useReactTable({
      data: itemRows,
      columns: itemColumns,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      onSortingChange: setSorting,
      state: { sorting },
   });

   return (
      <div className="p-5">
         <div className="mb-4 flex items-center justify-between">
            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Promotion Event details
               </h1>
               <p className="text-muted-foreground">
                  View event metadata and items
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
            {!event ? (
               <div className="rounded-md border bg-card p-6 text-sm text-muted-foreground">
                  {isLoading ? 'Loading...' : 'No event found.'}
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
                        <h2 className="text-lg font-semibold">Event Items</h2>
                     </div>
                     <div className="overflow-auto">
                        <Table>
                           <TableHeader>
                              {itemsTable.getHeaderGroups().map((hg) => (
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
                              {itemsTable.getRowModel().rows?.length ? (
                                 itemsTable
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
                                       colSpan={itemColumns.length}
                                       className="h-20 text-center"
                                    >
                                       No event items
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

export default PromotionEventDetailPage;
