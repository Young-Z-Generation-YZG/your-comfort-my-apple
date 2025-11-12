import {
   createApi,
   fetchBaseQuery,
   FetchBaseQueryError,
   FetchBaseQueryMeta,
   QueryReturnValue,
} from '@reduxjs/toolkit/query/react';
import {
   setLogin,
   setLogout,
   setUseRefreshToken,
} from '../redux/features/auth.slice';
import { RootState } from '../redux/store';
import { baseQuery } from './base-query';
import { setTenant } from '../redux/features/tenant.slice';
import envConfig from '../config/env.config';

// const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
//    const result = await baseQuery('identity-services')(args, api, extraOptions);

//    // Check if we received a 401 Unauthorized response
//    if (result.error && result.error.status === 401) {
//       // Dispatch logout action to clear auth state
//       api.dispatch(setLogout());
//    }

//    return result;
// };

export const authApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'identity-services',
      prepareHeaders: (headers, { getState }) => {
         const authAppState = (getState() as RootState).auth;

         const token = authAppState.useRefreshToken
            ? authAppState.currentUser?.refreshToken
            : authAppState.currentUser?.accessToken;

         console.log('token', token);

         if (token) {
            headers.set('Authorization', `Bearer ${token}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getIdentity: builder.query<any, any>({
         query: () => ({
            url: '/api/v1/auth/me',
            method: 'GET',
         }),
         providesTags: ['auth'],
      }),
      login: builder.mutation({
         query: (payload: any) => ({
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
                        userId: 'be0cd669-237a-484d-b3cf-793e0ad1b0ea',
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
      logout: builder.mutation<void, void>({
         queryFn: async (_, { getState }, __, baseQuery) => {
            const refreshToken = (getState() as RootState).auth.currentUser
               ?.refreshToken;

            console.log('refreshToken', refreshToken);

            await baseQuery({
               url: '/api/v1/auth/logout',
               method: 'POST',
               headers: {
                  Authorization: `Bearer ${refreshToken}`,
               },
            });

            return {
               data: true,
               error: null,
            } as unknown as QueryReturnValue<
               void,
               FetchBaseQueryError,
               FetchBaseQueryMeta
            >;
         },
         async onQueryStarted(arg, { dispatch, queryFulfilled }) {
            try {
               await queryFulfilled;
               dispatch(setUseRefreshToken(false));
               dispatch(setLogout());
               dispatch(
                  setTenant({
                     tenantId: null,
                  }),
               );
            } catch (error: unknown) {
               console.info(
                  '[AuthService]::logout::try/catch',
                  JSON.stringify(error, null, 2),
               );
            }
         },
      }),
   }),
});

export const { useLoginMutation, useLogoutMutation, useLazyGetIdentityQuery } =
   authApi;
