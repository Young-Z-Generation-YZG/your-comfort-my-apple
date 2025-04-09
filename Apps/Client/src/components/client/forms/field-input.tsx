'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import {
   type UseFormReturn,
   Controller,
   type Path,
   type FieldValues,
   type RegisterOptions,
   UseFormRegister,
} from 'react-hook-form';
import { AlertCircle, CircleIcon as ExclamationCircleIcon } from 'lucide-react';
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

interface FieldInputProps<T extends FieldValues> {
   form: UseFormReturn<T>;
   name: Path<T>;
   type?: 'text' | 'email' | 'password' | 'number' | 'color' | 'url';
   label: string;
   required?: boolean;
   className?: string;
   errorTextClassName?: string;
   disabled?: boolean;
   visibleError?: boolean;
   hasArrowButton?: boolean;
   fetchingFunc?: (data: any) => void;
   rules?: RegisterOptions;
   defaultValue?: string;
}

export function FieldInput<T extends FieldValues>({
   form,
   name,
   type = 'text',
   label,
   required = false,
   className = '',
   errorTextClassName = '',
   visibleError = true,
   disabled = false,
   hasArrowButton = false,
   fetchingFunc,
   rules,
   defaultValue = '',
}: FieldInputProps<T>) {
   const [isFocused, setIsFocused] = useState(false);
   const {
      control,
      watch,
      setValue,
      formState: { errors },
   } = form;

   // Get the error for this field if it exists
   const errorMessage = getNestedError(errors, name);
   const hasError = !!errorMessage;

   return (
      <div className="space-y-1">
         <div className={`relative`}>
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
                  const isActive = isFocused || field.value?.length > 0;

                  return (
                     <>
                        <motion.label
                           initial={{ y: 0, scale: 1 }}
                           animate={{
                              y: isActive ? -14 : 0,
                              scale: isActive ? 0.8 : 1,
                              color: hasError
                                 ? '#ef4444'
                                 : isActive
                                   ? '#666'
                                   : '#999',
                           }}
                           className="absolute left-4 origin-left cursor-text pointer-events-none font-SFProText text-base font-light"
                           style={{
                              top: '30%',
                              transform: 'translateY(-50%)',
                              transformOrigin: 'left top',
                           }}
                        >
                           {label}
                           {true && (
                              <span
                                 className={cn(
                                    'ml-1',
                                    isActive ? 'text-red-500' : '',
                                 )}
                              >
                                 *
                              </span>
                           )}
                        </motion.label>

                        <input
                           type={type}
                           disabled={disabled}
                           className={cn(
                              'w-full px-4 py-2 pt-5 focus:outline-none text-base h-[56px] border',
                              className,
                              `${
                                 hasError && visibleError
                                    ? 'border-red-500 focus:border-red-500'
                                    : 'border-gray-300 focus:border-blue-500'
                              }`,
                           )}
                           onFocus={() => setIsFocused(true)}
                           {...field}
                           value={field.value || ''}
                           onBlur={() => {
                              setIsFocused(false);
                              field.onBlur();
                           }}
                        />

                        {hasArrowButton && (
                           <button
                              type="button"
                              className="absolute right-4 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600"
                              onClick={() => {
                                 // You can add custom logic here if needed
                                 form.handleSubmit((data) =>
                                    fetchingFunc ? fetchingFunc(data) : null,
                                 )();
                              }}
                           >
                              <svg
                                 xmlns="http://www.w3.org/2000/svg"
                                 width="20"
                                 height="20"
                                 viewBox="0 0 24 24"
                                 fill="none"
                                 stroke="currentColor"
                                 strokeWidth="2"
                                 strokeLinecap="round"
                                 strokeLinejoin="round"
                                 className="lucide lucide-arrow-right-circle"
                              >
                                 <circle cx="12" cy="12" r="10" />
                                 <path d="m8 12 4 4" />
                                 <path d="m16 12-4 4" />
                                 <path d="M8 12h8" />
                              </svg>
                           </button>
                        )}
                     </>
                  );
               }}
            />
         </div>

         {hasError && visibleError && (
            <div
               className={cn(
                  'flex items-center text-red-500 text-sm px-1',
                  errorTextClassName,
               )}
            >
               <AlertCircle className="h-4 w-4 mr-1" />
               <span>{errorMessage}</span>
            </div>
         )}
      </div>
   );
}
