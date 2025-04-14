import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { IBasketItem } from '../interfaces/baskets/basket.interface';
import { IPromotion } from '../interfaces/baskets/promotion.interface';
import { ICheckoutPayload } from '../interfaces/baskets/checkout.interface';

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

const checkoutSchema = z.object({
   shipping_address: z.object({
      contact_name: z.string().min(1, {
         message: 'Contact name is required',
      }),
      contact_phone_number: z.string().min(1, {
         message: 'Contact phone number is required',
      }),
      address_line: z.string().min(1, {
         message: 'Address line is required',
      }),
      district: z.string().min(1, { message: 'District is required' }),
      province: z.string().min(1, { message: 'Province is required' }),
      country: z.string().min(1, { message: 'Country is required' }),
   }),
   payment_method: z.enum(['COD', 'VNPAY', 'MOMO'], {
      errorMap: () => ({ message: 'Payment method is required' }),
   }),
   discount_code: z.string().nullable().default(null),
   sub_total_amount: z.number().min(0, {
      message: 'Sub total amount is required',
   }),
   discount_amount: z.number().min(0, {
      message: 'Discount amount is required',
   }),
   total_amount: z.number().min(0, {
      message: 'Total amount is required',
   }),
} satisfies Record<keyof ICheckoutPayload, any>);

export type CheckoutFormType = z.infer<typeof checkoutSchema>;
export const CheckoutResolver = zodResolver(checkoutSchema);
