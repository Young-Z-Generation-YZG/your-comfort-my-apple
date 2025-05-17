import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const keycloakApi = createApi({
   reducerPath: 'keycloak-api',
   tagTypes: ['keycloak'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://213f-116-108-46-152.ngrok-free.app',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      authorizationCode: builder.mutation({
         query: (payload: { code: string }) => ({
            url: '/api/v1/auth/keycloak/authorization-code',
            method: 'POST',
            body: payload,
         }),
      }),
   }),
});

export const { useAuthorizationCodeMutation } = keycloakApi;
