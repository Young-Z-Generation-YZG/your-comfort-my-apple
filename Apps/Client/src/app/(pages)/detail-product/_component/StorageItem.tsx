/* eslint-disable react/react-in-jsx-scope */
'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';

interface StorageItemProps {
     isSelected: boolean;
     onClick: () => void;
}

const StorageItem = ({isSelected, onClick}: StorageItemProps) => {
     return(
          <div 
               className={cn(SFDisplayFont.variable, "font-SFProDisplay w-full h-fit border-2 rounded-[10px] p-[14px]", isSelected ? 'border-[#0071E3]' : 'border-[#ccc]')} 
               onClick={onClick}
          >
               <div className='w-full flex flex-row items-center'>
                    <div className='basis-2/3'>
                         <span className='text-[17px] font-semibold leading-[21px] tracking-[0.3px]'>128</span>
                         <span className='text-[14px] font-semibold leading-[21px] tracking-[0.3px]'>GB</span>
                    </div>
                    <div className='basis-1/3 text-end text-[12px] font-light leading-[16px] tracking-[0.4px]'>
                         <span>From $999</span>
                         <br/>
                         <span>or $41.62/mo</span>
                         <br/>
                         <span>for 24 mo.</span>
                    </div>
               </div>
          </div>
     )
}

export default StorageItem;
