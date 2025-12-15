import { TColor, TModel, TStorage } from './catalog.type';

export type TPromotion = {
   promotion_id: string;
   promotion_type: string;
   discount_type: string;
   discount_value: number;
};

export type TCartItem = {
   is_selected: boolean;
   model_id: string;
   sku_id: string;
   product_name: string;
   color: TColor;
   model: TModel;
   storage: TStorage;
   display_image_url: string;
   unit_price: number;
   quantity: number;
   quantity_remain: number; // server-provided available stock for this SKU
   sub_total_amount: number;
   promotion: TPromotion | null;
   discount_amount: number;
   total_amount: number;
   index: number;
};

export type TCart = {
   user_email: string;
   cart_items: TCartItem[];
   sub_total_amount: number;
   promotion_id: string | null;
   promotion_type: string | null;
   discount_type: string | null;
   discount_value: string | null;
   discount_amount: string | null;
   max_discount_amount: number | null;
   discount_coupon_error: string | null;
   total_amount: number;
};

export interface IStoreBasketItemPayload {
   is_selected: boolean;
   sku_id: string;
   quantity: number;
}

export interface IStoreBasketPayload {
   cart_items: IStoreBasketItemPayload[];
}

export interface IStoreEventItemPayload {
   event_item_id: string;
}

export interface ICheckoutPayload {
   shipping_address: {
      contact_name: string;
      contact_phone_number: string;
      address_line: string;
      district: string;
      province: string;
      country: string;
   };
   payment_method: string;
   discount_code: string | null;
}

export type TUSerInfo = {
   email: string;
   first_name: string;
   last_name: string;
   phone_number: string;
   birth_date: string;
   image_id: string;
   image_url: string;
   default_address_label: string;
   default_contact_name: string;
   default_contact_phone_number: string;
   default_address_line: string;
   default_address_district: string;
   default_address_province: string;
   default_address_country: string;
};

export type TCheckoutBasketItem = {
   is_selected: boolean;
   model_id: string;
   sku_id: string;
   product_name: string;
   color: TColor;
   model: TModel;
   storage: TStorage;
   display_image_url: string;
   unit_price: number;
   quantity: number;
   quantity_remain: number;
   sub_total_amount: number;
   promotion: TPromotion | null;
   discount_amount: number;
   total_amount: number;
   index: number;
};

export type TCheckoutBasket = {
   user_email: string;
   cart_items: TCheckoutBasketItem[];
   sub_total_amount: number;
   promotion_id: string;
   promotion_type: string;
   discount_type: string;
   discount_value: number;
   discount_amount: number;
   max_discount_amount: number | null;
   total_amount: number;
};

// Query Parameters
export interface IGetBasketQueryParams {
   _couponCode: string | null;
}
