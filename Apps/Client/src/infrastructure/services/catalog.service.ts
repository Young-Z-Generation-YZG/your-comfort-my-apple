import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { IIphoneModelResponse } from '~/domain/interfaces/catalogs/iPhone-model.inteface';
import {
   IIphonePromotionResponse,
   IIphoneResponse,
} from '~/domain/interfaces/catalogs/iPhone.interface';
import {
   IReviewPayload,
   IReviewResponse,
} from '~/domain/interfaces/catalogs/review.interface';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';

export const catalogApi = createApi({
   reducerPath: 'catalog-api',
   tagTypes: ['Catalogs'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://be2c-116-108-46-152.ngrok-free.app/catalog-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getIphonePromotionsAsync: builder.query<
         PaginationResponse<IIphonePromotionResponse>,
         void
      >({
         query: () => '/api/v1/products/iphone/promotions',
         providesTags: ['Catalogs'],
         transformResponse: (response) => {
            return response as PaginationResponse<IIphonePromotionResponse>;
         },
      }),
      getModelsAsync: builder.query<
         PaginationResponse<IIphoneModelResponse>,
         void
      >({
         query: () => '/api/v1/products/iphone/models',
         providesTags: ['Catalogs'],
      }),
      getModelBySlugAsync: builder.query<IIphoneModelResponse, string>({
         query: (slug) => `/api/v1/products/iphone/${slug}/models`,
         providesTags: ['Catalogs'],
      }),
      getIPhonesByModelAsync: builder.query<IIphoneResponse[], string>({
         query: (slug) => `/api/v1/products/iphone/models/${slug}/products`,
         providesTags: ['Catalogs'],
      }),
      getReviewByModelIdAsync: builder.query<
         PaginationResponse<IReviewResponse>,
         string
      >({
         query: (modelId) => `/api/v1/reviews/${modelId}`,
         providesTags: ['Catalogs'],
      }),
      createReviewAsync: builder.mutation({
         query: (body: IReviewPayload) => ({
            url: '/api/v1/reviews',
            method: 'POST',
            body,
         }),
      }),
   }),
});

export const {
   useGetIphonePromotionsAsyncQuery,
   useGetModelBySlugAsyncQuery,
   useGetIPhonesByModelAsyncQuery,
   useGetModelsAsyncQuery,
   useGetReviewByModelIdAsyncQuery,
   useCreateReviewAsyncMutation,
} = catalogApi;
