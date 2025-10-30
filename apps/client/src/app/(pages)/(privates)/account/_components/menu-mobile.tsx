'use client';

import { useState } from 'react';
import { AnimatePresence, motion } from 'framer-motion';
import { X } from 'lucide-react';
import { useRouter } from 'next/navigation';
import Image from 'next/image';
import svgs from '@assets/svgs';

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

const MobileFloatingMenu = () => {
   const [open, setOpen] = useState(false);
   const router = useRouter();

   return (
      <>
         <button
            onClick={() => setOpen(true)}
            className="fixed bottom-5 right-5 z-40 bg-white border border-gray-300 rounded-full w-14 h-14 flex items-center justify-center shadow-md hover:shadow-lg hover:scale-105 transition-transform"
         >
            <Image
               src={svgs.menuIcon}
               alt="Menu"
               width={24}
               height={24}
               quality={100}
               className="w-6 h-6 opacity-80"
            />
         </button>
         <AnimatePresence>
            {open && (
               <>
                  {/* Overlay */}
                  <motion.div
                     initial={{ opacity: 0 }}
                     animate={{ opacity: 0.4 }}
                     exit={{ opacity: 0 }}
                     transition={{ duration: 0.25 }}
                     className="fixed inset-0 bg-black z-40"
                     onClick={() => setOpen(false)}
                  />

                  {/* Bottom sheet */}
                  <motion.div
                     initial={{ y: '100%' }}
                     animate={{ y: 0 }}
                     exit={{ y: '100%' }}
                     transition={{
                        type: 'spring',
                        stiffness: 200,
                        damping: 25,
                     }}
                     className="fixed bottom-0 left-0 right-0 z-50 bg-[#fafafc] rounded-t-2xl shadow-[0_-4px_20px_rgba(0,0,0,0.15)] p-6 pb-10"
                  >
                     <div className="flex justify-between items-center mb-6">
                        <h3 className="text-xl font-semibold">Account Menu</h3>
                        <button onClick={() => setOpen(false)}>
                           <X className="w-6 h-6 text-gray-600" />
                        </button>
                     </div>

                     {/* Menu list */}
                     <ul className="flex flex-col text-base font-medium divide-y divide-gray-200">
                        {listSidebarItems.map((item, index) => (
                           <li
                              key={index}
                              className="flex items-center gap-3 py-3 cursor-pointer hover:bg-gray-100 transition-colors"
                              onClick={() => {
                                 router.push(item.link);
                                 setOpen(false);
                              }}
                           >
                              <Image
                                 src={item.icon}
                                 alt={item.label}
                                 width={20}
                                 height={20}
                                 quality={100}
                                 className="w-5 h-5 opacity-80"
                              />
                              <span>{item.label}</span>
                           </li>
                        ))}
                     </ul>
                  </motion.div>
               </>
            )}
         </AnimatePresence>
      </>
   );
};

export default MobileFloatingMenu;
