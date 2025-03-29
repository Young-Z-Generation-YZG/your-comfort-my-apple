'use client';

import { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { ChevronDown, HelpCircle } from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';

interface BirthdaySelectorProps {
   onChange?: (
      date: { month: string; day: number; year: number } | null,
   ) => void;
   className?: string;
   required?: boolean;
}

const BirthdaySelector = ({
   onChange,
   className = '',
   required = false,
}: BirthdaySelectorProps) => {
   const [month, setMonth] = useState<string>('');
   const [day, setDay] = useState<number | ''>('');
   const [year, setYear] = useState<number | ''>('');
   const [error, setError] = useState<string>('');

   const [monthOpen, setMonthOpen] = useState(false);
   const [dayOpen, setDayOpen] = useState(false);
   const [yearOpen, setYearOpen] = useState(false);

   const months = [
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

   // Generate days 1-31
   const days = Array.from({ length: 31 }, (_, i) => i + 1);

   // Generate years from current year - 100 to current year
   const currentYear = new Date().getFullYear();
   const years = Array.from({ length: 100 }, (_, i) => currentYear - i);

   // Get days in month
   const getDaysInMonth = (month: string, year: number): number => {
      const monthIndex = months.findIndex((m) => m === month);
      if (monthIndex === -1) return 31;

      return new Date(year, monthIndex + 1, 0).getDate();
   };

   // Validate date
   useEffect(() => {
      if (month && day && year) {
         const monthIndex = months.findIndex((m) => m === month);
         const daysInMonth = getDaysInMonth(month, year);

         if (day > daysInMonth) {
            setError(`${month} ${year} has only ${daysInMonth} days`);
            if (onChange) onChange(null);
            return;
         }

         const birthDate = new Date(year, monthIndex, day);
         const today = new Date();

         if (birthDate > today) {
            setError('Birthday cannot be in the future');
            if (onChange) onChange(null);
            return;
         }

         if (today.getFullYear() - year < 13) {
            setError('You must be at least 13 years old');
            if (onChange) onChange(null);
            return;
         }

         setError('');
         if (onChange)
            onChange({ month, day: day as number, year: year as number });
      } else if (required && (month || day || year)) {
         setError('Enter a valid birthday.');
         if (onChange) onChange(null);
      } else {
         setError(required ? 'Enter a valid birthday.' : '');
         if (onChange) onChange(null);
      }
   }, [month, day, year, required, onChange]);

   // Close all dropdowns when clicking outside
   useEffect(() => {
      const handleClickOutside = () => {
         setMonthOpen(false);
         setDayOpen(false);
         setYearOpen(false);
      };

      window.addEventListener('click', handleClickOutside);
      return () => window.removeEventListener('click', handleClickOutside);
   }, []);

   const handleDropdownClick = (
      e: React.MouseEvent,
      dropdown: 'month' | 'day' | 'year',
   ) => {
      e.stopPropagation();

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

   const getValidDays = () => {
      if (!month || !year) return days;

      const daysInMonth = getDaysInMonth(month, year as number);
      return days.slice(0, daysInMonth);
   };

   const validDays = getValidDays();

   return (
      <div className={`space-y-2 ${className}`}>
         <div className="flex items-center">
            <label className="text-gray-800 font-normal">Birthday</label>
            <button
               type="button"
               className="ml-1 text-gray-500 hover:text-gray-700"
               onClick={(e) => {
                  e.preventDefault();
                  alert(
                     'Your birthday helps us verify your identity and provide age-appropriate experiences.',
                  );
               }}
            >
               <HelpCircle size={16} />
            </button>
         </div>

         <div className="flex gap-3">
            {/* Month Selector */}
            <div className="relative flex-1">
               <div
                  className={`flex items-center h-[--input_h] justify-between border rounded-md px-3 py-2 cursor-pointer ${
                     error ? 'border-red-500 text-red-500' : 'border-gray-300'
                  }`}
                  onClick={(e) => handleDropdownClick(e, 'month')}
               >
                  <span
                     className={cn(
                        'font-light text-base',
                        month ? 'text-gray-900' : 'text-gray-400',
                     )}
                  >
                     {month || 'Month'}
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
                        {months.map((m) => (
                           <div
                              key={m}
                              className={`px-3 py-2 cursor-pointer hover:bg-gray-100 ${m === month ? 'bg-gray-100' : ''}`}
                              onClick={(e) => {
                                 e.stopPropagation();
                                 setMonth(m);
                                 setMonthOpen(false);
                              }}
                           >
                              {m}
                           </div>
                        ))}
                     </motion.div>
                  )}
               </AnimatePresence>
            </div>

            {/* Day Selector */}
            <div className="relative flex-1">
               <div
                  className={`flex items-center h-[--input_h] justify-between border rounded-md px-3 py-2 cursor-pointer ${
                     error ? 'border-red-500 text-red-500' : 'border-gray-300'
                  }`}
                  onClick={(e) => handleDropdownClick(e, 'day')}
               >
                  <span
                     className={cn(
                        'font-light text-base',
                        month ? 'text-gray-900' : 'text-gray-400',
                     )}
                  >
                     {day || 'Day'}
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
                        {validDays.map((d) => (
                           <div
                              key={d}
                              className={`px-3 py-2 cursor-pointer hover:bg-gray-100 ${d === day ? 'bg-gray-100' : ''}`}
                              onClick={(e) => {
                                 e.stopPropagation();
                                 setDay(d);
                                 setDayOpen(false);
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
            <div className="relative flex-1">
               <div
                  className={`flex items-center h-[--input_h] justify-between border rounded-md px-3 py-2 cursor-pointer ${
                     error ? 'border-red-500 text-red-500' : 'border-gray-300'
                  }`}
                  onClick={(e) => handleDropdownClick(e, 'year')}
               >
                  <span
                     className={cn(
                        'font-light text-base',
                        month ? 'text-gray-900' : 'text-gray-400',
                     )}
                  >
                     {year || 'Year'}
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
                        {years.map((y) => (
                           <div
                              key={y}
                              className={`px-3 py-2 cursor-pointer hover:bg-gray-100 ${y === year ? 'bg-gray-100' : ''}`}
                              onClick={(e) => {
                                 e.stopPropagation();
                                 setYear(y);
                                 setYearOpen(false);
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

export default BirthdaySelector;
