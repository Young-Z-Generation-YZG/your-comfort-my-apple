/* eslint-disable react/react-in-jsx-scope */
'use client';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { cn } from '~/infrastructure/lib/utils';

type ExperienceItemType = {
   id: number;
   subtitle: string;
   title: string;
   content: string;
   img: string;
   checkLightImg: boolean;
};

const ExperienceItem = ({ experience }: { experience: ExperienceItemType }) => {
   const { id, subtitle, title, content, img, checkLightImg } = experience;
   const checkSubTitle = subtitle !== '';

   // Tailwind-based responsive classes
   const containerWidth = 'w-full sm:w-[360px] md:w-[400px]';
   const containerHeight = 'h-[420px] sm:h-[520px] md:h-[580px]';
   const contentWidth = 'w-full sm:w-[320px] md:w-[360px]';
   const contentPadding = 'p-[20px] sm:p-[25px] md:p-[30px]';
   const subtitleSize = 'text-[10px] sm:text-[11px] md:text-[12px]';
   const titleSize = 'text-[22px] sm:text-[25px] md:text-[28px]';
   const titleLeading = 'leading-7 md:leading-8';
   const contentSize = 'text-[15px] sm:text-[16px] md:text-[17px]';
   const contentLeading = 'leading-[16px] sm:leading-[17px] md:leading-[18px]';

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay rounded-[20px] bg-cover bg-center bg-no-repeat shadow-[rgba(50,50,93,0.25)_0px_6px_12px_-2px,rgba(0,0,0,0.3)_0px_3px_7px_-3px]',
            containerWidth,
            containerHeight,
         )}
         style={{ backgroundImage: `url(${img})` }}
      >
         <div
            className={cn(contentWidth, contentPadding)}
            style={{ color: checkLightImg ? '#000' : '#fff' }}
         >
            <div
               className={cn(
                  'pb-[2px] font-normal leading-4 text-[#6E6E73] tracking-[0.5px] uppercase',
                  subtitleSize,
               )}
               style={{ visibility: checkSubTitle ? 'visible' : 'hidden' }}
            >
               {checkSubTitle ? subtitle : 'none'}
            </div>
            <div
               className={cn(
                  'pt-[6px] font-semibold tracking-[0.196px]',
                  titleSize,
                  titleLeading,
               )}
            >
               {title}
            </div>
            <div
               className={cn(
                  'pt-[6px] font-light tracking-[0.1px]',
                  contentSize,
                  contentLeading,
               )}
            >
               {content}
            </div>
         </div>
      </div>
   );
};

export type { ExperienceItemType };
export default ExperienceItem;
