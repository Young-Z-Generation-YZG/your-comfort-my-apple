import { motion } from 'framer-motion';
import Link from 'next/link';
import { LiaBoxSolid } from 'react-icons/lia';
import { IoBookmarkOutline } from 'react-icons/io5';
import { MdOutlineManageAccounts } from 'react-icons/md';
import { PiUserCircleFill } from 'react-icons/pi';
import { RiLogoutBoxRLine } from 'react-icons/ri';
import { useDispatch } from 'react-redux';
import { useAppSelector } from '~/infrastructure/redux/store';
import { setLogout } from '~/infrastructure/redux/features/auth.slice';

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

const UserMenu = () => {
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
               {!isAuthenticated ? (
                  <div className="font-SFProText">
                     <h2 className="text-2xl font-medium font-SFProText">
                        You are not sign-in yet.
                     </h2>
                     <p className="text-base text-slate-500 py-5 font-SFProText">
                        <Link
                           href="/sign-in"
                           className="text-blue-400 underline"
                        >
                           Sign-in
                        </Link>{' '}
                        to see your profile
                     </p>
                  </div>
               ) : null}

               <h3 className="text-sm text-slate-500 font-SFProText">
                  My Profile
               </h3>
               <ul className="pt-2">
                  <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                     <LiaBoxSolid className="size-4" />
                     <p>Your Orders</p>
                  </li>
                  <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                     <IoBookmarkOutline className="size-4" />
                     <p>Your Wishlist</p>
                  </li>
                  <li className="flex items-center gap-2 font-SFProText text-sm text-slate-900 cursor-pointer pb-3 hover:text-blue-600">
                     <MdOutlineManageAccounts className="size-4" />
                     <Link href="/account">
                        <p>Your Account</p>
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

export default UserMenu;
