import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { ILoginPayload } from '~/domain/interfaces/auth/login.interface';
import { setLogin } from '../redux/features/auth.slice';
import { HttpErrorResponse } from '~/domain/interfaces/errors/error.interface';
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
import { toast } from 'sonner';
import { isFetchBaseQueryError } from '../utils/http/fetch-error-handler';

export const AuthApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'identity-services',
      prepareHeaders: (headers, { getState }) => {
         const accessToken = (getState() as RootState).auth.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      login: builder.mutation({
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
                     setLogin({
                        userId: data.user_id,
                        userEmail: data.user_email,
                        username: data.username,
                        accessToken: data.access_token,
                        refreshToken: data.refresh_token,
                        accessTokenExpiredIn: data.access_token_expires_in,
                     }),
                  );
               }
            } catch (error: unknown) {
               console.log(isFetchBaseQueryError(error));

               console.info(
                  '[AuthService]::login::try/catch',
                  JSON.stringify(error, null, 2),
               );
            }
         },
      }),
      register: builder.mutation({
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
      verifyOtp: builder.mutation({
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
      sendEmailResetPassword: builder.mutation({
         query: (payload: sendEmailResetPasswordFormType) => ({
            url: '/api/v1/auth/reset-password',
            method: 'POST',
            body: payload,
         }),
      }),
      resetPassword: builder.mutation({
         query: (payload: resetPasswordFormType) => ({
            url: '/api/v1/auth/reset-password/verification',
            method: 'POST',
            body: payload,
         }),
      }),
      changePassword: builder.mutation({
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
   useLoginMutation,
   useRegisterMutation,
   useVerifyOtpMutation,
   useSendEmailResetPasswordMutation,
   useResetPasswordMutation,
   useChangePasswordMutation,
} = AuthApi;
