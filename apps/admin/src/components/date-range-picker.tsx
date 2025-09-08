'use client';

import { Calendar } from '@components/calendar';
import { Button } from '@components/ui/button';
import { FormDescription, FormItem, FormLabel } from '@components/ui/form';
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '@components/ui/popover';
import { Separator } from '@components/ui/separator';
import { addDays, format } from 'date-fns';
import { CalendarIcon } from 'lucide-react';
import { useEffect, useState } from 'react';
import { type DateRange } from 'react-day-picker';
import { cn } from '~/src/infrastructure/lib/utils';
import { type UseFormReturn, type Path, FieldValues } from 'react-hook-form';

type DateRangePickerProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   nameFrom: Path<T>;
   nameTo: Path<T>;
   label?: string;
   description?: string;
   className?: string;
   onValueChange?: (value: DateRange | undefined) => void;
   value?: DateRange | undefined;
};

export default function DateRangePicker<T extends FieldValues>({
   form,
   nameFrom,
   nameTo,
   label,
   description,
   className,
   onValueChange,
   value,
}: DateRangePickerProps<T>) {
   const [date, setDate] = useState<DateRange | undefined>({
      from: addDays(new Date(), -20),
      to: new Date(),
   });

   const fromDateErrorMessage = form.formState.errors[nameFrom]?.message;
   const toDateErrorMessage = form.formState.errors[nameTo]?.message;

   useEffect(() => {
      onValueChange?.(date);
   }, [date, onValueChange]);

   useEffect(() => {
      setDate(value);
   }, [value]);

   return (
      <div className={cn('grid gap-2 pb-2', className)}>
         <FormItem>
            <FormLabel className="">{label}</FormLabel>
         </FormItem>
         <Popover
            onOpenChange={(open) => {
               if (!open) {
                  form.trigger(nameFrom);
                  form.trigger(nameTo);
               }
            }}
         >
            <PopoverTrigger asChild>
               <Button
                  id="date"
                  variant="outline"
                  className={cn(
                     'w-full justify-start text-left font-normal',
                     !date && 'text-muted-foreground',
                  )}
               >
                  <CalendarIcon className="mr-2 h-4 w-4" />
                  {date?.from ? (
                     date.to ? (
                        <>
                           {format(date.from, 'LLL dd, y')} -{' '}
                           {format(date.to, 'LLL dd, y')}
                           {' | GMT+0700 (Indochina Time)'}
                        </>
                     ) : (
                        format(date.from, 'LLL dd, y')
                     )
                  ) : (
                     <span>Pick a date</span>
                  )}
               </Button>
            </PopoverTrigger>
            <PopoverContent className="w-auto p-0" align="start">
               <Calendar
                  autoFocus
                  mode="range"
                  defaultMonth={date?.from}
                  selected={date}
                  onSelect={setDate}
                  numberOfMonths={2}
               />

               <Separator className="my-2" />

               {/* <AppointmentPicker
                  onChange={(times) => console.log('Selected times:', times)}
                  minTime="00:00 AM"
                  maxTime="00:00 PM"
                  increment={30}
                  defaultDuration={60}
               /> */}
            </PopoverContent>
         </Popover>

         {description && (
            <FormDescription className={cn('')}>{description}</FormDescription>
         )}

         {fromDateErrorMessage && (
            <p className="text-red-500 text-[12.8px] font-medium">
               {String(fromDateErrorMessage)}
            </p>
         )}

         {toDateErrorMessage && (
            <p className="text-red-500 text-[12.8px] font-medium">
               {String(toDateErrorMessage)}
            </p>
         )}
      </div>
   );
}
