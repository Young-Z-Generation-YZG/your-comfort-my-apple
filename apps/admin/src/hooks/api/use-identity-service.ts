import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/src/hooks/use-check-error';

import { toast } from 'sonner';
import { useLazyGetUsersByAdminQuery } from '~/src/infrastructure/services/identity.service';

const useIdentityService = () => {
   const [getUsersByAdminTrigger, getUsersByAdminQueryState] =
      useLazyGetUsersByAdminQuery();

   useCheckApiError([]);

   const getUsersByAdminAsync = useCallback(
      async (params: any) => {
         try {
            const result = await getUsersByAdminTrigger(params).unwrap();
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
         getUsersByAdminQueryState.isLoading
      );
   }, [getUsersByAdminQueryState.isLoading]);

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
