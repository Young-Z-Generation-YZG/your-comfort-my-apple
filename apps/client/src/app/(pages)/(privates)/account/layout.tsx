'use client';

import { useRouter } from 'next/navigation';
import { useDispatch } from 'react-redux';
import { useAppSelector } from '~/infrastructure/redux/store';
import SideBar from './_components/layouts/side-bar';
import withAuth from '@components/HoCs/with-auth.hoc';

const AccountLayout = ({ children }: { children: React.ReactNode }) => {
   const router = useRouter();
   const dispatch = useDispatch();

   const { isAuthenticated, timerId, isLogoutTriggered } = useAppSelector(
      (state) => state.auth.value,
   );

   const currentPath = window.location.pathname;

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

export default withAuth(AccountLayout);
