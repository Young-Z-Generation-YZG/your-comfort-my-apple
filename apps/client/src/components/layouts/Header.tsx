'use client';

import Image from 'next/image';
import svgs from '@assets/svgs';
import {
   Dispatch,
   SetStateAction,
   useCallback,
   useEffect,
   useMemo,
   useState,
} from 'react';
import { PiUserCircleFill } from 'react-icons/pi';
import { AnimatePresence, motion } from 'framer-motion';
import { useRouter } from 'next/navigation';
import { Bell, RefreshCw, Menu, X } from 'lucide-react';
import Search from './_components/search';
import UserMenu from './_components/user-menu';
import BasketMenu from './_components/basket-menu';
import { exploreIphoneList } from './_data/explore-iphone-list';
import useIdentityService from '@components/hooks/api/use-identity-service';
import useNotificationService from '@components/hooks/api/use-notification-service';
import { useAppSelector } from '~/infrastructure/redux/store';
import { TNotification } from '~/domain/types/ordering.type';
import { INotificationQueryParams } from '~/infrastructure/services/notification.service';
import { Badge } from '@components/ui/badge';

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

const formatRelativeTime = (dateString?: string) => {
   if (!dateString) return '';
   const date = new Date(dateString);
   if (Number.isNaN(date.getTime())) return '';

   const diffInSeconds = Math.floor((Date.now() - date.getTime()) / 1000);
   if (diffInSeconds < 60) return 'Just now';
   if (diffInSeconds < 3600) return `${Math.floor(diffInSeconds / 60)}m ago`;
   if (diffInSeconds < 86400) return `${Math.floor(diffInSeconds / 3600)}h ago`;
   if (diffInSeconds < 604800)
      return `${Math.floor(diffInSeconds / 86400)}d ago`;

   return date.toLocaleDateString();
};

const formatMetaLabel = (value?: string | null) =>
   value ? value.replace(/_/g, ' ') : '';

const getStatusBadgeClass = (status?: string | null) => {
   if (!status) return 'bg-neutral-100 text-neutral-700';
   switch (status) {
      case 'PENDING':
         return 'bg-amber-100 text-amber-800';
      case 'COMPLETED':
      case 'SENT':
         return 'bg-emerald-100 text-emerald-800';
      case 'FAILED':
      case 'CANCELLED':
         return 'bg-rose-100 text-rose-800';
      default:
         return 'bg-neutral-100 text-neutral-700';
   }
};

type NotificationTab = 'all' | 'unread' | 'read';

