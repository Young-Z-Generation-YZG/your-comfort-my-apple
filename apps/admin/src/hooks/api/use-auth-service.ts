import { useCallback, useMemo } from 'react';
import {
   useLoginMutation,
   useLogoutMutation,
} from '~/src/infrastructure/services/auth.service';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { toast } from 'sonner';
import { useCheckApiError } from '~/src/hooks/use-check-error';
import { useRouter } from 'next/navigation';

const useAuthService = () => {
   const [loginMutation, loginMutationState] = useLoginMutation();
   const [logoutMutation, logoutMutationState] = useLogoutMutation();

   const router = useRouter();

   useCheckApiError([
      { title: 'Login failed', error: loginMutationState.error },
      // { title: 'Logout failed', error: logoutMutationState.error },
   ]);

   const authAppState = useAppSelector((state) => state.auth);

   const isAuthenticated = useMemo(() => {
      return Boolean(authAppState.accessToken && authAppState.isAuthenticated);
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
         await logoutMutation().unwrap();

         router.replace('/auth/sign-in');

         return { isSuccess: true, isError: false, data: true, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [logoutMutation, router]);

   // centrally track the loading state
   const isLoading = useMemo(() => {
      return loginMutationState.isLoading;
   }, [loginMutationState.isLoading]);

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

      // Actions
      loginAsync,
      logoutAsync,

      // Mutation states
      loginState: loginMutationState,
   };
};

export default useAuthService;
