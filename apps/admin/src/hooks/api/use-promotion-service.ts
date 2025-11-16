import { useCallback, useMemo } from 'react';
import {
   useLazyGetEventsQuery,
   useLazyGetEventDetailsQuery,
} from '../../infrastructure/services/promotion.service';

export const usePromotionService = () => {
   const [getEventsTrigger, eventsState] = useLazyGetEventsQuery();
   const [getEventDetailsTrigger, eventDetailsState] =
      useLazyGetEventDetailsQuery();

   const getEventsAsync = useCallback(async () => {
      try {
         const result = await getEventsTrigger().unwrap();
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
   }, [getEventsTrigger]);

   const getEventDetailsAsync = useCallback(
      async (eventId: string) => {
         try {
            const result = await getEventDetailsTrigger(eventId).unwrap();
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
      [getEventDetailsTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         eventsState.isLoading ||
         eventsState.isFetching ||
         eventDetailsState.isLoading ||
         eventDetailsState.isFetching
      );
   }, [
      eventsState.isLoading,
      eventsState.isFetching,
      eventDetailsState.isLoading,
      eventDetailsState.isFetching,
   ]);

   return {
      isLoading,
      eventsState,
      eventDetailsState,
      getEventsAsync,
      getEventDetailsAsync,
   };
};
