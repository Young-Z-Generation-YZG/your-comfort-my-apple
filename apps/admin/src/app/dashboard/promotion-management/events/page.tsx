'use client';

import { useEffect, useState } from 'react';
import { usePromotionService } from '~/src/hooks/api/use-promotion-service';
import {
   ColumnDef,
   flexRender,
   getCoreRowModel,
   getSortedRowModel,
   SortingState,
   useReactTable,
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
import { ArrowUpDown, MoreHorizontal, Plus } from 'lucide-react';
import { LoadingOverlay } from '@components/loading-overlay';
import { cn } from '~/src/infrastructure/lib/utils';
import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuLabel,
   DropdownMenuSeparator,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import type { TEvent } from '~/src/infrastructure/services/promotion.service';
import { useRouter } from 'next/navigation';

type TEventRow = {
   id: string;
   title: string;
   startDate: string;
   endDate: string;
   items: number;
};

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

const PromotionManagementEventsPage = () => {
   const router = useRouter();

   const columns: ColumnDef<TEventRow>[] = [
      {
         accessorKey: 'title',
         header: ({ column }) => (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Title
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         ),
         cell: ({ row }) => (
            <div className="font-medium">{row.getValue('title')}</div>
         ),
      },
      {
         accessorKey: 'startDate',
         header: 'Start Date',
         cell: ({ row }) => (
            <div className="text-muted-foreground">
               {formatDateTime(row.getValue('startDate') as string)}
            </div>
         ),
      },
      {
         accessorKey: 'endDate',
         header: 'End Date',
         cell: ({ row }) => (
            <div className="text-muted-foreground">
               {formatDateTime(row.getValue('endDate') as string)}
            </div>
         ),
      },
      {
         accessorKey: 'items',
         header: 'Event Items',
         cell: ({ row }) => <div>{row.getValue('items') as number}</div>,
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
                        Copy Event ID
                     </DropdownMenuItem>
                     <DropdownMenuSeparator />
                     <DropdownMenuItem
                        onClick={() =>
                           router.push(
                              `/dashboard/promotion-management/events/${item.id}`,
                           )
                        }
                     >
                        View details
                     </DropdownMenuItem>
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
   const { isLoading, getEventsAsync } = usePromotionService();
   const [data, setData] = useState<TEventRow[]>([]);
   const [error, setError] = useState<string | null>(null);

   const [sorting, setSorting] = useState<SortingState>([]);

   useEffect(() => {
      const fetchEvents = async () => {
         const res = await getEventsAsync();
         if (res.isSuccess && res.data) {
            const rows = (res.data as TEvent[]).map((e) => ({
               id: e.id,
               title: e.title,
               startDate: e.start_date,
               endDate: e.end_date,
               items: Array.isArray(e.event_items) ? e.event_items.length : 0,
            }));
            setData(rows);
            setError(null);
         } else {
            setError('Failed to load events');
         }
      };
      fetchEvents();
   }, [getEventsAsync]);

   const table = useReactTable({
      data,
      columns,
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
                  Promotion Management - Events
               </h1>
               <p className="text-muted-foreground">
                  Manage promotion events and items
               </p>
            </div>
            <Button
               onClick={() =>
                  router.push('/dashboard/promotion-management/events/create')
               }
            >
               <Plus className="mr-2 h-4 w-4" />
               Create Event
            </Button>
         </div>
         {error && <div className="mb-3 text-sm text-red-600">{error}</div>}
         <LoadingOverlay isLoading={isLoading}>
            <div className="rounded-lg border bg-card">
               <div className="overflow-auto">
                  <Table>
                     <TableHeader>
                        {table.getHeaderGroups().map((hg) => (
                           <TableRow key={hg.id}>
                              {hg.headers.map((header) => (
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
                           table.getRowModel().rows.map((row, idx) => (
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
                                 colSpan={columns.length}
                                 className="h-20 text-center"
                              >
                                 {isLoading ? 'Loading...' : 'No events found.'}
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

export default PromotionManagementEventsPage;
