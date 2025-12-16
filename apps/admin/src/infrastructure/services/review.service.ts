import {
   BaseQueryApi,
   FetchArgs,
   createApi,
} from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { TReviewItem } from '~/src/domain/types/catalog.type';
import { baseQuery } from './base-query';

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
   }),
});

export const { useLazyGetReviewByProductModelIdAsyncQuery } = reviewApi;
