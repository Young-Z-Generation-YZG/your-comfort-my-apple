'use client';

import {
   ColumnDef,
   flexRender,
   getCoreRowModel,
   getSortedRowModel,
   SortingState,
   useReactTable,
   VisibilityState,
   RowSelectionState,
} from '@tanstack/react-table';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   DropdownMenu,
   DropdownMenuCheckboxItem,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuLabel,
   DropdownMenuSeparator,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import {
   ArrowUpDown,
   ChevronDown,
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
   MoreHorizontal,
   Package,
} from 'lucide-react';
import { cn } from '~/src/infrastructure/lib/utils';
import useFilter from '~/src/hooks/use-filter';
import { LoadingOverlay } from '@components/loading-overlay';
import useInventoryService from '~/src/hooks/api/use-inventory-service';
import { useEffect, useMemo, useState } from 'react';
import usePagination from '~/src/hooks/use-pagination';
import Image from 'next/image';

const fakeData = {
   total_records: 40,
   total_pages: 1,
   page_size: 40,
   current_page: 1,
   items: [
      {
         id: '68f71b4a574ee8ac95e48e1f',
         code: 'IPHONE-IPHONE_15-128GB-YELLOW',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'YELLOW',
            normalized_name: 'YELLOW',
            hex_code: '#EDE6C8',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
            order: 2,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-128gb-yellow',
         created_at: '2025-10-21T05:34:02.499Z',
         updated_at: '2025-10-21T05:34:02.499Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e21',
         code: 'IPHONE-IPHONE_15-512GB-YELLOW',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'YELLOW',
            normalized_name: 'YELLOW',
            hex_code: '#EDE6C8',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
            order: 2,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-512gb-yellow',
         created_at: '2025-10-21T05:34:02.501Z',
         updated_at: '2025-10-21T05:34:02.501Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e37',
         code: 'IPHONE-IPHONE_15_PLUS-128GB-GREEN',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'GREEN',
            normalized_name: 'GREEN',
            hex_code: '#D0D9CA',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
            order: 3,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-128gb-green',
         created_at: '2025-10-21T05:34:02.516Z',
         updated_at: '2025-10-21T05:34:02.516Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e38',
         code: 'IPHONE-IPHONE_15_PLUS-256GB-GREEN',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'GREEN',
            normalized_name: 'GREEN',
            hex_code: '#D0D9CA',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
            order: 3,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-256gb-green',
         created_at: '2025-10-21T05:34:02.517Z',
         updated_at: '2025-10-21T05:34:02.517Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e2c',
         code: 'IPHONE-IPHONE_15_PLUS-256GB-BLUE',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-256gb-blue',
         created_at: '2025-10-21T05:34:02.509Z',
         updated_at: '2025-10-21T05:34:02.509Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e1b',
         code: 'IPHONE-IPHONE_15-128GB-PINK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'PINK',
            normalized_name: 'PINK',
            hex_code: '#EBD3D4',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
            order: 1,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-128gb-pink',
         created_at: '2025-10-21T05:34:02.496Z',
         updated_at: '2025-10-21T05:34:02.496Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e26',
         code: 'IPHONE-IPHONE_15-1TB-GREEN',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'GREEN',
            normalized_name: 'GREEN',
            hex_code: '#D0D9CA',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
            order: 3,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-1tb-green',
         created_at: '2025-10-21T05:34:02.505Z',
         updated_at: '2025-10-21T05:34:02.505Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e1d',
         code: 'IPHONE-IPHONE_15-512GB-PINK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'PINK',
            normalized_name: 'PINK',
            hex_code: '#EBD3D4',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
            order: 1,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-512gb-pink',
         created_at: '2025-10-21T05:34:02.498Z',
         updated_at: '2025-10-21T05:34:02.498Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e1e',
         code: 'IPHONE-IPHONE_15-1TB-PINK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'PINK',
            normalized_name: 'PINK',
            hex_code: '#EBD3D4',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
            order: 1,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-1tb-pink',
         created_at: '2025-10-21T05:34:02.499Z',
         updated_at: '2025-10-21T05:34:02.499Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e3d',
         code: 'IPHONE-IPHONE_15_PLUS-512GB-BLACK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'BLACK',
            normalized_name: 'BLACK',
            hex_code: '#4B4F50',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
            order: 4,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-512gb-black',
         created_at: '2025-10-21T05:34:02.52Z',
         updated_at: '2025-10-21T05:34:02.52Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e35',
         code: 'IPHONE-IPHONE_15_PLUS-512GB-YELLOW',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'YELLOW',
            normalized_name: 'YELLOW',
            hex_code: '#EDE6C8',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
            order: 2,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-512gb-yellow',
         created_at: '2025-10-21T05:34:02.515Z',
         updated_at: '2025-10-21T05:34:02.515Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e3b',
         code: 'IPHONE-IPHONE_15_PLUS-128GB-BLACK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'BLACK',
            normalized_name: 'BLACK',
            hex_code: '#4B4F50',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
            order: 4,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-128gb-black',
         created_at: '2025-10-21T05:34:02.519Z',
         updated_at: '2025-10-21T05:34:02.519Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e1c',
         code: 'IPHONE-IPHONE_15-256GB-PINK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'PINK',
            normalized_name: 'PINK',
            hex_code: '#EBD3D4',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
            order: 1,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-256gb-pink',
         created_at: '2025-10-21T05:34:02.497Z',
         updated_at: '2025-10-21T05:34:02.497Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e18',
         code: 'IPHONE-IPHONE_15-256GB-BLUE',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-256gb-blue',
         created_at: '2025-10-21T05:34:02.491Z',
         updated_at: '2025-10-21T05:34:02.491Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e34',
         code: 'IPHONE-IPHONE_15_PLUS-256GB-YELLOW',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'YELLOW',
            normalized_name: 'YELLOW',
            hex_code: '#EDE6C8',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
            order: 2,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-256gb-yellow',
         created_at: '2025-10-21T05:34:02.515Z',
         updated_at: '2025-10-21T05:34:02.515Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e3a',
         code: 'IPHONE-IPHONE_15_PLUS-1TB-GREEN',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'GREEN',
            normalized_name: 'GREEN',
            hex_code: '#D0D9CA',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
            order: 3,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-1tb-green',
         created_at: '2025-10-21T05:34:02.518Z',
         updated_at: '2025-10-21T05:34:02.518Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e3c',
         code: 'IPHONE-IPHONE_15_PLUS-256GB-BLACK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'BLACK',
            normalized_name: 'BLACK',
            hex_code: '#4B4F50',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
            order: 4,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-256gb-black',
         created_at: '2025-10-21T05:34:02.519Z',
         updated_at: '2025-10-21T05:34:02.519Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e24',
         code: 'IPHONE-IPHONE_15-256GB-GREEN',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'GREEN',
            normalized_name: 'GREEN',
            hex_code: '#D0D9CA',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
            order: 3,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-256gb-green',
         created_at: '2025-10-21T05:34:02.504Z',
         updated_at: '2025-10-21T05:34:02.504Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e1a',
         code: 'IPHONE-IPHONE_15-1TB-BLUE',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-1tb-blue',
         created_at: '2025-10-21T05:34:02.495Z',
         updated_at: '2025-10-21T05:34:02.495Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e27',
         code: 'IPHONE-IPHONE_15-128GB-BLACK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'BLACK',
            normalized_name: 'BLACK',
            hex_code: '#4B4F50',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
            order: 4,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-128gb-black',
         created_at: '2025-10-21T05:34:02.506Z',
         updated_at: '2025-10-21T05:34:02.506Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e33',
         code: 'IPHONE-IPHONE_15_PLUS-128GB-YELLOW',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'YELLOW',
            normalized_name: 'YELLOW',
            hex_code: '#EDE6C8',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
            order: 2,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-128gb-yellow',
         created_at: '2025-10-21T05:34:02.514Z',
         updated_at: '2025-10-21T05:34:02.514Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e17',
         code: 'IPHONE-IPHONE_15-128GB-BLUE',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: {
            event_id: '611db6eb-3d64-474e-9e23-3517ad0df6ec',
            event_item_id: '99a356c8-c026-4137-8820-394763f30521',
            event_name: '',
            reserved_quantity: 40,
         },
         state: 'ACTIVE',
         slug: 'iphone-iphone15-128gb-blue',
         created_at: '2025-10-21T05:34:02.487Z',
         updated_at: '2025-10-21T05:34:02.487Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e19',
         code: 'IPHONE-IPHONE_15-512GB-BLUE',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-512gb-blue',
         created_at: '2025-10-21T05:34:02.493Z',
         updated_at: '2025-10-21T05:34:02.493Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e2d',
         code: 'IPHONE-IPHONE_15_PLUS-512GB-BLUE',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-512gb-blue',
         created_at: '2025-10-21T05:34:02.51Z',
         updated_at: '2025-10-21T05:34:02.51Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e2e',
         code: 'IPHONE-IPHONE_15_PLUS-1TB-BLUE',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-1tb-blue',
         created_at: '2025-10-21T05:34:02.511Z',
         updated_at: '2025-10-21T05:34:02.511Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e36',
         code: 'IPHONE-IPHONE_15_PLUS-1TB-YELLOW',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'YELLOW',
            normalized_name: 'YELLOW',
            hex_code: '#EDE6C8',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
            order: 2,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-1tb-yellow',
         created_at: '2025-10-21T05:34:02.516Z',
         updated_at: '2025-10-21T05:34:02.516Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e22',
         code: 'IPHONE-IPHONE_15-1TB-YELLOW',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'YELLOW',
            normalized_name: 'YELLOW',
            hex_code: '#EDE6C8',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
            order: 2,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-1tb-yellow',
         created_at: '2025-10-21T05:34:02.503Z',
         updated_at: '2025-10-21T05:34:02.503Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e29',
         code: 'IPHONE-IPHONE_15-512GB-BLACK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'BLACK',
            normalized_name: 'BLACK',
            hex_code: '#4B4F50',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
            order: 4,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-512gb-black',
         created_at: '2025-10-21T05:34:02.507Z',
         updated_at: '2025-10-21T05:34:02.507Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e20',
         code: 'IPHONE-IPHONE_15-256GB-YELLOW',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'YELLOW',
            normalized_name: 'YELLOW',
            hex_code: '#EDE6C8',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-yellow_pwviwe',
            order: 2,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-256gb-yellow',
         created_at: '2025-10-21T05:34:02.501Z',
         updated_at: '2025-10-21T05:34:02.501Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e28',
         code: 'IPHONE-IPHONE_15-256GB-BLACK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'BLACK',
            normalized_name: 'BLACK',
            hex_code: '#4B4F50',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
            order: 4,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-256gb-black',
         created_at: '2025-10-21T05:34:02.507Z',
         updated_at: '2025-10-21T05:34:02.507Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e2a',
         code: 'IPHONE-IPHONE_15-1TB-BLACK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'BLACK',
            normalized_name: 'BLACK',
            hex_code: '#4B4F50',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
            order: 4,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-1tb-black',
         created_at: '2025-10-21T05:34:02.508Z',
         updated_at: '2025-10-21T05:34:02.508Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e2b',
         code: 'IPHONE-IPHONE_15_PLUS-128GB-BLUE',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'BLUE',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-128gb-blue',
         created_at: '2025-10-21T05:34:02.508Z',
         updated_at: '2025-10-21T05:34:02.508Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e31',
         code: 'IPHONE-IPHONE_15_PLUS-512GB-PINK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'PINK',
            normalized_name: 'PINK',
            hex_code: '#EBD3D4',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
            order: 1,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-512gb-pink',
         created_at: '2025-10-21T05:34:02.513Z',
         updated_at: '2025-10-21T05:34:02.513Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e39',
         code: 'IPHONE-IPHONE_15_PLUS-512GB-GREEN',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'GREEN',
            normalized_name: 'GREEN',
            hex_code: '#D0D9CA',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
            order: 3,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-512gb-green',
         created_at: '2025-10-21T05:34:02.518Z',
         updated_at: '2025-10-21T05:34:02.518Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e3e',
         code: 'IPHONE-IPHONE_15_PLUS-1TB-BLACK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'BLACK',
            normalized_name: 'BLACK',
            hex_code: '#4B4F50',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-black_hhhvfs',
            order: 4,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-1tb-black',
         created_at: '2025-10-21T05:34:02.52Z',
         updated_at: '2025-10-21T05:34:02.52Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e23',
         code: 'IPHONE-IPHONE_15-128GB-GREEN',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'GREEN',
            normalized_name: 'GREEN',
            hex_code: '#D0D9CA',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
            order: 3,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-128gb-green',
         created_at: '2025-10-21T05:34:02.504Z',
         updated_at: '2025-10-21T05:34:02.504Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e25',
         code: 'IPHONE-IPHONE_15-512GB-GREEN',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'GREEN',
            normalized_name: 'GREEN',
            hex_code: '#D0D9CA',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-green_yk0ln5',
            order: 3,
         },
         storage: {
            name: '512GB',
            normalized_name: '512GB',
            order: 2,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp',
         unit_price: 1200,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15-512gb-green',
         created_at: '2025-10-21T05:34:02.505Z',
         updated_at: '2025-10-21T05:34:02.505Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e2f',
         code: 'IPHONE-IPHONE_15_PLUS-128GB-PINK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'PINK',
            normalized_name: 'PINK',
            hex_code: '#EBD3D4',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
            order: 1,
         },
         storage: {
            name: '128GB',
            normalized_name: '128GB',
            order: 0,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
         unit_price: 1000,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-128gb-pink',
         created_at: '2025-10-21T05:34:02.511Z',
         updated_at: '2025-10-21T05:34:02.511Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e30',
         code: 'IPHONE-IPHONE_15_PLUS-256GB-PINK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'PINK',
            normalized_name: 'PINK',
            hex_code: '#EBD3D4',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
            order: 1,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
         unit_price: 1100,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-256gb-pink',
         created_at: '2025-10-21T05:34:02.512Z',
         updated_at: '2025-10-21T05:34:02.512Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
      {
         id: '68f71b4a574ee8ac95e48e32',
         code: 'IPHONE-IPHONE_15_PLUS-1TB-PINK',
         model_id: '664351e90087aa09993f5ae7',
         tenant_id: '664355f845e56534956be32b',
         branch_id: '664357a235e84033bbd0e6b6',
         product_classification: 'IPHONE',
         model: {
            name: 'IPHONE_15_PLUS',
            normalized_name: 'IPHONE_15_PLUS',
            order: 1,
         },
         color: {
            name: 'PINK',
            normalized_name: 'PINK',
            hex_code: '#EBD3D4',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-pink_j6v96t',
            order: 1,
         },
         storage: {
            name: '1TB',
            normalized_name: '1TB',
            order: 3,
         },
         display_image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp',
         unit_price: 1300,
         available_in_stock: 0,
         total_sold: 0,
         reserved_for_event: null,
         state: 'ACTIVE',
         slug: 'iphone-iphone15plus-1tb-pink',
         created_at: '2025-10-21T05:34:02.514Z',
         updated_at: '2025-10-21T05:34:02.514Z',
         deleted_at: null,
         deleted_by: null,
         is_deleted: false,
      },
   ],
   links: {
      first: '?_page=1&_limit=40',
      prev: null,
      next: null,
      last: '?_page=1&_limit=40',
   },
};

export type TSkuItem = (typeof fakeData.items)[number];

type ISkuFilter = {
   _page?: number | null;
   _limit?: number | null;
   _colors?: string[] | null;
   _storages?: string[] | null;
   _models?: string[] | null;
};

// Helper function to get state badge styles
const getStateStyle = (state: string) => {
   switch (state) {
      case 'ACTIVE':
         return 'bg-green-100 text-green-800 border-green-300';
      case 'INACTIVE':
         return 'bg-gray-100 text-gray-800 border-gray-300';
      case 'OUT_OF_STOCK':
         return 'bg-red-100 text-red-800 border-red-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const columns: ColumnDef<TSkuItem>[] = [
   {
      accessorKey: 'display_image_url',
      header: 'Image',
      cell: ({ row }) => {
         const imageUrl = row.getValue('display_image_url') as string;
         return (
            <div className="flex items-center justify-center">
               {imageUrl ? (
                  <Image
                     src={imageUrl}
                     alt={row.original.code}
                     width={48}
                     height={48}
                     className="rounded-md object-cover"
                  />
               ) : (
                  <div className="flex h-12 w-12 items-center justify-center rounded-md bg-gray-100">
                     <Package className="h-6 w-6 text-gray-400" />
                  </div>
               )}
            </div>
         );
      },
   },
   {
      accessorKey: 'code',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               SKU Code
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => (
         <div className="font-mono text-sm font-medium">
            {row.getValue('code')}
         </div>
      ),
   },
   {
      accessorKey: 'model.name',
      header: 'Model',
      cell: ({ row }) => (
         <div className="font-medium">
            {row.original.model?.name?.replace(/_/g, ' ') || '-'}
         </div>
      ),
   },
   {
      accessorKey: 'color.name',
      header: 'Color',
      cell: ({ row }) => {
         const colorName = row.original.color?.name || 'N/A';
         const hexCode = row.original.color?.hex_code;
         return (
            <div className="flex items-center gap-2">
               {hexCode && (
                  <div
                     className="h-6 w-6 rounded-full border-2 border-gray-300"
                     style={{ backgroundColor: hexCode }}
                  />
               )}
               <span className="capitalize">{colorName.toLowerCase()}</span>
            </div>
         );
      },
   },
   {
      accessorKey: 'storage.name',
      header: 'Storage',
      cell: ({ row }) => (
         <Badge variant="outline" className="font-semibold">
            {row.original.storage?.name || '-'}
         </Badge>
      ),
   },
   {
      accessorKey: 'unit_price',
      header: ({ column }) => {
         return (
            <Button
               variant="ghost"
               onClick={() =>
                  column.toggleSorting(column.getIsSorted() === 'asc')
               }
            >
               Unit Price
               <ArrowUpDown className="ml-2 h-4 w-4" />
            </Button>
         );
      },
      cell: ({ row }) => {
         const price = parseFloat(row.getValue('unit_price'));
         const formatted = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
         }).format(price);
         return <div className="font-medium">{formatted}</div>;
      },
   },
   {
      accessorKey: 'available_in_stock',
      header: 'In Stock',
      cell: ({ row }) => {
         const stock = row.getValue('available_in_stock') as number;
         return (
            <div
               className={cn(
                  'text-center font-semibold',
                  stock === 0
                     ? 'text-red-600'
                     : stock < 10
                       ? 'text-yellow-600'
                       : 'text-green-600',
               )}
            >
               {stock}
            </div>
         );
      },
   },
   {
      accessorKey: 'total_sold',
      header: 'Total Sold',
      cell: ({ row }) => (
         <div className="text-center">{row.getValue('total_sold')}</div>
      ),
   },
   {
      id: 'reserved_quantity',
      header: 'Reserved',
      cell: ({ row }) => {
         const reservedEvent = row.original.reserved_for_event;
         if (!reservedEvent) {
            return <div className="text-center text-gray-400">-</div>;
         }
         return (
            <div className="text-center">
               <Badge
                  variant="outline"
                  className="bg-orange-100 text-orange-800 border-orange-300"
               >
                  {reservedEvent.reserved_quantity}
               </Badge>
            </div>
         );
      },
   },
   {
      accessorKey: 'state',
      header: 'Status',
      cell: ({ row }) => {
         const state = row.getValue('state') as string;
         return (
            <Badge
               variant="outline"
               className={cn('capitalize', getStateStyle(state))}
            >
               {state.toLowerCase().replace(/_/g, ' ')}
            </Badge>
         );
      },
   },
   {
      id: 'actions',
      enableHiding: false,
      cell: ({ row }) => {
         const sku = row.original;

         return (
            <DropdownMenu>
               <DropdownMenuTrigger asChild>
                  <Button variant="ghost" className="h-8 w-8 p-0">
                     <span className="sr-only">Open menu</span>
                     <MoreHorizontal className="h-4 w-4" />
                  </Button>
               </DropdownMenuTrigger>
               <DropdownMenuContent align="end">
                  <DropdownMenuLabel>Actions</DropdownMenuLabel>
                  <DropdownMenuItem
                     onClick={() => navigator.clipboard.writeText(sku.id)}
                  >
                     Copy SKU ID
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem>View details</DropdownMenuItem>
                  <DropdownMenuItem>Edit SKU</DropdownMenuItem>
                  <DropdownMenuItem>Adjust stock</DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem className="text-destructive">
                     Delete SKU
                  </DropdownMenuItem>
               </DropdownMenuContent>
            </DropdownMenu>
         );
      },
   },
];

