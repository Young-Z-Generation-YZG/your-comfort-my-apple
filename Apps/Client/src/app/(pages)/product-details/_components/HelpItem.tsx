/* eslint-disable react/react-in-jsx-scope */
'use client';
import Image from 'next/image';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { CirclePlus } from 'lucide-react';

type HelpItemProps = {
   image?: string;
   className?: string;
   title: string;
   subTitle: string;
};

const HelpItem = ({
   image,
   title,
   subTitle,
   className = '',
}: HelpItemProps) => {
   const checkImage = !!image;

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full h-fit bg-[#f5f5f7] text-[14px] rounded-[10px] flex flex-row overflow-hidden',
         )}
      >
         {checkImage && (
            <Image
               alt=""
               src={image as string}
               className="w-[120px] h-auto"
               quality={100}
               width={2000}
               height={2000}
               style={{
                  objectFit: 'cover',
               }}
            />
         )}
         <div className="py-[17px] px-[16px] relative">
            <div className="text-[14px] font-semibold leading-[18px] tracking-[0.5px] pr-[22px]">
               {title}
            </div>
            <div className="text-sm mt-2 font-light leading-[18px] tracking-[0.5px]">
               {subTitle}
            </div>

            <span className="absolute right-2 top-2">
               <CirclePlus className="w-[18px] h-[18px]" />
            </span>
         </div>
      </div>
   );
};

export default HelpItem;
