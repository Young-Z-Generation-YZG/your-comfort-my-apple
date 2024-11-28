/* eslint-disable react/react-in-jsx-scope */
'use client';
import { useEffect, useState } from 'react';
import { useGetUsersQuery } from '~/services/example/user.service';
import '~/styles/globals.css';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import { FaFilter } from "react-icons/fa6";
import { Button } from '~/components/ui/button';
import { IoChatboxEllipsesOutline } from "react-icons/io5";
import { GoMultiSelect } from "react-icons/go";
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
 } from "~/components/ui/select"
import ProductShop from '~/app/(pages)/shop/ProductShop';
import {
   Accordion,
   AccordionContent,
   AccordionItem,
   AccordionTrigger,
} from "~/components/ui/accordion"
import { Slider } from "~/components/ui/slider"
import * as SliderPrimitive from '@radix-ui/react-slider';
import { DualRangeSlider } from '~/components/ui/dualRangeSlider';



const IphonePage = () => {
   const { data } = useGetUsersQuery('');

   useEffect(() => {
      console.log(data);
   }, [data]);

   const storage = ['64 GB', '128 GB', '256 GB', '512 GB', '1 TB'];
   // const color = ['Green', 'Red', 'Blue', 'Yellow', 'Purple', 'Black', 'White', 'Gray'];
   // const [valueStorage, setValueStorage] = useState('');
   const [values, setValues] = useState([0, 1000]);

   return (
      <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay w-full flex flex-col items-start justify-center bg-white')}>
         
         {/* FILTERS INFO */}
         <div className='w-full border-y border-[#ccc] text-[15px] font-semibold'>
            <div className='h-[4.514vw] w-full max-w-[1440px] mx-auto px-5 flex flex-row justify-start items-center'>
               <div className='flex flex-row mr-auto'>
                  <div className='text-[#218bff] flex flex-row items-center gap-2'>
                     <FaFilter /> 
                     <div>Filters</div>
                  </div>
                  <div className='mx-3 px-[18px] border-x-[1px] border-[#ccc] flex flex-row items-center'>
                     <div>10 Results</div>
                  </div>
                  <div className='flex flex-row items-center '>
                     <Button className='h-[22.5px] p-0 text-[15px] font-semibold border-b-2 border-[#000] rounded-none'>
                        Clear Filters
                     </Button>
                  </div>
               </div>
               <div className='flex flex-row gap-[50px]'>
                  <div className='flex flex-row items-center gap-1'>
                     <IoChatboxEllipsesOutline />
                     <div>Chat with an expert</div>
                  </div>
                  <Select>
                     <SelectTrigger className="w-fit flex items-center justify-center border-none">
                        <GoMultiSelect className='mr-2'/>
                        <SelectValue placeholder="Recommended" />
                     </SelectTrigger>
                     <SelectContent className='bg-[#f7f7f7]'>
                        <SelectGroup>
                           <SelectItem value="newest">Newest</SelectItem>
                           <SelectItem value="most-clicked">Most Clicked</SelectItem>
                           <SelectItem value="highest-rated">Highest Rated</SelectItem>
                           <SelectItem value="recommended">Recommended</SelectItem>
                           <SelectItem value="price-high-low">Price: High to Low</SelectItem>
                           <SelectItem value="price-low-high">Price: Low to High</SelectItem>
                           <SelectItem value="online-availability">Online Availability</SelectItem>
                           <SelectItem value="most-reviewed">Most Reviewed</SelectItem>
                        </SelectGroup>
                     </SelectContent>
                  </Select>
               </div>
               
            </div>
         </div>

         {/* SHOP INFO */}
         <div className='w-full h-fit flex flex-row justify-center items-start'>
            
            {/* SIDEBAR */}
            <div className='w-[354px] h-[4336px] relative'>
               <div className='w-full h-fit sticky top-0'>
                  <Accordion type="multiple" className="w-full h-full p-6">

                     {/* STORAGE FILTER */}
                     <AccordionItem value="item-1">
                        <AccordionTrigger className='hover:no-underline text-[15px] font-semibold pb-0 uppercase '>Storage</AccordionTrigger>
                        <AccordionContent className="pt-4">
                           <div className='grid grid-cols-3 gap-2'>
                              {storage.map((item, index)=>{
                                 return (
                                    <Button key={index} className='h-fit py-1 rounded-full text-[12px] font-normal 
                                    border border-[#000] opacity-50 hover:opacity-100'>
                                       {item}
                                    </Button>
                                 )
                              })}
                           </div>
                        </AccordionContent>
                     </AccordionItem>

                     {/* PRICE FILTER */}
                     <AccordionItem value="item-2">
                        <AccordionTrigger className='hover:no-underline text-[15px] font-semibold pb-0 uppercase '>Price</AccordionTrigger>
                        <AccordionContent className="pt-4">
                           {/* <div className='flex flex-col gap-2'>
                              {Array.from({ length: 4 }).map((_, index)=>{
                                 return (
                                    <Button key={index} className='h-fit py-1 rounded-full text-[12px] font-normal 
                                    border border-[#000] opacity-50 hover:opacity-100'>
                                       $0-$100
                                    </Button>
                                 )
                              })}
                           </div> */}
                           <div className={cn('w-full flex flex-col justify-center')}>
                              <div className={cn('w-full flex flex-row justify-between')}>
                                 <div>From: {values[0]}$</div>
                                 <div>To: {values[1]}$</div>
                              </div>
                              <DualRangeSlider
                                 className={cn('mt-3 bg-gray-200 rounded-lg')}
                                 value={values}
                                 onValueChange={setValues}
                                 min={0}
                                 max={1000}
                                 step={100}
                                 />
                           </div>
                        </AccordionContent>
                     </AccordionItem>

                     {/* CAMERA FILTER */}
                     <AccordionItem value="item-3">
                        <AccordionTrigger className='hover:no-underline text-[15px] font-semibold pb-0 uppercase '>Camera</AccordionTrigger>
                        <AccordionContent className="pt-4">
                           <div className='grid grid-cols-3 gap-2'>
                              {Array.from({ length: 4 }).map((_, index)=>{
                                 return (
                                    <Button key={index} className='h-fit py-1 rounded-full text-[12px] font-normal 
                                    border border-[#000] opacity-50 hover:opacity-100'>
                                       12 MP
                                    </Button>
                                 )
                              })}
                           </div>
                        </AccordionContent>
                     </AccordionItem>
                  </Accordion>
               </div>
            </div>
            
            {/* PRODUCTS */}
            <div className='w-[1086px] h-fit p-6 flex flex-col gap-6 bg-[#f7f7f7]'>
               <ProductShop/>
               <ProductShop/>
               <ProductShop/>
               <ProductShop/>
               <ProductShop/>
               <ProductShop/>
               <ProductShop/>
               <ProductShop/>
               <ProductShop/>
               <ProductShop/>
            </div>

         </div>
      </div>
   );
};

export default IphonePage;

