import PopularProduct from '@components/client/popular-product';
import useProductService from '@components/hooks/api/use-product-service';
import usePagination from '@components/hooks/use-pagination';
import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '@components/ui/carousel';
import { useEffect } from 'react';

const PopularProducts = () => {
   const { getPopularProductsAsync, getPopularProductsState, isLoading } =
      useProductService();

   const {
      currentPage,
      totalPages,
      pageSize,
      totalRecords,
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,
      paginationItems,
      getPageNumbers,
   } = usePagination(
      getPopularProductsState.isSuccess &&
         getPopularProductsState.data &&
         getPopularProductsState.data.items.length > 0
         ? getPopularProductsState.data
         : {
              total_records: 0,
              total_pages: 0,
              page_size: 0,
              current_page: 0,
              items: [],
              links: {
                 first: null,
                 last: null,
                 prev: null,
                 next: null,
              },
           },
   );

   useEffect(() => {
      const fetchPopularProducts = async () => {
         await getPopularProductsAsync();
      };
      fetchPopularProducts();
   }, [getPopularProductsAsync]);

   return (
      <div className="w-full px-4 py-8">
         <div className="mx-auto max-w-7xl">
            <h2 className="mb-8 text-center text-3xl font-semibold tracking-tight text-gray-900 dark:text-gray-100">
               Popular Products
            </h2>

            <Carousel
               opts={{
                  align: 'start',
                  loop: true,
               }}
               className="w-full"
            >
               <CarouselContent className="-ml-4">
                  {paginationItems.map((product) => (
                     <CarouselItem
                        key={product.id}
                        className="pl-4 md:basis-1/2 lg:basis-1/2 xl:basis-1/3"
                     >
                        <PopularProduct product={product} />
                     </CarouselItem>
                  ))}
               </CarouselContent>
               <CarouselPrevious className="hidden md:flex" />
               <CarouselNext className="hidden md:flex" />
            </Carousel>
         </div>
      </div>
   );
};

export default PopularProducts;
