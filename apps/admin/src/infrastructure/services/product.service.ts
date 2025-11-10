import { createApi } from '@reduxjs/toolkit/query/react';
import { baseQuery } from './base-query';
import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('catalog-services')(args, api, extraOptions);

   return result;
};

const fakePopularProductsData = {
   total_records: 1,
   total_pages: 1,
   page_size: 5,
   current_page: 1,
   items: [
      {
         id: '664351e90087aa09993f5ae7',
         category: {
            id: '67dc470aa9ee0a5e6fbafdab',
            name: 'iPhone',
            description: 'iPhone categories.',
            order: 2,
            slug: 'iphone',
            parent_id: null,
            created_at: '2025-11-07T14:53:17.3Z',
            updated_at: '2025-11-07T14:53:17.3Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         name: 'iPhone 15',
         normalized_model: 'IPHONE_15',
         product_classification: 'IPHONE',
         model_items: [
            {
               name: 'IPHONE_15',
               normalized_name: 'IPHONE_15',
               order: 0,
            },
            {
               name: 'IPHONE_15_PLUS',
               normalized_name: 'IPHONE_15_PLUS',
               order: 1,
            },
         ],
         color_items: [
            {
               name: 'Blue',
               normalized_name: 'BLUE',
               hex_code: '#D5DDDF',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
               order: 0,
            },
            {
               name: 'Pink',
               normalized_name: 'PINK',
               hex_code: '#EBD3D4',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
               order: 1,
            },
            {
               name: 'Yellow',
               normalized_name: 'YELLOW',
               hex_code: '#EDE6C8',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
               order: 2,
            },
            {
               name: 'Green',
               normalized_name: 'GREEN',
               hex_code: '#D0D9CA',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
               order: 3,
            },
            {
               name: 'Black',
               normalized_name: 'BLACK',
               hex_code: '#4B4F50',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
               order: 4,
            },
         ],
         storage_items: [
            {
               name: '128GB',
               normalized_name: '128GB',
               order: 0,
            },
            {
               name: '256GB',
               normalized_name: '256GB',
               order: 1,
            },
            {
               name: '512GB',
               normalized_name: '512GB',
               order: 2,
            },
            {
               name: '1TB',
               normalized_name: '1TB',
               order: 3,
            },
         ],
         sku_prices: [
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
         ],
         description: 'iPhone 15 model description.',
         showcase_images: [
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 0,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 1,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 2,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 3,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 4,
            },
         ],
         overall_sold: 0,
         rating_stars: [
            {
               star: 1,
               count: 0,
            },
            {
               star: 2,
               count: 0,
            },
            {
               star: 3,
               count: 0,
            },
            {
               star: 4,
               count: 0,
            },
            {
               star: 5,
               count: 0,
            },
         ],
         average_rating: {
            rating_average_value: 0,
            rating_count: 0,
         },
         promotion: {
            promotion_type: 'INSTANT_DISCOUNT',
            promotion_discount_type: 'PERCENTAGE',
            promotion_discount_value: 0.1,
            promotion_discount_amount: 130,
            final_price: 1170,
         },
         is_newest: true,
         slug: 'iphone-15',
         created_at: '2025-11-07T14:53:17.334Z',
         updated_at: '2025-11-07T14:53:17.334Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_page=1&_limit=5',
      prev: null,
      next: null,
      last: '?_page=1&_limit=5',
   },
};

const fakeNewestProductsData = {
   total_records: 1,
   total_pages: 1,
   page_size: 5,
   current_page: 1,
   items: [
      {
         id: '664351e90087aa09993f5ae7',
         category: {
            id: '67dc470aa9ee0a5e6fbafdab',
            name: 'iPhone',
            description: 'iPhone categories.',
            order: 2,
            slug: 'iphone',
            parent_id: null,
            created_at: '2025-11-07T14:53:17.3Z',
            updated_at: '2025-11-07T14:53:17.3Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         name: 'iPhone 15',
         normalized_model: 'IPHONE_15',
         product_classification: 'IPHONE',
         model_items: [
            {
               name: 'IPHONE_15',
               normalized_name: 'IPHONE_15',
               order: 0,
            },
            {
               name: 'IPHONE_15_PLUS',
               normalized_name: 'IPHONE_15_PLUS',
               order: 1,
            },
         ],
         color_items: [
            {
               name: 'Blue',
               normalized_name: 'BLUE',
               hex_code: '#D5DDDF',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
               order: 0,
            },
            {
               name: 'Pink',
               normalized_name: 'PINK',
               hex_code: '#EBD3D4',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
               order: 1,
            },
            {
               name: 'Yellow',
               normalized_name: 'YELLOW',
               hex_code: '#EDE6C8',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
               order: 2,
            },
            {
               name: 'Green',
               normalized_name: 'GREEN',
               hex_code: '#D0D9CA',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
               order: 3,
            },
            {
               name: 'Black',
               normalized_name: 'BLACK',
               hex_code: '#4B4F50',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
               order: 4,
            },
         ],
         storage_items: [
            {
               name: '128GB',
               normalized_name: '128GB',
               order: 0,
            },
            {
               name: '256GB',
               normalized_name: '256GB',
               order: 1,
            },
            {
               name: '512GB',
               normalized_name: '512GB',
               order: 2,
            },
            {
               name: '1TB',
               normalized_name: '1TB',
               order: 3,
            },
         ],
         sku_prices: [
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
         ],
         description: 'iPhone 15 model description.',
         showcase_images: [
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 0,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 1,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 2,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 3,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 4,
            },
         ],
         is_newest: true,
         slug: 'iphone-15',
         created_at: '2025-11-07T14:53:17.334Z',
         updated_at: '2025-11-07T14:53:17.334Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_page=1&_limit=5',
      prev: null,
      next: null,
      last: '?_page=1&_limit=5',
   },
};

const fakeSuggestionProductsData = {
   total_records: 1,
   total_pages: 1,
   page_size: 5,
   current_page: 1,
   items: [
      {
         id: '664351e90087aa09993f5ae7',
         category: {
            id: '67dc470aa9ee0a5e6fbafdab',
            name: 'iPhone',
            description: 'iPhone categories.',
            order: 2,
            slug: 'iphone',
            parent_id: null,
            created_at: '2025-11-07T14:53:17.3Z',
            updated_at: '2025-11-07T14:53:17.3Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         name: 'iPhone 15',
         normalized_model: 'IPHONE_15',
         product_classification: 'IPHONE',
         model_items: [
            {
               name: 'IPHONE_15',
               normalized_name: 'IPHONE_15',
               order: 0,
            },
            {
               name: 'IPHONE_15_PLUS',
               normalized_name: 'IPHONE_15_PLUS',
               order: 1,
            },
         ],
         color_items: [
            {
               name: 'Blue',
               normalized_name: 'BLUE',
               hex_code: '#D5DDDF',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
               order: 0,
            },
            {
               name: 'Pink',
               normalized_name: 'PINK',
               hex_code: '#EBD3D4',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
               order: 1,
            },
            {
               name: 'Yellow',
               normalized_name: 'YELLOW',
               hex_code: '#EDE6C8',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
               order: 2,
            },
            {
               name: 'Green',
               normalized_name: 'GREEN',
               hex_code: '#D0D9CA',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
               order: 3,
            },
            {
               name: 'Black',
               normalized_name: 'BLACK',
               hex_code: '#4B4F50',
               showcase_image_id:
                  'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
               order: 4,
            },
         ],
         storage_items: [
            {
               name: '128GB',
               normalized_name: '128GB',
               order: 0,
            },
            {
               name: '256GB',
               normalized_name: '256GB',
               order: 1,
            },
            {
               name: '512GB',
               normalized_name: '512GB',
               order: 2,
            },
            {
               name: '1TB',
               normalized_name: '1TB',
               order: 3,
            },
         ],
         sku_prices: [
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLUE',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'PINK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'YELLOW',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'GREEN',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15',
               normalized_color: 'BLACK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLUE',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'PINK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'YELLOW',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'GREEN',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '128GB',
               unit_price: 1000,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '256GB',
               unit_price: 1100,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '512GB',
               unit_price: 1200,
            },
            {
               normalized_model: 'IPHONE_15_PLUS',
               normalized_color: 'BLACK',
               normalized_storage: '1TB',
               unit_price: 1300,
            },
         ],
         description: 'iPhone 15 model description.',
         showcase_images: [
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 0,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 1,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 2,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 3,
            },
            {
               image_id: 'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
               image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
               image_name: '',
               image_description: '',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 4,
            },
         ],
         overall_sold: 0,
         rating_stars: [
            {
               star: 1,
               count: 0,
            },
            {
               star: 2,
               count: 0,
            },
            {
               star: 3,
               count: 0,
            },
            {
               star: 4,
               count: 0,
            },
            {
               star: 5,
               count: 0,
            },
         ],
         average_rating: {
            rating_average_value: 0,
            rating_count: 0,
         },
         promotion: {
            promotion_type: 'INSTANT_DISCOUNT',
            promotion_discount_type: 'PERCENTAGE',
            promotion_discount_value: 0.1,
            promotion_discount_amount: 130,
            final_price: 1170,
         },
         is_newest: true,
         slug: 'iphone-15',
         created_at: '2025-11-07T14:53:17.334Z',
         updated_at: '2025-11-07T14:53:17.334Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_page=1&_limit=5',
      prev: null,
      next: null,
      last: '?_page=1&_limit=5',
   },
};

export type TProductModel = {
   id: string;
   category: TCategory;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModelItem[];
   color_items: TColorItem[];
   storage_items: TStorageItem[];
   sku_prices: TSkuPrice[];
   showcase_images: TShowcaseImage[];
   description: string;
   average_rating: TAverageRating;
   rating_stars: TRatingStar[];
   overall_sold: number;
   promotion: TPromotion | null;
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TCategory = {
   id: string;
   name: string;
   description: string;
   order: number;
   slug: string;
   parent_id: string | null;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};
export type TModelItem = {
   name: string;
   normalized_name: string;
   order: number;
};
export type TColorItem = {
   name: string;
   normalized_name: string;
   hex_code: string;
   showcase_image_id: string;
   order: number;
};
export type TStorageItem = {
   name: string;
   normalized_name: string;
   order: number;
};
export type TSkuPrice = {
   sku_id: string;
   normalized_model: string;
   normalized_color: string;
   normalized_storage: string;
   unit_price: number;
};
export type TShowcaseImage = {
   image_id: string;
   image_url: string;
   image_name: string;
   image_description: string;
   image_width: number;
   image_height: number;
   image_bytes: number;
   image_order: number;
};
export type TRatingStar = {
   star: number;
   count: number;
};
export type TAverageRating = {
   rating_average_value: number;
   rating_count: number;
};
export type TPromotion = {
   promotion_type: 'INSTANT_DISCOUNT' | 'EVENT';
   promotion_discount_type: 'PERCENTAGE' | 'FIXED_AMOUNT';
   promotion_discount_value: number;
   promotion_discount_amount: number;
   final_price: number;
};

export type TPopularProduct = {
   id: string;
   category: TCategory;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModelItem[];
   color_items: TColorItem[];
   storage_items: TStorageItem[];
   sku_prices: TSkuPrice[];
   description: string;
   showcase_images: TShowcaseImage[];
   overall_sold: number;
   rating_stars: TRatingStar[];
   average_rating: TAverageRating;
   promotion: TPromotion | null;
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};
export type TNewestProduct = {
   id: string;
   category: TCategory;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModelItem[];
   color_items: TColorItem[];
   storage_items: TStorageItem[];
   sku_prices: TSkuPrice[];
   description: string | null;
   showcase_images: TShowcaseImage[];
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};
export type TSuggestionProduct = {
   id: string;
   category: TCategory;
   name: string;
   normalized_model: string;
   product_classification: string;
   model_items: TModelItem[];
   color_items: TColorItem[];
   storage_items: TStorageItem[];
   sku_prices: TSkuPrice[];
   description: string;
   showcase_images: TShowcaseImage[];
   overall_sold: number;
   rating_stars: TRatingStar[];
   average_rating: TAverageRating;
   promotion: TPromotion | null;
   is_newest: boolean;
   slug: string;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export const productApi = createApi({
   reducerPath: 'product-api',
   tagTypes: ['Products'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getProducts: builder.query<
         PaginationResponse<TProductModel>,
         Record<string, any>
      >({
         query: (query: Record<string, string>) => {
            return {
               url: '/api/v1/products',
               params: query,
            };
         },
         providesTags: ['Products'],
      }),
   }),
});

export const { useLazyGetProductsQuery } = productApi;
