import { useCallback, useMemo } from 'react';
import {
   useLazyGetIphoneModelsQuery,
   useLazyGetModelBySlugQuery,
} from '~/infrastructure/services/catalog.service';

const useCatalogService = () => {
   const [getIphoneModelsTrigger, getIphoneModelsState] =
      useLazyGetIphoneModelsQuery();
   const [getModelBySlugTrigger, getModelBySlugState] =
      useLazyGetModelBySlugQuery();

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

   const getModelBySlugAsync = useCallback(
      async (slug: string) => {
         try {
            const result = await getModelBySlugTrigger(slug).unwrap();

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
      [getModelBySlugTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getIphoneModelsState.isLoading ||
         getIphoneModelsState.isFetching ||
         getModelBySlugState.isLoading ||
         getModelBySlugState.isFetching
      );
   }, [
      getIphoneModelsState.isLoading,
      getIphoneModelsState.isFetching,
      getModelBySlugState.isLoading,
      getModelBySlugState.isFetching,
   ]);

   return {
      isLoading,
      getIphoneModelsState: getIphoneModelsState,
      getModelBySlugState: getModelBySlugState,

      getIphoneModelsAsync,
      getModelBySlugAsync,
   };
};

export default useCatalogService;
