import { createApi } from '@reduxjs/toolkit/query/react';

import { setLogout } from '../redux/features/auth.slice';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import {
   IReviewByOrderResponse,
   IReviewPayload,
   IReviewResponse,
   IUpdateReviewPayload,
} from '~/domain/interfaces/catalogs/review.interface';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const reviewApi = createApi({
   reducerPath: 'review-api',
   tagTypes: ['Reviews'],
   baseQuery: baseQueryHandler,
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
