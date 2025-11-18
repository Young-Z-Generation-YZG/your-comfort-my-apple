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

export const promotionApi = createApi({
   reducerPath: 'promotion-api',
   tagTypes: ['Promotions'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getEventDetails: builder.query<TEvent, string>({
         query: (eventId) => `/api/v1/promotions/events/${eventId}`,
         providesTags: ['Promotions'],
      }),
   }),
});

export const { useLazyGetEventDetailsQuery } = promotionApi;
