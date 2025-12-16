import { createApi } from '@reduxjs/toolkit/query/react';
import { baseQuery } from './base-query';
import { setLogout } from '../redux/features/auth.slice';
import { TEvent } from '~/domain/types/catalog.type';
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

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getEventDetails: builder.query<TEvent, string>({
         query: (eventId) => `/api/v1/promotions/events/${eventId}`,
         providesTags: ['Promotions'],
      }),
   }),
});

export const { useLazyGetEventDetailsQuery } = promotionApi;
