'use client';

import { useState, useEffect, useRef } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { ChevronDown, Check } from 'lucide-react';

const countries = [
   'Afghanistan',
   'Albania',
   'Algeria',
   'Andorra',
   'Angola',
   'Argentina',
   'Armenia',
   'Australia',
   'Austria',
   'Belgium',
   'Brazil',
   'Canada',
   'China',
   'Denmark',
   'Egypt',
   'Finland',
   'France',
   'Germany',
   'Greece',
   'India',
   'Indonesia',
   'Iran',
   'Iraq',
   'Ireland',
   'Israel',
   'Italy',
   'Japan',
   'Kenya',
   'Korea, South',
   'Mexico',
   'Netherlands',
   'New Zealand',
   'Norway',
   'Pakistan',
   'Poland',
   'Portugal',
   'Russia',
   'Saudi Arabia',
   'Singapore',
   'South Africa',
   'Spain',
   'Sweden',
   'Switzerland',
   'Taiwan',
   'Tajikistan',
   'Tanzania',
   'Thailand',
   'Timor-Leste',
   'Togo',
   'Tokelau',
   'Tonga',
   'Trinidad and Tobago',
   'Tunisia',
   'Türkiye',
   'Turkmenistan',
   'Turks and Caicos Islands',
   'Tuvalu',
   'Uganda',
   'Ukraine',
   'United Arab Emirates',
   'United Kingdom',
   'United States',
   'Vietnam',
];

interface SelectorProps {
   defaultValue?: string;
   onChange?: (country: string) => void;
   className?: string;
   label?: string;
}

const Selector = ({
   defaultValue = 'United States',
   onChange,
   className = '',
   label = 'Country/Region',
}: SelectorProps) => {
   const [selectedCountry, setSelectedCountry] = useState(defaultValue);
   const [isOpen, setIsOpen] = useState(false);
   const dropdownRef = useRef<HTMLDivElement>(null);

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

   const handleSelect = (country: string) => {
      setSelectedCountry(country);
      if (onChange) onChange(country);
      setIsOpen(false);
   };

   return (
      <div className={`relative ${className}`} ref={dropdownRef}>
         <div className="text-sm text-gray-500 mb-1 absolute left-3 top-1 font-light">
            {label}
         </div>
         <div
            className="flex items-end h-[--input_h] justify-between border border-gray-300 rounded-lg px-3 py-2 cursor-pointer bg-white"
            onClick={() => setIsOpen(!isOpen)}
         >
            <span className="text-gray-900 text-base font-normal">
               {selectedCountry}
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
                  {countries.map((country) => (
                     <div
                        key={country}
                        className={`px-3 py-2 cursor-pointer flex items-center justify-between ${
                           country === selectedCountry
                              ? 'bg-blue-500 text-white'
                              : 'hover:bg-gray-100'
                        }`}
                        onClick={() => handleSelect(country)}
                     >
                        {country}
                        {country === selectedCountry && (
                           <Check className="h-4 w-4" />
                        )}
                     </div>
                  ))}
               </motion.div>
            )}
         </AnimatePresence>
      </div>
   );
};

export default Selector;
