/* eslint-disable react/react-in-jsx-scope */
'use client';
import Image from 'next/image';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';

const HelpItem = ({ image, title, subTitle }: { image?: string, title: string, subTitle: string }) => {
     const checkImage = !!image;

     return (
          <div className={cn(SFDisplayFont.variable, "font-SFProDisplay w-full h-fit bg-[#f5f5f7] text-[14px] rounded-[10px] flex flex-row overflow-hidden")}>
               { checkImage && 
               <Image
                    alt=''
                    src={image as string}
                    className='w-[120px] h-auto'
                    quality={100}
                    width={2000}
                    height={2000}
                    style={{
                         objectFit: 'cover',
                    }}
               />}
               <div className='py-[17px] px-[16px]'>
                    <div className='text-[14px] font-semibold leading-[18px] tracking-[0.5px] pr-[22px]'>{title}</div>
                    <div className='text-[14px] font-light leading-[18px] tracking-[0.5px]'>{subTitle}</div>
               </div>
          </div>
     )
}

export default HelpItem