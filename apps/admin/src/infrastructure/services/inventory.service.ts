import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TSku } from '~/src/domain/types/catalog.type';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export interface IGetWarehousesQueryParams {
   _page?: number | null;
   _limit?: number | null;
   _colors?: string[] | null;
   _storages?: string[] | null;
   _models?: string[] | null;
   // Dynamic filters for stock
   _stock?: number | null;
   _stockOperator?: string | null; // ">=", ">", "<", "<=", "==", "!=", "in"
   // Dynamic filters for sold
   _sold?: number | null;
   _soldOperator?: string | null; // ">=", ">", "<", "<=", "==", "!=", "in"
}

export const inventoryApi = createApi({
   reducerPath: 'inventory-api',
   tagTypes: ['Inventory'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getWarehouses: builder.query<
         PaginationResponse<TSku>,
         IGetWarehousesQueryParams
      >({
         query: (params: IGetWarehousesQueryParams) => ({
            url: '/api/v1/inventory/skus',
            method: 'GET',
            params: params,
         }),
         providesTags: ['Inventory'],
      }),
      getSkuById: builder.query<TSku, string>({
         query: (id: string) => ({
            url: `/api/v1/inventory/skus/${id}`,
            method: 'GET',
         }),
         providesTags: (result, error, id) => [{ type: 'Inventory', id }],
      }),
      getSkuByIdWithImage: builder.query<TSku, string>({
         query: (id: string) => ({
            url: `/api/v1/inventory/skus/${id}/with-image`,
            method: 'GET',
         }),
         providesTags: (result, error, id) => [{ type: 'Inventory', id }],
      }),
   }),
});

export const {
   useLazyGetWarehousesQuery,
   useLazyGetSkuByIdQuery,
   useLazyGetSkuByIdWithImageQuery,
} = inventoryApi;
