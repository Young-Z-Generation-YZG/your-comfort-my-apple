import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const catalogApi = createApi({
   reducerPath: 'catalog-api',
   tagTypes: ['Products'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://c8fb-116-108-46-152.ngrok-free.app/catalog-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({}),
});

export const {} = catalogApi;
