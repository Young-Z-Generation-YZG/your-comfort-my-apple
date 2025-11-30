import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TIphoneModelDetails } from '~/domain/types/catalog.type';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export const catalogApi = createApi({
   reducerPath: 'iphone-api',
   tagTypes: ['Iphones'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getModelBySlug: builder.query<TIphoneModelDetails, string>({
         query: (slug) => `/api/v1/products/iphone/${slug}`,
         providesTags: ['Iphones'],
      }),
   }),
});

export const { useLazyGetModelBySlugQuery } = catalogApi;
