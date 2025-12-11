'use client';

import { motion } from 'framer-motion';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import '/globals.css';

import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '@components/ui/carousel';

import LatestItem from './_components/latest-item';

import CompareIPhoneSection from '@components/client/compare-iphone-section';
import ExperienceItem from '~/app/_components/experience-item';
import NewestProductSection from './_components/newest-product-section';
import PopularProductSection from './_components/popular-product-section';

const listLatestItem = [
   {
      id: 1,
      checkPreOrder: false,
      title: 'iPhone 16 Pro',
      subtitle: 'Apple Intelligence',
      price: 'From $999 or $41.62/mo. for 24 mo.*',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-iphone-16-pro-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1726165763260',
      checkLightImg: false,
   },
   {
      id: 2,
      checkPreOrder: true,
      title: 'iPad mini',
      subtitle: 'Apple Intelligence',
      price: 'From $499 or $41.58/mo. for 12 mo.*',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-mini-202410?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1727814498187',
      checkLightImg: true,
   },
   {
      id: 3,
      checkPreOrder: false,
      title: 'Apple Watch Series 10',
      subtitle: 'Thinstant classic.',
      price: 'From $399 or $33.25/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-watch-s10-202409?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1724095131742',
      checkLightImg: true,
   },
   {
      id: 4,
      checkPreOrder: false,
      title: 'iPhone 16',
      subtitle: 'Apple Intelligence',
      price: 'From $799 or $33.29/mo. for 24 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-iphone-16-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1725661572513',
      checkLightImg: false,
   },
   {
      id: 5,
      checkPreOrder: false,
      title: 'AirPods 4',
      subtitle: 'Iconic. Now supersonic.',
      price: 'Starting at $129 with Active Noise Cancellation $179',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-airpods-202409?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1722974321259',
      checkLightImg: true,
   },
   {
      id: 6,
      checkPreOrder: false,
      title: 'Apple Watch Ultra 2',
      subtitle: 'New finish. Never quit.',
      price: 'From $799 or $66.58/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-watch-ultra-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1725655432734',
      checkLightImg: false,
   },
   {
      id: 7,
      checkPreOrder: false,
      title: 'Apple Vision Pro',
      subtitle: 'Welcome to spatial computing.',
      price: 'From $3499 or $291.58/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-vision-pro-202401?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1705097770616',
      checkLightImg: true,
   },
   {
      id: 8,
      checkPreOrder: false,
      title: 'iPad Air',
      subtitle: 'Apple Intelligence',
      price: 'From $599 or $49.91/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-air-202405?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1713308272877',
      checkLightImg: true,
   },
   {
      id: 9,
      checkPreOrder: false,
      title: 'MacBook Pro',
      subtitle: 'Mind-blowing. Head-turning.',
      price: 'From $1599 or $133.25/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-macbook-pro-202310?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1696964122967',
      checkLightImg: true,
   },
   {
      id: 10,
      checkPreOrder: false,
      title: 'iPad Pro',
      subtitle: 'Apple Intelligence',
      price: 'From $999 or $83.25/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-pro-202405?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1713308272816',
      checkLightImg: false,
   },
   {
      id: 11,
      checkPreOrder: false,
      title: 'MacBook Air',
      subtitle: 'Designed to go places.',
      price: 'From $999 or $83.25/mo. for 12 mo.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-macbook-air-202402?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1707259289595',
      checkLightImg: true,
   },
];

