import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { IBasketItem } from '../interfaces/baskets/basket.interface';
import { IPromotion } from '../interfaces/baskets/promotion.interface';

const promotionSchema = z.object({
   promotion_id_or_code: z.string().min(1, {
      message: 'Promotion ID or code is required',
   }),
   promotion_event_type: z.string().min(1, {
      message: 'Promotion event type is required',
   }),
} satisfies Record<keyof IPromotion, any>);

const basketItemSchema = z.object({
   product_id: z.string().min(1, { message: 'Product ID is required' }),
   product_name: z.string().min(1, { message: 'Product name is required' }),
   product_color_name: z
      .string()
      .min(1, { message: 'Please choose your color' }),
   product_unit_price: z.number().min(1, {
      message: 'Product unit price is required',
   }),
   product_name_tag: z
      .string()
      .min(1, { message: 'Product name tag is required' }),
   product_image: z.string().min(1, { message: 'Product image is required' }),
   product_slug: z.string().min(1, { message: 'Product slug is required' }),
   category_id: z.string().min(1, { message: 'Category ID is required' }),
   promotion: promotionSchema.optional().nullable(),
   quantity: z.number().min(1, { message: 'Quantity is required' }),
   order: z.number(),
} satisfies Record<keyof IBasketItem, any>);

const storeBasketSchema = z.object({
   cart_items: z.array(basketItemSchema).min(1, {
      message: 'At least one item is required',
   }),
});

export type StoreBasketFormType = z.infer<typeof basketItemSchema>;

export const StoreBasketResolver = zodResolver(basketItemSchema);
