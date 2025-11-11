import { createApi } from '@reduxjs/toolkit/query/react';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';
import { setLogout } from '../redux/features/auth.slice';
import { baseQuery } from './base-query';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('ordering-services')(args, api, extraOptions);

   if (result.error && result.error.status === 401) {
      api.dispatch(setLogout());
   }

   return result;
};

const fakeOrdersData = {
   total_records: 2,
   total_pages: 1,
   page_size: 10,
   current_page: 1,
   items: [
      {
         tenant_id: null,
         branch_id: null,
         order_id: '0a573050-acb5-4e25-8623-776615333d96',
         customer_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
         customer_email: 'lov3rinve146@gmail.com',
         order_code: '#817928',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Foo Bar',
            contact_email: 'lov3rinve146@gmail.com',
            contact_phone_number: '0333284890',
            contact_address_line: '123 Street',
            contact_district: 'Thu Duc',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'f3ad50aa-50aa-45ca-93f6-583d3fe7adc7',
               order_id: '0a573050-acb5-4e25-8623-776615333d96',
               tenant_id: null,
               branch_id: null,
               sku_id: '690f4605e2295b9f94f23f87',
               model_id: '664351e90087aa09993f5ae7',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: '',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1000.0,
               created_at: '2025-11-09T15:29:16.633805Z',
               updated_at: '2025-11-09T15:29:16.633805Z',
               updated_by: null,
               is_deleted: false,
               deleted_at: null,
               deleted_by: null,
            },
         ],
         promotion_id: null,
         promotion_type: null,
         discount_type: null,
         discount_value: null,
         discount_amount: null,
         total_amount: 1000,
         created_at: '2025-11-09T15:29:16.632472Z',
         updated_at: '2025-11-09T15:29:16.632472Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'f4648f66-74d6-455d-80f9-4cf17c9a061c',
         customer_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
         customer_email: 'lov3rinve146@gmail.com',
         order_code: '#418511',
         status: 'DELIVERED',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'test',
            contact_email: 'lov3rinve146@gmail.com',
            contact_phone_number: 'test',
            contact_address_line: 'test',
            contact_district: 'test',
            contact_province: 'test',
            contact_country: 'test',
         },
         order_items: [
            {
               order_item_id: '091dee06-ae14-4ed6-b9cf-660b64661f9a',
               order_id: 'f4648f66-74d6-455d-80f9-4cf17c9a061c',
               tenant_id: null,
               branch_id: null,
               sku_id: '690f4601e2295b9f94f23f5f',
               model_id: '664351e90087aa09993f5ae7',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: '',
               quantity: 1,
               is_reviewed: true,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1000.0,
               created_at: '2025-11-09T18:03:13.928924Z',
               updated_at: '2025-11-09T18:03:13.928924Z',
               updated_by: null,
               is_deleted: false,
               deleted_at: null,
               deleted_by: null,
            },
         ],
         promotion_id: null,
         promotion_type: null,
         discount_type: null,
         discount_value: null,
         discount_amount: null,
         total_amount: 1000,
         created_at: '2025-11-09T18:03:13.928123Z',
         updated_at: '2025-11-09T18:03:13.928123Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_page=1&_limit=10',
      prev: null,
      next: null,
      last: '?_page=1&_limit=10',
   },
};

