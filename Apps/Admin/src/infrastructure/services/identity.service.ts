import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const identityApi = createApi({
   reducerPath: 'identity-api',
   tagTypes: ['Identity'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://54ff-116-108-46-152.ngrok-free.app',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({}),
});

export const {} = identityApi;
