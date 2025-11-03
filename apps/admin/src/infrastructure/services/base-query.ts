import { fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/src/infrastructure/config/env.config';
import { RootState } from '../redux/store';

export const baseQuery = (service: string) =>
   fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + service,
      prepareHeaders: (headers, { getState }) => {
         const { currentUser, impersonatedUser } = (getState() as RootState)
            .auth;

         const { tenantId, branchId, tenantSubDomain } = (
            getState() as RootState
         ).tenant;

         const accessToken =
            impersonatedUser?.accessToken || currentUser?.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         console.log('tenantId', tenantId);

         if (tenantId) {
            headers.set('X-TenantId', tenantId);
            // headers.set('X-BranchId', branchId);
            // headers.set('X-TenantSubDomain', tenantSubDomain);
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
