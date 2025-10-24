import { useCallback, useMemo } from 'react';
import { useLazyGetIphoneModelsAsyncQuery } from '~/infrastructure/services/catalog.service';

const useCatalogService = () => {
   const [getIphoneModelsTrigger, iphoneModelsQueryState] =
      useLazyGetIphoneModelsAsyncQuery();

   const getIphoneModelsAsync = useCallback(
      async (params: string) => {
         try {
            const result = await getIphoneModelsTrigger(params).unwrap();
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
      [getIphoneModelsTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         iphoneModelsQueryState.isLoading || iphoneModelsQueryState.isFetching
      );
   }, [iphoneModelsQueryState.isLoading, iphoneModelsQueryState.isFetching]);

   return {
      iphoneModelsState: iphoneModelsQueryState,
      getIphoneModelsAsync,
      isLoading,
   };
};

export default useCatalogService;
