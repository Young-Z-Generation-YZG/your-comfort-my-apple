import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const fakeTenantsList = [
   {
      id: '664355f845e56534956be32b',
      name: 'Ware house',
      sub_domain: 'admin',
      description: '',
      tenant_type: 'WARE_HOUSE',
      tenant_state: 'ACTIVE',
      embedded_branch: {
         id: '664357a235e84033bbd0e6b6',
         tenant_id: '664355f845e56534956be32b',
         name: 'Ware house branch',
         address: 'Ware house address',
         description: null,
         manager: null,
         created_at: '2025-11-09T17:21:33.829Z',
         updated_at: '2025-11-09T17:21:33.829Z',
         updated_by: '',
         is_deleted: false,
         deleted_at: null,
         deleted_by: '',
      },
      created_at: '2025-11-09T17:21:33.83Z',
      updated_at: '2025-11-09T17:21:33.83Z',
      updated_by: '',
      is_deleted: false,
      deleted_at: null,
      deleted_by: '',
   },
   {
      id: '690e034dff79797b05b3bc89',
      name: 'HCM TD KVC 1060',
      sub_domain: 'hcm-td-kvc-1060',
      description: '',
      tenant_type: 'BRANCH',
      tenant_state: 'ACTIVE',
      embedded_branch: {
         id: '690e034dff79797b05b3bc88',
         tenant_id: '690e034dff79797b05b3bc89',
         name: 'HCM_TD_KVC_1060',
         address: 'Số 1060, Kha Vạn Cân, Linh Chiểu, TD',
         description: null,
         manager: null,
         created_at: '2025-11-09T17:21:33.83Z',
         updated_at: '2025-11-09T17:21:33.83Z',
         updated_by: '',
         is_deleted: false,
         deleted_at: null,
         deleted_by: '',
      },
      created_at: '2025-11-09T17:21:33.83Z',
      updated_at: '2025-11-09T17:21:33.83Z',
      updated_by: '',
      is_deleted: false,
      deleted_at: null,
      deleted_by: '',
   },
];

export type TTenant = {
   id: string;
   name: string;
   sub_domain: string;
   description: string;
   tenant_type: string;
   tenant_state: string;
   embedded_branch: TBranch;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TBranch = {
   id: string;
   tenant_id: string;
   name: string;
   address: string;
   description: string;
   manager: any;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('catalog-services')(args, api, extraOptions);

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
      getTenants: builder.query<TTenant[], void>({
         query: () => {
            return {
               url: '/api/v1/tenants',
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
   }),
});

export const { useLazyGetTenantsQuery, useLazyGetTenantByIdQuery } = tenantApi;
