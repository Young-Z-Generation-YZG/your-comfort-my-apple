import {
   PROMOTION_EVENT_TYPE_ENUM,
   DISCOUNT_TYPE_ENUM,
} from '~/domain/enums/discount-type.enum';

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
   product_name_tag: string;
   product_variants: IIphonePromotionResponseWithVariant;
}

export interface IIphonePromotionResponseWithVariant {
   product_id: string;
   product_color_image: string;
   color_name: string;
   color_hex: string;
   color_image: string;
   order: string;
}
