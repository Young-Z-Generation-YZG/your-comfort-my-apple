import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '~/infrastructure/redux/features/auth.slice';
import { baseQuery } from './base-query';
import {
   TNewestProduct,
   TPopularProduct,
   TProductModel,
   TSuggestionProduct,
} from '~/domain/types/catalog.type';
import {
   IGetProductModelsQueryParams,
   IGetProductModelsByCategorySlugQueryParams,
} from '~/domain/interfaces/catalog.interface';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';

const baseQueryHandler = async (
   args: string | FetchArgs,
   api: BaseQueryApi,
   extraOptions: unknown,
) => {
   const result = await baseQuery('/catalog-services')(
      args,
      api,
      extraOptions as unknown as any,
   );

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export const productApi = createApi({
   reducerPath: 'product-api',
   tagTypes: ['Products'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getProductModels: builder.query<
         PaginationResponse<TProductModel>,
         IGetProductModelsQueryParams
      >({
         query: (queryParams: IGetProductModelsQueryParams) => ({
            url: '/api/v1/product-models',
            method: 'GET',
            params: queryParams,
         }),
         providesTags: ['Products'],
      }),
      getProductModelsByCategorySlug: builder.query<
         PaginationResponse<TProductModel>,
         {
            categorySlug: string;
            queryParams: IGetProductModelsByCategorySlugQueryParams;
         }
      >({
         query: ({ categorySlug, queryParams }) => ({
            url: `/api/v1/product-models/category/${categorySlug}`,
            method: 'GET',
            params: queryParams,
         }),
         providesTags: ['Products'],
      }),
      getPopularProducts: builder.query<
         PaginationResponse<TPopularProduct>,
         void
      >({
         query: () => ({
            url: `/api/v1/product-models/popular`,
            method: 'GET',
            params: {
               _limit: 999,
            },
         }),
         providesTags: ['Products'],
      }),
      getNewestProducts: builder.query<
         PaginationResponse<TNewestProduct>,
         void
      >({
         query: () => ({
            url: `/api/v1/product-models/newest`,
            method: 'GET',
            params: {
               _limit: 999,
            },
         }),
         providesTags: ['Products'],
      }),
      getSuggestionProducts: builder.query<
         PaginationResponse<TSuggestionProduct>,
         void
      >({
         query: () => ({
            url: `/api/v1/product-models/suggestion`,
            method: 'GET',
            params: {
               _limit: 999,
            },
         }),
         providesTags: ['Products'],
      }),
   }),
});

export const {
   useLazyGetProductModelsQuery,
   useLazyGetPopularProductsQuery,
   useLazyGetNewestProductsQuery,
   useLazyGetSuggestionProductsQuery,
   useLazyGetProductModelsByCategorySlugQuery,
} = productApi;
