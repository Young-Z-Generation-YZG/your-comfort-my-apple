import { useCallback, useMemo } from 'react';
import {
   useLazyGetOrderDetailsQuery,
   useLazyGetOrdersByAdminQuery,
} from '~/src/infrastructure/services/order.service';
import { useCheckApiError } from '~/src/hooks/use-check-error';

const useOrderingService = () => {
   const [getOrderDetailsTrigger, orderDetailsQueryState] =
      useLazyGetOrderDetailsQuery();
   const [getOrdersByAdminTrigger, ordersByAdminQueryState] =
      useLazyGetOrdersByAdminQuery();
   useCheckApiError([
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

   const getOrdersByAdminAsync = useCallback(
      async (params: any) => {
         try {
            const result = await getOrdersByAdminTrigger(params).unwrap();
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
      [getOrdersByAdminTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         orderDetailsQueryState.isLoading ||
         orderDetailsQueryState.isFetching ||
         ordersByAdminQueryState.isLoading ||
         ordersByAdminQueryState.isFetching
      );
   }, [
      orderDetailsQueryState.isLoading,
      orderDetailsQueryState.isFetching,
      ordersByAdminQueryState.isLoading,
      ordersByAdminQueryState.isFetching,
   ]);

   return {
      // Order details
      getOrderDetailsState: orderDetailsQueryState,
      getOrdersByAdminState: ordersByAdminQueryState,

      // Actions
      getOrderDetailsAsync,
      getOrdersByAdminAsync,

      // Loading
      isLoading,
   };
};

export default useOrderingService;
