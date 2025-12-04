import PopularProduct from '@components/client/popular-product';
import useProductService from '@components/hooks/api/use-product-service';
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
      <div className={cn('w-full py-8 px-3 md:px-4')}>
         <div className="mx-auto max-w-7xl">
            <h2
               className={cn(
                  'mb-8 text-center font-semibold tracking-tight text-gray-900 dark:text-gray-100 text-2xl md:text-3xl',
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
               <CarouselContent className={cn('-ml-4 pl-2 md:pl-0')}>
                  {productItems.length > 0
                     ? productItems.map((product) => (
                          <CarouselItem
                             key={product.id}
                             className={cn(
                                'pl-4 p-5 basis-full md:basis-[80%] lg:basis-[40%]',
                             )}
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
               <CarouselPrevious className={cn('hidden md:flex')} />
               <CarouselNext className={cn('hidden md:flex')} />
            </Carousel>
         </div>
      </div>
   );
};

export default PopularProducts;
