import { IPromotion } from './promotion.interface';

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
