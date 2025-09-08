import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { setLogin } from '../redux/features/auth.slice';

export const keycloakApi = createApi({
   reducerPath: 'keycloak-api',
   tagTypes: ['keycloak'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://1f6e-116-108-118-49.ngrok-free.app',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      authorizationCode: builder.mutation({
         query: (payload: { code: string }) => ({
            url: 'identity-services/api/v1/auth/keycloak/authorization-code',
            method: 'POST',
            body: payload,
         }),
         async onQueryStarted(arg, { dispatch, queryFulfilled }) {
            try {
               const { data } = await queryFulfilled;

               console.log('data', data);

               dispatch(
                  setLogin({
                     user_email: data.user_email,
                     access_token: data.access_token,
                     refresh_token: data.refresh_token,
                  }),
               );
            } catch (error) {
               console.log('[ERROR]::keycloakApi:', error);
            }
         },
      }),
   }),
});

export const { useAuthorizationCodeMutation } = keycloakApi;
