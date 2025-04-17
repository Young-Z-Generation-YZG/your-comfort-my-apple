import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { HttpErrorResponse } from '~/domain/interfaces/errors/error.interface';
import { IIpnCallbackPayload } from '~/domain/interfaces/orders/ipn-callback.interface';
import { OrderDetailsResponse } from '~/domain/interfaces/orders/order.interface';

export const orderApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://be2c-116-108-46-152.ngrok-free.app/ordering-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      ipnCallbackAsync: builder.mutation({
         query: (payload: IIpnCallbackPayload) => ({
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
   }),
});

export const { useIpnCallbackAsyncMutation } = orderApi;
