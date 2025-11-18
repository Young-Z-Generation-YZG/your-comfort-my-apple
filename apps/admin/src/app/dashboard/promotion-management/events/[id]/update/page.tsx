'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useRouter, useParams } from 'next/navigation';
import { useState, useEffect, useMemo } from 'react';
import { Button } from '@components/ui/button';
import {
   Form,
   FormControl,
   FormDescription,
   FormField,
   FormItem,
   FormLabel,
   FormMessage,
} from '@components/ui/form';
import { Input } from '@components/ui/input';
import { Textarea } from '@components/ui/textarea';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import { Badge } from '@components/ui/badge';
import {
   ColumnDef,
   flexRender,
   getCoreRowModel,
   getSortedRowModel,
   SortingState,
   useReactTable,
} from '@tanstack/react-table';
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '@components/ui/popover';
import { Calendar } from '@components/ui/calendar';
import { CalendarIcon, ArrowLeft, Plus, Trash2, X } from 'lucide-react';
import { format } from 'date-fns';
import { cn } from '~/src/infrastructure/lib/utils';
import { LoadingOverlay } from '@components/loading-overlay';
import { usePromotionService } from '~/src/hooks/api/use-promotion-service';
import { toast } from '~/src/hooks/use-toast';
import type {
   TEvent,
   TEventItem,
} from '~/src/infrastructure/services/promotion.service';

const updateEventSchema = z
   .object({
      title: z
         .string()
         .min(1, {
            message: 'Title is required.',
         })
         .min(3, {
            message: 'Title must be at least 3 characters.',
         })
         .optional(),
      description: z
         .string()
         .min(1, {
            message: 'Description is required.',
         })
         .min(10, {
            message: 'Description must be at least 10 characters.',
         })
         .optional(),
      start_date: z
         .date({
            required_error: 'Start date is required.',
         })
         .optional(),
      end_date: z
         .date({
            required_error: 'End date is required.',
         })
         .optional(),
   })
   .refine(
      (data) => {
         if (data.start_date && data.end_date) {
            return data.end_date >= data.start_date;
         }
         return true;
      },
      {
         message: 'End date must be after start date.',
         path: ['end_date'],
      },
   );

type UpdateEventFormValues = z.infer<typeof updateEventSchema>;

type NewEventItem = {
   sku_id: string;
   discount_type: string;
   discount_value: number;
   stock: number;
};

type EventItemTableRow = {
   id: string;
   model: string;
   color: string;
   storage: string;
   discount: string;
   discountValue: number;
   discountType: string;
   stock: number;
   sold: number;
   imageUrl: string;
};

