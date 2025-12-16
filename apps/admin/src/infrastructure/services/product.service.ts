import { createApi } from '@reduxjs/toolkit/query/react';
import { baseQuery } from './base-query';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import {
   TIphoneModelDetails,
   TProductModel,
} from '~/src/domain/types/catalog.type';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('/catalog-services')(args, api, extraOptions);

   return result;
};

export interface IGetProductModelsByCategorySlugQueryParams {
   _page?: number | null;
   _limit?: number | null;
   _colors?: string[] | null;
   _storages?: string[] | null;
   _models?: string[] | null;
   _minPrice?: number | null;
   _maxPrice?: number | null;
   _priceSort?: 'ASC' | 'DESC' | null;
}

// export type TProductModel = {
//    id: string;
//    category: TCategory;
//    name: string;
//    normalized_model: string;
//    product_classification: string;
//    model_items: TModelItem[];
//    color_items: TColorItem[];
//    storage_items: TStorageItem[];
//    sku_prices: TSkuPrice[];
//    showcase_images: TShowcaseImage[];
//    description: string;
//    average_rating: TAverageRating;
//    rating_stars: TRatingStar[];
//    overall_sold: number;
//    promotion: TPromotion | null;
//    is_newest: boolean;
//    slug: string;
//    created_at: string;
//    updated_at: string;
//    updated_by: string | null;
//    is_deleted: boolean;
//    deleted_at: string | null;
//    deleted_by: string | null;
// };

// export type TCategory = {
//    id: string;
//    name: string;
//    description: string;
//    order: number;
//    slug: string;
//    parent_id: string | null;
//    created_at: string;
//    updated_at: string;
//    updated_by: string | null;
//    is_deleted: boolean;
//    deleted_at: string | null;
//    deleted_by: string | null;
// };
// export type TModelItem = {
//    name: string;
//    normalized_name: string;
//    order: number;
// };
// export type TColorItem = {
//    name: string;
//    normalized_name: string;
//    hex_code: string;
//    showcase_image_id: string;
//    order: number;
// };
// export type TStorageItem = {
//    name: string;
//    normalized_name: string;
//    order: number;
// };
// export type TSkuPrice = {
//    sku_id: string;
//    normalized_model: string;
//    normalized_color: string;
//    normalized_storage: string;
//    unit_price: number;
// };
// export type TShowcaseImage = {
//    image_id: string;
//    image_url: string;
//    image_name: string;
//    image_description: string;
//    image_width: number;
//    image_height: number;
//    image_bytes: number;
//    image_order: number;
// };
// export type TRatingStar = {
//    star: number;
//    count: number;
// };
// export type TAverageRating = {
//    rating_average_value: number;
//    rating_count: number;
// };
// export type TPromotion = {
//    promotion_type: 'INSTANT_DISCOUNT' | 'EVENT';
//    promotion_discount_type: 'PERCENTAGE' | 'FIXED_AMOUNT';
//    promotion_discount_value: number;
//    promotion_discount_amount: number;
//    final_price: number;
// };

// export type TPopularProduct = {
//    id: string;
//    category: TCategory;
//    name: string;
//    normalized_model: string;
//    product_classification: string;
//    model_items: TModelItem[];
//    color_items: TColorItem[];
//    storage_items: TStorageItem[];
//    sku_prices: TSkuPrice[];
//    description: string;
//    showcase_images: TShowcaseImage[];
//    overall_sold: number;
//    rating_stars: TRatingStar[];
//    average_rating: TAverageRating;
//    promotion: TPromotion | null;
//    is_newest: boolean;
//    slug: string;
//    created_at: string;
//    updated_at: string;
//    updated_by: string | null;
//    is_deleted: boolean;
//    deleted_at: string | null;
//    deleted_by: string | null;
// };
// export type TNewestProduct = {
//    id: string;
//    category: TCategory;
//    name: string;
//    normalized_model: string;
//    product_classification: string;
//    model_items: TModelItem[];
//    color_items: TColorItem[];
//    storage_items: TStorageItem[];
//    sku_prices: TSkuPrice[];
//    description: string | null;
//    showcase_images: TShowcaseImage[];
//    is_newest: boolean;
//    slug: string;
//    created_at: string;
//    updated_at: string;
//    updated_by: string | null;
//    is_deleted: boolean;
//    deleted_at: string | null;
//    deleted_by: string | null;
// };
// export type TSuggestionProduct = {
//    id: string;
//    category: TCategory;
//    name: string;
//    normalized_model: string;
//    product_classification: string;
//    model_items: TModelItem[];
//    color_items: TColorItem[];
//    storage_items: TStorageItem[];
//    sku_prices: TSkuPrice[];
//    description: string;
//    showcase_images: TShowcaseImage[];
//    overall_sold: number;
//    rating_stars: TRatingStar[];
//    average_rating: TAverageRating;
//    promotion: TPromotion | null;
//    is_newest: boolean;
//    slug: string;
//    created_at: string;
//    updated_at: string;
//    updated_by: string | null;
//    is_deleted: boolean;
//    deleted_at: string | null;
//    deleted_by: string | null;
// };

export const productApi = createApi({
   reducerPath: 'product-api',
   tagTypes: ['Products'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getProductModelsByCategorySlug: builder.query<
         PaginationResponse<TProductModel>,
         {
            categorySlug: string;
            queryParams: IGetProductModelsByCategorySlugQueryParams;
         }
      >({
         query: ({
            categorySlug,
            queryParams,
         }: {
            categorySlug: string;
            queryParams: IGetProductModelsByCategorySlugQueryParams;
         }) => {
            return {
               url: `/api/v1/product-models/category/${categorySlug}`,
               params: queryParams,
            };
         },
         providesTags: ['Products'],
      }),
      getProductModelBySlug: builder.query<TIphoneModelDetails, string>({
         query: (slug: string) => ({
            url: `/api/v1/products/iphone/${slug}`,
            method: 'GET',
         }),
         providesTags: ['Products'],
      }),
   }),
});

export const {
   useLazyGetProductModelsByCategorySlugQuery,
   useLazyGetProductModelBySlugQuery,
} = productApi;
