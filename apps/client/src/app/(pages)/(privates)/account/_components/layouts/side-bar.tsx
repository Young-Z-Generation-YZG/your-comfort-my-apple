'use client';

import Link from 'next/link';
import svgs from '@assets/svgs';
import Image from 'next/image';
import { useState, useEffect } from 'react';
import { cn } from '~/infrastructure/lib/utils';
import { usePathname } from 'next/navigation';
import { AnimatePresence, motion } from 'framer-motion';
import { Menu } from 'lucide-react';

const listSidebarItems = [
   {
      label: 'Personal Information',
      link: '/account',
      icon: svgs.appleUserProfileIcon,
   },
   { label: 'Addresses', link: '/account/addresses', icon: svgs.appleHomeIcon },
   {
      label: 'Payment Methods',
      link: '/account/payment-methods',
      icon: svgs.applePaymentIcon,
   },
   { label: 'Orders', link: '/account/orders', icon: svgs.appleOrderIcon },
   { label: 'Vouchers', link: '/account/vouchers', icon: svgs.ticketIcon },
   {
      label: 'Security',
      link: '/account/security',
      icon: svgs.appleSecurityIcon,
   },
   {
      label: 'Preferences',
      link: '/account/preferences',
      icon: svgs.appleSettingsIcon,
   },
];

const SideBar = () => {
   const pathname = usePathname();
   const [active, setActive] = useState(pathname);
   const [isExpanded, setIsExpanded] = useState(true);

   // Co lại nếu màn hình nhỏ hơn 1024
   useEffect(() => {
      const handleResize = () => {
         setIsExpanded(window.innerWidth >= 1024);
      };
      handleResize();
      window.addEventListener('resize', handleResize);
      return () => window.removeEventListener('resize', handleResize);
   }, []);

   return (
      <>
         <motion.aside
            animate={{
               width: isExpanded ? 220 : 60,
            }}
            transition={{ duration: 0.1, ease: 'easeInOut' }}
            className={cn(
               'flex md:hidden bg-white border border-slate-200 rounded-2xl shadow-sm overflow-hidden flex-col transition-all duration-300',
            )}
         >
            <ul className="w-full py-3">
               {/* Nút menu toggle */}
               <li
                  className={cn(
                     'mx-2 my-1 transition-all duration-200 ease-in-out cursor-pointer flex items-center',
                     // isExpanded && 'justify-start',
                  )}
                  onClick={() => setIsExpanded((prev) => !prev)}
               >
                  <div className="flex items-center gap-2 px-3 py-2 text-slate-600">
                     <Menu size={18} />
                     {/* <AnimatePresence mode="wait">
                        {isExpanded && (
                           <motion.span
                              key="toggle"
                              initial={{ opacity: 0, x: -10 }}
                              animate={{ opacity: 1, x: 0 }}
                              exit={{ opacity: 0, x: -10 }}
                              transition={{ duration: 0.5 }}
                              className="text-sm font-SFProText"
                           >
                              Menu
                           </motion.span>
                        )}
                     </AnimatePresence> */}
                  </div>
               </li>
               {listSidebarItems.map((item, index) => (
                  <li
                     key={index}
                     className={cn(
                        'rounded-xl mx-2 my-1 transition-all duration-200 hover:bg-slate-100 ease-in-out',
                     )}
                     onClick={() => setActive(item.link)}
                  >
                     <Link
                        href={item.link}
                        className={cn(
                           'flex items-center gap-2 px-3 py-2 text-slate-500 font-SFProText text-sm opacity-70',
                           active === item.link &&
                              'opacity-100 text-slate-900 bg-slate-100 rounded-xl',
                        )}
                     >
                        <Image src={item.icon} alt="icon" className="w-5 h-5" />
                        <AnimatePresence mode="wait">
                           {isExpanded && <span>{item.label}</span>}
                        </AnimatePresence>
                     </Link>
                  </li>
               ))}
            </ul>
         </motion.aside>
         <ul className="hidden md:flex w-full flex-col">
            {listSidebarItems.map((item, index) => (
               <li
                  key={index}
                  className={cn(
                     'rounded-lg transition-all duration-200 hover:bg-slate-200/50 ease-in-out',
                  )}
                  onClick={() => setActive(item.link)}
               >
                  <Link
                     href={item.link}
                     className={cn(
                        'flex items-center gap-2 px-2 py-3 text-slate-500 font-SFProText text-sm opacity-50',
                        active === item.link && 'opacity-100 text-slate-900',
                     )}
                  >
                     <Image
                        src={item.icon}
                        alt={'icon'}
                        className={cn('w-5 h-5')}
                     />
                     {item.label}
                  </Link>
               </li>
            ))}
         </ul>
      </>
   );
};

export default SideBar;
