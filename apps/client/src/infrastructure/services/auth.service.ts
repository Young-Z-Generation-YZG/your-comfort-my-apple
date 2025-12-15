import {
   BaseQueryApi,
   createApi,
   fetchBaseQuery,
   FetchBaseQueryError,
   FetchBaseQueryMeta,
   QueryReturnValue,
} from '@reduxjs/toolkit/query/react';
import {
   setLogin,
   setLogout,
   setUseAccessToken,
} from '../redux/features/auth.slice';
import { EVerificationType } from '~/domain/enums/verification-type.enum';
import envConfig from '~/infrastructure/config/env.config';
import {
   changePasswordFormType,
   resetPasswordFormType,
   sendEmailResetPasswordFormType,
} from '~/domain/schemas/auth.schema';
import { RootState } from '~/infrastructure/redux/store';
import { setPreviousUnAuthenticatedPath } from '../redux/features/app.slice';

const baseQuery = () =>
   fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + '/identity-services',
      prepareHeaders: (headers, { getState, endpoint }) => {
         const refreshToken = (getState() as RootState).auth.refreshToken;
         const tenantId = (getState() as RootState).app.tenantId;

         if (!headers.get('Authorization')) {
            const isLogoutRequest =
               endpoint === 'logout' ||
               (typeof endpoint === 'string' && endpoint.includes('logout'));

            if (isLogoutRequest && refreshToken) {
               headers.set('Authorization', `Bearer ${refreshToken}`);
            }
         }

         if (tenantId) {
            headers.set('X-TenantId', tenantId);
         }

         headers.set('ngrok-skip-browser-warning', 'true');
      },
   });

const baseQueryHandler = async (
   args: any,
   api: BaseQueryApi,
   extraOptions: any,
) => {
   const result = await baseQuery()(args, api, extraOptions);

   if (result.error && result.error.status === 401) {
      const currentRoute = window.location.pathname;

      api.dispatch(setPreviousUnAuthenticatedPath(currentRoute));
      api.dispatch(setLogout());
   }
   return result;
};

export interface IRegisterPayload {
   email: string;
   password: string;
   confirm_password: string;
   first_name: string;
   last_name: string;
   phone_number: string;
   birth_date: string;
   country: string;
}

export interface IVerifyOtpPayload {
   email: string;
   token: string;
   otp: string;
}

export type TEmailVerificationResponse = {
   params: {
      _email: string;
      _token: string;
   };
   verification_type: string;
   token_expired_in: number;
};

export const authApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      login: builder.mutation({
         query: (payload: any) => ({
            url: '/api/v1/auth/login',
            method: 'POST',
            body: payload,
         }),
         async onQueryStarted(arg, { dispatch, queryFulfilled }) {
            try {
               const { data } = await queryFulfilled;

               if (
                  data.verification_type !==
                  EVerificationType.EMAIL_VERIFICATION
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
               console.info(
                  '[AuthService]::login::try/catch',
                  JSON.stringify(error, null, 2),
               );
            }
         },
      }),
      logout: builder.mutation<void, void>({
         //   queryFn: async (_, { getState }, __, baseQuery) => {
         //     const refreshToken = (getState() as RootState).auth.refreshToken;

         //     const result = await baseQuery({
         //        url: '/api/v1/auth/logout',
         //        method: 'POST',
         //        headers: {
         //           Authorization: `Bearer ${refreshToken}`,
         //        },
         //     });

         //     return result as unknown as QueryReturnValue<
         //        void,
         //        FetchBaseQueryError,
         //        FetchBaseQueryMeta
         //     >;
         //  },
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
      register: builder.mutation<
         TEmailVerificationResponse | unknown,
         IRegisterPayload
      >({
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
      sendEmailResetPassword: builder.mutation({
         query: (payload: sendEmailResetPasswordFormType) => ({
            url: '/api/v1/auth/reset-password',
            method: 'POST',
            body: payload,
         }),
      }),
      resetPassword: builder.mutation({
         query: (payload: resetPasswordFormType) => ({
            url: '/api/v1/auth/verification/reset-password',
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
      }),
      refreshToken: builder.mutation({
         queryFn: async (_, { getState }, __, baseQuery) => {
            const refreshToken = (getState() as RootState).auth.refreshToken;

            if (!refreshToken) {
               return {
                  error: {
                     status: 401,
                     data: { message: 'No refresh token available' },
                  },
               } as unknown as QueryReturnValue<
                  any,
                  FetchBaseQueryError,
                  FetchBaseQueryMeta
               >;
            }

            const result = await baseQuery({
               url: '/api/v1/auth/refresh',
               method: 'POST',
               body: { refresh_token: refreshToken },
            });

            return result as unknown as QueryReturnValue<
               any,
               FetchBaseQueryError,
               FetchBaseQueryMeta
            >;
         },
         async onQueryStarted(arg, { dispatch, queryFulfilled, getState }) {
            try {
               const { data } = await queryFulfilled;
               const currentState = (getState() as RootState).auth;

               // Update tokens while preserving user info
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
