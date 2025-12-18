import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/hooks/use-check-error';
import {
   useCancelOrderMutation,
   useConfirmOrderMutation,
   useLazyGetOrderDetailsQuery,
   useLazyGetOrdersQuery,
   useMomoIpnCallbackMutation,
   useVnpayIpnCallbackMutation,
} from '~/infrastructure/services/ordering.service';
import {
   MomoIpnFormType,
   VnpayIpnFormType,
} from '~/domain/schemas/order.schema';
import { IOrderFilterQueryParams } from '~/domain/interfaces/ordering.interface';
import { useCheckApiSuccess } from '../use-check-success';

const useOrderingService = () => {
   const [confirmOrderMutation, confirmOrderMutationState] =
      useConfirmOrderMutation();
   const [cancelOrderMutation, cancelOrderMutationState] =
      useCancelOrderMutation();
   const [vnpayIpnCallbackMutation, vnpayIpnCallbackMutationState] =
      useVnpayIpnCallbackMutation();
   const [momoIpnCallbackMutation, momoIpnCallbackMutationState] =
      useMomoIpnCallbackMutation();

   const [getOrdersTrigger, ordersQueryState] = useLazyGetOrdersQuery();
   const [getOrderDetailsTrigger, orderDetailsQueryState] =
      useLazyGetOrderDetailsQuery();

   useCheckApiError([
      { title: 'Confirm Order failed', error: confirmOrderMutationState.error },
      { title: 'Cancel Order failed', error: cancelOrderMutationState.error },
      { title: 'Can not find this order', error: orderDetailsQueryState.error },
      {
         title: 'Vnpay IPN Callback failed',
         error: vnpayIpnCallbackMutationState.error,
      },
      {
         title: 'Momo IPN Callback failed',
         error: momoIpnCallbackMutationState.error,
      },
   ]);

   useCheckApiSuccess([
      {
         title: 'Order confirmed successfully',
         isSuccess: confirmOrderMutationState.isSuccess,
      },
      {
         title: 'Order canceled successfully',
         isSuccess: cancelOrderMutationState.isSuccess,
      },
   ]);

   const getOrderDetailsAsync = useCallback(
      async (orderId: string) => {
         try {
            const result = await getOrderDetailsTrigger(orderId).unwrap();
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

   const getOrdersAsync = useCallback(
      async (queryParams: IOrderFilterQueryParams) => {
         try {
            const result = await getOrdersTrigger(queryParams).unwrap();
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
      [getOrdersTrigger],
   );

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

   const vnpayIpnCallbackAsync = useCallback(
      async (payload: VnpayIpnFormType) => {
         try {
            const result = await vnpayIpnCallbackMutation(payload).unwrap();

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
      [vnpayIpnCallbackMutation],
   );

   const momoIpnCallbackAsync = useCallback(
      async (payload: MomoIpnFormType) => {
         try {
            const result = await momoIpnCallbackMutation(payload).unwrap();
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
      [momoIpnCallbackMutation],
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
      getOrdersState: ordersQueryState,
      getOrderDetailsState: orderDetailsQueryState,

      // States
      confirmOrderState: confirmOrderMutationState,
      cancelOrderState: cancelOrderMutationState,
      vnpayIpnCallbackState: vnpayIpnCallbackMutationState,
      momoIpnCallbackState: momoIpnCallbackMutationState,

      // Actions
      getOrderDetailsAsync,
      getOrdersAsync,
      confirmOrderAsync,
      cancelOrderAsync,
      vnpayIpnCallbackAsync,
      momoIpnCallbackAsync,

      // Loading
      isLoading,
   };
};

export default useOrderingService;
