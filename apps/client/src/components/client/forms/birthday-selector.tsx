'use client';

import type React from 'react';

import { useState, useEffect, useRef } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { ChevronDown, HelpCircle, AlertCircle } from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';
import { FieldValues, Path, UseFormReturn } from 'react-hook-form';

// Define the months array with numeric values
const MONTH_NAMES = [
   'January',
   'February',
   'March',
   'April',
   'May',
   'June',
   'July',
   'August',
   'September',
   'October',
   'November',
   'December',
];

// Generate months 1-12
const MONTHS = Array.from({ length: 12 }, (_, i) => i + 1);

// Generate days 1-31
const DAYS = Array.from({ length: 31 }, (_, i) => i + 1);

// Generate years from current year - 100 to current year
const CURRENT_YEAR = new Date().getFullYear();
const YEARS = Array.from({ length: 100 }, (_, i) => CURRENT_YEAR - i);

// Helper function to get days in a month
const getDaysInMonth = (month: number, year: number): number => {
   if (month < 1 || month > 12) return 31;
   return new Date(year, month, 0).getDate();
};

// Helper function to get month name from number
const getMonthName = (month: number): string => {
   return MONTH_NAMES[month - 1] || '';
};

// birthdate type definition
export interface birthdate {
   month: number;
   day: number;
   year: number;
}

interface FormBirthDateSelectorProps<T extends FieldValues> {
   form: UseFormReturn<T>;
   name: Path<T>;
   label?: string;
   helpText?: string;
   className?: string;
   required?: boolean;
   disabled?: boolean;
   showMonthAs?: 'number' | 'name' | 'both';
}

