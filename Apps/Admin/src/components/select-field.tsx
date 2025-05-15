'use client';

import { Fragment } from 'react';
import {
   type UseFormReturn,
   type Path,
   type FieldValues,
} from 'react-hook-form';
import { cn } from '~/src/infrastructure/lib/utils';
import {
   FormControl,
   FormDescription,
   FormField,
   FormItem,
   FormLabel,
   FormMessage,
} from './ui/form';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';

export type OptionType = {
   value: string;
   label: string;
   icon?: React.ComponentType<{ className?: string }>;
};

type SelectFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: Path<T>;
   label: string;
   optionsData: OptionType[];
   description?: string;
   className?: string;
   placeholder?: string;
   disabled?: boolean;
};

export function SelectField<T extends FieldValues>({
   form,
   name,
   optionsData,
   label,
   description,
   className = '',
   placeholder = '',
   disabled = false,
}: SelectFieldProps<T>) {
   return (
      <div className={cn('pb-2', className)}>
         <FormField
            control={form.control}
            name={name}
            render={({ field }) => (
               <Fragment>
                  <FormItem>
                     <FormLabel className="">{label}</FormLabel>

                     <Select
                        onValueChange={field.onChange}
                        defaultValue={field.value}
                        disabled={disabled}
                     >
                        <FormControl>
                           <SelectTrigger>
                              <SelectValue
                                 placeholder={
                                    placeholder ??
                                    'Select a verified email to display'
                                 }
                              />
                           </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                           {optionsData.length ? (
                              optionsData.map((item, index) => {
                                 return (
                                    <SelectItem key={index} value={item.value}>
                                       {item.label}
                                    </SelectItem>
                                 );
                              })
                           ) : (
                              <p className="ml-3 text-sm">Empty item</p>
                           )}
                        </SelectContent>
                     </Select>

                     {description && (
                        <FormDescription className={cn('')}>
                           {description}
                        </FormDescription>
                     )}

                     <FormMessage className="!mt-1" />
                  </FormItem>
               </Fragment>
            )}
         />
      </div>
   );
}
