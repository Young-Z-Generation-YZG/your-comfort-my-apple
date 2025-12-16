import { useCallback, useMemo } from 'react';
import {
   IGetProductModelsByCategorySlugQueryParams,
   useLazyGetProductModelsByCategorySlugQuery,
   useLazyGetProductModelBySlugQuery,
} from '~/src/infrastructure/services/product.service';

const useProductService = () => {
   const [
      getProductModelsByCategorySlugTrigger,
      getProductModelsByCategorySlugState,
   ] = useLazyGetProductModelsByCategorySlugQuery();

   const [getProductModelBySlugTrigger, getProductModelBySlugState] =
      useLazyGetProductModelBySlugQuery();

   const getProductModelsByCategorySlugAsync = useCallback(
      async (
         categorySlug: string,
         queryParams: IGetProductModelsByCategorySlugQueryParams,
      ) => {
         try {
            const result = await getProductModelsByCategorySlugTrigger({
               categorySlug,
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
      [getProductModelsByCategorySlugTrigger],
   );

   const getProductModelBySlugAsync = useCallback(
      async (slug: string) => {
         try {
            const result = await getProductModelBySlugTrigger(slug).unwrap();
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
      [getProductModelBySlugTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getProductModelsByCategorySlugState.isLoading ||
         getProductModelsByCategorySlugState.isFetching ||
         getProductModelBySlugState.isLoading ||
         getProductModelBySlugState.isFetching
      );
   }, [
      getProductModelsByCategorySlugState.isLoading,
      getProductModelsByCategorySlugState.isFetching,
      getProductModelBySlugState.isLoading,
      getProductModelBySlugState.isFetching,
   ]);

   return {
      isLoading,
      getProductModelsByCategorySlugState,
      getProductModelBySlugState,
      getProductModelsByCategorySlugAsync,
      getProductModelBySlugAsync,
   };
};

export default useProductService;
