'use client';

import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '@components/ui/carousel';

import NewestProduct from '@components/client/newest-product';
import useProductService from '@components/hooks/api/use-product-service';
import { useEffect, useMemo } from 'react';

const NewestProducts = () => {
   const { getNewestProductsAsync, getNewestProductsState, isLoading } =
      useProductService();

   const productItems = useMemo(() => {
      return getNewestProductsState.data?.items ?? [];
   }, [getNewestProductsState.data]);

   useEffect(() => {
      const fetchNewestProducts = async () => {
         await getNewestProductsAsync();
      };
      fetchNewestProducts();
   }, [getNewestProductsAsync]);

   return (
      <div className="w-full px-4 py-8">
         <div className="mx-auto max-w-7xl">
            <h2 className="mb-8 text-center text-3xl font-semibold tracking-tight text-gray-900 dark:text-gray-100">
               Newest Products
            </h2>

            <Carousel
               opts={{
                  align: 'start',
                  loop: true,
               }}
               className="w-full"
            >
               <CarouselContent className="-ml-4">
                  {productItems.length > 0
                     ? productItems.map((product) => (
                          <CarouselItem
                             key={product.id}
                             className="pl-4 p-5 md:basis-1/2 lg:basis-1/2 xl:basis-1/3"
                          >
                             <NewestProduct product={product} />
                          </CarouselItem>
                       ))
                     : !isLoading && (
                          <div className="col-span-full text-center py-12 text-gray-500">
                             No newest products available
                          </div>
                       )}
               </CarouselContent>
               <CarouselPrevious className="hidden md:flex" />
               <CarouselNext className="hidden md:flex" />
            </Carousel>
         </div>
      </div>
   );
};

export default NewestProducts;
