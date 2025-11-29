import { createApi } from '@reduxjs/toolkit/query/react';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';

import { createQueryEncodedUrl } from '~/infrastructure/utils/query-encoded-url';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TColor, TModel, TStorage } from './catalog.service';

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

export type TCart = {
   user_email: string;
   cart_items: TCartItem[];
   sub_total_amount: number;
   promotion_id: string;
   promotion_type: string;
   discount_type: string;
   discount_value: number;
   discount_amount: number;
   max_discount_amount: number | null;
   total_amount: number;
};

export type TCartItem = {
   is_selected: boolean;
   model_id: string;
   sku_id: string;
   product_name: string;
   color: TColor;
   model: TModel;
   storage: TStorage;
   display_image_url: string;
   unit_price: number;
   quantity: number;
   sub_total_amount: number;
   promotion: TPromotion | null;
   discount_amount: number;
   total_amount: number;
   index: number;
};

export type TStoreBasketPayload = {
   cart_items: TStoreBasketItem[];
};
export type TStoreBasketItem = {
   is_selected: boolean;
   model_id: string;
   sku_id: string;
   model: {
      name: string;
      normalized_name: string;
   };
   color: {
      name: string;
      normalized_name: string;
   };
   storage: {
      name: string;
      normalized_name: string;
   };
   quantity: number;
};

export interface IStoreEventItemPayload {
   event_item_id: string;
}

export type TUSerInfo = {
   email: string;
   first_name: string;
   last_name: string;
   phone_number: string;
   birth_date: string;
   image_id: string;
   image_url: string;
   default_address_label: string;
   default_contact_name: string;
   default_contact_phone_number: string;
   default_address_line: string;
   default_address_district: string;
   default_address_province: string;
   default_address_country: string;
};

export type TPromotion = {
   promotion_id: string;
   promotion_type: string;
   discount_type: string;
   discount_value: number;
};

export type TCheckoutBasketItem = {
   is_selected: boolean;
   model_id: string;
   sku_id: string;
   product_name: string;
   color: TColor;
   model: TModel;
   storage: TStorage;
   display_image_url: string;
   unit_price: number;
   quantity: number;
   sub_total_amount: number;
   promotion: TPromotion | null;
   discount_amount: number;
   total_amount: number;
   index: number;
};

export type TCheckoutBasket = {
   user_email: string;
   cart_items: TCheckoutBasketItem[];
   sub_total_amount: number;
   promotion_id: string;
   promotion_type: string;
   discount_type: string;
   discount_value: number;
   discount_amount: number;
   max_discount_amount: number | null;
   total_amount: number;
};

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
      storeEventItem: builder.mutation<boolean, IStoreEventItemPayload>({
         query: (payload: IStoreEventItemPayload) => ({
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
      getCheckoutItems: builder.query<TCheckoutBasket, unknown>({
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
