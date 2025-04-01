'use client';

import Link from 'next/link';
import svgs from '@assets/svgs';
import Image from 'next/image';
import { useState } from 'react';
import { cn } from '~/infrastructure/lib/utils';

const SideBar = () => {
   const [active, setActive] = useState('account');

   return (
      <ul className="w-full">
         <li
            className={cn('px-2 py-3 rounded-lg transition-all duration-100')}
            onClick={() => setActive('account')}
         >
            <Link
               href="/account"
               className={cn(
                  'flex items-center gap-2 text-slate-500 font-SFProText text-sm opacity-50',
                  active === 'account' && 'opacity-100 text-slate-900',
               )}
            >
               <Image
                  src={svgs.appleUserProfileIcon}
                  alt="apple-user-profile-icon"
                  className={cn('w-5 h-5')}
               />
               Personal Information
            </Link>
         </li>
         <li
            className="px-2 py-3 rounded-lg transition-all duration-100"
            onClick={() => setActive('addresses')}
         >
            <Link
               href="/account/addresses"
               className={cn(
                  'flex items-center gap-2 text-slate-500 font-SFProText text-sm opacity-50',
                  active === 'addresses' && 'opacity-100 text-slate-900',
               )}
            >
               <Image
                  src={svgs.appleHomeIcon}
                  alt="apple-user-profile-icon"
                  className="w-5 h-5 opacity-50"
               />
               Addresses
            </Link>
         </li>

         <li
            className="px-2 py-3 rounded-lg transition-all duration-100"
            onClick={() => setActive('payment-methods')}
         >
            <Link
               href="/account/payment-methods"
               className={cn(
                  'flex items-center gap-2 text-slate-500 font-SFProText text-sm opacity-50',
                  active === 'payment-methods' && 'opacity-100 text-slate-900',
               )}
            >
               <Image
                  src={svgs.applePaymentIcon}
                  alt="apple-user-profile-icon"
                  className="w-5 h-5 opacity-50"
               />
               Payment Methods
            </Link>
         </li>

         <li
            className="px-2 py-3 rounded-lg transition-all duration-100"
            onClick={() => setActive('orders')}
         >
            <Link
               href="/account/orders"
               className={cn(
                  'flex items-center gap-2 text-slate-500 font-SFProText text-sm opacity-50',
                  active === 'orders' && 'opacity-100 text-slate-900',
               )}
            >
               <Image
                  src={svgs.appleOrderIcon}
                  alt="apple-user-profile-icon"
                  className="w-5 h-5 opacity-50"
               />
               Orders
            </Link>
         </li>

         <li
            className="px-2 py-3 rounded-lg transition-all duration-100"
            onClick={() => setActive('security')}
         >
            <Link
               href="/account/security"
               className={cn(
                  'flex items-center gap-2 text-slate-500 font-SFProText text-sm opacity-50',
                  active === 'security' && 'opacity-100 text-slate-900',
               )}
            >
               <Image
                  src={svgs.appleSecurityIcon}
                  alt="apple-user-profile-icon"
                  className="w-5 h-5 opacity-50"
               />
               Security
            </Link>
         </li>

         <li
            className="px-2 py-3 rounded-lg transition-all duration-100"
            onClick={() => setActive('preferences')}
         >
            <Link
               href="/account/preferences"
               className={cn(
                  'flex items-center gap-2 text-slate-500 font-SFProText text-sm opacity-50',
                  active === 'preferences' && 'opacity-100 text-slate-900',
               )}
            >
               <Image
                  src={svgs.appleSettingsIcon}
                  alt="apple-user-profile-icon"
                  className="w-5 h-5 opacity-50"
               />
               Preferences
            </Link>
         </li>
      </ul>
   );
};

export default SideBar;
