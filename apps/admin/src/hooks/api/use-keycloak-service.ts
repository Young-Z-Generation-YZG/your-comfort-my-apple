import { useCallback, useMemo } from 'react';

import { toast } from 'sonner';
import { useCheckApiError } from '~/src/hooks/use-check-error';
import {
   useAuthorizationCodeMutation,
   useImpersonateUserMutation,
} from '~/src/infrastructure/services/keycloak.service';
import { useDispatch } from 'react-redux';
import { setImpersonatedUser } from '~/src/infrastructure/redux/features/auth.slice';
import { RootState, useAppSelector } from '~/src/infrastructure/redux/store';
import { setTenant } from '~/src/infrastructure/redux/features/tenant.slice';

const useKeycloakService = () => {
   const [authorizationCodeMutation, authorizationCodeMutationState] =
      useAuthorizationCodeMutation();
   const [impersonateUserMutation, impersonateUserMutationState] =
      useImpersonateUserMutation();

   const dispatch = useDispatch();

   const { currentUser, impersonatedUser } = useAppSelector(
      (state: RootState) => state.auth,
   );

   useCheckApiError([
      {
         title: 'Authorization code failed',
         error: authorizationCodeMutationState.error,
      },
      {
         title: 'Impersonate user failed',
         error: impersonateUserMutationState.error,
      },
   ]);

   const authorizationCodeAsync = useCallback(
      async (data: unknown) => {
         try {
            const result = await authorizationCodeMutation(data).unwrap();
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
      [authorizationCodeMutation],
   );

   const impersonateUserAsync = useCallback(
      async (userId: string) => {
         try {
            dispatch(
               setImpersonatedUser({
                  impersonatedUser: {
                     userId: userId,
                  },
               }),
            );

            if (userId === currentUser?.userId) {
               dispatch(
                  setTenant({
                     tenantId: null,
                  }),
               );
            }

            const result = await impersonateUserMutation(userId).unwrap();

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
      [impersonateUserMutation, dispatch, currentUser],
   );

   // centrally track the loading state
   const isLoading = useMemo(() => {
      return (
         authorizationCodeMutationState.isLoading ||
         impersonateUserMutationState.isLoading
      );
   }, [
      authorizationCodeMutationState.isLoading,
      impersonateUserMutationState.isLoading,
   ]);

   // centrally track the success
   useMemo(() => {
      if (authorizationCodeMutationState.isSuccess) {
         toast.success('Welcome!', {
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
         });
      }
   }, [authorizationCodeMutationState]);

   return {
      // States
      isLoading,

      // Actions
      authorizationCodeAsync,
      impersonateUserAsync,

      // Mutation states
      authorizationCodeState: authorizationCodeMutationState,
      impersonateUserState: impersonateUserMutationState,
   };
};

export default useKeycloakService;
