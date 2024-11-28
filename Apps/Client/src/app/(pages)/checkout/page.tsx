/* eslint-disable react/react-in-jsx-scope */
'use client';
import { useEffect, useState } from "react";
import { useGetUsersQuery } from "~/services/example/user.service";
import '~/styles/globals.css';
import Image from 'next/image';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import images from '~/components/client/images';
import { Button } from '~/components/ui/button';
import { Input } from '~/components/ui/input';
import { Accordion, AccordionItem, AccordionTrigger, AccordionContent } from '~/components/ui/accordion';
import LineOrder from "~/app/(pages)/checkout/LineOrder";


const CheckoutPage = () => {
     const { data } = useGetUsersQuery('');

     useEffect(() => {
        console.log(data);
     }, [data]);

     const [checkLogin, setCheckLogin] = useState(true);

     return (
          <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay w-full flex flex-col items-center justify-start bg-white')}>
               <div className='max-w-[1156px] w-full grid grid-cols-12'>
                    
                    {/* LOGIN TRUE */}
                    { checkLogin &&
                    <div className="col-span-8">
                         <div className='w-full pr-[64px]'>
                              <div className="w-full text-[40px] font-semibold tracking-[0.2px]">
                                   Continue as a member
                              </div>    
                              <div className="w-full text-[20px] font-light tracking-[0.5px]">
                                   Sign in now to get FREE DELIVERY.
                              </div>
                              <Button className="w-[300px] h-fit bg-[#0070f0] text-white rounded-full text-[20px] font-medium tracking-[0.2px] mt-3 mb-3">
                                   Sign in to checkout
                              </Button>
                              <div className="w-full text-[20px] font-light tracking-[0.5px]">
                                   New to shop? Create an account to checkout faster!
                              </div>
                              <a href="#" className="w-full text-[20px] font-light tracking-[0.5px] text-[#0070f0] underline">
                                   Create an account
                              </a>
                         </div>
                         {/* <div className="w-full h-fit pr-[64px] py-8 flex flex-row justify-between items-center">
                              <div className="w-full border-t border-[#d9d9d9]"/>
                              <div className="text-[16px] text-[#555] font-bold tracking-[0.5px] mx-3">
                                   OR
                              </div>
                              <div className="w-full border-t border-[#d9d9d9]"/>
                         </div>
                         <div className='w-full h-fit pr-[64px] flex flex-col justify-start'>
                              <div className="w-full text-[24px] font-semibold tracking-[0.2px]">
                                   Continue as guest
                              </div>
                              <div className="w-full text-[14px] font-light tracking-[0.5px]">
                                   Using guest checkout? You could miss out on rewards and exclusive benefits.
                              </div>
                              <div className="w-full pt-2 flex flex-row gap-2 items-center">
                                   <Input className='basis-1/2 h-full p-0 border-x-0 border-t-0 border-b-[#000] focus:border-b-2 focus:border-b-[#0070f0] 
                                   rounded-none focus-visible:ring-0 focus-visible:ring-offset-0 text-start text-[18px] font-light tracking-[1px] pt-5 pb-1' 
                                   placeholder="Enter your email address to checkout" type='email' />
                                   <Button className="basis-1/2 h-fit bg-[#dddddd] text-[#aaaaaa] hover:bg-[#000] hover:text-white 
                                   rounded-full text-[14px] font-medium tracking-[0.2px] py-[10px]">
                                        Guest checkout
                                   </Button>
                              </div>
                         </div> */}
                    </div>
                    }

                    {/* LOGIN FALSE */}
                    { !checkLogin &&
                    <div className="col-span-8">
                         
                    </div>}

                    {/* ORDER SUMMARY */}
                    <div className='col-span-4 h-full'>
                         <form className='w-full h-fit flex flex-col justify-start items-start border border-[#dddddd] rounded-[10px] p-[30px]'>
                              <div className='order-summary w-full flex flex-col justify-start items-start'>
                                   <div className='w-full pb-3 text-[22px] text-black font-bold tracking-[0.8px]'>Order Summary</div>
                                   <div className='text-[16px] font-light tracking-[0.2px]'>You have 5 items in your cart</div>
                                   <div className="w-full flex flex-col justify-start items-center">
                                        <LineOrder image={images.ipSE}/>
                                        <LineOrder image={images.ipXSMax}/>
                                        <LineOrder image={images.ip12Mini}/>
                                        <LineOrder image={images.ip13Mini}/>
                                   </div>
                              </div>
                              <div className='summary w-full flex flex-col justify-start items-start py-6 border-b border-[#dddddd]'>
                                   <div className='w-full pb-3 text-[22px] text-black font-bold tracking-[0.8px]'>Summary</div>
                                   <div className='w-full flex flex-col justify-between'>
                                   <div className='w-full flex flex-row justify-between items-center text-[14px] tracking-[0.2px]'>
                                        <div className='font-light'>Subtotal</div>
                                        <div className='font-semibold'>$2,942.00</div>
                                   </div> 
                                   <Accordion type="multiple" className="w-full h-full ">
                                        <AccordionItem value="item-1" className='border-none'>
                                             <AccordionTrigger className='hover:no-underline text-[14px] font-semibold tracking-[0.2px] pb-0 pt-0'>
                                                  Total Savings
                                             </AccordionTrigger>
                                             <AccordionContent>
                                                  <div className='pt-1 pl-1 w-full flex flex-row justify-between items-center text-[14px] tracking-[0.2px]'>
                                                       <div className='font-light'>Other Discount</div>
                                                       <div className='font-semibold'>- $258.00</div>
                                                  </div> 
                                             </AccordionContent>
                                        </AccordionItem>
                                   </Accordion>
                                   </div>
                              </div>
                              <div className='total w-full flex flex-col justify-start items-start pt-6'>
                                   <div className='w-full flex flex-row justify-between items-center text-[24px] font-semibold tracking-[0.2px]'>
                                   <div className=''>Total</div>
                                   <div className=''>$2,684.00</div>
                                   </div> 
                                        <Button className='w-full h-fit bg-[#0070f0] text-white rounded-full text-[14px] font-medium tracking-[0.2px] mt-5'>
                                        Checkout
                                        </Button>
                                   <div className='w-full mt-5 flex flex-col gap-3 text-[12px] font-semibold tracking-[0.2px]'>
                                   <div className='w-full flex flex-row items-center'>
                                        <Image src={images.iconFreeShip} alt='icon-deal' width={1000} height={1000} quality={100} className='w-[20px] h-[20px]'/>
                                        <div className='pl-2'>Free delivery</div>
                                   </div>
                                   <div className='w-full flex flex-row items-center'>
                                        <Image src={images.iconDeal} alt='icon-deal' width={1000} height={1000} quality={100} className='w-[20px] h-[20px]'/>
                                        <div className='pl-2'>0% Instalment Plans</div>
                                   </div>
                                   <div className='w-full flex flex-row items-center'>
                                        <Image src={images.iconWarranty} alt='icon-deal' width={1000} height={1000} quality={100} className='w-[20px] h-[20px]'/>
                                        <div className='pl-2'>Warranty</div>
                                   </div>
                                   </div>
                              </div>
                         </form>
                    </div>
               </div>
          </div>
     );
}

export default CheckoutPage;