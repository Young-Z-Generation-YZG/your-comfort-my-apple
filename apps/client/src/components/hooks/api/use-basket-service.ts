import { useCallback, useMemo } from 'react';
import { IGetBasketQueries } from '~/domain/interfaces/baskets/basket.interface';
import {
   useDeleteBasketMutation,
   useLazyGetBasketQuery,
   useProceedToCheckoutMutation,
   useStoreBasketMutation,
   useLazyGetCheckoutItemsQuery,
   useCheckoutBasketMutation,
   useStoreEventItemMutation,
   useCheckoutBasketWithBlockchainMutation,
   TStoreBasketPayload,
} from '~/infrastructure/services/basket.service';
import { useCheckApiError } from '../use-check-error';
import { ICheckoutPayload } from '~/domain/interfaces/baskets/checkout.interface';
import { useDispatch } from 'react-redux';
import { SyncCart } from '~/infrastructure/redux/features/cart.slice';
import { useAppSelector } from '~/infrastructure/redux/store';

const useBasketService = () => {
   const dispatch = useDispatch();

   const cartAppState = useAppSelector((state) => state.cart);

   const [getBasketQueryTrigger, getBasketQueryState] = useLazyGetBasketQuery();
   const [getCheckoutItemsQueryTrigger, getCheckoutItemsQueryState] =
      useLazyGetCheckoutItemsQuery();

   const [storeBasketMutation, storeBasketMutationState] =
      useStoreBasketMutation();
   const [storeEventItemMutation, storeEventItemMutationState] =
      useStoreEventItemMutation();
   const [deleteBasketMutation, deleteBasketMutationState] =
      useDeleteBasketMutation();
   const [proceedToCheckoutMutation, proceedToCheckoutMutationState] =
      useProceedToCheckoutMutation();
   const [checkoutBasketMutation, checkoutBasketMutationState] =
      useCheckoutBasketMutation();
   const [
      checkoutBasketWithBlockchainMutation,
      checkoutBasketWithBlockchainMutationState,
   ] = useCheckoutBasketWithBlockchainMutation();

   useCheckApiError([
      { title: 'Get Basket failed', error: getBasketQueryState.error },
      {
         title: 'Proceed To Checkout failed',
         error: proceedToCheckoutMutationState.error,
      },
      {
         title: 'Get Checkout Items failed',
         error: getCheckoutItemsQueryState.error,
      },
      {
         title: 'Store Event Item failed',
         error: storeEventItemMutationState.error,
      },
      {
         title: 'Checkout Basket failed',
         error: checkoutBasketMutationState.error,
      },
      {
         title: 'Checkout Basket With Blockchain failed',
         error: checkoutBasketWithBlockchainMutationState.error,
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

   const getCheckoutItemsAsync = useCallback(
      async (queries: IGetBasketQueries) => {
         try {
            const result = await getCheckoutItemsQueryTrigger(queries).unwrap();
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
      [getCheckoutItemsQueryTrigger],
   );

   const storeBasketAsync = useCallback(
      async (payload: TStoreBasketPayload) => {
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

   const storeEventItemAsync = useCallback(
      async (payload: any) => {
         try {
            const result = await storeEventItemMutation(payload).unwrap();
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
      [storeEventItemMutation],
   );

   const deleteBasketAsync = useCallback(async () => {
      try {
         const result = await deleteBasketMutation().unwrap();
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

   const checkoutBasketAsync = useCallback(
      async (payload: ICheckoutPayload) => {
         try {
            const result = await checkoutBasketMutation(payload).unwrap();
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
      [checkoutBasketMutation],
   );

   const checkoutBasketWithBlockchainAsync = useCallback(
      async (
         payload: ICheckoutPayload & {
            crypto_uuid: string;
            customer_public_key: string;
            tx: string;
         },
      ) => {
         try {
            const result =
               await checkoutBasketWithBlockchainMutation(payload).unwrap();
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
      [checkoutBasketWithBlockchainMutation],
   );

   const isLoading = useMemo(() => {
      return (
         getBasketQueryState.isLoading ||
         getBasketQueryState.isFetching ||
         storeBasketMutationState.isLoading ||
         deleteBasketMutationState.isLoading ||
         proceedToCheckoutMutationState.isLoading ||
         getCheckoutItemsQueryState.isLoading ||
         getCheckoutItemsQueryState.isFetching ||
         checkoutBasketMutationState.isLoading ||
         storeEventItemMutationState.isLoading ||
         checkoutBasketWithBlockchainMutationState.isLoading
      );
   }, [
      getBasketQueryState.isLoading,
      getBasketQueryState.isFetching,
      storeBasketMutationState.isLoading,
      deleteBasketMutationState.isLoading,
      proceedToCheckoutMutationState.isLoading,
      getCheckoutItemsQueryState.isLoading,
      getCheckoutItemsQueryState.isFetching,
      checkoutBasketMutationState.isLoading,
      storeEventItemMutationState.isLoading,
      checkoutBasketWithBlockchainMutationState.isLoading,
   ]);

   return {
      isLoading,
      getBasketState: getBasketQueryState,
      storeBasketState: storeBasketMutationState,
      deleteBasketState: deleteBasketMutationState,
      proceedToCheckoutState: proceedToCheckoutMutationState,
      getCheckoutItemsState: getCheckoutItemsQueryState,
      checkoutBasketState: checkoutBasketMutationState,
      storeEventItemState: storeEventItemMutationState,
      getBasketAsync,
      getCheckoutItemsAsync,
      storeBasketAsync,
      deleteBasketAsync,
      proceedToCheckoutAsync,
      checkoutBasketAsync,
      storeEventItemAsync,
      checkoutBasketWithBlockchainAsync,
   };
};

export default useBasketService;
