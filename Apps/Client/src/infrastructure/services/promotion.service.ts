import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/infrastructure/config/env.config';

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + '/catalog-services',
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