const listExperienceItem = [
   {
      id: 1,
      subtitle: 'Apple TV+',
      title: 'Watch new Apple Originals every month.**',
      content: '',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-tv-services-202409?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1727220094622',
      checkLightImg: true,
   },
   {
      id: 2,
      subtitle: '',
      title: 'Six Apple services. One easy subscription.',
      content: '',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-subscriptions-202108_GEO_US?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1626375546000',
      checkLightImg: true,
   },
   {
      id: 3,
      subtitle: '',
      title: "We've got you covered.",
      content:
         'AppleCare+ now comes with unlimited repairs for accidental damage protection.',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-applecare-202409?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1723747544269',
      checkLightImg: true,
   },
   {
      id: 4,
      subtitle: '',
      title: 'Discover all the ways to use Apple Pay.',
      content: '',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-applepay-202409_GEO_US?wid=960&hei=1000&fmt=jpeg&qlt=90&.v=1725374577628',
      checkLightImg: true,
   },
   {
      id: 5,
      subtitle: 'home',
      title: 'See how one app can control your entire home.',
      content: '',
      img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-homekit-202405_GEO_US?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1715960298510',
      checkLightImg: true,
   },
];

const HomePage = () => {
   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full flex flex-col items-center justify-center bg-[#f5f5f7]',
         )}
      >
         <div
            className={cn(
               'hero-container w-full mx-auto flex px-4 py-12 flex-col gap-10 mb-16 md:max-w-[1264px] md:h-screen md:px-8 md:mb-[100px] md:flex-row md:gap-10',
            )}
         >
            <div
               className={cn(
                  'hero-content bg-transparent relative z-10 flex flex-col w-full items-center text-center gap-6 md:basis-1/2 md:h-full md:items-start md:text-left md:gap-[30px]',
               )}
            >
               <div
                  className={cn(
                     'content-header font-medium text-[#3a3a3a] text-[36px] leading-[42px] tracking-[-1px] px-2 sm:text-[48px] sm:leading-[56px] sm:tracking-[-1.5px] md:max-w-[520px] md:mt-6 md:text-[72px] md:leading-[80px] md:tracking-[-2.5px] lg:max-w-[600px] lg:mt-8 lg:text-[94px] lg:leading-[97px] lg:tracking-[-3.76px]',
                  )}
               >
                  Discover the latest Apple products and innovations
               </div>
               <div
                  className={cn(
                     'content-body text-[#425466] max-w-[640px] px-2 text-[16px] leading-[26px] text-center md:max-w-[540px] md:px-0 md:pl-2 md:pr-12 md:text-[18px] md:leading-[28px] md:tracking-[0.2px] md:text-left',
                  )}
               >
                  Explore the newest Apple devices and accessories designed to
                  elevate the way you work, create, and play—delivered with the
                  innovation and quality you expect.
               </div>
            </div>
            <div
               className={cn(
                  'hero-masonry bg-transparent relative z-10 w-full h-[360px] sm:h-[420px] md:basis-1/2 md:h-full',
               )}
            >
               <div className="bg-transparent h-full w-full absolute z-10">
                  <div className="bg-[linear-gradient(0deg,rgba(1,0,2,0),#f5f5f7)] h-10 w-full absolute top-0"></div>
                  <div className="bg-[linear-gradient(180deg,rgba(1,0,2,0),#f5f5f7)] h-10 w-full absolute bottom-0"></div>
               </div>
               <div className="overflow-hidden h-full relative z-0 px-1">
                  <motion.div
                     animate={{ y: ['0px', '-3384px'] }}
                     transition={{
                        y: {
                           duration: 60,
                           repeat: Infinity,
                           ease: 'linear',
                        },
                     }}
                     className="preview-masonry bg-transparent max-w-[600px] mx-auto"
                  >
                     <div className="grid grid-cols-[repeat(2,minmax(100px,285px))] grid-rows-[masonry] grid-flow-dense gap-[20px]">
                        <div className="bg-[url('/images/hero-imgs/picture01-half.jpg')] bg-cover bg-no-repeat bg-bottom rounded-[0_0_20px_20px] h-[168px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture02.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture03.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture04.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture05.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture06.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture07.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture08.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture09.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture10.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture11.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture12.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture13.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture14.jpg')] bg-cover bg-no-repeat bg-top row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture15.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture16.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture17.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture18.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture01.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture02.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture03.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture04.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture05.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture06.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                     </div>
                  </motion.div>
               </div>
            </div>
         </div>

         {/* NEWEST PRODUCTS */}
         <NewestProductSection />

         {/* POPULAR PRODUCTS */}
         <PopularProductSection />

         <div className="list-item-container w-full mb-[100px] flex flex-col justify-center items-center">
            <div className="w-full max-w-screen mb-[50px]">
               <div
                  className={cn(
                     'title-list w-full pb-[24px] px-6 text-center md:pl-[140px] md:text-left',
                  )}
               >
                  <span
                     className={cn(
                        'text-[24px] md:text-[28px] font-semibold text-[#1d1d1f] leading-[1.14] tracking-[0.007em]',
                     )}
                  >
                     The latest.{' '}
                  </span>
                  <span
                     className={cn(
                        'text-[24px] md:text-[28px] font-semibold text-[#6e6e73] leading-[1.14] tracking-[0.007em]',
                     )}
                  >
                     Take a look at what’s new, right now.
                  </span>
               </div>
               <Carousel className="w-full max-w-screen pl-5 relative flex justify-center items-center">
                  <CarouselContent
                     className={cn('w-full mb-5 px-4 md:pl-[140px] md:px-0')}
                  >
                     {listLatestItem.map((product, index) => {
                        return (
                           <CarouselItem
                              key={index}
                              className={cn(
                                 'mr-0 basis-full sm:basis-[50%] md:basis-[45%] lg:basis-[40%] xl:basis-[35%]',
                              )}
                           >
                              <LatestItem product={product} />
                           </CarouselItem>
                        );
                     })}
                  </CarouselContent>
                  <CarouselPrevious
                     className={cn(
                        'absolute -top-[40px] -translate-y-1/2 border-black bg-[#ccc] hidden sm:flex sm:left-[60%] md:left-[90%]',
                     )}
                  />
                  <CarouselNext
                     className={cn(
                        'absolute -top-[40px] -translate-y-1/2 border-black bg-[#ccc] hidden sm:flex sm:right-[10%] md:right-[5%]',
                     )}
                  />
               </Carousel>
            </div>

            <div className="experience-container w-full max-w-screen">
               <div
                  className={cn(
                     'title-list w-full pb-[24px] px-6 text-center md:pl-[140px] md:text-left',
                  )}
               >
                  <span
                     className={cn(
                        'text-[24px] md:text-[28px] font-semibold text-[#1d1d1f] leading-[1.14] tracking-[0.007em]',
                     )}
                  >
                     The Apple experience.{' '}
                  </span>
                  <span
                     className={cn(
                        'text-[24px] md:text-[28px] font-semibold text-[#6e6e73] leading-[1.14] tracking-[0.007em]',
                     )}
                  >
                     Do even more with Apple products and services.
                  </span>
               </div>
               <Carousel className="w-full max-w-screen pl-5 relative flex justify-center items-center">
                  <CarouselContent
                     className={cn('w-full mb-5 px-4 md:pl-[140px] md:px-0')}
                  >
                     {listExperienceItem.map((item, index) => {
                        return (
                           <CarouselItem
                              key={index}
                              className={cn(
                                 'mr-0 basis-full sm:basis-[50%] md:basis-[45%] lg:basis-[40%] xl:basis-[35%]',
                              )}
                           >
                              <ExperienceItem experience={item} />
                           </CarouselItem>
                        );
                     })}
                  </CarouselContent>
                  <CarouselPrevious
                     className={cn(
                        'absolute -top-[40px] -translate-y-1/2 border-black bg-[#ccc] hidden sm:flex sm:left-[60%] md:left-[90%]',
                     )}
                  />
                  <CarouselNext
                     className={cn(
                        'absolute -top-[40px] -translate-y-1/2 border-black bg-[#ccc] hidden sm:flex sm:right-[10%] md:right-[5%]',
                     )}
                  />
               </Carousel>
            </div>
         </div>
         <CompareIPhoneSection />
      </div>
   );
};

export default HomePage;
