'use client';

import Image from 'next/image';
import svgs from '@assets/svgs';
import { useEffect, useRef, useState } from 'react';
import { PiUserCircleFill } from 'react-icons/pi';
import { AnimatePresence, motion } from 'framer-motion';
import { useGetCategoriesAsyncQuery } from '~/infrastructure/services/category.service';
import { CategoryResponseType } from '~/domain/types/category.type';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useRouter } from 'next/navigation';
import Search from './_components/search';
import UserMenu from './_components/user-menu';
import BasketMenu from './_components/basket-menu';
import { categoryList } from './_data/category-list';
import { exploreIphoneList } from './_data/explore-iphone-list';

const containerVariants = {
   hidden: {},
   visible: {
      transition: {
         staggerChildren: 0.1,
         delayChildren: 0.1,
      },
   },
   exit: {
      transition: {
         staggerChildren: 0.08,
         staggerDirection: -1,
      },
   },
};

const columnVariants = {
   hidden: {
      opacity: 0,
      y: -20,
   },
   visible: (custom: number) => ({
      opacity: 1,
      y: 0,
      transition: {
         duration: 0.4,
         delay: custom * 0.1,
         ease: 'easeOut',
      },
   }),
   exit: (custom: number) => ({
      opacity: 0,
      y: 20,
      transition: {
         duration: 0.3,
         delay: custom * 0.08,
         ease: 'easeIn',
      },
   }),
};

const Header = () => {
   const [activeCategory, setActiveCategory] = useState<string | null>(null);
   const router = useRouter();

   const { items } = useAppSelector((state) => state.cart.value);

   return (
      <header
         className="relative w-full bg-[#fafafc]"
         onMouseLeave={() => {
            setActiveCategory(null);
         }}
      >
         <div className="flex flex-row items-center w-[1180px] h-[44px] px-[22px] mx-auto">
            <ul className="main-category flex flex-row justify-between items-center w-full">
               <div
                  className="px-[8px] cursor-pointer"
                  onClick={() => {
                     router.push('/home');
                  }}
               >
                  <Image
                     src={svgs.appleIcon}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className="w-[22px] h-[44px]"
                  />
               </div>

               {categoryList.map((item) => {
                  return (
                     <li
                        key={item.id}
                        className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                        onClick={() => {
                           router.push(item.navigate);
                        }}
                        onMouseEnter={() => {
                           setActiveCategory(item.name);
                        }}
                     >
                        <p className="antialiased opacity-[0.8] tracking-wide">
                           {item.name}
                        </p>
                     </li>
                  );
               })}

               <div
                  className="px-[8px] cursor-pointer"
                  onClick={() => setActiveCategory('Search')}
               >
                  <Image
                     src={svgs.appleSearchIcon}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className="w-[22px] h-[44px]"
                  />
               </div>
               <div
                  className="px-[8px] cursor-pointer relative"
                  onClick={() => {
                     setActiveCategory('BagMenu');
                  }}
               >
                  <Image
                     src={svgs.appleBasketIcon}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className="w-[22px] h-[44px]"
                  />

                  {/* {items.length > 0 && (
                     <span className="absolute top-5 text-[8px] w-4 rounded-full bg-black text-white font-semibold right-1 text-center leading-[16px]">
                        {items.reduce((acc, item) => {
                           return acc + item.quantity;
                        }, 0)}
                     </span>
                  )} */}
               </div>

               <div
                  className="px-[8px] cursor-pointer"
                  onClick={() => {
                     setActiveCategory('UserMenu');
                  }}
               >
                  <PiUserCircleFill className="size-5" />
               </div>
            </ul>

            <AnimatePresence>
               {activeCategory &&
                  activeCategory !== 'Search' &&
                  activeCategory !== 'BagMenu' &&
                  activeCategory !== 'UserMenu' && (
                     <motion.div
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
                        className="absolute top-[44px] left-0 -right-0 bg-[#fafafc] text-black z-[999]"
                        onMouseLeave={() => {
                           setActiveCategory(null);
                        }}
                     >
                        <div className="py-8">
                           <motion.div
                              className="mx-auto w-[980px] flex flex-row justify-between"
                              variants={containerVariants}
                              initial="hidden"
                              animate="visible"
                              exit="exit"
                           >
                              <motion.div
                                 className="w-1/3 pr-8"
                                 variants={columnVariants}
                                 custom={0}
                              >
                                 <h2 className="text-sm font-normal text-[#6e6e73] mb-3">
                                    Explore iPhone
                                 </h2>
                                 <ul className="space-y-2">
                                    {exploreIphoneList.map((item) => {
                                       return (
                                          <li
                                             key={item.name}
                                             className="text-2xl font-semibold text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer"
                                             onClick={() => {
                                                router.push(item.navigate);
                                             }}
                                          >
                                             {item.name}
                                          </li>
                                       );
                                    })}

                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors mt-4 cursor-pointer">
                                       Compare iPhone
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       Switch from Android
                                    </li>
                                 </ul>
                              </motion.div>

                              <motion.div
                                 className="w-1/3 pr-8"
                                 variants={columnVariants}
                                 custom={1}
                              >
                                 <h2 className="text-sm font-normal text-[#6e6e73] mb-3">
                                    Shop iPhone
                                 </h2>
                                 <ul className="space-y-2">
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       Shop iPhone
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       iPhone Accessories
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       Apple Trade In
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       Carrier Deals at Apple
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       Financing
                                    </li>
                                 </ul>
                              </motion.div>

                              <motion.div
                                 className="w-1/3"
                                 variants={columnVariants}
                                 custom={2}
                              >
                                 <h2 className="text-sm font-normal text-[#6e6e73] mb-3">
                                    More from iPhone
                                 </h2>
                                 <ul className="space-y-2">
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       iPhone Support
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       AppleCare+ for iPhone
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       iOS 18
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       Apple Intelligence
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       Apps by Apple
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       iPhone Privacy
                                    </li>
                                    <li className="text-sm text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                                       iCloud+
                                    </li>
                                 </ul>
                              </motion.div>
                           </motion.div>
                        </div>
                     </motion.div>
                  )}

               {activeCategory && activeCategory === 'Search' && <Search />}

               {activeCategory && activeCategory === 'BagMenu' && (
                  <BasketMenu />
               )}

               {activeCategory && activeCategory === 'UserMenu' && <UserMenu />}
            </AnimatePresence>

            {/* Blur overlay for the rest of the page */}
            <AnimatePresence>
               {activeCategory && (
                  <motion.div
                     className="fixed inset-0 bg-[#E8E8ED66] backdrop-blur-md z-40 pointer-events-none"
                     initial={{ opacity: 0 }}
                     animate={{
                        opacity: 1,
                        transition: { duration: 0.3 },
                     }}
                     exit={{
                        opacity: 0,
                        transition: { duration: 0.2 },
                     }}
                     style={{ top: '44px' }}
                  />
               )}
            </AnimatePresence>
         </div>
      </header>
   );
};

export default Header;
