import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { IUpdateOrderPayload } from '~/src/domain/interfaces/order.interface';

const UpdateOrderStatusSchema = z.object({
   order_id: z.string().min(1, {
      message: 'Order ID is required',
   }),
   update_status: z.enum(['PREPARING', 'DELIVERING', 'DELIVERED'], {
      message: 'Update status is required',
   }),
} satisfies Record<keyof IUpdateOrderPayload, any>);

export type UpdateOrderStatusFormType = z.infer<typeof UpdateOrderStatusSchema>;

export const UpdateOrderStatusResolver = zodResolver(UpdateOrderStatusSchema);
