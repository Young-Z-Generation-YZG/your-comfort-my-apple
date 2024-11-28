/* eslint-disable react/react-in-jsx-scope */
'use client';
import Image from 'next/image';
import { Button } from '~/components/ui/button';
import { FaRegTrashAlt } from 'react-icons/fa';
import images from '~/components/client/images';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import { Input } from '~/components/ui/input';


const LineOrder = ({image}:{image:string}) => {
     return (
          <div className={cn(SFDisplayFont.variable, "font-SFProDisplay w-full flex flex-col py-6 border-b border-[#ccc] ")}>
               <div className='w-full flex flex-row'>
                    <div className='w-[70px] h-fit'>
                         <Image 
                         src={image} 
                         alt='ip16' 
                         width={1000} 
                         height={1000} 
                         quality={100}
                         className='w-auto h-[62px] mx-auto my-1'/>
                    </div>
                    <div className='flex-1 flex flex-col justify-start items-start ml-2 text-[13px] tracking-[0.03px]'>
                         <div className='font-semibold '>
                              Galaxy S24 Ultra (Online Exclusive)
                         </div>
                         <div className='font-light'>
                              <span>Galaxy S24 Ultra, </span>
                              <span>Blue Titan, </span>
                              <span>512 GB</span>
                         </div>
                         <div className='font-semibold'>
                              $1,715.92
                         </div>
                    </div>
               </div>
          </div>
     );
}
export default LineOrder;

