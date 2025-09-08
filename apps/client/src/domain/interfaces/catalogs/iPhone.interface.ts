import {
   PROMOTION_EVENT_TYPE_ENUM,
   DISCOUNT_TYPE_ENUM,
} from '~/domain/enums/discount-type.enum';
import { IColorResponse } from '../common/color-response.interface';
import { IStorageResponse } from '../common/storage-response.interface';
import { IImageResponse } from '../common/image-response.interface';

export interface IIphonePromotionResponse {
   promotion_product_name: string;
   promotion_product_description: string;
   promotion_product_image: string;
   promotion_product_unit_price: number;
   promotion_id: string;
   promotion_title: string;
   promotion_event_type: PROMOTION_EVENT_TYPE_ENUM;
   promotion_discount_type: DISCOUNT_TYPE_ENUM;
   promotion_discount_value: number;
   promotion_final_price: number;
   promotion_product_slug: string;
   category_id: string;
   product_model_id: string;
   product_name_tag: string;
   product_variants: IIphonePromotionResponseWithVariant;
}

export interface IIphoneResponse {
   product_id: string;
   product_model: string;
   product_color: IColorResponse;
   product_storage: IStorageResponse;
   product_unit_price: number;
   product_available_in_stock: number;
   total_sold: number;
   product_state: string;
   product_description: string;
   product_name_tag: string;
   product_images: IImageResponse[];
   promotion: IIphoneDetailsPromotionResponse | null;
   product_slug: string;
   iphone_model_id: string;
   category_id: string;
   is_deleted: boolean;
   deleted_by: string | null;
   created_at: string;
   updated_at: string;
   deleted_at: string | null;
}

export interface IIphoneDetailsPromotionResponse {
   promotion_id: string;
   promotion_product_id: string;
   product_model_id: string | null;
   promotion_product_slug: string;
   promotion_title: string;
   promotion_event_type: string;
   promotion_discount_type: string;
   promotion_discount_value: 0.15;
   promotion_final_price: 594.15;
   product_name_tag: string;
   category_id: string;
}

export interface IIphonePromotionResponseWithVariant {
   product_id: string;
   product_color_image: string;
   color_name: string;
   color_hex: string;
   color_image: string;
   order: string;
}
