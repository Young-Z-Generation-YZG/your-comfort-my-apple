'use client';

import CategoriesCarousel from '@components/client/categories-carousel';
import PromotionBanner from './_components/promotion-banner';
import { ProductCardSkeleton } from './_components/product-card-skeleton';

import PromotionIPhone from './_components/promotion-iPhone';

const StorePage = () => {
   const isLoading = true;

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
                  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                     {/* <ProductGridSkeleton count={5} className="" /> */}
                     <ProductCardSkeleton />
                     <ProductCardSkeleton />
                     <ProductCardSkeleton />
                     <ProductCardSkeleton />
                     <PromotionIPhone />
                  </div>
               ) : (
                  <div className="grid grid-cols-3 gap-x-4 gap-y-8 mt-5">
                     {/* {products.map((item, index) => {
                        return <PromotionIPhone index={index} item={item} />;
                     })} */}
                  </div>
               )}
            </div>
         </div>
      </div>
   );
};

export default StorePage;
