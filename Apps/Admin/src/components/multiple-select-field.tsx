'use client';

import { Fragment } from 'react';
import {
   type UseFormReturn,
   type Path,
   type FieldValues,
   ControllerRenderProps,
   ArrayPath,
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
import { Input } from './ui/input';
import { Textarea } from './ui/textarea';
import { MultiSelect } from '@components/ui/multi-select';

export type OptionType = {
   value: string;
   label: string;
   icon?: React.ComponentType<{ className?: string }>;
};

type MultipleSelectFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: Path<T>;
   optionsData: OptionType[];
   onValueChange: (value: any[]) => void;
   label: string;
   type?:
      | 'text'
      | 'email'
      | 'password'
      | 'number'
      | 'color'
      | 'url'
      | 'textarea';
   description?: string;
   className?: string;
   disabled?: boolean;
   placeholder?: string;
   defaultValue?: OptionType[];
};

export function MultipleSelectField<T extends FieldValues>({
   form,
   name,
   optionsData,
   label,
   onValueChange,
   description,
   className = '',
   placeholder = '',
   defaultValue = [],
   disabled = false,
}: MultipleSelectFieldProps<T>) {
   return (
      <div className={cn('pb-2', className)}>
         <FormField
            control={form.control}
            name={name as Path<T>}
            render={({ field }) => (
               <Fragment>
                  <FormItem>
                     <FormLabel className="">{label}</FormLabel>

                     <FormControl>
                        <MultiSelect
                           options={optionsData}
                           onValueChange={onValueChange}
                           defaultValue={[]}
                           placeholder={placeholder || 'Select values...'}
                           variant="inverted"
                           animation={2}
                           maxCount={5}
                           disabled={disabled}
                        />
                     </FormControl>

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
