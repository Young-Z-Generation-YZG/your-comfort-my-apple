'use client';

import { ChevronDown, TrendingUp } from 'lucide-react';
import {
   CartesianGrid,
   LabelList,
   Line,
   LineChart,
   XAxis,
   YAxis,
} from 'recharts';

import {
   Card,
   CardContent,
   CardDescription,
   CardFooter,
   CardHeader,
   CardTitle,
} from '@components/ui/card';
import {
   ChartConfig,
   ChartContainer,
   ChartTooltip,
   ChartTooltipContent,
} from '@components/ui/chart';

import {
   DropdownMenu,
   DropdownMenuCheckboxItem,
   DropdownMenuContent,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import { isWithinInterval, startOfMonth } from 'date-fns';
import { useState, useMemo } from 'react';

import { ChevronDownIcon } from 'lucide-react';
import { type DateRange } from 'react-day-picker';

import { Button } from '~/src/components/ui/button';
import { Calendar } from '~/src/components/ui/calendar';
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '~/src/components/ui/popover';

export const description = 'A line chart with a label';

const fakeData = {
   total_records: 30,
   total_pages: 6,
   page_size: 5,
   current_page: 1,
   items: [
      // January orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'jan-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer1@gmail.com',
         order_code: '#100001',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Alice Johnson',
            contact_email: 'customer1@gmail.com',
            contact_phone_number: '0333111111',
            contact_address_line: '111 Main St',
            contact_district: 'District 1',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'jan-item-001',
               order_id: 'jan-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 2000.0,
               created_at: '2025-01-10T10:00:00.000000Z',
               updated_at: '2025-01-10T10:00:00.000000Z',
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
         total_amount: 2000,
         created_at: '2025-01-10T10:00:00.000000Z',
         updated_at: '2025-01-10T10:00:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'jan-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer2@gmail.com',
         order_code: '#100002',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'Bob Smith',
            contact_email: 'customer2@gmail.com',
            contact_phone_number: '0333222222',
            contact_address_line: '222 Second Ave',
            contact_district: 'District 2',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'jan-item-002',
               order_id: 'jan-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLACK',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1100.0,
               created_at: '2025-01-20T14:30:00.000000Z',
               updated_at: '2025-01-20T14:30:00.000000Z',
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
         total_amount: 1100,
         created_at: '2025-01-20T14:30:00.000000Z',
         updated_at: '2025-01-20T14:30:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'jan-003',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer3@gmail.com',
         order_code: '#100003',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Charlie Brown',
            contact_email: 'customer3@gmail.com',
            contact_phone_number: '0333333333',
            contact_address_line: '333 Third Blvd',
            contact_district: 'District 3',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'jan-item-003',
               order_id: 'jan-003',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'PINK',
               storage_name: '512GB',
               unit_price: 1500.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1500.0,
               created_at: '2025-01-25T16:45:00.000000Z',
               updated_at: '2025-01-25T16:45:00.000000Z',
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
         total_amount: 1500,
         created_at: '2025-01-25T16:45:00.000000Z',
         updated_at: '2025-01-25T16:45:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // February orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'feb-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer4@gmail.com',
         order_code: '#200001',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Diana Prince',
            contact_email: 'customer4@gmail.com',
            contact_phone_number: '0333444444',
            contact_address_line: '444 Fourth St',
            contact_district: 'District 4',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'feb-item-001',
               order_id: 'feb-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'WHITE',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 3,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 3000.0,
               created_at: '2025-02-08T09:15:00.000000Z',
               updated_at: '2025-02-08T09:15:00.000000Z',
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
         total_amount: 3000,
         created_at: '2025-02-08T09:15:00.000000Z',
         updated_at: '2025-02-08T09:15:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'feb-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer5@gmail.com',
         order_code: '#200002',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'Eve Adams',
            contact_email: 'customer5@gmail.com',
            contact_phone_number: '0333555555',
            contact_address_line: '555 Fifth Ave',
            contact_district: 'District 5',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'feb-item-002',
               order_id: 'feb-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'GREEN',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 2200.0,
               created_at: '2025-02-18T13:20:00.000000Z',
               updated_at: '2025-02-18T13:20:00.000000Z',
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
         total_amount: 2200,
         created_at: '2025-02-18T13:20:00.000000Z',
         updated_at: '2025-02-18T13:20:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // March orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'mar-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer6@gmail.com',
         order_code: '#300001',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Frank Miller',
            contact_email: 'customer6@gmail.com',
            contact_phone_number: '0333666666',
            contact_address_line: '666 Sixth Rd',
            contact_district: 'District 6',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'mar-item-001',
               order_id: 'mar-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 2200.0,
               created_at: '2025-03-05T11:00:00.000000Z',
               updated_at: '2025-03-05T11:00:00.000000Z',
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
         total_amount: 2200,
         created_at: '2025-03-05T11:00:00.000000Z',
         updated_at: '2025-03-05T11:00:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'mar-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer7@gmail.com',
         order_code: '#300002',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Grace Lee',
            contact_email: 'customer7@gmail.com',
            contact_phone_number: '0333777777',
            contact_address_line: '777 Seventh Ln',
            contact_district: 'District 7',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'mar-item-002',
               order_id: 'mar-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15_PRO',
               color_name: 'BLACK',
               storage_name: '512GB',
               unit_price: 1600.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15-pro',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1600.0,
               created_at: '2025-03-22T15:30:00.000000Z',
               updated_at: '2025-03-22T15:30:00.000000Z',
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
         total_amount: 1600,
         created_at: '2025-03-22T15:30:00.000000Z',
         updated_at: '2025-03-22T15:30:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // April orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'apr-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer8@gmail.com',
         order_code: '#400001',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'Henry Wilson',
            contact_email: 'customer8@gmail.com',
            contact_phone_number: '0333888888',
            contact_address_line: '888 Eighth Way',
            contact_district: 'District 8',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'apr-item-001',
               order_id: 'apr-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'PINK',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1000.0,
               created_at: '2025-04-12T10:20:00.000000Z',
               updated_at: '2025-04-12T10:20:00.000000Z',
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
         created_at: '2025-04-12T10:20:00.000000Z',
         updated_at: '2025-04-12T10:20:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'apr-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer9@gmail.com',
         order_code: '#400002',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Iris Taylor',
            contact_email: 'customer9@gmail.com',
            contact_phone_number: '0333999999',
            contact_address_line: '999 Ninth Ct',
            contact_district: 'District 9',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'apr-item-002',
               order_id: 'apr-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'WHITE',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 4,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 4400.0,
               created_at: '2025-04-25T14:50:00.000000Z',
               updated_at: '2025-04-25T14:50:00.000000Z',
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
         total_amount: 4400,
         created_at: '2025-04-25T14:50:00.000000Z',
         updated_at: '2025-04-25T14:50:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // May orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'may-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer10@gmail.com',
         order_code: '#500001',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Jack Robinson',
            contact_email: 'customer10@gmail.com',
            contact_phone_number: '0334000000',
            contact_address_line: '1000 Tenth Pkwy',
            contact_district: 'District 10',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'may-item-001',
               order_id: 'may-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '512GB',
               unit_price: 1500.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 3000.0,
               created_at: '2025-05-08T09:40:00.000000Z',
               updated_at: '2025-05-08T09:40:00.000000Z',
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
         total_amount: 3000,
         created_at: '2025-05-08T09:40:00.000000Z',
         updated_at: '2025-05-08T09:40:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'may-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer11@gmail.com',
         order_code: '#500002',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'Karen White',
            contact_email: 'customer11@gmail.com',
            contact_phone_number: '0334111111',
            contact_address_line: '1111 Eleventh Dr',
            contact_district: 'District 11',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'may-item-002',
               order_id: 'may-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLACK',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1100.0,
               created_at: '2025-05-20T12:15:00.000000Z',
               updated_at: '2025-05-20T12:15:00.000000Z',
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
         total_amount: 1100,
         created_at: '2025-05-20T12:15:00.000000Z',
         updated_at: '2025-05-20T12:15:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: '073b0b68-f7fb-46a0-8851-1e472092f29c',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#912039',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Foo Bar',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284890',
            contact_address_line: '123 Street',
            contact_district: 'Thu Duc',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: '23e54d5b-02e5-4194-87dd-585007a22c4b',
               order_id: '073b0b68-f7fb-46a0-8851-1e472092f29c',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1000.0,
               created_at: '2025-10-27T17:47:57.662349Z',
               updated_at: '2025-10-27T17:47:57.662349Z',
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
         created_at: '2025-10-27T17:47:57.661369Z',
         updated_at: '2025-10-27T17:47:57.66137Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: '5b7404da-3b34-48f8-ab46-fc858b0698d7',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#268356',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Foo Bar',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284890',
            contact_address_line: '123 Street',
            contact_district: 'Thu Duc',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: '77cd2f63-5890-4fc5-8f85-ea86b0f4aef1',
               order_id: '5b7404da-3b34-48f8-ab46-fc858b0698d7',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: '550e8400-e29b-41d4-a716-446655440000',
               promotion_type: 'COUPON',
               discount_type: 'PERCENTAGE',
               discount_value: 0.1,
               discount_amount: 100.0,
               sub_total_amount: 900.0,
               created_at: '2025-10-27T18:01:42.770772Z',
               updated_at: '2025-10-27T18:01:42.770772Z',
               updated_by: null,
               is_deleted: false,
               deleted_at: null,
               deleted_by: null,
            },
         ],
         promotion_id: '550e8400-e29b-41d4-a716-446655440000',
         promotion_type: 'COUPON',
         discount_type: 'PERCENTAGE',
         discount_value: 0.1,
         discount_amount: 100.0,
         total_amount: 900.0,
         created_at: '2025-10-27T18:01:42.769227Z',
         updated_at: '2025-10-27T18:01:42.769227Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
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
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: '0c9d7078-2949-43e7-a4af-6acf3dd8453e',
               order_id: 'a6c9fbce-ab37-4472-b8d6-a3684b9d8241',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: 'ModelId',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: '',
               quantity: 1,
               is_reviewed: false,
               promotion_id: '99a356c8-c026-4137-8820-394763f30521',
               promotion_type: 'EVENT',
               discount_type: 'PERCENTAGE',
               discount_value: 0.1,
               discount_amount: 110.0,
               sub_total_amount: 990.0,
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
         discount_amount: 110.0,
         total_amount: 990.0,
         created_at: '2025-10-27T18:07:03.606782Z',
         updated_at: '2025-10-27T18:07:03.606782Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'd27b588e-5185-4f89-8739-204ec52009e6',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#989931',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Test',
            contact_email: 'user@gmail.com',
            contact_phone_number: 'Test',
            contact_address_line: 'Test',
            contact_district: 'Test',
            contact_province: 'Test',
            contact_country: 'Test',
         },
         order_items: [
            {
               order_item_id: '9dc6c733-07f3-4966-b5e0-79fe26d54813',
               order_id: 'd27b588e-5185-4f89-8739-204ec52009e6',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '664351e90087aa09993f5ae7',
               model_name: 'IPHONE_15',
               color_name: 'PINK',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               model_slug: '',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1000.0,
               created_at: '2025-10-28T15:57:04.337316Z',
               updated_at: '2025-10-28T15:57:04.337316Z',
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
         created_at: '2025-10-28T15:57:04.337312Z',
         updated_at: '2025-10-28T15:57:04.337312Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'fde01a20-0c85-403f-8000-3fc279e60434',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#372909',
         status: 'CANCELLED',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'test',
            contact_email: 'user@gmail.com',
            contact_phone_number: 'test',
            contact_address_line: 'test',
            contact_district: 'test',
            contact_province: 'test',
            contact_country: 'test',
         },
         order_items: [
            {
               order_item_id: '958bde3a-758d-4e57-b66c-76a4b5379657',
               order_id: 'fde01a20-0c85-403f-8000-3fc279e60434',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '664351e90087aa09993f5ae7',
               model_name: 'IPHONE_15',
               color_name: 'PINK',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               model_slug: '',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1000.0,
               created_at: '2025-10-28T16:42:15.203427Z',
               updated_at: '2025-10-28T16:42:15.203427Z',
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
         created_at: '2025-10-28T16:42:15.203422Z',
         updated_at: '2025-10-28T16:42:15.203422Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // September orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'sep-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#456789',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'John Doe',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284890',
            contact_address_line: '456 Avenue',
            contact_district: 'District 1',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'sep-item-001',
               order_id: 'sep-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15_PRO',
               color_name: 'BLACK',
               storage_name: '256GB',
               unit_price: 1200.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15-pro',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 2400.0,
               created_at: '2025-09-15T10:30:00.000000Z',
               updated_at: '2025-09-15T10:30:00.000000Z',
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
         total_amount: 2400,
         created_at: '2025-09-15T10:30:00.000000Z',
         updated_at: '2025-09-15T10:30:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'sep-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#456790',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'Jane Smith',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284891',
            contact_address_line: '789 Street',
            contact_district: 'District 2',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'sep-item-002',
               order_id: 'sep-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'WHITE',
               storage_name: '128GB',
               unit_price: 950.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 950.0,
               created_at: '2025-09-22T14:20:00.000000Z',
               updated_at: '2025-09-22T14:20:00.000000Z',
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
         total_amount: 950,
         created_at: '2025-09-22T14:20:00.000000Z',
         updated_at: '2025-09-22T14:20:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // August orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'aug-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#345678',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Mike Johnson',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284892',
            contact_address_line: '321 Road',
            contact_district: 'District 3',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'aug-item-001',
               order_id: 'aug-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '512GB',
               unit_price: 1500.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1500.0,
               created_at: '2025-08-10T09:15:00.000000Z',
               updated_at: '2025-08-10T09:15:00.000000Z',
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
         total_amount: 1500,
         created_at: '2025-08-10T09:15:00.000000Z',
         updated_at: '2025-08-10T09:15:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'aug-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#345679',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Sarah Williams',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284893',
            contact_address_line: '654 Boulevard',
            contact_district: 'District 4',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'aug-item-002',
               order_id: 'aug-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'GREEN',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 3,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 3300.0,
               created_at: '2025-08-25T16:45:00.000000Z',
               updated_at: '2025-08-25T16:45:00.000000Z',
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
         total_amount: 3300,
         created_at: '2025-08-25T16:45:00.000000Z',
         updated_at: '2025-08-25T16:45:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // July orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'jul-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#234567',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'David Brown',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284894',
            contact_address_line: '987 Lane',
            contact_district: 'District 5',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'jul-item-001',
               order_id: 'jul-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'PINK',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               model_slug: 'iphone-15',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 2000.0,
               created_at: '2025-07-05T11:30:00.000000Z',
               updated_at: '2025-07-05T11:30:00.000000Z',
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
         total_amount: 2000,
         created_at: '2025-07-05T11:30:00.000000Z',
         updated_at: '2025-07-05T11:30:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'jul-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#234568',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Emily Davis',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284895',
            contact_address_line: '147 Plaza',
            contact_district: 'District 6',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'jul-item-002',
               order_id: 'jul-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1100.0,
               created_at: '2025-07-18T13:00:00.000000Z',
               updated_at: '2025-07-18T13:00:00.000000Z',
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
         total_amount: 1100,
         created_at: '2025-07-18T13:00:00.000000Z',
         updated_at: '2025-07-18T13:00:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // June orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'jun-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#123456',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Chris Wilson',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284896',
            contact_address_line: '258 Court',
            contact_district: 'District 7',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'jun-item-001',
               order_id: 'jun-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLACK',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1000.0,
               created_at: '2025-06-12T08:20:00.000000Z',
               updated_at: '2025-06-12T08:20:00.000000Z',
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
         created_at: '2025-06-12T08:20:00.000000Z',
         updated_at: '2025-06-12T08:20:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'jun-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#123457',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'Lisa Anderson',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284897',
            contact_address_line: '369 Circle',
            contact_district: 'District 8',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'jun-item-002',
               order_id: 'jun-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'WHITE',
               storage_name: '512GB',
               unit_price: 1500.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 3000.0,
               created_at: '2025-06-28T15:50:00.000000Z',
               updated_at: '2025-06-28T15:50:00.000000Z',
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
         total_amount: 3000,
         created_at: '2025-06-28T15:50:00.000000Z',
         updated_at: '2025-06-28T15:50:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // November orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'nov-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer12@gmail.com',
         order_code: '#110001',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Laura Martinez',
            contact_email: 'customer12@gmail.com',
            contact_phone_number: '0334222222',
            contact_address_line: '1212 Twelfth Ave',
            contact_district: 'District 12',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'nov-item-001',
               order_id: 'nov-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15_PRO',
               color_name: 'TITANIUM',
               storage_name: '256GB',
               unit_price: 1300.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15-pro',
               quantity: 3,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 3900.0,
               created_at: '2025-11-05T10:10:00.000000Z',
               updated_at: '2025-11-05T10:10:00.000000Z',
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
         total_amount: 3900,
         created_at: '2025-11-05T10:10:00.000000Z',
         updated_at: '2025-11-05T10:10:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'nov-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer13@gmail.com',
         order_code: '#110002',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'Michael Chen',
            contact_email: 'customer13@gmail.com',
            contact_phone_number: '0334333333',
            contact_address_line: '1313 Thirteenth St',
            contact_district: 'District 1',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'nov-item-002',
               order_id: 'nov-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'GREEN',
               storage_name: '128GB',
               unit_price: 1000.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 2000.0,
               created_at: '2025-11-18T14:25:00.000000Z',
               updated_at: '2025-11-18T14:25:00.000000Z',
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
         total_amount: 2000,
         created_at: '2025-11-18T14:25:00.000000Z',
         updated_at: '2025-11-18T14:25:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'nov-003',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer14@gmail.com',
         order_code: '#110003',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Nancy Kim',
            contact_email: 'customer14@gmail.com',
            contact_phone_number: '0334444444',
            contact_address_line: '1414 Fourteenth Rd',
            contact_district: 'District 2',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'nov-item-003',
               order_id: 'nov-003',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'PINK',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1100.0,
               created_at: '2025-11-25T16:40:00.000000Z',
               updated_at: '2025-11-25T16:40:00.000000Z',
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
         total_amount: 1100,
         created_at: '2025-11-25T16:40:00.000000Z',
         updated_at: '2025-11-25T16:40:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      // December orders
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'dec-001',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer15@gmail.com',
         order_code: '#120001',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Oliver Thompson',
            contact_email: 'customer15@gmail.com',
            contact_phone_number: '0334555555',
            contact_address_line: '1515 Fifteenth Blvd',
            contact_district: 'District 3',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'dec-item-001',
               order_id: 'dec-001',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15_PRO',
               color_name: 'BLACK',
               storage_name: '512GB',
               unit_price: 1600.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15-pro',
               quantity: 2,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 3200.0,
               created_at: '2025-12-10T11:30:00.000000Z',
               updated_at: '2025-12-10T11:30:00.000000Z',
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
         total_amount: 3200,
         created_at: '2025-12-10T11:30:00.000000Z',
         updated_at: '2025-12-10T11:30:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'dec-002',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer16@gmail.com',
         order_code: '#120002',
         status: 'PAID',
         payment_method: 'COD',
         shipping_address: {
            contact_name: 'Patricia Wong',
            contact_email: 'customer16@gmail.com',
            contact_phone_number: '0334666666',
            contact_address_line: '1616 Sixteenth Way',
            contact_district: 'District 4',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'dec-item-002',
               order_id: 'dec-002',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '256GB',
               unit_price: 1100.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 4,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 4400.0,
               created_at: '2025-12-20T15:00:00.000000Z',
               updated_at: '2025-12-20T15:00:00.000000Z',
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
         total_amount: 4400,
         created_at: '2025-12-20T15:00:00.000000Z',
         updated_at: '2025-12-20T15:00:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'dec-003',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'customer17@gmail.com',
         order_code: '#120003',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Quinn Roberts',
            contact_email: 'customer17@gmail.com',
            contact_phone_number: '0334777777',
            contact_address_line: '1717 Seventeenth Ct',
            contact_district: 'District 5',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_item_id: 'dec-item-003',
               order_id: 'dec-003',
               tenant_id: null,
               branch_id: null,
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'WHITE',
               storage_name: '512GB',
               unit_price: 1500.0,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               is_reviewed: false,
               promotion_id: null,
               promotion_type: null,
               discount_type: null,
               discount_value: null,
               discount_amount: null,
               sub_total_amount: 1500.0,
               created_at: '2025-12-28T17:45:00.000000Z',
               updated_at: '2025-12-28T17:45:00.000000Z',
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
         total_amount: 1500,
         created_at: '2025-12-28T17:45:00.000000Z',
         updated_at: '2025-12-28T17:45:00.000000Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_page=1&_limit=5',
      prev: null,
      next: '?_page=2&_limit=5',
      last: '?_page=2&_limit=5',
   },
};

// export type TOrder = typeof fakeData;
export type TOrderItem = (typeof fakeData.items)[number];

const chartConfig = {
   revenue: {
      label: 'Revenue ($)',
      color: 'var(--chart-1)',
   },
   orders: {
      label: 'Orders',
      color: 'var(--chart-2)',
   },
} satisfies ChartConfig;

type FilterMetric = 'revenue' | 'orders' | 'both';
type GroupBy = 'date' | 'month';

const RevenueAnalytics = () => {
   // Filter states for Chart 1
   const [range, setRange] = useState<DateRange | undefined>(undefined);
   const [filterMetric, setFilterMetric] = useState<FilterMetric>('both');
   const [groupBy, setGroupBy] = useState<GroupBy>('date');
   const [isFiltered, setIsFiltered] = useState(false);

   // Filter states for Chart 2 - Multi-select years
   const [selectedYears, setSelectedYears] = useState<{
      year2024: boolean;
      year2025: boolean;
   }>({
      year2024: true,
      year2025: true,
   });

   const toggleYear = (year: 'year2024' | 'year2025') => {
      setSelectedYears((prev) => ({
         ...prev,
         [year]: !prev[year],
      }));
   };

   const getYearFilterLabel = () => {
      if (selectedYears.year2024 && selectedYears.year2025) return 'Both Years';
      if (selectedYears.year2024) return '2024';
      if (selectedYears.year2025) return '2025';
      return 'Select Years';
   };

   // Process and filter data
   const chartData = useMemo(() => {
      const dateMap = new Map<
         string,
         { revenue: number; orders: number; timestamp: number }
      >();

      fakeData.items.forEach((order) => {
         if (order.status === 'PAID') {
            const date = new Date(order.created_at);

            // Apply date range filter if set and filter is active
            if (isFiltered && range?.from && range?.to) {
               const isInRange = isWithinInterval(date, {
                  start: range.from,
                  end: range.to,
               });
               if (!isInRange) return;
            }

            // Group by date or month
            let dateKey: string;
            let timestamp: number;

            if (groupBy === 'month') {
               dateKey = date.toLocaleDateString('en-US', {
                  month: 'short',
                  year: 'numeric',
               });
               timestamp = startOfMonth(date).getTime();
            } else {
               dateKey = date.toLocaleDateString('en-US', {
                  month: 'short',
                  day: 'numeric',
               });
               timestamp = date.getTime();
            }

            if (!dateMap.has(dateKey)) {
               dateMap.set(dateKey, {
                  revenue: 0,
                  orders: 0,
                  timestamp,
               });
            }

            const current = dateMap.get(dateKey)!;
            current.revenue += order.total_amount;
            current.orders += 1;
         }
      });

      // Sort by timestamp
      return Array.from(dateMap.entries())
         .sort(([, a], [, b]) => a.timestamp - b.timestamp)
         .map(([date, data]) => ({
            date,
            revenue: data.revenue,
            orders: data.orders,
         }));
   }, [range, groupBy, isFiltered]);

   // Handle filter submission
   const handleSubmitFilter = () => {
      setIsFiltered(true);
   };

   // Reset filter
   const handleResetFilter = () => {
      setRange(undefined);
      setFilterMetric('both');
      setGroupBy('date');
      setIsFiltered(false);
   };

   return (
      <div className="flex flex-col flex-1 gap-4 p-4">
         <h1 className="text-3xl font-bold tracking-tight">
            Revenue Analytics
         </h1>

         <p className="text-muted-foreground">
            View your revenue analytics and compare them with the previous
            month.
         </p>

         <div>
            {/* Filter section */}
            <div>
               <div className="flex items-center gap-4">
                  <DropdownMenu>
                     <DropdownMenuTrigger asChild>
                        <Button variant="outline">
                           Filter by:{' '}
                           {filterMetric === 'both'
                              ? 'All'
                              : filterMetric === 'revenue'
                                ? 'Revenue'
                                : 'Orders'}
                           <div className="flex items-center gap-2">
                              <ChevronDown />
                           </div>
                        </Button>
                     </DropdownMenuTrigger>
                     <DropdownMenuContent
                        align="start"
                        side="bottom"
                        sideOffset={4}
                     >
                        <DropdownMenuCheckboxItem
                           checked={filterMetric === 'both'}
                           onCheckedChange={() => setFilterMetric('both')}
                        >
                           All
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           checked={filterMetric === 'revenue'}
                           onCheckedChange={() => setFilterMetric('revenue')}
                        >
                           Revenue
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           checked={filterMetric === 'orders'}
                           onCheckedChange={() => setFilterMetric('orders')}
                        >
                           Quantity of orders
                        </DropdownMenuCheckboxItem>
                     </DropdownMenuContent>
                  </DropdownMenu>

                  <DropdownMenu>
                     <DropdownMenuTrigger asChild>
                        <Button variant="outline">
                           Group by: {groupBy === 'date' ? 'Date' : 'Month'}
                           <div className="flex items-center gap-2">
                              <ChevronDown />
                           </div>
                        </Button>
                     </DropdownMenuTrigger>
                     <DropdownMenuContent
                        align="start"
                        side="bottom"
                        sideOffset={4}
                     >
                        <DropdownMenuCheckboxItem
                           checked={groupBy === 'date'}
                           onCheckedChange={() => setGroupBy('date')}
                        >
                           Date
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           checked={groupBy === 'month'}
                           onCheckedChange={() => setGroupBy('month')}
                        >
                           Month
                        </DropdownMenuCheckboxItem>
                     </DropdownMenuContent>
                  </DropdownMenu>

                  {/* Date range picker */}
                  <div>
                     <Popover>
                        <PopoverTrigger asChild>
                           <Button
                              variant="outline"
                              id="dates"
                              className="w-56 justify-between font-normal"
                           >
                              {range?.from && range?.to
                                 ? `${range.from.toLocaleDateString()} - ${range.to.toLocaleDateString()}`
                                 : 'Select date'}
                              <ChevronDownIcon />
                           </Button>
                        </PopoverTrigger>
                        <PopoverContent
                           className="w-auto overflow-hidden p-0"
                           align="start"
                        >
                           <Calendar
                              mode="range"
                              selected={range}
                              captionLayout="dropdown"
                              onSelect={(range) => {
                                 setRange(range);
                              }}
                           />
                        </PopoverContent>
                     </Popover>
                  </div>
                  <Button
                     variant="ghost"
                     className="text-blue-600 hover:text-blue-600 hover:underline"
                     onClick={handleSubmitFilter}
                     disabled={!range?.from || !range?.to}
                  >
                     Apply Filter
                  </Button>
                  {isFiltered && (
                     <Button variant="outline" onClick={handleResetFilter}>
                        Reset
                     </Button>
                  )}
               </div>

               {/* Filter status */}
               {isFiltered && (
                  <div className="flex items-center gap-2 text-sm text-muted-foreground">
                     <span>
                        Showing{' '}
                        <strong>
                           {filterMetric === 'both'
                              ? 'All Metrics'
                              : filterMetric === 'revenue'
                                ? 'Revenue'
                                : 'Orders'}
                        </strong>{' '}
                        grouped by{' '}
                        <strong>{groupBy === 'date' ? 'Date' : 'Month'}</strong>
                        {range?.from && range?.to && (
                           <>
                              {' '}
                              from{' '}
                              <strong>
                                 {range.from.toLocaleDateString()}
                              </strong>{' '}
                              to{' '}
                              <strong>{range.to.toLocaleDateString()}</strong>
                           </>
                        )}
                     </span>
                  </div>
               )}

               {/* Chart 1 */}
               <Card className="mt-4">
                  <CardHeader>
                     <CardTitle>Revenue & Orders Chart</CardTitle>
                     <CardDescription>
                        January - December 2025 - Order Performance
                     </CardDescription>
                  </CardHeader>
                  <CardContent>
                     <ChartContainer config={chartConfig}>
                        <LineChart
                           accessibilityLayer
                           data={chartData}
                           margin={{
                              top: 20,
                              left: 12,
                              right: 12,
                           }}
                        >
                           <CartesianGrid vertical={false} />
                           <XAxis
                              dataKey="date"
                              tickLine={false}
                              axisLine={false}
                              tickMargin={8}
                           />
                           <YAxis
                              tickLine={false}
                              axisLine={false}
                              tickMargin={8}
                              tickFormatter={(value) => `${value}`}
                           />
                           <ChartTooltip
                              cursor={false}
                              content={<ChartTooltipContent indicator="line" />}
                           />
                           {(filterMetric === 'revenue' ||
                              filterMetric === 'both') && (
                              <Line
                                 dataKey="revenue"
                                 type="natural"
                                 stroke="hsl(var(--color-revenue))"
                                 strokeWidth={2}
                                 dot={{
                                    fill: 'hsl(var(--color-revenue))',
                                 }}
                                 activeDot={{
                                    r: 6,
                                 }}
                              >
                                 <LabelList
                                    position="top"
                                    offset={12}
                                    className="fill-foreground"
                                    fontSize={12}
                                 />
                              </Line>
                           )}
                           {(filterMetric === 'orders' ||
                              filterMetric === 'both') && (
                              <Line
                                 dataKey="orders"
                                 type="natural"
                                 stroke="hsl(var(--color-orders))"
                                 strokeWidth={2}
                                 dot={{
                                    fill: 'hsl(var(--color-orders))',
                                 }}
                                 activeDot={{
                                    r: 6,
                                 }}
                              >
                                 <LabelList
                                    position="top"
                                    offset={12}
                                    className="fill-foreground"
                                    fontSize={12}
                                 />
                              </Line>
                           )}
                        </LineChart>
                     </ChartContainer>
                  </CardContent>
                  <CardFooter className="flex-col items-start gap-2 text-sm">
                     <div className="flex gap-2 leading-none font-medium">
                        {filterMetric === 'revenue' && 'Revenue Analytics'}
                        {filterMetric === 'orders' && 'Order Analytics'}
                        {filterMetric === 'both' &&
                           'Revenue & Order Analytics'}{' '}
                        <TrendingUp className="h-4 w-4" />
                     </div>
                     <div className="text-muted-foreground leading-none">
                        {isFiltered
                           ? `Showing filtered data ${groupBy === 'month' ? 'grouped by month' : 'by date'}`
                           : 'Showing all paid orders from January - December 2025'}
                     </div>
                  </CardFooter>
               </Card>
            </div>

            <div className="mt-10">
               <div className="flex items-center gap-4">
                  <DropdownMenu>
                     <DropdownMenuTrigger asChild>
                        <Button variant="outline">
                           Filter by: {getYearFilterLabel()}
                           <div className="flex items-center gap-2">
                              <ChevronDown />
                           </div>
                        </Button>
                     </DropdownMenuTrigger>
                     <DropdownMenuContent
                        align="start"
                        side="bottom"
                        sideOffset={4}
                     >
                        <DropdownMenuCheckboxItem
                           checked={selectedYears.year2024}
                           onCheckedChange={() => toggleYear('year2024')}
                        >
                           2024
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           checked={selectedYears.year2025}
                           onCheckedChange={() => toggleYear('year2025')}
                        >
                           2025
                        </DropdownMenuCheckboxItem>
                     </DropdownMenuContent>
                  </DropdownMenu>
               </div>

               {/* Chart 2 - Multi-Year Comparison */}
               <Card className="mt-4">
                  <CardHeader>
                     <CardTitle>
                        {selectedYears.year2024 && selectedYears.year2025
                           ? 'Year-over-Year Revenue Comparison'
                           : selectedYears.year2024
                             ? '2024 Revenue Analytics'
                             : selectedYears.year2025
                               ? '2025 Revenue Analytics'
                               : 'Revenue Analytics'}
                     </CardTitle>
                     <CardDescription>
                        {selectedYears.year2024 && selectedYears.year2025
                           ? 'Monthly revenue comparison between 2024 and 2025'
                           : selectedYears.year2024
                             ? 'Monthly revenue for 2024'
                             : selectedYears.year2025
                               ? 'Monthly revenue for 2025'
                               : 'Select years to display'}
                     </CardDescription>
                     <div className="flex items-center gap-4">
                        {[
                           { year: 2024, color: '#e11d48', key: 'year2024' },
                           { year: 2025, color: '#3b82f6', key: 'year2025' },
                        ].map((item) => (
                           <div
                              key={item.key}
                              className="flex items-center gap-2"
                           >
                              <span
                                 className="w-5 h-2 rounded-full"
                                 style={{ backgroundColor: item.color }}
                              ></span>
                              <p className="text-sm font-medium">{item.year}</p>
                           </div>
                        ))}
                     </div>
                  </CardHeader>
                  <CardContent>
                     <ChartContainer
                        config={{
                           year2024: {
                              label: '2024',
                              color: 'hsl(var(--chart-1))',
                           },
                           year2025: {
                              label: '2025',
                              color: 'hsl(var(--chart-2))',
                           },
                        }}
                     >
                        <LineChart
                           accessibilityLayer
                           data={[
                              { month: 'Jan', year2024: 2800, year2025: 4600 },
                              { month: 'Feb', year2024: 3200, year2025: 5200 },
                              { month: 'Mar', year2024: 2900, year2025: 3800 },
                              { month: 'Apr', year2024: 3500, year2025: 5400 },
                              { month: 'May', year2024: 2700, year2025: 4100 },
                              { month: 'Jun', year2024: 3100, year2025: 4000 },
                              { month: 'Jul', year2024: 3400, year2025: 3100 },
                              { month: 'Aug', year2024: 3800, year2025: 4800 },
                              { month: 'Sep', year2024: 2600, year2025: 3350 },
                              { month: 'Oct', year2024: 3300, year2025: 3890 },
                              { month: 'Nov', year2024: 2900, year2025: 7000 },
                              { month: 'Dec', year2024: 4200, year2025: 9100 },
                           ]}
                           margin={{
                              top: 20,
                              left: 12,
                              right: 12,
                           }}
                        >
                           <CartesianGrid vertical={false} />
                           <XAxis
                              dataKey="month"
                              tickLine={false}
                              axisLine={false}
                              tickMargin={8}
                           />
                           <YAxis
                              tickLine={false}
                              axisLine={false}
                              tickMargin={8}
                              tickFormatter={(value) => `$${value}`}
                           />
                           <ChartTooltip
                              cursor={false}
                              content={<ChartTooltipContent indicator="line" />}
                           />
                           {selectedYears.year2024 && (
                              <Line
                                 dataKey="year2024"
                                 type="natural"
                                 stroke="#e11d48"
                                 strokeWidth={2}
                                 dot={{
                                    fill: '#e11d48',
                                 }}
                                 activeDot={{
                                    r: 6,
                                 }}
                              >
                                 <LabelList
                                    position="top"
                                    offset={12}
                                    className="fill-foreground"
                                    fontSize={12}
                                 />
                              </Line>
                           )}
                           {selectedYears.year2025 && (
                              <Line
                                 dataKey="year2025"
                                 type="natural"
                                 stroke="#3b82f6"
                                 strokeWidth={2}
                                 dot={{
                                    fill: '#3b82f6',
                                 }}
                                 activeDot={{
                                    r: 6,
                                 }}
                              >
                                 <LabelList
                                    position="top"
                                    offset={12}
                                    className="fill-foreground"
                                    fontSize={12}
                                 />
                              </Line>
                           )}
                        </LineChart>
                     </ChartContainer>
                  </CardContent>
                  <CardFooter className="flex-col items-start gap-2 text-sm">
                     <div className="flex gap-2 leading-none font-medium">
                        {selectedYears.year2024 &&
                           selectedYears.year2025 &&
                           '2025 shows 38% increase compared to 2024'}
                        {selectedYears.year2024 &&
                           !selectedYears.year2025 &&
                           'Showing 2024 revenue data'}
                        {selectedYears.year2025 &&
                           !selectedYears.year2024 &&
                           'Showing 2025 revenue data'}
                        {!selectedYears.year2024 &&
                           !selectedYears.year2025 &&
                           'No data selected'}{' '}
                        <TrendingUp className="h-4 w-4" />
                     </div>
                     <div className="text-muted-foreground leading-none">
                        {selectedYears.year2024 && selectedYears.year2025
                           ? 'Comparing monthly revenue across two years'
                           : selectedYears.year2024
                             ? 'Displaying 2024 monthly revenue trend'
                             : selectedYears.year2025
                               ? 'Displaying 2025 monthly revenue trend'
                               : 'Select at least one year to display data'}
                     </div>
                  </CardFooter>
               </Card>
            </div>
         </div>
      </div>
   );
};

export default RevenueAnalytics;
