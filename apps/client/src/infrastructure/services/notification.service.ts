import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { TNotification } from '~/domain/types/ordering.type';

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
   }),
});

export const {
   useLazyGetNotificationsQuery,
   useMarkAsReadMutation,
   useMarkAllAsReadMutation,
} = notificationApi;
