import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   ILoginPayload,
   ILoginResponse,
} from '~/src/domain/interfaces/auth/login.interface';
import { setLogin } from '../redux/features/auth.slice';
import { HttpErrorResponse } from '~/src/domain/interfaces/errors/error.interface';

import { VERIFICATION_TYPES } from '~/src/domain/enums/verification-type.enum';
import { baseQueryHandler } from './base-query';

export const authApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: (args, api, extraOptions) => {
      return baseQueryHandler(args, api, extraOptions, 'identity-services');
   },
   endpoints: (builder) => ({
      login: builder.mutation({
         query: (payload: ILoginPayload) => ({
            url: '/api/v1/auth/login',
            method: 'POST',
            body: payload,
         }),
         transformResponse: (response: ILoginResponse) => {
            return response;
         },
         transformErrorResponse: (error: HttpErrorResponse) => {
            return error.data;
         },
         async onQueryStarted(arg, { dispatch, queryFulfilled }) {
            try {
               const { data } = await queryFulfilled;

               if (
                  data.verification_type !==
                  VERIFICATION_TYPES.EMAIL_VERIFICATION
               ) {
                  dispatch(
                     setLogin({
                        user_email: data.user_email,
                        access_token: data.access_token,
                        refresh_token: data.refresh_token,
                     }),
                  );
               }
            } catch (error) {
               console.warn('Error', error);
            }
         },
      }),
   }),
});

export const { useLoginMutation } = authApi;
