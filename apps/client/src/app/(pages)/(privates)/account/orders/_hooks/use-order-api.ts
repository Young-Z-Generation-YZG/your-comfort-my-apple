import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/app/(pages)/(auth)/_hooks/use-check-error';
import {
   useCancelOrderMutation,
   useConfirmOrderMutation,
   useLazyGetOrderDetailsQuery,
   useLazyGetOrdersQuery,
} from '~/infrastructure/services/order.service';

const useOrderApi = () => {
   const [confirmOrderMutation, confirmOrderMutationState] =
      useConfirmOrderMutation();
   const [cancelOrderMutation, cancelOrderMutationState] =
      useCancelOrderMutation();

   const [getOrdersTrigger, ordersQueryState] = useLazyGetOrdersQuery();
   const [getOrderDetailsTrigger, orderDetailsQueryState] =
      useLazyGetOrderDetailsQuery();

   useCheckApiError([
      { title: 'Confirm Order failed', error: confirmOrderMutationState.error },
      { title: 'Cancel Order failed', error: cancelOrderMutationState.error },
      { title: 'Can not find this order', error: orderDetailsQueryState.error },
   ]);

   const getOrderDetailsAsync = useCallback(
      async (id: string) => {
         try {
            const result = await getOrderDetailsTrigger(id).unwrap();
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
      [getOrderDetailsTrigger],
   );

   const getOrdersAsync = useCallback(async () => {
      try {
         const result = await getOrdersTrigger().unwrap();
         return {
            isSuccess: true,
            isError: false,
            data: result,
            error: null,
         };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [getOrdersTrigger]);

   const confirmOrderAsync = useCallback(
      async (orderId: string) => {
         try {
            const result = await confirmOrderMutation(orderId).unwrap();
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
      [confirmOrderMutation],
   );

   const cancelOrderAsync = useCallback(
      async (orderId: string) => {
         try {
            const result = await cancelOrderMutation(orderId).unwrap();
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
      [cancelOrderMutation],
   );

   const isLoading = useMemo(() => {
      return (
         orderDetailsQueryState.isLoading ||
         confirmOrderMutationState.isLoading ||
         cancelOrderMutationState.isLoading ||
         ordersQueryState.isLoading ||
         orderDetailsQueryState.isFetching
      );
   }, [
      orderDetailsQueryState.isLoading,
      confirmOrderMutationState.isLoading,
      cancelOrderMutationState.isLoading,
      ordersQueryState.isLoading,
      orderDetailsQueryState.isFetching,
   ]);

   return {
      // Order details
      ordersState: ordersQueryState,
      orderDetailsState: orderDetailsQueryState,

      // States
      confirmOrderState: confirmOrderMutationState,
      cancelOrderState: cancelOrderMutationState,

      // Actions
      getOrderDetailsAsync,
      getOrdersAsync,
      confirmOrderAsync,
      cancelOrderAsync,

      // Loading
      isLoading,
   };
};

export default useOrderApi;
