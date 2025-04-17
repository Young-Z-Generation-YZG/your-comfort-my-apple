import { Badge } from '@components/ui/badge';
import RatingStar from '@components/ui/rating-star';
import { Sparkles } from 'lucide-react';
import Image from 'next/image';
import { Fragment } from 'react';
import { cn } from '~/src/infrastructure/lib/utils';

const IphoneModelItem = () => {
   return (
      <Fragment>
         <div className="flex flex-row bg-white px-5 py-5 rounded-md">
            <div className="image w-full">
               <Image
                  src={`https://res.cloudinary.com/delkyrtji/image/upload/v1744120615/pngimg.com_-_iphone16_PNG37_meffth.png`}
                  alt="iPhone Model"
                  width={1000}
                  height={1000}
                  className="w-[200px] h-[250px] object-cover rounded-md"
               />
            </div>

            <div className="content flex flex-col gap-2">
               <h2 className="font-SFProText font-normal text-xl">
                  iPhone 16 Pro Max
               </h2>

               <span className="flex flex-row gap-2">
                  <p className="first-letter:uppercase text-sm">colors:</p>
                  <p className="first-letter:uppercase text-sm">ultramarine</p>
               </span>

               <div className="flex flex-row gap-2">
                  <div
                     className={cn(
                        'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                     )}
                     style={{ backgroundColor: '#9AADF6' }}
                  />
                  <div
                     className={cn(
                        'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                     )}
                     style={{ backgroundColor: '#B0D4D2' }}
                  />
                  <div
                     className={cn(
                        'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                     )}
                     style={{ backgroundColor: '#F2ADDA' }}
                  />
                  <div
                     className={cn(
                        'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                     )}
                     style={{ backgroundColor: '#FAFAFA' }}
                  />
                  <div
                     className={cn(
                        'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                     )}
                     style={{ backgroundColor: '#3C4042' }}
                  />
               </div>

               <span className="flex flex-row gap-2 mt-2">
                  <p className="first-letter:uppercase text-sm">Storage:</p>
               </span>

               <div className="flex flex-row gap-2">
                  <span
                     className={cn(
                        'uppercase select-none text-xs font-medium min-w-[70px] py-1 border rounded-full text-center',
                     )}
                  >
                     128gb
                  </span>
                  <span
                     className={cn(
                        'uppercase select-none text-xs font-medium min-w-[70px] py-1 border rounded-full text-center',
                     )}
                  >
                     256gb
                  </span>
                  <span
                     className={cn(
                        'uppercase select-none text-xs font-medium min-w-[70px] py-1 border rounded-full text-center',
                     )}
                  >
                     512gb
                  </span>
                  <span
                     className={cn(
                        'uppercase select-none text-xs font-medium min-w-[70px] py-1 border rounded-full text-center',
                     )}
                  >
                     1tb
                  </span>
               </div>

               <Badge
                  className="w-fit bg-[#23C55E] text-white mt-2 rounded-full select-none"
                  variant="outline"
               >
                  In Stock
               </Badge>

               <p className="text-muted-foreground text-sm font-medium mt-2">
                  100 units available • 0 sold
               </p>

               <span className="gap-2 mt-2 text-right">
                  <p className="text-lg inline-block font-bold text-[#E11C48] mr-2">
                     $1079.10
                  </p>
                  <p className="text-sm inline-block font-normal line-through">
                     $1199.00
                  </p>
               </span>
            </div>

            {/* Promotion */}
            <div className="border-l border-mute py-2 px-5 w-full ml-5">
               <div className="relative">
                  <div className="absolute -top-1 -right-1 w-3 h-3 bg-rose-500 rounded-full animate-pulse"></div>
                  <div className="text-lg font-semibold text-rose-600">
                     Promotion
                  </div>
               </div>
               <div className="mt-4 relative overflow-hidden">
                  <div className="absolute -right-12 -top-4 w-24 h-24 bg-gradient-to-br from-amber-300 to-rose-500 rotate-45 opacity-20"></div>
                  <div className="relative z-10 p-3 bg-gradient-to-r from-amber-50 to-rose-50 rounded-md border border-amber-200">
                     <div className="flex items-center gap-2">
                        <Sparkles className="h-5 w-5 text-amber-500" />
                        <span className="font-medium text-amber-800">
                           Summer Sale 2024
                        </span>
                     </div>
                     <div className="mt-1 flex items-center gap-2">
                        <div className="px-2 py-0.5 bg-rose-100 rounded text-sm font-bold text-rose-700">
                           10% OFF
                        </div>
                        <span className="text-xs text-gray-500">
                           Ends{' '}
                           {new Date(
                              '2025-04-30T00:00:00Z',
                           ).toLocaleDateString()}
                        </span>
                     </div>
                  </div>
               </div>
            </div>

            <div className="border-l border-mute py-2 px-5">
               <div className="flex flex-col items-center gap-2 px-10 py-5 border border-dashed border-slate-400 rounded-lg bg-[#F9F9F9]">
                  <RatingStar rating={4.6} size="lg" />
                  <p className="font-medium text-base">
                     <span className="text-primary">4.6</span> out of 5 stars
                  </p>
                  <p className="text-sm font-semibold">100 reviews</p>

                  {Array(5)
                     .fill(0)
                     .map((_, index) => {
                        const fillPercentage = Math.min(
                           Math.max((5 - index) * 100, 0),
                           100,
                        );
                        return (
                           <div
                              key={index}
                              className="flex flex-row gap-2 items-center"
                           >
                              <span className="w-2 text-muted-foreground text-sm font-medium">
                                 {5 - index}
                              </span>

                              <span className="mr-2">
                                 <svg
                                    key={index}
                                    xmlns="http://www.w3.org/2000/svg"
                                    xmlnsXlink="http://www.w3.org/1999/xlink"
                                    width={16}
                                    height={16}
                                    viewBox="0 0 16 16"
                                    className="mdl-js"
                                 >
                                    <defs>
                                       <linearGradient
                                          id={`star-gradient-${5}`}
                                       >
                                          <stop
                                             offset={`${fillPercentage}%`}
                                             stopColor="#FFAA4E"
                                          />
                                          <stop
                                             offset={`${fillPercentage}%`}
                                             stopColor="#D9D9D9"
                                          />
                                       </linearGradient>
                                    </defs>
                                    <path
                                       fill={`url(#star-gradient-${5})`}
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
                              </span>

                              <RatingStar
                                 rating={5}
                                 size="sm"
                                 className="text-muted-foreground"
                              />

                              <p className="text-xs text-primary">(50)</p>
                           </div>
                        );
                     })}
               </div>
            </div>
         </div>
      </Fragment>
   );
};

export default IphoneModelItem;
