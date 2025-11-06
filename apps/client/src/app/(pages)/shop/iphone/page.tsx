'use client';

import { useEffect, useMemo } from 'react';
import { FaFilter } from 'react-icons/fa6';
import { Button } from '@components/ui/button';
import { IoChatboxEllipsesOutline } from 'react-icons/io5';
import { GoMultiSelect } from 'react-icons/go';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import FilterSection from './_components/_layout/filter-section';
import IphoneModel from './_components/iphone-model';
import { Skeleton } from '@components/ui/skeleton';
import { Separator } from '@components/ui/separator';
import {
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
} from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';
import useCatalogService from '@components/hooks/api/use-catalog-service';
import useFilter from '../_hooks/use-filter';
import usePagination from '@components/hooks/use-pagination';
import SuggestionProduct from '@components/client/suggestion-product';
import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '@components/ui/carousel';

type IphoneModelsFilter = {
   _colors: string[];
   _storages: string[];
   _models: string[];
   _minPrice: number;
   _maxPrice: number;
   _priceSort: 'ASC' | 'DESC' | null;
   _page: number;
   _limit: number;
};

const fakeData = {
   total_records: 1,
   total_pages: 1,
   page_size: 1,
   current_page: 1,
   items: [
      {
         id: '68e80ce7e424d0f8cd8a35d0',
         category: {
            id: '68e3fc0c240062be872a0379',
            name: 'iPhone',
            description: 'iPhone categories.',
            order: 0,
            slug: 'iphone',
            parent_id: null,
            created_at: '2025-10-10T21:23:02.958799+07:00',
            updated_at: '2025-10-10T21:23:02.9588779+07:00',
            modified_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         name: 'iPhone 15',
         model_items: [
            {
               name: 'iPhone 15',
               normalized_name: 'IPHONE_15',
               order: 0,
            },
            {
               name: 'iPhone 15 Plus',
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
               image_name: 'iPhone 15 blue',
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
               image_name: 'iPhone 15 pink',
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
               image_name: 'iPhone 15 yellow',
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
               image_name: 'iPhone 15 green',
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
               image_name: 'iPhone 15 black',
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
         slug: 'iphone-15',
      },
   ],
   links: {
      first: '',
      prev: '',
      next: '',
      last: '',
   },
};

const suggestionProductFakeData = {
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
            created_at: '2025-10-21T05:33:50.043Z',
            updated_at: '2025-10-21T05:33:50.043Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         name: 'iPhone 15',
         modelItems: [
            {
               name: 'iPhone 15',
               normalized_name: 'IPHONE_15',
               order: 0,
            },
            {
               name: 'iPhone 15 Plus',
               normalized_name: 'IPHONE_15_PLUS',
               order: 1,
            },
         ],
         colorItems: [
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
         storageItems: [
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
         skuPrices: [
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
         showcaseImages: [
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
         overallSold: 0,
         promotion: {
            promotion_type: 'INSTANT_DISCOUNT',
            promotion_discount_type: 'PERCENTAGE',
            promotion_discount_value: 0.1,
            promotion_discount_amount: 130,
            final_price: 1170,
         },
         ratingStars: [
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
         averageRating: {
            rating_average_value: 0,
            rating_count: 0,
         },
         slug: 'iphone-15',
      },
   ],
   links: {
      first: '?_page=1&_limit=5',
      prev: null,
      next: null,
      last: '?_page=1&_limit=5',
   },
};

export type TSuggestionProduct =
   (typeof suggestionProductFakeData.items)[number];

const IphoneShopPage = () => {
   const { filters, setFilters, clearFilters, activeFilterCount } =
      useFilter<IphoneModelsFilter>();

   const { getModelBySlugState, getIphoneModelsAsync, isLoading } =
      useCatalogService();

   // Convert filters to query string for API call
   const queryString = useMemo(() => {
      const params = new URLSearchParams();
      Object.entries(filters).forEach(([key, value]) => {
         if (value !== null && value !== undefined) {
            if (Array.isArray(value)) {
               if (value.length > 0) {
                  // Append each value separately for proper multi-value support
                  value.forEach((v) => {
                     params.append(key, String(v));
                  });
               }
            } else if (
               typeof value === 'number' ||
               typeof value === 'boolean'
            ) {
               params.set(key, String(value));
            } else if (typeof value === 'string') {
               if (value.length > 0) {
                  params.set(key, value);
               }
            }
         }
      });
      return params.toString();
   }, [filters]);

   useEffect(() => {
      getIphoneModelsAsync(queryString);
   }, [queryString, getIphoneModelsAsync]);

   // Use pagination data from API or fallback to fake data
   const paginationResponseData = useMemo(() => {
      if (getModelBySlugState.isSuccess && getModelBySlugState.data) {
         return getModelBySlugState.data;
      }
      return fakeData;
   }, [getModelBySlugState.isSuccess, getModelBySlugState.data]);

   const {
      currentPage,
      totalPages,
      pageSize,
      totalRecords,
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,
      paginationItems,
      getPageNumbers,
   } = usePagination(paginationResponseData);

   const handleSortChange = (value: string) => {
      // Map sort value to priceSort
      let priceSort: 'ASC' | 'DESC' | null = null;

      if (value === 'price-low-high') {
         priceSort = 'ASC';
      } else if (value === 'price-high-low') {
         priceSort = 'DESC';
      }

      setFilters({ _priceSort: priceSort });
   };

   const handlePageChange = (page: number) => {
      setFilters({ _page: page });
      window.scrollTo({ top: 0, behavior: 'smooth' });
   };

   const handlePageSizeChange = (size: string) => {
      setFilters({ _limit: Number(size), _page: 1 });
   };

   return (
      <div>
         {/* FILTERS INFO */}
         <div className="w-full border-y border-[#ccc] text-[15px] font-semibold">
            <div className="h-[4.514vw] w-full max-w-[1440px] mx-auto px-5 flex flex-row justify-start items-center">
               <div className="flex flex-row mr-auto">
                  <div className="text-[#218bff] flex flex-row items-center gap-2">
                     <FaFilter />
                     <div>
                        Filters
                        {activeFilterCount > 0 && (
                           <span className="ml-1">({activeFilterCount})</span>
                        )}
                     </div>
                  </div>
                  <div className="mx-3 px-[18px] border-x-[1px] border-[#ccc] flex flex-row items-center">
                     <div>
                        {totalRecords}{' '}
                        {totalRecords === 1 ? 'Result' : 'Results'}
                     </div>
                  </div>
                  {activeFilterCount > 0 && (
                     <div className="flex flex-row items-center">
                        <Button
                           onClick={() => clearFilters()}
                           className="h-[22.5px] p-0 text-[15px] font-semibold border-b border-[#000] hover:text-blue-600 rounded-none bg-transparent text-black hover:bg-transparent hover:border-b-blue-500/50 transition-colors"
                        >
                           Clear Filters
                        </Button>
                     </div>
                  )}
               </div>
               <div className="flex flex-row gap-[50px]">
                  <div className="flex flex-row items-center gap-1">
                     <IoChatboxEllipsesOutline />
                     <div>Chat with an expert</div>
                  </div>
                  <Select
                     value={
                        filters._priceSort === 'ASC'
                           ? 'price-low-high'
                           : filters._priceSort === 'DESC'
                             ? 'price-high-low'
                             : 'recommended'
                     }
                     onValueChange={handleSortChange}
                  >
                     <SelectTrigger className="w-fit flex items-center justify-center border-none focus:ring-0">
                        <GoMultiSelect className="mr-2" />
                        <SelectValue placeholder="Recommended" />
                     </SelectTrigger>
                     <SelectContent className="bg-[#f7f7f7]">
                        <SelectGroup>
                           {/* <SelectItem value="recommended">
                              Recommended
                           </SelectItem>
                           <SelectItem value="newest">Newest</SelectItem> */}
                           <SelectItem value="price-low-high">
                              Price: Low to High
                           </SelectItem>
                           <SelectItem value="price-high-low">
                              Price: High to Low
                           </SelectItem>
                           {/* <SelectItem value="most-clicked">
                              Most Clicked
                           </SelectItem>
                           <SelectItem value="highest-rated">
                              Highest Rated
                           </SelectItem>
                           <SelectItem value="most-reviewed">
                              Most Reviewed
                           </SelectItem>
                           <SelectItem value="online-availability">
                              Online Availability
                           </SelectItem> */}
                        </SelectGroup>
                     </SelectContent>
                  </Select>
               </div>
            </div>
         </div>

         {/* MAIN CONTENT: FILTERS + PRODUCTS */}
         <div className="w-full max-w-[1440px] mx-auto px-5">
            <div className="flex flex-row gap-6 py-6">
               {/* Left: Filter Section */}
               <FilterSection />

               {/* Right: Products Grid */}
               <div className="flex-1">
                  <div className="w-full">
                     {/* Products will be displayed here */}
                     <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {/* TODO: Map products here when API is integrated */}
                        <div className="col-span-full text-center py-12 text-gray-500">
                           {isLoading
                              ? Array(5)
                                   .fill(0)
                                   .map((_, index) => (
                                      <div
                                         key={index}
                                         className="flex flex-row bg-white px-5 py-5 rounded-md"
                                      >
                                         {/* image */}
                                         <div className="image flex basis-[23%] items-center justify-center h-[300px] rounded-lg overflow-hidden">
                                            <Skeleton className="h-full w-full rounded-lg" />
                                         </div>

                                         {/* content */}
                                         <div className="flex flex-col relative basis-[40%] lg:basis-[30%] px-7">
                                            <div className="content flex flex-col gap-2">
                                               <h2 className="font-SFProText font-normal text-xl">
                                                  <Skeleton className="h-5 w-[350px]" />
                                               </h2>

                                               <span className="flex flex-row gap-2">
                                                  <p className="first-letter:uppercase text-sm">
                                                     colors:
                                                  </p>
                                                  <p className="first-letter:uppercase text-sm">
                                                     {/* ultramarine */}
                                                  </p>
                                               </span>

                                               {/* Colors */}
                                               <div className="flex flex-row gap-2">
                                                  <Skeleton className="h-5 w-[350px]" />
                                               </div>

                                               <span className="flex flex-row gap-2 mt-2">
                                                  <p className="first-letter:uppercase text-sm">
                                                     Storage:
                                                  </p>
                                               </span>

                                               {/* Storage */}
                                               <div className="flex flex-row gap-2">
                                                  <Skeleton className="h-5 w-[350px]" />
                                               </div>

                                               <Separator className="mt-2" />

                                               <span className="flex flex-row gap-2 mt-2 items-center">
                                                  <Skeleton className="h-5 w-[350px]" />
                                               </span>

                                               <span className="gap-2 mt-2 text-right">
                                                  <Skeleton className="h-5 w-[350px]" />
                                               </span>
                                            </div>
                                         </div>

                                         {/* feature */}
                                         <div className="flex flex-col justify-between flex-1 border-l-2 border-[#E5E7EB] px-4">
                                            <div className="flex flex-col gap-2">
                                               <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                                                  <Skeleton className="h-10 w-full" />
                                               </div>

                                               <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                                                  <Skeleton className="h-10 w-full" />
                                               </div>

                                               <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                                                  <Skeleton className="h-10 w-full" />
                                               </div>

                                               <div className="flex flex-row gap-4 items-center">
                                                  <Skeleton className="h-10 w-full" />
                                               </div>
                                            </div>

                                            <Skeleton className="h-12 w-full" />
                                         </div>
                                      </div>
                                   ))
                              : paginationItems.map((item) => (
                                   <div key={item.id}>
                                      <IphoneModel
                                         models={item.model_items}
                                         colors={item.color_items}
                                         storages={item.storage_items}
                                         averageRating={item.average_rating}
                                         skuPrices={item.sku_prices}
                                         modelSlug={item.slug}
                                      />
                                   </div>
                                ))}
                        </div>
                     </div>

                     {/* Pagination */}
                     {totalPages > 0 && (
                        <div className="mt-8 flex flex-col sm:flex-row items-center justify-between gap-4 pb-6">
                           {/* Page Info & Size Selector */}
                           <div className="flex items-center gap-4">
                              <div className="text-sm text-gray-600">
                                 Showing{' '}
                                 <span className="font-semibold">
                                    {(currentPage - 1) * pageSize + 1}
                                 </span>{' '}
                                 to{' '}
                                 <span className="font-semibold">
                                    {Math.min(
                                       currentPage * pageSize,
                                       totalRecords,
                                    )}
                                 </span>{' '}
                                 of{' '}
                                 <span className="font-semibold">
                                    {totalRecords}
                                 </span>{' '}
                                 results
                              </div>

                              <Select
                                 value={filters._limit?.toString() || '10'}
                                 onValueChange={handlePageSizeChange}
                              >
                                 <SelectTrigger className="w-[100px] h-9">
                                    <SelectValue />
                                 </SelectTrigger>
                                 <SelectContent>
                                    <SelectGroup>
                                       <SelectItem value="5">
                                          5 / page
                                       </SelectItem>
                                       <SelectItem value="10">
                                          10 / page
                                       </SelectItem>
                                    </SelectGroup>
                                 </SelectContent>
                              </Select>
                           </div>

                           {/* Pagination Controls */}
                           <div className="flex items-center gap-2">
                              {/* First Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() => handlePageChange(1)}
                                 disabled={isFirstPage}
                              >
                                 <ChevronsLeft className="h-4 w-4" />
                              </Button>

                              {/* Previous Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() =>
                                    handlePageChange(currentPage - 1)
                                 }
                                 disabled={!isPrevPage}
                              >
                                 <ChevronLeft className="h-4 w-4" />
                              </Button>

                              {/* Page Numbers */}
                              <div className="flex items-center gap-1">
                                 {getPageNumbers().map((page, index) => {
                                    const pageNum = page as number | string;
                                    if (pageNum === '...') {
                                       return (
                                          <span
                                             key={`ellipsis-${index}`}
                                             className="px-2 text-gray-400"
                                          >
                                             ...
                                          </span>
                                       );
                                    }

                                    return (
                                       <Button
                                          key={`page-${pageNum}`}
                                          variant={
                                             currentPage === pageNum
                                                ? 'default'
                                                : 'outline'
                                          }
                                          size="icon"
                                          className={cn(
                                             'h-9 w-9',
                                             currentPage === pageNum &&
                                                'bg-black text-white hover:bg-black/90',
                                          )}
                                          onClick={() =>
                                             handlePageChange(pageNum as number)
                                          }
                                       >
                                          {pageNum}
                                       </Button>
                                    );
                                 })}
                              </div>

                              {/* Next Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() =>
                                    handlePageChange(currentPage + 1)
                                 }
                                 disabled={!isNextPage}
                              >
                                 <ChevronRight className="h-4 w-4" />
                              </Button>

                              {/* Last Page */}
                              <Button
                                 variant="outline"
                                 size="icon"
                                 className="h-9 w-9"
                                 onClick={() => handlePageChange(totalPages)}
                                 disabled={isLastPage}
                              >
                                 <ChevronsRight className="h-4 w-4" />
                              </Button>
                           </div>
                        </div>
                     )}
                  </div>
               </div>
            </div>
         </div>

         {/* SUGGESTION PRODUCTS */}
         <div className="w-full px-4 py-8">
            <div className="mx-auto max-w-7xl">
               <h2 className="mb-8 text-center text-3xl font-semibold tracking-tight text-gray-900 dark:text-gray-100">
                  You might also like
               </h2>

               <Carousel
                  opts={{
                     align: 'start',
                     loop: true,
                  }}
                  className="w-full"
               >
                  <CarouselContent className="-ml-4">
                     {suggestionProductFakeData.items.map((product) => (
                        <CarouselItem
                           key={product.id}
                           className="pl-4 md:basis-1/2 lg:basis-1/3 xl:basis-1/4"
                        >
                           <SuggestionProduct product={product} />
                        </CarouselItem>
                     ))}
                  </CarouselContent>
                  <CarouselPrevious className="hidden md:flex" />
                  <CarouselNext className="hidden md:flex" />
               </Carousel>
            </div>
         </div>
      </div>
   );
};

export default IphoneShopPage;
