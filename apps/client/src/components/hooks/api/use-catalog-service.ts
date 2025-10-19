import { useCallback, useMemo } from 'react';
import { useLazyGetModelsAsyncQuery } from '~/infrastructure/services/catalog.service';

const useCatalogService = () => {
   const [getModelsTrigger, modelsQueryState] = useLazyGetModelsAsyncQuery();

   const getModelsAsync = useCallback(
      async (params: string) => {
         try {
            const result = await getModelsTrigger(params).unwrap();
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
      [getModelsTrigger],
   );

   const isLoading = useMemo(() => {
      return modelsQueryState.isLoading || modelsQueryState.isFetching;
   }, [modelsQueryState.isLoading, modelsQueryState.isFetching]);

   return {
      modelsState: modelsQueryState,
      getModelsAsync,
      isLoading,
   };
};

export default useCatalogService;
