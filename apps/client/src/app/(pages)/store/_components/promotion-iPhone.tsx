import { cn } from '~/infrastructure/lib/utils';
import CardWrapper from './card-wrapper';
import NextImage from 'next/image';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import { listPromotionIphoneFakeData } from '../_data/fake-data';

interface PromotionIPhoneProps {
   className?: string;
}

const PromotionIPhone = ({ className }: PromotionIPhoneProps) => {
   // Get the first promotion item from fake data
   const promotionItem = listPromotionIphoneFakeData[0];

   // Calculate discount percentage
   const discountPercentage = Math.round(promotionItem.discount_value * 100);

   // Calculate progress bar values
   const totalAvailable = promotionItem.stock + promotionItem.sold;
   const soldPercentage = (promotionItem.sold / totalAvailable) * 100;

   return (
      <div className={cn('', className)}>
         <CardWrapper className="w-full">
            <div className="w-full overflow-hidden relative h-[300px]">
               <NextImage
                  src={promotionItem.image_url}
                  alt={`${promotionItem.model.name} ${promotionItem.color.name}`}
                  width={Math.round((1000 * 16) / 9)}
                  height={1000}
                  className="absolute top-0 left-0 w-full h-full object-cover"
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
                  style={{ backgroundColor: promotionItem.color.hex_code }}
               ></span>
            </div>

            <div className="p-6">
               <h3 className="mb-4 text-2xl font-semibold">
                  {promotionItem.model.name} {promotionItem.color.name}{' '}
                  {promotionItem.storage.name}
               </h3>

               <div className="flex items-end gap-2">
                  {/* discount price */}
                  <span className="text-2xl font-medium text-red-600 font-SFProText">
                     ${promotionItem.final_price}
                  </span>
                  {/* original price */}
                  <span className="text-lg text-gray-500 line-through font-SFProText">
                     ${promotionItem.original_price}
                  </span>
               </div>

               <p className="mt-1 text-sm text-gray-500 font-SFProText">
                  Save <strong>{discountPercentage}%</strong> for a limited time
               </p>

               {/* Progress Bar - Stock vs Sold */}
               <div className="mt-4">
                  <div className="flex justify-between items-center mb-2">
                     <div className="flex items-center gap-2">
                        <span className="text-sm font-medium text-gray-700 font-SFProText">
                           Sold:
                        </span>
                        <span className="inline-flex items-center justify-center min-w-[32px] px-2 py-1 bg-gradient-to-r from-purple-500 via-pink-500 to-red-500 text-white text-sm font-bold font-SFProText rounded-md shadow-md">
                           {promotionItem.sold}
                        </span>
                     </div>
                     <span className="text-sm text-gray-600 font-SFProText">
                        Stock:{' '}
                        <strong className="text-emerald-600">
                           {promotionItem.stock}
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

               <Button
                  className="w-full mt-5 cursor-pointer rounded-lg bg-blue-600 py-2 text-white font-SFProText font-medium hover:bg-blue-700"
                  onClick={() => {}}
               >
                  Buy now
               </Button>
            </div>
         </CardWrapper>
      </div>
   );
};

export default PromotionIPhone;
