import { IPromotionProductResponse } from '../discounts/promotion-product-response.interface';

export interface PaginationWithPromotionResponse<T> {
   total_records: number;
   total_pages: number;
   page_size: number;
   current_page: number;
   items: T[];
   promotion_items: IPromotionProductResponse[] | null;
   links: PaginationLinks;
}

export interface PaginationLinks {
   first?: string | null;
   prev?: string | null;
   next?: string | null;
   last?: string | null;
}
