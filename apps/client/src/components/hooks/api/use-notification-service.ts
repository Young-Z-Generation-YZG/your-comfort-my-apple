import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '@components/hooks/use-check-error';

import {
   useLazyGetNotificationsQuery,
   useMarkAsReadMutation,
   useMarkAllAsReadMutation,
   INotificationQueryParams,
} from '~/infrastructure/services/notification.service';

const useNotificationService = () => {
   const [getNotificationsTrigger, getNotificationsQueryState] =
      useLazyGetNotificationsQuery();

   const [markAsReadTrigger, markAsReadQueryState] = useMarkAsReadMutation();
   const [markAllAsReadTrigger, markAllAsReadQueryState] =
      useMarkAllAsReadMutation();

   useCheckApiError([
      {
         title: 'Failed to load notifications',
         error: getNotificationsQueryState.error,
      },
      {
         title: 'Failed to mark notification as read',
         error: markAsReadQueryState.error,
      },
      {
         title: 'Failed to mark all notifications as read',
         error: markAllAsReadQueryState.error,
      },
   ]);

   const getNotificationsAsync = useCallback(
      async (params?: INotificationQueryParams | null) => {
         try {
            const result = await getNotificationsTrigger(params ?? {}).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [getNotificationsTrigger],
   );

   const markAsReadAsync = useCallback(
      async (notificationId: string) => {
         try {
            const result = await markAsReadTrigger(notificationId).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return {
               isSuccess: false,
               isError: true,
               data: null,
               error,
            };
         }
      },
      [markAsReadTrigger],
   );

   const markAllAsReadAsync = useCallback(async () => {
      try {
         const result = await markAllAsReadTrigger().unwrap();
         return {
            isSuccess: true,
            isError: false,
            data: result,
            error: null,
         };
      } catch (error) {
         return {
            isSuccess: false,
            isError: true,
            data: null,
            error,
         };
      }
   }, [markAllAsReadTrigger]);

   const isLoading = useMemo(() => {
      return (
         getNotificationsQueryState.isLoading ||
         getNotificationsQueryState.isFetching ||
         markAsReadQueryState.isLoading ||
         markAllAsReadQueryState.isLoading
      );
   }, [
      getNotificationsQueryState.isLoading,
      getNotificationsQueryState.isFetching,
      markAsReadQueryState.isLoading,
      markAllAsReadQueryState.isLoading,
   ]);

   return {
      // states
      isLoading,
      getNotificationsQueryState,
      markAsReadQueryState,
      markAllAsReadQueryState,

      // actions
      getNotificationsAsync,
      markAsReadAsync,
      markAllAsReadAsync,
   };
};

export default useNotificationService;
