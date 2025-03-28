'use client';

import type React from 'react';

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';

interface PhoneInputProps {
   value: string;
   onChange: (value: string) => void;
   onValidChange?: (isValid: boolean) => void;
   className?: string;
   required?: boolean;
}

const PhoneInput = ({
   value,
   onChange,
   onValidChange,
   className = '',
   required = false,
}: PhoneInputProps) => {
   const [isFocused, setIsFocused] = useState(false);
   const [error, setError] = useState<string>('');
   const [formattedValue, setFormattedValue] = useState('');

   // Format Vietnamese phone number as user types
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

   // Handle input change
   const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const input = e.target.value;
      const digits = input.replace(/\D/g, '');

      // Limit to 10 digits (Vietnamese format)
      if (digits.length <= 10) {
         const formatted = formatVietnamesePhoneNumber(digits);
         setFormattedValue(formatted);
         onChange(digits); // Pass raw digits to parent
      }
   };

   // Validate Vietnamese phone number
   //    useEffect(() => {
   //       if (!value) {
   //          if (required) {
   //             setError('Số điện thoại là bắt buộc');
   //             if (onValidChange) onValidChange(false);
   //          } else {
   //             setError('');
   //             if (onValidChange) onValidChange(true);
   //          }
   //          return;
   //       }

   //       // Vietnamese mobile numbers validation
   //       // Should start with 0 and have 10 digits total
   //       const vietnamesePattern = /^0[3-9][0-9]{8}$/;

   //       if (!vietnamesePattern.test(value)) {
   //          if (value.length < 10) {
   //             setError('Vui lòng nhập đủ 10 số');
   //          } else if (!value.startsWith('0')) {
   //             setError('Số điện thoại phải bắt đầu bằng số 0');
   //          } else {
   //             setError('Số điện thoại không hợp lệ');
   //          }
   //          if (onValidChange) onValidChange(false);
   //       } else {
   //          setError('');
   //          if (onValidChange) onValidChange(true);
   //       }
   //    }, [value, required, onValidChange]);

   // Update formatted value when value prop changes
   useEffect(() => {
      setFormattedValue(formatVietnamesePhoneNumber(value));
   }, [value]);

   return (
      <div className={`space-y-1 ${className}`}>
         <div
            className={`relative border rounded-lg overflow-hidden transition-colors ${
               error
                  ? 'border-red-500'
                  : isFocused
                    ? 'border-blue-500'
                    : 'border-gray-300'
            }`}
         >
            <div className="flex items-center">
               <div className="pl-3 pr-2 text-gray-500 border-r border-gray-300 py-3 text-base font-light">
                  +84
               </div>

               <div className="relative flex-1">
                  <motion.label
                     initial={{ y: 0, scale: 1 }}
                     animate={{
                        y: isFocused || formattedValue ? -12 : 0,
                        scale: isFocused || formattedValue ? 0.8 : 1,
                        color: isFocused ? '#0066CC' : '#666',
                     }}
                     className="absolute left-3 pointer-events-none font-light text-base"
                     style={{
                        top: '35%',
                        transform: 'translateY(-50%)',
                        transformOrigin: 'left top',
                     }}
                  >
                     Phone Number
                  </motion.label>

                  <input
                     type="tel"
                     value={formattedValue}
                     onChange={handleChange}
                     onFocus={() => setIsFocused(true)}
                     onBlur={() => setIsFocused(false)}
                     className={`w-full px-3 pt-5 h-[--input_h] text-base pb-1 focus:outline-none ${error ? 'text-red-500' : 'text-gray-900'}`}
                     placeholder=""
                  />
               </div>
            </div>
         </div>

         {error && (
            <p className="text-red-500 text-sm flex items-center">
               <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="16"
                  height="16"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  className="mr-1"
               >
                  <circle cx="12" cy="12" r="10" />
                  <line x1="12" y1="8" x2="12" y2="12" />
                  <line x1="12" y1="16" x2="12.01" y2="16" />
               </svg>
               {error}
            </p>
         )}
      </div>
   );
};

export default PhoneInput;
