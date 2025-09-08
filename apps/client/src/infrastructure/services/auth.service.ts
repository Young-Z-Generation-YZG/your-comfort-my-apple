import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   ILoginPayload,
} from '~/domain/interfaces/auth/login.interface';
import { setAccessToken } from '../redux/features/auth.slice';
import {
   HttpErrorResponse,
} from '~/domain/interfaces/errors/error.interface';
import { IOtpPayload } from '~/domain/interfaces/auth/otp.interface';
import {
   IRegisterPayload,
   IRegisterResponse,
} from '~/domain/interfaces/auth/register.interface';
import { VERIFICATION_TYPES } from '~/domain/enums/verification-type.enum';
import envConfig from '~/infrastructure/config/env.config';
import {
   changePasswordFormType,
   resetPasswordFormType,
   sendEmailResetPasswordFormType,
} from '~/domain/schemas/auth.schema';
import { RootState } from '../redux/store';

export const AuthApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'identity-services',
      prepareHeaders: (headers, { getState }) => {
         const accessToken = (getState() as RootState).auth.value.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      loginAsync: builder.mutation({
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
                  VERIFICATION_TYPES.EMAIL_VERIFICATION
               ) {
                  dispatch(
                     setAccessToken({
                        user_email: data.user_email,
                        username: data.username,
                        access_token: data.access_token,
                        refresh_token: data.refresh_token,
                        access_token_expires_in: data.access_token_expires_in,
                     }),
                  );
               }
            } catch (error) {
               console.warn('[TRY/CATCH ERROR]::', error);
            }
         },
      }),
      registerAsync: builder.mutation({
         query: (payload: IRegisterPayload) => ({
            url: '/api/v1/auth/register',
            method: 'POST',
            body: payload,
         }),
         transformResponse: (response: IRegisterResponse) => {
            return response;
         },
         transformErrorResponse: (error: HttpErrorResponse) => {
            return error.data;
         },
      }),
      verifyOtpAsync: builder.mutation({
         query: (payload: IOtpPayload) => ({
            url: '/api/v1/auth/email/verification',
            method: 'POST',
            body: payload,
         }),
         transformErrorResponse: (error: HttpErrorResponse) => {
            return error.data;
         },
         transformResponse: (response: boolean) => {
            return response;
         },
      }),
      sendEmailResetPasswordAsync: builder.mutation({
         query: (payload: sendEmailResetPasswordFormType) => ({
            url: '/api/v1/auth/reset-password',
            method: 'POST',
            body: payload,
         }),
      }),
      resetPasswordAsync: builder.mutation({
         query: (payload: resetPasswordFormType) => ({
            url: '/api/v1/auth/reset-password/verification',
            method: 'POST',
            body: payload,
         }),
      }),
      changePasswordAsync: builder.mutation({
         query: (payload: changePasswordFormType) => ({
            url: '/api/v1/auth/change-password',
            method: 'POST',
            body: payload,
         }),
         transformErrorResponse: (error: HttpErrorResponse) => {
            return error.data;
         },
      }),
   }),
});

export const {
   useLoginAsyncMutation,
   useRegisterAsyncMutation,
   useVerifyOtpAsyncMutation,
   useSendEmailResetPasswordAsyncMutation,
   useResetPasswordAsyncMutation,
   useChangePasswordAsyncMutation,
} = AuthApi;
