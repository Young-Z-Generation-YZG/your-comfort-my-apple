import {
   createApi,
   FetchBaseQueryError,
   FetchBaseQueryMeta,
   QueryReturnValue,
} from '@reduxjs/toolkit/query/react';
import { setLogin, setLogout } from '../redux/features/auth.slice';
import { RootState } from '../redux/store';
import { baseQuery } from './base-query';
import { setTenant } from '../redux/features/tenant.slice';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('identity-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const authApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: baseQueryHandler,
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
