/* eslint-disable react/react-in-jsx-scope */
'use client';
import Image from 'next/image';
import { Button } from '~/components/ui/button';
import { FaRegTrashAlt } from 'react-icons/fa';
import images from '~/components/client/images';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import { Input } from '~/components/ui/input';
import { useState } from 'react';


const LineItem = () => {
     const [quantity, setQuantity] = useState(1);
     const handleQuantityUp = () => {
          if (Number(quantity))
               setQuantity(Number(quantity) + 1);
     };
     const handleQuantityDown = () => {
          if (Number(quantity)>0)
               setQuantity(Number(quantity) - 1);
     };

     return (
          <div className={cn(SFDisplayFont.variable, "font-SFProDisplay line-cart w-full flex flex-col py-6 border-b border-[#ccc] ")}>
               <div className='product-item w-full flex flex-row justify-start'>
                    <div className='w-[200px] h-[170px]'>
                         <Image 
                         src={images.ipXSMax } 
                         alt='ip16' 
                         width={1000} 
                         height={1000} 
                         quality={100}
                         className='w-auto h-full mx-auto'/>
                    </div>
                    <div className='flex-1 flex flex-col justify-start items-start pl-[16px]'>
                         <div className='w-full h-[60px] flex flex-row'>
                              <div className='flex-1 h-full text-[22px] font-bold'>
                                   Galaxy S24 Ultra (Online Exclusive)
                              </div>
                              <div className='h-full flex flex-col text-[18px] font-normal text-end'>
                                   {/* <div className='w-full font-bold'>$1,715.92</div> */}
                                   <div className='w-full text-[#0070F0] font-bold'>$1,715.92</div>
                                   <div className='w-full line-through '>$1,715.92</div>
                              </div>
                         </div>
                         <div className='w-full h-[60px] flex flex-row'>
                              <div className='w-full h-full'>
                                   <div className='text-[15px] font-light tracking-[0.2px]'>
                                        <span>Galaxy S24 Ultra, </span>
                                        <span>Blue Titan, </span>
                                        <span>512 GB</span>
                                   </div>
                                   <div className='text-[15px] font-light tracking-[0.2px] pt-2'>In stock</div>
                              </div>
                              <FaRegTrashAlt className='w-4 h-4 mt-3'/>
                         </div>
                         <div className='w-full flex flex-row justify-end'>
                              <Button onClick={handleQuantityDown} className='w-10 h-8 border border-[#ebebeb] rounded-l-full'>-</Button>
                              <Input type='text' className='w-12 h-8 border-x-0 border-[#ebebeb] text-center rounded-none' value={quantity}/>
                              <Button onClick={handleQuantityUp} className='w-10 h-8 border border-[#ebebeb] rounded-r-full'>+</Button>
                         </div>
                    </div>
               </div>
          </div>
     );
}
export default LineItem;

