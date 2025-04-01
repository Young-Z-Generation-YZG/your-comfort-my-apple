import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { IIphonePromotionResponse } from '~/domain/interfaces/catalog/iPhone.interface';
import { PaginationWithPromotionResponse } from '~/domain/interfaces/common/pagination-response.interface';

export const catalogApi = createApi({
   reducerPath: 'catalog-api',
   tagTypes: ['Products'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://ea0c-116-108-38-46.ngrok-free.app/catalog-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getIphonePromotionsAsync: builder.query<
         PaginationWithPromotionResponse<IIphonePromotionResponse>,
         void
      >({
         query: () => '/api/v1/products/iphone/promotions',
         providesTags: ['Products'],
         transformResponse: (response) => {
            return response as PaginationWithPromotionResponse<IIphonePromotionResponse>;
         },
      }),
   }),
});

export const { useGetIphonePromotionsAsyncQuery } = catalogApi;
