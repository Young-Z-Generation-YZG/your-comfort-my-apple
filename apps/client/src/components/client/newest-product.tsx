'use client';

import Image from 'next/image';
import { useMemo } from 'react';
import { TNewestProduct } from '~/infrastructure/services/product.service';

type NewestProductProps = {
   product: TNewestProduct;
};

const NewestProduct = ({ product }: NewestProductProps) => {
   const { minPrice, maxPrice } = useMemo(() => {
      if (!product.sku_prices || product.sku_prices.length === 0) {
         return { minPrice: 0, maxPrice: 0 };
      }

      const prices = product.sku_prices.map((sku) => sku.unit_price);
      return {
         minPrice: Math.min(...prices),
         maxPrice: Math.max(...prices),
      };
   }, [product.sku_prices]);

   const displayName = useMemo(() => {
      if (!product.model_items || product.model_items.length === 0) {
         return product.name;
      }

      return [...product.model_items]
         .sort((a, b) => a.order - b.order)
         .map((model) => model.name)
         .join(' & ');
   }, [product.model_items, product.name]);

   const firstImage = product.showcase_images?.[0];

   return (
      <div
         className="group relative flex h-full flex-col overflow-hidden rounded-3xl border-2 border-blue-200 
                    bg-gradient-to-br from-white to-blue-50/30 shadow-lg transition-all duration-300 
                    hover:shadow-2xl hover:scale-[1.02] dark:border-blue-700 dark:from-gray-800 dark:to-blue-950/30"
      >
         {/* NEW Badge */}
         <div className="absolute left-4 top-4 z-10">
            <div className="flex items-center gap-1.5 rounded-full bg-gradient-to-r from-blue-600 to-purple-600 px-4 py-2 shadow-lg">
               <svg
                  className="h-4 w-4 text-white"
                  fill="currentColor"
                  viewBox="0 0 20 20"
               >
                  <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
               </svg>
               <span className="text-sm font-bold uppercase tracking-wide text-white">
                  New
               </span>
            </div>
         </div>

         {/* Image Section */}
         <div className="relative h-[400px] w-full overflow-hidden">
            {firstImage ? (
               <Image
                  src={firstImage.image_url}
                  alt={displayName}
                  width={Math.round((1000 * 16) / 9)}
                  height={1000}
                  className="absolute left-0 top-0 h-full w-full object-cover"
               />
            ) : (
               <div className="flex h-full w-full items-center justify-center text-gray-400">
                  <span>No Image</span>
               </div>
            )}
         </div>

         {/* Content Section - More Spacious */}
         <div className="flex flex-1 flex-col p-7">
            {/* Product Name - Larger */}
            <h3 className="mb-4 line-clamp-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-gray-100">
               {displayName}
            </h3>

            {/* Price Section - Emphasized */}
            <div className="mt-auto mb-4">
               <div className="mb-1 text-sm font-medium text-gray-500 dark:text-gray-400">
                  Starting at
               </div>
               <div className="text-3xl font-bold text-blue-600 dark:text-blue-400">
                  {minPrice === maxPrice ? (
                     <span>${minPrice.toLocaleString()}</span>
                  ) : (
                     <span>${minPrice.toLocaleString()}</span>
                  )}
               </div>
               {minPrice !== maxPrice && (
                  <div className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                     Up to ${maxPrice.toLocaleString()}
                  </div>
               )}
            </div>

            {/* Learn More Button - More Prominent */}
            <button
               className="w-full rounded-full bg-gradient-to-r from-blue-600 to-purple-600 py-3.5 text-base 
                          font-semibold text-white shadow-lg transition-all duration-300 
                          hover:from-blue-700 hover:to-purple-700 hover:shadow-xl hover:-translate-y-0.5"
            >
               Discover now
            </button>
         </div>
      </div>
   );
};

export default NewestProduct;
