// ResponsiveHelpers.tsx
import React, { useState, useMemo } from 'react';
import { motion } from 'framer-motion';
import { FaFilter } from 'react-icons/fa';
import { GoMultiSelect } from 'react-icons/go';
import { Button } from '@components/ui/button';
import {
   Dialog,
   DialogContent,
   DialogFooter,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '@components/ui/dialog';
import FilterSection from './filter-section'; // đường dẫn tới component FilterSection hiện tại
import { cn } from '~/infrastructure/lib/utils';

// ---------- Types (tuỳ chỉnh theo project bạn) ----------
type ProductItem = {
   id: string | number;
   images?: string[]; // url list
   price?: number;
   title?: string;
   // ... các trường khác nếu cần
};

type PropsForHelpers = {
   // trạng thái filter & handler từ parent
   filters: any;
   setFilters: (f: any) => void;
   fetchProducts: (newFilters?: any) => Promise<void>; // gọi API load sản phẩm
   activeFilterCount: number;
   totalRecords: number;
   // products từ parent (full list) hoặc paginationItems tương tự bạn đang dùng
   products: ProductItem[];
   isLoading: boolean;
};

// ---------- Mobile sticky filter bar (hiện < lg) ----------
export function MobileFilterBar({
   activeFilterCount,
   totalRecords,
   openDialog,
   setOpenDialog,
   onSortClick,
}: {
   activeFilterCount: number;
   totalRecords: number;
   openDialog: boolean;
   setOpenDialog: (v: boolean) => void;
   onSortClick?: () => void;
}) {
   return (
      // only show on <lg
      <div className="lg:hidden w-full sticky top-0 z-50 bg-white border-b border-[#e6e6e6]">
         <div className="max-w-[1440px] mx-auto px-4 py-3 flex items-center gap-3">
            <button
               onClick={() => setOpenDialog(true)}
               className="flex items-center gap-2 px-3 py-2 border rounded-md text-sm font-semibold text-[#218bff] hover:bg-[#f3f8ff]"
               aria-label="Open filters"
            >
               <FaFilter />
               <span>
                  Filters{activeFilterCount > 0 && ` (${activeFilterCount})`}
               </span>
            </button>

            <div className="ml-2 text-sm text-gray-600">
               {totalRecords} {totalRecords === 1 ? 'Result' : 'Results'}
            </div>

            <div className="ml-auto flex items-center gap-3">
               <button
                  onClick={onSortClick}
                  className="flex items-center gap-2 px-3 py-2 border rounded-md text-sm"
                  aria-label="Sort"
               >
                  <GoMultiSelect />
                  <span>Sort</span>
               </button>
               {/* bạn có thể thêm nút chat / other icons ở đây */}
            </div>
         </div>
      </div>
   );
}

// ---------- Dialog wrapper for FilterSection ----------
export function FilterDialog({
   open,
   setOpen,
   filters,
   setFilters,
   onApply, // gọi khi apply
}: {
   open: boolean;
   setOpen: (v: boolean) => void;
   filters: any;
   setFilters: (f: any) => void;
   onApply: (newFilters: any) => void;
}) {
   // Local copy để user chỉnh trong dialog nhưng chưa apply
   const [localFilters, setLocalFilters] = useState(filters);

   // sync when dialog open with parent filters
   React.useEffect(() => {
      if (open) setLocalFilters(filters);
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [open]);

   const handleApply = () => {
      setFilters(localFilters);
      onApply(localFilters);
      setOpen(false);
   };

   const handleCancel = () => {
      setLocalFilters(filters);
      setOpen(false);
   };

   return (
      <Dialog open={open} onOpenChange={setOpen}>
         <DialogContent className="p-0 w-full max-w-[720px] h-[90vh] md:h-[80vh]">
            <DialogHeader className="px-4 py-3 border-b">
               <DialogTitle>Filters</DialogTitle>
            </DialogHeader>

            {/* Animate the content for nicer feel */}
            <motion.div
               initial={{ y: 20, opacity: 0 }}
               animate={{ y: 0, opacity: 1 }}
               exit={{ y: 10, opacity: 0 }}
               transition={{ duration: 0.2 }}
               className="overflow-auto h-[calc(100%-120px)]"
            >
               {/* Place your FilterSection here, passing handlers to update localFilters */}
               {/* Assuming FilterSection accepts props to control selected values.
              If not, you can adapt FilterSection to accept a `filters` + `onChange` pair. */}
               <div className="p-4">
                  <FilterSection
                     // Pass props that let FilterSection be controlled by localFilters
                     // If your FilterSection uses its own internal state, you can clone it
                     // or modify FilterSection to accept `initialFilters` + `onChange`.
                     // Below are example props — adapt to actual FilterSection API.
                     filters={localFilters}
                     setFilters={setLocalFilters}
                     // keep existing handlers like toggleColor/... if needed
                  />
               </div>
            </motion.div>

            <DialogFooter className="flex gap-2 p-4 border-t">
               <Button variant="outline" onClick={handleCancel}>
                  Cancel
               </Button>
               <Button onClick={handleApply}>Apply Filters</Button>
            </DialogFooter>
         </DialogContent>
      </Dialog>
   );
}

// ---------- Compact product card (image + price only) ----------
export function ProductCardCompact({ product }: { product: ProductItem }) {
   const image =
      product.images && product.images.length ? product.images[0] : '';
   const price = product.price ?? 0;

   return (
      <div className="bg-white rounded-md shadow-sm overflow-hidden">
         <div className="w-full aspect-[4/5] bg-gray-100 flex items-center justify-center overflow-hidden">
            {image ? (
               <img
                  src={image}
                  alt={product.title || 'product'}
                  className="object-cover w-full h-full"
               />
            ) : (
               <div className="text-gray-400">No image</div>
            )}
         </div>
         <div className="px-3 py-2">
            <div className="text-sm font-medium text-gray-800">
               {/* optional title */}
            </div>
            <div className="mt-1 text-lg font-semibold">
               {price?.toLocaleString?.()
                  ? `$${price.toLocaleString()}`
                  : `$${price}`}
            </div>
         </div>
      </div>
   );
}

// ---------- Compact grid for mobile / tablet (< lg) ----------
export function CompactProductGrid({
   products,
   isLoading,
}: {
   products: ProductItem[];
   isLoading: boolean;
}) {
   // responsive grid:
   // phone: 2 cols, md: 3 cols, lg: 4 cols (but this component you will render only for <lg)
   return (
      <div className="w-full">
         <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
            {isLoading
               ? Array(8)
                    .fill(0)
                    .map((_, i) => (
                       <div
                          key={i}
                          className="h-48 bg-gray-100 animate-pulse rounded-md"
                       />
                    ))
               : products.map((p) => (
                    <ProductCardCompact key={p.id} product={p} />
                 ))}
         </div>
      </div>
   );
}

export default null; // chỉ để tránh lỗi export ở file chung — import các functions/cmp bạn cần