const Header = () => {
   const [activeCategory, setActiveCategory] = useState<string | null>(null);
   const [notificationTab, setNotificationTab] =
      useState<NotificationTab>('all');
   const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
   const router = useRouter();
   const { isAuthenticated } = useAppSelector((state) => state.auth);

   const { getMeAsync, isLoading: isIdentityLoading } = useIdentityService();
   const {
      getNotificationsAsync,
      markAsReadAsync,
      markAllAsReadAsync,
      isLoading: isNotificationLoading,
   } = useNotificationService();

   const [notificationData, setNotificationData] = useState<{
      all: TNotification[];
      unread: TNotification[];
      read: TNotification[];
   }>({
      all: [],
      unread: [],
      read: [],
   });
   const [notificationCounts, setNotificationCounts] = useState<{
      all: number;
      unread: number;
      read: number;
   }>({
      all: 0,
      unread: 0,
      read: 0,
   });
   const isLoading = isNotificationLoading || isIdentityLoading;

   const buildNotificationQueryParams = useCallback(
      (tab: NotificationTab): INotificationQueryParams => {
         const baseParams: INotificationQueryParams = {
            _limit: 5,
            _page: 1,
         };

         if (tab === 'unread') {
            baseParams._isRead = false;
         } else if (tab === 'read') {
            baseParams._isRead = true;
         }
         // For 'all' tab, don't include _isRead parameter

         return baseParams;
      },
      [],
   );

   const fetchNotificationsByTab = useCallback(
      async (tab: NotificationTab) => {
         if (!isAuthenticated) {
            setNotificationData((prev) => ({ ...prev, [tab]: [] }));
            setNotificationCounts((prev) => ({ ...prev, [tab]: 0 }));
            return;
         }

         const queryParams = buildNotificationQueryParams(tab);
         const response = await getNotificationsAsync(queryParams);

         if (response?.isSuccess && response.data) {
            const items = response.data.items ?? [];
            const count = response.data.total_records ?? items.length ?? 0;
            console.log(`[Notifications] Fetched ${tab} tab:`, {
               tab,
               count,
               itemsCount: items.length,
               items,
            });
            setNotificationCounts((prev) => ({
               ...prev,
               [tab]: count,
            }));
            setNotificationData((prev) => ({
               ...prev,
               [tab]: items,
            }));
         } else {
            console.warn(
               `[Notifications] Failed to fetch ${tab} tab:`,
               response,
            );
         }
      },
      [buildNotificationQueryParams, getNotificationsAsync, isAuthenticated],
   );

   const fetchAllNotificationTabs = useCallback(async () => {
      if (!isAuthenticated) {
         setNotificationData({ all: [], unread: [], read: [] });
         setNotificationCounts({ all: 0, unread: 0, read: 0 });
         return;
      }

      const tabs: NotificationTab[] = ['all', 'unread', 'read'];
      await Promise.all(tabs.map((tab) => fetchNotificationsByTab(tab)));
   }, [fetchNotificationsByTab, isAuthenticated]);

   const notifications = useMemo(() => {
      const data = notificationData[notificationTab] ?? [];
      console.log(
         `[Notifications] Current tab "${notificationTab}" has ${data.length} items:`,
         data,
      );
      return data;
   }, [notificationData, notificationTab]);

   const totalNotifications = notificationCounts.all;
   const unreadCount = notificationCounts.unread;
   const readCount = notificationCounts.read;

   const handleMarkNotification = useCallback(
      async (notification: TNotification) => {
         await markAsReadAsync(notification.id);
         await fetchAllNotificationTabs();
         return notification;
      },
      [markAsReadAsync, fetchAllNotificationTabs],
   );

   const handleMarkAllNotifications = useCallback(async () => {
      await markAllAsReadAsync();
      await fetchAllNotificationTabs();
   }, [markAllAsReadAsync, fetchAllNotificationTabs]);
   const handleNotificationToggle = useCallback(() => {
      if (!isAuthenticated) return;
      const nextCategory =
         activeCategory === 'Notifications' ? null : 'Notifications';
      setActiveCategory(nextCategory);
      if (nextCategory === null) {
         setNotificationTab('all');
      }
      if (nextCategory === 'Notifications') {
         setNotificationTab('all');
         // Fetch all tabs when panel opens
         void fetchAllNotificationTabs();
      }
   }, [activeCategory, fetchAllNotificationTabs, isAuthenticated]);

   const handleNotificationPanelClose = useCallback(() => {
      setActiveCategory(null);
      setNotificationTab('all');
   }, []);

   useEffect(() => {
      const fetchMe = async () => {
         const result = await getMeAsync();
         if (result.isSuccess) {
            console.log(result.data);
         } else {
            console.log(result.error);
         }
      };

      fetchMe();
   }, [getMeAsync]);

   useEffect(() => {
      fetchAllNotificationTabs();
   }, [fetchAllNotificationTabs]);

   useEffect(() => {
      if (!isAuthenticated) {
         setActiveCategory((prev) => (prev === 'Notifications' ? null : prev));
         setNotificationTab('all');
      }
   }, [isAuthenticated]);

   return (
      <header
         className="relative w-full bg-[#fafafc]"
         onMouseLeave={() => {
            setActiveCategory(null);
         }}
      >
         <div className="flex flex-row items-center max-w-7xl h-12 md:h-[44px] px-4 sm:px-6 lg:px-8 mx-auto">
            {/* Mobile Header Layout */}
            <div className="flex lg:hidden w-full items-center justify-between">
               {/* Logo */}
               <div
                  className="cursor-pointer"
                  onClick={() => {
                     router.push('/');
                  }}
               >
                  <Image
                     src={svgs.appleIcon}
                     alt="Apple"
                     width={20}
                     height={44}
                     className="w-[18px] h-[44px] object-contain"
                  />
               </div>

               {/* Right Actions */}
               <div className="flex items-center gap-6">
                  <div
                     className="cursor-pointer"
                     onClick={() => setActiveCategory('Search')}
                  >
                     <Image
                        src={svgs.appleSearchIcon}
                        alt="Search"
                        width={32}
                        height={32}
                        className="w-14 h-14"
                     />
                  </div>
                  <div
                     className="cursor-pointer relative"
                     onClick={() => setActiveCategory('BagMenu')}
                  >
                     <Image
                        src={svgs.appleBasketIcon}
                        alt="Bag"
                        width={32}
                        height={32}
                        className="w-14 h-14"
                     />
                  </div>
                  <div
                     className="cursor-pointer"
                     onClick={() => setActiveCategory('UserMenu')}
                  >
                     <PiUserCircleFill className="w-12 h-6 text-[#1d1d1f]" />
                  </div>
                  <button
                     onClick={() => {
                        setIsMobileMenuOpen(true)

                        if(!isMobileMenuOpen) {
                           setActiveCategory(null);
                        }
                     }}
                     className="text-[#1d1d1f] focus:outline-none"
                     aria-label="Open menu"
                  >
                     <Menu className="w-5 h-5" />
                  </button>
               </div>
            </div>

            {/* Desktop navigation */}
            <ul className="hidden lg:flex main-category flex-row justify-between items-center w-full">
               <div
                  className="px-[8px] cursor-pointer"
                  onClick={() => {
                     router.push('/');
                  }}
               >
                  <Image
                     src={svgs.appleIcon}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className="w-5 h-10 md:w-[22px] md:h-[44px]"
                  />
               </div>

               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px] hidden md:block"
                  onClick={() => {
                     router.push('/sales/event/black-friday');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Event');
                  }}
               >
                  <Badge className="bg-gradient-to-r from-orange-500 to-red-600 text-white hover:from-orange-600 hover:to-red-700 shadow-lg border-0">
                     Black Friday 12.12
                  </Badge>
               </li>

               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('/shop');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Shop');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Store
                  </p>
               </li>

               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px] hidden xl:block"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Mac');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">Mac</p>
               </li>
               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('/store');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('iPad');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     iPad
                  </p>
               </li>
               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('iPhone');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     iPhone
                  </p>
               </li>
               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px] hidden lg:block"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Watch');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">Watch</p>
               </li>
               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('/store');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Store');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Store
                  </p>
               </li>
               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('HeadPhones');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     HeadPhones
                  </p>
               </li>
               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px] hidden xl:block"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Vision');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">Vision</p>
               </li>
               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('TV & Home');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     TV & Home
                  </p>
               </li>
               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Entertainment');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Entertainment
                  </p>
               </li>

               <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Accessories');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Accessories
                  </p>
               </li>

               {/* <li
                  className="cursor-pointer h-[44px] leading-[44px] px-[8px] font-normal text-[14px]"
                  onClick={() => {
                     router.push('#');
                  }}
                  onMouseEnter={() => {
                     setActiveCategory('Support');
                  }}
               >
                  <p className="antialiased opacity-[0.8] tracking-wide">
                     Support
                  </p>
               </li> */}

               <div
                  className="px-[8px] cursor-pointer hidden md:block"
                  onClick={() => {
                     setActiveCategory('Search');
                  }}
               >
                  <Image
                     src={svgs.appleSearchIcon}
                     alt="cover"
                     width={10}
                     height={10}
                     quality={100}
                     className="w-4 h-8 md:w-[22px] md:h-[44px]"
                  />
               </div>
               <div
                  className="px-[8px] cursor-pointer relative hidden md:block"
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
                     className="w-4 h-8 md:w-[22px] md:h-[44px]"
                  />
               </div>

               {isAuthenticated && (
                  <div
                     className="px-[8px] cursor-pointer relative hidden md:block"
                     onClick={handleNotificationToggle}
                  >
                     <Bell className="w-4 h-4 md:w-[18px] md:h-[18px] text-[#1d1d1f]" />
                     {unreadCount > 0 && (
                        <span className="absolute -top-0.5 right-1 min-w-[16px] rounded-full bg-[#0071e3] px-[4px] text-[10px] font-semibold leading-[16px] text-white text-center">
                           {unreadCount > 9 ? '9+' : unreadCount}
                        </span>
                     )}
                  </div>
               )}

               <div
                  className="px-[8px] cursor-pointer hidden md:block"
                  onClick={() => {
                     setActiveCategory('UserMenu');
                     if(isMobileMenuOpen) {
                           setActiveCategory(null);
                     }
                  }}
               >
                  <PiUserCircleFill className="w-4 h-4 md:size-5" />
               </div>
            </ul>

            {/* Mobile menu overlay */}
            <AnimatePresence>
               {isMobileMenuOpen && (
                  <motion.div
                     key="backdrop"
                     className="lg:hidden fixed inset-0 bg-black bg-opacity-50 z-40"
                     initial={{ opacity: 0 }}
                     animate={{ opacity: 1 }}
                     exit={{ opacity: 0 }}
                     onClick={() => setIsMobileMenuOpen(false)}
                  />
               )}
            </AnimatePresence>

            <AnimatePresence>
               {isMobileMenuOpen && (
                  <motion.div
                     key="drawer"
                     className="lg:hidden fixed inset-y-0 left-0 w-80 bg-[#fafafc] z-50 shadow-xl"
                     initial={{ x: -320 }}
                     animate={{ x: -5 }}
                     exit={{ x: -320 }}
                     transition={{ type: 'spring', stiffness: 300, damping: 50 }}
                  >
                     <div className="flex flex-col h-full">
                        {/* Mobile menu header */}
                        <div className="flex items-center justify-between px-4 h-12 md:h-[44px] border-b border-gray-200">
                           <div className="flex items-center">
                              <button
                                 onClick={() => setIsMobileMenuOpen(false)}
                                 className="p-2 rounded-md text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 aria-label="Close menu"
                              >
                                 <X className="h-5 w-5 md:h-6 md:w-6" />
                              </button>
                           </div>
                           <div className="flex-1 flex justify-center">
                              <div
                                 className="px-[8px] cursor-pointer"
                                 onClick={() => {
                                    router.push('/');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 <Image
                                    src={svgs.appleIcon}
                                    alt="cover"
                                    width={1200}
                                    height={1000}
                                    quality={100}
                                    className="w-5 h-10 md:w-[22px] md:h-[44px]"
                                 />
                              </div>
                           </div>
                           <div className="w-10"></div>
                        </div>

                        {/* Mobile menu items */}
                        <div className="flex-1 overflow-y-auto px-4 py-4">
                           <ul className="divide-y divide-[#e5e5ea]">
                              <li
                                 className="cursor-pointer py-3 px-4 font-medium text-[16px] text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 onClick={() => {
                                    router.push('/sales/event/black-friday');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 <Badge className="bg-gradient-to-r from-orange-500 to-red-600 text-white hover:from-orange-600 hover:to-red-700 shadow-lg border-0">
                                    Black Friday 12.12
                                 </Badge>
                              </li>
                              <li
                                 className="cursor-pointer py-3 px-4 font-medium text-[16px] text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 onClick={() => {
                                    router.push('/shop');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 Store
                              </li>
                              <li
                                 className="cursor-pointer py-3 px-4 font-medium text-[16px] text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 onClick={() => {
                                    router.push('/store');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 Mac
                              </li>
                              <li
                                 className="cursor-pointer py-3 px-4 font-medium text-[16px] text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 onClick={() => {
                                    router.push('/store');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 iPad
                              </li>
                              <li
                                 className="cursor-pointer py-3 px-4 font-medium text-[16px] text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 onClick={() => {
                                    router.push('#');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 iPhone
                              </li>
                              <li
                                 className="cursor-pointer py-3 px-4 font-medium text-[16px] text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 onClick={() => {
                                    router.push('#');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 Watch
                              </li>
                              <li
                                 className="cursor-pointer py-3 px-4 font-medium text-[16px] text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 onClick={() => {
                                    router.push('#');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 Vision
                              </li>
                              <li
                                 className="cursor-pointer py-3 px-4 font-medium text-[16px] text-[#1d1d1f] hover:bg-[#f5f5f7] transition-colors"
                                 onClick={() => {
                                    router.push('#');
                                    setIsMobileMenuOpen(false);
                                 }}
                              >
                                 Accessories
                              </li>
                           </ul>
                        </div>
                     </div>
                  </motion.div>
               )}
            </AnimatePresence>

            <AnimatePresence>
               {activeCategory &&
                  activeCategory !== 'Search' &&
                  activeCategory !== 'BagMenu' &&
                  activeCategory !== 'UserMenu' &&
                  activeCategory !== 'Notifications' && (
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

               {isAuthenticated && activeCategory === 'Notifications' && (
                  <NotificationPanel
                     notifications={notifications}
                     unreadCount={unreadCount}
                     readCount={readCount}
                     totalCount={totalNotifications}
                     activeTab={notificationTab}
                     onTabChange={setNotificationTab}
                     isLoading={isLoading}
                     isActionLoading={isNotificationLoading}
                     onMarkAllRead={handleMarkAllNotifications}
                     onMarkRead={handleMarkNotification}
                     onRefresh={fetchAllNotificationTabs}
                     onClose={handleNotificationPanelClose}
                  />
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

interface NotificationPanelProps {
   notifications: TNotification[];
   unreadCount: number;
   readCount: number;
   totalCount: number;
   activeTab: NotificationTab;
   onTabChange: Dispatch<SetStateAction<NotificationTab>>;
   isLoading: boolean;
   isActionLoading: boolean;
   onMarkAllRead(): void;
   onMarkRead(notification: TNotification): Promise<typeof notification>;
   onRefresh(): void;
   onClose(): void;
}

const NotificationPanel = ({
   notifications,
   unreadCount,
   readCount,
   totalCount,
   activeTab,
   onTabChange,
   isLoading,
   isActionLoading,
   onMarkAllRead,
   onMarkRead,
   onRefresh,
   onClose,
}: NotificationPanelProps) => {
   const hasNotifications = notifications.length > 0;

   const tabOptions: Array<{
      key: NotificationTab;
      label: string;
      count: number;
   }> = [
      { key: 'all', label: 'ALL', count: totalCount },
      { key: 'unread', label: 'unread', count: unreadCount },
      { key: 'read', label: 'readed', count: readCount },
   ];

   return (
      <motion.div
         initial={{ opacity: 0, y: -8 }}
         animate={{ opacity: 1, y: 0 }}
         exit={{ opacity: 0, y: -8 }}
         className="absolute top-[44px] right-[32px] w-[360px] rounded-2xl border border-black/10 bg-white shadow-2xl shadow-black/10 z-[999]"
         onMouseLeave={onClose}
      >
         <div className="flex items-center justify-between gap-3 px-4 pt-4">
            <div>
               <p className="text-sm font-semibold text-[#1d1d1f]">
                  Notifications
               </p>
               <p className="text-xs text-[#6e6e73]">
                  {unreadCount > 0
                     ? `${unreadCount} new update${unreadCount > 1 ? 's' : ''}`
                     : "You're all caught up"}
               </p>
            </div>
            <div className="flex items-center gap-2">
               <button
                  className="p-1.5 rounded-full hover:bg-[#f5f5f7] transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                  onClick={onRefresh}
                  disabled={isLoading || isActionLoading}
                  title="Refresh notifications"
               >
                  <RefreshCw
                     className={`h-4 w-4 text-[#0071e3] ${
                        isLoading ? 'animate-spin' : ''
                     }`}
                  />
               </button>
               <button
                  className="text-[11px] font-semibold uppercase tracking-wide text-[#0071e3] disabled:text-[#9f9f9f]"
                  onClick={onMarkAllRead}
                  disabled={!unreadCount || isActionLoading}
               >
                  Mark all read
               </button>
            </div>
         </div>
         <div className="px-4 pt-3">
            <div className="flex gap-1 rounded-full bg-[#f5f5f7] p-1">
               {tabOptions.map((tab) => {
                  const isActive = activeTab === tab.key;
                  return (
                     <button
                        key={tab.key}
                        className={`flex-1 rounded-full px-2 py-1 text-[11px] font-semibold transition ${
                           isActive
                              ? 'bg-white text-[#1d1d1f] shadow-sm'
                              : 'text-[#6e6e73]'
                        }`}
                        onClick={() => onTabChange(tab.key)}
                     >
                        <span>{tab.label}</span>
                        <span className="ml-1 rounded-full bg-[#e5e5ea] px-1.5 text-[10px]">
                           {tab.count}
                        </span>
                     </button>
                  );
               })}
            </div>
         </div>
         <div className="mt-3 max-h-[320px] overflow-y-auto px-4 pb-4 space-y-2">
            {isLoading ? (
               <div className="flex items-center justify-center py-8 text-sm text-[#6e6e73]">
                  <span className="mr-2 h-4 w-4 animate-spin rounded-full border border-[#d2d2d7] border-t-[#1d1d1f]" />
                  Loading notifications...
               </div>
            ) : !hasNotifications ? (
               <div className="rounded-xl border border-dashed border-[#d2d2d7] bg-[#f5f5f7] px-4 py-6 text-center">
                  <p className="text-sm font-semibold text-[#1d1d1f]">
                     Nothing new right now
                  </p>
                  <p className="text-xs text-[#6e6e73]">
                     We&apos;ll let you know when there&apos;s something to see.
                  </p>
               </div>
            ) : (
               notifications.map((notification) => {
                  const isUnread = !notification.is_read;
                  const hasLink =
                     typeof notification.link === 'string' &&
                     notification.link.trim().length > 0;

                  const handleNotificationClick = async () => {
                     if (isUnread) {
                        await onMarkRead(notification);
                     }
                     if (hasLink) {
                        window.location.href = notification.link;
                     }
                  };

                  return (
                     <div
                        key={notification.id}
                        onClick={handleNotificationClick}
                        className={`rounded-2xl border px-3.5 py-3 transition hover:border-[#0071e3] cursor-pointer ${
                           isUnread
                              ? 'border-[#d2e3ff] bg-[#f5f8ff]'
                              : 'border-[#e5e5ea] bg-white'
                        }`}
                     >
                        <div className="flex items-start justify-between gap-3">
                           <div className="space-y-1">
                              <p className="text-sm font-semibold text-[#1d1d1f] line-clamp-1">
                                 {notification.title}
                              </p>
                              {notification.content && (
                                 <p className="text-xs text-[#6e6e73] leading-relaxed line-clamp-2">
                                    {notification.content}
                                 </p>
                              )}
                           </div>
                           {isUnread && (
                              <span className="mt-1 h-2 w-2 rounded-full bg-[#0071e3]" />
                           )}
                        </div>
                        <div className="mt-2 flex flex-wrap items-center gap-2 text-[11px] text-[#6e6e73]">
                           <span>
                              {formatRelativeTime(notification.created_at)}
                           </span>
                           {notification.type && (
                              <span className="rounded-full bg-[#f0f0f5] px-2 py-0.5 uppercase tracking-wide text-[10px] font-semibold text-[#1d1d1f]">
                                 {formatMetaLabel(notification.type)}
                              </span>
                           )}
                           {notification.status && (
                              <span
                                 className={`rounded-full px-2 py-0.5 uppercase tracking-wide text-[10px] font-semibold ${getStatusBadgeClass(
                                    notification.status,
                                 )}`}
                              >
                                 {formatMetaLabel(notification.status)}
                              </span>
                           )}
                        </div>
                     </div>
                  );
               })
            )}
         </div>
      </motion.div>
   );
};

export default Header;
