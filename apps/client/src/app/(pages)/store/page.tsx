'use client';

import CategoriesCarousel from '@components/client/categories-carousel';
import PromotionBanner from './_components/promotion-banner';
import { ProductCardSkeleton } from './_components/product-card-skeleton';

import PromotionIPhone from './_components/promotion-iPhone';

export const fakeData = [
   {
      event_product_sku_id: '04edf970-b569-44ac-a116-9847731929ab',
      model_id: '68e403d5617b27ad030bf28f',
      category: {
         id: '68e3fc0c240062be872a0379',
         name: 'iPhone',
         description: 'iPhone categories.',
         order: 0,
         slug: 'iphone',
         parent_id: null,
         created_at: '2025-10-07T09:56:06.1838202+07:00',
         updated_at: '2025-10-07T09:56:06.1838619+07:00',
         modified_by: null,
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
         name: '128GB',
         normalized_name: '128GB',
         order: 0,
      },
      original_price: 1300,
      discount_type: 'PERCENTAGE',
      discount_value: 0.1,
      discount_amount: 130,
      final_price: 1170,
      stock: 10,
      sold: 5,
      image_url:
         'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
      model_slug: 'iphone-15',
   },
];

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
