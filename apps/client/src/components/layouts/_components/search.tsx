'use client';

import { MdArrowRightAlt } from 'react-icons/md';
import { motion } from 'framer-motion';
import svgs from '@assets/svgs';
import Image from 'next/image';
import { useEffect, useState } from 'react';
import { useDebounce } from '~/hooks/use-debouce';
import { CldImage } from 'next-cloudinary';
import { useRouter } from 'next/navigation';
import { useAppSelector } from '~/infrastructure/redux/store';
import {
   SearchLink,
   SearchProduct,
} from '~/infrastructure/redux/features/search.slice';

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

const linksDemo = [
   {
      label: 'iPhone 16 and iPhone 16 Plus',
      link: '#',
   },
   {
      label: 'iPhone 16 Pro and iPhone 16 Pro Max',
      link: '#',
   },
   {
      label: 'iPhone 16e',
      link: '#',
   },
];

const Search = () => {
   const [loading, setLoading] = useState(false);
   const [isSearchOpen, setIsSearchOpen] = useState(false);
   const [query, setQuery] = useState('');
   const [links, setLinks] = useState<SearchLink[]>([]);
   const [searchProducts, setSearchProducts] = useState<SearchProduct[]>([]);

   const router = useRouter();

   const debouncedQuery = useDebounce(query, 400);

   const searchSlice = useAppSelector((state) => state.search.value);

   const clearSearch = () => {
      setQuery('');
      setSearchProducts([]);
      setLinks([]);
   };

   useEffect(() => {
      if (debouncedQuery.length > 2) {
         setLoading(true);

         const searchProducts = searchSlice.searchProducts.filter((product) =>
            product.name.toLowerCase().includes(debouncedQuery.toLowerCase()),
         );

         const groupedProducts = searchProducts.reduce<
            Record<string, SearchProduct[]>
         >((acc, product) => {
            const key = product.slug;

            acc[key] = acc[key] || [];
            acc[key].push(product);

            return acc;
         }, {});

         setSearchProducts(searchProducts);

         const searchLinks = searchSlice.searchLinks.filter((link) =>
            link.label.toLowerCase().includes(debouncedQuery.toLowerCase()),
         );

         setLinks(searchLinks);
         return;
      }

      setLoading(false);
   }, [debouncedQuery]);

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
         <div className="py-8">
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
                        onFocus={() => setIsSearchOpen(true)}
                        className="w-full h-10 bg-[#fafafc] outline-none text-2xl font-medium font-SFProText"
                        placeholder="Search ygzStore.com"
                     />
                  </div>
               </div>
               <div>
                  {debouncedQuery.length >= 2 && true && links.length > 0 ? (
                     <div>
                        <h3 className="text-xs text-slate-500 font-SFProText">
                           Quick links
                        </h3>

                        <ul className="pt-2">
                           {links.map((link, index) => {
                              return (
                                 <li
                                    key={index}
                                    className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600"
                                 >
                                    <MdArrowRightAlt className="size-4" />
                                    <p>{link.label}</p>
                                 </li>
                              );
                           })}
                        </ul>
                     </div>
                  ) : (
                     <div>
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
                  )}
               </div>
               <div>
                  {debouncedQuery.length >= 2 && searchProducts.length > 0 ? (
                     <div className="pb-5">
                        <h3 className="text-xs text-slate-500 font-SFProText">
                           Suggest searches for "{debouncedQuery}"
                        </h3>
                        <ul className="py-2 flex flex-col gap-2 max-h-[200px] overflow-y-auto">
                           {searchProducts.map((product, index) => {
                              return (
                                 <li
                                    key={index}
                                    className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3"
                                    onClick={() => {
                                       router.push(`model/${product.slug}`);
                                    }}
                                 >
                                    <div className="w-[60px] h-[60px] overflow-hidden">
                                       <CldImage
                                          src={`${product.image}`}
                                          alt={'test'}
                                          width={500}
                                          height={500}
                                          className="h-[180%] w-full object-cover translate-y-[-24px]"
                                       />
                                    </div>

                                    <div className="flex flex-col">
                                       <p>{product.name}</p>
                                       <div className="">
                                          <div className="py-1">
                                             <p className="text-lg font-semibold inline-block mr-2">
                                                $
                                                {product.promotion_price.toFixed(
                                                   2,
                                                )}
                                             </p>
                                             <div className="inline-block">
                                                <p className="line-through text-sm inline-block mr-2">
                                                   $
                                                   {product.unit_price.toFixed(
                                                      2,
                                                   )}
                                                </p>
                                                <p className="inline-block">
                                                   -
                                                   {product.promotion_rate *
                                                      100}
                                                   %
                                                </p>
                                             </div>
                                          </div>
                                       </div>
                                       <p className="text-red-500 font-semibold text-sm">
                                          On promotion
                                       </p>
                                    </div>
                                 </li>
                              );
                           })}
                        </ul>
                     </div>
                  ) : null}
               </div>
            </motion.div>
         </div>
      </motion.div>
   );
};

export default Search;
