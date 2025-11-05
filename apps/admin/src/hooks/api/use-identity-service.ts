import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/src/hooks/use-check-error';

import { toast } from 'sonner';
import { useLazyGetUsersByAdminQuery } from '~/src/infrastructure/services/identity.service';

const useIdentityService = () => {
   const [getUsersByAdminTrigger, getUsersByAdminQueryState] =
      useLazyGetUsersByAdminQuery();

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

   const isLoading = useMemo(() => {
      return (
         getUsersByAdminQueryState.isLoading ||
         getUsersByAdminQueryState.isFetching
      );
   }, [
      getUsersByAdminQueryState.isLoading,
      getUsersByAdminQueryState.isFetching,
   ]);

   // Centrally track success with toasts
   useMemo(() => {
      const successToastStyle = {
         style: {
            backgroundColor: '#DCFCE7',
            color: '#166534',
            border: '1px solid #86EFAC',
         },
         cancel: {
            label: 'Close',
            onClick: () => {},
            actionButtonStyle: {
               backgroundColor: '#16A34A',
               color: '#FFFFFF',
            },
         },
      };
   }, []);

   return {
      // states
      isLoading,
      getUsersByAdminState: getUsersByAdminQueryState,

      // actions
      getUsersByAdminAsync,
   };
};

export default useIdentityService;
