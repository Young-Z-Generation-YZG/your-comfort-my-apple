import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import {
   IMomoIpnCallbackPayload,
   IVnpayIpnCallbackPayload,
} from '../interfaces/ordering.interface';

const vnpayIpnSchema = z.object({
   vnp_Amount: z
      .string()
      .min(1, {
         message: 'vnp_Amount code is required',
      })
      .default('0'),
   vnp_BankCode: z.string().min(1, {
      message: 'vnp_BankCode code is required',
   }),
   vnp_BankTranNo: z.string().min(1, {
      message: 'vnp_BankTranNo code is required',
   }),
   vnp_CardType: z.string().min(1, {
      message: 'vnp_CardType code is required',
   }),
   vnp_OrderInfo: z.string().min(1, {
      message: 'vnp_OrderInfo code is required',
   }),
   vnp_PayDate: z.string().min(1, {
      message: 'vnp_PayDate code is required',
   }),
   vnp_ResponseCode: z.string().min(1, {
      message: 'vnp_ResponseCode code is required',
   }),
   vnp_TmnCode: z.string().min(1, {
      message: 'vnp_TmnCode code is required',
   }),
   vnp_TransactionNo: z.string().min(1, {
      message: 'vnp_TransactionNo code is required',
   }),
   vnp_TransactionStatus: z.string().min(1, {
      message: 'vnp_TransactionStatus code is required',
   }),
   vnp_TxnRef: z.string().min(1, {
      message: 'vnp_TxnRef code is required',
   }),
   vnp_SecureHash: z.string().min(1, {
      message: 'vnp_SecureHash code is required',
   }),
} satisfies Record<keyof IVnpayIpnCallbackPayload, any>);

export type VnpayIpnFormType = z.infer<typeof vnpayIpnSchema>;
export const VnpayIpnResolver = zodResolver(vnpayIpnSchema);

// MOMO
const momoIpnSchema = z.object({
   momo_PartnerCode: z
      .string()
      .min(1, {
         message: 'momo_PartnerCode code is required',
      })
      .default('MOMO'),
   momo_AccessKey: z
      .string()
      .min(1, {
         message: 'momo_AccessKey code is required',
      })
      .default(''),
   momo_RequestId: z
      .string()
      .min(1, {
         message: 'momo_RequestId code is required',
      })
      .default(''),
   momo_Amount: z
      .string()
      .min(1, {
         message: 'momo_Amount code is required',
      })
      .default(''),
   momo_OrderId: z
      .string()
      .min(1, {
         message: 'momo_OrderId code is required',
      })
      .default(''),
   momo_OrderInfo: z
      .string()
      .min(1, {
         message: 'momo_OrderInfo code is required',
      })
      .default(''),
   momo_OrderType: z
      .string()
      .min(1, {
         message: 'momo_OrderType code is required',
      })
      .default('momo_wallet'),
   momo_TransId: z
      .string()
      .min(1, {
         message: 'momo_TransId code is required',
      })
      .default(''),
   momo_Message: z
      .string()
      .min(1, {
         message: 'momo_Message code is required',
      })
      .default('Success'),
   momo_LocalMessage: z
      .string()
      .min(1, {
         message: 'momo_LocalMessage code is required',
      })
      .default('Thành công'),
   momo_ResponseTime: z
      .string()
      .min(1, {
         message: 'momo_ResponseTime code is required',
      })
      .default(''),
   momo_ErrorCode: z
      .string()
      .min(1, {
         message: 'momo_ErrorCode code is required',
      })
      .default('0'),
   momo_PayType: z
      .string()
      .min(1, {
         message: 'momo_PayType code is required',
      })
      .default('web'),
   momo_ExtraData: z.string().default(''),
   momo_Signature: z
      .string()
      .min(1, {
         message: 'momo_Signature code is required',
      })
      .default(''),
} satisfies Record<keyof IMomoIpnCallbackPayload, any> & {});

export type MomoIpnFormType = z.infer<typeof momoIpnSchema>;
export const MomoIpnResolver = zodResolver(momoIpnSchema);
