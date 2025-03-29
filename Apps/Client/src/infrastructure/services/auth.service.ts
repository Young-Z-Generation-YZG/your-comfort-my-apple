import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {
   ILoginPayload,
   ILoginResponse,
} from '~/domain/interfaces/auth/login.interface';
import { setAccessToken } from '../redux/features/auth.slice';
import {
   CatchErrorResponse,
   HttpErrorResponse,
   ServerErrorResponse,
} from '~/domain/interfaces/errors/error.interface';

export const AuthApi = createApi({
   reducerPath: 'auth-api',
   tagTypes: ['auth'],
   baseQuery: fetchBaseQuery({
      baseUrl: 'https://cdf1-116-108-38-46.ngrok-free.app/identity-services',
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

               dispatch(
                  setAccessToken({
                     access_token: data.access_token,
                     refresh_token: data.refresh_token,
                     expiration: data.expiration,
                  }),
               );
            } catch (error) {
               const serverError = error as CatchErrorResponse;
               console.error('Login failed:', serverError.error);
            }
         },
      }),
   }),
});

export const { useLoginAsyncMutation } = AuthApi;
