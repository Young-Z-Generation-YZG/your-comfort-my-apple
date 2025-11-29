import { useCallback, useMemo } from 'react';
import {
   useLazyGetListTenantsQuery,
   useLazyGetTenantByIdQuery,
   ICreateTenantPayload,
   useCreateTenantMutation,
} from '~/src/infrastructure/services/tenant.service';

const useTenantService = () => {
   const [getListTenantsTrigger, getListTenantsState] =
      useLazyGetListTenantsQuery();
   const [getTenantByIdTrigger, getTenantByIdState] =
      useLazyGetTenantByIdQuery();

   const [createTenantTrigger, createTenantState] = useCreateTenantMutation();

   const getListTenantsAsync = useCallback(async () => {
      try {
         const result = await getListTenantsTrigger().unwrap();
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
   }, [getListTenantsTrigger]);

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
         getListTenantsState.isLoading ||
         getListTenantsState.isFetching ||
         getTenantByIdState.isLoading ||
         getTenantByIdState.isFetching ||
         createTenantState.isLoading
      );
   }, [
      getListTenantsState.isLoading,
      getListTenantsState.isFetching,
      getTenantByIdState.isLoading,
      getTenantByIdState.isFetching,
      createTenantState.isLoading,
   ]);

   return {
      // States
      isLoading,
      getListTenantsState,
      getTenantByIdState,
      createTenantState,

      // Actions
      getListTenantsAsync,
      getTenantByIdAsync,
      createTenantAsync,
   };
};

export default useTenantService;
