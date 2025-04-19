export interface IVnpayIpnCallbackPayload {
   vnp_Amount: string;
   vnp_BankCode: string;
   vnp_BankTranNo: string;
   vnp_CardType: string;
   vnp_OrderInfo: string;
   vnp_PayDate: string;
   vnp_ResponseCode: string;
   vnp_TmnCode: string;
   vnp_TransactionNo: string;
   vnp_TransactionStatus: string;
   vnp_TxnRef: string;
   vnp_SecureHash: string;
}

export interface IMomoIpnCallbackPayload {
   momo_PartnerCode: string;
   momo_AccessKey: string;
   momo_RequestId: string;
   momo_Amount: string;
   momo_OrderId: string;
   momo_OrderInfo: string;
   momo_OrderType: string;
   momo_TransId: string;
   momo_Message: string;
   momo_LocalMessage: string;
   momo_ResponseTime: string;
   momo_ErrorCode: string;
   momo_PayType: string;
   momo_ExtraData: string;
   momo_Signature: string;
}
