import { useCallback, useMemo } from 'react';
import { useLazyGetModelBySlugQuery } from '~/infrastructure/services/catalog.service';

const useCatalogService = () => {
   const [getModelBySlugTrigger, getModelBySlugState] =
      useLazyGetModelBySlugQuery();

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
      return getModelBySlugState.isLoading || getModelBySlugState.isFetching;
   }, [getModelBySlugState.isLoading, getModelBySlugState.isFetching]);

   return {
      isLoading,
      getModelBySlugState,

      getModelBySlugAsync,
   };
};

export default useCatalogService;
