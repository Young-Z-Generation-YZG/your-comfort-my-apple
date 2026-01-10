import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { TSkuRequest } from '~/src/domain/types/catalog.type';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export interface IGetSkuRequestsParams {
   _page?: number | null;
   _limit?: number | null;
   _requestState?: string | null;
   _transferType?: string | null;
   _branchId?: string | null;
}

export interface ICreateSkuRequestRequest {
   sender_user_id: string;
   from_branch_id: string;
   to_branch_id: string;
   sku_id: string;
   request_quantity: number;
}

export interface IUpdateSkuRequestRequest {
   state: string;
}

export const requestApi = createApi({
   reducerPath: 'request-api',
   tagTypes: ['Requests'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getSkuRequests: builder.query<
         PaginationResponse<TSkuRequest>,
         IGetSkuRequestsParams
      >({
         query: (params: IGetSkuRequestsParams) => ({
            url: '/api/v1/application-requests',
            method: 'GET',
            params: params,
         }),
         providesTags: ['Requests'],
      }),
      getSkuRequestById: builder.query<TSkuRequest, string>({
         query: (id: string) => ({
            url: `/api/v1/application-requests/${id}`,
            method: 'GET',
         }),
         providesTags: ['Requests'],
      }),
      createSkuRequest: builder.mutation<TSkuRequest, ICreateSkuRequestRequest>({
         query: (body: ICreateSkuRequestRequest) => ({
            url: '/api/v1/application-requests',
            method: 'POST',
            body: body,
         }),
         invalidatesTags: ['Requests'],
      }),
      updateSkuRequest: builder.mutation<
         TSkuRequest,
         { id: string; body: IUpdateSkuRequestRequest }
      >({
         query: ({ id, body }: { id: string; body: IUpdateSkuRequestRequest }) => ({
            url: `/api/v1/application-requests/${id}`,
            method: 'PUT',
            body: body,
         }),
         invalidatesTags: ['Requests'],
      }),
   }),
});

export const {
   useLazyGetSkuRequestsQuery,
   useLazyGetSkuRequestByIdQuery,
   useCreateSkuRequestMutation,
   useUpdateSkuRequestMutation,
} = requestApi;
