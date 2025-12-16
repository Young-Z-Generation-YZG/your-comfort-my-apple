import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '~/infrastructure/redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TIphoneModelDetails } from '~/domain/types/catalog.type';
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

export const catalogApi = createApi({
   reducerPath: 'iphone-api',
   tagTypes: ['Iphones'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getModelBySlug: builder.query<TIphoneModelDetails, string>({
         query: (slug: string) => ({
            url: `/api/v1/products/iphone/${slug}`,
            method: 'GET',
         }),
      }),
   }),
});

export const { useLazyGetModelBySlugQuery } = catalogApi;
