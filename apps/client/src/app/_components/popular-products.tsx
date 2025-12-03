import PopularProduct from '@components/client/popular-product';
import useProductService from '@components/hooks/api/use-product-service';
import useMediaQuery from '@components/hooks/use-media-query';
import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '@components/ui/carousel';
import { cn } from '~/infrastructure/lib/utils';
import { useEffect, useMemo } from 'react';

const PopularProducts = () => {
   const { getPopularProductsAsync, getPopularProductsState, isLoading } =
      useProductService();

   const { isMobile, isTablet } = useMediaQuery();
   const isSmallScreen = isMobile || isTablet;

   const productItems = useMemo(() => {
      return getPopularProductsState.data?.items ?? [];
   }, [getPopularProductsState.data]);

   useEffect(() => {
      const fetchPopularProducts = async () => {
         await getPopularProductsAsync();
      };
      fetchPopularProducts();
   }, [getPopularProductsAsync]);

   return (
      <div
         className={cn('w-full py-8', {
            'px-4': !isMobile,
            'px-3': isMobile,
         })}
      >
         <div className="mx-auto max-w-7xl">
            <h2
               className={cn(
                  'mb-8 text-center font-semibold tracking-tight text-gray-900 dark:text-gray-100',
                  {
                     'text-3xl': !isMobile,
                     'text-2xl': isMobile,
                  },
               )}
            >
               Popular Products
            </h2>

            <Carousel
               opts={{
                  align: 'start',
                  loop: true,
               }}
               className="w-full"
            >
               <CarouselContent
                  className={cn('-ml-4', {
                     'pl-2': isSmallScreen,
                  })}
               >
                  {productItems.length > 0
                     ? productItems.map((product) => (
                          <CarouselItem
                             key={product.id}
                             className={cn('pl-4 p-5', {
                                'md:basis-1/2 lg:basis-1/2 xl:basis-1/3':
                                   !isSmallScreen,
                                'basis-[80%]': isTablet,
                                'basis-full': isMobile,
                             })}
                          >
                             <PopularProduct product={product} />
                          </CarouselItem>
                       ))
                     : !isLoading && (
                          <div className="col-span-full text-center py-12 text-gray-500">
                             No popular products available
                          </div>
                       )}
               </CarouselContent>
               <CarouselPrevious
                  className={cn('md:flex', {
                     hidden: isSmallScreen,
                     flex: !isSmallScreen,
                  })}
               />
               <CarouselNext
                  className={cn('md:flex', {
                     hidden: isSmallScreen,
                     flex: !isSmallScreen,
                  })}
               />
            </Carousel>
         </div>
      </div>
   );
};

export default PopularProducts;
