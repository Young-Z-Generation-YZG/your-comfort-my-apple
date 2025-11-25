import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';
import {
   TCategory,
   TSkuPrice,
   TStorageItem,
   TColorItem,
   TModelItem,
   TShowcaseImage,
   TRatingStar,
   TAverageRating,
} from './product.service';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export type TIphoneModelDetails = {
   id: string;
   category: TCategory;
   name: string;
   model_items: TModelItem[];
   color_items: TColorItem[];
   storage_items: TStorageItem[];
   sku_prices: TSkuPrice[];
   description: string;
   showcase_images: TShowcaseImage[];
   overall_sold: number;
   rating_stars: TRatingStar[];
   average_rating: TAverageRating;
   branchs: TBranchWithSkus[];
};

export type TBranch = {
   id: string;
   tenant_id: string;
   name: string;
   address: string;
   description: string;
   manager: any;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TModel = {
   name: string;
   normalized_name: string;
   order: number;
};

export type TColor = {
   name: string;
   normalized_name: string;
   hex_code: string;
   showcase_image_id: string;
   order: number;
};

export type TStorage = {
   name: string;
   normalized_name: string;
   order: number;
};

export type TReservedForEvent = {
   event_id: string;
   event_item_id: string;
   event_name: string;
   reserved_quantity: number;
};

export type TSku = {
   id: string;
   code: string;
   model_id: string;
   tenant_id: string;
   branch_id: string;
   product_classification: string;
   model: TModel;
   color: TColor;
   storage: TStorage;
   unit_price: number;
   available_in_stock: 48;
   total_sold: 0;
   reserved_for_event: TReservedForEvent;
   state: string;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   deleted_at: string | null;
   deleted_by: string | null;
   is_deleted: boolean;
};

export type TBranchWithSkus = {
   branch: TBranch;
   skus: TSku[];
};

export type TGetIphoneModelsFilter = {
   _page?: number | null;
   _limit?: number | null;
   _colors?: string[] | null;
   _storages?: string[] | null;
   _models?: string[] | null;
   _minPrice?: number | null;
   _maxPrice?: number | null;
   _priceSort?: 'ASC' | 'DESC' | null;
};

export const catalogApi = createApi({
   reducerPath: 'catalog-api',
   tagTypes: ['Catalogs'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getIphonePromotions: builder.query<PaginationResponse<any>, void>({
         query: () => '/api/v1/products/iphone/promotions',
         providesTags: ['Catalogs'],
         transformResponse: (response) => {
            return response as PaginationResponse<any>;
         },
      }),
      getIphoneModels: builder.query<
         PaginationResponse<any>,
         TGetIphoneModelsFilter
      >({
         query: (params) => ({
            url: '/api/v1/products/iphone/models',
            params,
         }),
         providesTags: ['Catalogs'],
      }),
      getModelBySlug: builder.query<TIphoneModelDetails, string>({
         query: (slug) => `/api/v1/products/iphone/${slug}`,
         providesTags: ['Catalogs'],
      }),
      getIPhonesByModel: builder.query<any[], string>({
         query: (slug) => `/api/v1/products/iphone/models/${slug}/products`,
         providesTags: ['Catalogs'],
      }),
   }),
});

export const { useLazyGetIphoneModelsQuery, useLazyGetModelBySlugQuery } =
   catalogApi;
