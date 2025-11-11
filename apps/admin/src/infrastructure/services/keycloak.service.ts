import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { setImpersonatedUser, setLogin } from '../redux/features/auth.slice';
import envConfig from '../config/env.config';
import { RootState } from '../redux/store';
import { setIsImpersonating, setIsLoading } from '../redux/features/app.slice';

export const keycloakApi = createApi({
   reducerPath: 'keycloak-api',
   tagTypes: ['keycloak'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'identity-services',
      prepareHeaders: (headers, { getState, endpoint }) => {
         const authAppState = (getState() as RootState).auth;
         let accessToken = null;

         // Endpoints that should always use admin token (not impersonated token)
         const adminOnlyEndpoints = ['impersonateUser'];
         const useAdminTokenOnly = adminOnlyEndpoints.includes(endpoint || '');

         if (useAdminTokenOnly) {
            // Always use current user's (admin's) token
            accessToken = authAppState.currentUser?.accessToken;
         } else {
            // Use impersonated user's token if available, otherwise use current user's
            if (authAppState.impersonatedUser) {
               accessToken = authAppState.impersonatedUser.accessToken;
            } else {
               accessToken = authAppState.currentUser?.accessToken;
            }
         }

         console.log('accessToken 2', accessToken);

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      authorizationCode: builder.mutation({
         query: (payload: unknown) => ({
            url: '/api/v1/keycloak/exchange-token/authorization-code',
            method: 'POST',
            body: payload,
         }),
         async onQueryStarted(arg, { dispatch, queryFulfilled }) {
            try {
               const { data } = await queryFulfilled;

               dispatch(
                  setLogin({
                     currentUser: {
                        userId: data.user_id,
                        userEmail: data.user_email,
                        username: data.username,
                        accessToken: data.access_token,
                        refreshToken: data.refresh_token,
                        accessTokenExpiredIn: data.access_token_expires_in,
                        refreshTokenExpiredIn: data.refresh_token_expires_in,
                     },
                  }),
               );
            } catch (error: unknown) {
               throw error;
            }
         },
      }),
      impersonateUser: builder.mutation({
         query: (userId: string) => ({
            url: '/api/v1/keycloak/exchange-token/impersonate-user',
            method: 'POST',
            body: { user_id: userId },
         }),
         async onQueryStarted(arg, { dispatch, queryFulfilled, getState }) {
            try {
               const { currentUser, impersonatedUser } = (
                  getState() as RootState
               ).auth;

               dispatch(setIsLoading(true));

               const { data } = await queryFulfilled;

               dispatch(setIsLoading(false));

               // Get current user's ID from state
               const impersonatedUserId =
                  (getState() as RootState).auth.impersonatedUser?.userId ??
                  null;

               if (data.access_token) {
                  dispatch(setIsImpersonating(true));

                  dispatch(
                     setImpersonatedUser({
                        impersonatedUser: {
                           userId: impersonatedUserId,
                           userEmail: data.user_email,
                           username: data.username,
                           accessToken: data.access_token,
                           refreshToken: data.refresh_token,
                        },
                     }),
                  );
               }
            } catch (error: unknown) {
               throw error;
            }
         },
      }),
   }),
});

export const { useAuthorizationCodeMutation, useImpersonateUserMutation } =
   keycloakApi;
