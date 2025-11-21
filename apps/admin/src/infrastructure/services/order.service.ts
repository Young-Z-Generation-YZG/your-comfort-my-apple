import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('ordering-services')(args, api, extraOptions);

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
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
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

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
   promotion_id: null;
   promotion_type: string | null;
   discount_type: string | null;
   discount_value: number | null;
   discount_amount: number | null;
   total_amount: number;
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
   _orderStatus?: string[] | null;
}

export interface IUpdateOnlineOrderStatusPayload {
   update_status: string;
}

export const orderingApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getOrdersByAdmin: builder.query<
         PaginationResponse<TOrder>,
         IBaseQueryParams
      >({
         query: (params: IBaseQueryParams) => ({
            url: '/api/v1/orders/online',
            method: 'GET',
            params: {
               _page: params._page ?? 1,
               _limit: params._limit ?? 10,
               ...params,
            },
         }),
      }),
      getOrders: builder.query<PaginationResponse<TOrder>, IBaseQueryParams>({
         query: (params: IBaseQueryParams) => ({
            url: '/api/v1/orders',
            method: 'GET',
            params: {
               _page: params._page ?? 1,
               _limit: params._limit ?? 10,
               ...params,
            },
         }),
      }),
      getOrderDetails: builder.query<any, string>({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}`,
            method: 'GET',
         }),
      }),
      getRevenues: builder.query<TOrder[], void>({
         query: () => ({
            url: `/api/v1/orders/dashboard/revenues`,
            method: 'GET',
         }),
      }),
      getRevenuesByYears: builder.query<
         { groups: Record<string, TOrder[]> },
         { _years: string[] }
      >({
         query: (params) => ({
            url: `/api/v1/orders/dashboard/revenues/years`,
            method: 'GET',
            params: {
               _years: params?._years || [],
            },
         }),
      }),
      getRevenuesByTenants: builder.query<
         { groups: Record<string, TOrder[]> },
         { _tenants: string[] }
      >({
         query: (params) => {
            const tenantIds = (params?._tenants || []).filter(
               (id): id is string => Boolean(id) && id !== 'undefined',
            );
            return {
               url: `/api/v1/orders/dashboard/revenues/tenants`,
               method: 'GET',
               params: {
                  _tenants: tenantIds,
               },
            };
         },
      }),
      updateOnlineOrderStatus: builder.mutation<
         boolean,
         {
            order_id: string;
            payload: IUpdateOnlineOrderStatusPayload;
         }
      >({
         query: ({ order_id, payload }) => ({
            url: `/api/v1/orders/online/${order_id}/status`,
            method: 'PATCH',
            body: payload,
         }),
      }),
   }),
});

export const {
   useLazyGetOrderDetailsQuery,
   useLazyGetOrdersByAdminQuery,
   useLazyGetRevenuesQuery,
   useLazyGetRevenuesByYearsQuery,
   useLazyGetRevenuesByTenantsQuery,
   useUpdateOnlineOrderStatusMutation,
   useLazyGetOrdersQuery,
} = orderingApi;
