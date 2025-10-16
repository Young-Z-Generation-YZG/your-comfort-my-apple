import { useEffect, useRef } from 'react';
import {
   parseFetchError,
   isClientErrorResponse,
   isServerErrorResponse,
   isUnknownErrorResponse,
} from '~/infrastructure/utils/http/fetch-error-handler';
import { toast } from 'sonner';

const styleObj = {
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

         if (parsedError) {
            shownErrorsRef.current.add(errorKey);

            if (isClientErrorResponse(parsedError)) {
               toast.error(title, {
                  description: parsedError.error.message,
                  style: styleObj,
                  cancel: cancelObj,
                  duration: 2000,
               });
            } else if (isServerErrorResponse(parsedError)) {
               toast.error('Server Busy', {
                  description: parsedError.title,
                  style: styleObj,
                  cancel: cancelObj,
                  duration: 2000,
               });
            } else if (isUnknownErrorResponse(parsedError)) {
               toast.error('Server Busy', {
                  description: parsedError.status as string,
                  style: styleObj,
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
