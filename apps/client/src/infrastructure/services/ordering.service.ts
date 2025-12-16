import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '~/infrastructure/redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TOrder } from '~/domain/types/ordering.type';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';
import {
   IOrderFilterQueryParams,
   IVnpayIpnCallbackPayload,
   IMomoIpnCallbackPayload,
} from '~/domain/interfaces/ordering.interface';

const baseQueryHandler = async (
   args: string | FetchArgs,
   api: BaseQueryApi,
   extraOptions: unknown,
) => {
   const result = await baseQuery('/ordering-services')(
      args,
      api,
      extraOptions as unknown as any,
   );

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export const orderingApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getOrders: builder.query<
         PaginationResponse<TOrder>,
         IOrderFilterQueryParams
      >({
         query: (queryParams: IOrderFilterQueryParams) => ({
            url: '/api/v1/orders/users',
            method: 'GET',
            params: queryParams,
         }),
      }),
      getOrderDetails: builder.query<TOrder, string>({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}`,
            method: 'GET',
         }),
      }),
      vnpayIpnCallback: builder.mutation<boolean, IVnpayIpnCallbackPayload>({
         query: (payload: IVnpayIpnCallbackPayload) => ({
            url: '/api/v1/orders/payment/vnpay-ipn-callback',
            method: 'PATCH',
            body: payload,
         }),
      }),
      momoIpnCallback: builder.mutation<boolean, IMomoIpnCallbackPayload>({
         query: (payload: IMomoIpnCallbackPayload) => ({
            url: '/api/v1/orders/payment/momo-ipn-callback',
            method: 'PATCH',
            body: payload,
         }),
      }),
      confirmOrder: builder.mutation<boolean, string>({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}/status/confirm`,
            method: 'PATCH',
         }),
      }),
      cancelOrder: builder.mutation<boolean, string>({
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
