import {
   createApi,
   fetchBaseQuery,
   FetchBaseQueryError,
   FetchBaseQueryMeta,
   QueryReturnValue,
} from '@reduxjs/toolkit/query/react';
import { setLogin, setLogout } from '../redux/features/auth.slice';
import envConfig from '../config/env.config';
import { RootState } from '../redux/store';

export const authApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'identity-services',
      prepareHeaders: (headers, { getState }) => {
         const accessToken = (getState() as RootState).auth.currentUser
            ?.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
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

               console.log('data', data);

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
         onQueryStarted(arg, { dispatch }) {
            try {
               dispatch(setLogout());
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

export const { useLoginMutation, useLogoutMutation } = authApi;
