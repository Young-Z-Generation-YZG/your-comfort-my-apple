import {
   createApi,
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

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/identity-services')(
      args,
      api,
      extraOptions,
   );

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export type TLoginResponse = {
   user_email: string;
   username: string;
   access_token: string | null;
   refresh_token: string | null;
   access_token_expires_in: number | null;
   refresh_token_expires_in: number | null;
   verification_type: string;
   params: object | null;
};

export type TGetIdentityResponse = {
   id: string;
   tenant_id: string | null;
   branch_id: string | null;
   tenant_sub_domain: string | null;
   username: string;
   email: string;
   email_confirmed: boolean;
   phone_number: string | null;
   roles: string[];
};

export interface ILoginPayload {
   email: string;
   password: string;
}

export const authApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getIdentity: builder.query<TGetIdentityResponse, void>({
         query: () => ({
            url: '/api/v1/auth/me',
            method: 'GET',
         }),
         providesTags: ['auth'],
      }),
      login: builder.mutation<TLoginResponse, ILoginPayload>({
         query: (payload: ILoginPayload) => ({
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
                        userId: 'ADMIN_SUPER',
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
