'use client';

import { useCallback, useMemo } from 'react';
import {
   IGetReviewsQueryParams,
   useLazyGetReviewByProductModelIdAsyncQuery,
} from '~/src/infrastructure/services/review.service';
import { useCheckApiError } from '~/src/hooks/use-check-error';

const useReviewService = () => {
   const [triggerGetReviews, getReviewsState] =
      useLazyGetReviewByProductModelIdAsyncQuery();

   useCheckApiError([
      { title: 'Load reviews failed', error: getReviewsState.error },
   ]);

   const getReviewsByProductModelIdAsync = useCallback(
      async (id: string, queryParams: IGetReviewsQueryParams) => {
         try {
            const result = await triggerGetReviews({
               id,
               queryParams,
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
      [triggerGetReviews],
   );

   const isLoading = useMemo(
      () => getReviewsState.isLoading || getReviewsState.isFetching,
      [getReviewsState.isLoading, getReviewsState.isFetching],
   );

   return {
      isLoading,
      getReviewsState,
      getReviewsByProductModelIdAsync,
   };
};

export default useReviewService;


