import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
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
      getIphoneModels: builder.query<PaginationResponse<any>, string>({
         query: (params = '') => `/api/v1/products/iphone/models?${params}`,
         providesTags: ['Catalogs'],
      }),
      getIphoneModelBySlug: builder.query<any, string>({
         query: (slug) => `/api/v1/products/iphone/${slug}`,
         providesTags: ['Catalogs'],
      }),
   }),
});

export const { useLazyGetIphoneModelsQuery, useLazyGetIphoneModelBySlugQuery } =
   catalogApi;
