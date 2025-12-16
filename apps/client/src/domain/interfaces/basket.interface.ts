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
   promotion: null;
   quantity: number;
   order: number;
}
