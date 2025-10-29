import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';

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
      getOrders: builder.query<PaginationResponse<any>, void>({
         query: () => ({
            url: '/api/v1/orders/users',
            method: 'GET',
         }),
      }),
      getOrderDetails: builder.query<any, string>({
         query: (orderId) => ({
            url: `/api/v1/orders/${orderId}/details`,
            method: 'GET',
         }),
      }),
   }),
});

export const { useLazyGetOrdersQuery, useLazyGetOrderDetailsQuery } =
   orderingApi;
