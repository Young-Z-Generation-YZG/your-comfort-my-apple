'use client';

import Link from 'next/link';
import { CardContext, DefaultActionContent } from './_components/card-content';
import { MdKeyboardArrowRight } from 'react-icons/md';
import { Avatar, AvatarFallback, AvatarImage } from '~/components/ui/avatar';
import svgs from '@assets/svgs';
import Image from 'next/image';
import Badge from './_components/badge';

const AccountPage = () => {
   return (
      <div className="flex-1 flex flex-col gap-6">
         <CardContext>
            <div className="flex flex-col gap-2">
               <div className="flex justify-between">
                  <h3 className="text-xl font-medium">Personal Information</h3>

                  <Link
                     href="#"
                     className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600"
                  >
                     Edit
                     <MdKeyboardArrowRight className="size-5 text-blue-500" />
                  </Link>
               </div>

               <div className="flex gap-4 items-center mt-5">
                  <Avatar className="w-16 h-16">
                     <AvatarImage src="https://github.com/shadcn.png" />
                     <AvatarFallback>CN</AvatarFallback>
                  </Avatar>

                  <div className="flex flex-col gap-1">
                     <span className="text-lg font-medium font-SFProText">
                        John Doe
                     </span>
                     <span className="text-sm font-light text-slate-500 font-SFProText">
                        john.doe@example.com
                     </span>
                  </div>
               </div>

               <div className="mt-3 flex justify-between items-center w-[60%]">
                  <span className="flex flex-col gap-1">
                     <span className="text-sm text-slate-400 font-SFProText">
                        Phone
                     </span>
                     <span className="text-sm text-slate-500 font-SFProText">
                        +84 0333284890
                     </span>
                  </span>

                  <span className="flex flex-col gap-1">
                     <span className="text-sm text-slate-400 font-SFProText">
                        Date of Birth
                     </span>
                     <span className="text-sm text-slate-500 font-SFProText">
                        January 1, 1990
                     </span>
                  </span>
               </div>
            </div>
         </CardContext>

         <CardContext>
            <div className="flex flex-col gap-2">
               <div className="flex justify-between">
                  <h3 className="text-xl font-medium">Shipping Addresses</h3>

                  <Link
                     href="#"
                     className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600"
                  >
                     Manage
                     <MdKeyboardArrowRight className="size-5 text-blue-500" />
                  </Link>
               </div>

               <DefaultActionContent>
                  <div className="flex flex-col gap-1 text-slate-400 font-SFProText text-sm font-light">
                     <h3 className="text-xl font-medium text-black/80 font-SFProDisplay">
                        Home
                     </h3>
                     <p>John Doe</p>
                     <p>123 Apple Street</p>
                     <p>Cupertino, CA 95014</p>
                     <p>United States</p>
                  </div>
               </DefaultActionContent>
            </div>
         </CardContext>

         <CardContext>
            <div className="flex flex-col gap-2">
               <div className="flex justify-between">
                  <h3 className="text-xl font-medium">Payment methods</h3>

                  <Link
                     href="#"
                     className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600"
                  >
                     Manage
                     <MdKeyboardArrowRight className="size-5 text-blue-500" />
                  </Link>
               </div>

               <DefaultActionContent>
                  <div className="flex items-center gap-2">
                     <Image
                        src={svgs.vnpayLogo}
                        alt="vnpay-logo"
                        className="w-[60px] rounded-lg bg-[#f3f4f6] h-[50px]"
                     />

                     <div className="flex flex-col gap-1">
                        <span className="text-base font-medium text-black/80 font-SFProText">
                           VNPay
                        </span>

                        <span className="text-slate-400 font-SFProText text-sm font-light">
                           Expires 12/2025
                        </span>
                     </div>
                  </div>
               </DefaultActionContent>
            </div>
         </CardContext>

         <CardContext>
            <div className="flex flex-col gap-2">
               <div className="flex justify-between">
                  <h3 className="text-xl font-medium">Security</h3>

                  <Link
                     href="#"
                     className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600"
                  >
                     Manage
                     <MdKeyboardArrowRight className="size-5 text-blue-500" />
                  </Link>
               </div>

               <div className="flex flex-col gap-2">
                  <div className="flex justify-between items-center w-full mt-3">
                     <div className="flex flex-col gap-1">
                        <span className="text-base font-medium text-black/80 font-SFProText">
                           Two-Factor Authentication
                        </span>
                        <span className="text-slate-400 font-SFProText text-sm font-light">
                           Protect your account with an extra layer of security
                        </span>
                     </div>

                     <Badge variants="enabled" />
                  </div>

                  <div className="flex justify-between items-center w-full mt-3">
                     <div className="flex flex-col gap-1">
                        <span className="text-base font-medium text-black/80 font-SFProText">
                           Password
                        </span>
                        <span className="text-slate-400 font-SFProText text-sm font-light">
                           Last changed 3 months ago
                        </span>
                     </div>

                     <Link
                        href="#"
                        className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600"
                     >
                        Change
                     </Link>
                  </div>
               </div>
            </div>
         </CardContext>
      </div>
   );
};

export default AccountPage;
