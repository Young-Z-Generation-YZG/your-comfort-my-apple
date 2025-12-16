import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { baseQuery } from './base-query';
import { TUser, IAddNewStaffPayload } from '~/src/domain/types/identity.type';
import { setLogout } from '../redux/features/auth.slice';

// Domain entity types are defined in ~/src/domain/types. Use T-prefixed types there.

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/identity-services')(
      args,
      api,
      extraOptions,
   );

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

// Moved payload types to domain/types/identity.type.ts
// import interfaces instead of local type declarations

export const identityApi = createApi({
   reducerPath: 'identity-api',
   tagTypes: ['Users', 'UserSwitcher'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getUsersByAdmin: builder.query<PaginationResponse<TUser>, any>({
         query: (params: any) => ({
            url: '/api/v1/users/admin',
            method: 'GET',
            params,
            // __useSuperAdminToken: params.__useSuperAdminToken,
         }),
         providesTags: ['Users'],
      }),
      getUsers: builder.query<PaginationResponse<TUser>, any>({
         query: (params: any) => ({
            url: '/api/v1/users',
            method: 'GET',
            params,
         }),
         providesTags: ['Users'],
      }),
      getListUsers: builder.query<TUser[], { roles: string[] }>({
         query: (params: { roles: string[] }) => ({
            url: '/api/v1/users/list',
            method: 'GET',
            params: {
               _roles: params.roles,
            },
         }),
         providesTags: ['UserSwitcher'],
      }),
      addNewStaff: builder.mutation<boolean, IAddNewStaffPayload>({
         query: (data) => ({
            url: '/api/v1/auth/add-new-staff',
            method: 'POST',
            body: data,
         }),
         invalidatesTags: ['Users', 'UserSwitcher'],
      }),
   }),
});

export const {
   useLazyGetUsersByAdminQuery,
   useLazyGetListUsersQuery,
   useLazyGetUsersQuery,
   useAddNewStaffMutation,
} = identityApi;
