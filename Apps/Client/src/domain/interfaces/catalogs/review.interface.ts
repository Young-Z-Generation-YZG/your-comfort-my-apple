export interface IReviewPayload {
   product_id: string;
   model_id: string;
   rating: number;
   content: string;
   order_item_id: string;
}

export interface IReviewResponse {
   review_id: string;
   customer_name: string;
   customer_image: string;
   rating: number;
   content: string;
   created_at: string;
}
