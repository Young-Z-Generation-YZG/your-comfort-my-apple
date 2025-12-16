/* eslint-disable react-hooks/exhaustive-deps */
import { useCallback, useMemo } from 'react';
import {
   useChangePasswordMutation,
   useLoginMutation,
   useResetPasswordMutation,
   useSendEmailResetPasswordMutation,
   useVerifyOtpMutation,
   useRegisterMutation,
   useLogoutMutation,
   useRefreshTokenMutation,
} from '~/infrastructure/services/auth.service';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useCheckApiError } from '~/hooks/use-check-error';
import { useRouter } from 'next/navigation';
import { setUseAccessToken } from '~/infrastructure/redux/features/auth.slice';
import { useDispatch } from 'react-redux';
import { EVerificationTypeEnum } from '~/domain/enums/verification-type.enum';
import { useCheckApiSuccess } from '~/hooks/use-check-success';
import { setPreviousUnAuthenticatedPath } from '~/infrastructure/redux/features/app.slice';
import {
   LoginFormType,
   sendEmailResetPasswordFormType,
   resetPasswordFormType,
   changePasswordFormType,
} from '~/domain/schemas/auth.schema';
import {
   IRegisterPayload,
   TEmailVerificationResponse,
   IVerifyOtpPayload,
} from '~/domain/interfaces/identity.interface';

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

   useCheckApiSuccess([
      {
         title: 'Login successful',
         isSuccess: loginMutationState.isSuccess,
         onSuccess: () => {
            if (
               loginMutationState.data &&
               typeof loginMutationState.data === 'object' &&
               'verification_type' in loginMutationState.data &&
               loginMutationState.data.verification_type ===
                  EVerificationTypeEnum.EMAIL_VERIFICATION
            ) {
               return 'Please verify your email to continue!';
            } else {
               return 'Welcome back! 123';
            }
         },
      },
      {
         title: 'Verify OTP successful',
         isSuccess: verifyOtpMutationState.isSuccess,
      },
      {
         title: 'Send Email Reset Password successful',
         isSuccess: sendEmailResetPasswordMutationState.isSuccess,
      },
      {
         title: 'Reset Password successful',
         isSuccess: resetPasswordMutationState.isSuccess,
      },
      {
         title: 'Change Password successful',
         isSuccess: changePasswordMutationState.isSuccess,
      },
      { title: 'Logout', isSuccess: logoutMutationState.isSuccess },
      {
         title: 'Refresh Token successful',
         isSuccess: refreshTokenMutationState.isSuccess,
      },
   ]);

   const authAppState = useAppSelector((state) => state.auth);

   const isAuthenticated = useMemo(() => {
      return Boolean(authAppState.accessToken && authAppState.isAuthenticated);
   }, [authAppState]);

   const loginAsync = useCallback(
      async (data: LoginFormType) => {
         try {
            const result = await loginMutation(data).unwrap();

            if (
               result &&
               typeof result === 'object' &&
               'verification_type' in result &&
               result.verification_type ===
                  EVerificationTypeEnum.EMAIL_VERIFICATION
            ) {
               const typedData =
                  result as unknown as TEmailVerificationResponse;

               const params = typedData.params;
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
            } else {
               router.replace(
                  appStateRoute.previousUnAuthenticatedPath || '/shop/iphone',
               );
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
         dispatch(setPreviousUnAuthenticatedPath(null));

         await logoutMutation().unwrap();

         router.push('/sign-in');

         return { isSuccess: true, isError: false, data: true, error: null };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [logoutMutation, router, dispatch]);

   const refreshToken = useCallback(async () => {
      try {
         const result = await refreshTokenMutation().unwrap();
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
               result.verification_type ===
                  EVerificationTypeEnum.EMAIL_VERIFICATION
            ) {
               const typedResult =
                  result as unknown as TEmailVerificationResponse;
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

   const verifyOtpAsync = useCallback(
      async (data: IVerifyOtpPayload) => {
         try {
            const result = await verifyOtpMutation(data).unwrap();

            if (result && typeof result === 'boolean' && result === true) {
               router.replace('/sign-in');
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

   return {
      // States
      isLoading,
      isAuthenticated,

      // Actions
      loginAsync,
      logout,
      registerAsync,
      verifyOtpAsync,
      refreshToken,
      sendEmailResetPassword,
      resetPassword,
      changePassword,

      // Mutation states
      loginMutationState,
      registerMutationState,
      verifyOtpMutationState,
      refreshTokenState: refreshTokenMutationState,
      sendEmailResetPasswordState: sendEmailResetPasswordMutationState,
      resetPasswordState: resetPasswordMutationState,
      changePasswordState: changePasswordMutationState,
   };
};

export default useAuthService;
