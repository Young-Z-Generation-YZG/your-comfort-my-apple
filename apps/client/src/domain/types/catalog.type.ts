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

export type TImage = {
   image_id: string;
   image_url: string;
   image_name: string;
   image_description: string;
   image_width: number;
   image_height: number;
   image_bytes: number;
   image_order: number;
};

export type TRatingStar = {
   star: number;
   count: number;
};

export type TAverageRating = {
   rating_average_value: number;
   rating_count: number;
};

export type TSkuPrice = {
   sku_id: string;
   normalized_model: string;
   normalized_color: string;
   normalized_storage: string;
   unit_price: number;
};

export type TIphoneModelDetails = {
   id: string;
   category: TCategory;
   name: string;
   model_items: TModel[];
   color_items: TColor[];
   storage_items: TStorage[];
   sku_prices: TSkuPrice[];
   description: string;
   showcase_images: TImage[];
   overall_sold: number;
   rating_stars: TRatingStar[];
   average_rating: TAverageRating;
   branchs: {
      branch: TBranch;
      skus: TSku[];
   }[];
};

export type TProductModel = {
   id: string;
   category: TCategory;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModel[];
   color_items: TColor[];
   storage_items: TStorage[];
   sku_prices: TSkuPrice[];
   showcase_images: TImage[];
   description: string;
   average_rating: TAverageRating;
   rating_stars: TRatingStar[];
   overall_sold: number;
   promotion: null;
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TPopularProduct = {
   id: string;
   category: TCategory;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModel[];
   color_items: TColor[];
   storage_items: TStorage[];
   sku_prices: TSkuPrice[];
   description: string;
   showcase_images: TImage[];
   overall_sold: number;
   rating_stars: TRatingStar[];
   average_rating: TAverageRating;
   promotion: {
      promotion_type: 'COUPON' | 'EVENT';
      promotion_discount_type: 'PERCENTAGE' | 'FIXED_AMOUNT';
      promotion_discount_value: number;
      promotion_discount_amount: number;
      final_price: number;
   } | null;
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TNewestProduct = {
   id: string;
   category: TCategory;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModel[];
   color_items: TColor[];
   storage_items: TStorage[];
   sku_prices: TSkuPrice[];
   description: string | null;
   showcase_images: TImage[];
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TSuggestionProduct = {
   id: string;
   category: TCategory;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModel[];
   color_items: TColor[];
   storage_items: TStorage[];
   sku_prices: TSkuPrice[];
   description: string;
   showcase_images: TImage[];
   overall_sold: number;
   rating_stars: TRatingStar[];
   average_rating: TAverageRating;
   promotion: {
      promotion_type: 'COUPON' | 'EVENT_ITEM';
      promotion_discount_type: 'PERCENTAGE' | 'FIXED_AMOUNT';
      promotion_discount_value: number;
      promotion_discount_amount: number;
      final_price: number;
   } | null;
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

// Review Types
export type TReviewItem = {
   id: string;
   model_id: string;
   sku_id: string;
   order_info: {
      order_id: string;
      order_item_id: string;
   };
   customer_review_info: {
      name: string;
      avatar_image_url: string | null;
   };
   rating: number;
   content: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TReviewInOrderItem = {
   id: string;
   model_id: string;
   sku_id: string;
   order_info: {
      order_id: string;
      order_item_id: string;
   };
   rating: number;
   content: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

// Tenant Types
export type TTenant = {
   id: string;
   name: string;
   sub_domain: string;
   description: string;
   tenant_type: string;
   tenant_state: string;
   embedded_branch: TBranch;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TBranch = {
   id: string;
   tenant_id: string;
   name: string;
   address: string;
   description: string;
   manager: any;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TCategory = {
   id: string;
   name: string;
   description: string;
   order: number;
   slug: string;
   parent_id: string | null;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};
