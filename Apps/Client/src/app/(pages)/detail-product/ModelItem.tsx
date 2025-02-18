/* eslint-disable react/react-in-jsx-scope */
'use client';
import Image from 'next/image';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
// {product}: {product: any}
interface ModelItemProps {
   isSelected: boolean;
   onClick: () => void;
}

const ModelItem = ({isSelected, onClick}: ModelItemProps) => {

     return (
          <div 
               className={cn(SFDisplayFont.variable, "font-SFProDisplay w-full h-fit border-2 rounded-[10px] p-[14px] mt-[14px]", isSelected ? 'border-[#0071E3]' : 'border-[#ccc]')}  
               onClick={onClick}
          >
               <div className='w-full flex flex-row items-center'>
                    <div className='basis-2/3'>
                         <span className='text-[17px] font-semibold leading-[21px] tracking-[0.3px]'>iPhone 16 Pro</span>
                         <br/>
                         <span className='text-[12px] font-light leading-[16px] tracking-[0.4px]'>6.3-inch display</span>
                    </div>
                    <div className='basis-1/3 text-end text-[12px] font-light leading-[16px] tracking-[0.4px]'>
                         <span>From $999</span>
                         <br/>
                         <span>or $41.62/mo</span>
                         <br/>
                         <span>for 24 mo.</span>
                    </div>
               </div>
               <div className='w-full flex flex-row items-center justify-start pt-[10px]'>
                    <Image 
                         src={'/images/compare-imgs/icon-app-intell.jpg'} 
                         alt='icon-app-intell' 
                         className='w-[15px] h-[15px]' 
                         width={15} 
                         height={15} 
                         quality={100}/>
                    <div className='pl-[6px] text-[13px] font-light leading-[16px] tracking-[0.1px]'>Apple Intelligence</div>
               </div>
          </div>
     );
}

export default ModelItem;