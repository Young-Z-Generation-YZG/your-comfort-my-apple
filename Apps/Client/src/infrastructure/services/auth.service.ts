import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   ILoginPayload,
   ILoginResponse,
} from '~/domain/interfaces/auth/login.interface';
import { setAccessToken } from '../redux/features/auth.slice';
import {
   CatchErrorResponse,
   HttpErrorResponse,
} from '~/domain/interfaces/errors/error.interface';
import { IOtpPayload } from '~/domain/interfaces/auth/otp.interface';
import {
   IRegisterPayload,
   IRegisterResponse,
} from '~/domain/interfaces/auth/register.interface';
import { VERIFICATION_TYPES } from '~/domain/enums/verification-type.enum';

export const AuthApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://54ff-116-108-46-152.ngrok-free.app',
      prepareHeaders: (headers) => {
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
         transformResponse: (response: ILoginResponse) => {
            return response;
         },
         transformErrorResponse: (error: HttpErrorResponse) => {
            return error.data;
         },
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
                        access_token: data.access_token,
                        refresh_token: data.refresh_token,
                        access_token_expires_in: data.access_token_expires_in,
                     }),
                  );
               }
            } catch (error) {
               const serverError = error as CatchErrorResponse;
               console.error('Login failed:', serverError.error);
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
   }),
});

export const {
   useLoginAsyncMutation,
   useRegisterAsyncMutation,
   useVerifyOtpAsyncMutation,
} = AuthApi;
