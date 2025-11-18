import { createApi } from '@reduxjs/toolkit/query/react';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import {
   TAverageRating,
   TColorItem,
   TModelItem,
   TPromotion,
   TRatingStar,
   TShowcaseImage,
   TSkuPrice,
   TStorageItem,
} from './product.service';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export type ProductModelItem = {
   id: string;
   category: TCategoryItem;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModelItem[];
   color_items: TColorItem[];
   storage_items: TStorageItem[];
   sku_prices: TSkuPrice[];
   showcase_images: TShowcaseImage[];
   description: string;
   average_rating: TAverageRating;
   rating_stars: TRatingStar[];
   overall_sold: number;
   promotion: TPromotion | null;
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TCategoryItem = {
   id: string;
   name: string;
   description: string;
   order: number;
   slug: string;
   parent_category: TCategoryItem | null;
   sub_categories: TCategoryItem[] | null;
   product_models: ProductModelItem[] | null;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export interface ICreateCategoryPayload {
   name: string;
   description: string;
   parent_id: string;
}

export const categoryApi = createApi({
   reducerPath: 'category-api',
   tagTypes: ['Categories'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getCategories: builder.query<TCategoryItem[], void>({
         query: () => `/api/v1/categories`,
         providesTags: ['Categories'],
      }),
      getCategoryDetails: builder.query<TCategoryItem, string>({
         query: (categoryId) => `/api/v1/categories/${categoryId}`,
         providesTags: ['Categories'],
      }),
      createCategory: builder.mutation<boolean, ICreateCategoryPayload>({
         query: (payload) => ({
            url: '/api/v1/categories',
            method: 'POST',
            body: payload,
         }),
         invalidatesTags: ['Categories'],
      }),
   }),
});

export const {
   useLazyGetCategoriesQuery,
   useLazyGetCategoryDetailsQuery,
   useCreateCategoryMutation,
} = categoryApi;
