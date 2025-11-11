import { useCallback, useMemo } from 'react';
import {
   useLazyGetTenantsQuery,
   useLazyGetTenantByIdQuery,
} from '~/src/infrastructure/services/tenant.service';

const useTenantService = () => {
   const [getTenantsTrigger, getTenantsState] = useLazyGetTenantsQuery();
   const [getTenantByIdTrigger, getTenantByIdState] =
      useLazyGetTenantByIdQuery();

   const getTenantsAsync = useCallback(async () => {
      try {
         const result = await getTenantsTrigger().unwrap();
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
   }, [getTenantsTrigger]);

   const getTenantByIdAsync = useCallback(
      async (id: string) => {
         try {
            const result = await getTenantByIdTrigger(id).unwrap();

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
      [getTenantByIdTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getTenantsState.isLoading ||
         getTenantsState.isFetching ||
         getTenantByIdState.isLoading ||
         getTenantByIdState.isFetching
      );
   }, [
      getTenantsState.isLoading,
      getTenantsState.isFetching,
      getTenantByIdState.isLoading,
      getTenantByIdState.isFetching,
   ]);

   return {
      isLoading,
      getTenantsState: getTenantsState,
      getTenantByIdState: getTenantByIdState,

      getTenantsAsync,
      getTenantByIdAsync,
   };
};

export default useTenantService;
