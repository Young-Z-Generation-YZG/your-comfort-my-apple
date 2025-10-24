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
import { useCheckApiError } from '../use-check-error';
import { IRegisterPayload } from '~/domain/interfaces/auth/register.interface';
import { IOtpPayload } from '~/domain/interfaces/auth/otp.interface';
import { useRouter } from 'next/navigation';

const useAuthService = () => {
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

   const router = useRouter();

   useCheckApiError([
      { title: 'Login failed', error: loginMutationState.error },
      { title: 'Register failed', error: registerMutationState.error },
      { title: 'Verify OTP failed', error: verifyOtpMutationState.error },
      {
         title: 'Send Email Reset Password failed',
         error: sendEmailResetPasswordMutationState.error,
      },
      {
         title: 'Reset Password failed',
         error: resetPasswordMutationState.error,
      },
      {
         title: 'Change Password failed',
         error: changePasswordMutationState.error,
      },
      // { title: 'Logout failed', error: logoutMutationState.error },
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

         router.push('/sign-in');

         return { isSuccess: true, isError: false, data: true, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [logoutMutation, router]);

   const register = useCallback(
      async (data: IRegisterPayload) => {
         try {
            const result = await registerMutation(data).unwrap();
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
      [registerMutation],
   );

   const verifyOtp = useCallback(
      async (data: IOtpPayload) => {
         try {
            const result = await verifyOtpMutation(data).unwrap();
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
      [verifyOtpMutation],
   );

   // centrally track the loading state
   const isLoading = useMemo(() => {
      return (
         loginMutationState.isLoading ||
         registerMutationState.isLoading ||
         verifyOtpMutationState.isLoading
      );
   }, [
      loginMutationState.isLoading,
      registerMutationState.isLoading,
      verifyOtpMutationState.isLoading,
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
      } else if (verifyOtpMutationState.isSuccess) {
         toast.success('Verification successful!', {
            style: {
               backgroundColor: '#DCFCE7',
               color: '#166534',
               border: '1px solid #86EFAC',
            },
         });
      }
   }, [loginMutationState, verifyOtpMutationState]);

   return {
      // States
      isLoading,
      isAuthenticated,

      // Actions
      login,
      logout,
      register,
      verifyOtp,

      // Mutation states
      loginState: loginMutationState,
      registerState: registerMutationState,
      verifyOtpState: verifyOtpMutationState,
   };
};

export default useAuthService;
