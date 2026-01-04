import { useCallback, useMemo } from 'react';
import { useLazyGetEventDetailsQuery } from '~/infrastructure/services/promotion.service';
import { useCheckApiError } from '~/hooks/use-check-error';

const usePromotionService = () => {
   const [getEventDetailsTrigger, getEventDetailsState] =
      useLazyGetEventDetailsQuery();

   useCheckApiError([]);

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
      return getEventDetailsState.isLoading || getEventDetailsState.isFetching;
   }, [getEventDetailsState.isLoading, getEventDetailsState.isFetching]);

   return {
      isLoading,
      getEventDetailsState,
      getEventDetailsAsync,
   };
};

export default usePromotionService;
