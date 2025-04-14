import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://c8fb-116-108-46-152.ngrok-free.app/catalog-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({}),
});

export const {} = promotionApi;
