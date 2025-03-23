'use client';

import Image from 'next/image';
import svgs from '@assets/svg';
import { useEffect, useRef, useState } from 'react';
import { AnimatePresence, motion } from 'framer-motion';
import { useGetCategoriesAsyncQuery } from '~/infrastructure/services/category.service';
import { CategoryResponseType } from '~/domain/types/category.type';

const mainCategoriesDefault = ['Mac', 'iPad', 'iPhone', 'Watch', 'HeadPhones'];

const subCategories = ['iPhone 16 Pro', 'iPhone 16', 'iPhone 16e', 'iPhone 15'];

const Header = () => {
   const [activeCategory, setActiveCategory] = useState<string | null>(null);
   const [categories, setCategories] = useState<CategoryResponseType[]>([]);

   const categoryRefs = useRef<{ [key: string]: HTMLLIElement | null }>({});

   console.log('categoryRefs', categoryRefs);

   const {
      data: categoriesData,
      error: categoriesError,
      isLoading: categoriesLoading,
   } = useGetCategoriesAsyncQuery();

   useEffect(() => {
      if (categoriesData) {
         setCategories(categoriesData);
      }
   }, [categoriesData]);

   const renderHeaderCategories = () => {
      if (!categories.length) {
         return mainCategoriesDefault.map((category, index) => {
            return (
               <li
                  key={index}
                  className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  ref={(el) => {
                     categoryRefs.current[category] = el;
                  }}
                  data-category_id={category}
                  onMouseEnter={() => handleMouseEnter(category)}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     {category}
                  </p>
               </li>
            );
         });
      }

      var sortedCategories = categories
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
               onMouseEnter={() => handleMouseEnter(category.category_name)}
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

   // Column animation variants
   const columnVariants = {
      hidden: {
         opacity: 0,
         y: 20,
      },
      visible: (custom: number) => ({
         opacity: 1,
         y: 0,
         transition: {
            duration: 0.4,
            delay: custom * 0.1,
         },
      }),
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
         className="relative w-screen bg-[#fafafc]"
         onMouseLeave={handleMouseLeave}
      >
         <div className="flex flex-row items-center w-[1180px] h-[44px] px-[22px] mx-auto">
            <ul className="main-category flex flex-row justify-between items-center w-full">
               <div className="px-[8px]">
                  <Image
                     src={svgs.appleIcon}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className="w-[22px] h-[44px]"
                  />
               </div>
               <li className="category-item cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]">
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
               <div className="px-[8px]">
                  <Image
                     src={svgs.appleSearchIcon}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className="w-[22px] h-[44px]"
                  />
               </div>
               <div className="px-[8px]">
                  <Image
                     src={svgs.appleBasketIcon}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className="w-[22px] h-[44px]"
                  />
               </div>
            </ul>

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
            </AnimatePresence>

            {/* Blur overlay for the rest of the page */}
            <AnimatePresence>
               {activeCategory && subcategoryContent[activeCategory] && (
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
                     style={{ top: '44px' }} // Position it just below the header
                  />
               )}
            </AnimatePresence>
         </div>
      </header>
   );
};

export default Header;

{
   /* <div className="sub-category absolute top-[44px] left-0 h-auto px-[22px] pt-[40px] pb-[84px] bg-[#fafafc] w-screen"></div>; */
}
