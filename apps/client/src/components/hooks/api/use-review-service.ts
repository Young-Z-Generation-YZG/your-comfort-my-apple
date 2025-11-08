import { useCallback, useMemo } from 'react';
import {
   useLazyGetReviewByProductModelSlugAsyncQuery,
   useLazyGetReviewByOrderIdAsyncQuery,
   useCreateReviewAsyncMutation,
   useUpdateReviewAsyncMutation,
   useDeleteReviewAsyncMutation,
} from '~/infrastructure/services/review.service';
import {
   IReviewPayload,
   IUpdateReviewPayload,
} from '~/domain/interfaces/catalogs/review.interface';
import { useCheckApiError } from '../use-check-error';

const useReviewService = () => {
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

   const getReviewByProductModelSlugAsync = useCallback(
      async (
         slug: string,
         options?: {
            page?: number;
            limit?: number;
            sortOrder?: 'asc' | 'desc';
         },
      ) => {
         try {
            const result = await getReviewByProductModelSlugTrigger({
               slug,
               page: options?.page,
               limit: options?.limit,
               sortOrder: options?.sortOrder,
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
               body: payload,
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
         getReviewByProductModelSlugState.isLoading ||
         getReviewByProductModelSlugState.isFetching ||
         getReviewByOrderIdState.isLoading ||
         getReviewByOrderIdState.isFetching ||
         createReviewMutationState.isLoading ||
         updateReviewMutationState.isLoading ||
         deleteReviewMutationState.isLoading
      );
   }, [
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
      getReviewByProductModelSlugState,
      getReviewByOrderIdState,
      createReviewState: createReviewMutationState,
      updateReviewState: updateReviewMutationState,
      deleteReviewState: deleteReviewMutationState,

      getReviewByProductModelSlugAsync,
      getReviewByOrderIdAsync,
      createReviewAsync,
      updateReviewAsync,
      deleteReviewAsync,
   };
};

export default useReviewService;
