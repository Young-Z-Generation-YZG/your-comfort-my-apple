'use client';

import Image from 'next/image';
import svgs from '@assets/svgs';
import { useEffect, useRef, useState } from 'react';
import { PiUserCircleFill } from 'react-icons/pi';
import { AnimatePresence, motion } from 'framer-motion';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useRouter } from 'next/navigation';
import Search from './_components/search';
import UserMenu from './_components/user-menu';
import BasketMenu from './_components/basket-menu';
import { categoryList } from './_data/category-list';
import { exploreIphoneList } from './_data/explore-iphone-list';
import { useGetCategoriesAsyncQuery } from '~/infrastructure/services/category.service';
import { CategoryResponseType } from '~/domain/types/category.type';

const mainCategoriesDefault = [
   {
      category_id: 'Mac',
      category_name: 'Mac',
      category_parent_id: null,
      category_slug: 'mac',
      category_order: 1,
   },
   {
      category_id: 'iPad',
      category_name: 'iPad',
      category_parent_id: null,
      category_slug: 'ipad',
      category_order: 2,
   },
   {
      category_id: 'iPhone',
      category_name: 'iPhone',
      category_parent_id: null,
      category_slug: 'iphone',
      category_order: 3,
   },
   {
      category_id: 'Watch',
      category_name: 'Watch',
      category_parent_id: null,
      category_slug: 'watch',
      category_order: 3,
   },
   {
      category_id: 'HeadPhones',
      category_name: 'HeadPhones',
      category_parent_id: null,
      category_slug: 'headphones',
      category_order: 3,
   },
];

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

const subCategories = ['iPhone 16 Pro', 'iPhone 16', 'iPhone 16e', 'iPhone 15'];

