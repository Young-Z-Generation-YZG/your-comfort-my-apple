'use client';

import { createContext, useContext, useState, type ReactNode } from 'react';
import { LoadingOverlay } from '../client/loading-overlay';

interface LoadingContextType {
   showLoading: (options?: LoadingOptions) => void;
   hideLoading: () => void;
   isLoading: boolean;
}

interface LoadingOptions {
   text?: string;
   variant?: 'spinner' | 'dots' | 'pulse' | 'apple';
   size?: 'sm' | 'md' | 'lg';
   color?: string;
}

const LoadingContext = createContext<LoadingContextType | undefined>(undefined);

export function LoadingProvider({ children }: { children: ReactNode }) {
   const [isLoading, setIsLoading] = useState(false);
   const [options, setOptions] = useState<LoadingOptions>({
      text: 'Loading...',
      variant: 'apple',
      size: 'lg',
      color: '#0066CC',
   });

   const showLoading = (newOptions?: LoadingOptions) => {
      if (newOptions) {
         setOptions({ ...options, ...newOptions });
      }
      setIsLoading(true);
   };

   const hideLoading = () => {
      setIsLoading(false);
   };

   return (
      <LoadingContext.Provider value={{ showLoading, hideLoading, isLoading }}>
         {children}
         <LoadingOverlay
            isLoading={isLoading}
            fullScreen={true}
            text={options.text}
            variant={options.variant}
            size={options.size}
            color={options.color}
         />
      </LoadingContext.Provider>
   );
}

export function useLoading() {
   const context = useContext(LoadingContext);
   if (context === undefined) {
      throw new Error('useLoading must be used within a LoadingProvider');
   }
   return context;
}
