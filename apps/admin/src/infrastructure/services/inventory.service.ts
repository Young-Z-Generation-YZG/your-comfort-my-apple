import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const inventoryApi = createApi({
   reducerPath: 'inventory-api',
   tagTypes: ['Inventory'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getWarehouses: builder.query<PaginationResponse<any>, any>({
         query: (params: any) => ({
            url: '/api/v1/inventory/skus',
            method: 'GET',
            params: params,
         }),
      }),
   }),
});

export const { useLazyGetWarehousesQuery } = inventoryApi;
