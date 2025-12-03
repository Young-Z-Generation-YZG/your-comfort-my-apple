'use client';

import { cn } from '~/infrastructure/lib/utils';
import CardWrapper from './card-wrapper';
import NextImage from 'next/image';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import { useRouter } from 'next/navigation';
import useBasketService from '@components/hooks/api/use-basket-service';
import { TEventItem } from '~/domain/types/catalog.type';

interface PromotionIPhoneProps {
   item: TEventItem;
   className?: string;
}

const PromotionIPhone = ({ item, className }: PromotionIPhoneProps) => {
   const router = useRouter();
   const { storeEventItemAsync, isLoading: isStoreEventItemLoading } =
      useBasketService();

   // discount_value is already a percentage (e.g., 11 for 11%)
   const discountPercentage = item.discount_value;

   // Calculate progress bar values
   const totalAvailable = (item.stock || 0) + (item.sold || 0);
   const soldPercentage =
      totalAvailable > 0 ? ((item.sold || 0) / totalAvailable) * 100 : 0;

   const handleBuy = async () => {
      const result = await storeEventItemAsync({
         event_item_id: item.id,
      });

      if (result.isSuccess) {
         router.push('/checkout');
      }
   };

   return (
      <div className={cn('h-full', className)}>
         <CardWrapper className="w-full h-full flex flex-col">
            <div className="w-full overflow-hidden relative h-[300px]">
               <NextImage
                  src={item.image_url}
                  alt={`${item.model_name} ${item.color_name} ${item.storage_name}`}
                  width={Math.round((1000 * 16) / 9)}
                  height={1000}
                  className="absolute top-0 left-0 w-full h-full object-cover"
                  loading="lazy"
               />
               {/* Discount Badge */}
               <Badge
                  variant="destructive"
                  className="absolute top-4 right-4 text-base px-3 py-1"
               >
                  -{discountPercentage}% OFF
               </Badge>
            </div>

            {/* Color */}
            <div className="flex justify-center mt-5">
               <span
                  className={cn(
                     'h-[40px] w-[40px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector ring-offset-white',
                  )}
                  style={{ backgroundColor: item.color_hex_code }}
               ></span>
            </div>

            <div className="p-6 flex flex-col flex-1">
               <h3 className="mb-4 text-2xl font-semibold min-h-[64px]">
                  {item.model_name} {item.color_name} {item.storage_name}
               </h3>

               <div className="flex items-end gap-2">
                  {/* discount price */}
                  <span className="text-2xl font-medium text-red-600 font-SFProText">
                     ${item.final_price}
                  </span>
                  {/* original price */}
                  <span className="text-lg text-gray-500 line-through font-SFProText">
                     ${item.original_price}
                  </span>
               </div>

               <p className="mt-1 text-sm text-gray-500 font-SFProText">
                  Save <strong>${item.discount_amount}</strong> (
                  {discountPercentage}% off) for a limited time
               </p>

               {/* Progress Bar - Stock vs Sold */}
               {totalAvailable > 0 && (
                  <div className="mt-4">
                     <div className="flex justify-between items-center mb-2">
                        <div className="flex items-center gap-2">
                           <span className="text-sm font-medium text-gray-700 font-SFProText">
                              Sold:
                           </span>
                           <span className="inline-flex items-center justify-center min-w-[32px] px-2 py-1 bg-gradient-to-r from-purple-500 via-pink-500 to-red-500 text-white text-sm font-bold font-SFProText rounded-md shadow-md">
                              {item.sold || 0}
                           </span>
                        </div>
                        <span className="text-sm text-gray-600 font-SFProText">
                           Stock:{' '}
                           <strong className="text-emerald-600">
                              {item.stock || 0}
                           </strong>
                        </span>
                     </div>
                     <div className="relative w-full h-3 bg-gradient-to-r from-gray-100 to-gray-200 rounded-full overflow-hidden shadow-inner">
                        {/* Animated gradient background shimmer */}
                        <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/30 to-transparent animate-shimmer"></div>

                        {/* Progress fill with colorful gradient and animation */}
                        <div
                           className="relative h-full bg-gradient-to-r from-purple-500 via-pink-500 to-red-500 transition-all duration-700 ease-out animate-gradient-x shadow-lg"
                           style={{ width: `${soldPercentage}%` }}
                        >
                           {/* Shine effect */}
                           <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/40 to-transparent animate-shine"></div>
                        </div>
                     </div>
                  </div>
               )}

               <Button
                  className="w-full mt-5 cursor-pointer rounded-lg bg-blue-600 py-2 text-white font-SFProText font-medium hover:bg-blue-700 disabled:opacity-50"
                  onClick={handleBuy}
                  disabled={isStoreEventItemLoading}
               >
                  {isStoreEventItemLoading ? 'Adding...' : 'Buy now'}
               </Button>
            </div>
         </CardWrapper>
      </div>
   );
};

export default PromotionIPhone;
