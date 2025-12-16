import { createApi } from '@reduxjs/toolkit/query/react';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';
import { setLogout } from '~/infrastructure/redux/features/auth.slice';
import { baseQuery } from './base-query';
import {
   IStoreEventItemPayload,
   TCheckoutBasket,
   TCart,
   IStoreBasketItemPayload,
   IStoreBasketPayload,
   IGetBasketQueryParams,
} from '~/domain/types/basket.type';

const baseQueryHandler = async (
   args: string | FetchArgs,
   api: BaseQueryApi,
   extraOptions: unknown,
) => {
   const result = await baseQuery('/basket-services')(
      args,
      api,
      extraOptions as unknown as any,
   );

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export const basketApi = createApi({
   reducerPath: 'basket-api',
   tagTypes: ['Baskets'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      storeBasket: builder.mutation<boolean, IStoreBasketPayload>({
         query: (payload: IStoreBasketPayload) => ({
            url: `/api/v1/baskets`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      storeEventItem: builder.mutation<boolean, IStoreEventItemPayload>({
         query: (payload: IStoreEventItemPayload) => ({
            url: `/api/v1/baskets/event-item`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      storeBasketItem: builder.mutation<boolean, IStoreBasketItemPayload>({
         query: (payload: IStoreBasketItemPayload) => ({
            url: `/api/v1/baskets/product-model-item`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      syncBasket: builder.mutation<boolean, IStoreBasketPayload>({
         query: (payload: IStoreBasketPayload) => ({
            url: `/api/v1/baskets/sync`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      getBasket: builder.query<TCart, IGetBasketQueryParams>({
         query: (params: IGetBasketQueryParams) => ({
            url: '/api/v1/baskets',
            params: params,
         }),
         providesTags: ['Baskets'],
      }),
      getCheckoutItems: builder.query<TCheckoutBasket, IGetBasketQueryParams>({
         query: (params: IGetBasketQueryParams) => ({
            url: '/api/v1/baskets/checkout-items',
            params: params,
         }),
         providesTags: ['Baskets'],
      }),
      deleteBasket: builder.mutation<boolean, void>({
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
      checkoutBasket: builder.mutation<unknown, unknown>({
         query: (payload: unknown) => ({
            url: `/api/v1/baskets/checkout`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      checkoutBasketWithBlockchain: builder.mutation<unknown, unknown>({
         query: (payload: unknown) => ({
            url: `/api/v1/baskets/checkout/blockchain`,
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
   useStoreBasketItemMutation,
   useSyncBasketMutation,
   useGetBasketQuery,
   useLazyGetBasketQuery,
   useLazyGetCheckoutItemsQuery,
   useDeleteBasketMutation,
   useProceedToCheckoutMutation,
   useCheckoutBasketMutation,
   useCheckoutBasketWithBlockchainMutation,
} = basketApi;
