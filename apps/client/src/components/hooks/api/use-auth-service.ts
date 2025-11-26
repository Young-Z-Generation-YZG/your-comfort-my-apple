/* eslint-disable react-hooks/exhaustive-deps */
import { useCallback, useMemo } from 'react';
import {
   LoginFormType,
   sendEmailResetPasswordFormType,
   resetPasswordFormType,
   changePasswordFormType,
} from '~/domain/schemas/auth.schema';
import {
   useChangePasswordMutation,
   useLoginMutation,
   useResetPasswordMutation,
   useSendEmailResetPasswordMutation,
   useVerifyOtpMutation,
   useRegisterMutation,
   useLogoutMutation,
   useRefreshTokenMutation,
   IRegisterPayload,
   TEmailVerificationResponse,
} from '~/infrastructure/services/auth.service';
import { useAppSelector } from '~/infrastructure/redux/store';
import { toast } from 'sonner';
import { useCheckApiError } from '~/components/hooks/use-check-error';
import { useRouter } from 'next/navigation';
import { setUseAccessToken } from '~/infrastructure/redux/features/auth.slice';
import { useDispatch } from 'react-redux';
import { EVerificationType } from '~/domain/enums/verification-type.enum';

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
   const [refreshTokenMutation, refreshTokenMutationState] =
      useRefreshTokenMutation();

   const dispatch = useDispatch();
   const router = useRouter();

   const appStateRoute = useAppSelector((state) => state.app.route);

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
      { title: 'Logout failed', error: logoutMutationState.error },
      { title: 'Refresh Token failed', error: refreshTokenMutationState.error },
   ]);

   const authAppState = useAppSelector((state) => state.auth);

   const isAuthenticated = useMemo(() => {
      return Boolean(authAppState.accessToken && authAppState.isAuthenticated);
   }, [authAppState]);

   const loginAsync = useCallback(
      async (data: LoginFormType) => {
         try {
            const result = await loginMutation(data).unwrap();

            if (result.isSuccess && result.data) {
               if (
                  result.data.verification_type ===
                  EVerificationType.EMAIL_VERIFICATION
               ) {
                  const params = result.data.params;
                  const queryParams = new URLSearchParams();

                  for (const key in params) {
                     if (params.hasOwnProperty(key)) {
                        queryParams.set(key, String(params[key]));
                     }
                  }

                  router.push(`/verify/otp?${queryParams.toString()}`, {
                     scroll: false,
                  });
               } else {
                  router.replace(
                     appStateRoute.previousUnAuthenticatedPath ||
                        '/shop/iphone',
                  );
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
      [loginMutation],
   );

   const logout = useCallback(async () => {
      try {
         dispatch(setUseAccessToken(false));

         await logoutMutation().unwrap();

         router.push('/sign-in');

         return { isSuccess: true, isError: false, data: true, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [logoutMutation, router, dispatch]);

   const refreshToken = useCallback(async () => {
      try {
         const result = await refreshTokenMutation(null).unwrap();
         return {
            isSuccess: true,
            isError: false,
            data: result,
            error: null,
         };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [refreshTokenMutation]);

   const registerAsync = useCallback(
      async (data: IRegisterPayload) => {
         try {
            const result = await registerMutation(data).unwrap();

            if (
               result &&
               typeof result === 'object' &&
               'verification_type' in result &&
               result.verification_type === EVerificationType.EMAIL_VERIFICATION
            ) {
               const typedResult = result as TEmailVerificationResponse;
               const params = typedResult.params;

               const queryParams = new URLSearchParams();

               for (const key in params) {
                  if (params.hasOwnProperty(key)) {
                     queryParams.set(
                        key,
                        String(params[key as keyof typeof params]),
                     );
                  }
               }

               router.push(`/verify/otp?${queryParams.toString()}`, {
                  scroll: false,
               });
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
      [registerMutation],
   );

   const verifyOtp = useCallback(
      async (data: any) => {
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

   const sendEmailResetPassword = useCallback(
      async (data: sendEmailResetPasswordFormType) => {
         try {
            const result = await sendEmailResetPasswordMutation(data).unwrap();
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
      [sendEmailResetPasswordMutation],
   );

   const resetPassword = useCallback(
      async (data: resetPasswordFormType) => {
         try {
            const result = await resetPasswordMutation(data).unwrap();
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
      [resetPasswordMutation],
   );

   const changePassword = useCallback(
      async (data: changePasswordFormType) => {
         try {
            const result = await changePasswordMutation(data).unwrap();
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
      [changePasswordMutation],
   );

   // centrally track the loading state
   const isLoading = useMemo(() => {
      return (
         loginMutationState.isLoading ||
         registerMutationState.isLoading ||
         verifyOtpMutationState.isLoading ||
         logoutMutationState.isLoading ||
         refreshTokenMutationState.isLoading ||
         sendEmailResetPasswordMutationState.isLoading ||
         resetPasswordMutationState.isLoading ||
         changePasswordMutationState.isLoading
      );
   }, [
      loginMutationState.isLoading,
      registerMutationState.isLoading,
      verifyOtpMutationState.isLoading,
      logoutMutationState.isLoading,
      refreshTokenMutationState.isLoading,
      sendEmailResetPasswordMutationState.isLoading,
      resetPasswordMutationState.isLoading,
      changePasswordMutationState.isLoading,
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
      } else if (sendEmailResetPasswordMutationState.isSuccess) {
         toast.success('Reset password email sent!', {
            style: {
               backgroundColor: '#DCFCE7',
               color: '#166534',
               border: '1px solid #86EFAC',
            },
         });
      } else if (resetPasswordMutationState.isSuccess) {
         toast.success('Password reset successful!', {
            style: {
               backgroundColor: '#DCFCE7',
               color: '#166534',
               border: '1px solid #86EFAC',
            },
         });
      } else if (changePasswordMutationState.isSuccess) {
         toast.success('Password changed successfully!', {
            style: {
               backgroundColor: '#DCFCE7',
               color: '#166534',
               border: '1px solid #86EFAC',
            },
         });
      }
   }, [
      loginMutationState,
      verifyOtpMutationState,
      sendEmailResetPasswordMutationState,
      resetPasswordMutationState,
      changePasswordMutationState,
   ]);

   return {
      // States
      isLoading,
      isAuthenticated,

      // Actions
      loginAsync,
      logout,
      registerAsync,
      verifyOtp,
      refreshToken,
      sendEmailResetPassword,
      resetPassword,
      changePassword,

      // Mutation states
      loginMutationState,
      registerMutationState,
      verifyOtpState: verifyOtpMutationState,
      refreshTokenState: refreshTokenMutationState,
      sendEmailResetPasswordState: sendEmailResetPasswordMutationState,
      resetPasswordState: resetPasswordMutationState,
      changePasswordState: changePasswordMutationState,
   };
};

export default useAuthService;
