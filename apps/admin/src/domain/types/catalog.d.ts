export type TSku = {
   id: string;
   code: string;
   model_id: string;
   tenant_id: string;
   branch_id: string;
   product_classification: string;
   model: TModel;
   color: TColor;
   storage: TStorage;
   display_image_url: string;
   unit_price: number;
   available_in_stock: number;
   total_sold: number;
   reserved_for_event: {
      event_id: string;
      event_item_id: string;
      event_name: string;
      reserved_quantity: number;
   } | null;
   state: string;
   slug: string;
   created_at: string;
   updated_at: string;
   deleted_at: string | null;
   deleted_by: string | null;
   is_deleted: boolean;
};

export type TModel = {
   name: string;
   normalized_name: string;
   order: number;
};

export type TColor = {
   name: string;
   normalized_name: string;
   hex_code: string;
   showcase_image_id: string;
   order: number;
};

export type TStorage = {
   name: string;
   normalized_name: string;
   order: number;
};
