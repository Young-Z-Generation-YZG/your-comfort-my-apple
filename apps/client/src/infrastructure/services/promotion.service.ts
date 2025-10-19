import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/infrastructure/config/env.config';
import { RootState } from '../redux/store';

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'catalog-services',
      prepareHeaders: (headers, { getState }) => {
         const accessToken = (getState() as RootState).auth.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getActivePromotionEventAsync: builder.query<any, void>({
         query: () => '/api/v1/promotions/events/active',
         providesTags: ['Promotions'],
      }),
   }),
});

export const { useGetActivePromotionEventAsyncQuery } = promotionApi;
