import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TNotification } from '~/src/domain/types/ordering';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';

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

export interface INotificationQueryParams {
   _page?: number | null;
   _limit?: number | null;
   _types?: string[] | null;
   _statuses?: string[] | null;
   _isRead?: boolean | null;
}

export const notificationApi = createApi({
   reducerPath: 'order-notification-api',
   tagTypes: ['OrderNotifications'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getNotifications: builder.query<
         PaginationResponse<TNotification>,
         INotificationQueryParams
      >({
         query: (params: INotificationQueryParams) => ({
            url: '/api/v1/orders/notifications',
            method: 'GET',
            params: params,
         }),
         providesTags: ['OrderNotifications'],
      }),
      markAsRead: builder.mutation<boolean, string>({
         query: (id: string) => ({
            url: `/api/v1/orders/notifications/${id}/read`,
            method: 'PUT',
         }),
         invalidatesTags: ['OrderNotifications'],
      }),
      markAllAsRead: builder.mutation<boolean, void>({
         query: () => ({
            url: `/api/v1/orders/notifications/read-all`,
            method: 'PUT',
         }),
         invalidatesTags: ['OrderNotifications'],
      }),
      //   getListTenants: builder.query<TTenant[], void>({
      //      query: () => {
      //         return {
      //            url: '/api/v1/tenants/list',
      //            method: 'GET',
      //         };
      //      },
      //      providesTags: ['Tenants'],
      //   }),
      //   getTenantById: builder.query<TTenant, string>({
      //      query: (id: string) => {
      //         return {
      //            url: `/api/v1/tenants/${id}`,
      //            method: 'GET',
      //         };
      //      },
      //      providesTags: ['Tenants'],
      //   }),
      //   createTenant: builder.mutation<boolean, ICreateTenantPayload>({
      //      query: (payload) => {
      //         return {
      //            url: '/api/v1/tenants',
      //            method: 'POST',
      //            body: payload,
      //         };
      //      },
      //      invalidatesTags: ['Tenants'],
      //   }),
   }),
});

export const {
   useLazyGetNotificationsQuery,
   useMarkAsReadMutation,
   useMarkAllAsReadMutation,
} = notificationApi;
