'use client';

import { useState, useEffect, useRef } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { ChevronDown, Check, AlertCircle } from 'lucide-react';
import {
   Controller,
   type UseFormReturn,
   type Path,
   type FieldValues,
} from 'react-hook-form';

interface FormSelectorProps<T extends FieldValues> {
   form: UseFormReturn<T>;
   name: Path<T>;
   label?: string;
   placeholder?: string;
   options?: string[];
   className?: string;
   disabled?: boolean;
   required?: boolean;
   onSelect?: (value: string) => void;
}

export function FormSelector<T extends FieldValues>({
   form,
   name,
   label = 'Select an option',
   placeholder = 'Select...',
   options = [],
   className = '',
   disabled = false,
   required = false,
   onSelect,
}: FormSelectorProps<T>) {
   const [isOpen, setIsOpen] = useState(false);
   const dropdownRef = useRef<HTMLDivElement>(null);

   const {
      control,
      formState: { errors },
   } = form;

   // Get the error for this field if it exists
   const errorMessage = errors[name]?.message as string | undefined;
   const hasError = !!errorMessage;

   // Handle clicking outside to close dropdown
   useEffect(() => {
      function handleClickOutside(event: MouseEvent) {
         if (
            dropdownRef.current &&
            !dropdownRef.current.contains(event.target as Node)
         ) {
            setIsOpen(false);
         }
      }

      document.addEventListener('mousedown', handleClickOutside);
      return () => {
         document.removeEventListener('mousedown', handleClickOutside);
      };
   }, []);

   return (
      <div className={`space-y-1 ${className}`}>
         <div className="relative" ref={dropdownRef}>
            <Controller
               control={control}
               name={name}
               render={({ field }) => (
                  <>
                     <div className="text-sm text-gray-500 mb-1 absolute left-3 top-1 font-normal">
                        {label}{' '}
                        {required && <span className="text-red-500">*</span>}
                     </div>

                     <div
                        className={`flex items-end h-[56px] justify-between border rounded-lg px-3 py-2 cursor-pointer bg-white ${
                           hasError
                              ? 'border-red-500 focus:border-red-500'
                              : 'border-gray-300 focus:border-blue-500'
                        } ${disabled ? 'opacity-60 cursor-not-allowed' : ''}`}
                        onClick={() => {
                           if (!disabled) {
                              setIsOpen(!isOpen);
                           }
                        }}
                     >
                        <span
                           className={`text-base font-normal ${!field.value ? 'text-gray-400' : 'text-gray-900'}`}
                        >
                           {field.value || placeholder}
                        </span>
                        <ChevronDown
                           className={`h-5 w-5 text-gray-500 absolute right-5 top-5 transition-transform ${isOpen ? 'rotate-180' : ''}`}
                        />
                     </div>

                     <AnimatePresence>
                        {isOpen && (
                           <motion.div
                              initial={{ opacity: 0, y: -10 }}
                              animate={{ opacity: 1, y: 0 }}
                              exit={{ opacity: 0, y: -10 }}
                              transition={{ duration: 0.15 }}
                              className="absolute z-10 mt-1 w-full bg-white border border-gray-300 rounded-lg shadow-lg max-h-60 overflow-y-auto"
                           >
                              {options.map((option) => (
                                 <div
                                    key={option}
                                    className={`px-3 py-2 cursor-pointer flex items-center justify-between ${
                                       option === field.value
                                          ? 'bg-blue-500 text-white'
                                          : 'hover:bg-gray-100'
                                    }`}
                                    onClick={() => {
                                       field.onChange(option);
                                       if (onSelect) onSelect(option);
                                       setIsOpen(false);
                                    }}
                                 >
                                    {option}
                                    {option === field.value && (
                                       <Check className="h-4 w-4" />
                                    )}
                                 </div>
                              ))}
                           </motion.div>
                        )}
                     </AnimatePresence>
                  </>
               )}
            />
         </div>

         {hasError && (
            <div className="flex items-center text-red-500 text-sm px-1">
               <AlertCircle className="h-4 w-4 mr-1" />
               <span>{errorMessage}</span>
            </div>
         )}
      </div>
   );
}
