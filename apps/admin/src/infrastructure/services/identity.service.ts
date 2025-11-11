import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import envConfig from '../config/env.config';
import { RootState } from '../redux/store';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';

const fakeUsersList = [
   {
      id: '65dad719-7368-4d9f-b623-f308299e9575',
      tenant_id: '690b6214ed407c59d0537d18',
      branch_id: null,
      tenant_code: null,
      user_name: 'admin@gmail.com',
      normalized_user_name: 'ADMIN@GMAIL.COM',
      email: 'admin@gmail.com',
      normalized_email: 'ADMIN@GMAIL.COM',
      email_confirmed: true,
      phone_number: '0333284890',
      profile: null,
      created_at: '0001-01-01T00:00:00',
      updated_at: '0001-01-01T00:00:00',
      updated_by: null,
      is_deleted: false,
      deleted_at: null,
      deleted_by: null,
   },
   {
      id: 'e79d0b6f-af5a-4162-a6fd-8194d5a5f616',
      tenant_id: '690b6214ed407c59d0537d18',
      branch_id: null,
      tenant_code: null,
      user_name: 'staff@gmail.com',
      normalized_user_name: 'STAFF@GMAIL.COM',
      email: 'staff@gmail.com',
      normalized_email: 'STAFF@GMAIL.COM',
      email_confirmed: true,
      phone_number: '0333284890',
      profile: null,
      created_at: '0001-01-01T00:00:00',
      updated_at: '0001-01-01T00:00:00',
      updated_by: null,
      is_deleted: false,
      deleted_at: null,
      deleted_by: null,
   },
];

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
