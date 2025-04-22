import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   IAddressPayload,
   IAddressResponse,
} from '~/domain/interfaces/identity/address';
import { IMeResponse } from '~/domain/interfaces/identity/me';
import { IProfilePayload } from '~/domain/interfaces/identity/profile';

export const identityApi = createApi({
   reducerPath: 'identity-api',
   tagTypes: ['Identity'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://54ff-116-108-46-152.ngrok-free.app',
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
      updateProfileAsync: builder.mutation({
         query: (body: IProfilePayload) => ({
            url: `/api/v1/users/profiles`,
            method: 'PUT',
            body: body,
         }),
         invalidatesTags: ['Identity'],
      }),
      getAddressesAsync: builder.query<IAddressResponse[], void>({
         query: () => '/api/v1/users/addresses',
         providesTags: ['Identity'],
      }),
      addAddressAsync: builder.mutation({
         query: (body: IAddressPayload) => ({
            url: '/api/v1/users/addresses',
            method: 'POST',
            body: body,
         }),
         invalidatesTags: ['Identity'],
      }),
      updateAddressAsync: builder.mutation({
         query: (body: { id: string; payload: IAddressPayload }) => ({
            url: `/api/v1/users/addresses/${body.id}`,
            method: 'PUT',
            body: body.payload,
         }),
         invalidatesTags: ['Identity'],
      }),
      setDefaultAddressAsync: builder.mutation({
         query: (id: string) => ({
            url: `/api/v1/users/addresses/${id}/is-default`,
            method: 'PATCH',
         }),
         invalidatesTags: ['Identity'],
      }),
      deleteAddressAsync: builder.mutation({
         query: (id: string) => ({
            url: `/api/v1/users/addresses/${id}`,
            method: 'DELETE',
         }),
         invalidatesTags: ['Identity'],
      }),
   }),
});

export const {
   useGetMeAsyncQuery,
   useUpdateProfileAsyncMutation,
   useGetAddressesAsyncQuery,
   useAddAddressAsyncMutation,
   useUpdateAddressAsyncMutation,
   useSetDefaultAddressAsyncMutation,
   useDeleteAddressAsyncMutation,
} = identityApi;
