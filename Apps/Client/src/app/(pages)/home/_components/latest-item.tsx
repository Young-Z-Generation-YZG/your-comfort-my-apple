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
   const { id, checkPreOrder, title, subtitle, price, img, checkLightImg } =
      product;
   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-[450px] h-[600px] rounded-[20px] bg-cover bg-center bg-no-repeat shadow-[rgba(50,50,93,0.25)_0px_6px_12px_-2px,rgba(0,0,0,0.3)_0px_3px_7px_-3px]',
         )}
         style={{ backgroundImage: `url(${img})` }}
      >
         <div
            className="p-[30px]"
            style={{ color: checkLightImg ? '#000' : '#fff' }}
         >
            <div
               className="pb-[2px] text-[12px] font-normal leading-[16px] text-[#6E6E73] tracking-[0.5px] uppercase"
               style={{ visibility: checkPreOrder ? 'visible' : 'hidden' }}
            >
               pre-order now
            </div>
            <div className="pt-[6px] text-[28px] font-semibold leading-[32px] tracking-[0.196px] ">
               {title}
            </div>
            <div className="pt-[10px] text-[18px] font-semibold leading-[21px] tracking-[0.1px]">
               {subtitle}
            </div>
            <div className="pt-[6px] text-[15px] font-light leading-[18px] tracking-[0.1px]">
               {price}
            </div>
         </div>
      </div>
   );
};
export type { LastestItemType };
export default LatestItem;
