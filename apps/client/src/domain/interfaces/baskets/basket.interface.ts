import {
   IBasketPromotionResponse,
   IPromotionPayload,
} from './promotion.interface';

export interface IBasketItemPayload {
   product_id: string;
   model_id: string;
   product_name: string;
   product_color_name: string;
   product_unit_price: number;
   product_name_tag: string;
   product_image: string;
   product_slug: string;
   category_id: string;
   quantity: number;
   promotion: IPromotionPayload | null;
   order: number;
}

export interface IStoreBasketPayload {
   cart_items: IBasketItemPayload[];
}

export interface IGetBasketQueries {
   _couponCode?: string | null;
}

export interface IGetBasketResponse {
   user_email: string;
   cart_items: ICartItemResponse[];
   total_amount: number;
}

export interface ICartItemResponse {
   product_id: string;
   model_id: string;
   product_name: string;
   product_color_name: string;
   product_unit_price: number;
   product_name_tag: string;
   product_image: string;
   product_slug: string;
   category_id: string;
   quantity: number;
   sub_total_amount: number;
   promotion: IBasketPromotionResponse | null;
   order_index: number;
}

export interface ICheckoutResponse {
   payment_redirect_url: string | null;
   order_details_redirect_url: string | null;
}
