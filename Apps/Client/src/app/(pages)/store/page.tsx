'use client';

import { useGetIphonePromotionsAsyncQuery } from '~/infrastructure/services/catalog.service';
import { useGetActivePromotionEventAsyncQuery } from '~/infrastructure/services/promotion.service';
import CategoriesCarousel from '@components/layouts/categories-carousel';
import { useEffect, useState } from 'react';
import PromotionBanner from './_components/promotion-banner';
import { ProductGridSkeleton } from './_components/product-card-skeleton';
import {
   PROMOTION_EVENT_TYPE_ENUM,
   DISCOUNT_TYPE_ENUM,
} from '~/domain/enums/discount-type.enum';

import { useDispatch } from 'react-redux';
import { useAppSelector } from '~/infrastructure/redux/store';
import PromotionIPhone from './_components/promotion-iPhone';

export interface ProductPromotion {
   promotion_product_name: string;
   promotion_product_description: string;
   promotion_product_image: string;
   promotion_product_unit_price: number;
   promotion_id: string;
   promotion_title: string;
   promotion_event_type: PROMOTION_EVENT_TYPE_ENUM;
   promotion_discount_type: DISCOUNT_TYPE_ENUM;
   promotion_discount_value: number;
   promotion_final_price: number;
   promotion_product_slug: string;
   product_model_id: string;
   category_id: string;
   product_name_tag: string;
   product_variants: ProductPromotionWithVariants[];
}

interface ProductPromotionWithVariants {
   product_id: string;
   product_color_image: string;
   color_name: string;
   color_hex: string;
   color_image: string;
   order: string;
}

const StorePage = () => {
   const [products, setProducts] = useState<ProductPromotion[]>([]);
   const [isImagesLoading, setIsImagesLoading] = useState(true);

   const dispatch = useDispatch();
   const { items } = useAppSelector((state) => state.cart.value);

   const {
      data: activePromotionEvent,
      isLoading: isLoadingActivePromotionEvent,
      isError: isErrorActivePromotionEvent,
      error: errorActivePromotionEvent,
   } = useGetActivePromotionEventAsyncQuery();

   const {
      data: iphonePromotions,
      isLoading: isLoadingIphonePromotions,
      isError: isErrorIphonePromotions,
      error: errorIphonePromotions,
   } = useGetIphonePromotionsAsyncQuery();

   useEffect(() => {
      if (iphonePromotions) {
         const productPromotions: ProductPromotion[] =
            iphonePromotions.items?.map((item) => {
               return {
                  promotion_product_name: item.promotion_product_name
                     .replace(/p/g, 'P')
                     .replace(/g/g, 'G')
                     .replace(/b/g, 'B')
                     .replace(/t/g, 'T'),
                  promotion_product_description:
                     item.promotion_product_description,
                  promotion_product_image: item.promotion_product_image,
                  promotion_product_unit_price:
                     item.promotion_product_unit_price,
                  promotion_id: item.promotion_id,
                  promotion_title: item.promotion_title,
                  promotion_event_type: item.promotion_event_type,
                  promotion_discount_type: item.promotion_discount_type,
                  promotion_discount_value: item.promotion_discount_value,
                  promotion_final_price: item.promotion_final_price,
                  promotion_product_slug: item.promotion_product_slug,
                  category_id: item.category_id,
                  product_model_id: item.product_model_id,
                  product_name_tag: item.product_name_tag,
                  product_variants: (Array.isArray(item.product_variants)
                     ? item.product_variants.map((variant) => ({
                          product_id: variant.product_id,
                          product_color_image: variant.product_color_image,
                          color_name: variant.color_name,
                          color_hex: variant.color_hex,
                          color_image: variant.color_image,
                          order: variant.order,
                       }))
                     : []) as ProductPromotionWithVariants[],
               };
            }) || [];

         setProducts(productPromotions);
      }
   }, [iphonePromotions]);

   // Determine if we're in a loading state
   const isLoading = isLoadingActivePromotionEvent || isLoadingIphonePromotions;

   return (
      <div className="bg-[#f5f5f7]">
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
                  <ProductGridSkeleton count={6} />
               ) : (
                  <div className="grid grid-cols-3 gap-x-4 gap-y-8 mt-5">
                     {products.map((item, index) => {
                        return <PromotionIPhone index={index} item={item} />;
                     })}
                  </div>
               )}
            </div>
         </div>
      </div>
   );
};

export default StorePage;
