'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useRouter } from 'next/navigation';
import { useState } from 'react';
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
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '@components/ui/popover';
import { Calendar } from '@components/ui/calendar';
import { CalendarIcon } from 'lucide-react';
import { format } from 'date-fns';
import { cn } from '~/src/infrastructure/lib/utils';
import { LoadingOverlay } from '@components/loading-overlay';
import { usePromotionService } from '~/src/hooks/api/use-promotion-service';
import { toast } from '~/src/hooks/use-toast';
import { ArrowLeft } from 'lucide-react';

const createEventSchema = z
   .object({
      title: z
         .string()
         .min(1, {
            message: 'Title is required.',
         })
         .min(3, {
            message: 'Title must be at least 3 characters.',
         }),
      description: z
         .string()
         .min(1, {
            message: 'Description is required.',
         })
         .min(10, {
            message: 'Description must be at least 10 characters.',
         }),
      start_date: z.date({
         required_error: 'Start date is required.',
      }),
      end_date: z.date({
         required_error: 'End date is required.',
      }),
   })
   .refine(
      (data) => {
         return data.end_date >= data.start_date;
      },
      {
         message: 'End date must be after start date.',
         path: ['end_date'],
      },
   );

type CreateEventFormValues = z.infer<typeof createEventSchema>;

const PromotionManagementEventsCreatePage = () => {
   const router = useRouter();
   const { createEventAsync, isLoading } = usePromotionService();
   const [isSubmitting, setIsSubmitting] = useState(false);

   const form = useForm<CreateEventFormValues>({
      resolver: zodResolver(createEventSchema),
      defaultValues: {
         title: '',
         description: '',
      },
   });

   const onSubmit = async (data: CreateEventFormValues) => {
      setIsSubmitting(true);

      try {
         // Convert dates to ISO string format
         const payload = {
            title: data.title,
            description: data.description,
            start_date: data.start_date.toISOString(),
            end_date: data.end_date.toISOString(),
         };

         const result = await createEventAsync(payload);

         if (result.isSuccess) {
            toast({
               title: 'Success',
               description: 'Event created successfully!',
            });

            // Navigate back to events list after a short delay
            setTimeout(() => {
               router.push('/dashboard/promotion-management/events');
            }, 1000);
         } else {
            toast({
               variant: 'destructive',
               title: 'Error',
               description: result.error
                  ? (result.error as any)?.data?.message ||
                    'Failed to create event'
                  : 'Failed to create event. Please try again.',
            });
         }
      } catch (error) {
         console.error('Error creating event:', error);
         toast({
            variant: 'destructive',
            title: 'Error',
            description: 'An unexpected error occurred. Please try again.',
         });
      } finally {
         setIsSubmitting(false);
      }
   };

   return (
      <div className="p-5">
         <LoadingOverlay isLoading={isLoading || isSubmitting} />

         <div className="mb-6">
            <Button
               variant="ghost"
               onClick={() => router.back()}
               className="mb-4"
            >
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>

            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Create New Event
               </h1>
               <p className="text-muted-foreground mt-1">
                  Create a new promotion event with title, description, and date
                  range
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

                  <div className="flex justify-end gap-4 pt-4">
                     <Button
                        type="button"
                        variant="outline"
                        onClick={() => router.back()}
                        disabled={isSubmitting}
                     >
                        Cancel
                     </Button>
                     <Button type="submit" disabled={isSubmitting || isLoading}>
                        {isSubmitting ? 'Creating...' : 'Create Event'}
                     </Button>
                  </div>
               </form>
            </Form>
         </div>
      </div>
   );
};

export default PromotionManagementEventsCreatePage;
