'use client';
import { motion } from 'framer-motion';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import '/globals.css';

import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '@components/ui/carousel';

import LatestItem from './_components/latest-item';

import CompareIPhoneSection from '@components/client/compare-iphone-section';
import { useDispatch } from 'react-redux';
import ExperienceItem from '@components/client/experience-item';
import NewestProduct from '@components/client/newest-product';
import PopularProduct from '@components/client/popular-product';

const listLatestItem = [
   {
      id: 1,
      checkPreOrder: false,
      title: 'iPhone 16 Pro',
      subtitle: 'Apple Intelligence',
      price: 'From $999 or $41.62/mo. for 24 mo.*',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-iphone-16-pro-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1726165763260',
      checkLightImg: false,
   },
   {
      id: 2,
      checkPreOrder: true,
      title: 'iPad mini',
      subtitle: 'Apple Intelligence',
      price: 'From $499 or $41.58/mo. for 12 mo.*',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-mini-202410?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1727814498187',
      checkLightImg: true,
   },
   {
      id: 3,
      checkPreOrder: false,
      title: 'Apple Watch Series 10',
      subtitle: 'Thinstant classic.',
      price: 'From $399 or $33.25/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-watch-s10-202409?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1724095131742',
      checkLightImg: true,
   },
   {
      id: 4,
      checkPreOrder: false,
      title: 'iPhone 16',
      subtitle: 'Apple Intelligence',
      price: 'From $799 or $33.29/mo. for 24 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-iphone-16-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1725661572513',
      checkLightImg: false,
   },
   {
      id: 5,
      checkPreOrder: false,
      title: 'AirPods 4',
      subtitle: 'Iconic. Now supersonic.',
      price: 'Starting at $129 with Active Noise Cancellation $179',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-airpods-202409?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1722974321259',
      checkLightImg: true,
   },
   {
      id: 6,
      checkPreOrder: false,
      title: 'Apple Watch Ultra 2',
      subtitle: 'New finish. Never quit.',
      price: 'From $799 or $66.58/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-watch-ultra-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1725655432734',
      checkLightImg: false,
   },
   {
      id: 7,
      checkPreOrder: false,
      title: 'Apple Vision Pro',
      subtitle: 'Welcome to spatial computing.',
      price: 'From $3499 or $291.58/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-vision-pro-202401?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1705097770616',
      checkLightImg: true,
   },
   {
      id: 8,
      checkPreOrder: false,
      title: 'iPad Air',
      subtitle: 'Apple Intelligence',
      price: 'From $599 or $49.91/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-air-202405?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1713308272877',
      checkLightImg: true,
   },
   {
      id: 9,
      checkPreOrder: false,
      title: 'MacBook Pro',
      subtitle: 'Mind-blowing. Head-turning.',
      price: 'From $1599 or $133.25/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-macbook-pro-202310?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1696964122967',
      checkLightImg: true,
   },
   {
      id: 10,
      checkPreOrder: false,
      title: 'iPad Pro',
      subtitle: 'Apple Intelligence',
      price: 'From $999 or $83.25/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-pro-202405?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1713308272816',
      checkLightImg: false,
   },
   {
      id: 11,
      checkPreOrder: false,
      title: 'MacBook Air',
      subtitle: 'Designed to go places.',
      price: 'From $999 or $83.25/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-macbook-air-202402?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1707259289595',
      checkLightImg: true,
   },
];

const listExperienceItem = [
   {
      id: 1,
      subtitle: 'Apple TV+',
      title: 'Watch new Apple Originals every month.**',
      content: '',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-tv-services-202409?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1727220094622',
      checkLightImg: true,
   },
   {
      id: 2,
      subtitle: '',
      title: 'Six Apple services. One easy subscription.',
      content: '',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-subscriptions-202108_GEO_US?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1626375546000',
      checkLightImg: true,
   },
   {
      id: 3,
      subtitle: '',
      title: "We've got you covered.",
      content:
         'AppleCare+ now comes with unlimited repairs for accidental damage protection.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-applecare-202409?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1723747544269',
      checkLightImg: true,
   },
   {
      id: 4,
      subtitle: '',
      title: 'Discover all the ways to use Apple Pay.',
      content: '',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-applepay-202409_GEO_US?wid=960&hei=1000&fmt=jpeg&qlt=90&.v=1725374577628',
      checkLightImg: true,
   },
   {
      id: 5,
      subtitle: 'home',
      title: 'See how one app can control your entire home.',
      content: '',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-homekit-202405_GEO_US?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1715960298510',
      checkLightImg: true,
   },
];

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
         is_newest: true,
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

