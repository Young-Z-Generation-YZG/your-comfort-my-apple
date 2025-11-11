import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/src/hooks/use-check-error';

import { toast } from 'sonner';
import {
   useLazyGetListUsersQuery,
   useLazyGetUsersByAdminQuery,
} from '~/src/infrastructure/services/identity.service';

const useIdentityService = () => {
   const [getUsersByAdminTrigger, getUsersByAdminQueryState] =
      useLazyGetUsersByAdminQuery();
   const [getListUsersTrigger, getListUsersQueryState] =
      useLazyGetListUsersQuery();

   useCheckApiError([]);

   const getUsersByAdminAsync = useCallback(
      async (params: any, options?: { useSuperAdminToken?: boolean }) => {
         try {
            // Merge custom option into params
            const queryParams = {
               ...params,
               __useSuperAdminToken: options?.useSuperAdminToken || false,
            };

            // Call RTK Query trigger with merged params
            const result = await getUsersByAdminTrigger(
               queryParams,
               false,
            ).unwrap();
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
      [getUsersByAdminTrigger],
   );

   const getListUsersAsync = useCallback(
      async (params: { roles: string[] }) => {
         try {
            const result = await getListUsersTrigger(params).unwrap();
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
      [getListUsersTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getUsersByAdminQueryState.isLoading ||
         getUsersByAdminQueryState.isFetching ||
         getListUsersQueryState.isLoading ||
         getListUsersQueryState.isFetching
      );
   }, [
      getUsersByAdminQueryState.isLoading,
      getUsersByAdminQueryState.isFetching,
      getListUsersQueryState.isLoading,
      getListUsersQueryState.isFetching,
   ]);

   return {
      // states
      isLoading,
      getUsersByAdminState: getUsersByAdminQueryState,
      getListUsersState: getListUsersQueryState,

      // actions
      getUsersByAdminAsync,
      getListUsersAsync,
   };
};

export default useIdentityService;
