import { fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/infrastructure/config/env.config';
import { RootState } from '../redux/store';

export const baseQuery = (service: string) =>
   fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + service,
      prepareHeaders: (headers, { getState }) => {
         const accessToken = (getState() as RootState).auth.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
      paramsSerializer: (params: Record<string, any>) => {
         const searchParams = new URLSearchParams();

         Object.entries(params).forEach(([key, value]) => {
            if (value === null || value === undefined) {
               // Skip null/undefined values
               return;
            }

            if (Array.isArray(value)) {
               // For arrays, append each value separately
               value.forEach((item) => {
                  if (item !== null && item !== undefined) {
                     searchParams.append(key, String(item));
                  }
               });
            } else {
               searchParams.append(key, String(value));
            }
         });

         return searchParams.toString();
      },
   });
