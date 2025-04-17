import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';

export const selectorSchema = z.object({
   model: z.string().min(1, {
      message: 'Please choose your model',
   }),
   color: z.string().min(1, {
      message: 'Please choose your color',
   }),
   storage: z.string().min(1, {
      message: 'Please choose your storage',
   }),
});

export type SelectorFormType = z.infer<typeof selectorSchema>;
export const SelectorResolver = zodResolver(selectorSchema);
