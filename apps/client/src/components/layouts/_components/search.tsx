'use client';

import { MdArrowRightAlt } from 'react-icons/md';
import { motion } from 'framer-motion';
import svgs from '@assets/svgs';
import Image from 'next/image';
import { useEffect, useState } from 'react';
import { useDebounce } from '@components/hooks/use-debounce';
import { useRouter } from 'next/navigation';
import useProductService from '@components/hooks/api/use-product-service';
import usePagination from '@components/hooks/use-pagination';
import { Badge } from '@components/ui/badge';
import { LoadingOverlay } from '@components/client/loading-overlay';

// Staggered animation variants
const containerVariants = {
   hidden: {},
   visible: {
      transition: {
         staggerChildren: 0.1,
         delayChildren: 0.1,
      },
   },
};

const fakeSearchProductsData = {
   total_records: 2,
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
         promotion: {
            promotion_type: 'INSTANT_DISCOUNT',
            promotion_discount_type: 'PERCENTAGE',
            promotion_discount_value: 0.1,
            promotion_discount_amount: 130,
            final_price: 1170,
         },
         is_newest: true,
         slug: 'iphone-15',
      },
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
         promotion: null,
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

export type TSearchProductsItem = (typeof fakeSearchProductsData.items)[number];

const Search = () => {
   const router = useRouter();
   const [query, setQuery] = useState('');
   const [isSearchOpen, setIsSearchOpen] = useState(false);
   const debouncedQuery = useDebounce(query, 500);

   const { getProductsAsync, getProductsState, isLoading } =
      useProductService();

   const { paginationItems } = usePagination(
      getProductsState.isSuccess &&
         getProductsState.data &&
         getProductsState.data.items.length > 0
         ? getProductsState.data
         : fakeSearchProductsData,
   );

   useEffect(() => {
      const fetchProducts = async () => {
         const result = await getProductsAsync({
            _textSearch: debouncedQuery,
         });

         console.log(result);
      };

      if (debouncedQuery.length > 2) {
         fetchProducts();
      }
   }, [debouncedQuery, getProductsAsync]);

   return (
      <motion.div
         className="sub-category absolute top-[44px] left-0 w-full bg-[#fafafc] text-black z-50"
         initial={{ height: 0, opacity: 0 }}
         animate={{
            height: 'auto',
            opacity: 1,
            transition: {
               height: { duration: 0.5 },
               opacity: { duration: 0.3, delay: 0.1 },
            },
         }}
         exit={{
            height: 0,
            opacity: 0,
            transition: {
               height: { duration: 0.6 },
               opacity: { duration: 0.3 },
            },
         }}
      >
         <div className="py-8">
            <motion.div
               className="mx-auto w-[980px]"
               variants={containerVariants}
               initial="hidden"
               animate="visible"
            >
               <div className="font-SFProText">
                  <div className="flex items-center pb-5">
                     <Image
                        src={svgs.appleSearchIcon}
                        alt="cover"
                        width={1200}
                        height={1000}
                        quality={100}
                        className="w-[60px] h-[60px]"
                     />
                     <input
                        type="text"
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        onFocus={() => setIsSearchOpen(true)}
                        className="w-full h-10 bg-[#fafafc] outline-none text-2xl font-medium font-SFProText"
                        placeholder="Search ygzStore.com"
                     />
                  </div>
               </div>
               <LoadingOverlay isLoading={isLoading}>
                  <div>
                     {paginationItems.length > 0 && (
                        <ul className="py-2 flex flex-col gap-2 max-h-[200px] overflow-y-auto">
                           {paginationItems.length > 0 && (
                              <h3 className="text-xs text-slate-500 font-SFProText mb-5">
                                 Suggested searches for &quot;
                                 {debouncedQuery}
                                 &quot;
                              </h3>
                           )}

                           {paginationItems.map(
                              (item: TSearchProductsItem, index) => {
                                 const prices = item.skuPrices.map(
                                    (sku: any) => sku.unit_price,
                                 );
                                 const minPrice = Math.min(...prices);
                                 const maxPrice = Math.max(...prices);
                                 const displayName = item.modelItems
                                    .map((model: any) => model.name)
                                    .join(' & ');
                                 const hasPromotion =
                                    item.promotion &&
                                    item.promotion.final_price;

                                 return (
                                    <div key={index} className="">
                                       <li
                                          className="flex items-center gap-3 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:bg-gray-100 p-2 rounded-lg transition-colors"
                                          onClick={() => {
                                             router.push(`/shop/${item.slug}`);
                                          }}
                                       >
                                          <div className="w-[60px] h-[60px] overflow-hidden flex-shrink-0">
                                             <Image
                                                src={
                                                   item.showcaseImages[0]
                                                      .image_url
                                                }
                                                alt={displayName}
                                                width={500}
                                                height={500}
                                                className="h-[180%] w-full object-cover translate-y-[-24px]"
                                             />
                                          </div>

                                          <div className="flex flex-col">
                                             <p className="text-sm font-semibold mb-1">
                                                {displayName}
                                             </p>
                                             <div className="flex items-center gap-2">
                                                {hasPromotion ? (
                                                   <>
                                                      <span className="text-base font-bold text-red-600">
                                                         $
                                                         {item.promotion.final_price.toLocaleString()}
                                                      </span>
                                                      <span className="line-through text-sm text-gray-500">
                                                         $
                                                         {minPrice.toLocaleString()}
                                                      </span>
                                                      <span className="text-sm text-red-600">
                                                         -
                                                         {(
                                                            item.promotion
                                                               .promotion_discount_value *
                                                            100
                                                         ).toFixed(0)}
                                                         %
                                                      </span>
                                                   </>
                                                ) : (
                                                   <p className="text-base font-bold">
                                                      {minPrice === maxPrice
                                                         ? `$${minPrice.toLocaleString()}`
                                                         : `$${minPrice.toLocaleString()} - $${maxPrice.toLocaleString()}`}
                                                   </p>
                                                )}
                                             </div>
                                             {hasPromotion && (
                                                <Badge
                                                   variant="destructive"
                                                   className="mt-1 text-xs w-fit"
                                                >
                                                   On promotion
                                                </Badge>
                                             )}
                                          </div>
                                       </li>
                                    </div>
                                 );
                              },
                           )}
                        </ul>
                     )}

                     <h3 className="text-xs text-slate-500 font-SFProText">
                        Quick links
                     </h3>

                     <ul className="pt-2">
                        <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                           <MdArrowRightAlt className="size-4" />
                           <p>Find a Store</p>
                        </li>
                        <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                           <MdArrowRightAlt className="size-4" />
                           <p>Apple Vision Pro</p>
                        </li>
                        <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                           <MdArrowRightAlt className="size-4" />
                           <p>HeadPhones</p>
                        </li>
                        <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                           <MdArrowRightAlt
                              className="size-4"
                              aria-hidden="true"
                           />
                           <p>Apple Intelligence</p>
                        </li>
                     </ul>
                  </div>
               </LoadingOverlay>
            </motion.div>
         </div>
      </motion.div>
   );
};

export default Search;
