import { useCallback, useMemo } from 'react';
import {
   useLazyGetReviewByOrderIdAsyncQuery,
   useCreateReviewAsyncMutation,
   useUpdateReviewAsyncMutation,
   useDeleteReviewAsyncMutation,
   IGetReviewsQueryParams,
   useLazyGetReviewByProductModelIdAsyncQuery,
   useLazyGetReviewByProductModelSlugAsyncQuery,
} from '~/infrastructure/services/review.service';
import {
   IReviewPayload,
   IUpdateReviewPayload,
} from '~/domain/interfaces/catalog.interface';
import { useCheckApiError } from '../use-check-error';

const useReviewService = () => {
   const [getReviewByProductModelIdTrigger, getReviewByProductModelIdState] =
      useLazyGetReviewByProductModelIdAsyncQuery();
   const [
      getReviewByProductModelSlugTrigger,
      getReviewByProductModelSlugState,
   ] = useLazyGetReviewByProductModelSlugAsyncQuery();
   const [getReviewByOrderIdTrigger, getReviewByOrderIdState] =
      useLazyGetReviewByOrderIdAsyncQuery();

   const [createReviewMutation, createReviewMutationState] =
      useCreateReviewAsyncMutation();
   const [updateReviewMutation, updateReviewMutationState] =
      useUpdateReviewAsyncMutation();
   const [deleteReviewMutation, deleteReviewMutationState] =
      useDeleteReviewAsyncMutation();

   useCheckApiError([
      {
         title: 'Get Review By Product Model Id failed',
         error: getReviewByProductModelIdState.error,
      },
      {
         title: 'Get Review By Product Model Slug failed',
         error: getReviewByProductModelSlugState.error,
      },
      {
         title: 'Get Review By Order Id failed',
         error: getReviewByOrderIdState.error,
      },
      {
         title: 'Create Review failed',
         error: createReviewMutationState.error,
      },
      {
         title: 'Update Review failed',
         error: updateReviewMutationState.error,
      },
      {
         title: 'Delete Review failed',
         error: deleteReviewMutationState.error,
      },
   ]);

   const getReviewByProductModelIdAsync = useCallback(
      async (id: string, queryParams: IGetReviewsQueryParams) => {
         try {
            const result = await getReviewByProductModelIdTrigger({
               id,
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
      [getReviewByProductModelIdTrigger],
   );

   const getReviewByOrderIdAsync = useCallback(
      async (orderId: string) => {
         try {
            const result = await getReviewByOrderIdTrigger(orderId).unwrap();
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
      [getReviewByOrderIdTrigger],
   );

   const getReviewByProductModelSlugAsync = useCallback(
      async (slug: string, queryParams: IGetReviewsQueryParams) => {
         try {
            const result = await getReviewByProductModelSlugTrigger({
               slug,
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
      [getReviewByProductModelSlugTrigger],
   );

   const createReviewAsync = useCallback(
      async (payload: IReviewPayload) => {
         try {
            const result = await createReviewMutation(payload).unwrap();
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
      [createReviewMutation],
   );

   const updateReviewAsync = useCallback(
      async (reviewId: string, payload: IUpdateReviewPayload) => {
         try {
            const result = await updateReviewMutation({
               reviewId,
               payload,
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
      [updateReviewMutation],
   );

   const deleteReviewAsync = useCallback(
      async (reviewId: string) => {
         try {
            const result = await deleteReviewMutation(reviewId).unwrap();
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
      [deleteReviewMutation],
   );

   const isLoading = useMemo(() => {
      return (
         getReviewByProductModelIdState.isLoading ||
         getReviewByProductModelIdState.isFetching ||
         getReviewByProductModelSlugState.isLoading ||
         getReviewByProductModelSlugState.isFetching ||
         getReviewByOrderIdState.isLoading ||
         getReviewByOrderIdState.isFetching ||
         createReviewMutationState.isLoading ||
         updateReviewMutationState.isLoading ||
         deleteReviewMutationState.isLoading
      );
   }, [
      getReviewByProductModelIdState.isLoading,
      getReviewByProductModelIdState.isFetching,
      getReviewByProductModelSlugState.isLoading,
      getReviewByProductModelSlugState.isFetching,
      getReviewByOrderIdState.isLoading,
      getReviewByOrderIdState.isFetching,
      createReviewMutationState.isLoading,
      updateReviewMutationState.isLoading,
      deleteReviewMutationState.isLoading,
   ]);

   return {
      isLoading,
      getReviewByProductModelIdState,
      getReviewByProductModelSlugState,
      getReviewByOrderIdState,
      createReviewMutationState,
      updateReviewMutationState,
      deleteReviewMutationState,

      getReviewByProductModelIdAsync,
      getReviewByProductModelSlugAsync,
      getReviewByOrderIdAsync,
      createReviewAsync,
      updateReviewAsync,
      deleteReviewAsync,
   };
};

export default useReviewService;
