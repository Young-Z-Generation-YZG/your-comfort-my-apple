import { useEffect, useRef } from 'react';
import { useToast } from '@components/hooks/use-toast';

/**
 * Custom hook for handling API errors with toast notifications
 *
 * Best practices:
 * - Prevents duplicate toasts for the same error
 * - Extracts error messages from various error formats
 * - Configurable toast options
 */

interface UseApiErrorHandlerOptions {
   title?: string;
   description?: string;
   showToast?: boolean;
}

export const useApiErrorHandler = (
   isError: boolean,
   error: unknown,
   options: UseApiErrorHandlerOptions = {},
) => {
   const { toast } = useToast();
   const hasShownErrorRef = useRef(false);

   const {
      title = 'Error',
      description = 'An error occurred. Please try again.',
      showToast = true,
   } = options;

   useEffect(() => {
      // Only show toast once per error
      if (isError && showToast && !hasShownErrorRef.current) {
         hasShownErrorRef.current = true;

         // Extract error message from various error formats
         const errorMessage = extractErrorMessage(error, description);

         toast({
            title,
            description: errorMessage,
            variant: 'destructive',
         });
      }

      // Reset when error is cleared
      if (!isError) {
         hasShownErrorRef.current = false;
      }
   }, [isError, error, toast, title, description, showToast]);
};

/**
 * Extract error message from various error formats
 */
const extractErrorMessage = (error: unknown, fallback: string): string => {
   if (!error) return fallback;

   // RTK Query error format
   if (typeof error === 'object' && error !== null) {
      if (
         'data' in error &&
         typeof error.data === 'object' &&
         error.data !== null
      ) {
         if ('message' in error.data) {
            return String(error.data.message);
         }
         if ('error' in error.data) {
            return String(error.data.error);
         }
      }

      if ('message' in error) {
         return String(error.message);
      }

      if ('status' in error) {
         const status = error.status;
         switch (status) {
            case 400:
               return 'Invalid request. Please check your filters.';
            case 401:
               return 'Unauthorized. Please sign in.';
            case 403:
               return 'Access forbidden.';
            case 404:
               return 'Products not found.';
            case 500:
               return 'Server error. Please try again later.';
            case 503:
               return 'Service unavailable. Please try again later.';
            default:
               return `Request failed with status ${status}`;
         }
      }
   }

   return fallback;
};
