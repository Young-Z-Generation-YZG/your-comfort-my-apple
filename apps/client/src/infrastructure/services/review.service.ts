import { createApi } from '@reduxjs/toolkit/query/react';

import { setLogout } from '~/infrastructure/redux/features/auth.slice';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import {
   IReviewPayload,
   IUpdateReviewPayload,
} from '~/domain/interfaces/catalog.interface';
import { baseQuery } from './base-query';
import { TReviewInOrderItem, TReviewItem } from '~/domain/types/catalog.type';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';

const baseQueryHandler = async (
   args: string | FetchArgs,
   api: BaseQueryApi,
   extraOptions: unknown,
) => {
   const result = await baseQuery('/catalog-services')(
      args,
      api,
      extraOptions as unknown as any,
   );

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

export interface IGetReviewsQueryParams {
   _page?: number | null;
   _limit?: number | null;
   _sortOrder?: 'ASC' | 'DESC' | null;
}

export const reviewApi = createApi({
   reducerPath: 'review-api',
   tagTypes: ['Reviews'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getReviewByProductModelSlugAsync: builder.query<
         PaginationResponse<TReviewItem>,
         {
            slug: string;
            queryParams: IGetReviewsQueryParams;
         }
      >({
         query: ({ slug, queryParams }) => ({
            url: `/api/v1/reviews/model/${slug}`,
            method: 'GET',
            params: queryParams,
         }),
         providesTags: ['Reviews'],
      }),
      getReviewByProductModelIdAsync: builder.query<
         PaginationResponse<TReviewItem>,
         {
            id: string;
            queryParams: IGetReviewsQueryParams;
         }
      >({
         query: ({ id, queryParams }) => ({
            url: `/api/v1/reviews/product-models/${id}`,
            method: 'GET',
            params: queryParams,
         }),
         providesTags: ['Reviews'],
      }),
      getReviewByOrderIdAsync: builder.query<TReviewInOrderItem[], string>({
         query: (orderId) => ({
            url: `/api/v1/reviews/orders/${orderId}`,
            method: 'GET',
         }),
         providesTags: ['Reviews'],
      }),
      createReviewAsync: builder.mutation<boolean, IReviewPayload>({
         query: (payload: IReviewPayload) => ({
            url: '/api/v1/reviews',
            method: 'POST',
            body: payload,
         }),
      }),
      updateReviewAsync: builder.mutation<
         boolean,
         {
            reviewId: string;
            payload: IUpdateReviewPayload;
         }
      >({
         query: ({ reviewId, payload }) => ({
            url: `/api/v1/reviews/${reviewId}`,
            method: 'PUT',
            body: payload,
         }),
         invalidatesTags: ['Reviews'],
      }),
      deleteReviewAsync: builder.mutation<boolean, string>({
         query: (reviewId: string) => ({
            url: `/api/v1/reviews/${reviewId}`,
            method: 'DELETE',
         }),
         invalidatesTags: ['Reviews'],
      }),
   }),
});

export const {
   useLazyGetReviewByProductModelSlugAsyncQuery,
   useLazyGetReviewByProductModelIdAsyncQuery,
   useCreateReviewAsyncMutation,
   useLazyGetReviewByOrderIdAsyncQuery,
   useUpdateReviewAsyncMutation,
   useDeleteReviewAsyncMutation,
} = reviewApi;
