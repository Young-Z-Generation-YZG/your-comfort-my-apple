import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TOrder } from '~/src/domain/types/ordering.type';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/ordering-services')(
      args,
      api,
      extraOptions,
   );

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export interface IBaseQueryParams {
   _page?: number;
   _limit?: number;
   _orderCode?: string | null;
   _customerEmail?: string | null;
   _orderStatus?: string[] | null;
   _paymentMethod?: string[] | null;
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
         providesTags: ['Orders'],
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
         providesTags: ['Orders'],
      }),
      getOrderDetails: builder.query<any, string>({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}`,
            method: 'GET',
         }),
         providesTags: ['Orders'],
      }),
      getUserOrdersDetails: builder.query<
         PaginationResponse<TOrder>,
         {
            userId: string;
            params: IBaseQueryParams;
         }
      >({
         query: ({ userId, params }) => ({
            url: `/api/v1/orders/users/${userId}`,
            method: 'GET',
            params: params,
         }),
         providesTags: ['Orders'],
      }),
      getRevenues: builder.query<TOrder[], void>({
         query: () => ({
            url: `/api/v1/orders/dashboard/revenues`,
            method: 'GET',
         }),
         providesTags: ['Orders'],
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
         providesTags: ['Orders'],
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
         providesTags: ['Orders'],
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
         invalidatesTags: ['Orders'],
      }),
   }),
});

export const {
   useLazyGetOrderDetailsQuery,
   useLazyGetOrdersByAdminQuery,
   useLazyGetRevenuesQuery,
   useLazyGetRevenuesByYearsQuery,
   useLazyGetRevenuesByTenantsQuery,
   useLazyGetOrdersQuery,
   useLazyGetUserOrdersDetailsQuery,
   useUpdateOnlineOrderStatusMutation,
} = orderingApi;
