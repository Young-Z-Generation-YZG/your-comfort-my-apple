import { createApi } from '@reduxjs/toolkit/query/react';
import { baseQuery } from './base-query';
import { setLogout } from '~/src/infrastructure/redux/features/auth.slice';
import { TUser } from '~/src/domain/types/identity';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/identity-services')(
      args,
      api,
      extraOptions,
   );

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export interface IUpdateProfileByIdPayload {
   first_name?: string | null;
   last_name?: string | null;
   phone_number?: string | null;
   birthday?: string | null;
   gender?: string | null;
}

export const userApi = createApi({
   reducerPath: 'user-api',
   tagTypes: ['Users'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getUserByUserId: builder.query<TUser, string>({
         query: (id: string) => ({
            url: `/api/v1/users/${id}`,
            method: 'GET',
         }),
         providesTags: ['Users'],
      }),
      updateProfileByUserId: builder.mutation({
         query: ({
            userId,
            body,
         }: {
            userId: string;
            body: IUpdateProfileByIdPayload;
         }) => ({
            url: `/api/v1/users/${userId}/profiles`,
            method: 'PUT',
            body: body,
         }),
         invalidatesTags: ['Users'],
      }),
      getAccountDetails: builder.query<TUser, void>({
         query: () => ({
            url: `/api/v1/users/account`,
            method: 'GET',
         }),
         providesTags: ['Users'],
      }),
   }),
});

export const {
   useLazyGetUserByUserIdQuery,
   useLazyGetAccountDetailsQuery,
   useUpdateProfileByUserIdMutation,
} = userApi;
