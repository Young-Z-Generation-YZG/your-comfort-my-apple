import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import {
   ICreatePromotionCategoryPayload,
   ICreatePromotionEventPayload,
   ICreatePromotionProductPayload,
} from '~/src/domain/interfaces/discounts/ICreatePromotionEventPayload';

const StateEnumArray = ['ACTIVE', 'INACTIVE'] as const;

const promotionProductSchema = z
   .object({
      product_slug: z.string().min(1, {
         message: 'Product is required',
      }),
      product_image: z.string(),
      discount_type: z.enum(['PERCENTAGE', 'FIXED']),
      discount_value: z.number().min(0, {
         message: 'Discount value must be greater than 0',
      }),
   } satisfies Record<keyof ICreatePromotionProductPayload, any>)
   .superRefine((data, ctx) => {
      if (data.discount_type === 'PERCENTAGE' && data.discount_value === 0) {
         ctx.addIssue({
            code: z.ZodIssueCode.custom,
            message:
               'Discount value must be greater than 0 when discount type is PERCENTAGE',
            path: ['discount_value'],
         });
      }
   });

const promotionCategorySchema = z
   .object({
      category_id: z.string().min(1, {
         message: 'Category is required',
      }),
      category_name: z.string(),
      category_slug: z.string(),
      discount_type: z.enum(['PERCENTAGE', 'FIXED']),
      discount_value: z.number().min(0, {
         message: 'Discount value must be greater than 0',
      }),
      promotion_products: z.array(promotionProductSchema),
   } satisfies Record<keyof ICreatePromotionCategoryPayload, any>)
   .superRefine((data, ctx) => {
      if (data.discount_type === 'PERCENTAGE' && data.discount_value === 0) {
         ctx.addIssue({
            code: z.ZodIssueCode.custom,
            message:
               'Discount value must be greater than 0 when discount type is PERCENTAGE',
            path: ['discount_value'],
         });
      }
   });

const promotionEventSchema = z.object({
   event_title: z.string().min(1, {
      message: 'Event title is required',
   }),
   event_description: z.string().min(1, {
      message: 'Event description is required',
   }),
   event_valid_from: z.date().refine((date) => date > new Date(), {
      message: 'Event valid from date must be in the future',
   }),
   event_valid_to: z.date().refine((date) => date > new Date(), {
      message: 'Event valid to date must be in the future',
   }),
   event_state: z.enum(StateEnumArray, {
      message: `Select a valid event state`,
   }),
   promotion_categories: z.array(promotionCategorySchema),
   promotion_products: z.array(promotionProductSchema),
} satisfies Record<keyof ICreatePromotionEventPayload, any>);

export type PromotionEventSchemaType = z.infer<typeof promotionEventSchema>;
export const promotionEventResolver = zodResolver(promotionEventSchema);
