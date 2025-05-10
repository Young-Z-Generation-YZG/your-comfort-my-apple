import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { CategoryResponseType } from '~/domain/types/category.type';
import envConfig from '~/infrastructure/config/env.config';
import { RootState } from '../redux/store';

export const CategoryApi = createApi({
   reducerPath: 'category-api',
   tagTypes: ['Categories'],
   baseQuery: fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + 'catalog-services',
      prepareHeaders: (headers, { getState }) => {
         const accessToken = (getState() as RootState).auth.value.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getCategoriesAsync: builder.query<CategoryResponseType[], void>({
         query: () => '/api/v1/categories',
         providesTags: ['Categories'],
      }),
   }),
});

export const { useGetCategoriesAsyncQuery } = CategoryApi;
