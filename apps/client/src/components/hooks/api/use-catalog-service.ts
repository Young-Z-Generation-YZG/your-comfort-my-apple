import { useCallback, useMemo } from 'react';
import {
   useLazyGetIphoneModelsQuery,
   useLazyGetModelBySlugQuery,
   TGetIphoneModelsFilter,
} from '~/infrastructure/services/catalog.service';

const useCatalogService = () => {
   const [getIphoneModelsTrigger, getIphoneModelsState] =
      useLazyGetIphoneModelsQuery();
   const [getModelBySlugTrigger, getModelBySlugState] =
      useLazyGetModelBySlugQuery();

   const getIphoneModelsAsync = useCallback(
      async (
         params: TGetIphoneModelsFilter = {
            _page: 1,
            _limit: 10,
            _colors: null,
            _storages: null,
            _models: null,
            _minPrice: null,
            _maxPrice: null,
            _priceSort: null,
         },
      ) => {
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