export type TPopularProduct = (typeof fakePopularProductsData.items)[number];

export type TNewestProduct = (typeof fakeNewestProductsData.items)[number];

const HomePage = () => {
   const dispatch = useDispatch();

   //  const { data: iphonePromotions, isSuccess: isSuccessIphonePromotions } =
   //     useGetIphonePromotionsAsyncQuery();

   //  const {
   //     data: modelsDataAsync,
   //     isLoading: modelsDataIsLoading,
   //     isSuccess: modelsDataIsSuccess,
   //  } = useGetModelsAsyncQuery('');

   //  useEffect(() => {
   //     if (isSuccessIphonePromotions && modelsDataIsSuccess) {
   //        const links = modelsDataAsync.items.map((model) => {
   //           return {
   //              label: model.model_items.map((item) => {
   //                 return item.model_name;
   //              }),
   //              slug: model.model_slug,
   //           };
   //        });

   //        const products = iphonePromotions.items.map((item) => {
   //           const model = modelsDataAsync.items.find(
   //              (model) => model.model_id === item.product_model_id,
   //           );

   //           return {
   //              name: item.promotion_product_name
   //                 .replace(/p/g, 'P')
   //                 .replace(/g/g, 'G')
   //                 .replace(/b/g, 'B')
   //                 .replace(/t/g, 'T'),
   //              image: item.promotion_product_image,
   //              unit_price: item.promotion_product_unit_price,
   //              promotion_price: item.promotion_final_price,
   //              promotion_rate: item.promotion_discount_value,
   //              slug: model!.model_slug,
   //           };
   //        });

   //        dispatch(
   //           setSearchLinks(
   //              links.map((item) => {
   //                 return {
   //                    label: item.label.join(' and '),
   //                    slug: item.slug,
   //                 };
   //              }),
   //           ),
   //        );
   //        dispatch(setSearchProducts(products));
   //     }
   //  }, [iphonePromotions, modelsDataAsync]);

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col items-center justify-center bg-[#f5f5f7]',
         )}
      >
         <div className="hero-container max-w-[1264px] h-screen px-8 mb-[100px] mx-auto flex flex-row">
            {/* <div className='hero-gradient bg-gray-300 h-[100px] w-full absolute top-0 bottom-0 left-0 right-0 z-0'></div> */}
            <div className="hero-content basis-1/2 h-full bg-transparent relative z-10 flex flex-col items-start gap-[30px]">
               <div className="content-header max-w-[600px] mt-8 text-[94px] font-medium leading-[97px] tracking-[-3.76px] text-[#3a3a3a]">
                  Financial infrastructure to grow your revenue
               </div>
               <div className="content-body max-w-[540px] pl-2 pr-12 text-[18px] font-normal leading-[28px] tracking-[0.2px] text-[#425466]">
                  Join the millions of companies of all sizes that use Stripe to
                  accept payments online and in person, embed financial
                  services, power custom revenue models, and build a more
                  profitable business.
               </div>
            </div>
            <div className="hero-masonry basis-1/2 h-full bg-transparent relative z-10">
               <div className="bg-transparent h-full w-full absolute z-10">
                  <div className="bg-[linear-gradient(0deg,rgba(1,0,2,0),#f5f5f7)] h-10 w-full absolute top-0"></div>
                  <div className="bg-[linear-gradient(180deg,rgba(1,0,2,0),#f5f5f7)] h-10 w-full absolute bottom-0"></div>
               </div>
               <div className="overflow-hidden h-full relative z-0 px-1">
                  <motion.div
                     animate={{ y: ['0px', '-3384px'] }}
                     transition={{
                        y: {
                           duration: 60,
                           repeat: Infinity,
                           ease: 'linear',
                        },
                     }}
                     className="preview-masonry bg-transparent max-w-[600px] mx-auto"
                  >
                     <div className="grid grid-cols-[repeat(2,minmax(100px,285px))] grid-rows-[masonry] grid-flow-dense gap-[20px]">
                        <div className="bg-[url('/images/hero-imgs/picture01-half.jpg')] bg-cover bg-no-repeat bg-bottom rounded-[0_0_20px_20px] h-[168px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture02.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture03.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture04.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture05.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture06.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture07.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture08.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture09.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture10.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture11.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture12.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture13.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture14.jpg')] bg-cover bg-no-repeat bg-top row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture15.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture16.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture17.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture18.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture01.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture02.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture03.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture04.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture05.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture06.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                     </div>
                  </motion.div>
               </div>
            </div>
         </div>

         {/* NEWEST PRODUCTS */}
         <div className="w-full px-4 py-8">
            <div className="mx-auto max-w-7xl">
               <h2 className="mb-8 text-center text-3xl font-semibold tracking-tight text-gray-900 dark:text-gray-100">
                  Newest Products
               </h2>

               <Carousel
                  opts={{
                     align: 'start',
                     loop: true,
                  }}
                  className="w-full"
               >
                  <CarouselContent className="-ml-4">
                     {fakeNewestProductsData.items.map((product) => (
                        <CarouselItem
                           key={product.id}
                           className="pl-4 md:basis-1/2 lg:basis-1/2 xl:basis-1/3"
                        >
                           <NewestProduct product={product} />
                        </CarouselItem>
                     ))}
                  </CarouselContent>
                  <CarouselPrevious className="hidden md:flex" />
                  <CarouselNext className="hidden md:flex" />
               </Carousel>
            </div>
         </div>

         {/* POPULAR PRODUCTS */}
         <div className="w-full px-4 py-8">
            <div className="mx-auto max-w-7xl">
               <h2 className="mb-8 text-center text-3xl font-semibold tracking-tight text-gray-900 dark:text-gray-100">
                  Popular Products
               </h2>

               <Carousel
                  opts={{
                     align: 'start',
                     loop: true,
                  }}
                  className="w-full"
               >
                  <CarouselContent className="-ml-4">
                     {fakePopularProductsData.items.map((product) => (
                        <CarouselItem
                           key={product.id}
                           className="pl-4 md:basis-1/2 lg:basis-1/2 xl:basis-1/3"
                        >
                           <PopularProduct product={product} />
                        </CarouselItem>
                     ))}
                  </CarouselContent>
                  <CarouselPrevious className="hidden md:flex" />
                  <CarouselNext className="hidden md:flex" />
               </Carousel>
            </div>
         </div>

         <div className="list-item-container w-full mb-[100px] flex flex-col justify-center items-center">
            <div className="lastest-item-container w-full max-w-screen mb-[50px]">
               <div className="title-list pl-[140px] pb-[24px]">
                  <span className="text-[28px] font-semibold text-[#1d1d1f] leading-[1.14] tracking-[0.007em]">
                     The latest.{' '}
                  </span>
                  <span className="text-[28px] font-semibold text-[#6e6e73] leading-[1.14] tracking-[0.007em]">
                     Take a look at what’s new, right now.
                  </span>
               </div>
               <Carousel className="w-full max-w-screen relative flex justify-center items-center">
                  <CarouselContent className="w-screen pl-[140px] mb-5">
                     {listLatestItem.map((product, index) => {
                        return (
                           <CarouselItem
                              key={index}
                              className="lg:basis-[30%] mr-[0px]"
                           >
                              <LatestItem product={product} />
                           </CarouselItem>
                        );
                     })}
                  </CarouselContent>
                  <CarouselPrevious className="absolute -top-[40px] -translate-y-1/2 left-[90%] border-black bg-[#ccc]" />
                  <CarouselNext className="absolute -top-[40px] -translate-y-1/2 right-[5%] border-black bg-[#ccc]" />
               </Carousel>
            </div>

            <div className="experience-container w-full max-w-screen">
               <div className="title-list pl-[140px] pb-[24px]">
                  <span className="text-[28px] font-semibold text-[#1d1d1f] leading-[1.14] tracking-[0.007em]">
                     The Apple experience.{' '}
                  </span>
                  <span className="text-[28px] font-semibold text-[#6e6e73] leading-[1.14] tracking-[0.007em]">
                     Do even more with Apple products and services.
                  </span>
               </div>
               <Carousel className="w-full max-w-screen relative flex justify-center items-center">
                  <CarouselContent className="w-screen pl-[140px] mb-5">
                     {listExperienceItem.map((item, index) => {
                        return (
                           <CarouselItem
                              key={index}
                              className="md:basis-[30%] lg:basis-[36%] mr-[0px]"
                           >
                              <ExperienceItem experience={item} />
                           </CarouselItem>
                        );
                     })}
                  </CarouselContent>
                  <CarouselPrevious className="absolute -top-[40px] -translate-y-1/2 left-[90%] border-black bg-[#ccc]" />
                  <CarouselNext className="absolute -top-[40px] -translate-y-1/2 right-[5%] border-black bg-[#ccc]" />
               </Carousel>
            </div>
         </div>
         <CompareIPhoneSection />
      </div>
   );
};

export default HomePage;
