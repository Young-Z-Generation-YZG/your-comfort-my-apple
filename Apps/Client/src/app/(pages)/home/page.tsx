/* eslint-disable react/react-in-jsx-scope */
'use client';

import { useEffect } from 'react';
import { useGetUsersQuery } from '~/services/example/user.service';
import { motion } from "framer-motion"
import '~/styles/globals.css';
import Image from 'next/image';
import { Input } from '~/components/ui/input';
import { Button } from '~/components/ui/button';

const HomePage = () => {
   const { data } = useGetUsersQuery('');

   useEffect(() => {
      console.log(data);
   }, [data]);

   return (
      <div className='w-full flex flex-col items-center justify-center'>
         <div className='hero-container flex flex-row max-w-[1264px] h-[800px] px-8 mb-[100px] mx-auto '>
            {/* <div className='hero-gradient bg-gray-300 h-[100px] w-full absolute top-0 bottom-0 left-0 right-0 z-0'></div> */}
            <div className='hero-content basis-1/2 h-fit bg-transparent relative z-10 flex flex-col items-start gap-[30px]'>
               <div className='content-header max-w-[600px] mt-8 text-[94px] font-medium leading-[97px] tracking-[-3.76px] text-[#3a3a3a] opacity-30'>Financial infrastructure to grow your revenue</div>
               <div className='content-body max-w-[540px] pl-2 pr-10 text-[18px] font-normal leading-[28px] tracking-[0.2px] text-[#425466]'>
                     Join the millions of companies of all sizes that use Stripe to accept   
                     payments online and in person, embed financial services, power custom 
                     revenue models, and build a more profitable business.
               </div>
               <div className='content-form'>
                  <div className='flex flex-row gap-2 border border-black rounded-[10px]'>
                     <Input type='text' placeholder='Email address' className='rounded-[10px]'/>
                     <Button variant="outline" className='bg-black text-white rounded-[10px]'>Start now</Button> 
                  </div>
               </div>
            </div>
            <div className='hero-masonry basis-1/2 h-full bg-transparent relative z-10'> 
               <div className='bg-transparent h-full w-full absolute z-10'>
                  <div className='bg-[linear-gradient(0deg,rgba(1,0,2,0),#fff)] h-20 w-full absolute top-0'></div>
                  <div className='bg-[linear-gradient(180deg,rgba(1,0,2,0),#fff)] h-20 w-full absolute bottom-0'></div>
               </div>
               <div className="overflow-hidden h-full relative z-0 px-1">
                  <motion.div 
                     animate={{ y: ["0px", "-3384px"] }}
                     // animate={{ y: ["-3384px", "-3384px"] }}
                     // animate={{ y: ["0px", "0px"] }}
                     transition={{
                        y: {
                           duration: 20,
                           repeat: Infinity,
                           ease: "linear"
                        }
                     }}
                     className='preview-masonry bg-transparent max-w-[600px] mx-auto'>
                     <div className='grid grid-cols-[repeat(2,minmax(100px,285px))] grid-rows-[masonry] grid-flow-dense gap-[20px]'>
                        <div className="bg-[url('/images/hero-imgs/picture01-half.jpg')] bg-cover bg-no-repeat bg-bottom rounded-[0_0_20px_20px] h-[168px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture02.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture03.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture04.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture05.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture06.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture07.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture08.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture09.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture10.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture11.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture12.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture13.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture14.jpg')] bg-cover bg-no-repeat bg-top row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture15.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture16.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture17.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture18.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture01.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture02.jpg')] bg-cover bg-no-repeat bg-bottom row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture03.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture04.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture05.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                        <div className="bg-[url('/images/hero-imgs/picture06.jpg')] bg-cover bg-no-repeat bg-center row-end-[span_2] rounded-[20px] h-[356px]"></div>
                     </div>
                  </motion.div>
               </div>
            </div>
         </div>
         
         <div className='preview-container w-full bg-[#161616] mb-[32px]'>
            <div className='preview-masonry bg-transparent py-[100px] max-w-[996px] mx-auto'> 
               <div className='grid grid-cols-[repeat(6,minmax(140px,1fr))] grid-rows-[masonry] grid-flow-dense gap-[20px]'>
                  <div className='bg-black col-end-[span_4] flex flex-row rounded-[20px]' style={{blockSize:"400px"}}>
                     <div className='track-img w-full basis-3/5 '>
                        <Image 
                           className='h-full w-fit mx-auto pt-10' src='/images/preview-imgs/picture01.png' 
                           alt="Picture of the author" 
                           width={1000} 
                           height={1000} 
                           quality={100}/>
                     </div>
                     <div className='track-content w-full basis-2/5 text-white flex flex-col justify-center pl-[20px] pr-[36px]'>
                        <div className='text-xl text-[#cbcbcb] font-[500] pb-1'>Dynamic Island</div>
                        <div className='text-[28px] text-[#cbcbcb] font-[600] leading-[30px]'>A magical way to interact with iPhone.</div>
                     </div>
                  </div>

                  <div className='bg-black col-end-[span_2] flex flex-col rounded-[20px] justify-center items-center' style={{blockSize:"400px"}}>
                     <div className='track-content w-full basis-2/5 text-white pt-[30px]'>
                        <div className='text-[25px] text-[#cbcbcb] font-[600] leading-[30px] text-center'>48MP Main camera.</div>
                        <div className='text-[25px] text-[#cbcbcb] font-[600] leading-[30px] text-center'>Mind-blowing detail.</div>
                     </div>
                     <div className='track-img w-full basis-3/5'>
                        <Image 
                           className='h-[290px] w-fit mx-auto' src='/images/preview-imgs/picture02.png' 
                           alt="Picture of the author" 
                           width={1000} 
                           height={1000} 
                           quality={100}/>
                     </div>
                  </div>

                  <div className='bg-black col-end-[span_3] flex flex-col rounded-[20px]' style={{blockSize:"400px"}}>
                     <div className='track-content w-full basis-2/5 text-white flex flex-col justify-center items-start'>
                        <div className='text-[32px] text-[#cbcbcb] font-[600] leading-[35px] pl-[40px]'>The megafast chip  </div>
                        <div className='text-[32px] text-[#cbcbcb] font-[600] leading-[35px] pl-[40px]'>that makes it all possible.</div>
                     </div>
                     <div className='track-img w-full basis-3/5 flex justify-end items-end '>
                        <Image  
                           className='h-fit w-[450px]' src='/images/preview-imgs/picture03.png' 
                           alt="Picture of the author" 
                           width={1000} 
                           height={1000} 
                           quality={100}/>
                     </div>
                  </div>

                  <div className='bg-black col-end-[span_3] flex flex-row rounded-[20px]' style={{blockSize:"400px"}}>
                     <div className='track-content w-full text-white flex flex-col justify-center items-start'>
                        <div className='text-[28px] leading-[32px] font-[600] text-[#f9afff] pl-[67px]'>A battery that's</div>
                        <div className='text-[100px] leading-[100px] tracking-tight font-[600] text-[#de8cff] pl-[67px]'>all in,</div>
                        <div className='text-[100px] leading-[100px] tracking-tight font-[600] text-[#bf5cff] pl-[131px]'>all day.</div>
                     </div>
                  </div>

                  <div className="bg-[url('/images/preview-imgs/picture04.png')] bg-cover bg-no-repeat bg-center col-end-[span_4] flex flex-row rounded-[20px]" style={{blockSize:"400px"}}>
                     <div className='track-content w-full text-white flex flex-col justify-end items-center pb-4'>
                        <div className='text-[22px] text-[#cbcbcb] font-[600] leading-[35px]'>Cinematic mode</div>
                        <div className='text-[28px] text-[#cbcbcb] font-[600] leading-[35px]'>Film like a Pro in 4K HDR at 24 fps.</div>
                     </div>
                  </div>

                  <div className='bg-black col-end-[span_2] flex flex-col rounded-[20px]' style={{blockSize:"400px"}}>
                     <div className='track-content w-full basis-2/5 text-white pt-[28px]'>
                        <div className='text-[24px] text-[#cbcbcb] font-[600] leading-[28px] text-center'>Always-On display.</div>
                        <div className='text-[24px] text-[#cbcbcb] font-[600] leading-[28px] px-10 text-center'>A subtle way to stay in the know.</div>
                     </div>
                     <div className='track-img w-full basis-3/5 '>
                        <Image 
                           className='h-[270px] w-fit mx-auto' src='/images/preview-imgs/picture05.png' 
                           alt="Picture of the author" 
                           width={1000} 
                           height={1000} 
                           quality={100}/>
                     </div>
                  </div>

                  <div className="bg-[url('/images/preview-imgs/picture06.png')] bg-cover bg-no-repeat bg-center col-end-[span_2] flex flex-row rounded-[20px]" style={{blockSize:"400px"}}>
                     <div className='track-content w-full text-white flex flex-col justify-end items-center pb-[100px]'>
                        <div className='text-[24px] text-[#cbcbcb] font-[600] leading-[28px]'>(Phew.)</div>
                     </div>
                  </div>

                  <div className='bg-black col-end-[span_2] flex flex-col rounded-[20px]' style={{blockSize:"400px"}}>
                     <div className='track-content w-full basis-2/5 text-white flex flex-col justify-center items-center'>
                        <div className='text-[22px] text-[#cbcbcb] font-[600] leading-[32px]'>Ceramic Shield</div>
                        <div className='text-[28px] text-[#cbcbcb] font-[600] leading-[32px] px-10 text-center'>Tougher than any smartphone glass.</div>
                     </div>
                     <div className='track-img w-full basis-3/5 flex justify-center items-end '>
                        <Image 
                           className='h-[250px] w-fit' src='/images/preview-imgs/picture07.png' 
                           alt="Picture of the author" 
                           width={1000}   
                           height={1000} 
                           quality={100}/>
                     </div>
                  </div>
                  
                  <div className="bg-[url('/images/preview-imgs/picture08.png')] bg-cover bg-no-repeat bg-center col-end-[span_2] flex flex-row rounded-[20px]" style={{blockSize:"400px"}}>
                     <div className='track-content w-full text-white flex flex-col justify-start items-center pt-[26px]'>
                        <div className='text-[22px] text-[#cbcbcb] font-[600] leading-[32px]'>Action mode</div>
                        <div className='text-[28px] text-[#cbcbcb] font-[600] leading-[32px] px-[75px] text-center'>Shaky shots, stable video.</div>
                     </div>
                  </div>
                  
                  <div className="bg-[url('/images/preview-imgs/picture09.png')] bg-cover bg-no-repeat bg-center col-end-[span_4] rounded-[20px]" style={{blockSize:"400px"}}>
                  </div>

                  <div className='bg-black col-end-[span_2] flex flex-row rounded-[20px]' style={{blockSize:"400px"}}>
                     <div className='track-img w-full flex justify-center items-center '>
                        <Image 
                           className='h-[260px] w-[260px]' src='/images/preview-imgs/picture10.png' 
                           alt="Picture of the author" 
                           width={260}   
                           height={260} 
                           quality={100}/>
                     </div>
                  </div>

                  <div className='bg-black col-end-[span_3] flex flex-col rounded-[20px]' style={{blockSize:"400px"}}>
                     <div className='track-content w-full text-white flex flex-col justify-end items-center pt-[50px]'>
                        <div className='text-[32px] text-[#cbcbcb] font-[600] leading-[32px]'>A camera in</div>
                        <div className='text-[32px] text-[#cbcbcb] font-[600] leading-[32px] pt-1'>a class by itselfie.</div>
                     </div>
                     <div className='track-img w-full pt-[40px]'>
                        <Image 
                           className='h-[200px] w-fit mx-auto' src='/images/preview-imgs/picture11.png' 
                           alt="Picture of the author" 
                           width={1000}   
                           height={1000} 
                           quality={100}/>
                     </div>
                  </div>

                  <div className='bg-black col-end-[span_3] flex flex-row rounded-[20px]' style={{blockSize:"400px"}}>
                     <div className='w-full flex flex-col justify-end items-center basis-[55%]'>
                        <div className='track-content text-white'>
                           <div className='text-[40px] text-[#cbcbcb] font-[600] leading-[32px] text-center'>6.7"</div>
                           <div className='text-[22px] text-[#cbcbcb] font-[600] leading-[32px] text-center pt-3'>iPhone 14 Pro Max</div>
                        </div>
                        <div className='track-img pt-[32px]'>
                           <Image 
                              className='h-[260px] w-fit' src='/images/preview-imgs/picture12.png' 
                              alt="Picture of the author" 
                              width={1000}   
                              height={1000} 
                              quality={100}/>
                        </div>
                     </div>
                     <div className='w-full flex flex-col justify-end items-center basis-[45%]'>
                        <div className='track-content text-white'>
                           <div className='text-[40px] text-[#cbcbcb] font-[600] leading-[32px] text-center'>6.1"</div>
                           <div className='text-[22px] text-[#cbcbcb] font-[600] leading-[32px] text-center pt-3'>iPhone 14 Pro</div>
                        </div>
                        <div className='track-img pt-[32px]'>
                           <Image 
                              className='h-[224px] w-fit' src='/images/preview-imgs/picture12.png' 
                              alt="Picture of the author" 
                              width={1000}   
                              height={1000} 
                              quality={100}/>
                        </div>
                     </div>
                  </div>
               </div>
            </div>
         </div>

         <div className='hero-container w-full h-[500px] bg-cyan-200 mb-[32px] flex justify-center items-center'>

         </div>

         <div className='hero-container w-full h-[1000px] bg-cyan-200 mb-[32px] static'>
         </div>    
      </div>
   );
};

export default HomePage;
