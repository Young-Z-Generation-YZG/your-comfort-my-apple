import { useCallback, useMemo } from 'react';
import { useLazyGetEventWithItemsQuery } from '~/infrastructure/services/promotion.service';

const usePromotionService = () => {
   const [getEventWithItemsTrigger, getEventWithItemsState] =
      useLazyGetEventWithItemsQuery();

   const getEventWithItemsAsync = useCallback(async () => {
      try {
         const result = await getEventWithItemsTrigger().unwrap();
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
   }, [getEventWithItemsTrigger]);

   const isLoading = useMemo(() => {
      return (
         getEventWithItemsState.isLoading || getEventWithItemsState.isFetching
      );
   }, [getEventWithItemsState.isLoading, getEventWithItemsState.isFetching]);

   return {
      isLoading,
      getEventWithItemsState,

      getEventWithItemsAsync,
   };
};

export default usePromotionService;
