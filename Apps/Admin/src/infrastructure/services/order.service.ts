import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const orderApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://54ff-116-108-46-152.ngrok-free.app/ordering-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({}),
});

export const {} = orderApi;
