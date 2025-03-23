import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { CategoryResponseType } from '~/domain/types/category.type';

export const categoryApi = createApi({
   reducerPath: 'category-api',
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://44e5-116-108-33-248.ngrok-free.app/catalog-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   tagTypes: ['Categories'],
   endpoints: (builder) => ({
      getCategoriesAsync: builder.query<CategoryResponseType[], void>({
         query: () => '/api/v1/categories',
         providesTags: ['Categories'],
      }),
   }),
});

export const { useGetCategoriesAsyncQuery } = categoryApi;
