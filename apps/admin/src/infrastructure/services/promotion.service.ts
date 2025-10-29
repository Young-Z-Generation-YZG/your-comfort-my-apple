import { createApi } from '@reduxjs/toolkit/query/react';
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

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getEventWithItems: builder.query<any, void>({
         query: () => '/api/v1/promotions/event/event-with-items',
         providesTags: ['Promotions'],
      }),
   }),
});

export const { useLazyGetEventWithItemsQuery } = promotionApi;