const WarehousesPage = () => {
   const { getWarehousesAsync, getWarehousesState, isLoading } =
      useInventoryService();

   const { filters, setFilters } = useFilter<ISkuFilter>();

   const [sorting, setSorting] = useState<SortingState>([]);
   const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
      {},
   );
   const [rowSelection, setRowSelection] = useState<RowSelectionState>({});

   // Get warehouse data from state or use empty array
   const warehouseData = useMemo(() => {
      return getWarehousesState.data?.items || [];
   }, [getWarehousesState.data]);

   const {
      currentPage,
      totalPages,
      pageSize,
      totalRecords,
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,
      getPageNumbers,
   } = usePagination(
      getWarehousesState.data && getWarehousesState.data.items.length > 0
         ? getWarehousesState.data
         : {
              total_records: 0,
              total_pages: 0,
              page_size: 0,
              current_page: 0,
              items: [],
              links: {
                 first: null,
                 last: null,
                 prev: null,
                 next: null,
              },
           },
   );

   // Setup table
   const table = useReactTable({
      data: warehouseData,
      columns,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      onSortingChange: setSorting,
      onColumnVisibilityChange: setColumnVisibility,
      onRowSelectionChange: setRowSelection,
      state: {
         sorting,
         columnVisibility,
         rowSelection,
      },
      manualPagination: true,
      pageCount: totalPages,
   });

   useEffect(() => {
      const fetchWarehouses = async () => {
         await getWarehousesAsync(filters);
      };

      fetchWarehouses();
   }, [filters, getWarehousesAsync]);

   return (
      <div className="p-5">
         <div className="flex items-center justify-between">
            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Warehouse Management
               </h1>
               <p className="text-muted-foreground">
                  Manage inventory and SKU items across all warehouses
               </p>
            </div>
         </div>

         <LoadingOverlay isLoading={isLoading}>
            {/* Column Visibility Toggle */}
            <div className="flex items-center justify-end py-4">
               <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                     <Button variant="outline" className="ml-auto">
                        Columns <ChevronDown />
                     </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                     {table
                        .getAllColumns()
                        .filter((column) => column.getCanHide())
                        .map((column) => {
                           return (
                              <DropdownMenuCheckboxItem
                                 key={column.id}
                                 className="capitalize"
                                 checked={column.getIsVisible()}
                                 onCheckedChange={(value) =>
                                    column.toggleVisibility(!!value)
                                 }
                              >
                                 {column.id}
                              </DropdownMenuCheckboxItem>
                           );
                        })}
                  </DropdownMenuContent>
               </DropdownMenu>
            </div>

            {/* Data Table */}
            <div className="rounded-lg border bg-card">
               <div className="overflow-auto">
                  <Table>
                     <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                           <TableRow key={headerGroup.id}>
                              {headerGroup.headers.map((header) => {
                                 return (
                                    <TableHead key={header.id}>
                                       {header.isPlaceholder
                                          ? null
                                          : flexRender(
                                               header.column.columnDef.header,
                                               header.getContext(),
                                            )}
                                    </TableHead>
                                 );
                              })}
                           </TableRow>
                        ))}
                     </TableHeader>
                     <TableBody>
                        {table.getRowModel().rows?.length ? (
                           table.getRowModel().rows.map((row, index) => (
                              <TableRow
                                 key={row.id}
                                 data-state={row.getIsSelected() && 'selected'}
                                 className={`cursor-pointer transition-colors ${
                                    row.getIsSelected()
                                       ? '!bg-blue-400/20 hover:bg-blue-200'
                                       : `hover:bg-slate-300/50 ${
                                            index % 2 === 0
                                               ? 'bg-white'
                                               : 'bg-slate-300/30'
                                         }`
                                 }`}
                                 onClick={() => row.toggleSelected()}
                              >
                                 {row.getVisibleCells().map((cell) => (
                                    <TableCell key={cell.id}>
                                       {flexRender(
                                          cell.column.columnDef.cell,
                                          cell.getContext(),
                                       )}
                                    </TableCell>
                                 ))}
                              </TableRow>
                           ))
                        ) : (
                           <TableRow>
                              <TableCell
                                 colSpan={columns.length}
                                 className="h-24 text-center"
                              >
                                 No SKU items found.
                              </TableCell>
                           </TableRow>
                        )}
                     </TableBody>
                  </Table>
               </div>

               {/* Pagination */}
               {totalPages >= 1 && (
                  <div className="flex items-center justify-between px-4 py-4 border-t">
                     <div className="flex items-center gap-2">
                        <Select
                           value={filters._limit?.toString() || '10'}
                           onValueChange={(value) => {
                              setFilters({ _limit: Number(value), _page: 1 });
                           }}
                        >
                           <SelectTrigger className="w-auto h-9">
                              <SelectValue />
                           </SelectTrigger>
                           <SelectContent>
                              <SelectGroup>
                                 <SelectItem value="10">10 / page</SelectItem>
                                 <SelectItem value="20">20 / page</SelectItem>
                                 <SelectItem value="50">50 / page</SelectItem>
                              </SelectGroup>
                           </SelectContent>
                        </Select>

                        <div className="text-muted-foreground text-sm">
                           Showing{' '}
                           <span className="font-medium">
                              {warehouseData.length > 0
                                 ? (currentPage - 1) * pageSize + 1
                                 : 0}
                           </span>{' '}
                           to{' '}
                           <span className="font-medium">
                              {Math.min(currentPage * pageSize, totalRecords)}
                           </span>{' '}
                           of{' '}
                           <span className="font-medium">{totalRecords}</span>{' '}
                           items
                        </div>
                     </div>

                     <div className="flex items-center gap-2">
                        {/* First Page */}
                        <Button
                           variant="outline"
                           size="icon"
                           className="h-9 w-9"
                           onClick={() => {
                              setFilters({ _page: 1 });
                           }}
                           disabled={isFirstPage}
                        >
                           <ChevronsLeft className="h-4 w-4" />
                        </Button>

                        {/* Previous Page */}
                        <Button
                           variant="outline"
                           size="icon"
                           className="h-9 w-9"
                           onClick={() => {
                              if (currentPage > 1) {
                                 setFilters({ _page: currentPage - 1 });
                              }
                           }}
                           disabled={!isPrevPage}
                        >
                           <ChevronLeft className="h-4 w-4" />
                        </Button>

                        {/* Page Numbers */}
                        <div className="flex items-center gap-1">
                           {getPageNumbers().map((page, index) => {
                              if (page === '...') {
                                 return (
                                    <span
                                       key={`ellipsis-${index}`}
                                       className="px-2 text-gray-400"
                                    >
                                       <Ellipsis className="h-4 w-4" />
                                    </span>
                                 );
                              }

                              return (
                                 <Button
                                    key={index}
                                    variant={
                                       currentPage === page
                                          ? 'default'
                                          : 'outline'
                                    }
                                    size="icon"
                                    className={cn(
                                       'h-9 w-9',
                                       currentPage === page &&
                                          'bg-black text-white hover:bg-black/90',
                                    )}
                                    onClick={() => {
                                       setFilters({ _page: page as number });
                                    }}
                                 >
                                    {page as number}
                                 </Button>
                              );
                           })}
                        </div>

                        {/* Next Page */}
                        <Button
                           variant="outline"
                           size="icon"
                           className="h-9 w-9"
                           onClick={() => {
                              if (currentPage < totalPages) {
                                 setFilters({ _page: currentPage + 1 });
                              }
                           }}
                           disabled={!isNextPage}
                        >
                           <ChevronRight className="h-4 w-4" />
                        </Button>

                        {/* Last Page */}
                        <Button
                           variant="outline"
                           size="icon"
                           className="h-9 w-9"
                           onClick={() => {
                              setFilters({ _page: totalPages });
                           }}
                           disabled={isLastPage}
                        >
                           <ChevronsRight className="h-4 w-4" />
                        </Button>
                     </div>
                  </div>
               )}
            </div>
         </LoadingOverlay>
      </div>
   );
};

export default WarehousesPage;
