import { useCallback, useMemo } from 'react';

import { toast } from 'sonner';
import { useCheckApiError } from '~/src/hooks/use-check-error';
import {
   useAuthorizationCodeMutation,
   useImpersonateUserMutation,
} from '~/src/infrastructure/services/keycloak.service';
import { useDispatch } from 'react-redux';
import {
   setCurrentUserKey,
   setIdentity,
   setImpersonatedUser,
   setRoles,
} from '~/src/infrastructure/redux/features/auth.slice';
import { RootState, useAppSelector } from '~/src/infrastructure/redux/store';
import useAuthService from './use-auth-service';

const useKeycloakService = () => {
   const [authorizationCodeMutation, authorizationCodeMutationState] =
      useAuthorizationCodeMutation();
   const [impersonateUserMutation, impersonateUserMutationState] =
      useImpersonateUserMutation();

   const { getIdentityAsync } = useAuthService();

   const dispatch = useDispatch();

   const { currentUser } = useAppSelector((state: RootState) => state.auth);

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
            // If selecting current user, stop impersonation without API call
            if (userId === currentUser?.userId) {
               // First switch back to current user (this changes the token used in API calls)
               dispatch(setCurrentUserKey('currentUser'));
               dispatch(
                  setImpersonatedUser({
                     impersonatedUser: null,
                  }),
               );

               // Now fetch identity using current user's token to get roles
               const identityResult = await getIdentityAsync();

               // Update roles for current user if identity fetch succeeded
               if (identityResult.isSuccess && identityResult.data) {
                  dispatch(
                     setRoles({
                        currentUser: {
                           roles: identityResult.data.roles || null,
                        },
                        impersonatedUser: null,
                     }),
                  );
               }

               return {
                  isSuccess: true,
                  isError: false,
                  data: null,
                  error: null,
               };
            }

            // Start impersonating new user
            dispatch(setCurrentUserKey('impersonatedUser'));

            // Call API to get impersonation tokens
            const result = await impersonateUserMutation(userId).unwrap();

            // Fetch identity for the impersonated user
            const identityResult = await getIdentityAsync();

            if (identityResult.isSuccess && identityResult.data) {
               // Store identity data
               dispatch(
                  setIdentity({
                     currentUser: null,
                     impersonatedUser: {
                        userId: identityResult.data.id,
                        tenantId: identityResult.data.tenant_id,
                        branchId: identityResult.data.branch_id,
                        tenantSubDomain: identityResult.data.tenant_sub_domain,
                     },
                  }),
               );

               // Store roles for the impersonated user
               dispatch(
                  setRoles({
                     currentUser: null,
                     impersonatedUser: {
                        roles: identityResult.data.roles || null,
                     },
                  }),
               );
            }

            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            // On error, revert to current user
            dispatch(setCurrentUserKey('currentUser'));
            dispatch(
               setImpersonatedUser({
                  impersonatedUser: null,
               }),
            );
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [impersonateUserMutation, dispatch, currentUser, getIdentityAsync],
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
