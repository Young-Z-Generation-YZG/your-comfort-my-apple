import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
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

export const productApi = createApi({
   reducerPath: 'product-api',
   tagTypes: ['Products'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getProducts: builder.query<PaginationResponse<any>, Record<string, any>>({
         query: (query: Record<string, any>) => {
            return {
               url: '/api/v1/products',
               params: query,
            };
         },
         providesTags: ['Products'],
      }),
      getPopularProducts: builder.query<PaginationResponse<any>, string>({
         query: () => `/api/v1/products/popular`,
         providesTags: ['Products'],
      }),
      getNewestProducts: builder.query<PaginationResponse<any>, void>({
         query: () => `/api/v1/products/newest`,
         providesTags: ['Products'],
      }),
      getSuggestionProducts: builder.query<PaginationResponse<any>, void>({
         query: () => `/api/v1/products/suggestion`,
         providesTags: ['Products'],
      }),
   }),
});

export const {
   useLazyGetProductsQuery,
   useLazyGetPopularProductsQuery,
   useLazyGetNewestProductsQuery,
   useLazyGetSuggestionProductsQuery,
} = productApi;
