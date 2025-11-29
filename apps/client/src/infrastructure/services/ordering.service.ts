import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

export type TOrder = {
   tenant_id: string | null;
   branch_id: string | null;
   order_id: string;
   customer_id: string;
   customer_email: string;
   order_code: string;
   status: string;
   payment_method: string;
   shipping_address: {
      contact_name: string;
      contact_email: string;
      contact_phone_number: string;
      contact_address_line: string;
      contact_district: string;
      contact_province: string;
      contact_country: string;
   };
   order_items: TOrderItem[];
   promotion_id: string | null;
   promotion_type: string | null;
   discount_type: string | null;
   discount_value: number | null;
   discount_amount: number | null;
   total_amount: 1000;
   created_at: string;
   updated_at: string;
   updated_by: null;
   is_deleted: boolean;
   deleted_at: null;
   deleted_by: string | null;
};

export type TOrderItem = {
   order_item_id: string;
   order_id: string;
   tenant_id: string | null;
   branch_id: string | null;
   sku_id: string | null;
   model_id: string;
   model_name: string;
   color_name: string;
   storage_name: string;
   unit_price: number;
   display_image_url: string;
   model_slug: string;
   quantity: number;
   is_reviewed: boolean;
   promotion_id: string | null;
   promotion_type: string | null;
   discount_type: string | null;
   discount_value: number | null;
   discount_amount: number | null;
   sub_total_amount: number;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export interface IBaseQueryParams {
   _page?: number;
   _limit?: number;
}

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/ordering-services')(
      args,
      api,
      extraOptions,
   );

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
      getOrders: builder.query<PaginationResponse<TOrder>, IBaseQueryParams>({
         query: (params?: IBaseQueryParams | null) => ({
            url: '/api/v1/orders/users',
            method: 'GET',
            params: params ?? {},
         }),
      }),
      getOrderDetails: builder.query<TOrder, string>({
         query: (orderId) => ({
            url: `/api/v1/orders/${orderId}`,
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
