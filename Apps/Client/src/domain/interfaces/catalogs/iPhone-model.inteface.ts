import { IColorResponse } from '../common/color-response.interface';
import { IImageResponse } from '../common/image-response.interface';
import { IStorageResponse } from '../common/storage-response.interface';

export interface IIphoneModelResponse {
   model_id: string;
   model_name: string;
   model_items: IModelItemResponse[];
   color_items: IColorResponse[];
   storage_items: IStorageResponse[];
   general_model: string;
   model_description: string;
   minimun_unit_price: number;
   maximun_unit_price: number;
   overall_sold: number;
   average_rating: IAverageRatingResponse;
   rating_stars: IRatingStarResponse[];
   model_images: IImageResponse[];
   model_promotion: IModelPromotionResponse;
   model_slug: string;
   category_id: string;
   is_deleted: boolean;
   deleted_by: string | null;
   created_at: string;
   updated_at: string;
   deleted_at: string | null;
}

export interface IModelItemResponse {
   model_name: string;
   model_order: number;
}

export interface IAverageRatingResponse {
   rating_average_value: number;
   rating_count: number;
}

export interface IRatingStarResponse {
   star: number;
   count: number;
}

export interface IModelPromotionResponse {
   minimum_promotion_price: number;
   maximum_promotion_price: number;
   minimum_discount_percentage: number;
   maximum_discount_percentage: number;
}
