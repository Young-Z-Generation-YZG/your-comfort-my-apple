import { createApi } from '@reduxjs/toolkit/query/react';
import { baseQuery } from './base-query';
import { setLogout } from '../redux/features/auth.slice';
import { TEvent } from '~/domain/types/catalog.type';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
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
