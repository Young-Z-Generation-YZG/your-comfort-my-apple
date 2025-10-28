import { createApi } from '@reduxjs/toolkit/query/react';

import { ICheckoutPayload } from '~/domain/interfaces/baskets/checkout.interface';
import { createQueryEncodedUrl } from '~/infrastructure/utils/query-encoded-url';
import {
   IBasket,
   ICheckoutResponse,
   IGetBasketQueries,
   IStoreBasketPayload,
} from '~/domain/interfaces/baskets/basket.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('basket-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
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
      getBasket: builder.query<IBasket, IGetBasketQueries>({
         query: (queries: IGetBasketQueries) =>
            createQueryEncodedUrl('/api/v1/baskets', queries),
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
      checkoutBasket: builder.mutation<ICheckoutResponse, ICheckoutPayload>({
         query: (payload: ICheckoutPayload) => ({
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
   useGetBasketQuery,
   useLazyGetBasketQuery,
   useDeleteBasketMutation,
   useProceedToCheckoutMutation,
   useCheckoutBasketMutation,
} = basketApi;
