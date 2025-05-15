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

type InputFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: Path<T> | ArrayPath<T>;
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
};

export function InputField<T extends FieldValues>({
   form,
   name,
   label,
   type = 'text',
   description,
   className = '',
   disabled = false,
}: InputFieldProps<T>) {
   const renderInput = (field: ControllerRenderProps<T, Path<T>>) => {
      switch (type) {
         case 'textarea':
            return (
               <Textarea
                  {...field}
                  value={field.value || ''}
                  disabled={disabled}
               />
            );
         default:
            return (
               <Input
                  type="text"
                  {...field}
                  value={field.value || ''}
                  disabled={disabled}
               />
            );
      }
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
                     <FormControl>{renderInput(field)}</FormControl>

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
