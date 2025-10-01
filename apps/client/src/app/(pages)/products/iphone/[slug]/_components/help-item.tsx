'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { CirclePlus } from 'lucide-react';

interface HelpItemProps {
   className?: string;
}

const HelpItem = ({ className = '' }: HelpItemProps) => {
   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full h-fit bg-[#f5f5f7] text-[14px] rounded-[12px] flex flex-row overflow-hidden cursor-pointer hover:bg-[#e8e8ed] transition-colors',
            className,
         )}
      >
         <div className="py-[14px] px-[16px] pr-[40px] relative w-full">
            <div className="text-[14px] font-semibold leading-[18px] tracking-[-0.01em] text-[#1D1D1F]">
               Need help choosing a model?
            </div>
            <div className="text-[14px] mt-[4px] font-normal leading-[18px] tracking-[-0.01em] text-[#1D1D1F]">
               Explore the differences in screen size and battery life.
            </div>

            <span className="absolute right-[16px] top-[14px]">
               <CirclePlus className="w-[20px] h-[20px] text-[#1D1D1F] stroke-[1.5]" />
            </span>
         </div>
      </div>
   );
};

export default HelpItem;
