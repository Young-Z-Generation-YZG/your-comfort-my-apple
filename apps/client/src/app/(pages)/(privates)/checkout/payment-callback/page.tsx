'use client';

import { useEffect, useRef, useState } from 'react';
import { useSearchParams } from 'next/navigation';
import {
   MomoIpnFormType,
   MomoIpnResolver,
   VnpayIpnFormType,
   VnpayIpnResolver,
} from '~/domain/schemas/order.schema';
import { LoadingOverlay } from '@components/client/loading-overlay';
import SuccessResult from './_components/success-result';
import FailureResult from './_components/failure-result';
import { useForm } from 'react-hook-form';
import useOrderingService from '@components/hooks/api/use-ordering-service';
import { useDispatch } from 'react-redux';
import { CleanSelectedItems } from '~/infrastructure/redux/features/cart.slice';

const fakeData = {
   tenant_id: null,
   branch_id: null,
   order_id: 'a6c9fbce-ab37-4472-b8d6-a3684b9d8241',
   customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
   customer_email: 'user@gmail.com',
   order_code: '#750653',
   status: 'PAID',
   payment_method: 'VNPAY',
   shipping_address: {
      contact_name: 'Foo Bar',
      contact_email: 'user@gmail.com',
      contact_phone_number: '0333284890',
      contact_address_line: '123 Street',
      contact_district: 'Thu Duc',
      contact_province: 'Ho Chi Minh',
      contact_country: 'Việt Nam',
   },
   order_items: [
      {
         order_id: 'a6c9fbce-ab37-4472-b8d6-a3684b9d8241',
         order_item_id: '0c9d7078-2949-43e7-a4af-6acf3dd8453e',
         sku_id: null,
         model_id: 'ModelId',
         model_name: 'IPHONE_15',
         color_name: 'BLUE',
         storage_name: '256GB',
         unit_price: 1100,
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         model_slug: '',
         quantity: 1,
         promotion: {
            promotion_id: '99a356c8-c026-4137-8820-394763f30521',
            promotion_type: 'EVENT',
            discount_type: 'PERCENTAGE',
            discount_value: 0.1,
            discount_amount: 110,
            final_price: 990,
         },
         is_reviewed: false,
         created_at: '2025-10-27T18:07:03.60679Z',
         updated_at: '2025-10-27T18:07:03.60679Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   promotion_id: '99a356c8-c026-4137-8820-394763f30521',
   promotion_type: 'EVENT',
   discount_type: 'PERCENTAGE',
   discount_value: 0.1,
   discount_amount: 110,
   total_amount: 990,
   created_at: '2025-10-27T18:07:03.606782Z',
   updated_at: '2025-10-27T18:07:03.606782Z',
   updated_by: null,
   is_deleted: false,
   deleted_at: null,
   deleted_by: null,
};

export type TOrder = typeof fakeData;

const PaymentCallbackPage = () => {
   const searchParams = useSearchParams();
   const [order, setOrder] = useState<TOrder | null>(null);
   const hasCalledApi = useRef(false);

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

   const {
      vnpayIpnCallbackAsync,
      momoIpnCallbackAsync,
      vnpayIpnCallbackState,
      momoIpnCallbackState,
      isLoading,
   } = useOrderingService();

   const dispatch = useDispatch();

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

   const handleVnpayIpnCallback = async (data: VnpayIpnFormType) => {
      console.log('VNPAY IPN Callback Data:', data);

      const isValid = await vnpayForm.trigger();

      if (isValid) {
         const result = await vnpayIpnCallbackAsync({
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

         if (result.isSuccess) {
            setOrder(result.data as unknown as TOrder);
         }
      }
   };

   const handleMomoIpnCallback = async (data: MomoIpnFormType) => {
      console.log('MOMO IPN Callback Data:', data);

      const isValid = await momoForm.trigger();

      if (isValid) {
         const result = await momoIpnCallbackAsync({
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

         if (result.isSuccess) {
            setOrder(result.data as unknown as TOrder);
         }
      }
   };

   useEffect(() => {
      if (hasCalledApi.current) {
         console.log('API call skipped: Already called');
         return;
      }

      handleVnpayIpnCallback(vnpayForm.getValues());
      handleMomoIpnCallback(momoForm.getValues());

      hasCalledApi.current = true;
   }, []);

   useEffect(() => {
      dispatch(CleanSelectedItems());
   }, [dispatch]);

   return (
      <div className="min-h-screen bg-gray-50 text-gray-900 font-sans">
         <LoadingOverlay
            isLoading={isLoading}
            fullScreen={true}
            text="Processing your payment..."
         />

         {(vnpayIpnCallbackState.isSuccess || momoIpnCallbackState.isSuccess) &&
            order && <SuccessResult order={order} />}

         {!isLoading &&
            (vnpayIpnCallbackState.isError || momoIpnCallbackState.isError) &&
            !order && <FailureResult />}
      </div>
   );
};

export default PaymentCallbackPage;
