import { PromotionResponse } from '../common/promotion.interface';

export interface OrderResponse {}

export interface OrderDetailsResponse {
   order_id: string;
   order_code: string;
   order_customer_email: string;
   order_status: string;
   order_payment_method: string;
   order_shipping_address: {
      contact_name: string;
      contact_email: string;
      contact_phone_number: string;
      contact_address_line: string;
      contact_district: string;
      contact_province: string;
      contact_country: string;
   };
   order_items: OrderItemResponse[];
   order_sub_total_amount: number;
   order_discount_amount: number;
   order_total_amount: number;
   order_created_at: string;
   order_updated_at: string;
   order_last_modified_by: null;
}

interface OrderItemResponse {
   order_item_id: string;
   product_id: string;
   product_name: string;
   product_image: string;
   product_color_name: string;
   product_unit_price: number;
   quantity: number;
   promotion: PromotionResponse | null;
   sub_total_amount: number;
}
