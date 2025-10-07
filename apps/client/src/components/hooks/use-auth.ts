import { useCallback, useMemo } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import { useDispatch } from 'react-redux';
import { useAppSelector } from '~/infrastructure/redux/store';
import {
   setAccessToken,
   setLogout,
   setTimerId,
} from '~/infrastructure/redux/features/auth.slice';
import {
   useLoginAsyncMutation,
   useRegisterAsyncMutation,
   useVerifyOtpAsyncMutation,
   useSendEmailResetPasswordAsyncMutation,
   useResetPasswordAsyncMutation,
   useChangePasswordAsyncMutation,
} from '~/infrastructure/services/auth.service';
import { IRegisterPayload } from '~/domain/interfaces/auth/register.interface';
import { IOtpPayload } from '~/domain/interfaces/auth/otp.interface';
import {
   IResetPasswordPayload,
   IChangePasswordPayload,
} from '~/domain/interfaces/auth/password.interface';
import { LoginFormType } from '~/domain/schemas/auth.schema';

/**
 * Custom hook for authentication management
 *
 * Provides:
 * - Current auth state (user, tokens, authentication status)
 * - Auth mutations (login, register, logout, password management)
 * - Helper methods (isAuthenticated, requireAuth, etc.)
 * - Navigation helpers for protected routes
 *
 * @example
 * ```tsx
 * const MyComponent = () => {
 *   const { user, isAuthenticated, login, logout } = useAuth();
 *
 *   return (
 *     <div>
 *       {isAuthenticated ? (
 *         <button onClick={logout}>Logout {user.username}</button>
 *       ) : (
 *         <button onClick={() => login(credentials)}>Login</button>
 *       )}
 *     </div>
 *   );
 * };
 * ```
 */
export const useAuth = () => {
   const dispatch = useDispatch();
   const router = useRouter();
   const pathname = usePathname();

   // Get auth state from Redux
   const authState = useAppSelector((state) => state.auth.value);

   // RTK Query mutations
   const [loginMutation, loginMutationState] = useLoginAsyncMutation();
   const [registerMutation, registerMutationState] = useRegisterAsyncMutation();
   const [verifyOtpMutation, verifyOtpMutationState] =
      useVerifyOtpAsyncMutation();
   const [sendEmailResetPasswordMutation] =
      useSendEmailResetPasswordAsyncMutation();
   const [resetPasswordMutation, resetPasswordMutationState] =
      useResetPasswordAsyncMutation();
   const [changePasswordMutation, changePasswordMutationState] =
      useChangePasswordAsyncMutation();

   // Computed values
   const isAuthenticated = useMemo(() => {
      return Boolean(authState.accessToken && authState.isAuthenticated);
   }, [authState.accessToken, authState.isAuthenticated]);

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

   const user = useMemo(() => {
      if (!isAuthenticated) return null;

      return {
         email: authState.userEmail,
         username: authState.username,
      };
   }, [authState.userEmail, authState.username, isAuthenticated]);

   // Auth actions
   const login = useCallback(
      async (credentials: LoginFormType) => {
         try {
            const result = await loginMutation(credentials).unwrap();
            return { isSuccess: true, isError: false, data: result };
         } catch (error) {
            console.error('Login failed:', error);
            return { isSuccess: false, isError: true, error };
         }
      },
      [loginMutation],
   );

   const register = useCallback(
      async (userData: IRegisterPayload) => {
         try {
            const result = await registerMutation(userData).unwrap();
            return { success: true, data: result };
         } catch (error) {
            console.error('Registration failed:', error);
            return { success: false, error };
         }
      },
      [registerMutation],
   );

   const logout = useCallback(() => {
      // Clear auth state
      dispatch(setLogout());

      // Clear any timers
      if (authState.timerId) {
         clearTimeout(authState.timerId);
      }

      // Redirect to sign-in page
      router.push('/sign-in');
   }, [dispatch, router, authState.timerId]);

   const verifyOtp = useCallback(
      async (otpData: IOtpPayload) => {
         try {
            const result = await verifyOtpMutation(otpData).unwrap();
            return { success: true, data: result };
         } catch (error) {
            console.error('OTP verification failed:', error);
            return { success: false, error };
         }
      },
      [verifyOtpMutation],
   );

   const sendResetPasswordEmail = useCallback(
      async (email: string) => {
         try {
            const result = await sendEmailResetPasswordMutation({
               email,
            }).unwrap();
            return { success: true, data: result };
         } catch (error) {
            console.error('Send reset password email failed:', error);
            return { success: false, error };
         }
      },
      [sendEmailResetPasswordMutation],
   );

   const resetPassword = useCallback(
      async (resetData: IResetPasswordPayload) => {
         try {
            const result = await resetPasswordMutation(resetData).unwrap();
            return { success: true, data: result };
         } catch (error) {
            console.error('Reset password failed:', error);
            return { success: false, error };
         }
      },
      [resetPasswordMutation],
   );

   const changePassword = useCallback(
      async (passwordData: IChangePasswordPayload) => {
         try {
            const result = await changePasswordMutation(passwordData).unwrap();
            return { success: true, data: result };
         } catch (error) {
            console.error('Change password failed:', error);
            return { success: false, error };
         }
      },
      [changePasswordMutation],
   );

   // Helper methods
   const updateTokens = useCallback(
      (tokens: {
         user_email: string;
         username: string;
         access_token: string | null;
         refresh_token: string | null;
         access_token_expires_in: number | null;
      }) => {
         dispatch(setAccessToken(tokens));
      },
      [dispatch],
   );

   const setTimer = useCallback(
      (timerId: string | null) => {
         dispatch(setTimerId(timerId));
      },
      [dispatch],
   );

   /**
    * Checks if user is authenticated and redirects to sign-in if not
    */
   const requireAuth = useCallback(() => {
      if (!isAuthenticated) {
         router.push('/sign-in');
         return false;
      }
      return true;
   }, [isAuthenticated, router]);

   /**
    * Redirects authenticated users away from auth pages
    */
   const redirectIfAuthenticated = useCallback(
      (redirectTo: string = '/') => {
         if (
            isAuthenticated &&
            (pathname === '/sign-in' || pathname === '/sign-up')
         ) {
            router.push(redirectTo);
            return true;
         }
         return false;
      },
      [isAuthenticated, pathname, router],
   );

   /**
    * Checks if current route is a public auth page
    */
   const isPublicAuthPage = useCallback(() => {
      return pathname === '/sign-in' || pathname === '/sign-up';
   }, [pathname]);

   return {
      // State
      authState,
      user,
      isAuthenticated,
      isLoading,
      isLogoutTriggered: authState.isLogoutTriggered,
      accessToken: authState.accessToken,
      refreshToken: authState.refreshToken,

      // Actions
      login,
      logout,
      register,
      verifyOtp,
      sendResetPasswordEmail,
      resetPassword,
      changePassword,
      updateTokens,
      setTimer,

      // Helpers
      requireAuth,
      redirectIfAuthenticated,
      isPublicAuthPage,

      // Mutation states (for handling loading/error states)
      loginState: loginMutationState,
      registerState: registerMutationState,
      verifyOtpState: verifyOtpMutationState,
      resetPasswordState: resetPasswordMutationState,
      changePasswordState: changePasswordMutationState,
   };
};

export default useAuth;
