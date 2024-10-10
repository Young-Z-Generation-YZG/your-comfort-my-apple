'use client';

import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { IUserResponse } from './user.type';
import * as utils from '~/utils';

const baseUrl = 'https://jsonplaceholder.typicode.com/';

export const userApi = createApi({
   reducerPath: 'getUsers',
   baseQuery: fetchBaseQuery({
      baseUrl: baseUrl,
      prepareHeaders: (headers) => {
         const accessToken = utils.serializeUrl('access-token');

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         return headers;
      },
   }),
   endpoints: (builder) => ({
      getUsers: builder.query<IUserResponse, string>({
         query: () => 'users',
      }),
   }),
});

export const { useGetUsersQuery } = userApi;
