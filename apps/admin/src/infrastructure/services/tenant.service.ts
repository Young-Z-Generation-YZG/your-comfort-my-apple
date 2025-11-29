import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TTenant } from '~/src/domain/types/catalog';

export interface ICreateTenantPayload {
   name: string;
   sub_domain: string;
   branch_address: string;
   tenant_type: string;
   tenant_description: string;
   branch_description: string;
}

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const tenantApi = createApi({
   reducerPath: 'tenant-api',
   tagTypes: ['Tenants'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getListTenants: builder.query<TTenant[], void>({
         query: () => {
            return {
               url: '/api/v1/tenants/list',
               method: 'GET',
            };
         },
         providesTags: ['Tenants'],
      }),
      getTenantById: builder.query<TTenant, string>({
         query: (id: string) => {
            return {
               url: `/api/v1/tenants/${id}`,
               method: 'GET',
            };
         },
         providesTags: ['Tenants'],
      }),
      createTenant: builder.mutation<boolean, ICreateTenantPayload>({
         query: (payload) => {
            return {
               url: '/api/v1/tenants',
               method: 'POST',
               body: payload,
            };
         },
         invalidatesTags: ['Tenants'],
      }),
   }),
});

export const {
   useLazyGetListTenantsQuery,
   useLazyGetTenantByIdQuery,
   useCreateTenantMutation,
} = tenantApi;
