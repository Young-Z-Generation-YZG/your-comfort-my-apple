import { fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/src/infrastructure/config/env.config';
import { RootState } from '../redux/store';

export const baseQuery = (service: string) =>
   fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + service,
      prepareHeaders: (headers, { getState }) => {
         const { currentUser, impersonatedUser } = (getState() as RootState)
            .auth;

         const { tenantId } = (getState() as RootState).tenant;

         // Use impersonated user token if available, otherwise current user
         const accessToken =
            impersonatedUser?.accessToken || currentUser?.accessToken;

         console.log('accessToken', accessToken);
         console.log('currentUser', currentUser);
         console.log('impersonatedUser', impersonatedUser);

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         if (tenantId) {
            headers.set('X-TenantId', tenantId);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
      paramsSerializer: (params: Record<string, any>) => {
         const searchParams = new URLSearchParams();

         Object.entries(params).forEach(([key, value]) => {
            if (Array.isArray(value)) {
               // For arrays, append each value separately
               value.forEach((item) => {
                  if (item !== null && item !== undefined) {
                     searchParams.append(key, String(item));
                  }
               });
            } else if (value !== null && value !== undefined) {
               searchParams.append(key, String(value));
            }
         });

         return searchParams.toString();
      },
   });
