import { useCallback, useMemo } from 'react';
import {
   useLazyGetTenantsQuery,
   useLazyGetTenantByIdQuery,
   ICreateTenantPayload,
   useCreateTenantMutation,
} from '~/src/infrastructure/services/tenant.service';

const useTenantService = () => {
   const [getTenantsTrigger, getTenantsState] = useLazyGetTenantsQuery();
   const [getTenantByIdTrigger, getTenantByIdState] =
      useLazyGetTenantByIdQuery();
   const [createTenantTrigger, createTenantState] = useCreateTenantMutation();

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

   const createTenantAsync = useCallback(
      async (payload: ICreateTenantPayload) => {
         try {
            const result = await createTenantTrigger(payload).unwrap();
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
      [createTenantTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getTenantsState.isLoading ||
         getTenantsState.isFetching ||
         getTenantByIdState.isLoading ||
         getTenantByIdState.isFetching ||
         createTenantState.isLoading
      );
   }, [
      getTenantsState.isLoading,
      getTenantsState.isFetching,
      getTenantByIdState.isLoading,
      getTenantByIdState.isFetching,
      createTenantState.isLoading,
   ]);

   return {
      isLoading,
      getTenantsState,
      getTenantByIdState,
      createTenantState,
      getTenantsAsync,
      getTenantByIdAsync,
      createTenantAsync,
   };
};

export default useTenantService;
