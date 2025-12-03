/* eslint-disable react/react-in-jsx-scope */
'use client';
import { SFDisplayFont } from '@assets/fonts/font.config';
import useMediaQuery from '@components/hooks/use-media-query';
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

   const { isMobile, isTablet, isDesktop, isLargeDesktop } = useMediaQuery();

   // Responsive dimensions
   const containerWidth = isMobile
      ? 'w-full'
      : isTablet
        ? 'w-[350px]'
        : isDesktop
          ? 'w-[400px]'
          : 'w-[450px]';

   const containerHeight = isMobile
      ? 'h-[500px]'
      : isTablet
        ? 'h-[550px]'
        : 'h-[600px]';

   const contentWidth = isMobile
      ? 'w-full'
      : isTablet
        ? 'w-[320px]'
        : isDesktop
          ? 'w-[370px]'
          : 'w-[400px]';

   const contentPadding = isMobile
      ? 'p-[20px]'
      : isTablet
        ? 'p-[25px]'
        : 'p-[30px]';

   const subtitleSize = isMobile
      ? 'text-[10px]'
      : isTablet
        ? 'text-[11px]'
        : 'text-[12px]';

   const titleSize = isMobile
      ? 'text-[22px]'
      : isTablet
        ? 'text-[25px]'
        : 'text-[28px]';

   const titleLeading = isMobile
      ? 'leading-7'
      : isDesktop
        ? 'leading-8'
        : 'leading-7';

   const contentSize = isMobile
      ? 'text-[15px]'
      : isTablet
        ? 'text-[16px]'
        : 'text-[17px]';

   const contentLeading = isMobile
      ? 'leading-[16px]'
      : isTablet
        ? 'leading-[17px]'
        : 'leading-[18px]';

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
