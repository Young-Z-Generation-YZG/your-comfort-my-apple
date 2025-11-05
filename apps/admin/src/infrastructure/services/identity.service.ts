import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import envConfig from '../config/env.config';
import { RootState } from '../redux/store';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';

const identityBaseQuery = async (args: any, api: any, extraOptions: any) => {
   const state = api.getState() as RootState;
   const { currentUser, impersonatedUser } = state.auth;
   const { tenantId } = state.tenant;

   // Extract custom option from args
   const useSuperAdminToken = args.__useSuperAdminToken || false;

   // Choose token based on option
   let accessToken = null;
   if (impersonatedUser?.accessToken && currentUser?.accessToken) {
      accessToken = useSuperAdminToken
         ? currentUser?.accessToken
         : impersonatedUser?.accessToken;
   } else {
      accessToken = impersonatedUser?.accessToken || currentUser?.accessToken;
   }

   // Create base query with correct token
   const baseQueryFn = fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'identity-services',
      prepareHeaders: (headers) => {
         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }
         if (tenantId && !useSuperAdminToken) {
            headers.set('X-TenantId', tenantId);
         }
         headers.set('ngrok-skip-browser-warning', 'true');
         return headers;
      },
      paramsSerializer: (params: Record<string, any>) => {
         const searchParams = new URLSearchParams();

         Object.entries(params).forEach(([key, value]) => {
            if (Array.isArray(value)) {
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

   // Remove custom option before making request
   const { __useSuperAdminToken, ...cleanArgs } = args;
   const result = await baseQueryFn(cleanArgs, api, extraOptions);

   // Check for 401
   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export const identityApi = createApi({
   reducerPath: 'identity-api',
   tagTypes: ['Users'],
   baseQuery: identityBaseQuery,
   endpoints: (builder) => ({
      getUsersByAdmin: builder.query<PaginationResponse<any>, any>({
         query: (params: any) => ({
            url: '/api/v1/users/admin',
            method: 'GET',
            params,
            __useSuperAdminToken: params.__useSuperAdminToken,
         }),
         providesTags: ['Users'],
      }),
   }),
});

export const { useLazyGetUsersByAdminQuery } = identityApi;
