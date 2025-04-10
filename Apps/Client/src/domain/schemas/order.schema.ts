import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { IIpnCallbackPayload } from '../interfaces/orders/ipn-callback.interface';

const vnpayIpnSchema = z.object({
   vnp_Amount: z.string().default('0'),
   vnp_BankCode: z.string().default(''),
   vnp_BankTranNo: z.string().default(''),
   vnp_CardType: z.string().default(''),
   vnp_OrderInfo: z.string().default(''),
   vnp_PayDate: z.string().default(''),
   vnp_ResponseCode: z.string().default(''),
   vnp_TmnCode: z.string().default(''),
   vnp_TransactionNo: z.string().default(''),
   vnp_TransactionStatus: z.string().default(''),
   vnp_TxnRef: z.string().default(''),
   vnp_SecureHash: z.string().default(''),
} satisfies Record<keyof IIpnCallbackPayload, any>);

export type VnpayIpnFormType = z.infer<typeof vnpayIpnSchema>;

export const VnpayIpnResolver = zodResolver(vnpayIpnSchema);
