/* eslint-disable react/no-unescaped-entities */
'use client';

import { useState } from 'react';
import { cn } from '~/infrastructure/lib/utils';
import { Button } from '@components/ui/button';
import images from '@components/client/images';
import Link from 'next/link';
import Image from 'next/image';
import { MdOutlineArrowDropDown, MdArrowDropUp } from 'react-icons/md';
import ServiceCard from './service-card';
import { appleServices } from '../_constants/services-data';
import { ApplePickupIcon, DeliveryTruckIcon } from '@components/icon';

interface ProductInfoProps {
   selectedModel?: {
      name: string;
      normalized_name: string;
   } | null;
   selectedColor?: {
      name: string;
      normalized_name: string;
   } | null;
   selectedStorage?: {
      name: string;
      normalized_name: string;
   } | null;
}

const ProductInfo = ({
   selectedModel,
   selectedColor,
   selectedStorage,
}: ProductInfoProps) => {
   const [isShowMoreInfo, setIsShowMoreInfo] = useState(false);

   return (
      <div
         className={cn(
            'w-full mt-[73px] bg-[#f5f5f7] flex flex-col items-center overflow-hidden relative',
            {
               'h-[550px]': !isShowMoreInfo,
            },
         )}
      >
         <div
            id="info"
            className="min-w-[980px] max-w-[1240px] w-[87.5%] mx-auto mt-[39px] flex flex-row gap-0"
         >
            <div className="basis-[34.76%]">
               <div className="pr-[10px] mr-[41px] flex flex-col items-start justify-start">
                  <span className="text-[#1D1D1F] tracking-[0.3px] w-full text-[38px] font-semibold leading-[46px]">
                     Your new <br />
                     {'NO DATA AVAILABLE'}.
                  </span>
                  <span className="text-[#86868B] tracking-[0.3px] w-full text-[38px] font-semibold leading-[46px]">
                     Just the way you <br />
                     want it.
                  </span>
                  <Image
                     src={images.ip16ProWhiteTitaniumSelect}
                     className="w-full h-auto pt-[42px]"
                     width={2000}
                     height={2000}
                     quality={100}
                     alt=""
                  />
               </div>
            </div>
            <div className="basis-[38.9%]">
               <div className="pr-[20px] mr-[41px] flex flex-col items-start justify-start">
                  <div className="product-info w-full pb-11 text-[17px] text-[#1D1D1F] leading-[25px] tracking-[0.3px]">
                     <div className="w-full pb-[7px] font-light">
                        {selectedModel?.name} {selectedStorage?.name}{' '}
                        {selectedColor?.name}
                     </div>
                     <div className="w-full font-semibold">$1,299.00</div>
                     <div className="w-full pt-[35px] font-semibold">
                        One-time payment
                     </div>
                     <div className="w-full pt-[14px] text-[14px] font-light">
                        Get 3% Daily Cash with Apple Card
                     </div>
                  </div>
                  <div className="save-info w-full py-[21px] opacity-50 border-t-[0.8px] border-[#86868b] text-[14px] text-[#1D1D1F] leading-[20px] tracking-[0.3px]">
                     <div className="w-full font-semibold">Need a moment?</div>
                     <div className="w-full font-light pr-5 pb-2">
                        Keep all your selections by saving this device to Your
                        Saves, then come back anytime and pick up right where
                        you left off.
                     </div>
                     <Link
                        className="w-full font-light flex flex-row text-[#06c]"
                        href="#"
                     >
                        <svg
                           width="21"
                           height="21"
                           className="as-svgicon as-svgicon-bookmark as-svgicon-tiny as-svgicon-bookmarktiny"
                           role="img"
                           aria-hidden="true"
                        >
                           <path fill="none" d="M0 0h21v21H0z"></path>
                           <path
                              fill="#06c"
                              d="M12.8 4.25a1.202 1.202 0 0 1 1.2 1.2v10.818l-2.738-2.71a1.085 1.085 0 0 0-1.524 0L7 16.269V5.45a1.202 1.202 0 0 1 
                           1.2-1.2h4.6m0-1H8.2A2.2 2.2 0 0 0 6 5.45v11.588a.768.768 0 0 0 .166.522.573.573 0 0 0 .455.19.644.644 0 0 0 .38-.128 5.008 5.008 0 0 0 
                           .524-.467l2.916-2.885a.084.084 0 0 1 .118 0l2.916 2.886a6.364 6.364 0 0 0 .52.463.628.628 0 0 0 .384.131.573.573 0 0 0 .456-.19.768.768 0 0 0 
                           .165-.522V5.45a2.2 2.2 0 0 0-2.2-2.2Z"
                           />
                        </svg>
                        Save for later
                     </Link>
                  </div>
               </div>
            </div>
            <div className="basis-[26.34%]">
               <div className="flex flex-row">
                  <DeliveryTruckIcon width={25} height={25} className="mr-2" />
                  <div className="flex flex-col text-[15px] text-[#1D1D1F] font-light leading-[25px] tracking-[0.3px]">
                     <div className="font-semibold">Delivery:</div>
                     <div>In Stock</div>
                     <div>Free Shipping</div>
                     <Link href="#" className="text-[#06c]">
                        Get delivery dates
                     </Link>
                  </div>
               </div>
               <div className="flex flex-row mt-4">
                  <ApplePickupIcon width={25} height={25} className="mr-2" />
                  <div className="flex flex-col text-[15px] text-[#1D1D1F] font-light leading-[25px] tracking-[0.3px]">
                     <div className="font-semibold">Pickup:</div>
                     <Link href="#" className="text-[#06c]">
                        Check availability
                     </Link>
                  </div>
               </div>

               <div className="button mt-12" onClick={() => {}}>
                  <Button className="w-full bg-[#06c] text-white text-[15px] font-light leading-[18px] tracking-[0.8px] h-fit py-3">
                     Continue
                  </Button>
               </div>
            </div>
         </div>
         <div
            className={cn('w-full bg-[#fff]', {
               invisible: !isShowMoreInfo,
            })}
         >
            <div className="w-[980px] mx-auto">
               <div className="w-full pt-16 pb-[41px] text-[40px] font-semibold leading-[44x] tracking-[0.5px] text-center">
                  What's in the Box
               </div>
               <div className="w-full flex flex-col gap-0 justify-center items-center">
                  <div className="w-full bg-[#fafafc] flex flex-row gap-0 justify-center items-center">
                     <div className="basis-[25%]">
                        <Image
                           src={images.ip16ProWhiteTitaniumWitb}
                           className="w-auto h-[339px] mx-auto"
                           width={2000}
                           height={2000}
                           quality={100}
                           alt=""
                        />
                     </div>
                     <div className="basis-[25%]">
                        <Image
                           src={images.ip16ProBraidedCable}
                           className="w-auto h-[339px] mx-auto"
                           width={2000}
                           height={2000}
                           quality={100}
                           alt=""
                        />
                     </div>
                  </div>
                  <div className="w-full flex flex-row gap-0 justify-center items-center">
                     <div className="basis-[25%] pt-5 pb-[50px] text-center text-[14px] font-light leading-[20px] tracking-[0.3px]">
                        {'NO DATA AVAILABLE'}
                     </div>
                     <div className="basis-[25%] pt-5 pb-[50px] text-center text-[14px] font-light leading-[20px] tracking-[0.3px]">
                        USB-C Charge Cable
                     </div>
                  </div>
               </div>
            </div>
         </div>
         <div className="w-full bg-[#fff] pb-10">
            <div className="w-[980px] mx-auto">
               <div className="w-fit pt-12 pb-5 px-5 text-[40px] mx-auto font-semibold leading-[44px] tracking-[0.5px] text-center">
                  Your new iPhone comes
                  <br /> with so much more.
               </div>
               <div className="w-full pt-9 pb-[54px] px-4 mt-[13px] flex flex-row gap-8 justify-center items-center">
                  {appleServices.map((service, index) => (
                     <ServiceCard
                        key={index}
                        icon={service.icon}
                        title={service.title}
                        description={service.description}
                     />
                  ))}
               </div>
            </div>
         </div>

         <Button
            className="button-more absolute bottom-12 left-1/2 -translate-x-1/2 bg-transparent font-thin text-xl hover:bg-transparent border-t-0 border-l-0 border-r-0 text-blue-500 border-b border-b-blue-500 rounded-none hover:text-blue-600 py-0 h-fit"
            variant="outline"
            onClick={() => {
               setIsShowMoreInfo(!isShowMoreInfo);
            }}
         >
            {isShowMoreInfo ? 'Collapse' : 'Expand'}
            {isShowMoreInfo ? <MdArrowDropUp /> : <MdOutlineArrowDropDown />}
         </Button>
      </div>
   );
};

export default ProductInfo;
