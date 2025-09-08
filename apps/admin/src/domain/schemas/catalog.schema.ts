import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import {
   IColorItemPayload,
   ICreateModelPayload,
   IImagePayload,
   IModelItemPayload,
} from '../interfaces/catalogs/payloads/ICreateModelPayload';

/// Create iPhone Model schema
const ModelItemSchema = z.object({
   model_name: z.string().min(1, {
      message: 'Model name is required',
   }),
   model_order: z.number().min(0, {
      message: 'Order index must be greater than or equal to 0',
   }),
} satisfies Record<keyof IModelItemPayload, any>);

const ColorItemSchema = z.object({
   color_name: z.string().min(1, {
      message: 'Color name is required',
   }),
   color_hex: z.string().min(1, {
      message: 'Color Hex is required',
   }),
   color_image: z.string().min(1, {
      message: 'Color image is required',
   }),
   color_order: z.number().min(0, {
      message: 'Order index must be greater than or equal to 0',
   }),
} satisfies Record<keyof IColorItemPayload, any>);

const ImageItemSchema = z.object({
   image_id: z.string().min(1, {
      message: 'Image ID is required',
   }),
   image_url: z.string().min(1, {
      message: 'Image URL is required',
   }),
   image_name: z.string().min(1, {
      message: 'Image name is required',
   }),
   image_description: z.string().min(1, {
      message: 'Image description is required',
   }),
   image_width: z.number().min(0, {
      message: 'Image width must be greater than or equal to 0',
   }),
   image_height: z.number().min(0, {
      message: 'Image height must be greater than or equal to 0',
   }),
   image_bytes: z.number().min(0, {
      message: 'Image bytes must be greater than or equal to 0',
   }),
   image_order: z.number().min(0, {
      message: 'Order index must be greater than or equal to 0',
   }),
} satisfies Record<keyof IImagePayload, any>);

const CreateModelSchema = z.object({
   name: z.string().min(1, {
      message: 'Model name is required',
   }),
   models: z.array(ModelItemSchema).min(1, {
      message: 'At least one model is required',
   }),
   colors: z.array(ColorItemSchema).min(1, {
      message: 'At least one color is required',
   }),
   storages: z
      .array(z.number().int().positive('Storage must be a positive integer'))
      .min(1, {
         message: 'At least one storage is required',
      }),
   description: z.string().min(1, {
      message: 'Description is required',
   }),
   description_images: z.array(ImageItemSchema).min(1, {
      message: 'At least one image is required',
   }),
   category_id: z.string().min(1, {
      message: 'Category ID is required',
   }),
} satisfies Record<keyof ICreateModelPayload, any>);

export type TCreateModelSchema = z.infer<typeof CreateModelSchema>;
export const CreateModelResolver = zodResolver(CreateModelSchema);
