import { fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/src/infrastructure/config/env.config';
import { RootState } from '../redux/store';

export const baseQuery = (service: string) =>
   fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + service,
      prepareHeaders: (headers, { getState, endpoint }) => {
         const { currentUser, impersonatedUser, currentUserKey } = (
            getState() as RootState
         ).auth;

         console.log('[Fn:baseQuery]::ENDPOINT: ', endpoint);

         const isImpersonating = currentUserKey === 'impersonatedUser';

         // Check if Authorization header is already set (e.g., by logout mutation)
         // If not set, determine which token to use
         if (!headers.get('Authorization')) {
            // Check if this is a logout request - use refreshToken instead of accessToken
            const isLogoutRequest =
               endpoint === 'logout' ||
               (typeof endpoint === 'string' && endpoint.includes('logout'));

            if (isLogoutRequest) {
               // For logout, use refreshToken
               const refreshToken = currentUser?.refreshToken;

               if (refreshToken) {
                  headers.set('Authorization', `Bearer ${refreshToken}`);
               }
            } else {
               // For all other requests, use accessToken
               const accessToken = isImpersonating
                  ? impersonatedUser?.accessToken
                  : currentUser?.accessToken;

               if (accessToken) {
                  headers.set('Authorization', `Bearer ${accessToken}`);
               }
            }
         }

         const { tenantId } = (getState() as RootState).tenant;

         console.log('TENANT ID: ', tenantId);

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
