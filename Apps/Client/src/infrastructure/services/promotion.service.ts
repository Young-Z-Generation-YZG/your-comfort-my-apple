import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://1726-116-108-46-152.ngrok-free.app/catalog-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getActivePromotionEventAsync: builder.query<any, void>({
         query: () => '/api/v1/promotions/events',
         providesTags: ['Promotions'],
      }),
   }),
});

export const { useGetActivePromotionEventAsyncQuery } = promotionApi;
