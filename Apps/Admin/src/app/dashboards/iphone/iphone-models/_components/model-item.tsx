import { Badge } from '@components/ui/badge';
import RatingStar from '@components/ui/rating-star';
import { Sparkles } from 'lucide-react';
import { Fragment } from 'react';
import { IIphoneModelResponse } from '~/src/domain/interfaces/catalogs/iPhone-model.interface';
import { cn } from '~/src/infrastructure/lib/utils';
import { CldImage } from 'next-cloudinary';
import { Star } from 'lucide-react';
import PromotionSection from './promotion-banner';

type IphoneModelItemProps = {
   className?: string;
   item: IIphoneModelResponse;
};

const IphoneModelItem = ({ className = '', item }: IphoneModelItemProps) => {
   return (
      <Fragment>
         <div className="flex flex-row bg-white px-5 py-5 rounded-md">
            <div className="image flex basis-[20%] items-center justify-center h-[250px] rounded-lg overflow-hidden mr-5">
               <CldImage
                  width={1000}
                  height={1000}
                  className="w-auto h-[170%] object-cover"
                  src={`${item.model_images[0].image_url}`}
                  alt="iPhone Model"
               />
            </div>

            <div className="content flex flex-col gap-2 flex-1 mr-5">
               <h2 className="font-SFProText font-normal text-xl">
                  {item.model_items.length === 2 ? (
                     <span>
                        <p>{item.model_items[0]?.model_name || 'Unknown'} &</p>
                        <p>{item.model_items[1]?.model_name || 'Unknown'}</p>
                     </span>
                  ) : (
                     <p>{item.model_items[0]?.model_name || 'Unknown'}</p>
                  )}
               </h2>

               <span className="flex flex-row gap-2">
                  <p className="first-letter:uppercase text-sm">colors:</p>
                  <p className="first-letter:uppercase text-sm">ultramarine</p>
               </span>

               <div className="flex flex-row gap-2">
                  {item.color_items.map((color, index) => {
                     return (
                        <div
                           key={index}
                           className={cn(
                              'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                           )}
                           style={{ backgroundColor: `${color.color_hex}` }}
                        />
                     );
                  })}
               </div>

               <span className="flex flex-row gap-2 mt-2">
                  <p className="first-letter:uppercase text-sm">Storage:</p>
               </span>

               <div className="flex flex-row gap-2">
                  {item.storage_items.map((storage, index) => {
                     return (
                        <span
                           key={index}
                           className={cn(
                              'uppercase select-none text-xs font-medium min-w-[70px] py-1 border rounded-full text-center',
                           )}
                        >
                           {storage.storage_name}
                        </span>
                     );
                  })}
               </div>

               <Badge
                  className="w-fit bg-[#23C55E] text-white mt-2 rounded-full select-none"
                  variant="outline"
               >
                  In Stock
               </Badge>

               <p className="text-muted-foreground text-sm font-medium mt-2">
                  100 units available • {item.overall_sold} sold
               </p>

               <span className="gap-2 mt-2 text-right">
                  {item.model_promotion ? (
                     <div>
                        <p className="text-sm font-normal line-through">
                           {[item.minimun_unit_price, item.maximun_unit_price]
                              .map((price) => `$${price}`)
                              .join(' - ')}
                        </p>
                        <p className="text-lg font-bold text-[#E11C48]">
                           ${item.model_promotion.minimum_promotion_price} - $
                           {item.model_promotion.maximum_promotion_price}
                        </p>
                     </div>
                  ) : (
                     <p
                        className={cn('text-sm font-normal', {
                           'text-base font-semibold': !item.model_promotion,
                        })}
                     >
                        {[item.minimun_unit_price, item.maximun_unit_price]
                           .map((price) => `$${price.toFixed(2)}`)
                           .join(' - ')}
                     </p>
                  )}
               </span>
            </div>

            {/* Promotion */}
            {item?.model_promotion && (
               <PromotionSection promotion={item.model_promotion} />
            )}

            <div className="border-l border-mute py-2 px-5">
               <div className="flex flex-col items-center gap-2 px-10 py-5 border border-dashed border-slate-400 rounded-lg bg-[#F9F9F9]">
                  <div className="flex flex-row gap-2 items-center">
                     <RatingStar
                        rating={item.average_rating.rating_average_value}
                        size={24}
                     />
                  </div>
                  <p className="font-medium text-base">
                     <span className="text-primary">
                        {item.average_rating.rating_average_value}
                     </span>{' '}
                     out of 5 stars
                  </p>
                  <p className="text-sm font-semibold">
                     {item.average_rating.rating_count} reviews
                  </p>

                  {item.rating_stars.map((star, index) => {
                     return (
                        <div
                           key={index}
                           className="flex flex-row gap-2 items-center"
                        >
                           <span className="w-2 text-muted-foreground text-sm font-medium">
                              {5 - index}
                           </span>

                           <span className="mr-2">
                              <Star
                                 className={`h-[24px] w-[24px] ${
                                    1 <= 5
                                       ? 'text-yellow-400 fill-yellow-400'
                                       : 'text-gray-300'
                                 }`}
                              />
                           </span>

                           <RatingStar
                              rating={5 - index}
                              size={16}
                              className="text-muted-foreground"
                           />

                           <p className="text-xs text-primary">
                              ({star.count})
                           </p>
                        </div>
                     );
                  })}
               </div>
            </div>
         </div>
      </Fragment>
   );
};

export default IphoneModelItem;
