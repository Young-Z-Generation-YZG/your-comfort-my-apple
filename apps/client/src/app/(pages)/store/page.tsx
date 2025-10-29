'use client';

import CategoriesCarousel from '@components/client/categories-carousel';
import PromotionBanner from './_components/promotion-banner';
import { ProductCardSkeleton } from './_components/product-card-skeleton';

import PromotionIPhone from './_components/promotion-iPhone';
import usePromotionService from '@components/hooks/api/use-promotion-service';
import { useEffect } from 'react';
import useBasketService from '@components/hooks/api/use-basket-service';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { useRouter } from 'next/navigation';

export const fakeData = {
   event: {
      id: '611db6eb-3d64-474e-9e23-3517ad0df6ec',
      title: 'Black Friday',
      description: 'Sale all item in shop with special price',
      startDate: '2025-10-21T13:31:22.941134Z',
      endDate: '2025-10-31T13:31:22.941156Z',
   },
   event_items: [
      {
         id: '99a356c8-c026-4137-8820-394763f30521',
         model_id: '664351e90087aa09993f5ae7',
         category: {
            id: '67dc470aa9ee0a5e6fbafdab',
            name: 'iPhone',
            description: 'iPhone categories.',
            order: 2,
            slug: 'iphone',
            parent_id: null,
            created_at: '2025-10-21T05:33:50.043Z',
            updated_at: '2025-10-21T05:33:50.043Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         model: {
            name: 'iPhone 15',
            normalized_name: 'IPHONE_15',
            order: 0,
         },
         color: {
            name: 'Blue',
            normalized_name: 'BLUE',
            hex_code: '#D5DDDF',
            showcase_image_id:
               'iphone-15-finish-select-202309-6-1inch-blue_zgxzmz',
            order: 0,
         },
         storage: {
            name: '256GB',
            normalized_name: '256GB',
            order: 1,
         },
         original_price: 1000,
         discount_type: 'PERCENTAGE',
         discount_value: 0.1,
         discount_amount: 100,
         final_price: 900,
         stock: 10,
         sold: 0,
         image_url:
            'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
         model_slug: 'iphone-15',
      },
   ],
};

export type TEvent = typeof fakeData.event;
export type TEventItem = (typeof fakeData.event_items)[number];

const StorePage = () => {
   const router = useRouter();

   const { getEventWithItemsAsync, getEventWithItemsState, isLoading } =
      usePromotionService();

   const { storeEventItemAsync, isLoading: isStoreEventItemLoading } =
      useBasketService();

   useEffect(() => {
      const fetchData = async () => {
         getEventWithItemsAsync();
      };
      fetchData();
   }, [getEventWithItemsAsync]);

   return (
      <div className="bg-[#f5f5f7]">
         <LoadingOverlay
            isLoading={isStoreEventItemLoading}
            fullScreen={true}
         ></LoadingOverlay>

         <div className="bg-white text-red-500 font-bold">
            <PromotionBanner />
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
            <span className="text-5xl w-[700px] text-wrap font-semibold text-slate-900/60 leading-[60px]">
               <p className="text-black">Black Friday Special</p>
               <p className="inline text-base text-slate-400 font-SFProText font-light relative -top-5">
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
                  <div className="grid grid-cols-3 gap-x-4 gap-y-8 mt-5">
                     {getEventWithItemsState.data?.event_items.map(
                        (item: TEventItem, index: number) => {
                           return (
                              <PromotionIPhone
                                 key={index}
                                 item={item}
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
