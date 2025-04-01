'use client';

import { useGetIphonePromotionsAsyncQuery } from '~/infrastructure/services/catalog.service';
import { useGetActivePromotionEventAsyncQuery } from '~/infrastructure/services/promotion.service';
import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '~/components/ui/carousel';
import Image from 'next/image';
import Link from 'next/link';
import CategoriesCarousel from '~/components/layouts/CategoriesCarousel';
import CartWrapper from './_components/CardWrapper';
import Button from '../(auth)/_components/Button';
import { ShoppingBag } from 'lucide-react';
import { useEffect, useState } from 'react';
import PromotionBanner from './_components/PromotionBanner';
import { Skeleton } from '~/components/ui/skeleton';
import { ProductGridSkeleton } from './_components/ProductCardSkeleton';

interface ProductPromotion {
   product_id: string | undefined;
   product_name: string | undefined;
   product_unit_price: number | undefined;
   product_image_url: string | undefined;
   product_slug: string | undefined;
   promotion_discount_type: string | undefined;
   promotion_discount_value: number | undefined;
   promotion_final_price: number | undefined;
}

const StorePage = () => {
   const [products, setProducts] = useState<ProductPromotion[]>([]);
   const [isImagesLoading, setIsImagesLoading] = useState(true);

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
            iphonePromotions.promotion_items?.map((item) => {
               const originalProduct = iphonePromotions.items.find(
                  (product) => product.product_id === item.promotion_product_id,
               );

               const productName = originalProduct?.product_slug.replace(
                  '-',
                  ' ',
               );

               console.log(
                  'productName',
                  originalProduct?.product_images[0].image_url,
               );

               return {
                  product_id: originalProduct?.product_id,
                  product_name: productName,
                  product_unit_price: originalProduct?.product_unit_price,
                  product_image_url:
                     originalProduct?.product_images[0].image_url,
                  product_slug: originalProduct?.product_slug,
                  promotion_discount_type: item.promotion_discount_type,
                  promotion_discount_value: item.promotion_discount_value,
                  promotion_final_price: item.promotion_final_price,
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
               <p className="inline text-black">Store.</p>
               The best way to buy the products you love.
            </p>
         </div>

         <div>
            <CategoriesCarousel />
         </div>

         <div className="w-[1200px] mx-auto pt-[80px] pb-[64px]">
            <p className="text-5xl w-[700px] text-wrap font-semibold text-slate-900/60 leading-[60px]">
               <p className="text-black">Black Friday Special</p>
               <p className="inline text-base text-slate-400 font-SFProText font-light relative -top-5">
                  Limited-time savings on your favorite Apple products.
               </p>
            </p>

            <div className="w-full">
               {isLoading ? (
                  <ProductGridSkeleton count={6} />
               ) : (
                  <div className="grid grid-cols-3 gap-x-4 gap-y-8 mt-5">
                     {products.map((item, index) => {
                        return (
                           <div className="col-span-1" key={index}>
                              <CartWrapper>
                                 <div className="relative cursor-pointer">
                                    <span className="absolute top-3 right-3 bg-red-600 text-white text-xs font-bold px-2 py-1 rounded-full">
                                       Save 10 %
                                    </span>
                                    <div className="flex items-center justify-center bg-[#f5f5f7] overflow-hidden transition-all hover:shadow-md">
                                       <Link href="#">
                                          <Image
                                             src={`${item.product_image_url}?wid=800&hei=800`}
                                             alt="test"
                                             width={1000}
                                             height={1000}
                                             className="relative -top-10 object-none"
                                          />
                                       </Link>
                                    </div>
                                 </div>

                                 <div className="p-6">
                                    <div className="mb-4">
                                       <h3 className="font-semibold text-2xl">
                                          {item.product_name}
                                       </h3>
                                    </div>

                                    <div className="mb-4">
                                       <div className="flex items-end gap-2">
                                          <span className="text-2xl font-medium text-red-600 font-SFProText">
                                             ${item.promotion_final_price}
                                          </span>
                                          <span className="text-base text-gray-500 line-through font-SFProText">
                                             ${item.product_unit_price}
                                          </span>
                                       </div>
                                       <p className="text-sm font-SFProText text-gray-500 mt-1">
                                          Save $
                                          {(
                                             (item.product_unit_price ?? 0) -
                                             (item.promotion_final_price ?? 0)
                                          ).toFixed(2)}{' '}
                                          for a limited time
                                       </p>
                                    </div>

                                    <div className="flex flex-wrap gap-1 mb-4"></div>

                                    <div className="flex gap-2">
                                       <Button className=" font-SFProText font-medium bg-blue-600 hover:bg-blue-700 cursor-pointer w-full py-2 text-white rounded-lg text-center">
                                          Buy
                                       </Button>
                                       <Button className="rounded-full py-3 px-2 border hover:bg-gray-100">
                                          <ShoppingBag className="h-5 w-5" />
                                          <span className="sr-only">
                                             Add to bag
                                          </span>
                                       </Button>
                                    </div>
                                 </div>
                              </CartWrapper>
                           </div>
                        );
                     })}
                  </div>
               )}
            </div>
         </div>
      </div>
   );
};

export default StorePage;
