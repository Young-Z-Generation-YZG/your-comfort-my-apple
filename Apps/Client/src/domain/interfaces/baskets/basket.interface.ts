import { IBasketPromotionResponse, IPromotion } from './promotion.interface';

export interface IBasketItem {
   product_id: string;
   product_name: string;
   product_color_name: string;
   product_unit_price: number;
   product_name_tag: string;
   product_image: string;
   product_slug: string;
   category_id: string;
   quantity: number;
   promotion: IPromotion | null;
   order: number;
}

export interface IStoreBasketPayload {
   cart_items: IBasketItem[];
}

export interface IGetBasketResponse {
   user_email: string;
   cart_items: ICartItemResponse[];
   total_amount: number;
}

export interface ICartItemResponse {
   product_id: string;
   product_name: string;
   product_color_name: string;
   product_unit_price: number;
   product_name_tag: string;
   product_image: string;
   product_slug: string;
   category_id: string;
   quantity: number;
   sub_total_amount: number;
   promotion: IBasketPromotionResponse;
   order_index: number;
}

export interface ICheckoutResponse {
   payment_redirect_url: string;
}
