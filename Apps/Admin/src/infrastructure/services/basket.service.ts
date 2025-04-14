import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const basketApi = createApi({
   reducerPath: 'basket-api',
   tagTypes: ['Baskets'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://c8fb-116-108-46-152.ngrok-free.app/basket-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({}),
});

export const {} = basketApi;
