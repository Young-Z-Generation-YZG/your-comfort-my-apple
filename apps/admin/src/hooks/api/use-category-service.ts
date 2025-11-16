import { useCallback, useMemo } from 'react';
import {
   useLazyGetCategoriesQuery,
   useLazyGetCategoryDetailsQuery,
} from '../../infrastructure/services/category.service';

export const useCategoryService = () => {
   const [getCategoriesTrigger, categoriesState] = useLazyGetCategoriesQuery();
   const [getCategoryDetailsTrigger, categoryDetailsState] =
      useLazyGetCategoryDetailsQuery();

   const getCategoriesAsync = useCallback(async () => {
      try {
         const result = await getCategoriesTrigger().unwrap();
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
   }, [getCategoriesTrigger]);

   const getCategoryDetailsAsync = useCallback(
      async (categoryId: string) => {
         try {
            const result = await getCategoryDetailsTrigger(categoryId).unwrap();
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
      [getCategoryDetailsTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         categoriesState.isLoading ||
         categoriesState.isFetching ||
         categoryDetailsState.isLoading ||
         categoryDetailsState.isFetching
      );
   }, [
      categoriesState.isLoading,
      categoriesState.isFetching,
      categoryDetailsState.isLoading,
      categoryDetailsState.isFetching,
   ]);

   return {
      isLoading,
      categoriesState,
      categoryDetailsState,
      getCategoriesAsync,
      getCategoryDetailsAsync,
   };
};
