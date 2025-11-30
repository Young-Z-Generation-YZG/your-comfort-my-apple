import { useCallback, useMemo } from 'react';
import {
   useLazyGetProductModelsQuery,
   useLazyGetPopularProductsQuery,
   useLazyGetNewestProductsQuery,
   useLazyGetSuggestionProductsQuery,
   IGetProductModelsQueryParams,
} from '~/infrastructure/services/product.service';

const useProductService = () => {
   const [getProductModelsTrigger, getProductModelsState] =
      useLazyGetProductModelsQuery();
   const [getPopularProductsTrigger, getPopularProductsState] =
      useLazyGetPopularProductsQuery();
   const [getNewestProductsTrigger, getNewestProductsState] =
      useLazyGetNewestProductsQuery();
   const [getSuggestionProductsTrigger, getSuggestionProductsState] =
      useLazyGetSuggestionProductsQuery();

   const getProductModelsAsync = useCallback(
      async (query: IGetProductModelsQueryParams) => {
         try {
            const result = await getProductModelsTrigger(query).unwrap();
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
      [getProductModelsTrigger],
   );

   const getPopularProductsAsync = useCallback(async () => {
      try {
         const result = await getPopularProductsTrigger().unwrap();
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
   }, [getPopularProductsTrigger]);

   const getNewestProductsAsync = useCallback(async () => {
      try {
         const result = await getNewestProductsTrigger().unwrap();
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
   }, [getNewestProductsTrigger]);

   const getSuggestionProductsAsync = useCallback(async () => {
      try {
         const result = await getSuggestionProductsTrigger().unwrap();
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
   }, [getSuggestionProductsTrigger]);

   const isLoading = useMemo(() => {
      return (
         getProductModelsState.isLoading ||
         getProductModelsState.isFetching ||
         getPopularProductsState.isLoading ||
         getPopularProductsState.isFetching ||
         getNewestProductsState.isLoading ||
         getNewestProductsState.isFetching ||
         getSuggestionProductsState.isLoading ||
         getSuggestionProductsState.isFetching
      );
   }, [
      getProductModelsState.isLoading,
      getProductModelsState.isFetching,
      getPopularProductsState.isLoading,
      getPopularProductsState.isFetching,
      getNewestProductsState.isLoading,
      getNewestProductsState.isFetching,
      getSuggestionProductsState.isLoading,
      getSuggestionProductsState.isFetching,
   ]);

   return {
      isLoading,
      getProductModelsState,
      getPopularProductsState,
      getNewestProductsState,
      getSuggestionProductsState,

      getProductModelsAsync,
      getPopularProductsAsync,
      getNewestProductsAsync,
      getSuggestionProductsAsync,
   };
};

export default useProductService;
