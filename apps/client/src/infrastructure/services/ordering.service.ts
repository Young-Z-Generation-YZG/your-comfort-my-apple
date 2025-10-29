import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
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
      getOrders: builder.query<PaginationResponse<any>, void>({
         query: () => ({
            url: '/api/v1/orders',
            method: 'GET',
         }),
      }),
      getOrderDetails: builder.query<any, string>({
         query: (orderId) => ({
            url: `/api/v1/orders/${orderId}/details`,
            method: 'GET',
         }),
      }),
      vnpayIpnCallback: builder.mutation({
         query: (payload: any) => ({
            url: '/api/v1/orders/payment/vnpay-ipn-callback',
            method: 'PATCH',
            body: payload,
            providesTags: ['Orders'],
         }),
         transformResponse: (response: any) => {
            return response;
         },
         transformErrorResponse: (error: any) => {
            return error.data;
         },
      }),
      momoIpnCallback: builder.mutation({
         query: (payload: any) => ({
            url: '/api/v1/orders/payment/momo-ipn-callback',
            method: 'PATCH',
            body: payload,
            providesTags: ['Orders'],
         }),
         transformResponse: (response: any) => {
            return response;
         },
         transformErrorResponse: (error: any) => {
            return error.data;
         },
      }),
      confirmOrder: builder.mutation({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}/status/confirm`,
            method: 'PATCH',
         }),
      }),
      cancelOrder: builder.mutation({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}/status/cancel`,
            method: 'PATCH',
         }),
      }),
   }),
});

export const {
   useVnpayIpnCallbackMutation,
   useMomoIpnCallbackMutation,
   useConfirmOrderMutation,
   useCancelOrderMutation,
   useLazyGetOrdersQuery,
   useLazyGetOrderDetailsQuery,
} = orderingApi;
