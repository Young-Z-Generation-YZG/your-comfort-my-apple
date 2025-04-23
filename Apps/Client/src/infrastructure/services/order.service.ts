import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { HttpErrorResponse } from '~/domain/interfaces/errors/error.interface';
import {
   IMomoIpnCallbackPayload,
   IVnpayIpnCallbackPayload,
} from '~/domain/interfaces/orders/ipn-callback.interface';
import {
   OrderDetailsResponse,
   OrderResponse,
} from '~/domain/interfaces/orders/order.interface';
import envConfig from '~/infrastructure/config/env.config';

export const orderApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'ordering-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getOrdersAsync: builder.query<PaginationResponse<OrderResponse>, void>({
         query: () => ({
            url: '/api/v1/orders/users',
            method: 'GET',
         }),
      }),
      getOrderDetailsAsync: builder.query<OrderDetailsResponse, string>({
         query: (orderId) => ({
            url: `/api/v1/orders/${orderId}/order-items`,
            method: 'GET',
         }),
      }),
      vnpayIpnCallbackAsync: builder.mutation({
         query: (payload: IVnpayIpnCallbackPayload) => ({
            url: '/api/v1/orders/payment/vnpay-ipn-callback',
            method: 'PATCH',
            body: payload,
            providesTags: ['Orders'],
         }),
         transformResponse: (response: OrderDetailsResponse) => {
            return response;
         },
         transformErrorResponse: (error: HttpErrorResponse) => {
            return error.data;
         },
      }),
      momoIpnCallbackAsync: builder.mutation({
         query: (payload: IMomoIpnCallbackPayload) => ({
            url: '/api/v1/orders/payment/momo-ipn-callback',
            method: 'PATCH',
            body: payload,
            providesTags: ['Orders'],
         }),
         transformResponse: (response: OrderDetailsResponse) => {
            return response;
         },
         transformErrorResponse: (error: HttpErrorResponse) => {
            return error.data;
         },
      }),
   }),
});

export const {
   useVnpayIpnCallbackAsyncMutation,
   useMomoIpnCallbackAsyncMutation,
   useGetOrdersAsyncQuery,
   useGetOrderDetailsAsyncQuery,
} = orderApi;