const PromotionManagementEventsUpdatePage = () => {
   const router = useRouter();
   const params = useParams<{ id: string }>();
   const { updateEventAsync, getEventDetailsAsync, isLoading } =
      usePromotionService();
   const [isSubmitting, setIsSubmitting] = useState(false);
   const [event, setEvent] = useState<TEvent | null>(null);
   const [isLoadingEvent, setIsLoadingEvent] = useState(true);
   const [newEventItems, setNewEventItems] = useState<NewEventItem[]>([]);
   const [removedItemIds, setRemovedItemIds] = useState<string[]>([]);
   const [sorting, setSorting] = useState<SortingState>([]);

   // Define columns for event items table
   const eventItemColumns: ColumnDef<EventItemTableRow>[] = [
      {
         accessorKey: 'imageUrl',
         header: 'Image',
         cell: ({ row }) => {
            const src = row.getValue('imageUrl') as string;
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
         header: 'Model',
         cell: ({ row }) => (
            <div className="font-medium">{row.getValue('model')}</div>
         ),
      },
      {
         accessorKey: 'color',
         header: 'Color',
      },
      {
         accessorKey: 'storage',
         header: 'Storage',
      },
      {
         accessorKey: 'discount',
         header: 'Discount',
         cell: ({ row }) => {
            const original = row.original;
            return (
               <Badge variant="secondary">
                  {original.discountValue}% {original.discountType}
               </Badge>
            );
         },
      },
      {
         accessorKey: 'stock',
         header: 'Stock',
      },
      {
         accessorKey: 'sold',
         header: 'Sold',
      },
      {
         id: 'actions',
         header: 'Actions',
         cell: ({ row }) => {
            const item = row.original;
            return (
               <Button
                  type="button"
                  variant="ghost"
                  size="sm"
                  onClick={() => {
                     setRemovedItemIds([...removedItemIds, item.id]);
                  }}
                  disabled={isSubmitting}
                  className="text-destructive hover:text-destructive"
               >
                  <Trash2 className="h-4 w-4" />
               </Button>
            );
         },
      },
   ];

   // Prepare table data from event items
   const eventItemTableData: EventItemTableRow[] = useMemo(() => {
      if (!event?.event_items) return [];
      return event.event_items
         .filter((item) => !removedItemIds.includes(item.id))
         .map((item: TEventItem) => ({
            id: item.id,
            model: item.model_name,
            color: item.color_name,
            storage: item.storage_name,
            discount: `${item.discount_value}% ${item.discount_type}`,
            discountValue: item.discount_value,
            discountType: item.discount_type,
            stock: item.stock,
            sold: item.sold,
            imageUrl: item.image_url,
         }));
   }, [event?.event_items, removedItemIds]);

   // Create table instance
   const eventItemsTable = useReactTable({
      data: eventItemTableData,
      columns: eventItemColumns,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      onSortingChange: setSorting,
      state: { sorting },
   });

   const form = useForm<UpdateEventFormValues>({
      resolver: zodResolver(updateEventSchema),
      defaultValues: {
         title: '',
         description: '',
      },
   });

   // Fetch event details on mount
   useEffect(() => {
      const fetchEvent = async () => {
         if (!params?.id) {
            toast({
               variant: 'destructive',
               title: 'Error',
               description: 'Event ID is missing.',
            });
            router.push('/dashboard/promotion-management/events');
            return;
         }

         setIsLoadingEvent(true);
         const result = await getEventDetailsAsync(params.id as string);

         if (result.isSuccess && result.data) {
            const eventData = result.data as TEvent;
            setEvent(eventData);

            // Pre-populate form with existing data
            form.reset({
               title: eventData.title,
               description: eventData.description,
               start_date: eventData.start_date
                  ? new Date(eventData.start_date)
                  : undefined,
               end_date: eventData.end_date
                  ? new Date(eventData.end_date)
                  : undefined,
            });

            // Reset event items state
            setNewEventItems([]);
            setRemovedItemIds([]);
         } else {
            toast({
               variant: 'destructive',
               title: 'Error',
               description: 'Failed to load event details.',
            });
            router.push('/dashboard/promotion-management/events');
         }
         setIsLoadingEvent(false);
      };

      fetchEvent();
   }, [params?.id, getEventDetailsAsync, router, form]);

   const onSubmit = async (data: UpdateEventFormValues) => {
      if (!params?.id) {
         toast({
            variant: 'destructive',
            title: 'Error',
            description: 'Event ID is missing.',
         });
         return;
      }

      setIsSubmitting(true);

      try {
         // Build payload with only changed fields
         const payload: {
            title?: string | null;
            description?: string | null;
            start_date?: string | null;
            end_date?: string | null;
            add_event_items?:
               | {
                    sku_id: string;
                    discount_type: string;
                    discount_value: number;
                    stock: number;
                 }[]
               | null;
            remove_event_item_ids?: string[] | null;
         } = {};

         if (data.title !== undefined && data.title !== event?.title) {
            payload.title = data.title;
         }

         if (
            data.description !== undefined &&
            data.description !== event?.description
         ) {
            payload.description = data.description;
         }

         if (data.start_date) {
            const newStartDate = data.start_date.toISOString();
            if (newStartDate !== event?.start_date) {
               payload.start_date = newStartDate;
            }
         }

         if (data.end_date) {
            const newEndDate = data.end_date.toISOString();
            if (newEndDate !== event?.end_date) {
               payload.end_date = newEndDate;
            }
         }

         // Add event items if any
         if (newEventItems.length > 0) {
            payload.add_event_items = newEventItems.filter(
               (item) =>
                  item.sku_id.trim() &&
                  item.discount_type &&
                  item.discount_value > 0 &&
                  item.stock > 0,
            );
         }

         // Remove event items if any
         if (removedItemIds.length > 0) {
            payload.remove_event_item_ids = removedItemIds;
         }

         // Only update if there are changes
         if (
            Object.keys(payload).length === 0 ||
            (Object.keys(payload).length === 1 &&
               payload.add_event_items?.length === 0)
         ) {
            toast({
               title: 'Info',
               description: 'No changes detected.',
            });
            setIsSubmitting(false);
            return;
         }

         const result = await updateEventAsync(params.id as string, payload);

         if (result.isSuccess) {
            toast({
               title: 'Success',
               description: 'Event updated successfully!',
            });

            // Reset state
            setNewEventItems([]);
            setRemovedItemIds([]);

            // Navigate back to event details after a short delay
            setTimeout(() => {
               router.push(
                  `/dashboard/promotion-management/events/${params.id}`,
               );
            }, 1000);
         } else {
            toast({
               variant: 'destructive',
               title: 'Error',
               description: result.error
                  ? (result.error as any)?.data?.message ||
                    'Failed to update event'
                  : 'Failed to update event. Please try again.',
            });
         }
      } catch (error) {
         console.error('Error updating event:', error);
         toast({
            variant: 'destructive',
            title: 'Error',
            description: 'An unexpected error occurred. Please try again.',
         });
      } finally {
         setIsSubmitting(false);
      }
   };

   if (isLoadingEvent) {
      return (
         <div className="p-5">
            <LoadingOverlay isLoading={true} />
            <div className="rounded-lg border bg-card p-6">
               <p className="text-muted-foreground">Loading event details...</p>
            </div>
         </div>
      );
   }

   return (
      <div className="p-5">
         <LoadingOverlay isLoading={isLoading || isSubmitting} fullScreen />

         <div className="mb-6">
            <Button
               variant="ghost"
               onClick={() =>
                  router.push(
                     `/dashboard/promotion-management/events/${params?.id}`,
                  )
               }
               className="mb-4"
            >
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>

            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Update Event
               </h1>
               <p className="text-muted-foreground mt-1">
                  Update promotion event details
               </p>
            </div>
         </div>

         <div className="rounded-lg border bg-card p-6 w-auto">
            <Form {...form}>
               <form
                  onSubmit={form.handleSubmit(onSubmit)}
                  className="space-y-6"
               >
                  <FormField
                     control={form.control}
                     name="title"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Event Title</FormLabel>
                           <FormControl>
                              <Input
                                 placeholder="e.g., Black Friday Sale 2024"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Enter a descriptive title for the promotion event.
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="description"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Description</FormLabel>
                           <FormControl>
                              <Textarea
                                 placeholder="e.g., Limited-time savings on your favorite Apple products..."
                                 className="min-h-[100px]"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Provide a detailed description of the event.
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                     <FormField
                        control={form.control}
                        name="start_date"
                        render={({ field }) => (
                           <FormItem className="flex flex-col">
                              <FormLabel>Start Date</FormLabel>
                              <Popover>
                                 <PopoverTrigger asChild>
                                    <FormControl>
                                       <Button
                                          variant={'outline'}
                                          className={cn(
                                             'w-full pl-3 text-left font-normal',
                                             !field.value &&
                                                'text-muted-foreground',
                                             isSubmitting &&
                                                'opacity-50 cursor-not-allowed',
                                          )}
                                          disabled={isSubmitting}
                                       >
                                          {field.value ? (
                                             format(field.value, 'PPP')
                                          ) : (
                                             <span>Pick a start date</span>
                                          )}
                                          <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                       </Button>
                                    </FormControl>
                                 </PopoverTrigger>
                                 <PopoverContent
                                    className="w-auto p-0"
                                    align="start"
                                 >
                                    <Calendar
                                       mode="single"
                                       selected={field.value}
                                       onSelect={field.onChange}
                                       disabled={(date) =>
                                          date < new Date() || isSubmitting
                                       }
                                       initialFocus
                                    />
                                 </PopoverContent>
                              </Popover>
                              <FormDescription>
                                 The date when the event starts.
                              </FormDescription>
                              <FormMessage />
                           </FormItem>
                        )}
                     />

                     <FormField
                        control={form.control}
                        name="end_date"
                        render={({ field }) => (
                           <FormItem className="flex flex-col">
                              <FormLabel>End Date</FormLabel>
                              <Popover>
                                 <PopoverTrigger asChild>
                                    <FormControl>
                                       <Button
                                          variant={'outline'}
                                          className={cn(
                                             'w-full pl-3 text-left font-normal',
                                             !field.value &&
                                                'text-muted-foreground',
                                             isSubmitting &&
                                                'opacity-50 cursor-not-allowed',
                                          )}
                                          disabled={isSubmitting}
                                       >
                                          {field.value ? (
                                             format(field.value, 'PPP')
                                          ) : (
                                             <span>Pick an end date</span>
                                          )}
                                          <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                       </Button>
                                    </FormControl>
                                 </PopoverTrigger>
                                 <PopoverContent
                                    className="w-auto p-0"
                                    align="start"
                                 >
                                    <Calendar
                                       mode="single"
                                       selected={field.value}
                                       onSelect={field.onChange}
                                       disabled={(date) => {
                                          const startDate =
                                             form.watch('start_date');
                                          return (
                                             (startDate && date < startDate) ||
                                             date < new Date() ||
                                             isSubmitting
                                          );
                                       }}
                                       initialFocus
                                    />
                                 </PopoverContent>
                              </Popover>
                              <FormDescription>
                                 The date when the event ends. Must be after
                                 start date.
                              </FormDescription>
                              <FormMessage />
                           </FormItem>
                        )}
                     />
                  </div>

                  {/* Event Items Management */}
                  <div className="space-y-4 pt-6 border-t">
                     <div className="flex items-center justify-between">
                        <div>
                           <h3 className="text-lg font-semibold">
                              Event Items
                           </h3>
                           <p className="text-sm text-muted-foreground">
                              Manage items included in this event
                           </p>
                        </div>
                        <Button
                           type="button"
                           variant="outline"
                           size="sm"
                           onClick={() => {
                              setNewEventItems([
                                 ...newEventItems,
                                 {
                                    sku_id: '',
                                    discount_type: 'PERCENTAGE',
                                    discount_value: 0,
                                    stock: 0,
                                 },
                              ]);
                           }}
                           disabled={isSubmitting}
                        >
                           <Plus className="mr-2 h-4 w-4" />
                           Add Item
                        </Button>
                     </div>

                     {/* Existing Event Items */}
                     {event?.event_items && event.event_items.length > 0 && (
                        <div className="rounded-md border overflow-auto">
                           <Table>
                              <TableHeader>
                                 {eventItemsTable
                                    .getHeaderGroups()
                                    .map((headerGroup) => (
                                       <TableRow key={headerGroup.id}>
                                          {headerGroup.headers.map((header) => (
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
                                 {eventItemsTable.getRowModel().rows?.length ? (
                                    eventItemsTable
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
                                             {row
                                                .getVisibleCells()
                                                .map((cell) => (
                                                   <TableCell key={cell.id}>
                                                      {flexRender(
                                                         cell.column.columnDef
                                                            .cell,
                                                         cell.getContext(),
                                                      )}
                                                   </TableCell>
                                                ))}
                                          </TableRow>
                                       ))
                                 ) : (
                                    <TableRow>
                                       <TableCell
                                          colSpan={eventItemColumns.length}
                                          className="h-20 text-center"
                                       >
                                          No event items
                                       </TableCell>
                                    </TableRow>
                                 )}
                              </TableBody>
                           </Table>
                        </div>
                     )}

                     {/* New Event Items Form */}
                     {newEventItems.length > 0 && (
                        <div className="space-y-4">
                           <h4 className="text-sm font-medium">
                              New Items to Add
                           </h4>
                           {newEventItems.map((item, index) => (
                              <div
                                 key={index}
                                 className="rounded-md border p-4 space-y-4"
                              >
                                 <div className="flex items-center justify-between mb-2">
                                    <span className="text-sm font-medium">
                                       Item {index + 1}
                                    </span>
                                    <Button
                                       type="button"
                                       variant="ghost"
                                       size="sm"
                                       onClick={() => {
                                          setNewEventItems(
                                             newEventItems.filter(
                                                (_, i) => i !== index,
                                             ),
                                          );
                                       }}
                                       disabled={isSubmitting}
                                    >
                                       <X className="h-4 w-4" />
                                    </Button>
                                 </div>
                                 <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-4">
                                    <div>
                                       <label className="text-sm font-medium mb-1 block">
                                          SKU ID
                                       </label>
                                       <Input
                                          placeholder="Enter SKU ID"
                                          value={item.sku_id}
                                          onChange={(e) => {
                                             const updated = [...newEventItems];
                                             updated[index].sku_id =
                                                e.target.value;
                                             setNewEventItems(updated);
                                          }}
                                          disabled={isSubmitting}
                                       />
                                    </div>
                                    <div>
                                       <label className="text-sm font-medium mb-1 block">
                                          Discount Type
                                       </label>
                                       <Select
                                          value={item.discount_type}
                                          onValueChange={(value) => {
                                             const updated = [...newEventItems];
                                             updated[index].discount_type =
                                                value;
                                             setNewEventItems(updated);
                                          }}
                                          disabled={isSubmitting}
                                       >
                                          <SelectTrigger>
                                             <SelectValue placeholder="Select type" />
                                          </SelectTrigger>
                                          <SelectContent>
                                             <SelectItem value="PERCENTAGE">
                                                Percentage
                                             </SelectItem>
                                             <SelectItem value="FIXED_AMOUNT">
                                                Fixed Amount
                                             </SelectItem>
                                          </SelectContent>
                                       </Select>
                                    </div>
                                    <div>
                                       <label className="text-sm font-medium mb-1 block">
                                          Discount Value
                                       </label>
                                       <Input
                                          type="number"
                                          placeholder="e.g., 10"
                                          value={item.discount_value || ''}
                                          onChange={(e) => {
                                             const updated = [...newEventItems];
                                             updated[index].discount_value =
                                                parseFloat(e.target.value) || 0;
                                             setNewEventItems(updated);
                                          }}
                                          disabled={isSubmitting}
                                          min="0"
                                          step="0.01"
                                       />
                                    </div>
                                    <div>
                                       <label className="text-sm font-medium mb-1 block">
                                          Stock
                                       </label>
                                       <Input
                                          type="number"
                                          placeholder="e.g., 100"
                                          value={item.stock || ''}
                                          onChange={(e) => {
                                             const updated = [...newEventItems];
                                             updated[index].stock =
                                                parseInt(e.target.value) || 0;
                                             setNewEventItems(updated);
                                          }}
                                          disabled={isSubmitting}
                                          min="1"
                                       />
                                    </div>
                                 </div>
                              </div>
                           ))}
                        </div>
                     )}

                     {/* Show removed items count */}
                     {removedItemIds.length > 0 && (
                        <div className="rounded-md border border-destructive bg-destructive/10 p-3">
                           <p className="text-sm text-destructive">
                              {removedItemIds.length} item(s) will be removed
                              from the event
                           </p>
                        </div>
                     )}
                  </div>

                  <div className="flex justify-end gap-4 pt-4">
                     <Button
                        type="button"
                        variant="outline"
                        onClick={() =>
                           router.push(
                              `/dashboard/promotion-management/events/${params?.id}`,
                           )
                        }
                        disabled={isSubmitting}
                     >
                        Cancel
                     </Button>
                     <Button type="submit" disabled={isSubmitting || isLoading}>
                        {isSubmitting ? 'Updating...' : 'Update Event'}
                     </Button>
                  </div>
               </form>
            </Form>
         </div>
      </div>
   );
};

export default PromotionManagementEventsUpdatePage;
