/* eslint-disable react/react-in-jsx-scope */
'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import images from '~/components/client/images';
import Image from 'next/image';
import { FaUserAlt } from "react-icons/fa";


const ItemReview = () => {
     const starsRatingNew = (rating: number) => {
          return Array(5).fill(0).map((_, index) => {
               const fillPercentage = Math.min(Math.max((rating - index) * 100, 0), 100);
               return (
                    <svg 
                    xmlns="http://www.w3.org/2000/svg" 
                    xmlnsXlink="http://www.w3.org/1999/xlink" 
                    width="16" height="16" viewBox="0 0 16 16" className="mdl-js">
                         <defs>
                                   <linearGradient id={`star-gradient`}>
                                        <stop offset={`${fillPercentage}%`} stopColor="#FFAA4E"/>
                                        <stop offset={`${fillPercentage}%`} stopColor="#D9D9D9"/>
                                   </linearGradient>
                         </defs>
                         <path 
                                   fill={`url(#star-gradient)`} 
                                   d="M7.322 1.038c.255-.622 1.066-.633 1.341-.034l.015.034 1.773 4.316 4.685.341c.662.048.926.796.468 
                                   1.245l-.025.023-.026.023-3.585 3.008 1.12 4.523c.16.644-.468 1.127-1.037.832l-.03-.015-.028-.017L8 
                                   12.857l-3.993 2.46c-.564.348-1.217-.103-1.109-.735l.006-.032.008-.033 1.12-4.523L.446 6.986C-.063 
                                   6.56.162 5.8.795 5.703l.034-.005.035-.003 4.685-.34 1.773-4.317z" 
                                   transform="translate(-116 -202) translate(37 186) translate(39 14) translate(0 2) translate(40)"
                         />
                    </svg>
               );
          })
       };


     return (
          <div className={cn(SFDisplayFont.variable, "font-SFProDisplay w-full h-fit pt-[10px] pb-[20px] flex flex-row text-[13px]")} >
               <div className={cn('basis-1/5 h-[120px] pt-1 border-r border-[#ccc] flex flex-row')}>
                    <FaUserAlt className={cn('h-16 w-16')}/>
                    <div className={cn('flex-1 pl-2')}>
                         <div className={cn('flex flex-row mb-1')}>
                              {starsRatingNew(2)}
                         </div>
                         <p className={cn('font-medium')}>
                              Ellenvs
                         </p>
                         <p className={cn('font-thin')}>
                              29/10/2024 13:10
                         </p>
                    </div>
               </div>
               <div className={cn('basis-4/5 h-[120px] flex flex-col')}>
                    <div className={cn('pl-2 text-[16px] font-semibold')}>
                         Lorem a aliquam officia eaque dolor sed pariatur minus. 
                    </div>
                    <div className={cn('pl-2 font-normal')}>
                         Lorem, adipisicing elit. Nesciunt, quas magni. Obcaecati earum fugit, blanditiis voluptates a aliquam officia eaque dolor sed pariatur minus. Lorem ipsum dolor sit amet consectetur, adipisicing elit. Nesciunt, quas magni. Obcaecati ullam voluptas, tenetur esse, nulla molestias earum fugit, blanditiis voluptates a aliquam officia eaque dolor sed pariatur minus. Lorem ipsum dolor sit amet consectetur, adipisicing elit. Nesciunt, quas magni. Obcaecati ullam voluptas, tenetur esse, nulla molestias earum fugit, blanditiis voluptates a aliquam officia eaque dolor sed pariatur minus. 
                    </div>
               </div>


                    {/* <div className={cn('h-full flex flex-row gap-3')}>
                         <FaUserAlt className={cn('h-full w-auto')}/>
                         <div className={cn('h-full flex flex-col justify-between')}>
                              <p>Ellenvs</p>
                              <p>4 hours ago</p>
                         </div>
                    </div>
                    <div className={cn('h-full flex flex-col gap-3')}>
                         <p>Snel, scherp, 1 hand te bedienen</p>
                         <div className='flex flex-row justify-end'>
                              {starsRating(4)}
                         </div>

                    </div> */}
          </div>
     )
}

export default ItemReview;