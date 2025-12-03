/* eslint-disable react/react-in-jsx-scope */
'use client';
import { SFDisplayFont } from '@assets/fonts/font.config';
import useMediaQuery from '@components/hooks/use-media-query';
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
   const { isMobile, isTablet } = useMediaQuery();
   const isSmallScreen = isMobile || isTablet;
   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full rounded-[20px] bg-cover bg-center bg-no-repeat shadow-[rgba(50,50,93,0.25)_0px_6px_12px_-2px,rgba(0,0,0,0.3)_0px_3px_7px_-3px]',
            {
               'max-w-[400px] h-[580px]': !isSmallScreen,
               'max-w-[360px] h-[520px]': isTablet && !isMobile,
               'max-w-full h-[420px]': isMobile,
            },
         )}
         style={{ backgroundImage: `url(${img})` }}
      >
         <div
            className={cn('p-[30px]', {
               'p-6': isSmallScreen,
               'p-5': isMobile,
            })}
            style={{ color: checkLightImg ? '#000' : '#fff' }}
         >
            <div
               className={cn(
                  'pb-[2px] text-[#6E6E73] font-normal tracking-[0.5px] uppercase',
                  {
                     'text-[12px] leading-[16px]': !isMobile,
                     'text-[11px] leading-[14px]': isMobile,
                  },
               )}
               style={{ visibility: checkPreOrder ? 'visible' : 'hidden' }}
            >
               pre-order now
            </div>
            <div
               className={cn('font-semibold tracking-[0.196px]', {
                  'pt-[6px] text-[28px] leading-[32px]': !isSmallScreen,
                  'pt-[4px] text-[24px] leading-[28px]': isTablet && !isMobile,
                  'pt-[2px] text-[22px] leading-[26px]': isMobile,
               })}
            >
               {title}
            </div>
            <div
               className={cn('font-semibold tracking-[0.1px]', {
                  'pt-[10px] text-[18px] leading-[21px]': !isSmallScreen,
                  'pt-[8px] text-[16px] leading-[20px]': isTablet && !isMobile,
                  'pt-[6px] text-[15px] leading-[18px]': isMobile,
               })}
            >
               {subtitle}
            </div>
            <div
               className={cn('font-light tracking-[0.1px]', {
                  'pt-[6px] text-[15px] leading-[18px]': !isSmallScreen,
                  'pt-[4px] text-[14px] leading-[17px]': isTablet && !isMobile,
                  'pt-[3px] text-[13px] leading-[16px]': isMobile,
               })}
            >
               {price}
            </div>
         </div>
      </div>
   );
};
export type { LastestItemType };
export default LatestItem;
