import { useCallback, useMemo } from 'react';
import {
   IGetProductModelsByCategorySlugQueryParams,
   useLazyGetProductModelsByCategorySlugQuery,
} from '~/src/infrastructure/services/product.service';

const useProductService = () => {
   const [
      getProductModelsByCategorySlugTrigger,
      getProductModelsByCategorySlugState,
   ] = useLazyGetProductModelsByCategorySlugQuery();

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

   const isLoading = useMemo(() => {
      return (
         getProductModelsByCategorySlugState.isLoading ||
         getProductModelsByCategorySlugState.isFetching
      );
   }, [
      getProductModelsByCategorySlugState.isLoading,
      getProductModelsByCategorySlugState.isFetching,
   ]);

   return {
      isLoading,
      getProductModelsByCategorySlugState,

      getProductModelsByCategorySlugAsync,
   };
};

export default useProductService;
