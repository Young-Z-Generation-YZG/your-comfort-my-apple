import { createApi } from '@reduxjs/toolkit/query/react';
import { IAddressPayload } from '~/domain/interfaces/identity.interface';
import { IUpdateProfilePayload } from '~/domain/interfaces/identity.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import { TAccount, TAddress } from '~/domain/types/identity.type';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';

const baseQueryHandler = async (
   args: string | FetchArgs,
   api: BaseQueryApi,
   extraOptions: unknown,
) => {
   const result = await baseQuery('/identity-services')(
      args,
      api,
      extraOptions as unknown as any,
   );

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export const identityApi = createApi({
   reducerPath: 'identity-api',
   tagTypes: ['Profile', 'Addresses'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getMe: builder.query<TAccount, void>({
         query: () => '/api/v1/users/me',
         providesTags: ['Profile'],
      }),
      updateProfile: builder.mutation({
         query: (payload: IUpdateProfilePayload) => ({
            url: `/api/v1/users/profiles`,
            method: 'PUT',
            body: payload,
         }),
         invalidatesTags: ['Profile'],
      }),
      getAddresses: builder.query<TAddress[], void>({
         query: () => '/api/v1/users/addresses',
         providesTags: ['Addresses'],
      }),
      addAddress: builder.mutation({
         query: (payload: IAddressPayload) => ({
            url: '/api/v1/users/addresses',
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Addresses'],
      }),
      updateAddress: builder.mutation({
         query: ({
            addressId,
            payload,
         }: {
            addressId: string;
            payload: IAddressPayload;
         }) => ({
            url: `/api/v1/users/addresses/${addressId}`,
            method: 'PUT',
            body: payload,
         }),
         invalidatesTags: ['Addresses'],
      }),
      setDefaultAddress: builder.mutation({
         query: (addressId: string) => ({
            url: `/api/v1/users/addresses/${addressId}/is-default`,
            method: 'PATCH',
         }),
         invalidatesTags: ['Addresses'],
      }),
      deleteAddress: builder.mutation({
         query: (addressId: string) => ({
            url: `/api/v1/users/addresses/${addressId}`,
            method: 'DELETE',
         }),
         invalidatesTags: ['Addresses'],
      }),
   }),
});

export const {
   useLazyGetMeQuery,
   useLazyGetAddressesQuery,
   useUpdateProfileMutation,
   useAddAddressMutation,
   useUpdateAddressMutation,
   useSetDefaultAddressMutation,
   useDeleteAddressMutation,
} = identityApi;
