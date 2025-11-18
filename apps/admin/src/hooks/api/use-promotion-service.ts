import { useCallback, useMemo } from 'react';
import {
   useLazyGetEventsQuery,
   useLazyGetEventDetailsQuery,
   useCreateEventMutation,
   useUpdateEventMutation,
   ICreateEventPayload,
   IUpdateEventPayload,
} from '../../infrastructure/services/promotion.service';

export const usePromotionService = () => {
   const [getEventsTrigger, eventsState] = useLazyGetEventsQuery();
   const [getEventDetailsTrigger, eventDetailsState] =
      useLazyGetEventDetailsQuery();
   const [createEventTrigger, createEventState] = useCreateEventMutation();
   const [updateEventTrigger, updateEventState] = useUpdateEventMutation();

   const createEventAsync = useCallback(
      async (payload: ICreateEventPayload) => {
         try {
            const result = await createEventTrigger(payload).unwrap();
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
      [createEventTrigger],
   );

   const updateEventAsync = useCallback(
      async (eventId: string, payload: IUpdateEventPayload) => {
         try {
            const result = await updateEventTrigger({
               eventId,
               payload,
            }).unwrap();
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
      [updateEventTrigger],
   );

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
         eventDetailsState.isFetching ||
         createEventState.isLoading ||
         updateEventState.isLoading
      );
   }, [
      eventsState.isLoading,
      eventsState.isFetching,
      eventDetailsState.isLoading,
      eventDetailsState.isFetching,
      createEventState.isLoading,
      updateEventState.isLoading,
   ]);

   return {
      isLoading,
      eventsState,
      eventDetailsState,
      createEventState,
      updateEventState,
      getEventsAsync,
      getEventDetailsAsync,
      createEventAsync,
      updateEventAsync,
   };
};
