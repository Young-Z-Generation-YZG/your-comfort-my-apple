/* eslint-disable react/react-in-jsx-scope */
'use client';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
const ExperienceItem = ({ experience }: { experience: any }) => {
     const {
          id,
          subtitle,
          title,
          content,
          img,
          checkLightImg
     } = experience;
     const checkSubTitle = subtitle !== '';
     return ( 
          <div className={cn(SFDisplayFont.variable, "font-SFProDisplay w-[480px] h-[500px] rounded-[20px] bg-cover bg-center bg-no-repeat shadow-[rgba(50,50,93,0.25)_0px_6px_12px_-2px,rgba(0,0,0,0.3)_0px_3px_7px_-3px]")} style={{ backgroundImage: `url(${img})` }}>
               <div className="w-[400px] p-[30px]" style={{color: checkLightImg ? '#000' : '#fff'}}>
                    <div className="pb-[2px] text-[12px] font-normal leading-4 text-[#6E6E73] tracking-[0.5px] uppercase" style={{visibility: checkSubTitle ? 'visible' : 'hidden'}}>{checkSubTitle ? subtitle : 'none'}</div>
                    <div className="pt-[6px] text-[28px] font-semibold leading-8 tracking-[0.196px]">{title}</div>
                    <div className="pt-[6px] text-[17px] font-light leading-[18px] tracking-[0.1px]">{content}</div>
               </div>
          </div>
     );
}

export default ExperienceItem;