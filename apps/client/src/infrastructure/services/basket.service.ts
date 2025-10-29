import { createApi } from '@reduxjs/toolkit/query/react';

import { createQueryEncodedUrl } from '~/infrastructure/utils/query-encoded-url';
import { IStoreBasketPayload } from '~/domain/interfaces/baskets/basket.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('basket-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());

      // Redirect to sign-in page
      if (typeof window !== 'undefined') {
         window.location.href = '/sign-in';
      }
   }

   return result;
};

export const basketApi = createApi({
   reducerPath: 'basket-api',
   tagTypes: ['Baskets'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      storeBasket: builder.mutation({
         query: (payload: IStoreBasketPayload) => ({
            url: `/api/v1/baskets`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      storeEventItem: builder.mutation<any, any>({
         query: (payload: any) => ({
            url: `/api/v1/baskets/event-item`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      getBasket: builder.query<any, any>({
         query: (queries: any) =>
            createQueryEncodedUrl('/api/v1/baskets', queries),
         providesTags: ['Baskets'],
      }),
      getCheckoutItems: builder.query<any, any>({
         query: (queries: any) =>
            createQueryEncodedUrl('/api/v1/baskets/checkout-items', queries),
         providesTags: ['Baskets'],
      }),
      deleteBasket: builder.mutation({
         query: () => ({
            url: `/api/v1/baskets`,
            method: 'DELETE',
         }),
         invalidatesTags: ['Baskets'],
      }),
      proceedToCheckout: builder.mutation<boolean, void>({
         query: () => ({
            url: `/api/v1/baskets/proceed-checkout`,
            method: 'POST',
            body: {},
         }),
         invalidatesTags: ['Baskets'],
      }),
      checkoutBasket: builder.mutation<any, any>({
         query: (payload: any) => ({
            url: `/api/v1/baskets/checkout`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
   }),
});

export const {
   useStoreBasketMutation,
   useStoreEventItemMutation,
   useGetBasketQuery,
   useLazyGetBasketQuery,
   useLazyGetCheckoutItemsQuery,
   useDeleteBasketMutation,
   useProceedToCheckoutMutation,
   useCheckoutBasketMutation,
} = basketApi;
