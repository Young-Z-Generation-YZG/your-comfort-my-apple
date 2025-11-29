import { useEffect, useRef } from 'react';
import {
   parseFetchError,
   isClientErrorResponse,
   isServerErrorResponse,
   isUnknownErrorResponse,
   isValidationErrorResponse,
} from '~/infrastructure/utils/http/fetch-error-handler';
import { toast } from 'sonner';

const errorStyleObj = {
   backgroundColor: '#FEE2E2',
   color: '#991B1B',
   border: '1px solid #FCA5A5',
};

const cancelObj = {
   label: 'Close',
   onClick: () => {},
   actionButtonStyle: {
      backgroundColor: '#991B1B',
      color: '#FFFFFF',
   },
};

export const useCheckApiError = (
   errors: {
      title: string;
      error: unknown;
   }[],
) => {
   const shownErrorsRef = useRef<Set<string>>(new Set());

   useEffect(() => {
      errors.forEach(({ title, error }) => {
         if (!error) return;

         const errorKey = JSON.stringify(error);

         if (shownErrorsRef.current.has(errorKey)) return;

         const parsedError = parseFetchError(error);

         console.log({
            isClientErrorResponse: isClientErrorResponse(parsedError),
            isServerErrorResponse: isServerErrorResponse(parsedError),
            isValidationErrorResponse: isValidationErrorResponse(parsedError),
            isUnknownErrorResponse: isUnknownErrorResponse(parsedError),
         });

         console.log('parsedError', parsedError);

         if (parsedError) {
            shownErrorsRef.current.add(errorKey);

            if (isClientErrorResponse(parsedError)) {
               toast.error(title, {
                  description: parsedError.error.message,
                  style: errorStyleObj,
                  cancel: cancelObj,
                  duration: 2000,
               });
            } else if (isValidationErrorResponse(parsedError)) {
               toast.error('Validation failed', {
                  description: parsedError.errors
                     .map((error) => error.message)
                     .join(', '),
                  style: errorStyleObj,
                  cancel: cancelObj,
                  duration: 2000,
               });
            } else if (isServerErrorResponse(parsedError)) {
               toast.error('Server Busy', {
                  description: parsedError.title,
                  style: errorStyleObj,
                  cancel: cancelObj,
                  duration: 2000,
               });
            } else if (isUnknownErrorResponse(parsedError)) {
               toast.error('Server Busy', {
                  description: parsedError.status as string,
                  style: errorStyleObj,
                  cancel: cancelObj,
                  duration: 2000,
               });
            }
         }
      });
   }, [errors]);

   useEffect(() => {
      const hasError = errors.some(
         ({ error }) => error !== undefined && error !== null,
      );
      if (!hasError) {
         shownErrorsRef.current.clear();
      }
   }, [errors]);
};
