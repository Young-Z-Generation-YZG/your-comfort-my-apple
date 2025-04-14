'use client';

import Link from 'next/link';
import svgs from '@assets/svgs';
import Image from 'next/image';
import { useState } from 'react';
import { cn } from '~/infrastructure/lib/utils';

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
   const [active, setActive] = useState('/account');

   return (
      <ul className="w-full">
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
   );
};

export default SideBar;
