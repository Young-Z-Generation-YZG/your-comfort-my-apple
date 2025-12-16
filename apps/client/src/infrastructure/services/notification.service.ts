import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '~/infrastructure/redux/features/auth.slice';
import { baseQuery } from './base-query';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { TNotification } from '~/domain/types/ordering.type';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';
import { INotificationQueryParams } from '~/domain/interfaces/ordering.interface';

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

export const notificationApi = createApi({
   reducerPath: 'order-notification-api',
   tagTypes: ['OrderNotifications'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getNotifications: builder.query<
         PaginationResponse<TNotification>,
         INotificationQueryParams
      >({
         query: (queryParams: INotificationQueryParams) => ({
            url: '/api/v1/orders/notifications',
            method: 'GET',
            params: queryParams,
         }),
         providesTags: ['OrderNotifications'],
      }),
      markAsRead: builder.mutation<boolean, string>({
         query: (notificationId: string) => ({
            url: `/api/v1/orders/notifications/${notificationId}/read`,
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
