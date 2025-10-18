import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   IAddressPayload,
   IAddressResponse,
} from '~/domain/interfaces/identity/address';
import { IMeResponse } from '~/domain/interfaces/identity/me';
import { IProfilePayload } from '~/domain/interfaces/identity/profile';
import envConfig from '~/infrastructure/config/env.config';
import { RootState } from '../redux/store';
import { setLogout } from '../redux/features/auth.slice';

const baseQueryWithAuth = fetchBaseQuery({
   baseUrl: envConfig.API_ENDPOINT + 'identity-services',
   prepareHeaders: (headers, { getState }) => {
      const accessToken = (getState() as RootState).auth.accessToken;

      if (accessToken) {
         headers.set('Authorization', `Bearer ${accessToken}`);
      }

      headers.set('ngrok-skip-browser-warning', 'true');

      return headers;
   },
});

const baseQueryWithUnauthorizedHandler = async (
   args: any,
   api: any,
   extraOptions: any,
) => {
   const result = await baseQueryWithAuth(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const identityApi = createApi({
   reducerPath: 'identity-api',
   tagTypes: ['Profile', 'Addresses'],
   baseQuery: baseQueryWithUnauthorizedHandler,
   endpoints: (builder) => ({
      getMe: builder.query<IMeResponse, void>({
         query: () => '/api/v1/users/me',
         providesTags: ['Profile'],
      }),
      updateProfile: builder.mutation({
         query: (body: IProfilePayload) => ({
            url: `/api/v1/users/profiles`,
            method: 'PUT',
            body: body,
         }),
         invalidatesTags: ['Profile'],
      }),
      getAddresses: builder.query<IAddressResponse[], void>({
         query: () => '/api/v1/users/addresses',
         providesTags: ['Addresses'],
      }),
      addAddress: builder.mutation({
         query: (body: IAddressPayload) => ({
            url: '/api/v1/users/addresses',
            method: 'POST',
            body: body,
         }),
         invalidatesTags: ['Addresses'],
      }),
      updateAddress: builder.mutation({
         query: (body: { id: string; payload: IAddressPayload }) => ({
            url: `/api/v1/users/addresses/${body.id}`,
            method: 'PUT',
            body: body.payload,
         }),
         invalidatesTags: ['Addresses'],
      }),
      setDefaultAddress: builder.mutation({
         query: (id: string) => ({
            url: `/api/v1/users/addresses/${id}/is-default`,
            method: 'PATCH',
         }),
         invalidatesTags: ['Addresses'],
      }),
      deleteAddress: builder.mutation({
         query: (id: string) => ({
            url: `/api/v1/users/addresses/${id}`,
            method: 'DELETE',
         }),
         invalidatesTags: ['Addresses'],
      }),
   }),
});

export const {
   useGetMeQuery,
   useUpdateProfileMutation,
   useGetAddressesQuery,
   useAddAddressMutation,
   useUpdateAddressMutation,
   useSetDefaultAddressMutation,
   useDeleteAddressMutation,
} = identityApi;
