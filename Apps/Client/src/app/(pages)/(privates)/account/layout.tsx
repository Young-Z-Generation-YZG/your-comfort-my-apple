'use client';

import { useRouter } from 'next/navigation';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { setRouter } from '~/infrastructure/redux/features/app.slice';
import { useAppSelector } from '~/infrastructure/redux/store';
import SideBar from './_components/layouts/side-bar';
import Link from 'next/link';
import { CardContext, DefaultActionContent } from './_components/card-content';
import { MdKeyboardArrowRight } from 'react-icons/md';
import { Avatar, AvatarFallback, AvatarImage } from '~/components/ui/avatar';
import svgs from '@assets/svgs';
import Image from 'next/image';
import Badge from './_components/badge';

const AccountLayout = ({ children }: { children: React.ReactNode }) => {
   const router = useRouter();
   const dispatch = useDispatch();

   const { isAuthenticated, timerId, isLogoutTriggered } = useAppSelector(
      (state) => state.auth.value,
   );

   const currentPath = window.location.pathname;

   const appRouter = useAppSelector((state) => state.app.value.router);

   useEffect(() => {
      if (!isAuthenticated) {
         if (isLogoutTriggered) {
            dispatch(
               setRouter({
                  previousPath: null,
               }),
            );

            router.push('/sign-in');
         }

         if (!isLogoutTriggered && appRouter.previousPath) {
            router.push(appRouter.previousPath);
         } else {
            dispatch(
               setRouter({
                  previousPath: currentPath,
               }),
            );
            router.push('/sign-in');
         }
      }
   }, [isAuthenticated, timerId, router]);

   return (
      <div className="w-full bg-[#f5f5f7] px-5 py-10">
         <div className="w-[1200px] mx-auto mt-10">
            <h2 className="text-4xl font-medium">Account</h2>
            <p className="font-light font-SFProText mt-1 text-slate-500">
               Manage your Account ID and account settings.
            </p>

            <div className="flex gap-2 mt-10 w-full">
               <div className="w-1/4">
                  <SideBar />
               </div>

               <div className="content w-full">{children}</div>
            </div>
         </div>
      </div>
   );
};

export default AccountLayout;
