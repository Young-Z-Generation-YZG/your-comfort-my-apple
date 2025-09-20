'use client';

import type React from 'react';

import { useState } from 'react';
import { motion } from 'framer-motion';
import {
   Controller,
   type UseFormReturn,
   type Path,
   type FieldValues,
} from 'react-hook-form';
import { AlertCircle } from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';

interface FormPhoneInputProps<T extends FieldValues> {
   form: UseFormReturn<T>;
   name: Path<T>;
   label?: string;
   className?: string;
   required?: boolean;
   disabled?: boolean;
   countryCode?: string;
   countryLabel?: string;
   labelClassName?: string;
   errorTextClassName?: string;
}

export function FormPhoneInput<T extends FieldValues>({
   form,
   name,
   label = 'Phone Number',
   className = '',
   required = false,
   disabled = false,
   countryCode = '84',
   countryLabel = '+84',
   labelClassName = '',
   errorTextClassName = '',
}: FormPhoneInputProps<T>) {
   const [isFocused, setIsFocused] = useState(false);

   const {
      control,
      formState: { errors },
      watch,
   } = form;

   // Get the error for this field if it exists
   const errorMessage = errors[name]?.message as string | undefined;
   const hasError = !!errorMessage;

   // Get the current value from the form
   const phoneValue = (watch(name) as string) || '';

   // Format Vietnamese phone number
   const formatVietnamesePhoneNumber = (input: string): string => {
      // Remove all non-digit characters
      const digits = input.replace(/\D/g, '');

      // Handle the leading zero
      let formattedDigits = digits;
      if (digits.startsWith('0') && digits.length > 1) {
         formattedDigits = digits;
      } else if (!digits.startsWith('0') && digits.length > 0) {
         formattedDigits = '0' + digits;
      }

      // Format based on length
      if (formattedDigits.length <= 4) {
         return formattedDigits;
      } else if (formattedDigits.length <= 7) {
         return `${formattedDigits.slice(0, 4)} ${formattedDigits.slice(4)}`;
      } else {
         return `${formattedDigits.slice(0, 4)} ${formattedDigits.slice(4, 7)} ${formattedDigits.slice(7, 10)}`;
      }
   };

   return (
      <div className={`space-y-1`}>
         <Controller
            control={control}
            name={name}
            render={({ field }) => {
               // Format the displayed value
               const formattedValue = formatVietnamesePhoneNumber(
                  field.value || '',
               );

               // Handle input change
               const handleChange = (
                  e: React.ChangeEvent<HTMLInputElement>,
               ) => {
                  const input = e.target.value;
                  const digits = input.replace(/\D/g, '');

                  // Limit to 10 digits (Vietnamese format)
                  if (digits.length <= 10) {
                     // Pass raw digits to form
                     field.onChange(digits);
                  }
               };

               return (
                  <>
                     <div
                        className={`relative border rounded-lg overflow-hidden transition-colors ${
                           hasError
                              ? 'border-red-500'
                              : isFocused
                                ? 'border-blue-500'
                                : 'border-gray-300'
                        } ${disabled ? 'opacity-60 cursor-not-allowed' : ''}`}
                     >
                        <div className="flex items-center">
                           <div
                              className={cn(
                                 'pl-3 pr-2 text-gray-500 border-r border-gray-300 py-3 text-base font-light',
                                 labelClassName,
                              )}
                           >
                              {countryLabel}
                           </div>

                           <div className="relative flex-1">
                              <motion.label
                                 initial={{ y: 0, scale: 1 }}
                                 animate={{
                                    y: isFocused || formattedValue ? -14 : 0,
                                    scale:
                                       isFocused || formattedValue ? 0.8 : 1,
                                    color: hasError
                                       ? '#ef4444'
                                       : isFocused
                                         ? '#0066CC'
                                         : '#666',
                                 }}
                                 className={cn(
                                    'absolute left-4 origin-left cursor-text pointer-events-none font-SFProText text-base font-light z-50',
                                    labelClassName,
                                 )}
                                 style={{
                                    top: '30%',
                                    transform: 'translateY(-50%)',
                                    transformOrigin: 'left top',
                                 }}
                              >
                                 {label}{' '}
                                 {required && (
                                    <span className="text-red-500">*</span>
                                 )}
                              </motion.label>

                              <input
                                 type="tel"
                                 value={formattedValue}
                                 onChange={handleChange}
                                 onFocus={() => setIsFocused(true)}
                                 onBlur={() => {
                                    setIsFocused(false);
                                    field.onBlur();
                                 }}
                                 disabled={disabled}
                                 className={cn(
                                    `w-full px-4 pb-2 pt-5 h-[56px] text-base focus:outline-none ${
                                       hasError
                                          ? 'text-red-500'
                                          : 'text-gray-900'
                                    }`,
                                    className,
                                 )}
                                 placeholder=""
                              />
                           </div>
                        </div>
                     </div>

                     {hasError && (
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
                  </>
               );
            }}
         />
      </div>
   );
}
