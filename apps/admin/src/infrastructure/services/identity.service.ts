import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { baseQuery } from './base-query';
import { setLogout } from '../redux/features/auth.slice';

export type TUser = {
   id: string;
   tenant_id: string;
   branch_id: string;
   tenant_code: string;
   user_name: string;
   normalized_user_name: string;
   email: string;
   normalized_email: string;
   email_confirmed: boolean;
   phone_number: string;
   profile: TUserProfile;
   created_at: string;
   updated_at: string;
   updated_by: string;
   is_deleted: boolean;
   deleted_at: string;
   deleted_by: string;
};

export type TUserProfile = {
   id: string;
   user_id: string;
   first_name: string;
   last_name: string;
   birth_day: string;
   gender: string;
   image_id: string;
   image_url: string | null;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

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
            params,
            // __useSuperAdminToken: params.__useSuperAdminToken,
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
         providesTags: ['Users'],
      }),
   }),
});

export const { useLazyGetUsersByAdminQuery, useLazyGetListUsersQuery } =
   identityApi;
