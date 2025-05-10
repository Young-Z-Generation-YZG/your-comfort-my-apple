export interface IReviewPayload {
   product_id: string;
   model_id: string;
   rating: number;
   content: string;
   order_id: string;
   order_item_id: string;
   customer_username: string;
}

export interface IUpdateReviewPayload {
   rating: number;
   content: string;
}

export interface IReviewResponse {
   review_id: string;
   customer_name: string;
   customer_image: string;
   rating: number;
   content: string;
   created_at: string;
}

export interface IReviewByOrderResponse {
   review_id: string;
   product_id: string;
   model_id: string;
   order_id: string;
   order_item_id: string;
   rating: number;
   content: string;
   created_at: string;
}
