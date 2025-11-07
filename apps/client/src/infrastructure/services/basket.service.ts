import { createApi } from '@reduxjs/toolkit/query/react';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';

import { createQueryEncodedUrl } from '~/infrastructure/utils/query-encoded-url';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (
   args: string | FetchArgs,
   api: BaseQueryApi,
   extraOptions: unknown,
) => {
   const result = await baseQuery('basket-services')(
      args,
      api,
      extraOptions as unknown as any,
   );

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

const fakeGetBasketResponse = {
   user_email: 'lov3rinve146@gmail.com',
   cart_items: [
      {
         is_selected: true,
         model_id: '664351e90087aa09993f5ae7',
         product_name: 'iPhone 15 128GB Blue',
         color: {
            name: 'Blue',
            normalized_name: 'BLUE',
            hex_code: '',
            showcase_image_id: '',
            order: 0,
         },
         model: {
            name: 'iPhone 15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1000,
         promotion: {
            promotion_id: '550e8400-e29b-41d4-a716-446655440000',
            promotion_type: 'COUPON',
            product_unit_price: 1000,
            discount_type: 'PERCENTAGE',
            discount_value: 0.1,
            discount_amount: 100.0,
            final_price: 900.0,
         },
         quantity: 1,
         sub_total_amount: 1000,
         index: 1,
      },
   ],
   total_amount: 1000,
};

export type TCart = {
   user_email: string;
   cart_items: TCartItem[];
   total_amount: number;
};

export type TCartItem = {
   is_selected: boolean;
   model_id: string;
   product_name: string;
   color: {
      name: string;
      normalized_name: string;
      hex_code: string;
      showcase_image_id: string;
      order: number;
   };
   model: {
      name: string;
      normalized_name: string;
      order: number;
   };
   storage: {
      name: string;
      normalized_name: string;
      order: number;
   };
   display_image_url: string;
   unit_price: number;
   promotion: {
      promotion_id: string;
      promotion_type: string;
      product_unit_price: number;
      discount_type: string;
      discount_value: number;
      discount_amount: number;
      final_price: number;
   } | null;
   quantity: number;
   sub_total_amount: number;
   index: number;
};

const storeBasketPayloadExample = {
   cart_items: [
      {
         is_selected: false,
         model_id: '68e403d5617b27ad030bf28f',
         model: {
            name: 'iPhone 15',
            normalized_name: 'IPHONE_15',
         },
         color: {
            name: 'Blue',
            normalized_name: 'BLUE',
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
         },
         quantity: 1,
      },
   ],
};

export type TStoreBasketPayload = typeof storeBasketPayloadExample;
export type TStoreBasketItem =
   (typeof storeBasketPayloadExample.cart_items)[number];

const storeEventItemPayloadExample = {
   event_item_id: '04edf970-b569-44ac-a116-9847731929ab',
};

export type TStoreEventItemPayload = typeof storeEventItemPayloadExample;

export const basketApi = createApi({
   reducerPath: 'basket-api',
   tagTypes: ['Baskets'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      storeBasket: builder.mutation<boolean, TStoreBasketPayload>({
         query: (payload: TStoreBasketPayload) => ({
            url: `/api/v1/baskets`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      storeEventItem: builder.mutation<boolean, TStoreEventItemPayload>({
         query: (payload: TStoreEventItemPayload) => ({
            url: `/api/v1/baskets/event-item`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      getBasket: builder.query<TCart, unknown>({
         query: (queries: unknown) =>
            createQueryEncodedUrl('/api/v1/baskets', queries as object),
         providesTags: ['Baskets'],
      }),
      getCheckoutItems: builder.query<TCart, unknown>({
         query: (queries: unknown) =>
            createQueryEncodedUrl(
               '/api/v1/baskets/checkout-items',
               queries as object,
            ),
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
   useGetBasketQuery,
   useLazyGetBasketQuery,
   useLazyGetCheckoutItemsQuery,
   useDeleteBasketMutation,
   useProceedToCheckoutMutation,
   useCheckoutBasketMutation,
   useCheckoutBasketWithBlockchainMutation,
} = basketApi;
