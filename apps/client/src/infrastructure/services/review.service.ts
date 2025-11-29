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

const productModelReviewsFakeData = {
   total_records: 1,
   total_pages: 1,
   page_size: 10,
   current_page: 1,
   items: [
      {
         id: '690f765b5343112fafb4a908',
         model_id: '664351e90087aa09993f5ae7',
         sku_id: '690f4605e2295b9f94f23f87',
         order_info: {
            order_id: '93ded754-2b7d-4a1f-9487-2af0f9fa6e09',
            order_item_id: '958bde3a-758d-4e57-b66c-76a4b5379657',
         },
         customer_review_info: {
            name: 'lov3rinve146@gmail.com',
            avatar_image_url: null,
         },
         rating: 5,
         content: 'This is a review content.',
         created_at: '2025-11-08 16:56:59',
         updated_at: '2025-11-08 16:56:59',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_modelId=664351e90087aa09993f5ae7&_page=1&_limit=10&_sortOrder=desc',
      prev: null,
      next: null,
      last: '?_modelId=664351e90087aa09993f5ae7&_page=1&_limit=10&_sortOrder=desc',
   },
};

const reviewsInOrderFakeData = [
   {
      id: '690f765b5343112fafb4a908',
      model_id: '664351e90087aa09993f5ae7',
      sku_id: '690f4605e2295b9f94f23f87',
      order_info: {
         order_id: '93ded754-2b7d-4a1f-9487-2af0f9fa6e09',
         order_item_id: '958bde3a-758d-4e57-b66c-76a4b5379657',
      },
      rating: 5,
      content: 'This is a review content.',
      created_at: '2025-11-08 16:56:59',
      updated_at: '2025-11-08 16:56:59',
      updated_by: null,
      is_deleted: false,
      deleted_at: null,
      deleted_by: null,
   },
];

export type TReviewItem = {
   id: string;
   model_id: string;
   sku_id: string;
   order_info: {
      order_id: string;
      order_item_id: string;
   };
   customer_review_info: {
      name: string;
      avatar_image_url: string | null;
   };
   rating: number;
   content: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TReviewInOrderItem = {
   id: string;
   model_id: string;
   sku_id: string;
   order_info: {
      order_id: string;
      order_item_id: string;
   };
   rating: number;
   content: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

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
      getReviewByProductModelSlugAsync: builder.query<
         PaginationResponse<TReviewItem>,
         {
            slug: string;
            page?: number;
            limit?: number;
            sortOrder?: 'asc' | 'desc';
         }
      >({
         query: ({ slug, page = 1, limit = 10 }) => {
            const params = new URLSearchParams({
               _page: page.toString(),
               _limit: limit.toString(),
            });
            return {
               url: `/api/v1/reviews/model/${slug}?${params.toString()}`,
               method: 'GET',
            };
         },
         providesTags: ['Reviews'],
      }),
      getReviewByOrderIdAsync: builder.query<TReviewInOrderItem[], string>({
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
   useLazyGetReviewByProductModelSlugAsyncQuery,
   useCreateReviewAsyncMutation,
   useLazyGetReviewByOrderIdAsyncQuery,
   useUpdateReviewAsyncMutation,
   useDeleteReviewAsyncMutation,
} = reviewApi;
