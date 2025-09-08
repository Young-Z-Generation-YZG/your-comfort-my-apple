import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { baseQueryHandler } from './base-query';

export const catalogApi = createApi({
   reducerPath: 'catalog-api',
   tagTypes: ['Products'],
   baseQuery: (args, api, extraOptions) => {
      return baseQueryHandler(args, api, extraOptions, 'identity-services');
   },
   endpoints: (builder) => ({}),
});

export const {} = catalogApi;
