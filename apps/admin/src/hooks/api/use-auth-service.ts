import { useCallback, useMemo } from 'react';
import {
   useLazyGetIdentityQuery,
   useLoginMutation,
   useLogoutMutation,
} from '~/src/infrastructure/services/auth.service';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { toast } from 'sonner';
import { useCheckApiError } from '~/src/hooks/use-check-error';
import { useRouter } from 'next/navigation';
import { setUseRefreshToken } from '~/src/infrastructure/redux/features/auth.slice';
import { useDispatch } from 'react-redux';

const useAuthService = () => {
   const [loginMutation, loginMutationState] = useLoginMutation();
   const [logoutMutation, logoutMutationState] = useLogoutMutation();
   const [getIdentityTrigger, getIdentityState] = useLazyGetIdentityQuery();

   const dispatch = useDispatch();
   const router = useRouter();

   useCheckApiError([
      { title: 'Login failed', error: loginMutationState.error },
      { title: 'Logout failed', error: logoutMutationState.error },
   ]);

   const authAppState = useAppSelector((state) => state.auth);

   const isAuthenticated = useMemo(() => {
      return Boolean(authAppState.currentUser?.accessToken);
   }, [authAppState]);

   const loginAsync = useCallback(
      async (data: any) => {
         try {
            const result = await loginMutation(data).unwrap();
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
      [loginMutation],
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
         const result = await getIdentityTrigger({}).unwrap();
         return { isSuccess: true, isError: false, data: result, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [getIdentityTrigger]);

   // centrally track the loading state
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

      // Actions
      loginAsync,
      logoutAsync,
      getIdentityAsync,

      // Mutation states
      loginState: loginMutationState,
   };
};

export default useAuthService;
