import { EOrderNotificationStatus } from '~/domain/enums/order-notification-status';
import { EOrderNotificationType } from '~/domain/enums/order-notification-type';

export type TOrderItem = {
   order_item_id: string;
   order_id: string;
   tenant_id: string | null;
   branch_id: string | null;
   sku_id: string | null;
   model_id: string;
   model_name: string;
   color_name: string;
   storage_name: string;
   unit_price: number;
   display_image_url: string;
   model_slug: string;
   quantity: number;
   is_reviewed: boolean;
   promotion_id?: string | null;
   promotion_type?: string | null;
   discount_type?: string | null;
   discount_value?: number | null;
   discount_amount?: number | null;
   sub_total_amount?: number | null;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TOrder = {
   tenant_id: string | null;
   branch_id: string | null;
   order_id: string;
   customer_id: string;
   customer_email: string;
   order_code: string;
   status: string;
   payment_method: string;
   shipping_address: {
      contact_name: string;
      contact_email: string;
      contact_phone_number: string;
      contact_address_line: string;
      contact_district: string;
      contact_province: string;
      contact_country: string;
   };
   order_items: TOrderItem[];
   promotion_id: null;
   promotion_type: string | null;
   discount_type: string | null;
   discount_value: number | null;
   discount_amount: number | null;
   total_amount: number;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

// Notification
export type TNotification = {
   id: string;
   sender_id: string | null;
   receiver_id: string | null;
   title: string;
   content: string;
   type: EOrderNotificationType;
   status: EOrderNotificationStatus;
   is_read: boolean;
   link: string;
   is_system: boolean;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};
