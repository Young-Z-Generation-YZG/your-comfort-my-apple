'use client';

import Image from 'next/image';
import { useMemo } from 'react';
import RatingStar from '@components/ui/rating-star';
import { TSuggestionProduct } from '~/infrastructure/services/product.service';

type SuggestionProductProps = {
   product: TSuggestionProduct;
};

const SuggestionProduct = ({ product }: SuggestionProductProps) => {
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
   const hasPromotion = product.promotion && product.promotion.final_price;

   return (
      <div
         className="group relative flex h-full flex-col overflow-hidden rounded-2xl border border-gray-200 
                    bg-white shadow-sm transition-all hover:shadow-lg dark:border-gray-700 dark:bg-gray-800"
      >
         {/* Image Section */}
         <div className="relative h-[300px] w-full overflow-hidden">
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

            {/* Promotion Badge */}
            {hasPromotion && (
               <div className="absolute right-3 top-3 rounded-full bg-red-500 px-3 py-1 text-xs font-semibold text-white shadow-md">
                  Sale
               </div>
            )}
         </div>

         {/* Content Section */}
         <div className="flex flex-1 flex-col p-5">
            {/* Product Name */}
            <h3 className="mb-2 line-clamp-2 text-lg font-semibold tracking-tight text-gray-900 dark:text-gray-100">
               {displayName}
            </h3>

            {/* Rating and Sold Count */}
            <div className="mb-3 flex items-center gap-3 text-sm">
               <div className="flex items-center gap-1">
                  <RatingStar
                     rating={product.average_rating?.rating_average_value || 0}
                     size="sm"
                  />
                  <span className="ml-1 text-xs text-gray-600 dark:text-gray-400">
                     ({product.average_rating?.rating_count || 0})
                  </span>
               </div>
               {product.overall_sold > 0 && (
                  <>
                     <span className="text-gray-300 dark:text-gray-600">|</span>
                     <span className="text-xs text-gray-600 dark:text-gray-400">
                        {product.overall_sold.toLocaleString()} sold
                     </span>
                  </>
               )}
            </div>

            {/* Price Section */}
            <div className="mt-auto">
               {hasPromotion ? (
                  <div className="flex flex-col gap-1">
                     <div className="flex items-center gap-2">
                        <span className="text-xl font-bold text-red-600 dark:text-red-500">
                           ${product.promotion!.final_price.toLocaleString()}
                        </span>
                        <span className="text-sm text-gray-500 line-through dark:text-gray-400">
                           ${minPrice.toLocaleString()}
                        </span>
                     </div>
                     <div className="text-xs text-gray-500 dark:text-gray-400">
                        {product.promotion!.promotion_discount_type ===
                        'PERCENTAGE'
                           ? `Save ${(product.promotion!.promotion_discount_value * 100).toFixed(0)}%`
                           : `Save $${product.promotion!.promotion_discount_amount}`}
                     </div>
                  </div>
               ) : (
                  <div className="text-xl font-bold text-gray-900 dark:text-gray-100">
                     {minPrice === maxPrice ? (
                        <span>${minPrice.toLocaleString()}</span>
                     ) : (
                        <span>
                           ${minPrice.toLocaleString()} - $
                           {maxPrice.toLocaleString()}
                        </span>
                     )}
                  </div>
               )}
            </div>

            {/* Learn More Button */}
            <button
               className="mt-4 w-full rounded-full bg-blue-600 py-2.5 text-sm font-medium text-white 
                          transition-all duration-300 hover:bg-blue-700"
            >
               Learn more
            </button>
         </div>
      </div>
   );
};

export default SuggestionProduct;
