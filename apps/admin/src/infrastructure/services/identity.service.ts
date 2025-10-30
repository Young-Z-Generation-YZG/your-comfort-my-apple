import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('identity-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const identityApi = createApi({
   reducerPath: 'identity-api',
   tagTypes: ['Users'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getUsersByAdmin: builder.query<PaginationResponse<any>, any>({
         query: (params: any) => ({
            url: '/api/v1/users/admin',
            method: 'GET',
            params: params,
         }),
         providesTags: ['Users'],
      }),
   }),
});

export const { useLazyGetUsersByAdminQuery } = identityApi;
