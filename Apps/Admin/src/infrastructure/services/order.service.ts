import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import {
   OrderDetailsResponse,
   OrderResponse,
} from '~/src/domain/interfaces/orders/order.interface';
import { UpdateOrderStatusFormType } from '~/src/domain/schemas/order.schema';

export const orderApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://213f-116-108-46-152.ngrok-free.app/ordering-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getOrdersAsync: builder.query<PaginationResponse<OrderResponse>, void>({
         query: () => ({
            url: '/api/v1/orders/admin',
            method: 'GET',
         }),
      }),
      getOrderDetailsAsync: builder.query<OrderDetailsResponse, string>({
         query: (orderId) => ({
            url: `/api/v1/orders/${orderId}/details`,
            method: 'GET',
         }),
      }),
      updateOrderAsync: builder.mutation({
         query: (body: UpdateOrderStatusFormType) => ({
            url: `/api/v1/orders/admin/${body.order_id}/status?_updateStatus=${body.update_status}`,
            method: 'PATCH',
         }),
      }),
   }),
});

export const {
   useGetOrdersAsyncQuery,
   useUpdateOrderAsyncMutation,
   useGetOrderDetailsAsyncQuery,
} = orderApi;
