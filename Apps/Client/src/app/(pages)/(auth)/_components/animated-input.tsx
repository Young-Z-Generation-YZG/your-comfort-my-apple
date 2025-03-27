'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';

interface AnimatedInputProps {
   type?: string;
   label: string;
   value: string;
   onChange: (value: string) => void;
   className?: string;
   hasArrowButton?: boolean;
}

const AnimatedInput = ({
   type = 'text',
   label,
   value,
   onChange,
   className = '',
   hasArrowButton = false,
}: AnimatedInputProps) => {
   const [isFocused, setIsFocused] = useState(false);

   const isActive = isFocused || value.length > 0;

   return (
      <div className={`relative ${className}`}>
         <motion.label
            initial={{ y: 0, scale: 1 }}
            animate={{
               y: isActive ? -14 : 0,
               scale: isActive ? 0.8 : 1,
               color: isActive ? '#666' : '#999',
            }}
            className="absolute left-4 origin-left cursor-text pointer-events-none font-SFProText"
            style={{
               top: '35%',
               transform: 'translateY(-50%)',
               transformOrigin: 'left top',
            }}
         >
            {label}
         </motion.label>

         <input
            type={type}
            value={value}
            onChange={(e) => onChange(e.target.value)}
            onFocus={() => setIsFocused(true)}
            onBlur={() => setIsFocused(false)}
            className="w-full px-4 py-2 pt-5 focus:outline-none text-base h-[56px]"
         />

         {hasArrowButton && (
            <button className="absolute right-4 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600">
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
      </div>
   );
};

export default AnimatedInput;
