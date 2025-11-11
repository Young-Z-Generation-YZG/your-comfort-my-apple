import { useCallback, useMemo } from 'react';
import {
   TBaseQueryParams,
   useLazyGetOrderDetailsQuery,
   useLazyGetOrdersByAdminQuery,
   useLazyGetRevenuesQuery,
   useLazyGetRevenuesByYearsQuery,
   useLazyGetRevenuesByTenantsQuery,
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
      async (params: TBaseQueryParams) => {
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
         revenuesByTenantsQueryState.isFetching
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
   ]);

   return {
      // Order details
      getOrderDetailsState: orderDetailsQueryState,
      getOrdersByAdminState: ordersByAdminQueryState,
      getRevenuesState: revenuesQueryState,
      getRevenuesByYearsState: revenuesByYearsQueryState,
      getRevenuesByTenantsState: revenuesByTenantsQueryState,

      // Actions
      getOrderDetailsAsync,
      getOrdersByAdminAsync,
      getRevenuesAsync,
      getRevenuesByYearsAsync,
      getRevenuesByTenantsAsync,

      // Loading
      isLoading,
   };
};

export default useOrderingService;
