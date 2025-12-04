'use client';

import Link from 'next/link';
import { CardContext, DefaultActionContent } from './_components/card-content';
import { MdKeyboardArrowRight } from 'react-icons/md';
import { Avatar, AvatarFallback, AvatarImage } from '@components/ui/avatar';
import svgs from '@assets/svgs';
import Image from 'next/image';
import Badge from './_components/badge';
import { useEffect, useMemo, useState } from 'react';
import { ProfilePicture } from './_components/profile-picture';
import TwoRowSkeleton from '@components/ui/two-row-skeleton';
import ImageSkeleton from '@components/ui/image-skeleton';
import { Skeleton } from '@components/ui/skeleton';
import { IoChevronBack } from 'react-icons/io5';
import ProfileForm from './_components/form/profile-form';
import useIdentityService from '@components/hooks/api/use-identity-service';

const AccountPage = () => {
   const [editProfile, setEditProfile] = useState(false);

   //  const { data: queryData, ...queryDataState } = useGetMeQuery();
   const { getMeAsync, getMeState, isLoading } = useIdentityService();

   useEffect(() => {
      const fetchMe = async () => {
         const result = await getMeAsync();
         if (result.isSuccess) {
            console.log(result.data);
         }
      };
      fetchMe();
   }, [getMeAsync]);

   const displayMeInfo = useMemo(() => {
      if (getMeState.isSuccess && getMeState.data) {
         return getMeState.data;
      }
      return null;
   }, [getMeState.isSuccess, getMeState.data]);

   const renderEditProfile = () => {
      return (
         <div>
            <CardContext>
               <div className="flex flex-col gap-2">
                  <div className="flex flex-col sm:flex-row sm:justify-between gap-2">
                     <h3 className="font-medium text-lg md:text-xl">Profile picture</h3>

                     <span
                        onClick={() => {
                           setEditProfile(!editProfile);
                        }}
                        className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600 font-medium cursor-pointer"
                     >
                        <IoChevronBack />
                        <p>Back</p>
                     </span>
                  </div>

                  <ProfilePicture />
               </div>
            </CardContext>

            {/* <CardContext className="mt-5">
               <ProfileForm
                  profile={{
                     email: meInfo.email,
                     firstName: meInfo.first_name,
                     lastName: meInfo.last_name,
                     phoneNumber: meInfo.phone_number,
                     birthDate: meInfo.birth_date,
                     imageId: meInfo.image_id,
                     imageUrl: meInfo.image_url,
                  }}
               />
            </CardContext> */}
         </div>
      );
   };

   return (
      <div className="flex-1 flex flex-col gap-6">
         {editProfile ? (
            renderEditProfile()
         ) : (
            <div className="flex gap-5 flex-col">
               <CardContext>
                  <div className="flex flex-col gap-2">
                     <div className="flex flex-col sm:flex-row sm:justify-between gap-2">
                        <h3 className="font-medium text-lg md:text-xl">
                           Personal Information
                        </h3>

                        <span
                           onClick={() => {
                              setEditProfile(!editProfile);
                           }}
                           className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600 cursor-pointer"
                        >
                           Edit
                           <MdKeyboardArrowRight className="size-5 text-blue-500" />
                        </span>
                     </div>

                     <div className="flex gap-4 items-center mt-5">
                        {getMeState.isLoading || getMeState.isFetching ? (
                           <ImageSkeleton className="w-16 h-16" />
                        ) : (
                           <Avatar className="w-16 h-16">
                              <AvatarImage src="https://github.com/shadcn.png" />
                              <AvatarFallback>CN</AvatarFallback>
                           </Avatar>
                        )}

                        <div className="flex flex-col gap-1">
                           {getMeState.isLoading || getMeState.isFetching ? (
                              <TwoRowSkeleton className="h-[30px] w-[200px]" />
                           ) : (
                              <span className="flex flex-col gap-1">
                                 <span className="text-lg font-medium font-SFProText">
                                    {displayMeInfo?.first_name}{' '}
                                    {displayMeInfo?.last_name}
                                 </span>
                                 <span className="text-sm font-light text-slate-500 font-SFProText">
                                    {displayMeInfo?.email}
                                 </span>
                              </span>
                           )}
                        </div>
                     </div>

                     <div className="mt-3 flex flex-col sm:flex-row sm:justify-between items-start sm:items-center w-full sm:w-[65%] gap-3">
                        <span className="flex flex-col gap-1">
                           <span className="text-sm text-slate-400 font-SFProText">
                              Phone
                           </span>
                           {isLoading ? (
                              <Skeleton className="h-[24px] w-[200px]" />
                           ) : (
                              <span className="text-sm text-slate-500 font-SFProText">
                                 +84 {displayMeInfo?.phone_number}
                              </span>
                           )}
                        </span>

                        <span className="flex flex-col gap-1">
                           <span className="text-sm text-slate-400 font-SFProText">
                              Date of Birth
                           </span>
                           {isLoading ? (
                              <Skeleton className="h-[24px] w-[200px]" />
                           ) : (
                              <span className="text-sm text-slate-500 font-SFProText w-[200px]">
                                 {displayMeInfo?.birth_date &&
                                    new Date(
                                       displayMeInfo.birth_date,
                                    ).toLocaleDateString('en-US', {
                                       year: 'numeric',
                                       month: 'long',
                                       day: 'numeric',
                                    })}
                              </span>
                           )}
                        </span>
                     </div>
                  </div>
               </CardContext>

               <CardContext>
                  <div className="flex flex-col gap-2">
                     <div className="flex flex-col sm:flex-row sm:justify-between gap-2">
                        <h3 className="font-medium text-lg md:text-xl">
                           Shipping Addresses
                        </h3>

                        <Link
                           href="/account/addresses"
                           className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600"
                        >
                           Manage
                           <MdKeyboardArrowRight className="size-5 text-blue-500" />
                        </Link>
                     </div>

                     <DefaultActionContent>
                        {isLoading ? (
                           <div className="flex flex-col gap-1 text-slate-400 font-SFProText text-sm font-light w-full">
                              <Skeleton className="h-[26px] w-full" />
                              <Skeleton className="h-[26px] w-full" />
                              <Skeleton className="h-[26px] w-full" />
                              <Skeleton className="h-[26px] w-full" />
                           </div>
                        ) : (
                           <div className="flex flex-col gap-1 text-slate-400 font-SFProText text-sm font-light">
                              <h3 className="text-xl font-medium text-black/80 font-SFProDisplay">
                                 {displayMeInfo?.default_address_label}
                              </h3>
                              <p>
                                 {displayMeInfo?.default_contact_name} -{' '}
                                 {displayMeInfo?.default_contact_phone_number}
                              </p>
                              <p>{displayMeInfo?.default_address_line}</p>
                              <p>{displayMeInfo?.default_address_district}</p>
                              <p>{displayMeInfo?.default_address_province}</p>
                              <p>{displayMeInfo?.default_address_country}</p>
                           </div>
                        )}
                     </DefaultActionContent>
                  </div>
               </CardContext>

               <CardContext>
                  <div className="flex flex-col gap-2">
                     <div className="flex flex-col sm:flex-row sm:justify-between gap-2">
                        <h3 className="font-medium text-lg md:text-xl">Payment methods</h3>

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
                     <div className="flex flex-col sm:flex-row sm:justify-between gap-2">
                        <h3 className="font-medium text-lg md:text-xl">Security</h3>

                        <Link
                           href="/account/security"
                           className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600"
                        >
                           Manage
                           <MdKeyboardArrowRight className="size-5 text-blue-500" />
                        </Link>
                     </div>

                     <div className="flex flex-col gap-2">
                        <div className="flex flex-col sm:flex-row sm:justify-between items-start sm:items-center w/full mt-3 gap-3">
                           <div className="flex flex-col gap-1">
                              <span className="text-base font-medium text-black/80 font-SFProText">
                                 Two-Factor Authentication
                              </span>
                              <span className="text-slate-400 font-SFProText text-sm font-light">
                                 Protect your account with an extra layer of
                                 security
                              </span>
                           </div>

                           <Badge variants="enabled" />
                        </div>

                        <div className="flex flex-col sm:flex-row sm:justify-between items-start sm:items-center w/full mt-3 gap-3">
                           <div className="flex flex-col gap-1">
                              <span className="text-base font-medium text-black/80 font-SFProText">
                                 Password
                              </span>
                              <span className="text-slate-400 font-SFProText text-sm font-light">
                                 Last changed 3 months ago
                              </span>
                           </div>

                           <Link
                              href="/account/security"
                              className="flex justify-center items-center gap-1 font-SFProText text-sm text-blue-500 hover:text-blue-600"
                           >
                              Change
                           </Link>
                        </div>
                     </div>
                  </div>
               </CardContext>
            </div>
         )}
      </div>
   );
};

export default AccountPage;
