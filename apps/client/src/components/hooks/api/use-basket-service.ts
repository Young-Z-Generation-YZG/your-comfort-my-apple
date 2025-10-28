import { useCallback, useMemo } from 'react';
import {
   IGetBasketQueries,
   IStoreBasketPayload,
} from '~/domain/interfaces/baskets/basket.interface';
import {
   useDeleteBasketMutation,
   useLazyGetBasketQuery,
   useProceedToCheckoutMutation,
   useStoreBasketMutation,
} from '~/infrastructure/services/basket.service';
import { useCheckApiError } from '../use-check-error';

const useBasketService = () => {
   const [getBasketQueryTrigger, getBasketQueryState] = useLazyGetBasketQuery();

   const [storeBasketMutation, storeBasketMutationState] =
      useStoreBasketMutation();
   const [deleteBasketMutation, deleteBasketMutationState] =
      useDeleteBasketMutation();
   const [proceedToCheckoutMutation, proceedToCheckoutMutationState] =
      useProceedToCheckoutMutation();

   useCheckApiError([
      { title: 'Get Basket failed', error: getBasketQueryState.error },
      {
         title: 'Proceed To Checkout failed',
         error: proceedToCheckoutMutationState.error,
      },
      // { title: 'Store Basket failed', error: storeBasketMutationState.error },
      // { title: 'Delete Basket failed', error: deleteBasketMutationState.error },
   ]);

   const getBasketAsync = useCallback(
      async (queries: IGetBasketQueries) => {
         try {
            const result = await getBasketQueryTrigger(queries).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [getBasketQueryTrigger],
   );

   const storeBasketAsync = useCallback(
      async (payload: IStoreBasketPayload) => {
         try {
            const result = await storeBasketMutation(payload).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [storeBasketMutation],
   );

   const deleteBasketAsync = useCallback(async () => {
      try {
         const result = await deleteBasketMutation({}).unwrap();
         return {
            isSuccess: true,
            isError: false,
            data: result,
            error: null,
         };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [deleteBasketMutation]);

   const proceedToCheckoutAsync = useCallback(async () => {
      try {
         const result = await proceedToCheckoutMutation().unwrap();
         return { isSuccess: true, isError: false, data: result, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [proceedToCheckoutMutation]);

   const isLoading = useMemo(() => {
      return (
         getBasketQueryState.isLoading ||
         getBasketQueryState.isFetching ||
         storeBasketMutationState.isLoading ||
         deleteBasketMutationState.isLoading ||
         proceedToCheckoutMutationState.isLoading
      );
   }, [
      getBasketQueryState.isLoading,
      getBasketQueryState.isFetching,
      storeBasketMutationState.isLoading,
      deleteBasketMutationState.isLoading,
      proceedToCheckoutMutationState.isLoading,
   ]);

   return {
      isLoading,
      getBasketState: getBasketQueryState,
      storeBasketState: storeBasketMutationState,
      deleteBasketState: deleteBasketMutationState,
      proceedToCheckoutState: proceedToCheckoutMutationState,

      getBasketAsync,
      storeBasketAsync,
      deleteBasketAsync,
      proceedToCheckoutAsync,
   };
};

export default useBasketService;
