import { createApi } from '@reduxjs/toolkit/query/react';
import { IIphoneModelResponse } from '~/domain/interfaces/catalogs/iPhone-model.interface';
import {
   IIphonePromotionResponse,
   IIphoneResponse,
} from '~/domain/interfaces/catalogs/iPhone.interface';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { IIphoneModel } from '~/domain/interfaces/catalogs/iphone/iPhone-model.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const catalogApi = createApi({
   reducerPath: 'catalog-api',
   tagTypes: ['Catalogs'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getIphonePromotions: builder.query<
         PaginationResponse<IIphonePromotionResponse>,
         void
      >({
         query: () => '/api/v1/products/iphone/promotions',
         providesTags: ['Catalogs'],
         transformResponse: (response) => {
            return response as PaginationResponse<IIphonePromotionResponse>;
         },
      }),
      getIphoneModels: builder.query<PaginationResponse<IIphoneModel>, string>({
         query: (params = '') => `/api/v1/products/iphone/models?${params}`,
         providesTags: ['Catalogs'],
      }),
      getModelBySlug: builder.query<any, string>({
         query: (slug) => `/api/v1/products/iphone/${slug}/products`,
         providesTags: ['Catalogs'],
      }),
      getIPhonesByModel: builder.query<IIphoneResponse[], string>({
         query: (slug) => `/api/v1/products/iphone/models/${slug}/products`,
         providesTags: ['Catalogs'],
      }),
   }),
});

export const { useLazyGetIphoneModelsQuery, useLazyGetModelBySlugQuery } =
   catalogApi;
