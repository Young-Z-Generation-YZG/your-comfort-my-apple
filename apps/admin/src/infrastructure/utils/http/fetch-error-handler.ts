import { FetchBaseQueryError } from '@reduxjs/toolkit/query';

export type ClientErrorResponse = {
   title: string;
   path: string;
   status: number;
   error: {
      code: string;
      message: string;
   };
   trace_id: string;
   type: string;
};

export type ServerErrorResponse = {
   title: string;
   path: string;
   status: number;
   trace_id: string;
   type: string;
};

export type UnknownErrorResponse = {
   status: unknown;
   data: unknown;
   error: unknown;
};

export const isClientErrorResponse = (
   error: unknown,
): error is ClientErrorResponse => {
   return (
      typeof error === 'object' &&
      error !== null &&
      'title' in error &&
      'path' in error &&
      'status' in error &&
      'error' in error &&
      'trace_id' in error &&
      'type' in error
   );
};

export const isServerErrorResponse = (
   error: unknown,
): error is ServerErrorResponse => {
   return (
      typeof error === 'object' &&
      error !== null &&
      'status' in error &&
      'title' in error &&
      !('error' in error)
   );
};

export const isUnknownErrorResponse = (
   error: unknown,
): error is UnknownErrorResponse => {
   return (
      typeof error === 'object' &&
      error !== null &&
      'status' in error &&
      ('data' in error || 'error' in error)
   );
};

export const parseFetchError = (
   error: unknown,
): ClientErrorResponse | ServerErrorResponse | UnknownErrorResponse | null => {
   if (!isFetchError(error)) {
      return null;
   }

   if (isClientErrorResponse(error.data)) {
      return error.data as ClientErrorResponse;
   }

   if (isServerErrorResponse(error.data)) {
      return error.data as ServerErrorResponse;
   }

   // Fallback to UnknownErrorResponse
   return {
      status: error.status,
      data: 'data' in error ? error.data : undefined,
      error: 'error' in error ? error.error : undefined,
   } as UnknownErrorResponse;
};

const isFetchError = (error: unknown): error is FetchBaseQueryError => {
   return (
      typeof error === 'object' &&
      error !== null &&
      'status' in error &&
      ('data' in error || 'error' in error)
   );
};
