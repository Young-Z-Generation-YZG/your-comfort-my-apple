import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { IPromotionEventResponse } from '~/src/domain/interfaces/discounts/IPromotionEventResponse';
import { PromotionEventSchemaType } from '~/src/domain/schemas/discount.schema';
import { baseQueryHandler } from '~/src/infrastructure/services/base-query';

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: (args, api, extraOptions) => {
      return baseQueryHandler(args, api, extraOptions, 'discount-services');
   },
   endpoints: (builder) => ({
      getPromotionEvents: builder.query<
         PaginationResponse<IPromotionEventResponse>,
         string
      >({
         query: () => '/api/v1/promotions/events',
         providesTags: ['Promotions'],
      }),
      createPromotionEvent: builder.mutation({
         query: (data: PromotionEventSchemaType) => ({
            url: '/api/v1/promotions/events',
            method: 'POST',
            body: data,
         }),
         invalidatesTags: ['Promotions'],
      }),
   }),
});

export const { useGetPromotionEventsQuery, useCreatePromotionEventMutation } =
   promotionApi;
