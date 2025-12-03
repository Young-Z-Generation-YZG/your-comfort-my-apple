'use client';

import CategoriesCarousel from '@components/client/categories-carousel';
import PromotionIPhone from './promotion-iphone';
import { TEvent, TEventItem } from '~/domain/types/catalog.type';
import { BlackFridayBanner } from './banner';

interface EventContentProps {
   eventData: TEvent;
   eventItems: TEventItem[];
}

const EventContent = ({ eventData, eventItems }: EventContentProps) => {
   return (
      <div className="bg-[#f5f5f7]">
         <div className="bg-white text-red-500 font-bold">
            <BlackFridayBanner />
         </div>

         <div className="row w-[1200px] mx-auto pt-[80px] pb-[64px]">
            <p className="text-5xl w-[700px] text-wrap font-semibold text-slate-900/60 leading-[60px]">
               <span className="inline text-black">Store.</span>
               The best way to buy the products you love.
            </p>
         </div>

         <div>
            <CategoriesCarousel />
         </div>

         <div className="w-[1200px] mx-auto pt-[80px] pb-[64px]">
            <div className="text-5xl w-[700px] text-wrap font-semibold text-slate-900/60 leading-[60px]">
               <h1 className="text-black">
                  {eventData.title || 'Black Friday Special'}
               </h1>
               <p className="inline text-base text-slate-400 font-SFProText font-light relative -top-5">
                  {eventData.description ||
                     'Limited-time savings on your favorite Apple products.'}
               </p>
            </div>

            {eventItems && eventItems.length > 0 && (
               <div className="w-full">
                  <div className="grid grid-cols-3 gap-x-4 gap-y-8 mt-5 items-stretch">
                     {eventItems.map((item: TEventItem, index: number) => {
                        return (
                           <PromotionIPhone
                              key={item.id || index}
                              item={item}
                              className="h-full"
                           />
                        );
                     })}
                  </div>
               </div>
            )}
         </div>
      </div>
   );
};

export default EventContent;
