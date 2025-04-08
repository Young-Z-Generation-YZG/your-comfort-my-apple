import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { IStoreBasketPayload } from '~/domain/interfaces/baskets/basket.interface';

export const basketApi = createApi({
   reducerPath: 'basket-api',
   tagTypes: ['Baskets'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://1726-116-108-46-152.ngrok-free.app/basket-services',
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
      getBasketAsync: builder.query({
         query: () => `/api/v1/baskets`,
         providesTags: ['Baskets'],
         transformResponse: (response) => {
            return response;
         },
      }),
   }),
});

export const { useStoreBasketAsyncMutation, useGetBasketAsyncQuery } =
   basketApi;
