'use client';

import type React from 'react';
import {
   useState,
   useRef,
   useEffect,
   type KeyboardEvent,
   type ClipboardEvent,
} from 'react';
import { motion } from 'framer-motion';

interface OTPInputProps {
   length?: number;
   onComplete?: (otp: string) => void;
   autoFocus?: boolean;
   disabled?: boolean;
   className?: string;
   inputClassName?: string;
   isError?: boolean;
   errorMessage?: string;
   onChange?: (otp: string) => void;
}

export function OTPInput({
   length = 6,
   onComplete,
   autoFocus = true,
   disabled = false,
   className = '',
   inputClassName = '',
   isError = false,
   errorMessage = 'Please enter a valid code',
   onChange,
}: OTPInputProps) {
   // State to store OTP digits
   const [otp, setOtp] = useState<string[]>(Array(length).fill(''));

   // Refs for input elements
   const inputRefs = useRef<(HTMLInputElement | null)[]>([]);

   // Initialize refs array
   useEffect(() => {
      inputRefs.current = inputRefs.current.slice(0, length);
   }, [length]);

   // Auto-focus first input on mount
   useEffect(() => {
      if (autoFocus && inputRefs.current[0] && !disabled) {
         inputRefs.current[0].focus();
      }
   }, [autoFocus, disabled]);

   // Handle input change
   const handleChange = (
      e: React.ChangeEvent<HTMLInputElement>,
      index: number,
   ) => {
      const value = e.target.value;

      // Only accept single digit numbers
      if (!/^\d*$/.test(value)) return;

      // Update OTP state
      const newOtp = [...otp];

      // Take only the last character if multiple characters are entered
      newOtp[index] = value.substring(value.length - 1);
      setOtp(newOtp);

      // Call onChange callback
      if (onChange) {
         onChange(newOtp.join(''));
      }

      // Auto-focus next input if value is entered
      if (value && index < length - 1 && inputRefs.current[index + 1]) {
         inputRefs.current[index + 1]?.focus();
      }

      // Check if OTP is complete
      const otpValue = newOtp.join('');
      if (otpValue.length === length && onComplete) {
         onComplete(otpValue);
      }
   };

   // Handle key down events (for backspace and arrow keys)
   const handleKeyDown = (
      e: KeyboardEvent<HTMLInputElement>,
      index: number,
   ) => {
      if (e.key === 'Backspace') {
         // If current input is empty, focus previous input
         if (!otp[index] && index > 0 && inputRefs.current[index - 1]) {
            inputRefs.current[index - 1]?.focus();

            // Clear previous input
            const newOtp = [...otp];
            newOtp[index - 1] = '';
            setOtp(newOtp);

            // Call onChange callback
            if (onChange) {
               onChange(newOtp.join(''));
            }

            e.preventDefault();
         }
      } else if (e.key === 'ArrowLeft' && index > 0) {
         inputRefs.current[index - 1]?.focus();
         e.preventDefault();
      } else if (e.key === 'ArrowRight' && index < length - 1) {
         inputRefs.current[index + 1]?.focus();
         e.preventDefault();
      }
   };

   // Handle paste event
   const handlePaste = (e: ClipboardEvent<HTMLInputElement>, index: number) => {
      e.preventDefault();
      const pastedData = e.clipboardData.getData('text/plain').trim();

      // Check if pasted data contains only digits
      if (!/^\d*$/.test(pastedData)) return;

      // Fill OTP fields with pasted data
      const newOtp = [...otp];
      for (let i = 0; i < Math.min(length - index, pastedData.length); i++) {
         newOtp[index + i] = pastedData[i];
      }

      setOtp(newOtp);

      // Focus the next empty input or the last input
      const nextEmptyIndex = newOtp.findIndex(
         (digit, i) => i >= index && !digit,
      );
      if (nextEmptyIndex !== -1 && nextEmptyIndex < length) {
         inputRefs.current[nextEmptyIndex]?.focus();
      } else {
         inputRefs.current[length - 1]?.focus();
      }

      // Call onChange callback
      if (onChange) {
         onChange(newOtp.join(''));
      }

      // Check if OTP is complete
      const otpValue = newOtp.join('');
      if (otpValue.length === length && onComplete) {
         onComplete(otpValue);
      }
   };

   return (
      <div className={`space-y-2 ${className}`}>
         <div className="flex justify-between gap-2">
            {Array(length)
               .fill(null)
               .map((_, index) => (
                  <motion.div
                     key={index}
                     initial={{ opacity: 0, y: 10 }}
                     animate={{ opacity: 1, y: 0 }}
                     transition={{ duration: 0.2, delay: index * 0.05 }}
                     className="relative flex-1"
                  >
                     <input
                        ref={(ref) => {
                           inputRefs.current[index] = ref;
                        }}
                        type="text"
                        inputMode="numeric"
                        pattern="\d*"
                        maxLength={1}
                        value={otp[index]}
                        onChange={(e) => handleChange(e, index)}
                        onKeyDown={(e) => handleKeyDown(e, index)}
                        onPaste={(e) => handlePaste(e, index)}
                        disabled={disabled}
                        className={`w-full h-14 text-center text-xl font-medium rounded-lg border ${
                           isError
                              ? 'border-red-500 bg-red-50'
                              : otp[index]
                                ? 'border-green-500 bg-green-50'
                                : 'border-gray-300'
                        } focus:outline-none focus:ring-2 ${
                           isError
                              ? 'focus:ring-red-200'
                              : 'focus:ring-blue-200'
                        } transition-all ${inputClassName}`}
                        aria-label={`Digit ${index + 1} of ${length}`}
                     />

                     {/* Animated underline effect */}
                     <motion.div
                        initial={{ scaleX: 0 }}
                        animate={{ scaleX: otp[index] ? 1 : 0 }}
                        className={`absolute bottom-0 left-0 right-0 h-1 rounded-b-lg ${
                           isError ? 'bg-red-500' : 'bg-green-500'
                        }`}
                        style={{ transformOrigin: 'center' }}
                     />
                  </motion.div>
               ))}
         </div>

         {isError && errorMessage && (
            <motion.p
               initial={{ opacity: 0 }}
               animate={{ opacity: 1 }}
               className="text-red-500 text-sm flex items-center"
            >
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
               {errorMessage}
            </motion.p>
         )}
      </div>
   );
}
