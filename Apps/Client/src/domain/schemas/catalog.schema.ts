import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { IReviewPayload } from '../interfaces/catalogs/review.interface';

const reviewSchema = z.object({
   product_id: z.string().min(1, {
      message: 'Product ID is required',
   }),
   model_id: z.string().min(1, {
      message: 'Model ID is required',
   }),
   order_item_id: z.string().min(1, {
      message: 'Order Item ID is required',
   }),
   rating: z.number().min(1, {
      message: 'Rating is required',
   }),
   content: z.string().min(1, {
      message: 'Review is required',
   }),
} satisfies Record<keyof IReviewPayload, any>);

export type ReviewFormType = z.infer<typeof reviewSchema>;

export const ReviewResolver = zodResolver(reviewSchema);