const fakeRevenuesData = [
   {
      tenant_id: '690e034dff79797b05b3bc89',
      branch_id: null,
      order_id: '0a573050-acb5-4e25-8623-776615333d96',
      customer_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
      customer_email: 'lov3rinve146@gmail.com',
      order_code: '#817928',
      status: 'PAID',
      payment_method: 'VNPAY',
      shipping_address: {
         contact_name: 'Foo Bar',
         contact_email: 'lov3rinve146@gmail.com',
         contact_phone_number: '0333284890',
         contact_address_line: '123 Street',
         contact_district: 'Thu Duc',
         contact_province: 'Ho Chi Minh',
         contact_country: 'Vietnam',
      },
      order_items: [
         {
            order_item_id: 'f3ad50aa-50aa-45ca-93f6-583d3fe7adc7',
            order_id: '0a573050-acb5-4e25-8623-776615333d96',
            tenant_id: null,
            branch_id: null,
            sku_id: '690f4605e2295b9f94f23f87',
            model_id: '664351e90087aa09993f5ae7',
            model_name: 'IPHONE_15',
            color_name: 'BLUE',
            storage_name: '128GB',
            unit_price: 1000,
            display_image_url:
               'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
            model_slug: '',
            quantity: 1,
            is_reviewed: false,
            promotion_id: null,
            promotion_type: null,
            discount_type: null,
            discount_value: null,
            discount_amount: null,
            sub_total_amount: 1000,
            created_at: '2025-11-09T15:29:16.633805Z',
            updated_at: '2025-11-09T15:29:16.633805Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
      ],
      promotion_id: null,
      promotion_type: null,
      discount_type: null,
      discount_value: null,
      discount_amount: null,
      total_amount: 1000,
      created_at: '2025-11-09T15:29:16.632472Z',
      updated_at: '2025-11-09T15:29:16.632472Z',
      updated_by: null,
      is_deleted: false,
      deleted_at: null,
      deleted_by: null,
   },
   {
      tenant_id: '690e034dff79797b05b3bc89',
      branch_id: null,
      order_id: 'f4648f66-74d6-455d-80f9-4cf17c9a061c',
      customer_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
      customer_email: 'lov3rinve146@gmail.com',
      order_code: '#418511',
      status: 'DELIVERED',
      payment_method: 'VNPAY',
      shipping_address: {
         contact_name: 'test',
         contact_email: 'lov3rinve146@gmail.com',
         contact_phone_number: 'test',
         contact_address_line: 'test',
         contact_district: 'test',
         contact_province: 'test',
         contact_country: 'test',
      },
      order_items: [
         {
            order_item_id: '091dee06-ae14-4ed6-b9cf-660b64661f9a',
            order_id: 'f4648f66-74d6-455d-80f9-4cf17c9a061c',
            tenant_id: null,
            branch_id: null,
            sku_id: '690f4601e2295b9f94f23f5f',
            model_id: '664351e90087aa09993f5ae7',
            model_name: 'IPHONE_15',
            color_name: 'BLUE',
            storage_name: '128GB',
            unit_price: 1000,
            display_image_url:
               'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
            model_slug: '',
            quantity: 1,
            is_reviewed: true,
            promotion_id: null,
            promotion_type: null,
            discount_type: null,
            discount_value: null,
            discount_amount: null,
            sub_total_amount: 1000,
            created_at: '2025-11-09T18:03:13.928924Z',
            updated_at: '2025-11-09T18:03:13.928924Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
      ],
      promotion_id: null,
      promotion_type: null,
      discount_type: null,
      discount_value: null,
      discount_amount: null,
      total_amount: 1000,
      created_at: '2025-11-09T18:03:13.928123Z',
      updated_at: '2025-11-09T18:03:13.928123Z',
      updated_by: null,
      is_deleted: false,
      deleted_at: null,
      deleted_by: null,
   },
];

const fakeRevenuesByYearsData = {
   groups: {
      '2024': [],
      '2025': [
         {
            tenant_id: '690e034dff79797b05b3bc89',
            branch_id: null,
            order_id: '0a573050-acb5-4e25-8623-776615333d96',
            customer_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
            customer_email: 'lov3rinve146@gmail.com',
            order_code: '#817928',
            status: 'PAID',
            payment_method: 'VNPAY',
            shipping_address: {
               contact_name: 'Foo Bar',
               contact_email: 'lov3rinve146@gmail.com',
               contact_phone_number: '0333284890',
               contact_address_line: '123 Street',
               contact_district: 'Thu Duc',
               contact_province: 'Ho Chi Minh',
               contact_country: 'Vietnam',
            },
            order_items: [
               {
                  order_item_id: 'f3ad50aa-50aa-45ca-93f6-583d3fe7adc7',
                  order_id: '0a573050-acb5-4e25-8623-776615333d96',
                  tenant_id: null,
                  branch_id: null,
                  sku_id: '690f4605e2295b9f94f23f87',
                  model_id: '664351e90087aa09993f5ae7',
                  model_name: 'IPHONE_15',
                  color_name: 'BLUE',
                  storage_name: '128GB',
                  unit_price: 1000,
                  display_image_url:
                     'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
                  model_slug: '',
                  quantity: 1,
                  is_reviewed: false,
                  promotion_id: null,
                  promotion_type: null,
                  discount_type: null,
                  discount_value: null,
                  discount_amount: null,
                  sub_total_amount: 1000,
                  created_at: '2025-11-09T15:29:16.633805Z',
                  updated_at: '2025-11-09T15:29:16.633805Z',
                  updated_by: null,
                  is_deleted: false,
                  deleted_at: null,
                  deleted_by: null,
               },
            ],
            promotion_id: null,
            promotion_type: null,
            discount_type: null,
            discount_value: null,
            discount_amount: null,
            total_amount: 1000,
            created_at: '2025-11-09T15:29:16.632472Z',
            updated_at: '2025-11-09T15:29:16.632472Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         {
            tenant_id: '690e034dff79797b05b3bc89',
            branch_id: null,
            order_id: 'f4648f66-74d6-455d-80f9-4cf17c9a061c',
            customer_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
            customer_email: 'lov3rinve146@gmail.com',
            order_code: '#418511',
            status: 'DELIVERED',
            payment_method: 'VNPAY',
            shipping_address: {
               contact_name: 'test',
               contact_email: 'lov3rinve146@gmail.com',
               contact_phone_number: 'test',
               contact_address_line: 'test',
               contact_district: 'test',
               contact_province: 'test',
               contact_country: 'test',
            },
            order_items: [
               {
                  order_item_id: '091dee06-ae14-4ed6-b9cf-660b64661f9a',
                  order_id: 'f4648f66-74d6-455d-80f9-4cf17c9a061c',
                  tenant_id: null,
                  branch_id: null,
                  sku_id: '690f4601e2295b9f94f23f5f',
                  model_id: '664351e90087aa09993f5ae7',
                  model_name: 'IPHONE_15',
                  color_name: 'BLUE',
                  storage_name: '128GB',
                  unit_price: 1000,
                  display_image_url:
                     'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
                  model_slug: '',
                  quantity: 1,
                  is_reviewed: true,
                  promotion_id: null,
                  promotion_type: null,
                  discount_type: null,
                  discount_value: null,
                  discount_amount: null,
                  sub_total_amount: 1000,
                  created_at: '2025-11-09T18:03:13.928924Z',
                  updated_at: '2025-11-09T18:03:13.928924Z',
                  updated_by: null,
                  is_deleted: false,
                  deleted_at: null,
                  deleted_by: null,
               },
            ],
            promotion_id: null,
            promotion_type: null,
            discount_type: null,
            discount_value: null,
            discount_amount: null,
            total_amount: 1000,
            created_at: '2025-11-09T18:03:13.928123Z',
            updated_at: '2025-11-09T18:03:13.928123Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
      ],
   },
};

const fakeRevenuesByTenantsData = {
   groups: {
      '690e034dff79797b05b3bc89': [
         {
            tenant_id: '690e034dff79797b05b3bc89',
            branch_id: null,
            order_id: '0a573050-acb5-4e25-8623-776615333d96',
            customer_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
            customer_email: 'lov3rinve146@gmail.com',
            order_code: '#817928',
            status: 'PAID',
            payment_method: 'VNPAY',
            shipping_address: {
               contact_name: 'Foo Bar',
               contact_email: 'lov3rinve146@gmail.com',
               contact_phone_number: '0333284890',
               contact_address_line: '123 Street',
               contact_district: 'Thu Duc',
               contact_province: 'Ho Chi Minh',
               contact_country: 'Vietnam',
            },
            order_items: [
               {
                  order_item_id: 'f3ad50aa-50aa-45ca-93f6-583d3fe7adc7',
                  order_id: '0a573050-acb5-4e25-8623-776615333d96',
                  tenant_id: null,
                  branch_id: null,
                  sku_id: '690f4605e2295b9f94f23f87',
                  model_id: '664351e90087aa09993f5ae7',
                  model_name: 'IPHONE_15',
                  color_name: 'BLUE',
                  storage_name: '128GB',
                  unit_price: 1000,
                  display_image_url:
                     'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
                  model_slug: '',
                  quantity: 1,
                  is_reviewed: false,
                  promotion_id: null,
                  promotion_type: null,
                  discount_type: null,
                  discount_value: null,
                  discount_amount: null,
                  sub_total_amount: 1000,
                  created_at: '2025-11-09T15:29:16.633805Z',
                  updated_at: '2025-11-09T15:29:16.633805Z',
                  updated_by: null,
                  is_deleted: false,
                  deleted_at: null,
                  deleted_by: null,
               },
            ],
            promotion_id: null,
            promotion_type: null,
            discount_type: null,
            discount_value: null,
            discount_amount: null,
            total_amount: 1000,
            created_at: '2025-11-09T15:29:16.632472Z',
            updated_at: '2025-11-09T15:29:16.632472Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         {
            tenant_id: '690e034dff79797b05b3bc89',
            branch_id: null,
            order_id: 'f4648f66-74d6-455d-80f9-4cf17c9a061c',
            customer_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
            customer_email: 'lov3rinve146@gmail.com',
            order_code: '#418511',
            status: 'DELIVERED',
            payment_method: 'VNPAY',
            shipping_address: {
               contact_name: 'test',
               contact_email: 'lov3rinve146@gmail.com',
               contact_phone_number: 'test',
               contact_address_line: 'test',
               contact_district: 'test',
               contact_province: 'test',
               contact_country: 'test',
            },
            order_items: [
               {
                  order_item_id: '091dee06-ae14-4ed6-b9cf-660b64661f9a',
                  order_id: 'f4648f66-74d6-455d-80f9-4cf17c9a061c',
                  tenant_id: null,
                  branch_id: null,
                  sku_id: '690f4601e2295b9f94f23f5f',
                  model_id: '664351e90087aa09993f5ae7',
                  model_name: 'IPHONE_15',
                  color_name: 'BLUE',
                  storage_name: '128GB',
                  unit_price: 1000,
                  display_image_url:
                     'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
                  model_slug: '',
                  quantity: 1,
                  is_reviewed: true,
                  promotion_id: null,
                  promotion_type: null,
                  discount_type: null,
                  discount_value: null,
                  discount_amount: null,
                  sub_total_amount: 1000,
                  created_at: '2025-11-09T18:03:13.928924Z',
                  updated_at: '2025-11-09T18:03:13.928924Z',
                  updated_by: null,
                  is_deleted: false,
                  deleted_at: null,
                  deleted_by: null,
               },
            ],
            promotion_id: null,
            promotion_type: null,
            discount_type: null,
            discount_value: null,
            discount_amount: null,
            total_amount: 1000,
            created_at: '2025-11-09T18:03:13.928123Z',
            updated_at: '2025-11-09T18:03:13.928123Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
      ],
   },
};

export type TShippingAddress = {
   contact_name: string;
   contact_email: string;
   contact_phone_number: string;
   contact_address_line: string;
   contact_district: string;
   contact_province: string;
   contact_country: string;
};

export type TOrderItem = {
   order_item_id: string;
   order_id: string;
   tenant_id: string | null;
   branch_id: string | null;
   sku_id: string | null;
   model_id: string;
   model_name: string;
   color_name: string;
   storage_name: string;
   unit_price: number;
   display_image_url: string;
   model_slug: string;
   quantity: number;
   is_reviewed: boolean;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TOrder = {
   tenant_id: string | null;
   branch_id: string | null;
   order_id: string;
   customer_id: string;
   customer_email: string;
   order_code: string;
   status: string;
   payment_method: string;
   shipping_address: TShippingAddress;
   order_items: TOrderItem[];
   promotion_id: null;
   promotion_type: string | null;
   discount_type: string | null;
   discount_value: number | null;
   discount_amount: number | null;
   total_amount: number;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TBaseQueryParams = {
   _page?: number;
   _limit?: number;
};

export const orderingApi = createApi({
   reducerPath: 'order-api',
   tagTypes: ['Orders'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getOrdersByAdmin: builder.query<
         PaginationResponse<TOrder>,
         TBaseQueryParams
      >({
         query: (params: TBaseQueryParams) => ({
            url: '/api/v1/orders/admin',
            method: 'GET',
            params: {
               _page: params._page ?? 1,
               _limit: params._limit ?? 10,
               ...params,
            },
         }),
      }),
      getOrderDetails: builder.query<any, string>({
         query: (orderId: string) => ({
            url: `/api/v1/orders/${orderId}`,
            method: 'GET',
         }),
      }),
      getRevenues: builder.query<TOrder[], void>({
         query: () => ({
            url: `/api/v1/orders/dashboard/revenues`,
            method: 'GET',
         }),
      }),
      getRevenuesByYears: builder.query<
         { groups: Record<string, TOrder[]> },
         { _years: string[] }
      >({
         query: (params) => ({
            url: `/api/v1/orders/dashboard/revenues/years`,
            method: 'GET',
            params: {
               _years: params?._years || [],
            },
         }),
      }),
      getRevenuesByTenants: builder.query<
         { groups: Record<string, TOrder[]> },
         { _tenants: string[] }
      >({
         query: (params) => {
            const tenantIds = (params?._tenants || []).filter(
               (id): id is string => Boolean(id) && id !== 'undefined',
            );
            return {
               url: `/api/v1/orders/dashboard/revenues/tenants`,
               method: 'GET',
               params: {
                  _tenants: tenantIds,
               },
            };
         },
      }),
   }),
});

export const {
   useLazyGetOrderDetailsQuery,
   useLazyGetOrdersByAdminQuery,
   useLazyGetRevenuesQuery,
   useLazyGetRevenuesByYearsQuery,
   useLazyGetRevenuesByTenantsQuery,
} = orderingApi;
