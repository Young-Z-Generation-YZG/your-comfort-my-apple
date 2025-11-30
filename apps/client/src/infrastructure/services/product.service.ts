import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import {
   TNewestProduct,
   TPopularProduct,
   TProductModel,
   TSuggestionProduct,
} from '~/domain/types/catalog.type';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export interface IGetProductModelsQueryParams {
   _page?: number | null;
   _limit?: number | null;
   _textSearch?: string | null;
}

export const productApi = createApi({
   reducerPath: 'product-api',
   tagTypes: ['Products'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getProductModels: builder.query<
         PaginationResponse<TProductModel>,
         IGetProductModelsQueryParams
      >({
         query: (query: IGetProductModelsQueryParams) => {
            return {
               url: '/api/v1/product-models',
               params: query,
            };
         },
         providesTags: ['Products'],
      }),
      getPopularProducts: builder.query<
         PaginationResponse<TPopularProduct>,
         void
      >({
         query: () => `/api/v1/product-models/popular`,
         providesTags: ['Products'],
      }),
      getNewestProducts: builder.query<
         PaginationResponse<TNewestProduct>,
         void
      >({
         query: () => `/api/v1/product-models/newest`,
         providesTags: ['Products'],
      }),
      getSuggestionProducts: builder.query<
         PaginationResponse<TSuggestionProduct>,
         void
      >({
         query: () => `/api/v1/product-models/suggestion`,
         providesTags: ['Products'],
      }),
   }),
});

export const {
   useLazyGetProductModelsQuery,
   useLazyGetPopularProductsQuery,
   useLazyGetNewestProductsQuery,
   useLazyGetSuggestionProductsQuery,
} = productApi;
