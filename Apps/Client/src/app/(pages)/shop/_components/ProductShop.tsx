/* eslint-disable react/react-in-jsx-scope */
'use client';
import Image from 'next/image';
import Link from 'next/link';
import { useState } from 'react';
import images from '~/components/client/images';
import { Button } from '~/components/ui/button';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { cn } from '~/infrastructure/lib/utils';


const colours = [
     {
          hashCodeColor: '#2596be',
          name:'blue',
     },
     {
          hashCodeColor: '#e28743',
          name:'orange',
     },
     {
          hashCodeColor: '#6f400a',
          name:'brown',
     },
     {
          hashCodeColor: '#000',
          name:'black',
     }
]

const storages = ['64GB', '128GB', '256GB', '512GB', '1TB']

const ProductShop = () => {
     // check colour
     const [selectedColour, setSelectedColour] = useState(colours[0]);
     const handleColour = (index: number) => {
          setSelectedColour(colours[index]);
     }

     // check colour
     const [selectedStorage, setSelectedStorage] = useState(storages[0]);
     const handleStorage= (index: number) => {
          setSelectedStorage(storages[index]);
     }

     // stars html
     const renderStars = (rating: number) => {
          return Array(5).fill(0).map((_, index) => {
               const fillPercentage = Math.min(Math.max((rating - index) * 100, 0), 100);
               return (
                    <svg 
                    key={index} 
                    xmlns="http://www.w3.org/2000/svg" 
                    xmlnsXlink="http://www.w3.org/1999/xlink" 
                    width="16" height="16" viewBox="0 0 16 16" className="mdl-js">
                         <defs>
                              <linearGradient id={`star-gradient-${index}`}>
                                   <stop offset={`${fillPercentage}%`} stopColor="#FFAA4E"/>
                                   <stop offset={`${fillPercentage}%`} stopColor="#D9D9D9"/>
                              </linearGradient>
                         </defs>
                         <path 
                              fill={`url(#star-gradient-${index})`} 
                              d="M7.322 1.038c.255-.622 1.066-.633 1.341-.034l.015.034 1.773 4.316 4.685.341c.662.048.926.796.468 
                              1.245l-.025.023-.026.023-3.585 3.008 1.12 4.523c.16.644-.468 1.127-1.037.832l-.03-.015-.028-.017L8 
                              12.857l-3.993 2.46c-.564.348-1.217-.103-1.109-.735l.006-.032.008-.033 1.12-4.523L.446 6.986C-.063 
                              6.56.162 5.8.795 5.703l.034-.005.035-.003 4.685-.34 1.773-4.317z" 
                              transform="translate(-116 -202) translate(37 186) translate(39 14) translate(0 2) translate(40)"
                         />
                         <path 
                              stroke="#000" 
                              strokeWidth="0.5" 
                              fill="none" 
                              d="M7.322 1.038c.255-.622 1.066-.633 1.341-.034l.015.034 1.773 4.316 4.685.341c.662.048.926.796.468 
                              1.245l-.025.023-.026.023-3.585 3.008 1.12 4.523c.16.644-.468 1.127-1.037.832l-.03-.015-.028-.017L8 
                              12.857l-3.993 2.46c-.564.348-1.217-.103-1.109-.735l.006-.032.008-.033 1.12-4.523L.446 6.986C-.063 
                              6.56.162 5.8.795 5.703l.034-.005.035-.003 4.685-.34 1.773-4.317z" 
                              transform="translate(-116 -202) translate(37 186) translate(39 14) translate(0 2) translate(40)"
                         />
                    </svg>
               );
          });
     };

     return (
          <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay w-full h-[412px] bg-white rounded-[20px] py-6 pl-6')}>
               <div className='w-full h-full flex flex-row items-center'>
                    <div className='basis-[25.41%] h-full flex flex-col'>
                         <div className='w-full flex-1 flex justify-center items-end'>
                              <Image src={images.ipXSMax} 
                              alt='product'
                              className='w-[180px] h-auto' 
                              width={1000} 
                              height={1000} 
                              quality={100}/>
                         </div>
                         <div className='w-full flex justify-center mt-7'>
                              <Button className='h-fit px-4 py-[6px] border border-[#000] rounded-full text-[12px] font-medium 
                              hover:bg-black hover:text-white bg-white text-black '>
                                   Quick Look
                              </Button>
                         </div>
                    </div>
                    <div className='flex-1 h-full flex flex-row'>
                         <div className='basis-[55%] pt-9 px-6 font-medium border-r border-[#ccc] relative'>

                              {/* NEW FEATURE */}
                              <div className='px-2 py-[2px] absolute top-[4px] left-[24px] z-10 bg-[#2188fe] rounded-full 
                              text-[12px] font-medium text-white'>
                                   New
                              </div>

                              {/* PRODUCT NAME */}
                              <div className='w-full text-[20px]'>Iphone 13 Pro Max</div>

                              {/* PRODUCT COLOR */}
                              <div className='w-full mt-1 flex flex-col gap-2'>
                                   <div className='w-full text-[16px]'>
                                        <span className='font-normal'>Colour: </span>
                                        <span className='font-light'>{selectedColour.name}</span> 
                                   </div>
                                   <div className='flex flex-row gap-2'>
                                        {colours.map((colour,index) => {
                                             return (
                                                  selectedColour.hashCodeColor === colour.hashCodeColor ?
                                                  <div 
                                                  key={index}
                                                  className='w-[15px] h-[15px] rounded-full border border-[#5c5c5c] 
                                                  ring-offset-2 ring-1 ring-[#000]' 
                                                  onClick={()=>handleColour(index)}
                                                  style={{backgroundColor: `${colour.hashCodeColor}`}}/> 
                                                  :
                                                  <div 
                                                  key={index}
                                                  className='w-[15px] h-[15px] rounded-full border border-[#5c5c5c] 
                                                  hover:ring-offset-2 hover:ring-1 hover:ring-[#888]' 
                                                  onClick={()=>handleColour(index)}
                                                  style={{backgroundColor: `${colour.hashCodeColor}`}}/> 
                                             )
                                        })}
                                   </div>
                              </div>

                              {/* PRODUCT STORAGE */}
                              <div className='w-full mt-1 flex flex-col gap-2'>
                                   <div className='w-full text-[16px]'>
                                        <span className='font-normal'>Storage: </span>
                                   </div>
                                   <div className='flex flex-row gap-2'>
                                        {storages.map((storage,index) => {
                                             return (
                                                  selectedStorage === storage ?
                                                  <Button
                                                  key={index}
                                                  onClick={()=>handleStorage(index)}
                                                  className='w-14 h-fit px-2 py-0 border border-[#000] rounded-full text-[13px] font-light bg-black text-white hover:bg-black hover:text-white'>
                                                       {storage}
                                                  </Button>
                                                  :
                                                  <Button 
                                                  key={index}
                                                  onClick={()=>handleStorage(index)}
                                                  className='w-14 h-fit px-2 py-0 border border-[#000] rounded-full text-[13px] font-light bg-white text-black hover:bg-black hover:text-white'>
                                                       {storage}
                                                  </Button>
                                             )
                                        })}
                                   </div>
                              </div>

                              {/* PRODUCT RATING */}
                              <div className='w-full border-t border-[#eee] pt-4 mt-4 flex flex-row justify-start items-center gap-2 text-[14px]'>
                                   <div className='flex flex-row gap-[2px]'>
                                        {renderStars(4.6)}
                                   </div>
                                   <div>4.6 (232)</div>
                              </div>

                              {/* PRODUCT DESCRIPTION */}
                              <div className='w-full text-[12px] font-light flex justify-start items-center gap-2 mt-2 pl-3'>
                                   <ul className='flex flex-col list-disc pl-3'>
                                        <li>The most efficient way to search on a smartphone</li>
                                        <li>The thinnest, lightest design</li>
                                        <li>The most powerful processor on Samsung Galaxy foldables</li>
                                   </ul>
                              </div>
                         </div>
                         <div className='basis-[45%] px-6 flex flex-col justify-between'>
                              <div className='w-full text-[12px] font-light mt-10 pl-3'>
                                   <ul className='flex flex-col list-decimal pl-3'>
                                        <li>
                                             <span>Weight: </span>
                                             <span className='font-semibold'>239g</span>
                                        </li>
                                        <li>
                                             <span>CPU Speed: </span>
                                             <span className='font-semibold'>3.39GHz, 3.1GHz, 2.9GHz, 2.2GHz</span>
                                        </li>
                                   </ul>
                              </div>
                              <Button asChild className='h-fit px-4 py-2 rounded-full text-[16px] font-medium text-white bg-[#000] hover:bg-[#656565]'>
                                   <Link href="/detail-product">Buy now</Link>
                              </Button>
                         </div>
                    </div>
               </div>
          </div>
     )
}

export default ProductShop;
