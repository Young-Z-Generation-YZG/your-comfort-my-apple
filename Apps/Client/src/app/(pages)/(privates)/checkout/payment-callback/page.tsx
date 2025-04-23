'use client';

import { useEffect, useState } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import {
   useMomoIpnCallbackAsyncMutation,
   useVnpayIpnCallbackAsyncMutation,
} from '~/infrastructure/services/order.service';
import {
   MomoIpnFormType,
   MomoIpnResolver,
   VnpayIpnFormType,
   VnpayIpnResolver,
} from '~/domain/schemas/order.schema';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { OrderDetailsResponse } from '~/domain/interfaces/orders/order.interface';
import SuccessResult from './_components/success-result';
import FailureResult from './_components/failure-result';
import { useForm } from 'react-hook-form';

const PaymentCallbackPage = () => {
   const searchParams = useSearchParams();
   const router = useRouter();
   const [loading, setLoading] = useState(false);
   const [showRedirect, setShowRedirect] = useState(false);
   const [order, setOrder] = useState<OrderDetailsResponse | null>(null);

   const amount = searchParams.get('vnp_Amount');
   const bankCode = searchParams.get('vnp_BankCode');
   const bankTranNo = searchParams.get('vnp_BankTranNo');
   const cardType = searchParams.get('vnp_CardType');
   const orderInfo = searchParams.get('vnp_OrderInfo');
   const payDate = searchParams.get('vnp_PayDate');
   const responseCode = searchParams.get('vnp_ResponseCode');
   const tmnCode = searchParams.get('vnp_TmnCode');
   const transactionNo = searchParams.get('vnp_TransactionNo');
   const transactionStatus = searchParams.get('vnp_TransactionStatus');
   const txnRef = searchParams.get('vnp_TxnRef');
   const secureHash = searchParams.get('vnp_SecureHash');

   const momoPartnerCode = searchParams.get('partnerCode');
   const momoAccessKey = searchParams.get('accessKey');
   const momoRequestId = searchParams.get('requestId');
   const momoAmount = searchParams.get('amount');
   const momoOrderId = searchParams.get('orderId');
   const momoOrderInfo = searchParams.get('orderInfo');
   const momoOrderType = searchParams.get('orderType');
   const momoTransId = searchParams.get('transId');
   const momoMessage = searchParams.get('message');
   const momoLocalMessage = searchParams.get('localMessage');
   const momoResponseTime = searchParams.get('responseTime');
   const momoErrorCode = searchParams.get('errorCode');
   const momoPayType = searchParams.get('payType');
   const momoExtraData = searchParams.get('extraData');
   const momoSignature = searchParams.get('signature');

   const [
      ipnCallbackAsyncVnpay,
      { isLoading, isSuccess: isSuccessVnpay, isError, error, data },
   ] = useVnpayIpnCallbackAsyncMutation();

   const [
      ipnCallbackAsyncMomo,
      {
         isLoading: loadingMomo,
         isSuccess: isSuccessMomo,
         isError: isErrorMomo,
         error: errorMomo,
         data: dataMomo,
      },
   ] = useMomoIpnCallbackAsyncMutation();

   const vnpayForm = useForm<VnpayIpnFormType>({
      resolver: VnpayIpnResolver,
      defaultValues: {
         vnp_Amount: amount || '000',
         vnp_BankCode: bankCode || '',
         vnp_BankTranNo: bankTranNo || '',
         vnp_CardType: cardType || '',
         vnp_OrderInfo: orderInfo || '',
         vnp_PayDate: payDate || '',
         vnp_ResponseCode: responseCode || '',
         vnp_TmnCode: tmnCode || '',
         vnp_TransactionNo: transactionNo || '',
         vnp_TransactionStatus: transactionStatus || '',
         vnp_TxnRef: txnRef || '',
         vnp_SecureHash: secureHash || '',
      },
   });

   const momoForm = useForm<MomoIpnFormType>({
      resolver: MomoIpnResolver,
      defaultValues: {
         momo_PartnerCode: momoPartnerCode || 'MOMO',
         momo_AccessKey: momoAccessKey || '',
         momo_RequestId: momoRequestId || '',
         momo_Amount: momoAmount || '0',
         momo_OrderId: momoOrderId || '',
         momo_OrderInfo: momoOrderInfo || '',
         momo_OrderType: momoOrderType || 'momo_wallet',
         momo_TransId: momoTransId || '',
         momo_Message: momoMessage || '',
         momo_LocalMessage: momoLocalMessage || '',
         momo_ResponseTime: momoResponseTime || '',
         momo_ErrorCode: momoErrorCode || '0',
         momo_PayType: momoPayType || 'web',
         momo_ExtraData: momoExtraData || '',
         momo_Signature: momoSignature || '',
      },
   });

   // Handle redirect completion
   const handleRedirectComplete = () => {
      router.push('/');
   };

   // Handle redirect cancellation
   const handleRedirectCancel = () => {
      setShowRedirect(false);
   };

   const handleVnpayIpnCallback = async (data: VnpayIpnFormType) => {
      console.log('VNPAY IPN Callback Data:', data);
      const isValid = await vnpayForm.trigger();

      if (isValid) {
         const result = await ipnCallbackAsyncVnpay({
            vnp_Amount: amount || '00',
            vnp_BankCode: bankCode || '',
            vnp_BankTranNo: bankTranNo || '',
            vnp_CardType: cardType || '',
            vnp_OrderInfo: orderInfo || '',
            vnp_PayDate: payDate || '',
            vnp_ResponseCode: responseCode || '',
            vnp_TmnCode: tmnCode || '',
            vnp_TransactionNo: transactionNo || '',
            vnp_TransactionStatus: transactionStatus || '',
            vnp_TxnRef: txnRef || '',
            vnp_SecureHash: secureHash || '',
         });
         if (result?.data) {
            setOrder(result.data);
            setLoading(false);
         }
      }
   };

   const handleMomoIpnCallback = async (data: MomoIpnFormType) => {
      console.log('MOMO IPN Callback Data:', data);

      const isValid = await momoForm.trigger();

      if (isValid) {
         const result = await ipnCallbackAsyncMomo({
            momo_PartnerCode: momoPartnerCode || 'MOMO',
            momo_AccessKey: momoAccessKey || '',
            momo_RequestId: momoRequestId || '',
            momo_Amount: momoAmount || '0',
            momo_OrderId: momoOrderId || '',
            momo_OrderInfo: momoOrderInfo || '',
            momo_OrderType: momoOrderType || 'momo_wallet',
            momo_TransId: momoTransId || '',
            momo_Message: momoMessage || '',
            momo_LocalMessage: momoLocalMessage || '',
            momo_ResponseTime: momoResponseTime || '',
            momo_ErrorCode: momoErrorCode || '0',
            momo_PayType: momoPayType || 'web',
            momo_ExtraData: momoExtraData || '',
            momo_Signature: momoSignature || '',
         });

         if (result?.data) {
            setOrder(result.data);
            setLoading(false);
         }
      }
   };

   useEffect(() => {
      // handleVnpayIpnCallback(vnpayForm.getValues());
      handleMomoIpnCallback(momoForm.getValues());
   }, []);

   return (
      <div className="min-h-screen bg-gray-50 text-gray-900 font-sans">
         <LoadingOverlay
            isLoading={isLoading || loading}
            fullScreen={true}
            text="Processing your payment..."
         />

         {isSuccessVnpay || isSuccessMomo ? (
            <SuccessResult order={order} />
         ) : (
            <FailureResult />
         )}
      </div>
   );
};

export default PaymentCallbackPage;
