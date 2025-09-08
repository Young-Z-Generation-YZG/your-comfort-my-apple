import { createApi } from '@reduxjs/toolkit/query/react';
import { baseQueryHandler } from './base-query';

export const CategoryApi = createApi({
   reducerPath: 'category-api',
   tagTypes: ['Categories'],
   baseQuery: (args, api, extraOptions) => {
      return baseQueryHandler(args, api, extraOptions, 'catalog-services');
   },
   endpoints: (builder) => ({}),
});

export const {} = CategoryApi;
