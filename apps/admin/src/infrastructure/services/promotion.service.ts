import { createApi } from '@reduxjs/toolkit/query/react';
import { baseQuery } from './base-query';
import { setLogout } from '../redux/features/auth.slice';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export type TEventItem = {
   id: string;
   event_id: string;
   model_name: string;
   normalized_model: string;
   color_name: string;
   normalized_color: string;
   color_hex_code: string;
   storage_name: string;
   normalized_storage: string;
   product_classification: string;
   image_url: string;
   discount_type: string;
   discount_value: number;
   original_price: number;
   stock: number;
   sold: number;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TEvent = {
   id: string;
   title: string;
   description: string;
   start_date: string;
   end_date: string;
   event_items: TEventItem[];
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export interface ICreateEventPayload {
   title: string;
   description: string;
   start_date: string;
   end_date: string;
}

export interface IUpdateEventPayload {
   title?: string | null;
   description?: string | null;
   start_date?: string | null;
   end_date?: string | null;
   add_event_items?:
      | {
           sku_id: string;
           discount_type: string;
           discount_value: number;
           stock: number;
        }[]
      | null;
   remove_event_item_ids?: string[] | null;
}

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getEvents: builder.query<TEvent[], void>({
         query: () => '/api/v1/promotions/events',
         providesTags: ['Promotions'],
      }),
      getEventDetails: builder.query<TEvent, string>({
         query: (eventId) => `/api/v1/promotions/events/${eventId}`,
         providesTags: ['Promotions'],
      }),
      createEvent: builder.mutation<boolean, ICreateEventPayload>({
         query: (payload) => ({
            url: '/api/v1/promotions/events',
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Promotions'],
      }),
      updateEvent: builder.mutation<
         boolean,
         { eventId: string; payload: IUpdateEventPayload }
      >({
         query: ({ eventId, payload }) => ({
            url: `/api/v1/promotions/events/${eventId}`,
            method: 'PATCH',
            body: payload,
         }),
         invalidatesTags: ['Promotions'],
      }),
   }),
});

export const {
   useLazyGetEventsQuery,
   useLazyGetEventDetailsQuery,
   useCreateEventMutation,
   useUpdateEventMutation,
} = promotionApi;
