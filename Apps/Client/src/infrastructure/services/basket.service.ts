import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   ICheckoutResponse,
   IGetBasketResponse,
   IStoreBasketPayload,
} from '~/domain/interfaces/baskets/basket.interface';
import { ICheckoutPayload } from '~/domain/interfaces/baskets/checkout.interface';

export const basketApi = createApi({
   reducerPath: 'basket-api',
   tagTypes: ['Baskets'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://4235-116-108-46-152.ngrok-free.app/basket-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      storeBasketAsync: builder.mutation({
         query: (payload: IStoreBasketPayload) => ({
            url: `/api/v1/baskets`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      checkoutBasketAsync: builder.mutation<
         ICheckoutResponse,
         ICheckoutPayload
      >({
         query: (payload: ICheckoutPayload) => ({
            url: `/api/v1/baskets/checkout`,
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Baskets'],
      }),
      getBasketAsync: builder.query<IGetBasketResponse, void>({
         query: () => `/api/v1/baskets`,
         providesTags: ['Baskets'],
      }),
      deleteBasketAsync: builder.mutation({
         query: () => ({
            url: `/api/v1/baskets`,
            method: 'DELETE',
         }),
         invalidatesTags: ['Baskets'],
      }),
   }),
});

export const {
   useStoreBasketAsyncMutation,
   useGetBasketAsyncQuery,
   useDeleteBasketAsyncMutation,
   useCheckoutBasketAsyncMutation,
} = basketApi;
