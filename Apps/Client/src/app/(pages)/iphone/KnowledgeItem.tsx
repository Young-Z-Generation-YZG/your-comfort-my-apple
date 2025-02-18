/* eslint-disable react/react-in-jsx-scope */
'use client';
import Image from 'next/image';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
const KnowledgeItem = ({ item }: { item: any }) => {
     const {
          id,
          title,
          subtitle,
          img,
          checkLightImg
     } = item;
     return ( 
          <div className={cn(SFDisplayFont.variable, "font-SFProDisplay relative overflow-hidden w-[405px] h-[600px] rounded-[20px] bg-cover bg-center bg-no-repeat shadow-[rgba(50,50,93,0.25)_0px_6px_12px_-2px,rgba(0,0,0,0.3)_0px_3px_7px_-3px]")}>
               <Image src={img} alt="Knowledge Item" className="w-full h-auto object-cover " width={1000} height={1000} quality={100} />
               <div className="p-[32px] w-full absolute top-0" style={{color: checkLightImg ? '#000' : '#fff'}}>
                    <div className="text-[18px] font-medium leading-[21px] tracking-[0.1px]">{subtitle}</div>
                    <div className="mt-2 text-[28px] font-semibold leading-[32px] tracking-[0.196px] ">{title}</div>
               </div>
          </div>
     );
}

export default KnowledgeItem;
