'use client';

import { useCallback, useMemo } from 'react';
import { useLazyGetImagesAsyncQuery } from '~/src/infrastructure/services/upload.service';
import { useCheckApiError } from '~/src/hooks/use-check-error';

export const useUploadService = () => {
   const [triggerGetImages, getImagesState] = useLazyGetImagesAsyncQuery();

   useCheckApiError([
      { title: 'Load images failed', error: getImagesState.error },
   ]);

   const getImagesAsync = useCallback(async () => {
      try {
         const data = await triggerGetImages().unwrap();
         return {
            isSuccess: true,
            isError: false,
            data,
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
   }, [triggerGetImages]);

   const isLoading = useMemo(
      () => getImagesState.isLoading || getImagesState.isFetching,
      [getImagesState.isLoading, getImagesState.isFetching],
   );

   return {
      isLoading,
      getImagesState,
      getImagesAsync,
   };
};

export default useUploadService;

