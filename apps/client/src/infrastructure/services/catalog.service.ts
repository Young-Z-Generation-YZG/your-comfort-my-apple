import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { IIphoneModelResponse } from '~/domain/interfaces/catalogs/iPhone-model.interface';
import {
   IIphonePromotionResponse,
   IIphoneResponse,
} from '~/domain/interfaces/catalogs/iPhone.interface';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import envConfig from '~/infrastructure/config/env.config';
import { RootState } from '../redux/store';
import { IIphoneModel } from '~/domain/interfaces/catalogs/iphone/iPhone-model.interface';

export const catalogApi = createApi({
   reducerPath: 'catalog-api',
   tagTypes: ['Catalogs'],
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
      getIphoneModelsAsync: builder.query<
         PaginationResponse<IIphoneModel>,
         string
      >({
         query: (params = '') => `/api/v1/products/iphone/models?${params}`,
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
   }),
});

export const { useLazyGetIphoneModelsAsyncQuery } = catalogApi;
