// import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
// import { setLogin } from '../redux/features/auth.slice';

// export const keycloakApi = createApi({
//    reducerPath: 'keycloak-api',
//    tagTypes: ['keycloak'],
//    baseQuery: fetchBaseQuery({
//       baseUrl: 'https://1f6e-116-108-118-49.ngrok-free.app',
//       prepareHeaders: (headers) => {
//          headers.set('ngrok-skip-browser-warning', 'true');

//          return headers;
//       },
//    }),
//    endpoints: (builder) => ({
//       authorizationCode: builder.mutation({
//          query: (payload: { code: string }) => ({
//             url: 'identity-services/api/v1/auth/keycloak/authorization-code',
//             method: 'POST',
//             body: payload,
//          }),
//          async onQueryStarted(arg, { dispatch, queryFulfilled }) {
//             try {
//                const { data } = await queryFulfilled;

//                console.log('data', data);

//                dispatch(
//                   setLogin({
//                      user_email: data.user_email,
//                      access_token: data.access_token,
//                      refresh_token: data.refresh_token,
//                   }),
//                );
//             } catch (error) {
//                console.log('[ERROR]::keycloakApi:', error);
//             }
//          },
//       }),
//    }),
// });

// export const { useAuthorizationCodeMutation } = keycloakApi;

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
            console.log('useAdminTokenOnly', useAdminTokenOnly);
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
            url: '/api/v1/auth/login',
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
               console.info(
                  '[AuthService]::login::try/catch',
                  JSON.stringify(error, null, 2),
               );
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

               if (currentUser?.userId !== impersonatedUser?.userId) {
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
               } else {
                  dispatch(
                     setImpersonatedUser({
                        impersonatedUser: null,
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
   }),
});

export const { useAuthorizationCodeMutation, useImpersonateUserMutation } =
   keycloakApi;
