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
   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-[280px] h-[380px] sm:w-[320px] sm:h-[440px] md:w-[360px] md:h-[500px] lg:w-[400px] lg:h-[560px] xl:w-[450px] xl:h-[600px] rounded-[20px] bg-cover bg-center bg-no-repeat shadow-[rgba(50,50,93,0.25)_0px_6px_12px_-2px,rgba(0,0,0,0.3)_0px_3px_7px_-3px]',
         )}
         style={{ backgroundImage: `url(${img})` }}
      >
         <div
            className="w-[400px] p-[30px]"
            style={{ color: checkLightImg ? '#000' : '#fff' }}
         >
            <div
               className="pb-[2px] text-[12px] font-normal leading-4 text-[#6E6E73] tracking-[0.5px] uppercase"
               style={{ visibility: checkSubTitle ? 'visible' : 'hidden' }}
            >
               {checkSubTitle ? subtitle : 'none'}
            </div>
            <div className="pt-[6px] text-[28px] font-semibold leading-8 tracking-[0.196px]">
               {title}
            </div>
            <div className="pt-[6px] text-[17px] font-light leading-[18px] tracking-[0.1px]">
               {content}
            </div>
         </div>
      </div>
   );
};

export type { ExperienceItemType };
export default ExperienceItem;
