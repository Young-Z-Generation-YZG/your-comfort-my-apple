'use client';

import { useForm } from 'react-hook-form';
import * as z from 'zod';
import { Button } from '@components/ui/button';
import { Calendar } from '@components/ui/calendar';
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
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '@components/ui/popover';
import { Textarea } from '@components/ui/textarea';
import { CalendarIcon } from 'lucide-react';
import { format } from 'date-fns';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { zodResolver } from '@hookform/resolvers/zod';
import { cn } from '~/src/infrastructure/lib/utils';

const formSchema = z.object({
   name: z.string().min(2, {
      message: 'Name must be at least 2 characters.',
   }),
   description: z.string().optional(),
   startDate: z.date({
      required_error: 'Start date is required.',
   }),
   endDate: z.date({
      required_error: 'End date is required.',
   }),
   discountType: z.enum(['percentage', 'fixed']),
   discountValue: z.string().min(1, {
      message: 'Discount value is required.',
   }),
   status: z.enum(['draft', 'scheduled', 'active']),
   applyToAll: z.boolean(),
   categories: z.array(z.string()).optional(),
});

export function EventPromotionForm() {
   const form = useForm<z.infer<typeof formSchema>>({
      resolver: zodResolver(formSchema),
      defaultValues: {
         name: '',
         description: '',
         discountType: 'percentage',
         discountValue: '',
         status: 'draft',
         applyToAll: true,
      },
   });

   function onSubmit(values: z.infer<typeof formSchema>) {
      console.log(values);
      // Submit form data to your API
   }

   return (
      <Form {...form}>
         <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="space-y-6 py-4"
         >
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
               <FormField
                  control={form.control}
                  name="name"
                  render={({ field }) => (
                     <FormItem>
                        <FormLabel>Promotion Name</FormLabel>
                        <FormControl>
                           <Input placeholder="Summer Sale 2024" {...field} />
                        </FormControl>
                        <FormDescription>
                           Enter a descriptive name for your promotion event.
                        </FormDescription>
                        <FormMessage />
                     </FormItem>
                  )}
               />

               <FormField
                  control={form.control}
                  name="status"
                  render={({ field }) => (
                     <FormItem>
                        <FormLabel>Status</FormLabel>
                        <Select
                           onValueChange={field.onChange}
                           defaultValue={field.value}
                        >
                           <FormControl>
                              <SelectTrigger>
                                 <SelectValue placeholder="Select status" />
                              </SelectTrigger>
                           </FormControl>
                           <SelectContent>
                              <SelectItem value="draft">Draft</SelectItem>
                              <SelectItem value="scheduled">
                                 Scheduled
                              </SelectItem>
                              <SelectItem value="active">Active</SelectItem>
                           </SelectContent>
                        </Select>
                        <FormDescription>
                           Set the current status of this promotion.
                        </FormDescription>
                        <FormMessage />
                     </FormItem>
                  )}
               />
            </div>

            <FormField
               control={form.control}
               name="description"
               render={({ field }) => (
                  <FormItem>
                     <FormLabel>Description</FormLabel>
                     <FormControl>
                        <Textarea
                           placeholder="Describe your promotion event..."
                           className="resize-none"
                           {...field}
                        />
                     </FormControl>
                     <FormDescription>
                        Provide details about this promotion event.
                     </FormDescription>
                     <FormMessage />
                  </FormItem>
               )}
            />

            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
               <FormField
                  control={form.control}
                  name="startDate"
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
                                       !field.value && 'text-muted-foreground',
                                    )}
                                 >
                                    {field.value ? (
                                       format(field.value, 'PPP')
                                    ) : (
                                       <span>Pick a date</span>
                                    )}
                                    <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                 </Button>
                              </FormControl>
                           </PopoverTrigger>
                           <PopoverContent className="w-auto p-0" align="start">
                              <Calendar
                                 mode="single"
                                 selected={field.value}
                                 onSelect={field.onChange}
                                 initialFocus
                              />
                           </PopoverContent>
                        </Popover>
                        <FormMessage />
                     </FormItem>
                  )}
               />

               <FormField
                  control={form.control}
                  name="endDate"
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
                                       !field.value && 'text-muted-foreground',
                                    )}
                                 >
                                    {field.value ? (
                                       format(field.value, 'PPP')
                                    ) : (
                                       <span>Pick a date</span>
                                    )}
                                    <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                 </Button>
                              </FormControl>
                           </PopoverTrigger>
                           <PopoverContent className="w-auto p-0" align="start">
                              <Calendar
                                 mode="single"
                                 selected={field.value}
                                 onSelect={field.onChange}
                                 initialFocus
                              />
                           </PopoverContent>
                        </Popover>
                        <FormMessage />
                     </FormItem>
                  )}
               />
            </div>

            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
               <FormField
                  control={form.control}
                  name="discountType"
                  render={({ field }) => (
                     <FormItem>
                        <FormLabel>Discount Type</FormLabel>
                        <Select
                           onValueChange={field.onChange}
                           defaultValue={field.value}
                        >
                           <FormControl>
                              <SelectTrigger>
                                 <SelectValue placeholder="Select discount type" />
                              </SelectTrigger>
                           </FormControl>
                           <SelectContent>
                              <SelectItem value="percentage">
                                 Percentage (%)
                              </SelectItem>
                              <SelectItem value="fixed">
                                 Fixed Amount ($)
                              </SelectItem>
                           </SelectContent>
                        </Select>
                        <FormDescription>
                           Choose how the discount will be applied.
                        </FormDescription>
                        <FormMessage />
                     </FormItem>
                  )}
               />

               <FormField
                  control={form.control}
                  name="discountValue"
                  render={({ field }) => (
                     <FormItem>
                        <FormLabel>Discount Value</FormLabel>
                        <FormControl>
                           <Input
                              type="text"
                              placeholder={
                                 form.watch('discountType') === 'percentage'
                                    ? '20'
                                    : '50'
                              }
                              {...field}
                           />
                        </FormControl>
                        <FormDescription>
                           {form.watch('discountType') === 'percentage'
                              ? 'Enter percentage value (e.g., 20 for 20%)'
                              : 'Enter fixed amount in dollars (e.g., 50 for $50)'}
                        </FormDescription>
                        <FormMessage />
                     </FormItem>
                  )}
               />
            </div>
         </form>
      </Form>
   );
}
