import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   IAddressPayload,
   IAddressResponse,
} from '~/domain/interfaces/identity/address';
import { IMeResponse } from '~/domain/interfaces/identity/me';

export const identityApi = createApi({
   reducerPath: 'identity-api',
   tagTypes: ['Identity'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://4235-116-108-46-152.ngrok-free.app/identity-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getMeAsync: builder.query<IMeResponse, void>({
         query: () => '/api/v1/users/me',
         providesTags: ['Identity'],
      }),
      getAddressesAsync: builder.query<IAddressResponse[], void>({
         query: () => '/api/v1/users/addresses',
         providesTags: ['Identity'],
      }),
      addAddressAsync: builder.mutation({
         query: (payload: IAddressPayload) => ({
            url: '/api/v1/users/addresses',
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Identity'],
      }),
   }),
});

export const {
   useGetMeAsyncQuery,
   useGetAddressesAsyncQuery,
   useAddAddressAsyncMutation,
} = identityApi;
