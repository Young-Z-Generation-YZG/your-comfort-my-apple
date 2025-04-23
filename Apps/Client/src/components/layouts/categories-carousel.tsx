import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '@components/ui/carousel';
import Image from 'next/image';
import Link from 'next/link';

interface CategoryItem {
   category_name: string;
   category_image: string;
}

const listCategoryItem: CategoryItem[] = [
   {
      category_name: 'Mac',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-mac-nav-202503?wid=400&hei=260&fmt=png-alpha&.v=1739502780055',
   },
   {
      category_name: 'iPhone',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-iphone-nav-202502_GEO_US?wid=400&hei=260&fmt=png-alpha&.v=1738706422688',
   },
   {
      category_name: 'iPad',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-ipad-nav-202405?wid=400&hei=260&fmt=png-alpha&.v=1714168620875',
   },
   {
      category_name: 'Apple Watch',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-watch-nav-202409?wid=400&hei=260&fmt=png-alpha&.v=1724165625838',
   },
   {
      category_name: 'HeadPhone',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-airpods-nav-202409?wid=400&hei=260&fmt=png-alpha&.v=1722974349822',
   },
   {
      category_name: 'AirTag',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-airtags-nav-202108?wid=400&hei=260&fmt=png-alpha&.v=1625783380000',
   },
   {
      category_name: 'Accessories',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-accessories-nav-202503?wid=400&hei=260&fmt=png-alpha&.v=1739502322543',
   },
   {
      category_name: 'Apple Vision Pro',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-vision-pro-nav-202401?wid=400&hei=260&fmt=png-alpha&.v=1702403595269',
   },
   {
      category_name: 'Apple TV 4K',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-appletv-nav-202210?wid=400&hei=260&fmt=png-alpha&.v=1664628458484',
   },
   {
      category_name: 'HomePod',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-homepod-nav-202301?wid=400&hei=260&fmt=png-alpha&.v=1670389216654',
   },
   {
      category_name: 'Apple Gift Card',
      category_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-13-holiday-giftcards-asit-agc-nav-202111?wid=400&hei=260&fmt=png-alpha&.v=1653339351054',
   },
];

const CategoriesCarousel = () => {
   return (
      <Carousel className="w-full max-w-screen relative flex justify-center items-center">
         <CarouselContent className="w-screen mb-5">
            {listCategoryItem.length > 0 &&
               listCategoryItem.map((item, index) => {
                  return (
                     <CarouselItem key={index} className="basis-[10%]">
                        <Link
                           href="#"
                           className="flex flex-col justify-center items-center"
                        >
                           <Image
                              src={
                                 item.category_image ||
                                 'https://via.placeholder.com/150'
                              }
                              width={1000}
                              height={1000}
                              className="w-[100px]"
                              alt="test"
                           />
                           <span className="font-SFProText mt-3 text-sm font-medium">
                              {item.category_name}
                           </span>
                        </Link>
                     </CarouselItem>
                  );
               })}
         </CarouselContent>
         <CarouselPrevious className="absolute top-[15%] left-[1%] bg-[#E5E5E9] disabled:opacity-0 hover:bg-slate-200/80 border-none w-[50px] h-[50px] opacity-0 hover:opacity-90 transition-all ease-linear" />
         <CarouselNext className="absolute top-[15%] right-[1%] bg-[#E5E5E9] hover:bg-slate-200/80 border-none w-[50px] h-[50px] opacity-0 hover:opacity-90 transition-all ease-linear" />
      </Carousel>
   );
};

export default CategoriesCarousel;
