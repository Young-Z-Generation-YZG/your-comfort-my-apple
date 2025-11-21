import { useCallback, useMemo } from 'react';
import {
   IBaseQueryParams,
   useLazyGetOrderDetailsQuery,
   useLazyGetOrdersByAdminQuery,
   useLazyGetRevenuesQuery,
   useLazyGetRevenuesByYearsQuery,
   useLazyGetRevenuesByTenantsQuery,
   useUpdateOnlineOrderStatusMutation,
   IUpdateOnlineOrderStatusPayload,
   useLazyGetOrdersQuery,
} from '~/src/infrastructure/services/order.service';
import { useCheckApiError } from '~/src/hooks/use-check-error';

const useOrderingService = () => {
   const [getOrderDetailsTrigger, orderDetailsQueryState] =
      useLazyGetOrderDetailsQuery();
   const [getOrdersByAdminTrigger, ordersByAdminQueryState] =
      useLazyGetOrdersByAdminQuery();
   const [getRevenuesTrigger, revenuesQueryState] = useLazyGetRevenuesQuery();
   const [getRevenuesByYearsTrigger, revenuesByYearsQueryState] =
      useLazyGetRevenuesByYearsQuery();
   const [getRevenuesByTenantsTrigger, revenuesByTenantsQueryState] =
      useLazyGetRevenuesByTenantsQuery();
   const [getOrdersTrigger, ordersQueryState] = useLazyGetOrdersQuery();

   const [updateOnlineOrderStatusTrigger, updateOnlineOrderStatusQueryState] =
      useUpdateOnlineOrderStatusMutation();

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

   const getOrdersAsync = useCallback(
      async (params: IBaseQueryParams) => {
         try {
            const result = await getOrdersTrigger(params).unwrap();
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

   const getOrdersByAdminAsync = useCallback(
      async (params: IBaseQueryParams) => {
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

   const getRevenuesAsync = useCallback(async () => {
      try {
         const result = await getRevenuesTrigger().unwrap();
         return {
            isSuccess: true,
            isError: false,
            data: result,
            error: null,
         };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [getRevenuesTrigger]);

   const getRevenuesByYearsAsync = useCallback(
      async (years: string[]) => {
         try {
            const result = await getRevenuesByYearsTrigger({
               _years: years,
            }).unwrap();
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
      [getRevenuesByYearsTrigger],
   );

   const getRevenuesByTenantsAsync = useCallback(
      async (tenants: string[]) => {
         try {
            const result = await getRevenuesByTenantsTrigger({
               _tenants: tenants,
            }).unwrap();
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
      [getRevenuesByTenantsTrigger],
   );

   const updateOnlineOrderStatusAsync = useCallback(
      async (orderId: string, payload: IUpdateOnlineOrderStatusPayload) => {
         try {
            const result = await updateOnlineOrderStatusTrigger({
               order_id: orderId,
               payload,
            }).unwrap();
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
      [updateOnlineOrderStatusTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         orderDetailsQueryState.isLoading ||
         orderDetailsQueryState.isFetching ||
         ordersByAdminQueryState.isLoading ||
         ordersByAdminQueryState.isFetching ||
         revenuesQueryState.isLoading ||
         revenuesQueryState.isFetching ||
         revenuesByYearsQueryState.isLoading ||
         revenuesByYearsQueryState.isFetching ||
         revenuesByTenantsQueryState.isLoading ||
         revenuesByTenantsQueryState.isFetching ||
         updateOnlineOrderStatusQueryState.isLoading ||
         ordersQueryState.isLoading ||
         ordersQueryState.isFetching
      );
   }, [
      orderDetailsQueryState.isLoading,
      orderDetailsQueryState.isFetching,
      ordersByAdminQueryState.isLoading,
      ordersByAdminQueryState.isFetching,
      revenuesQueryState.isLoading,
      revenuesQueryState.isFetching,
      revenuesByYearsQueryState.isLoading,
      revenuesByYearsQueryState.isFetching,
      revenuesByTenantsQueryState.isLoading,
      revenuesByTenantsQueryState.isFetching,
      updateOnlineOrderStatusQueryState.isLoading,
      ordersQueryState.isLoading,
      ordersQueryState.isFetching,
   ]);

   return {
      // Order details
      getOrderDetailsState: orderDetailsQueryState,
      getOrdersByAdminState: ordersByAdminQueryState,
      getRevenuesState: revenuesQueryState,
      getRevenuesByYearsState: revenuesByYearsQueryState,
      getRevenuesByTenantsState: revenuesByTenantsQueryState,
      updateOnlineOrderStatusState: updateOnlineOrderStatusQueryState,
      getOrdersState: ordersQueryState,

      // Actions
      getOrderDetailsAsync,
      getOrdersByAdminAsync,
      getRevenuesAsync,
      getRevenuesByYearsAsync,
      getRevenuesByTenantsAsync,
      updateOnlineOrderStatusAsync,
      getOrdersAsync,

      // Loading
      isLoading,
   };
};

export default useOrderingService;