const Header = () => {
   const [activeCategory, setActiveCategory] = useState<string | null>(null);
   const [categories, setCategories] = useState<CategoryResponseType[]>([]);
   const router = useRouter();
   const { items } = useAppSelector((state) => state.cart.value);
   const [menuOpen, setMenuOpen] = useState<boolean>(false);
   const categoryRefs = useRef<{ [key: string]: HTMLLIElement | null }>({});

   const {
      data: categoriesData,
      // error: categoriesError,
      // isLoading: categoriesLoading,
   } = useGetCategoriesAsyncQuery();

   useEffect(() => {
      if (categoriesData) {
         setCategories(categoriesData);
      }
   }, [categoriesData]);

   const renderHeaderCategories02 = () => {
      if (!categories.length) {
         return mainCategoriesDefault.map((category, index) => {
            return (
               <li
                  key={index}
                  className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px] border-red-300"
                  ref={(el) => {
                     categoryRefs.current[category.category_id] = el;
                  }}
                  data-category_id={category}
                  onMouseEnter={() => handleMouseEnter(category.category_name)}
                  onClick={() => {
                     router.push(`/shop`);
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     {category.category_name}
                  </p>
               </li>
            );
         });
      }
   };

   const renderHeaderCategories = () => {
      if (!categories.length) {
         return mainCategoriesDefault.map((category, index) => {
            return (
               <li
                  key={index}
                  className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  ref={(el) => {
                     categoryRefs.current[category.category_id] = el;
                  }}
                  data-category_id={category}
                  onMouseEnter={() => handleMouseEnter(category.category_name)}
                  onClick={() => {
                     router.push('/home');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     {category.category_name}
                  </p>
               </li>
            );
         });
      }

      const sortedCategories = categories
         .filter((category) => category.category_parent_id === null)
         .sort((a, b) => a.category_order - b.category_order);

      return sortedCategories.map((category: CategoryResponseType) => {
         return (
            <li
               key={category.category_id}
               ref={(el) => {
                  categoryRefs.current[category.category_id] = el;
               }}
               data-category_id={category.category_id}
               className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
               onMouseEnter={() => {
                  handleMouseEnter(category.category_name);
               }}
               onClick={() => {
                  router.push(`/shop`);
               }}
            >
               <p className="antialiased opacity-[0.8] tracking-wide">
                  {category.category_name}
               </p>
            </li>
         );
      });
   };

   const renderChildCategories = (parentName: string) => {
      if (!categories.length) {
         return subCategories.map((category, index) => {
            return (
               <li
                  key={index}
                  ref={(el) => {
                     categoryRefs.current[category] = el;
                  }}
                  data-category_id={category}
                  className="text-2xl font-semibold text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer"
               >
                  {category}
               </li>
            );
         });
      }

      const parent = categories.find(
         (category) => category.category_name === parentName,
      );

      const child = categories.filter(
         (category) => category.category_parent_id === parent?.category_id,
      );

      return child.map((category) => {
         return (
            <li
               className="text-2xl font-semibold text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer"
               ref={(el) => {
                  categoryRefs.current[category.category_id] = el;
               }}
               data-category_id={category.category_id}
               key={category.category_id}
            >
               {category.category_name}
            </li>
         );
      });
   };

   const handleMouseEnter = (category: string) => {
      setActiveCategory(category);
   };

   const handleMouseLeave = () => {
      setActiveCategory(null);
   };

   const subcategoryContent: Record<string, React.ReactNode> = {
      iPhone: (
         <motion.div
            className="mx-auto w-[980px] flex flex-row justify-between"
            variants={containerVariants}
            initial="hidden"
            animate="visible"
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
                  <li className="text-2xl font-semibold text-[#1d1d1f] hover:text-[#0066cc] transition-colors cursor-pointer">
                     Explore All iPhone
                  </li>
                  {activeCategory && renderChildCategories(activeCategory)}
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

            <motion.div className="w-1/3" variants={columnVariants} custom={2}>
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
      ),
   };

   return (
      <header
         className="relative w-full bg-[#fafafc]"
         onMouseLeave={handleMouseLeave}
      >
         <div className="flex flex-row items-center max-w-[1180px] h-auto px-[22px] mx-auto">
            <ul className="md:flex hidden main-category flex-row justify-between items-center w-full">
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
               <li
                  className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('/store');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Store
                  </p>
               </li>
               {renderHeaderCategories()}
               <li className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]">
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Vision
                  </p>
               </li>
               <li className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]">
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     TV & Home
                  </p>
               </li>
               <li className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]">
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Entertainment
                  </p>
               </li>
               <li className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]">
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Accessories
                  </p>
               </li>
               <li className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]">
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Support
                  </p>
               </li>
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

                  {items.length > 0 && (
                     <span className="absolute top-5 text-[8px] w-4 rounded-full bg-black text-white font-semibold right-1 text-center leading-[16px]">
                        {items.reduce((acc, item) => {
                           return acc + item.quantity;
                        }, 0)}
                     </span>
                  )}
               </div>

               <div
                  className="px-[8px] cursor-pointer"
                  onClick={() => {
                     setActiveCategory('UserMenu');
                  }}
               >
                  <PiUserCircleFill className="size-5" aria-hidden="true" />
               </div>
            </ul>

            {/* Header mobile version */}
            <div className="md:hidden flex flex-row justify-between items-center w-full h-[44px] px-4 bg-[#fafafc] border-b border-[#ddd]">
               {/* Logo */}
               <div
                  className="cursor-pointer"
                  onClick={() => {
                     router.push('/home');
                  }}
               >
                  <Image
                     src={svgs.appleIcon}
                     alt="Apple Logo"
                     width={1200}
                     height={1000}
                     quality={100}
                     className="w-[22px] h-[60px]"
                  />
               </div>

               {/* Right buttons */}
               <div className="flex items-center gap-3">
                  <Image
                     src={svgs.appleSearchIcon}
                     alt="Search"
                     width={20}
                     height={20}
                     quality={100}
                     className="w-[40px] h-[40px] cursor-pointer"
                     onClick={() => setActiveCategory('Search')}
                  />

                  <div
                     className="relative cursor-pointer"
                     onClick={() => setActiveCategory('BagMenu')}
                  >
                     <Image
                        src={svgs.appleBasketIcon}
                        alt="Basket"
                        width={20}
                        height={20}
                        quality={100}
                        className="w-[40px] h-[40px]"
                     />
                     {items.length > 0 && (
                        <span className="absolute -top-1 -right-1 text-[9px] w-4 h-4 rounded-full bg-black text-white font-semibold text-center leading-[16px]">
                           {items.reduce((acc, item) => acc + item.quantity, 0)}
                        </span>
                     )}
                  </div>

                  <button
                     className="p-1 cursor-pointer"
                     onClick={() => setMenuOpen(!menuOpen)}
                  >
                     <Image
                        src={svgs.menuIcon}
                        alt="Menu"
                        width={20}
                        height={20}
                        quality={100}
                        className="w-[20px] h-[20px]"
                     />
                  </button>
               </div>
            </div>

            {/* Mobile dropdown */}
            <AnimatePresence>
               {menuOpen && (
                  <>
                     {/* Overlay */}
                     <motion.div
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 0.5 }}
                        exit={{ opacity: 0 }}
                        transition={{ duration: 0.3 }}
                        className="fixed inset-0 bg-black z-40"
                        onClick={() => setMenuOpen(false)}
                     />
                     {/* Slide menu */}
                     <motion.div
                        initial={{ x: '100%', opacity: 0 }}
                        animate={{ x: '0%', opacity: 1 }}
                        exit={{ x: '100%', opacity: 0 }}
                        transition={{ duration: 0.4, ease: 'easeInOut' }}
                        className="fixed top-0 right-0 w-[70%] h-full bg-[#fafafc] z-50 p-6 shadow-xl flex flex-col"
                     >
                        <div className="flex justify-between items-center mb-6">
                           <h2 className="text-lg font-semibold">Menu</h2>
                           <button
                              className="text-sm text-[#555]"
                              onClick={() => setMenuOpen(false)}
                           >
                              ✕
                           </button>
                        </div>

                        <ul className="flex flex-col gap-3 text-base font-normal">
                           <li
                              className="cursor-pointer"
                              onClick={() => {
                                 router.push('/store');
                                 setMenuOpen(false);
                              }}
                           >
                              Store
                           </li>
                           {mainCategoriesDefault.map((category) => (
                              <li
                                 key={category.category_id}
                                 className="cursor-pointer"
                                 onClick={() => {
                                    router.push('/shop');
                                    setMenuOpen(false);
                                 }}
                              >
                                 {category.category_name}
                              </li>
                           ))}
                           <li
                              className="cursor-pointer"
                              onClick={() => {
                                 router.push('/support');
                                 setMenuOpen(false);
                              }}
                           >
                              Support
                           </li>
                           <li
                              className="cursor-pointer"
                              onClick={() => {
                                 setActiveCategory('UserMenu');
                                 setMenuOpen(false);
                              }}
                           >
                              Account
                           </li>
                        </ul>
                     </motion.div>
                  </>
               )}
            </AnimatePresence>

            <AnimatePresence>
               {activeCategory && subcategoryContent[activeCategory] && (
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
                     onMouseEnter={() => setActiveCategory(activeCategory)}
                  >
                     <div className="py-8">
                        {subcategoryContent[activeCategory]}
                     </div>
                  </motion.div>
               )}
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
