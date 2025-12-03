'use client';

import CategoriesCarousel from '@components/client/categories-carousel';
import PromotionBanner from './_components/promotion-banner';
import { ProductCardSkeleton } from './_components/product-card-skeleton';

import PromotionIPhone from './_components/promotion-iPhone';
import usePromotionService from '@components/hooks/api/use-promotion-service';
import { useEffect, useMemo } from 'react';
import useBasketService from '@components/hooks/api/use-basket-service';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { useRouter } from 'next/navigation';
import { cn } from '~/infrastructure/lib/utils';
import { TEventItem } from '~/domain/types/catalog.type';

const StorePage = () => {
   const router = useRouter();

   const {
      getEventDetailsAsync,
      getEventDetailsState,
      isLoading: isGettingData,
   } = usePromotionService();

   const { storeEventItemAsync, isLoading: isStoreEventItemLoading } =
      useBasketService();

   useEffect(() => {
      getEventDetailsAsync('611db6eb-3d64-474e-9e23-3517ad0df6ec');
   }, [getEventDetailsAsync]);


   const isLoading = useMemo(() => {
      return isGettingData || isStoreEventItemLoading;
   }, [isGettingData, isStoreEventItemLoading]);

   return (
      <div className="bg-[#f5f5f7]">
         <LoadingOverlay
            isLoading={isStoreEventItemLoading}
            fullScreen={true}
         ></LoadingOverlay>

         <div className={cn("bg-white text-red-500 font-bold", {})}>
            <PromotionBanner />
         </div>
         <div className="container mx-auto px-4 sm:px-6 lg:px-8 pt-10 md:pt-20 pb-8 md:pb-16">
      <p className="text-3xl md:text-5xl max-w-lg md:max-w-2xl text-wrap font-semibold text-slate-900/60 leading-tight md:leading-[60px]">
         <span className="inline text-black">Store.</span>
         The best way to buy the products you love.
      </p>
   </div>

         <div>
            <CategoriesCarousel />
         </div>

         <div className="container mx-auto px-4 sm:px-6 lg:px-8 pt-10 md:pt-20 pb-8 md:pb-16">
      <span className="text-3xl md:text-5xl max-w-lg md:max-w-2xl text-wrap font-semibold text-slate-900/60 leading-tight md:leading-[60px]">
         <p className="text-black">Black Friday Special</p>
         <p className="inline text-sm md:text-base text-slate-400 font-SFProText font-light relative -top-2 md:-top-5">
            Limited-time savings on your favorite Apple products.
         </p>
      </span>

            <div className="w-full">
               {isLoading ? (
                  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                     {/* <ProductGridSkeleton count={5} className="" /> */}
                     <ProductCardSkeleton />
                     <ProductCardSkeleton />
                     <ProductCardSkeleton />
                     <ProductCardSkeleton />
                     <ProductCardSkeleton />
                     <ProductCardSkeleton />
                  </div>
               ) : (
                  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 md:gap-6 mt-5 items-stretch">
                     {getEventDetailsState.data?.event_items.map(
                        (item: TEventItem, index: number) => {
                           return (
                              <PromotionIPhone
                                 key={index}
                                 item={item}
                                 className="h-full"
                                 onBuy={async () => {
                                    const result = await storeEventItemAsync({
                                       event_item_id: item.id,
                                    });

                                    if (result.isSuccess) {
                                       router.push('/checkout');
                                    }
                                 }}
                              />
                           );
                        },
                     )}
                  </div>
               )}
            </div>
         </div>
      </div>
   );
};

export default StorePage;
