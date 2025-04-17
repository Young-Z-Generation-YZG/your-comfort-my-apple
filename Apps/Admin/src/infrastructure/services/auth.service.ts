import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   ILoginPayload,
   ILoginResponse,
} from '~/src/domain/interfaces/auth/login.interface';
import { setAccessToken } from '../redux/features/auth.slice';
import {
   CatchErrorResponse,
   HttpErrorResponse,
} from '~/src/domain/interfaces/errors/error.interface';

import { VERIFICATION_TYPES } from '~/src/domain/enums/verification-type.enum';

export const authApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://be2c-116-108-46-152.ngrok-free.app/identity-services',
      prepareHeaders: (headers) => {
         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   }),
   endpoints: (builder) => ({
      loginAsync: builder.mutation({
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
                     setAccessToken({
                        user_email: data.user_email,
                        access_token: data.access_token,
                        refresh_token: data.refresh_token,
                        access_token_expires_in: data.access_token_expires_in,
                     }),
                  );
               }
            } catch (error) {
               const serverError = error as CatchErrorResponse;
               console.error('Login failed:', serverError.error);
            }
         },
      }),
   }),
});

export const { useLoginAsyncMutation } = authApi;
