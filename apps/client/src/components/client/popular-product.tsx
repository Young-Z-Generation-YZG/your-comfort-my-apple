'use client';

import Image from 'next/image';
import { useMemo } from 'react';
import RatingStar from '@components/ui/rating-star';
import { TPopularProduct } from '~/infrastructure/services/product.service';

type PopularProductProps = {
   product: TPopularProduct;
};

const PopularProduct = ({ product }: PopularProductProps) => {
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
         className="group relative flex h-full flex-col overflow-hidden rounded-3xl border-2 border-orange-200 
                    bg-gradient-to-br from-white to-orange-50/30 shadow-lg transition-all duration-300 
                    hover:shadow-2xl hover:scale-[1.02] dark:border-orange-700 dark:from-gray-800 dark:to-orange-950/30"
      >
         {/* POPULAR Badge */}
         <div className="absolute left-4 top-4 z-10">
            <div className="flex items-center gap-1.5 rounded-full bg-gradient-to-r from-orange-600 to-red-600 px-4 py-2 shadow-lg">
               <svg
                  className="h-4 w-4 text-white"
                  fill="currentColor"
                  viewBox="0 0 20 20"
               >
                  <path d="M3.172 5.172a4 4 0 015.656 0L10 6.343l1.172-1.171a4 4 0 115.656 5.656L10 17.657l-6.828-6.829a4 4 0 010-5.656z" />
               </svg>
               <span className="text-sm font-bold uppercase tracking-wide text-white">
                  Popular
               </span>
            </div>
         </div>

         {/* Promotion Badge */}
         {hasPromotion && (
            <div className="absolute right-4 top-4 z-10">
               <div className="rounded-full bg-red-500 px-3 py-1 text-xs font-semibold text-white shadow-md">
                  Sale
               </div>
            </div>
         )}

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
            <h3 className="mb-3 line-clamp-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-gray-100">
               {displayName}
            </h3>

            {/* Rating and Sold Count */}
            <div className="mb-4 flex items-center gap-3 text-sm">
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

            {/* Price Section - Emphasized */}
            <div className="mt-auto mb-4">
               {hasPromotion ? (
                  <div className="flex flex-col gap-1">
                     <div className="mb-1 text-sm font-medium text-gray-500 dark:text-gray-400">
                        Special Price
                     </div>
                     <div className="flex items-center gap-2">
                        <span className="text-3xl font-bold text-red-600 dark:text-red-500">
                           ${product.promotion!.final_price.toLocaleString()}
                        </span>
                        <span className="text-lg text-gray-500 line-through dark:text-gray-400">
                           ${minPrice.toLocaleString()}
                        </span>
                     </div>
                     <div className="text-sm text-orange-600 dark:text-orange-400">
                        {product.promotion!.promotion_discount_type ===
                        'PERCENTAGE'
                           ? `Save ${(product.promotion!.promotion_discount_value * 100).toFixed(0)}%`
                           : `Save $${product.promotion!.promotion_discount_amount}`}
                     </div>
                  </div>
               ) : (
                  <div>
                     <div className="mb-1 text-sm font-medium text-gray-500 dark:text-gray-400">
                        Starting at
                     </div>
                     <div className="text-3xl font-bold text-orange-600 dark:text-orange-400">
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
               )}
            </div>

            {/* Learn More Button - More Prominent */}
            <button
               className="w-full rounded-full bg-gradient-to-r from-orange-600 to-red-600 py-3.5 text-base 
                          font-semibold text-white shadow-lg transition-all duration-300 
                          hover:from-orange-700 hover:to-red-700 hover:shadow-xl hover:-translate-y-0.5"
            >
               Buy now
            </button>
         </div>
      </div>
   );
};

export default PopularProduct;
