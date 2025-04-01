import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '~/components/ui/carousel';
import Image from 'next/image';
import Link from 'next/link';

const CategoriesCarousel = () => {
   return (
      <Carousel className="w-full max-w-screen relative flex justify-center items-center">
         <CarouselContent className="w-screen mb-5">
            {/* Mac */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-mac-nav-202503?wid=400&hei=260&fmt=png-alpha&.v=1739502780055'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     Mac
                  </span>
               </Link>
            </CarouselItem>

            {/* iPhone */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-iphone-nav-202502_GEO_US?wid=400&hei=260&fmt=png-alpha&.v=1738706422688'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     iPhone
                  </span>
               </Link>
            </CarouselItem>
            {/* iPad */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-ipad-nav-202405?wid=400&hei=260&fmt=png-alpha&.v=1714168620875'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     iPad
                  </span>
               </Link>
            </CarouselItem>

            {/* Apple Watch */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-watch-nav-202409?wid=400&hei=260&fmt=png-alpha&.v=1724165625838'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     Apple Watch
                  </span>
               </Link>
            </CarouselItem>

            {/* HeadPhone */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-airpods-nav-202409?wid=400&hei=260&fmt=png-alpha&.v=1722974349822'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     HeadPhone
                  </span>
               </Link>
            </CarouselItem>

            {/* AirTag */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-airtags-nav-202108?wid=400&hei=260&fmt=png-alpha&.v=1625783380000'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     AirTag
                  </span>
               </Link>
            </CarouselItem>

            {/* Accessories */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-accessories-nav-202503?wid=400&hei=260&fmt=png-alpha&.v=1739502322543'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     Accessories
                  </span>
               </Link>
            </CarouselItem>

            {/* Apple Vision Pro */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-vision-pro-nav-202401?wid=400&hei=260&fmt=png-alpha&.v=1702403595269'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     Apple Vision Pro
                  </span>
               </Link>
            </CarouselItem>

            {/* Apple TV 4K */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-appletv-nav-202210?wid=400&hei=260&fmt=png-alpha&.v=1664628458484'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     Apple TV 4K
                  </span>
               </Link>
            </CarouselItem>

            {/* HomePod */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-homepod-nav-202301?wid=400&hei=260&fmt=png-alpha&.v=1670389216654'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     HomePod
                  </span>
               </Link>
            </CarouselItem>

            {/* Apple Gift Card */}
            <CarouselItem key={1} className="basis-[10%]">
               <Link
                  href="#"
                  className="flex flex-col justify-center items-center"
               >
                  <Image
                     src={
                        'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-holiday-giftcards-asit-agc-nav-202111?wid=400&hei=260&fmt=png-alpha&.v=1653339351054'
                     }
                     width={1000}
                     height={1000}
                     className="w-[100px]"
                     alt="test"
                  />
                  <span className="font-SFProText mt-3 text-sm font-medium">
                     Apple Gift Card
                  </span>
               </Link>
            </CarouselItem>
         </CarouselContent>
         <CarouselPrevious className="absolute top-[15%] left-[1%] bg-[#E5E5E9] disabled:opacity-0 hover:bg-slate-200/80 border-none w-[50px] h-[50px] opacity-0 hover:opacity-90 transition-all ease-linear" />
         <CarouselNext className="absolute top-[15%] right-[1%] bg-[#E5E5E9] hover:bg-slate-200/80 border-none w-[50px] h-[50px] opacity-0 hover:opacity-90 transition-all ease-linear" />
      </Carousel>
   );
};

export default CategoriesCarousel;
