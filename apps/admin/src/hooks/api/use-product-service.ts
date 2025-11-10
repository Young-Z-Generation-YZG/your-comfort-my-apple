import { useCallback, useMemo } from 'react';
import { useLazyGetProductsQuery } from '~/src/infrastructure/services/product.service';

const useProductService = () => {
   const [getProductsTrigger, getProductsState] = useLazyGetProductsQuery();

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

   const isLoading = useMemo(() => {
      return getProductsState.isLoading || getProductsState.isFetching;
   }, [getProductsState.isLoading, getProductsState.isFetching]);

   return {
      isLoading,
      getProductsState,

      getProductsAsync,
   };
};

export default useProductService;
