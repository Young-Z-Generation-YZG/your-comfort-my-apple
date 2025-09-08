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
import { RootState } from '../redux/store';
import { setLogout } from '../redux/features/auth.slice';

const baseQueryWithAuth = fetchBaseQuery({
   baseUrl: envConfig.API_ENDPOINT + 'ordering-services',
   prepareHeaders: (headers, { getState }) => {
      const accessToken = (getState() as RootState).auth.value.accessToken;

      if (accessToken) {
         headers.set('Authorization', `Bearer ${accessToken}`);
      }

      headers.set('ngrok-skip-browser-warning', 'true');

      return headers;
   },
   responseHandler: (response) => {
      return response.json();
   },
});

const baseQueryWithUnauthorizedHandler = async (
   args: any,
   api: any,
   extraOptions: any,
) => {
   const result = await baseQueryWithAuth(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const orderApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: baseQueryWithUnauthorizedHandler,
   endpoints: (builder) => ({
      getOrdersAsync: builder.query<PaginationResponse<OrderResponse>, void>({
         query: () => ({
            url: '/api/v1/orders/users',
            method: 'GET',
         }),
      }),
      getOrderDetailsAsync: builder.query<OrderDetailsResponse, string>({
         query: (orderId) => ({
            url: `/api/v1/orders/${orderId}/details`,
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
      confirmOrderAsync: builder.mutation({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}/status/confirm`,
            method: 'PATCH',
         }),
      }),
      cancelOrderAsync: builder.mutation({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}/status/cancel`,
            method: 'PATCH',
         }),
      }),
   }),
});

export const {
   useVnpayIpnCallbackAsyncMutation,
   useMomoIpnCallbackAsyncMutation,
   useGetOrdersAsyncQuery,
   useGetOrderDetailsAsyncQuery,
   useConfirmOrderAsyncMutation,
   useCancelOrderAsyncMutation,
} = orderApi;
