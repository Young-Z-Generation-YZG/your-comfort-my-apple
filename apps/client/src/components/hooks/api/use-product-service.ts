import { useCallback, useMemo } from 'react';
import {
   useLazyGetProductsQuery,
   useLazyGetPopularProductsQuery,
   useLazyGetNewestProductsQuery,
   useLazyGetSuggestionProductsQuery,
} from '~/infrastructure/services/product.service';

const useProductService = () => {
   const [getProductsTrigger, getProductsState] = useLazyGetProductsQuery();
   const [getPopularProductsTrigger, getPopularProductsState] =
      useLazyGetPopularProductsQuery();
   const [getNewestProductsTrigger, getNewestProductsState] =
      useLazyGetNewestProductsQuery();
   const [getSuggestionProductsTrigger, getSuggestionProductsState] =
      useLazyGetSuggestionProductsQuery();

   const getProductsAsync = useCallback(
      async (query: Record<string, any>) => {
         try {
            const result = await getProductsTrigger(query).unwrap();
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
      [getProductsTrigger],
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
         getProductsState.isLoading ||
         getProductsState.isFetching ||
         getPopularProductsState.isLoading ||
         getPopularProductsState.isFetching ||
         getNewestProductsState.isLoading ||
         getNewestProductsState.isFetching ||
         getSuggestionProductsState.isLoading ||
         getSuggestionProductsState.isFetching
      );
   }, [
      getProductsState.isLoading,
      getProductsState.isFetching,
      getPopularProductsState.isLoading,
      getPopularProductsState.isFetching,
      getNewestProductsState.isLoading,
      getNewestProductsState.isFetching,
      getSuggestionProductsState.isLoading,
      getSuggestionProductsState.isFetching,
   ]);

   return {
      isLoading,
      getProductsState,
      getPopularProductsState,
      getNewestProductsState,
      getSuggestionProductsState,

      getProductsAsync,
      getPopularProductsAsync,
      getNewestProductsAsync,
      getSuggestionProductsAsync,
   };
};

export default useProductService;
