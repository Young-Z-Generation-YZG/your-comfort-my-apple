import { Button } from '@components/ui/button';
import { motion } from 'framer-motion';
import { useRouter } from 'next/navigation';
import { useAppSelector } from '~/infrastructure/redux/store';
import HeaderBagItem from './header-bag-item';
import { PiUserCircleFill } from 'react-icons/pi';
import { LiaBoxSolid } from 'react-icons/lia';
import { RiLogoutBoxRLine } from 'react-icons/ri';
import { IoBookmarkOutline } from 'react-icons/io5';
import { MdOutlineManageAccounts } from 'react-icons/md';
import Link from 'next/link';
import { setLogout } from '~/infrastructure/redux/features/auth.slice';
import { useDispatch } from 'react-redux';

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

const BasketMenu = () => {
   const router = useRouter();
   const { items } = useAppSelector((state) => state.cart.value);

   const dispatch = useDispatch();

   const { isAuthenticated, userEmail } = useAppSelector(
      (state) => state.auth.value,
   );

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
               <div className="font-SFProText flex justify-between items-center">
                  <h2 className="text-2xl font-medium font-SFProText">Bag</h2>
                  <Button
                     className="rounded-full bg-sky-500 text-white hover:bg-sky-600 font-normal"
                     variant="secondary"
                     onClick={() => {
                        router.push('/cart');
                     }}
                  >
                     Review Bag
                  </Button>
               </div>

               <div className="py-5 flex gap-5 flex-col">
                  {items.length > 0 &&
                     items
                        .filter((i) => i.order < 4)
                        .map((item) => {
                           return (
                              <HeaderBagItem
                                 key={item.product_id}
                                 item={item}
                              />
                           );
                        })}

                  <p className="text-sm text-slate-500 mt-2">
                     {items.filter((i) => i.order > 3).length} more items in
                     your bag
                  </p>
               </div>

               <h3 className="text-sm text-slate-500 font-SFProText">
                  My Profile
               </h3>
               <ul className="pt-2">
                  <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                     <LiaBoxSolid className="size-4" />
                     <p>Orders</p>
                  </li>
                  <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                     <IoBookmarkOutline className="size-4" />
                     <p>Your Saves</p>
                  </li>
                  <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                     <MdOutlineManageAccounts className="size-4" />
                     <Link href="/account">
                        <p>Account</p>
                     </Link>
                  </li>
                  {!isAuthenticated ? (
                     <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                        <PiUserCircleFill
                           className="size-4"
                           aria-hidden="true"
                        />
                        <Link href="/sign-in">Sign-in</Link>
                     </li>
                  ) : (
                     <li
                        className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600"
                        onClick={() => {
                           dispatch(setLogout());
                        }}
                     >
                        <RiLogoutBoxRLine
                           className="size-4"
                           aria-hidden="true"
                        />
                        <p>Logout</p>
                     </li>
                  )}
               </ul>
            </motion.div>
         </div>
      </motion.div>
   );
};

export default BasketMenu;
