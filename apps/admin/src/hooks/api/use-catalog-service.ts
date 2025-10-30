import { useCallback, useMemo } from 'react';
import {
   useLazyGetIphoneModelsQuery,
   useLazyGetIphoneModelBySlugQuery,
} from '~/src/infrastructure/services/catalog.service';

const useCatalogService = () => {
   const [getIphoneModelsTrigger, getIphoneModelsState] =
      useLazyGetIphoneModelsQuery();
   const [getIphoneModelBySlugTrigger, getIphoneModelBySlugState] =
      useLazyGetIphoneModelBySlugQuery();

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

   const getIphoneModelBySlugAsync = useCallback(
      async (slug: string) => {
         try {
            const result = await getIphoneModelBySlugTrigger(slug).unwrap();

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
      [getIphoneModelBySlugTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getIphoneModelsState.isLoading ||
         getIphoneModelsState.isFetching ||
         getIphoneModelBySlugState.isLoading ||
         getIphoneModelBySlugState.isFetching
      );
   }, [
      getIphoneModelsState.isLoading,
      getIphoneModelsState.isFetching,
      getIphoneModelBySlugState.isLoading,
      getIphoneModelBySlugState.isFetching,
   ]);

   return {
      isLoading,
      getIphoneModelsState: getIphoneModelsState,
      getIphoneModelBySlugState: getIphoneModelBySlugState,

      getIphoneModelsAsync,
      getIphoneModelBySlugAsync,
   };
};

export default useCatalogService;
