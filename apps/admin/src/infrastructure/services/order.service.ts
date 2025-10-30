import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('ordering-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const orderingApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getOrdersByAdmin: builder.query<PaginationResponse<any>, any>({
         query: (params: any) => ({
            url: '/api/v1/orders/admin',
            method: 'GET',
            params: params,
         }),
      }),
      getOrderDetails: builder.query<any, string>({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}`,
            method: 'GET',
         }),
      }),
   }),
});

export const { useLazyGetOrderDetailsQuery, useLazyGetOrdersByAdminQuery } =
   orderingApi;
