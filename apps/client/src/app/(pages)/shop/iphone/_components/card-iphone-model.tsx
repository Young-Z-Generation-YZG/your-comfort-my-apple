'use client';

import NextImage from 'next/image';
import Link from 'next/link';
import { cn } from '~/infrastructure/lib/utils';
import {
   IModel,
   IColor,
   IStorage,
   IAverageRating,
   ISKUPrice,
} from '~/domain/interfaces/common/value-objects.interface';

type ProductItem = {
   id: string;
   slug: string;
   model_items: IModel[];
   color_items: IColor[];
   storage_items: IStorage[];
   average_rating: IAverageRating;
   sku_prices: ISKUPrice[];
};

export const CardIphoneModel = ({ item }: { item: ProductItem }) => {
   const minPrice = item.sku_prices?.[0]?.price;
   const rating =
      item.average_rating?.rating_average_value?.toFixed(1) ?? '0.0';
   const ratingCount = item.average_rating?.rating_count ?? 0;
   const modelName = item.model_items?.[0]?.name ?? 'Product Name';
   const colors = item.color_items ?? [];
   const storageCount = item.storage_items?.length ?? 0;

   // Calculate min and max prices from skuPrices
   const priceRange = item.sku_prices.reduce(
      (acc, sku) => {
         if (sku.unit_price < acc.min) acc.min = sku.unit_price;
         if (sku.unit_price > acc.max) acc.max = sku.unit_price;
         return acc;
      },
      { min: Infinity, max: -Infinity },
   );

   // Format price for display
   const formatPrice = (price: number) => {
      return new Intl.NumberFormat('en-US', {
         style: 'currency',
         currency: 'USD',
         minimumFractionDigits: 0,
         maximumFractionDigits: 0,
      }).format(price);
   };

   return (
      <Link
         href={`/products/iphone/${item.slug}`}
         className={cn(
            'block bg-white rounded-2xl border border-gray-200 overflow-hidden shadow-sm active:scale-[0.98] transition-transform duration-100',
         )}
      >
         {/* Image */}
         <div className="relative w-full aspect-[4/3] bg-gray-50">
            <NextImage
               src={
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp'
               }
               alt={modelName}
               fill
               className="object-contain p-4"
               sizes="(max-width: 768px) 100vw, 50vw"
               priority
            />
         </div>

         {/* Content */}
         <div className="p-4 flex flex-col gap-2">
            {/* Name */}
            <h2 className="font-SFProText text-xl text-start font-semibold">
               {item.model_items && item.model_items.length > 0 ? (
                  item.model_items.length === 1 ? (
                     <span>{item.model_items[0].name}</span>
                  ) : (
                     <span>
                        {item.model_items
                           .slice(0, 2)
                           .map((m) => m.name)
                           .join(' & ')}
                        {item.model_items.length > 2 &&
                           ` & ${item.model_items
                              .slice(2)
                              .map((m) => m.name)
                              .join(', ')}`}
                     </span>
                  )
               ) : (
                  'Product Name'
               )}
            </h2>

            {/* Rating */}
            <div className="flex items-center gap-1 text-xs text-gray-500">
               <span>⭐ {rating}</span>
               <span className="text-gray-400">({ratingCount})</span>
            </div>

            {/* Colors */}
            <div className="flex flex-row gap-2 mt-2">
               {colors.map((color, index) => (
                  <div
                     key={index}
                     className={cn(
                        'h-[28px] w-[28px] cursor-pointer rounded-full border-2 border-solid border-gray-200 shadow-sm transition-all duration-300 ease-in-out',
                        'active:scale-95',
                     )}
                     style={{ backgroundColor: color.hex_code }}
                  />
               ))}
            </div>

            {/* Storage */}
            {/* <div className="text-xs text-gray-500 mt-1">
               Storages:{' '}
               {item.storage_items.map((s, i) => (
                  <span key={i}>
                     {s.name}
                     {i < item.storage_items.length - 1 && ', '}
                  </span>
               ))}
            </div> */}
            <div className="mt-1">
               <div className="flex flex-row flex-wrap gap-2">
                  {item.storage_items.map((s, i) => (
                     <span
                        key={i}
                        className="uppercase select-none text-xs font-medium min-w-[70px] py-1 border rounded-full text-center text-gray-700"
                     >
                        {s.name}
                     </span>
                  ))}
               </div>
            </div>

            {/* Price */}
            <div className="flex flex-row gap-2 mt-2">
               {priceRange.min === priceRange.max ? (
                  <span className="text-lg font-semibold text-gray-900">
                     {formatPrice(priceRange.min)}
                  </span>
               ) : (
                  <span className="text-lg font-semibold text-gray-900">
                     {formatPrice(priceRange.min)} -{' '}
                     {formatPrice(priceRange.max)}
                  </span>
               )}
            </div>
         </div>
      </Link>
   );
};
