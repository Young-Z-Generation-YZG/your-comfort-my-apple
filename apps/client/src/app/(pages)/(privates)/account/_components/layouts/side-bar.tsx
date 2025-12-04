'use client';

import Link from 'next/link';
import svgs from '@assets/svgs';
import Image from 'next/image';
import { useState } from 'react';
import { cn } from '~/infrastructure/lib/utils';
import { usePathname } from 'next/navigation';
import { MdMenu } from 'react-icons/md';

const listSidebarItems = [
   {
      label: 'Personal Information',
      link: '/account',
      icon: svgs.appleUserProfileIcon,
   },
   {
      label: 'Addresses',
      link: '/account/addresses',
      icon: svgs.appleHomeIcon,
   },
   {
      label: 'Payment Methods',
      link: '/account/payment-methods',
      icon: svgs.applePaymentIcon,
   },
   {
      label: 'Orders',
      link: '/account/orders',
      icon: svgs.appleOrderIcon,
   },
   {
      label: 'Vouchers',
      link: '/account/vouchers',
      icon: svgs.ticketIcon,
   },
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
   const [isOpen, setIsOpen] = useState(false);

   return (
      <>
         <ul className="w-full hidden md:block">
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

         <div className="md:hidden">
            <div
               className={cn(
                  'fixed inset-0 z-40 bg-black/25 transition-opacity duration-300',
                  isOpen
                     ? 'opacity-100 pointer-events-auto'
                     : 'opacity-0 pointer-events-none',
               )}
               onClick={() => setIsOpen(false)}
            />
            <div className="fixed left-4 right-4 bottom-24 z-50">
               <div
                  className={cn(
                     'rounded-xl bg-white border shadow-lg p-2 max-h-[60vh] overflow-y-auto transition-all duration-300 transform',
                     isOpen
                        ? 'opacity-100 translate-y-0 scale-100 pointer-events-auto'
                        : 'opacity-0 translate-y-4 scale-95 pointer-events-none',
                  )}
               >
                  <ul className="w-full">
                     {listSidebarItems.map((item, index) => (
                        <li
                           key={index}
                           className={cn(
                              'rounded-lg transition-all duration-200 hover:bg-slate-200/50 ease-in-out',
                           )}
                           onClick={() => {
                              setActive(item.link);
                              setIsOpen(false);
                           }}
                        >
                           <Link
                              href={item.link}
                              className={cn(
                                 'flex items-center gap-2 px-2 py-3 text-slate-500 font-SFProText text-sm opacity-50',
                                 active === item.link &&
                                    'opacity-100 text-slate-900',
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
               </div>
            </div>

            <button
               type="button"
               aria-label="Open menu"
               className="fixed bottom-6 right-6 z-50 w-14 h-14 rounded-full bg-white border shadow-lg flex items-center justify-center active:scale-[0.98] transition-transform"
               onClick={() => setIsOpen((v) => !v)}
            >
               <MdMenu className="w-7 h-7 text-slate-700" />
            </button>
         </div>
      </>
   );
};

export default SideBar;
