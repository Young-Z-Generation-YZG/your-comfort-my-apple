import { useCallback, useMemo } from 'react';
import {
   useLazyGetCategoriesQuery,
   useLazyGetCategoryDetailsQuery,
   useCreateCategoryMutation,
   ICreateCategoryPayload,
} from '../../infrastructure/services/category.service';

export const useCategoryService = () => {
   const [getCategoriesTrigger, categoriesState] = useLazyGetCategoriesQuery();
   const [getCategoryDetailsTrigger, categoryDetailsState] =
      useLazyGetCategoryDetailsQuery();
   const [createCategoryTrigger, createCategoryState] =
      useCreateCategoryMutation();

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

   const createCategoryAsync = useCallback(
      async (payload: ICreateCategoryPayload) => {
         try {
            const result = await createCategoryTrigger(payload).unwrap();
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
      [createCategoryTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         categoriesState.isLoading ||
         categoriesState.isFetching ||
         categoryDetailsState.isLoading ||
         categoryDetailsState.isFetching ||
         createCategoryState.isLoading
      );
   }, [
      categoriesState.isLoading,
      categoriesState.isFetching,
      categoryDetailsState.isLoading,
      categoryDetailsState.isFetching,
      createCategoryState.isLoading,
   ]);

   return {
      isLoading,
      categoriesState,
      categoryDetailsState,
      createCategoryState,

      getCategoriesAsync,
      getCategoryDetailsAsync,
      createCategoryAsync,
   };
};
