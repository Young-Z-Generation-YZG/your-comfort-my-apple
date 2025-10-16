import { useCallback, useMemo } from 'react';
import { LoginFormType } from '~/domain/schemas/auth.schema';
import {
   useChangePasswordMutation,
   useLoginMutation,
   useResetPasswordMutation,
   useSendEmailResetPasswordMutation,
   useVerifyOtpMutation,
   useRegisterMutation,
   useLogoutMutation,
} from '~/infrastructure/services/auth.service';
import { useAppSelector } from '~/infrastructure/redux/store';
import { toast } from 'sonner';
import { useCheckApiError } from './use-check-error';

const useAuth = () => {
   const [loginMutation, loginMutationState] = useLoginMutation();
   const [registerMutation, registerMutationState] = useRegisterMutation();
   const [verifyOtpMutation, verifyOtpMutationState] = useVerifyOtpMutation();
   const [sendEmailResetPasswordMutation, sendEmailResetPasswordMutationState] =
      useSendEmailResetPasswordMutation();
   const [resetPasswordMutation, resetPasswordMutationState] =
      useResetPasswordMutation();
   const [changePasswordMutation, changePasswordMutationState] =
      useChangePasswordMutation();
   const [logoutMutation, logoutMutationState] = useLogoutMutation();

   useCheckApiError([
      { title: 'Login failed', error: loginMutationState.error },
   ]);

   const authAppState = useAppSelector((state) => state.auth);

   const isAuthenticated = useMemo(() => {
      return Boolean(authAppState.accessToken && authAppState.isAuthenticated);
   }, [authAppState]);

   const login = useCallback(
      async (data: LoginFormType) => {
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

   const logout = useCallback(async () => {
      try {
         await logoutMutation().unwrap();
         return { isSuccess: true, isError: false, data: true, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [logoutMutation]);

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
      login,
      logout,
   };
};

export default useAuth;
