'use client';

import type { ReactNode } from 'react';
import { motion, AnimatePresence } from 'framer-motion';

interface LoadingOverlayProps {
   isLoading?: boolean;
   children?: ReactNode;
   variant?: 'spinner' | 'dots' | 'pulse' | 'apple';
   size?: 'sm' | 'md' | 'lg';
   color?: string;
   text?: string;
   blur?: boolean;
   fullScreen?: boolean;
   className?: string;
}
export const LoadingOverlay: React.FC<LoadingOverlayProps> = ({
   isLoading = true,
   children,
   variant = 'apple',
   size = 'lg',
   color = '#0066CC',
   text = 'Loading...',
   blur = false,
   fullScreen = false,
   className = '',
}: LoadingOverlayProps) => {
   // Size mappings
   const sizeMap = {
      sm: { container: 'h-4 w-4', text: 'text-xs' },
      md: { container: 'h-8 w-8', text: 'text-sm' },
      lg: { container: 'h-12 w-12', text: 'text-base' },
   };

   // If not loading and not fullScreen, just render children
   if (!isLoading && !fullScreen) {
      return <>{children}</>;
   }

   return (
      <div className={`relative ${className}`}>
         {children}

         <AnimatePresence>
            {isLoading && (
               <motion.div
                  initial={{ opacity: 0 }}
                  animate={{ opacity: 1 }}
                  exit={{ opacity: 0 }}
                  transition={{ duration: 0.2 }}
                  className={`${
                     fullScreen
                        ? 'fixed inset-0 z-[9999]'
                        : 'absolute inset-0 z-50'
                  } flex flex-col items-center justify-center bg-white/80 dark:bg-black/80 backdrop-blur-sm`}
               >
                  <div className="flex items-center justify-center">
                     <svg
                        className={`${sizeMap[size].container}`}
                        viewBox="0 0 100 100"
                        xmlns="http://www.w3.org/2000/svg"
                     >
                        {[
                           0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300,
                           330,
                        ].map((degree, i) => (
                           <motion.line
                              key={i}
                              x1="50"
                              y1="25"
                              x2="50"
                              y2="35"
                              strokeWidth="6"
                              strokeLinecap="round"
                              stroke={color}
                              transform={`rotate(${degree} 50 50)`}
                              initial={{ opacity: i === 0 ? 1 : 0.2 }}
                              animate={{
                                 opacity: [0.2, 1, 0.2],
                              }}
                              transition={{
                                 repeat: Number.POSITIVE_INFINITY,
                                 duration: 1.2,
                                 delay: i * 0.1,
                                 ease: 'easeInOut',
                              }}
                           />
                        ))}
                     </svg>
                  </div>

                  {text && (
                     <p
                        className={`mt-4 ${sizeMap[size].text} text-gray-700 select-none dark:text-gray-300`}
                     >
                        {text}
                     </p>
                  )}
               </motion.div>
            )}
         </AnimatePresence>
      </div>
   );
};
