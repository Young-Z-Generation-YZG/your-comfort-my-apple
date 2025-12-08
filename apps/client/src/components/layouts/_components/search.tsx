'use client';

import { MdArrowRightAlt } from 'react-icons/md';
import { motion } from 'framer-motion';
import svgs from '@assets/svgs';
import Image from 'next/image';
import { useEffect, useMemo, useState } from 'react';
import { useDebounce } from '@components/hooks/use-debounce';
import { useRouter } from 'next/navigation';
import useProductService from '@components/hooks/api/use-product-service';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { TModel, TProductModel } from '~/domain/types/catalog.type';

// Staggered animation variants
const containerVariants = {
   hidden: {},
   visible: {
      transition: {
         staggerChildren: 0.1,
         delayChildren: 0.1,
      },
   },
};

const Search = () => {
   const router = useRouter();
   const [query, setQuery] = useState('');
   const debouncedQuery = useDebounce(query, 500);

   const { getProductModelsAsync, getProductModelsState, isLoading } =
      useProductService();

   const productModelsData = useMemo(() => {
      return getProductModelsState.data?.items ?? [];
   }, [getProductModelsState.data]);

   useEffect(() => {
      if (debouncedQuery.length > 2) {
         getProductModelsAsync({
            _textSearch: debouncedQuery,
            _limit: 999,
         });
      }
   }, [debouncedQuery, getProductModelsAsync]);

   const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
      if (e.key === 'Enter') {
         const trimmedQuery = query.trim();
         if (trimmedQuery) {
            router.push(
               `/shop?_textSearch=${encodeURIComponent(trimmedQuery)}`,
            );
         } else {
            router.push('/shop');
         }
      }
   };

   return (
      <motion.div
         className="sub-category absolute top-[44px] left-0 w-full bg-[#fafafc] text-black z-50"
         initial={{ height: 0, opacity: 0 }}
         animate={{
            height: 'auto',
            opacity: 1,
            transition: {
               height: { duration: 0.5 },
               opacity: { duration: 0.3, delay: 0.1 },
            },
         }}
         exit={{
            height: 0,
            opacity: 0,
            transition: {
               height: { duration: 0.6 },
               opacity: { duration: 0.3 },
            },
         }}
      >
         <div className="py-8 px-5">
            <motion.div
               className="mx-auto w-[980px]"
               variants={containerVariants}
               initial="hidden"
               animate="visible"
            >
               <div className="font-SFProText">
                  <div className="flex items-center pb-5">
                     <Image
                        src={svgs.appleSearchIcon}
                        alt="cover"
                        width={1200}
                        height={1000}
                        quality={100}
                        className="w-[60px] h-[60px]"
                     />
                     <input
                        type="text"
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        onKeyDown={handleKeyDown}
                        className="w-full h-10 bg-[#fafafc] outline-none text-2xl font-medium font-SFProText"
                        placeholder="Search ygzStore.com"
                     />
                  </div>
               </div>
               <LoadingOverlay isLoading={isLoading}>
                  <div>
                     {productModelsData.length > 0 && (
                        <ul className="py-2 flex flex-col gap-2 max-h-[200px] overflow-y-auto">
                           <h3 className="text-xs text-slate-500 font-SFProText mb-5">
                              Suggested searches for &quot;
                              {debouncedQuery}
                              &quot;
                           </h3>

                           {productModelsData.map(
                              (item: TProductModel, index) => {
                                 const prices = item.sku_prices.map(
                                    (sku: any) => sku.unit_price,
                                 );
                                 const minPrice = Math.min(...prices);
                                 const maxPrice = Math.max(...prices);
                                 const displayName = item.model_items
                                    .map((model: TModel) => model.name)
                                    .join(' & ');

                                 return (
                                    <div key={index} className="">
                                       <li
                                          className="flex items-center gap-3 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:bg-gray-100 p-2 rounded-lg transition-colors"
                                          onClick={() => {
                                             router.push(
                                                `/shop/iphone/${item.slug}`,
                                             );
                                          }}
                                       >
                                          <div className="w-[60px] h-[60px] overflow-hidden flex-shrink-0">
                                             <Image
                                                src={
                                                   item.showcase_images[0]
                                                      .image_url
                                                }
                                                alt={displayName}
                                                width={500}
                                                height={500}
                                                className="h-[180%] w-full object-cover translate-y-[-24px]"
                                             />
                                          </div>

                                          <div className="flex flex-col">
                                             <p className="text-sm font-semibold mb-1">
                                                {displayName}
                                             </p>
                                             <div className="flex items-center gap-2">
                                                <p className="text-base font-bold">
                                                   {minPrice === maxPrice
                                                      ? `$${minPrice.toLocaleString()}`
                                                      : `$${minPrice.toLocaleString()} - $${maxPrice.toLocaleString()}`}
                                                </p>
                                             </div>
                                          </div>
                                       </li>
                                    </div>
                                 );
                              },
                           )}
                        </ul>
                     )}

                     <h3 className="text-xs text-slate-500 font-SFProText">
                        Quick links
                     </h3>

                     <ul className="pt-2">
                        <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                           <MdArrowRightAlt className="size-4" />
                           <p>Find a Store</p>
                        </li>
                        <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                           <MdArrowRightAlt className="size-4" />
                           <p>Apple Vision Pro</p>
                        </li>
                        <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                           <MdArrowRightAlt className="size-4" />
                           <p>HeadPhones</p>
                        </li>
                        <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                           <MdArrowRightAlt
                              className="size-4"
                              aria-hidden="true"
                           />
                           <p>Apple Intelligence</p>
                        </li>
                     </ul>
                  </div>
               </LoadingOverlay>
            </motion.div>
         </div>
      </motion.div>
   );
};

export default Search;
