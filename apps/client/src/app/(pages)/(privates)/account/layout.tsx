'use client';

import SideBar from './_components/layouts/side-bar';
import withAuth from '@components/HoCs/with-auth.hoc';
import MobileFloatingMenu from './_components/menu-mobile';

const AccountLayout = ({ children }: { children: React.ReactNode }) => {
   return (
      <div className="w-full bg-[#f5f5f7] px-5 py-10">
         <div className="max-w-[1200px] mx-auto mt-10">
            <h2 className="text-4xl font-medium">Account</h2>
            <p className="font-light font-SFProText mt-1 text-slate-500">
               Manage your Account ID and account settings.
            </p>
            <div className="flex gap-2 mt-10 w-full">
               <div className="max-[600px]:hidden flex md:w-1/4">
                  <SideBar />
               </div>
               <div className="content md:w-3/4 w-full">{children}</div>
               <div className="max-[600px]:flex hidden">
                  <MobileFloatingMenu />
               </div>
            </div>
         </div>
      </div>
   );
};

export default withAuth(AccountLayout);
