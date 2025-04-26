export const orderData = [
   {
      order_id: '3de0c2e6-d082-4d24-b84e-805905674b09',
      order_code: '#64500665',
      order_customer_email: 'd7610ca1-2909-49d3-af23-d502a297da29',
      order_status: 'PAID',
      order_payment_method: 'VNPAY',
      order_shipping_address: {
         contact_name: 'Bach Le',
         contact_email: 'lov3rinve146@gmail.com',
         contact_phone_number: '0333284890',
         contact_address_line: 'Viet Nam',
         contact_district: 'Thu Duc',
         contact_province: 'Ho Chi Minh',
         contact_country: 'Viet Nam',
      },
      order_items_count: 1,
      order_sub_total_amount: 799,
      order_discount_amount: 159.8,
      order_total_amount: 639.2,
      order_created_at: '2025-04-17T06:35:56.636992Z',
      order_updated_at: '2025-04-17T06:35:56.636992Z',
      order_last_modified_by: null,
   },
];

type Order = {
   id: string;
   orderNumber: string;
   date: string;
   status: 'processing' | 'shipped' | 'delivered' | 'canceled';
   total: string;
   items: number;
};

const sampleOrders: Order[] = [
   {
      id: '1',
      orderNumber: 'W12345678',
      date: 'Apr 12, 2023',
      status: 'delivered',
      total: '$2,399.00',
      items: 2,
   },
   {
      id: '2',
      orderNumber: 'W12345679',
      date: 'Mar 28, 2023',
      status: 'shipped',
      total: '$129.00',
      items: 1,
   },
   {
      id: '3',
      orderNumber: 'W12345680',
      date: 'Feb 15, 2023',
      status: 'delivered',
      total: '$1,299.00',
      items: 1,
   },
   {
      id: '4',
      orderNumber: 'W12345681',
      date: 'Jan 7, 2023',
      status: 'delivered',
      total: '$59.00',
      items: 3,
   },
   {
      id: '5',
      orderNumber: 'W12345682',
      date: 'Dec 24, 2022',
      status: 'delivered',
      total: '$249.00',
      items: 2,
   },
   {
      id: '6',
      orderNumber: 'W12345683',
      date: 'Nov 18, 2022',
      status: 'delivered',
      total: '$19.99',
      items: 1,
   },
   {
      id: '7',
      orderNumber: 'W12345684',
      date: 'Oct 5, 2022',
      status: 'delivered',
      total: '$549.00',
      items: 1,
   },
   {
      id: '8',
      orderNumber: 'W12345685',
      date: 'Apr 15, 2023',
      status: 'processing',
      total: '$1,599.00',
      items: 1,
   },
   {
      id: '9',
      orderNumber: 'W12345686',
      date: 'Apr 10, 2023',
      status: 'canceled',
      total: '$299.00',
      items: 2,
   },
];