export function FormBirthDateSelector<T extends FieldValues>({
   form,
   name,
   label = 'Birth Date',
   helpText = 'Your birthdate helps us verify your identity and provide age-appropriate experiences.',
   className = '',
   required = false,
   disabled = false,
   showMonthAs = 'both',
}: FormBirthDateSelectorProps<T>) {
   // State for dropdown visibility
   const [monthOpen, setMonthOpen] = useState(false);
   const [dayOpen, setDayOpen] = useState(false);
   const [yearOpen, setYearOpen] = useState(false);

   // Refs for handling outside clicks
   const monthRef = useRef<HTMLDivElement>(null);
   const dayRef = useRef<HTMLDivElement>(null);
   const yearRef = useRef<HTMLDivElement>(null);

   const {
      formState: { errors },
      watch,
      setValue,
   } = form;

   // Get the current value from the form
   const birthdate = watch(name) as birthdate | undefined;

   // Get the error for this field if it exists
   const formError = errors[name] as any;

   const hasError = !!formError;

   const errorField = formError?.day ?? formError?.month ?? formError?.year;

   const errorText = formError?.message ?? errorField?.message;

   // Handle outside clicks to close dropdowns
   useEffect(() => {
      const handleClickOutside = (event: MouseEvent) => {
         if (
            monthRef.current &&
            !monthRef.current.contains(event.target as Node)
         ) {
            setMonthOpen(false);
         }
         if (dayRef.current && !dayRef.current.contains(event.target as Node)) {
            setDayOpen(false);
         }
         if (
            yearRef.current &&
            !yearRef.current.contains(event.target as Node)
         ) {
            setYearOpen(false);
         }
      };

      window.addEventListener('click', handleClickOutside);
      return () => window.removeEventListener('click', handleClickOutside);
   }, []);

   // Get valid days based on selected month and year
   const getValidDays = () => {
      if (!birthdate?.month || !birthdate?.year) return DAYS;

      const daysInMonth = getDaysInMonth(birthdate.month, birthdate.year);
      return DAYS.slice(0, daysInMonth);
   };

   const validDays = getValidDays();

   // Handle dropdown toggle
   const handleDropdownClick = (
      e: React.MouseEvent,
      dropdown: 'month' | 'day' | 'year',
   ) => {
      e.stopPropagation();

      if (disabled) return;

      if (dropdown === 'month') {
         setMonthOpen(!monthOpen);
         setDayOpen(false);
         setYearOpen(false);
      } else if (dropdown === 'day') {
         setDayOpen(!dayOpen);
         setMonthOpen(false);
         setYearOpen(false);
      } else {
         setYearOpen(!yearOpen);
         setMonthOpen(false);
         setDayOpen(false);
      }
   };

   // Handle selection of a month
   const handleMonthSelect = (month: number) => {
      const newbirthdate = {
         ...(birthdate || { day: undefined, year: undefined }),
         month,
      };

      // If the current day is invalid for the new month, reset it
      if (birthdate?.day && birthdate.year) {
         const daysInMonth = getDaysInMonth(month, birthdate.year);
         if (birthdate.day > daysInMonth) {
            newbirthdate.day = undefined;
         }
      }

      setValue(name, newbirthdate as any, { shouldValidate: true });
      setMonthOpen(false);
   };

   // Handle selection of a day
   const handleDaySelect = (day: number) => {
      setValue(
         name,
         {
            ...(birthdate || { month: undefined, year: undefined }),
            day,
         } as any,
         { shouldValidate: true },
      );
      setDayOpen(false);
   };

   // Handle selection of a year
   const handleYearSelect = (year: number) => {
      const newbirthdate = {
         ...(birthdate || { month: undefined, day: undefined }),
         year,
      };

      // If the current day is invalid for the selected month and year, reset it
      if (birthdate?.month && birthdate?.day) {
         const daysInMonth = getDaysInMonth(birthdate.month, year);
         if (birthdate.day > daysInMonth) {
            newbirthdate.day = undefined;
         }
      }

      setValue(name, newbirthdate as any, { shouldValidate: true });
      setYearOpen(false);
   };

   // Format month display based on showMonthAs prop
   const formatMonthDisplay = (month?: number): string => {
      if (!month) return 'Month';

      switch (showMonthAs) {
         case 'number':
            return month.toString();
         case 'name':
            return getMonthName(month);
         case 'both':
         default:
            return `${month} - ${getMonthName(month)}`;
      }
   };

   return (
      <div className={`space-y-2 ${className}`}>
         <div className="flex items-center">
            <label className="text-gray-800 font-normal">
               {label} {required && <span className="text-red-500">*</span>}
            </label>
            <button
               type="button"
               className="ml-1 text-gray-500 hover:text-gray-700"
               onClick={(e) => {
                  e.preventDefault();
                  alert(helpText);
               }}
               disabled={disabled}
            >
               <HelpCircle size={16} />
            </button>
         </div>

         <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
            {/* Month Selector */}
            <div className="relative w-full" ref={monthRef}>
               <div
                  className={cn(
                     'flex items-center h-[56px] justify-between border rounded-md px-3 py-2 cursor-pointer',
                     hasError
                        ? 'border-red-500 text-red-500'
                        : 'border-gray-300',
                     disabled && 'opacity-60 cursor-not-allowed',
                  )}
                  onClick={(e) => handleDropdownClick(e, 'month')}
               >
                  <span
                     className={cn(
                        'font-normal text-base',
                        birthdate?.month ? 'text-gray-900' : 'text-gray-400',
                     )}
                  >
                     {formatMonthDisplay(birthdate?.month)}
                  </span>
                  <ChevronDown
                     size={16}
                     className={`transition-transform ${monthOpen ? 'rotate-180' : ''}`}
                  />
               </div>

               <AnimatePresence>
                  {monthOpen && (
                     <motion.div
                        initial={{ opacity: 0, y: -10 }}
                        animate={{ opacity: 1, y: 0 }}
                        exit={{ opacity: 0, y: -10 }}
                        className="absolute z-20 mt-1 w-full bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-y-auto"
                     >
                        {MONTHS.map((m, index) => (
                           <div
                              key={index}
                              className={`px-3 py-2 cursor-pointer hover:bg-gray-100 ${
                                 m === birthdate?.month ? 'bg-gray-100' : ''
                              }`}
                              onClick={(e) => {
                                 e.stopPropagation();
                                 handleMonthSelect(m);
                              }}
                           >
                              {showMonthAs === 'number'
                                 ? m
                                 : showMonthAs === 'name'
                                   ? getMonthName(m)
                                   : `${m} - ${getMonthName(m)}`}
                           </div>
                        ))}
                     </motion.div>
                  )}
               </AnimatePresence>
            </div>

            {/* Day Selector */}
            <div className="relative w-full" ref={dayRef}>
               <div
                  className={cn(
                     'flex items-center h-[56px] justify-between border rounded-md px-3 py-2 cursor-pointer',
                     hasError
                        ? 'border-red-500 text-red-500'
                        : 'border-gray-300',
                     disabled && 'opacity-60 cursor-not-allowed',
                  )}
                  onClick={(e) => handleDropdownClick(e, 'day')}
               >
                  <span
                     className={cn(
                        'font-normal text-base',
                        birthdate?.day ? 'text-gray-900' : 'text-gray-400',
                     )}
                  >
                     {birthdate?.day || 'Day'}
                  </span>
                  <ChevronDown
                     size={16}
                     className={`transition-transform ${dayOpen ? 'rotate-180' : ''}`}
                  />
               </div>

               <AnimatePresence>
                  {dayOpen && (
                     <motion.div
                        initial={{ opacity: 0, y: -10 }}
                        animate={{ opacity: 1, y: 0 }}
                        exit={{ opacity: 0, y: -10 }}
                        className="absolute z-20 mt-1 w-full bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-y-auto"
                     >
                        {validDays.map((d, index) => (
                           <div
                              key={index}
                              className={`px-3 py-2 cursor-pointer hover:bg-gray-100 ${d === birthdate?.day ? 'bg-gray-100' : ''}`}
                              onClick={(e) => {
                                 e.stopPropagation();
                                 handleDaySelect(d);
                              }}
                           >
                              {d}
                           </div>
                        ))}
                     </motion.div>
                  )}
               </AnimatePresence>
            </div>

            {/* Year Selector */}
            <div className="relative w-full" ref={yearRef}>
               <div
                  className={cn(
                     'flex items-center h-[56px] justify-between border rounded-md px-3 py-2 cursor-pointer',
                     hasError
                        ? 'border-red-500 text-red-500'
                        : 'border-gray-300',
                     disabled && 'opacity-60 cursor-not-allowed',
                  )}
                  onClick={(e) => handleDropdownClick(e, 'year')}
               >
                  <span
                     className={cn(
                        'font-normal text-base',
                        birthdate?.year ? 'text-gray-900' : 'text-gray-400',
                     )}
                  >
                     {birthdate?.year || 'Year'}
                  </span>
                  <ChevronDown
                     size={16}
                     className={`transition-transform ${yearOpen ? 'rotate-180' : ''}`}
                  />
               </div>

               <AnimatePresence>
                  {yearOpen && (
                     <motion.div
                        initial={{ opacity: 0, y: -10 }}
                        animate={{ opacity: 1, y: 0 }}
                        exit={{ opacity: 0, y: -10 }}
                        className="absolute z-20 mt-1 w-full bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-y-auto"
                     >
                        {YEARS.map((y, index) => (
                           <div
                              key={index}
                              className={`px-3 py-2 cursor-pointer hover:bg-gray-100 ${
                                 y === birthdate?.year ? 'bg-gray-100' : ''
                              }`}
                              onClick={(e) => {
                                 e.stopPropagation();
                                 handleYearSelect(y);
                              }}
                           >
                              {y}
                           </div>
                        ))}
                     </motion.div>
                  )}
               </AnimatePresence>
            </div>
         </div>

         {hasError && (
            <div className="flex items-center text-red-500 text-sm">
               <AlertCircle size={16} className="mr-1" />
               <span>{errorText}</span>
            </div>
         )}
      </div>
   );
}
