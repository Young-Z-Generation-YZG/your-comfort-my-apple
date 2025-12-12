import { useCallback, useMemo } from 'react';
import {
   ILoginPayload,
   useLazyGetIdentityQuery,
   useLoginMutation,
   useLogoutMutation,
} from '~/src/infrastructure/services/auth.service';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { toast } from 'sonner';
import { useCheckApiError } from '~/src/hooks/use-check-error';
import { useRouter } from 'next/navigation';
import {
   setIdentity,
   setLogin,
   setRoles,
   setUseRefreshToken,
} from '~/src/infrastructure/redux/features/auth.slice';
import { useDispatch } from 'react-redux';
import { ERole } from '~/src/domain/enums/role.enum';
import { setTenant } from '~/src/infrastructure/redux/features/tenant.slice';
import { useAddNewStaffMutation } from '~/src/infrastructure/services/identity.service';
import { IAddNewStaffPayload } from '~/src/domain/types/identity.type';

const useAuthService = () => {
   const [loginMutation, loginMutationState] = useLoginMutation();
   const [logoutMutation, logoutMutationState] = useLogoutMutation();
   const [getIdentityTrigger, getIdentityState] = useLazyGetIdentityQuery();
   const [addNewStaffMutation, addNewStaffState] = useAddNewStaffMutation();

   const dispatch = useDispatch();
   const router = useRouter();

   useCheckApiError([
      { title: 'Login failed', error: loginMutationState.error },
      { title: 'Logout failed', error: logoutMutationState.error },
      { title: 'Add staff failed', error: addNewStaffState.error },
   ]);

   const authAppState = useAppSelector((state) => state.auth);

   const isAuthenticated = useMemo(() => {
      return Boolean(authAppState.currentUser?.accessToken);
   }, [authAppState]);

   const loginAsync = useCallback(
      async (data: ILoginPayload) => {
         try {
            const result = await loginMutation(data).unwrap();

            if (result) {
               const identityResult = await getIdentityTrigger().unwrap();

               if (identityResult) {
                  dispatch(
                     setRoles({
                        currentUser: {
                           roles: identityResult.roles,
                        },
                        impersonatedUser: null,
                     }),
                  );
                  dispatch(
                     setIdentity({
                        currentUser: {
                           userId: identityResult.id,
                           tenantId: identityResult.tenant_id,
                           branchId: identityResult.branch_id,
                           tenantSubDomain: identityResult.tenant_sub_domain,
                        },
                        impersonatedUser: null,
                     }),
                  );

                  if (!identityResult.roles.includes(ERole.ADMIN_SUPER)) {
                     dispatch(
                        setTenant({
                           tenantId: identityResult.tenant_id,
                           branchId: identityResult.branch_id,
                           tenantSubDomain: identityResult.tenant_sub_domain,
                        }),
                     );
                  }
               }
            }

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
      [loginMutation, getIdentityTrigger, dispatch],
   );

   const logoutAsync = useCallback(async () => {
      try {
         dispatch(setUseRefreshToken(true));

         await logoutMutation().unwrap();

         router.replace('/auth/sign-in');

         return { isSuccess: true, isError: false, data: true, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [logoutMutation, router, dispatch]);

   const getIdentityAsync = useCallback(async () => {
      try {
         const result = await getIdentityTrigger().unwrap();
         return { isSuccess: true, isError: false, data: result, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [getIdentityTrigger]);

   const addNewStaffAsync = useCallback(
      async (payload: IAddNewStaffPayload) => {
         try {
            const result = await addNewStaffMutation(payload).unwrap();
            toast.success('Staff member added successfully');
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
      [addNewStaffMutation],
   );

   // centrally track the loading state (auth-related only)
   const isLoading = useMemo(() => {
      return (
         loginMutationState.isLoading ||
         getIdentityState.isLoading ||
         getIdentityState.isFetching
      );
   }, [
      loginMutationState.isLoading,
      getIdentityState.isLoading,
      getIdentityState.isFetching,
   ]);

   // centrally track the success
   useMemo(() => {
      if (loginMutationState.isSuccess) {
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
   }, [loginMutationState]);

   return {
      // States
      isLoading,
      isAuthenticated,
      getIdentityState,
      addNewStaffState,

      // Actions
      loginAsync,
      logoutAsync,
      getIdentityAsync,
      addNewStaffAsync,

      // Mutation states
      loginState: loginMutationState,
   };
};

export default useAuthService;
