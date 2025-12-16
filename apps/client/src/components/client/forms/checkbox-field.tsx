import { Checkbox } from '~/components/ui/checkbox';
import { Label } from '~/components/ui/label';
import { motion, AnimatePresence } from 'framer-motion';
import { AlertCircle } from 'lucide-react';
import {
   Controller,
   FieldValues,
   Path,
   RegisterOptions,
   UseFormReturn,
} from 'react-hook-form';
import { cn } from '~/infrastructure/lib/utils';

const getNestedError = (errors: any, name: string): string | undefined => {
   const keys = name.split('.');
   let error = errors;
   for (const key of keys) {
      error = error?.[key];
      if (!error) return undefined;
   }
   return error?.message as string | undefined;
};

type CheckboxFieldProps<T extends FieldValues> = {
   name: Path<T>;
   form: UseFormReturn<T>;
   rules?: RegisterOptions;
   label?: string;
   disabled?: boolean;
   className?: string;
   checkboxClassName?: string;
   onCheckedChange?: (checked: boolean) => void;
};

const CheckboxField = <T extends FieldValues>({
   name,
   form,
   rules,
   label,
   disabled = false,
   className,
   checkboxClassName,
   onCheckedChange,
}: CheckboxFieldProps<T>) => {
   const {
      control,
      formState: { errors },
   } = form;

   const errorMessage = getNestedError(errors, name);
   const hasError = !!errorMessage;

   return (
      <div className={cn('flex flex-col', className)}>
         <Controller
            control={control}
            name={name}
            rules={
               rules as
                  | Omit<
                       RegisterOptions<T, Path<T>>,
                       | 'setValueAs'
                       | 'disabled'
                       | 'valueAsNumber'
                       | 'valueAsDate'
                    >
                  | undefined
            }
            render={({ field }) => {
               return (
                  <>
                     <motion.div
                        className="flex items-center space-x-2"
                        variants={{
                           hidden: { opacity: 0, y: 20 },
                           visible: {
                              opacity: 1,
                              y: 0,
                              transition: {
                                 type: 'spring',
                                 stiffness: 300,
                                 damping: 24,
                              },
                           },
                        }}
                     >
                        <Checkbox
                           id={name}
                           checked={field.value}
                           onCheckedChange={(checked) => {
                              field.onChange(checked);
                              onCheckedChange?.(checked === true);
                           }}
                           disabled={disabled}
                           className={cn(checkboxClassName)}
                        />
                        {label && (
                           <Label
                              htmlFor={name}
                              className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 cursor-pointer"
                           >
                              {label}
                           </Label>
                        )}
                     </motion.div>

                     {hasError && (
                        <AnimatePresence>
                           <motion.p
                              initial={{ opacity: 0, height: 0 }}
                              animate={{ opacity: 1, height: 'auto' }}
                              exit={{ opacity: 0, height: 0 }}
                              className="text-sm text-red-600 flex items-center mt-1"
                           >
                              <AlertCircle className="h-3 w-3 mr-1" />
                              {errorMessage}
                           </motion.p>
                        </AnimatePresence>
                     )}
                  </>
               );
            }}
         />
      </div>
   );
};

export default CheckboxField;
