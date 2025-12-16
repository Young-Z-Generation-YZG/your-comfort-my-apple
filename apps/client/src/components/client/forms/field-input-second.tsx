import { Input } from '~/components/ui/input';
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

type FieldInputProps<T extends FieldValues> = {
   name: Path<T>;
   form: UseFormReturn<T>;
   rules?: RegisterOptions;
   label?: string;
   placeholder?: string;
   disabled?: boolean;
   type?: 'text' | 'email' | 'password' | 'number' | 'color' | 'url';
   defaultValue?: string;
};

const FieldInputSecond = <T extends FieldValues>({
   name,
   form,
   rules,
   label,
   placeholder,
   disabled = false,
   type = 'text',
   defaultValue = '',
}: FieldInputProps<T>) => {
   const {
      control,
      formState: { errors },
   } = form;

   const errorMessage = getNestedError(errors, name);
   const hasError = !!errorMessage;

   return (
      <div>
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
            defaultValue={defaultValue as any}
            render={({ field }) => {
               return (
                  <>
                     <motion.div
                        className="relative"
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
                        <Label htmlFor={name}>{label}</Label>

                        <Input
                           {...field}
                           type={type}
                           disabled={disabled}
                           placeholder={placeholder}
                           onBlur={() => {
                              field.onBlur();
                           }}
                           className={cn(
                              'mt-2 transition-all font-SFProText duration-200',
                              {
                                 'no-spinner': type === 'number',
                              },
                           )}
                        />

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
                     </motion.div>
                  </>
               );
            }}
         />
      </div>
   );
};

export default FieldInputSecond;
