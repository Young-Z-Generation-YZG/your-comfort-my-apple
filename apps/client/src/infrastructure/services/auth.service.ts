import { BaseQueryApi, createApi } from '@reduxjs/toolkit/query/react';
import {
   setLogin,
   setLogout,
   setUseAccessToken,
} from '../redux/features/auth.slice';
import { RootState } from '~/infrastructure/redux/store';
import { setPreviousUnAuthenticatedPath } from '../redux/features/app.slice';
import { EVerificationTypeEnum } from '~/domain/enums/verification-type.enum';
import {
   IRegisterPayload,
   TEmailVerificationResponse,
   IVerifyOtpPayload,
   ILoginPayload,
   ILoginResponse,
   IResetPasswordPayload,
   IChangePasswordPayload,
   ISendEmailResetPasswordPayload,
   IRefreshTokenResponse,
} from '~/domain/interfaces/identity.interface';
import { baseQuery } from './base-query';

const baseQueryHandler = async (
   args: any,
   api: BaseQueryApi,
   extraOptions: any,
) => {
   // baseQuery already handles 401 and refresh automatically
   // Only handle logout for refresh/logout endpoints that fail
   const result = await baseQuery('/identity-services')(
      args,
      api,
      extraOptions,
   );

   // If refresh or logout endpoint fails, logout user
   if (
      result.error &&
      result.error.status === 401 &&
      typeof args !== 'string' &&
      args.url &&
      (args.url.includes('/auth/refresh') || args.url.includes('/auth/logout'))
   ) {
      const currentRoute = window.location.pathname;
      api.dispatch(setPreviousUnAuthenticatedPath(currentRoute));
      api.dispatch(setLogout());
   }
   return result;
};

export const authApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      login: builder.mutation<ILoginResponse, ILoginPayload>({
         query: (payload: ILoginPayload) => ({
            url: '/api/v1/auth/login',
            method: 'POST',
            body: payload,
         }),
         async onQueryStarted(arg, { dispatch, queryFulfilled }) {
            try {
               const { data } = await queryFulfilled;

               if (
                  data.verification_type !==
                  EVerificationTypeEnum.EMAIL_VERIFICATION
               ) {
                  dispatch(
                     setLogin({
                        userId: 'USER_ID',
                        userEmail: data.user_email,
                        username: data.username,
                        accessToken: data.access_token,
                        refreshToken: data.refresh_token,
                        accessTokenExpiredIn: data.access_token_expires_in,
                     }),
                  );
               }
            } catch (error: unknown) {
               console.info(
                  '[AuthService]::login::try/catch',
                  JSON.stringify(error, null, 2),
               );
            }
         },
      }),
      logout: builder.mutation<void, void>({
         query: () => ({
            url: '/api/v1/auth/logout',
            method: 'POST',
         }),
         async onQueryStarted(arg, { dispatch, queryFulfilled }) {
            try {
               await queryFulfilled;

               dispatch(setUseAccessToken(false));
               dispatch(setLogout());
            } catch (error: unknown) {
               dispatch(setUseAccessToken(false));
               dispatch(setLogout());

               console.info(
                  '[AuthService]::logout::try/catch',
                  JSON.stringify(error, null, 2),
               );
            }
         },
      }),
      register: builder.mutation<TEmailVerificationResponse, IRegisterPayload>({
         query: (payload: IRegisterPayload) => ({
            url: '/api/v1/auth/register',
            method: 'POST',
            body: payload,
         }),
      }),
      verifyOtp: builder.mutation<boolean, IVerifyOtpPayload>({
         query: (payload: IVerifyOtpPayload) => ({
            url: '/api/v1/auth/verification/email',
            method: 'POST',
            body: payload,
         }),
      }),
      sendEmailResetPassword: builder.mutation<
         boolean,
         ISendEmailResetPasswordPayload
      >({
         query: (payload: ISendEmailResetPasswordPayload) => ({
            url: '/api/v1/auth/reset-password',
            method: 'POST',
            body: payload,
         }),
      }),
      resetPassword: builder.mutation<boolean, IResetPasswordPayload>({
         query: (payload: IResetPasswordPayload) => ({
            url: '/api/v1/auth/verification/reset-password',
            method: 'POST',
            body: payload,
         }),
      }),
      changePassword: builder.mutation<boolean, IChangePasswordPayload>({
         query: (payload: IChangePasswordPayload) => ({
            url: '/api/v1/auth/change-password',
            method: 'POST',
            body: payload,
         }),
      }),
      refreshToken: builder.mutation<IRefreshTokenResponse, void>({
         query: () => ({
            url: '/api/v1/auth/refresh',
            method: 'POST',
            // Refresh token sent via Bearer header in prepareHeaders
         }),
         async onQueryStarted(arg, { dispatch, queryFulfilled, getState }) {
            try {
               const { data } = await queryFulfilled;
               const currentState = (getState() as RootState).auth;

               dispatch(
                  setLogin({
                     userId: currentState.userId!,
                     userEmail: currentState.userEmail!,
                     username: currentState.username!,
                     accessToken: data.access_token,
                     refreshToken: data.refresh_token,
                     accessTokenExpiredIn: data.access_token_expires_in,
                  }),
               );
            } catch (error: unknown) {
               console.info(
                  '[AuthService]::refreshToken::try/catch',
                  JSON.stringify(error, null, 2),
               );
            }
         },
      }),
   }),
});

export const {
   useLoginMutation,
   useRegisterMutation,
   useVerifyOtpMutation,
   useSendEmailResetPasswordMutation,
   useResetPasswordMutation,
   useChangePasswordMutation,
   useLogoutMutation,
   useRefreshTokenMutation,
} = authApi;
