/* eslint-disable react/react-in-jsx-scope */
'use client';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { cn } from '~/infrastructure/lib/utils';

type LastestItemType = {
   id: number;
   checkPreOrder: boolean;
   title: string;
   subtitle: string;
   price: string;
   img: string;
   checkLightImg: boolean;
};

const LatestItem = ({ product }: { product: LastestItemType }) => {
   const { checkPreOrder, title, subtitle, price, img, checkLightImg } =
      product;

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full rounded-[20px] bg-cover bg-center bg-no-repeat shadow-[rgba(50,50,93,0.25)_0px_6px_12px_-2px,rgba(0,0,0,0.3)_0px_3px_7px_-3px] max-w-full h-[420px] sm:max-w-[360px] sm:h-[520px] md:max-w-[400px] md:h-[580px]',
         )}
         style={{ backgroundImage: `url(${img})` }}
      >
         <div
            className={cn('p-5 sm:p-6 md:p-[30px]')}
            style={{ color: checkLightImg ? '#000' : '#fff' }}
         >
            <div
               className={cn(
                  'pb-[2px] text-[#6E6E73] font-normal tracking-[0.5px] uppercase text-[11px] leading-[14px] md:text-[12px] md:leading-[16px]',
               )}
               style={{ visibility: checkPreOrder ? 'visible' : 'hidden' }}
            >
               pre-order now
            </div>
            <div
               className={cn(
                  'font-semibold tracking-[0.196px] pt-[2px] text-[22px] leading-[26px] sm:pt-[4px] sm:text-[24px] sm:leading-[28px] md:pt-[6px] md:text-[28px] md:leading-[32px]',
               )}
            >
               {title}
            </div>
            <div
               className={cn(
                  'font-semibold tracking-[0.1px] pt-[6px] text-[15px] leading-[18px] sm:pt-[8px] sm:text-[16px] sm:leading-[20px] md:pt-[10px] md:text-[18px] md:leading-[21px]',
               )}
            >
               {subtitle}
            </div>
            <div
               className={cn(
                  'font-light tracking-[0.1px] pt-[3px] text-[13px] leading-[16px] sm:pt-[4px] sm:text-[14px] sm:leading-[17px] md:pt-[6px] md:text-[15px] md:leading-[18px]',
               )}
            >
               {price}
            </div>
         </div>
      </div>
   );
};
export type { LastestItemType };
export default LatestItem;
