import { FetchBaseQueryError } from '@reduxjs/toolkit/query';

export const isFetchBaseQueryError = (
   error: unknown,
): error is FetchBaseQueryError => {
   if (typeof error === 'object' && error !== null && 'error' in error) {
      const nestedError = error.error;

      return (
         typeof nestedError === 'object' &&
         nestedError !== null &&
         'status' in nestedError &&
         'error' in nestedError
      );
   }

   return false;
};
