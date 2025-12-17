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
   useLazyGetUserOrdersDetailsQuery,
} from '~/src/infrastructure/services/ordering.service';
import { useCheckApiError } from '~/src/hooks/use-check-error';
import { useCheckApiSuccess } from '~/src/hooks/use-check-success';

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
   const [getUserOrdersDetailsTrigger, userOrdersDetailsQueryState] =
      useLazyGetUserOrdersDetailsQuery();
   const [updateOnlineOrderStatusTrigger, updateOnlineOrderStatusQueryState] =
      useUpdateOnlineOrderStatusMutation();

   useCheckApiError([
      { title: 'Can not find this order', error: orderDetailsQueryState.error },
   ]);

   useCheckApiSuccess([
      {
         title: 'Order status updated',
         isSuccess: updateOnlineOrderStatusQueryState.isSuccess,
      },
   ]);

   const getUserOrdersDetailsAsync = useCallback(
      async (userId: string, params: IBaseQueryParams) => {
         try {
            const result = await getUserOrdersDetailsTrigger({
               userId,
               params,
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
      [getUserOrdersDetailsTrigger],
   );

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
         ordersQueryState.isFetching ||
         userOrdersDetailsQueryState.isLoading ||
         userOrdersDetailsQueryState.isFetching
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
      userOrdersDetailsQueryState.isLoading,
      userOrdersDetailsQueryState.isFetching,
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
      getUserOrdersDetailsState: userOrdersDetailsQueryState,

      // Actions
      getOrderDetailsAsync,
      getOrdersByAdminAsync,
      getRevenuesAsync,
      getRevenuesByYearsAsync,
      getRevenuesByTenantsAsync,
      updateOnlineOrderStatusAsync,
      getOrdersAsync,
      getUserOrdersDetailsAsync,

      // Loading
      isLoading,
   };
};

export default useOrderingService;
