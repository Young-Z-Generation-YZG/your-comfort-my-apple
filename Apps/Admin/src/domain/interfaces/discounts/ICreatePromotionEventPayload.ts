export interface ICreatePromotionEventPayload {
   event_title: string;
   event_description: string;
   event_valid_from: Date;
   event_valid_to: Date;
   event_state: string;
   promotion_categories: ICreatePromotionCategoryPayload[];
   promotion_products: ICreatePromotionCategoryPayload[];
}

export interface ICreatePromotionCategoryPayload {
   category_id: string;
   category_name: string;
   category_slug: string;
   discount_type: string;
   discount_value: number;
}

export interface ICreatePromotionProductPayload {
   product_id: string;
   product_slug: string;
   product_image: string;
   discount_type: string;
   discount_value: number;
}
