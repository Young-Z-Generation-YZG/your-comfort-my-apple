'use client';

import { Fragment } from 'react';
import {
   type UseFormReturn,
   type Path,
   type FieldValues,
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
   name: Path<T> | ArrayPath<T>;
   label: string;
   optionsData: OptionType[];
   description?: string;
   className?: string;
   placeholder?: string;
   disabled?: boolean;
   defaultValue?: string;
   onChange?: (value: string) => void;
   onRenderOptions?: () => React.ReactNode;
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
   defaultValue,
   onRenderOptions,
   onChange,
}: SelectFieldProps<T>) {
   const renderOptions = () => {
      if (onRenderOptions) {
         return onRenderOptions();
      }

      return optionsData.length > 0 ? (
         optionsData.map((item, index) => {
            return (
               <SelectItem key={index} value={item.value}>
                  {item.label}
               </SelectItem>
            );
         })
      ) : (
         <p className="ml-3 text-sm">Empty item</p>
      );
   };

   return (
      <div className={cn('pb-2', className)}>
         <FormField
            control={form.control}
            name={name as Path<T>}
            render={({ field }) => (
               <Fragment>
                  <FormItem>
                     <FormLabel className="">{label}</FormLabel>

                     <Select
                        onValueChange={(value) => {
                           field.onChange(value);
                           if (onChange) {
                              onChange(value);
                           }
                        }}
                        defaultValue={defaultValue || field.value}
                        disabled={disabled}
                     >
                        <FormControl>
                           <SelectTrigger>
                              <SelectValue
                                 placeholder={
                                    placeholder ??
                                    'Select a verified email to display'
                                 }
                                 defaultValue={field.value}
                              />
                           </SelectTrigger>
                        </FormControl>
                        <SelectContent>{renderOptions()}</SelectContent>
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
