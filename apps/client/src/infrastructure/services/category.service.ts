import { createApi } from '@reduxjs/toolkit/query/react';
import { CategoryResponseType } from '~/domain/types/category.type';
import { baseQuery } from './base-query';
import { setLogout } from '../redux/features/auth.slice';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const categoryApi = createApi({
   reducerPath: 'category-api',
   tagTypes: ['Categories'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getCategoriesAsync: builder.query<CategoryResponseType[], void>({
         query: () => '/api/v1/categories',
         providesTags: ['Categories'],
      }),
   }),
});

export const { useGetCategoriesAsyncQuery } = categoryApi;
