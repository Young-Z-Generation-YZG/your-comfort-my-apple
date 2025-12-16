'use client';

import SideBar from './_components/layouts/side-bar';
import withAuth from '~/components/HoCs/with-auth.hoc';

const AccountLayout = ({ children }: { children: React.ReactNode }) => {
   return (
      <div className="w-full bg-[#f5f5f7] px-4 sm:px-6 md:px-8 py-8 md:py-10">
         <div className="w-full max-w-[1200px] mx-auto mt-6 md:mt-10">
            <h2 className="font-medium text-2xl sm:text-3xl md:text-4xl">
               Account
            </h2>
            <p className="font-light font-SFProText mt-1 text-slate-500 text-sm md:text-base">
               Manage your Account ID and account settings.
            </p>

            <div className="flex flex-col lg:flex-row gap-4 lg:gap-6 mt-6 md:mt-10 w-full">
               <div className="w-full lg:w-1/4">
                  <SideBar />
               </div>

               <div className="content w-full lg:flex-1 mt-6 lg:mt-0">
                  {children}
               </div>
            </div>
         </div>
      </div>
   );
};

export default withAuth(AccountLayout);
