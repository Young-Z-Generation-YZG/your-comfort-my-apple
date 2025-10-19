import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

import envConfig from '~/infrastructure/config/env.config';
import { RootState } from '../redux/store';
import { setLogout } from '../redux/features/auth.slice';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import {
   IReviewByOrderResponse,
   IReviewPayload,
   IReviewResponse,
   IUpdateReviewPayload,
} from '~/domain/interfaces/catalogs/review.interface';

const baseQueryWithAuth = fetchBaseQuery({
   baseUrl: envConfig.API_ENDPOINT + 'catalog-services',
   prepareHeaders: (headers, { getState }) => {
      const accessToken = (getState() as RootState).auth.accessToken;

      if (accessToken) {
         headers.set('Authorization', `Bearer ${accessToken}`);
      }

      headers.set('ngrok-skip-browser-warning', 'true');

      return headers;
   },
   responseHandler: (response) => {
      return response.json();
   },
});

const baseQueryWithUnauthorizedHandler = async (
   args: any,
   api: any,
   extraOptions: any,
) => {
   const result = await baseQueryWithAuth(args, api, extraOptions);

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export const reviewApi = createApi({
   reducerPath: 'review-api',
   tagTypes: ['Reviews'],
   baseQuery: baseQueryWithUnauthorizedHandler,
   endpoints: (builder) => ({
      getReviewByModelIdAsync: builder.query<
         PaginationResponse<IReviewResponse>,
         string
      >({
         query: (modelId) => `/api/v1/reviews/${modelId}`,
         providesTags: ['Reviews'],
      }),
      getReviewByOrderIdAsync: builder.query<IReviewByOrderResponse[], string>({
         query: (orderId) => `/api/v1/reviews/orders/${orderId}`,
         providesTags: ['Reviews'],
      }),
      createReviewAsync: builder.mutation({
         query: (body: IReviewPayload) => ({
            url: '/api/v1/reviews',
            method: 'POST',
            body,
         }),
      }),
      updateReviewAsync: builder.mutation({
         query: ({
            body,
            reviewId,
         }: {
            body: IUpdateReviewPayload;
            reviewId: string;
         }) => ({
            url: `/api/v1/reviews/${reviewId}`,
            method: 'PUT',
            body,
         }),
         invalidatesTags: ['Reviews'],
      }),
      deleteReviewAsync: builder.mutation({
         query: (reviewId: string) => ({
            url: `/api/v1/reviews/${reviewId}`,
            method: 'DELETE',
         }),
         invalidatesTags: ['Reviews'],
      }),
   }),
});

export const {
   useGetReviewByModelIdAsyncQuery,
   useCreateReviewAsyncMutation,
   useGetReviewByOrderIdAsyncQuery,
   useUpdateReviewAsyncMutation,
   useDeleteReviewAsyncMutation,
} = reviewApi;
