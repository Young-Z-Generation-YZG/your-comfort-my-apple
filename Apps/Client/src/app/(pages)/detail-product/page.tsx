/* eslint-disable react/react-in-jsx-scope */
'use client';
import { useEffect, useState } from 'react';
import { useGetUsersQuery } from '~/services/example/user.service';
import '~/styles/globals.css';
import Image from 'next/image';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import {
   Carousel,
   CarouselApi,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from "~/components/ui/carousel"
import Link from 'next/link';
import { Button } from '~/components/ui/button';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from "~/components/ui/select";

import images from '~/components/client/images';
import ModelItem from '~/app/(pages)/detail-product/ModelItem';
import HelpItem from '~/app/(pages)/detail-product/HelpItem';
import CompareItem from '~/app/(pages)/detail-product/CompareItem';
import ColorItem from '~/app/(pages)/detail-product/ColorItem';
import StorageItem from '~/app/(pages)/detail-product/StorageItem';

const listProduct = [
   {
      id: 'ip14proMax',
      checkNew: false,
      name: 'iPhone 14 Pro Max',
      image: images.ip14ProMax,
      price: 999,
      colors: ['#594f63', '#f4e8ce', '#f0f2f2', '#403e3d'],
      screen: ['6,7"', 'Super Retina XDR display','ProMotion technology','Always-On display'], // max 4 item
      checkDynamic: true,
      chip: [16,'A16 Bionic chip with 5-core GPU'], // max 2 item
      battery: 'Up to 29 hours video playback',
      biometricAuthen: "Face ID",
      crashDetection: [ 'Emergency SOS','Crash Detection'], //max 5 item
      camera: ['ultraWT','Pro camera system','48MP Fusion | Ultra Wide Telephoto','Photonic Engine cho màu sắc và chi tiết ấn tượng','Camera trước TrueDepth có khả năng tự động lấy nét'], // max 9 item
      material: ['Titanium with textured matte glass back','Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C','Supports USB 2']
   },
   {
      id: 'ip14plus',
      checkNew: false,
      name: 'iPhone 14 Plus',
      image: images.ip14Plus,
      price: 499,
      colors: ['#a0b4c7', '#e6ddeb', '#f9e479', '#222930','#faf6f2','#fc0324'],
      screen: ['6,7"', 'Super Retina XDR display'], // max 4 item
      checkDynamic: false,
      chip: [15,'A15 Bionic chip with 5-core GPU'], // max 2 item
      battery: 'Up to 26 hours video playback',
      biometricAuthen: "Face ID",
      crashDetection: [ 'Emergency SOS','Crash Detection'], //max 5 item
      camera: ['ultraWX','Advanced dual-camera system','12MP Fusion | Ultra Wide','Photonic Engine cho màu sắc và chi tiết ấn tượng','Camera trước TrueDepth có khả năng tự động lấy nét'], // max 9 item
      material: ['Titanium with textured matte glass back','Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C','Supports USB 2']
   },
   {
      id: 'ip13proMax',
      checkNew: false,
      name: 'iPhone 13 Pro Max',
      image: images.ip13ProMax,
      price: 599,
      colors: ['#576856', '#f1f2ed', '#fae7cf', '#54524f','#a7c1d9'],
      screen: ['6,7"', 'Super Retina XDR display','ProMotion technology'], // max 4 item
      checkDynamic: false,
      chip: [15,'A15 Bionic chip with 5-core GPU'], // max 2 item
      battery: 'Up to 28 hours video playback',
      biometricAuthen: "Face ID",
      crashDetection: [ 'Emergency SOS'], //max 5 item
      camera: ['ultraWT','Pro camera system','12MP Fusion | Ultra Wide Telephoto','Camera trước TrueDepth'], // max 9 item
      material: ['Titanium with textured matte glass back','Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C','Supports USB 2']
   },
   {
      id: 'ip13mini',
      checkNew: false,
      name: 'iPhone 13 Mini',
      image: images.ip13Mini,
      price: 499,
      colors: ['#394c38', '#faddd7', '#276787', '#232a31','#faf6f2','#be0013'],
      screen: ['5,4"', 'Super Retina XDR display'], // max 4 item
      checkDynamic: false,
      chip: [15,'A15 Bionic chip with 4-core GPU'], // max 2 item
      battery: 'Up to 17 hours video playback',
      biometricAuthen: "Face ID",
      crashDetection: [ 'Emergency SOS'], //max 5 item
      camera: ['ultraWX','Dual-camera system','12MP Fusion | Ultra Wide','Camera trước TrueDepth'], // max 9 item
      material: ['Titanium with textured matte glass back','Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C','Supports USB 2']
   },
   {
      id: 'ip12mini',
      checkNew: false,
      name: 'iPhone 12 Mini',
      image: images.ip12Mini,
      price: 459,
      colors: ['#b7afe6', '#023b63', '#d8efd5', '#d82e2e','#f6f2ef','#25212b'],
      screen: ['5,4"', 'Super Retina XDR display'], // max 4 item
      checkDynamic: false,
      chip: [14,'A14 Bionic chip with 4-core GPU'], // max 2 item
      battery: 'Up to 15 hours video playback',
      biometricAuthen: "Face ID",
      crashDetection: [ 'Emergency SOS'], //max 5 item
      camera: ['ultraWV','Dual-camera system','12MP Fusion | Ultra Wide','Camera trước TrueDepth'], // max 9 item
      material: ['Titanium with textured matte glass back','Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C','Supports USB 2']
   },
   {
      id: 'ipXSMax',
      checkNew: false,
      name: 'iPhone XS Max',
      image: images.ipXSMax,
      price: 599,
      colors: ['#e4e4e2', '#262529', '#fadcc2'],
      screen: ['6,5"', 'Super Retina HD display'], // max 4 item
      checkDynamic: false,
      chip: [12,'A12 Bionic chip with 4-core GPU'], // max 2 item
      battery: 'Up to 15 hours video playback',
      biometricAuthen: "Face ID",
      crashDetection: [ 'Emergency SOS'], //max 5 item
      camera: ['telephoto','Dual-camera system','12MP Fusion | Telephoto','Camera trước TrueDepth'], // max 9 item
      material: ['Titanium with textured matte glass back','Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C','Supports USB 2']
   },
]

const DetailProductPage = () => {
   const { data } = useGetUsersQuery('');

   useEffect(() => {
      console.log(data);
   }, [data]);

   const [api, setApi] = useState<CarouselApi>()
   const [current, setCurrent] = useState(0)
   const [count, setCount] = useState(0)

   // Carousel
   useEffect(() => {
      if (!api) {
        return
      }
   
      setCount(api.scrollSnapList().length)
      setCurrent(api.selectedScrollSnap() + 1)
   
      api.on("select", () => {
        setCurrent(api.selectedScrollSnap() + 1)
      })
    }, [api])

   const [selectedOption1, setSelectedOption1] = useState('ip14proMax');
   const [selectedOption2, setSelectedOption2] = useState('ip14plus');
   const [selectedOption3, setSelectedOption3] = useState('ip13proMax');
   const [selectedOption4, setSelectedOption4] = useState('ip13mini');

   // Visible
   const [visibleModel,setVisibleModel] = useState({
      order: 1,
      visible: true,
      isSelected: -1
   })

   const [visibleColor,setVisibleColor] = useState({
      order: 2,
      visible: false,
      isSelected: -1
  })

   const [visibleStorage,setVisibleStorage] = useState({
      order: 3,
      visible: false,
      isSelected: -1
   })

   const handleVisible = (order:number, index:number) => {
      if(order === 1) {
         setVisibleModel({...visibleModel, isSelected: index});
         setVisibleColor({...visibleColor, visible: true});
      }
      else if(order === 2) {
         setVisibleColor({...visibleColor, isSelected: index});
         setVisibleStorage({...visibleStorage, visible: true});
      }
      else if(order === 3) {
         setVisibleStorage({...visibleStorage, isSelected: index});
      }
   }

   return (
      <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay w-full flex flex-col items-start justify-center bg-[#fff]')}>
         
         <div className='carrier-container w-full pt-[14px] relative bg-transparent'>
            <div className='w-[980px] py-[14px] h-[66px] rounded-[5px] flex flex-row justify-center items-center text-[12px] font-normal bg-[#f5f5f7] mx-auto'>
               <div className='h-full pl-[20px] w-[176px] flex flex-col'>
                  <div className='leading-[18px] tracking-[0.4px] text-[15px] font-medium'>Carrier Deals at Apple</div>
                  <div className='leading-[20px] tracking-[0.4px] text-[15px] font-light text-blue-500'>
                     <a href='http://localhost:3000/home' target='_blank'>See all deals</a>
                  </div>
               </div>
               <div className='h-full pl-[20px] w-[190px] flex flex-row items-center'>
                  <div className='basis-1/6 w-[24px]'>
                     <Image
                        src={images.icAtt}
                        className='h-[24px] w-auto mx-auto'
                        alt='apple-logo'
                        width={1000}
                        height={1000}
                        quality={100}
                     />
                  </div>
                  <div className='basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light'>Save up to $1000 after trade-in.</div>
               </div>
               <div className='h-full pl-[20px] w-[190px] flex flex-row items-center'>
                  <div className='basis-1/6 w-[24px]'>
                     <Image
                        src={images.icLightYear}
                        className='h-[24px] w-auto mx-auto'
                        alt='apple-logo'
                        width={1000}
                        height={1000}
                        quality={100}
                     />
                  </div>
                  <div className='basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light'>Save up to $1000. No trade-in needed.</div>
               </div>
               <div className='h-full pl-[20px] w-[190px] flex flex-row items-center'>
                  <div className='basis-1/6 w-[24px]'>
                     <Image
                        src={images.icTmobile}
                        className='h-[24px] w-auto mx-auto'
                        alt='apple-logo'
                        width={1000}
                        height={1000}
                        quality={100}
                     />
                  </div>
                  <div className='basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light'>Save up to $1000 after trade-in.</div>
               </div>
               <div className='h-full pl-[20px] w-[190px] flex flex-row items-center'>
                  <div className='basis-1/6 w-[24px]'>
                     <Image
                        src={images.icVerizon}
                        className='h-[24px] w-auto mx-auto'
                        alt='apple-logo'
                        width={1000}
                        height={1000}
                        quality={100}
                     />
                  </div>
                  <div className='basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light'>Save up to $1000 after trade-in.</div>
               </div>
            </div>
         </div>

         <div className='detail-container min-w-[980px] max-w-[2632px] w-[87.5%] mx-auto relative bg-transparent'>
            {/* Product Title */}
            <div className='w-full bg-transparent flex flex-row pt-[52px] pb-[22px]'>
               <div className='basis-[70%] bg-transparent'>
                  <span className='text-[18px] text-[#b64400] font-semibold leading-[20px]'>New</span>
                  <h1 className='text-[48px] font-semibold leading-[52px] pb-2 mb-[13px]'>Buy iPhone 16 Pro </h1>
                  <div className='text-[15px] font-light leading-[20px]'>From $999 or $41.62/mo. for 24 mo.</div>
               </div>
               <div className='basis-[30%] bg-transparent'>
                  <div className='h-full flex flex-col mt-1'>
                     <div className='w-full basis-1/2 flex flex-row justify-end'>
                        <div className='w-fit text-[13px] font-light leading-[16px] tracking-[0.7px] px-[16px] py-[12px] my-[6px] flex items-center bg-[#f5f5f7] rounded-full'>
                           Get $40–$650 for your trade-in.
                        </div>
                     </div>
                     <div className='w-full basis-1/2 flex flex-row justify-end'>
                        <div className='w-fit text-[13px] font-light leading-[16px] tracking-[0.7px] px-[16px] py-[12px] my-[6px] flex items-center bg-[#f5f5f7] rounded-full'>
                           Get 3% Daily Cash back with Apple Card.
                        </div>
                     </div>
                  </div>
               </div>
            </div>

            {/* Product Detail */}
            <div className='w-full bg-transparent flex flex-row relative pb-[30px]'>
               <div className='basis-3/4 h-fit pr-[60px] sticky top-[100px]'>
                  <div className='w-full flex items-center justify-center pt-10'>
                     <Carousel setApi={setApi} className="w-full z-10">
                        <CarouselContent className='min-h-[460px] h-[calc(100vh-189px)]'>
                           <CarouselItem>
                              <Image
                              src={images.ip16ProMaxMasonry1}
                              className='h-full w-full rounded-[20px]'
                              width={2000}
                              height={2000}
                              quality={100}
                              style={{
                                 objectFit: 'cover',
                                 }}
                              alt='iphone-16-pro-model-unselect-gallery-1-202409'/>
                           </CarouselItem>
                           {Array.from({ length: 5 }).map((_, index) => (
                           <CarouselItem key={index}>
                              <Image
                              src={images.ip16ProMaxMasonry2}
                              className='h-full w-full rounded-[20px]'
                              width={2000}
                              height={2000}
                              quality={100}
                              style={{
                                 objectFit: 'cover',
                              }}
                              alt='iphone-16-pro-model-unselect-gallery-1-202409'/>
                           </CarouselItem>))}
                        </CarouselContent>
                        <CarouselPrevious className='left-[1rem]'/>
                        <CarouselNext className='right-[1rem]'/>
                        <div className='absolute bottom-2 left-0 w-full z-50 flex flex-row items-center justify-center gap-2'>
                           {/* Slide {current} of {count} */}
                           {Array.from({length:count}).map((_,index) => (
                              <div className='w-[10px] h-[10px] rounded-full' style={{'backgroundColor': current === index + 1 ? '#6b7280' : '#d1d5db'}} key={index}/>
                           ))}
                        </div>
                     </Carousel>
                  </div>
               </div>
               <div className='basis-1/4 h-fit min-w-[328px]'>
                  {/* model */}
                  <div className='type-model w-full mt-[100px]'>
                     <div className='text-[24px] font-semibold leading-[28px] pb-[13px]'>
                        <span className='text-[#1D1D1F]'>Model. </span> 
                        <span className='text-[#86868B]'>Which is best for you?</span> 
                     </div>
                     {Array.from({length: 2}).map((_, index) => (
                        <ModelItem 
                           key={index}
                           isSelected={visibleModel.isSelected === index}
                           onClick={() => {
                              if (visibleModel.visible) {
                                 handleVisible(1, index);
                              }
                           }}
                        />
                     ))}
                     <div className='mt-[20px] mb-[6px]'>
                        <HelpItem
                           title='Need help choosing a model?'
                           subTitle='Explore the differences in screen size and battery life.'
                        />
                     </div> 
                  </div>

                  {/* color */}
                  <div className='type-color w-full mt-[100px]'>
                     <div className='text-[24px] font-semibold leading-[28px] pb-[13px]'>
                        <span className='text-[#1D1D1F]'>Finish. </span> 
                        <span className='text-[#868a8b]'>Which is best for you?</span> 
                     </div>
                     <div className='pt-5 text-[17px] font-semibold leading-[25px] tracking-[0.5px]'>Color</div>
                     <div className='flex flex-row gap-3 pt-[17px] pb-[13px]' style={{opacity: visibleColor.visible ? 1 : 0.5}}>
                        {Array.from({length: 5}).map((_, index) => (
                           <ColorItem
                              key={index}
                              isSelected={visibleColor.isSelected === index}
                              onClick={() => {
                                 if (visibleColor.visible) {
                                    handleVisible(2, index);
                                 }
                              }}
                              color="#21e9ff"
                           />
                        ))}
                     </div>
                  </div>

                  {/* storage */}
                  <div className='type-storage w-full mt-[200px]'>
                     <div className='text-[24px] font-semibold leading-[28px] pb-[13px]'>
                        <span className='text-[#1D1D1F]'>Storage. </span>
                        <span className='text-[#86868B]'>How much space do you need?</span> 
                     </div>
                     <div className='flex flex-col gap-[14px] my-[14px]' style={{opacity: visibleStorage.visible ? 1 : 0.5}}>
                        {Array.from({length: 3}).map((_, index) => (
                           <StorageItem 
                              key={index}
                              isSelected={visibleStorage.isSelected === index}
                              onClick={() => {
                                 if (visibleStorage.visible) {
                                    handleVisible(3, index);
                                 }
                           }}/>
                        ))}
                        <HelpItem
                           image={images.helpStorage}
                           title='Not sure how much storage to get?'
                           subTitle='Get a better understanding of how much space you’ll need.'
                        />
                     </div>
                  </div>
               </div>
            </div>

            {/* Apple Trade In */}
            <div className='w-full bg-transparent flex flex-col mt-[74px] h-fit'>
               <div className='apple-trade-in-title flex flex-row'>
                     <div className='basis-3/4'>
                        <div className='w-full text-[24px] font-semibold leading-[28px]'>
                           <span className='text-[#1D1D1F] tracking-[0.3px]'>Apple Trade In. </span>
                           <span className='text-[#86868B] tracking-[0.3px]'>Get $40–$650 credit toward your new iPhone.</span> 
                        </div>
                     </div>
                     <div className='basis-1/4 min-w-[328px]'>
                     </div>
                  </div>
                  <div className='apple-trade-in-items flex flex-row mt-6'>
                     <div className='basis-3/4 list-items w-full grid grid-cols-2 gap-4 pr-12'>
                        <div className='basis-1/2 border-[0.8px] border-[#86868b] h-[83px] rounded-[10px] p-[14px] flex flex-col items-center justify-center'>
                           <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.5px] text-center'>Select a smartphone</div>
                           <div className='item-sub-title text-[12px] font-light leading-[16px] tracking-[0.8px] text-center mt-1'>Answer a few questions to get your estimate.</div>
                        </div>
                        <div className='basis-1/2 border-[0.8px] border-[#86868b] h-[83px] rounded-[10px] p-[14px] flex flex-col items-center justify-center'>
                           <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.5px] text-center'>No trade-in</div>
                        </div>
                     </div>
                     <div className='basis-1/4 min-w-[328px]'>
                        <HelpItem
                           image={images.helpTradeInPrePurchase}
                           title='How does trade-in work?'
                           subTitle='We’ll walk you through the step-by-step process.'
                        />
                     </div>
                  </div>
            </div>

            {/* Payment options */}
            <div className='w-full bg-transparent flex flex-col mt-[74px] h-fit'>
               <div className='payment-title flex flex-row'>
                  <div className='basis-3/4'>
                     <div className='w-full text-[24px] font-semibold leading-[28px]'>
                        <span className='text-[#1D1D1F] tracking-[0.3px]'>Payment options. </span>
                        <span className='text-[#86868B] tracking-[0.3px]'>Select the one that works for you.</span> 
                     </div>
                  </div>
                  <div className='basis-1/4 min-w-[328px]'></div>
               </div>
               <div className='payment-items flex flex-row mt-6'>
                  <div className='basis-3/4 list-items w-full grid grid-cols-3 gap-4 pr-12'>
                     <div className='basis-1/3 border-[0.8px] border-[#86868b] rounded-[10px] py-[21px] px-[17px] flex flex-col items-start'>
                        <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] mb-[10px]'>Buy</div>
                        <div className='item-title text-[17px] font-light leading-[21px] tracking-[0.5px] mb-[10px]'>
                           $1,299.00
                        </div>
                        <div className='item-sub-title text-[12px] font-light leading-[16px] tracking-[0.8px] pt-[18px] mt-auto'>
                           Pay with Apple Pay or other payment methods.
                        </div>
                     </div>
                     <div className='basis-1/3 border-[0.8px] border-[#86868b] rounded-[10px] py-[21px] px-[17px] flex flex-col items-start'>
                        <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] mb-[10px]'>Finance</div>
                        <div className='item-title text-[17px] font-light leading-[21px] tracking-[0.5px] mb-[10px]'>
                           $54.12/mo. for 24 mo.
                        </div>
                        <div className='item-title text-[17px] font-light leading-[21px] tracking-[0.5px] mb-[10px]'>
                           From $8.31/mo. over 36 mo. with a select carrier deal
                        </div>
                        <div className='item-sub-title text-[12px] font-light leading-[16px] tracking-[0.8px] pt-[18px] mt-auto'>
                           Pay over time at 0% APR.
                        </div>
                     </div>
                     <div className='basis-1/3 border-[0.8px] border-[#86868b] rounded-[10px] py-[21px] px-[17px] flex flex-col items-start'>
                        <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] mb-[10px]'>Apple iPhone Upgrade Program</div>
                        <div className='item-title text-[17px] font-light leading-[21px] tracking-[0.5px] mb-[10px]'>
                           $62.41/mo. for 24 mo.
                        </div>
                        <div className='item-sub-title text-[12px] font-light leading-[16px] tracking-[0.8px] pt-[18px] mt-auto'>
                           Pay monthly at 0% APR with the option to upgrade to a new iPhone every year.
                        </div>
                     </div>
                  </div>
                  <div className='basis-1/4 min-w-[328px]'>
                     <HelpItem 
                        title='Not sure which payment option to choose?'
                        subTitle='Compare all pricing and deals to find the option that’s right for you.'
                     />
                  </div>
               </div>
            </div>

            {/* Connectivity */}
            <div className='w-full bg-transparent flex flex-col mt-[74px] h-fit'>
               <div className='connectivity-title flex flex-row'>
                  <div className='basis-3/4'>
                     <div className='w-full text-[24px] font-semibold leading-[28px]'>
                        <span className='text-[#1D1D1F] tracking-[0.3px]'>Connectivity. </span>
                        <span className='text-[#86868B] tracking-[0.3px]'>Choose a carrier.</span> 
                     </div>
                  </div>
                  <div className='basis-1/4 min-w-[328px]'></div>
               </div>
               <div className='payment-items flex flex-row mt-6'>
                  <div className='basis-3/4 list-items w-full grid grid-cols-3 gap-4 pr-12'>
                     <div className='h-[140px] border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col'>
                        <div className='w-full'>
                           <Image
                           src={images.logoAtt}
                           className='w-[63px] h-[26px]'
                           width={2000}
                           height={2000}
                           quality={100}
                           alt=''/>
                        </div>
                        <div className='item-title w-full text-[14px] font-light leading-[21px] tracking-[0.5px] pt-[3px]'>$1,299.00</div>
                     </div>
                     <div className='h-[140px] border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col'>
                        <div className='w-full'>
                           <Image
                           src={images.logoLightYear}
                           className='w-[63px] h-[26px]'
                           width={2000}
                           height={2000}
                           quality={100}
                           alt=''/>
                        </div>
                        <div className='item-title w-full text-[14px] font-light leading-[21px] tracking-[0.5px] pt-[3px]'>$1,299.00</div>
                        <div className='item-sub-title w-full text-[12px] font-light leading-[16px] tracking-[0.8px] mt-[15px]'>For new Boost Mobile customers only</div>
                     </div>
                     <div className='h-[140px] border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col'>
                        <div className='w-full'>
                           <Image
                           src={images.logoTmobile}
                           className='w-[63px] h-[26px]'
                           width={2000}
                           height={2000}
                           quality={100}
                           alt=''/>
                        </div>
                        <div className='item-title w-full text-[14px] font-light leading-[21px] tracking-[0.5px] pt-[3px]'>$1,299.00</div>
                     </div>
                     <div className='h-[140px] border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col'>
                        <div className='w-full'>
                           <Image
                           src={images.logoVerizon}
                           className='w-[63px] h-[26px]'
                           width={2000}
                           height={2000}
                           quality={100}
                           alt=''/>
                        </div>
                        <div className='item-title w-full text-[14px] font-light leading-[21px] tracking-[0.5px] pt-[3px]'>$1,299.00</div>
                     </div>
                     <div className='h-[140px] border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col'>
                        <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] mb-[10px]'>Connect to any carrier later</div>
                        <div className='item-title w-full text-[14px] font-light leading-[21px] tracking-[0.5px] pt-[3px]'>$1,299.00</div>
                     </div>
                  </div>
                  <div className='basis-1/4 min-w-[328px]'>
                     <HelpItem
                        title='Is it easy to get connected?'
                        subTitle='Yes. We’ll help you set up your carrier, number, and rate plan. And your new iPhone will be unlocked.'
                     />
                  </div>
               </div>            
            </div>

            {/* Coverage */}
            <div className='w-full bg-transparent flex flex-col mt-[74px] h-fit'>
               <div className='coverage-title flex flex-row'>
                  <div className='basis-3/4'>
                     <div className='w-full text-[24px] font-semibold leading-[28px]'>
                        <span className='text-[#1D1D1F] tracking-[0.3px]'>AppleCare+ coverage. </span>
                        <span className='text-[#86868B] tracking-[0.3px]'>Protect your new iPhone.</span>
                     </div>
                  </div>
                  <div className='basis-1/4 min-w-[328px]'>
                  </div>
               </div>
               <div className='coverage-items flex flex-row mt-6'>
                  <div className='basis-3/4 list-items w-full grid grid-cols-3 gap-4 pr-12'>
                     <div className='border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col items-start'>
                        <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] flex flex-row items-start justify-start'>
                           <span className='flex flex-row items-start justify-start'>
                              <span className='w-fit'>
                                 <svg viewBox="5 0 21 21" width="21" height="21">
                                    <path fill="red" d="M18.4 8.146a3.5 3.5 0 00-1.675 2.948 3.411 3.411 0 002.075 3.129 8.151 8.151 0 01-1.063 2.2c-.662.953-1.354 
                                    1.905-2.407 1.905s-1.324-.612-2.537-.612c-1.183 0-1.6.632-2.567.632s-1.634-.882-2.407-1.965A9.5 9.5 0 016.2 11.255c0-3.008 1.955-4.6 
                                    3.881-4.6 1.023 0 1.875.672 2.517.672.612 0 1.564-.712 2.727-.712a3.648 3.648 0 013.075 1.531zM12.68 6.442a1.152 1.152 0 01-.211-.02 1.376 
                                    1.376 0 01-.03-.281 3.362 3.362 0 01.852-2.1 3.464 3.464 0 012.276-1.173 1.486 1.486 0 01.03.311 3.458 3.458 0 01-.822 2.156 3 3 0 01-2.095 1.107z"/>
                                 </svg>
                              </span> 
                              <span>AppleCare+</span>
                           </span>
                        </div>
                        <div className='item-price w-full pt-1 pb-[21px] border-b-[0.8px] border-[#86868b] text-[14px] font-light leading-[21px] tracking-[0.5px]'>
                           $199.00 or $9.99/mo.
                        </div>
                        <div className='item-sub-title w-full pt-[18px] text-[12px] font-light leading-[16px] tracking-[0.8px] '>
                           <ul className='list-disc ml-4'>
                              <li className='pr-3 pb-[15px] tracking-[0.7px]'>Unlimited repairs for accidental damage protection</li>
                              <li className='pr-3 pb-[15px] tracking-[0.7px]'>Apple-certified repairs using genuine Apple parts</li>
                              <li className='pr-3 pb-[15px] tracking-[0.7px]'>Express Replacement Service - we'll ship you a replacement so you don't have to wait for a repair</li>
                              <li className='pr-3 pb-[15px] tracking-[0.7px]'>24/7 priority access to Apple experts</li>
                           </ul>
                        </div>
                     </div>
                     <div className='border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col items-start'>
                        <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] flex flex-row items-start'>
                           <span className='flex flex-row items-start justify-start'>
                              <span className='w-fit'>
                                 <svg viewBox="5 0 21 21" width="21" height="21">
                                    <path fill="red" d="M18.4 8.146a3.5 3.5 0 00-1.675 2.948 3.411 3.411 0 002.075 3.129 8.151 8.151 0 01-1.063 2.2c-.662.953-1.354 1.905-2.407 
                                    1.905s-1.324-.612-2.537-.612c-1.183 0-1.6.632-2.567.632s-1.634-.882-2.407-1.965A9.5 9.5 0 016.2 11.255c0-3.008 1.955-4.6 3.881-4.6 1.023 0 1.875.672 
                                    2.517.672.612 0 1.564-.712 2.727-.712a3.648 3.648 0 013.075 1.531zM12.68 6.442a1.152 1.152 0 01-.211-.02 1.376 1.376 0 01-.03-.281 3.362 3.362 0 
                                    01.852-2.1 3.464 3.464 0 012.276-1.173 1.486 1.486 0 01.03.311 3.458 3.458 0 01-.822 2.156 3 3 0 01-2.095 1.107z"/>
                                 </svg>
                              </span> 
                              <span>AppleCare+ with Theft and Loss</span>
                           </span>
                        </div>
                        <div className='item-price w-full pt-1 pb-[21px] border-b-[0.8px] border-[#86868b] text-[14px] font-light leading-[21px] tracking-[0.5px]'>
                           $269.00 or $13.49/mo.
                        </div>
                        <div className='item-sub-title w-full pt-[18px] text-[12px] font-light leading-[16px] tracking-[0.8px] '>
                           <ul className='list-disc ml-4'>
                              <li className='pr-3 pb-[15px] tracking-[0.7px]'>Everything in AppleCare+ with additional coverage for theft and loss</li>
                              <li className='pr-3 pb-[15px] tracking-[0.7px]'>We can ship your replacement iPhone to any country where AppleCare+ with Theft and Loss is available.</li>
                           </ul>
                        </div>
                     </div>
                     <div className='border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col items-start'>
                        <div className='item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] flex flex-row items-center justify-start'>
                           No AppleCare+ coverage
                        </div>
                     </div>
                  </div>
                  <div className='basis-1/4 min-w-[328px]'>
                     <HelpItem
                        image={images.helpAppleCare}
                        title='What kind of protection do you need?'
                        subTitle='Compare the additional features and coverage of the two AppleCare+ plans.'
                     />
                  </div>
               </div>
            </div>
         </div>

         <div className='more-container w-full h-fit relative bg-[#f5f5f7] mt-[73px] flex items-center justify-center'>
            <div className='min-w-[980px] max-w-[1240px] w-[87.5%] mx-auto mt-[39px] flex flex-row gap-0'>
               <div className='basis-[34.76%]'>
                  <div className='pr-[10px] mr-[41px] flex flex-col items-start justify-start'>
                     <span className='text-[#1D1D1F] tracking-[0.3px] w-full text-[38px] font-semibold leading-[46px]'>Your new <br/>iPhone 16 Pro.</span>
                     <span className='text-[#86868B] tracking-[0.3px] w-full text-[38px] font-semibold leading-[46px]'>Just the way you <br/>want it.</span>
                     <Image
                        src={images.ip16ProWhiteTitaniumSelect}
                        className='w-full h-auto pt-[42px]'
                        width={2000}
                        height={2000}
                        quality={100}
                        alt=''
                     />
                  </div>
               </div>
               <div className='basis-[38.9%]'>
                  <div className='pr-[20px] mr-[41px] flex flex-col items-start justify-start'>
                     <div className='product-info w-full pb-11 text-[17px] text-[#1D1D1F] leading-[25px] tracking-[0.3px]'>
                        <div className='w-full pb-[7px] font-light'>iPhone 16 Pro 512GB White Titanium</div>
                        <div className='w-full font-semibold'>$1,299.00</div>
                        <div className='w-full pt-[35px] font-semibold'>One-time payment</div>
                        <div className='w-full pt-[14px] text-[14px] font-light'>Get 3% Daily Cash with Apple Card</div>
                     </div>
                     <div className='save-info w-full py-[21px] opacity-50 border-t-[0.8px] border-[#86868b] text-[14px] text-[#1D1D1F] leading-[20px] tracking-[0.3px]'>
                        <div className='w-full font-semibold'>Need a moment?</div>
                        <div className='w-full font-light pr-5 pb-2'>
                           Keep all your selections by saving this device to Your Saves, then come back anytime and pick up right where you left off.
                        </div>
                        <Link className='w-full font-light flex flex-row text-[#06c]' href='#'>
                           <svg width="21" height="21" className="as-svgicon as-svgicon-bookmark as-svgicon-tiny as-svgicon-bookmarktiny" role="img" aria-hidden="true">
                              <path fill="none" d="M0 0h21v21H0z"></path>
                              <path fill="#06c" d="M12.8 4.25a1.202 1.202 0 0 1 1.2 1.2v10.818l-2.738-2.71a1.085 1.085 0 0 0-1.524 0L7 16.269V5.45a1.202 1.202 0 0 1 
                              1.2-1.2h4.6m0-1H8.2A2.2 2.2 0 0 0 6 5.45v11.588a.768.768 0 0 0 .166.522.573.573 0 0 0 .455.19.644.644 0 0 0 .38-.128 5.008 5.008 0 0 0 
                              .524-.467l2.916-2.885a.084.084 0 0 1 .118 0l2.916 2.886a6.364 6.364 0 0 0 .52.463.628.628 0 0 0 .384.131.573.573 0 0 0 .456-.19.768.768 0 0 0 
                              .165-.522V5.45a2.2 2.2 0 0 0-2.2-2.2Z"/>
                           </svg>
                           Save for later
                        </Link>
                     </div>
                  </div>
               </div>   
               <div className='basis-[26.34%]'>
                  {/* <div className='flex flex-col'> */}
                     <div className='flex flex-row'>
                        <svg width="25" height="25" viewBox="0 0 25 25" className="icon mr-2">
                           <path d="M23.4824,12.8467,20.5615,9.6382A1.947,1.947,0,0,0,18.9863,9H17V6.495a2.5,2.5,0,0,0-2.5-2.5H3.5A2.5,2.5,0,0,0,1,6.495v9.75a2.5,2.5,0,0,0,2.5,
                           2.5h.5479A2.7457,2.7457,0,0,0,6.75,21.02,2.6183,2.6183,0,0,0,9.4222,19H16.103a2.7445,2.7445,0,0,0,5.3467-.23h.7349A1.6564,1.6564,0,0,0,24,
                           16.9805V14.1724A1.9371,1.9371,0,0,0,23.4824,12.8467ZM8.4263,18.745a1.7394,1.7394,0,0,1-3.3526,0,1.5773,1.5773,0,0,1,.0157-1,1.7382,1.7382,0,0,1,3.3213,
                           0,1.5782,1.5782,0,0,1,.0156,1ZM9.447,18a2.7258,2.7258,0,0,0-5.394-.255H3.5a1.5016,1.5016,0,0,1-1.5-1.5V6.495a1.5017,1.5017,0,0,1,1.5-1.5h11a1.5016,1.5016,
                           0,0,1,1.5,1.5V18Zm10.9715.77a1.7385,1.7385,0,0,1-3.3369,0,1.5727,1.5727,0,0,1,0-1,1.742,1.742,0,1,1,3.3369,1ZM23,16.9805c0,.5684-.2285.79-.8154.79H21.45A2.73,
                           2.73,0,0,0,17,16.165V10h1.9863a.9758.9758,0,0,1,.8379.3135l2.9268,3.2148a.95.95,0,0,1,.249.6441ZM21.6762,13.62A.5117.5117,0,0,1,21.85,14H18.5435A.499.499,0,
                           0,1,18,13.4718V11h1.0725a.7592.7592,0,0,1,.594.2676Z" fill="#1d1d1f"></path>
                        </svg>
                        <div className='flex flex-col text-[15px] text-[#1D1D1F] font-light leading-[25px] tracking-[0.3px]'>
                           <div className='font-semibold'>Delivery:</div>
                           <div>In Stock</div>
                           <div>Free Shipping</div>
                           <Link href='#' className='text-[#06c]'>Get delivery dates</Link>
                        </div>
                     </div>
                     <div className='flex flex-row mt-4'>
                        <svg width="25" height="25" viewBox="0 0 25 25" className="icon mr-2">
                           <path d="m0 0h25v25h-25z" fill="none"></path>
                           <path d="m18.5 5.0005h-1.7755c-.1332-2.2255-1.967-4.0005-4.2245-4.0005s-4.0913 1.775-4.2245 4.0005h-1.7755c-1.3789 0-2.5 1.1216-2.5 
                           2.5v11c0 1.3784 1.1211 2.4995 2.5 2.4995h12c1.3789 0 2.5-1.1211 2.5-2.4995v-11c0-1.3784-1.1211-2.5-2.5-2.5zm-6-3.0005c1.7058 0 3.0935 1.3264 
                           3.2245 3.0005h-6.449c.131-1.6741 1.5187-3.0005 3.2245-3.0005zm7.5 16.5005c0 .8271-.6729 1.5-1.5 1.5h-12c-.8271 0-1.5-.6729-1.5-1.5v-11c0-.8271.6729-1.5005 
                           1.5-1.5005h12c.8271 0 1.5.6734 1.5 1.5005zm-4.8633-7.5066c-.0377.0378-.7383.4304-.7383 1.3289 0 1.0344.8965 1.3968.9266 1.4044 0 
                           .0227-.1356.5059-.4746.9891-.2938.4228-.6177.8532-1.0848.8532-.4746 0-.5876-.2794-1.1375-.2794-.5273 0-.7157.2869-1.1451.2869-.4369 
                           0-.7383-.3926-1.0848-.8834-.3917-.5663-.7232-1.4572-.7232-2.3028 0-1.3515.8814-2.0688 1.7402-2.0688.4671 0 .8437.302 1.13.302.2787 0 .7006-.3171 
                           1.2204-.3171.2034-.0001.9115.015 1.3711.687zm-2.5538-.7626c-.0377 0-.0678-.0076-.0979-.0076 0-.0227-.0075-.0755-.0075-.1284 0-.3624.1883-.7097.3842-.9438.2486-.2945.6629-.521 
                           1.017-.5285.0075.0378.0075.0831.0075.1359 0 .3624-.1507.7097-.3616.974-.2336.287-.6253.4984-.9417.4984z" fill="#1d1d1f"></path>
                        </svg>
                        <div className='flex flex-col text-[15px] text-[#1D1D1F] font-light leading-[25px] tracking-[0.3px]'>
                           <div className='font-semibold'>Pickup:</div>
                           <Link href='#' className='text-[#06c]'>Check availability</Link>
                        </div>
                     </div>
                  {/* </div> */}
                  <div className='button mt-12' onClick={() => {}}>
                     <Button className='w-full bg-[#06c] text-white text-[15px] font-light leading-[18px] tracking-[0.8px] h-fit py-3'>Continue</Button>
                  </div>
               </div>
            </div>
         </div>

         <div className='box-container w-[980px] mx-auto'>
            <div className='w-full pt-16 pb-[41px] text-[40px] font-semibold leading-[44x] tracking-[0.5px] text-center'>
               What’s in the Box
            </div>
            <div className='w-full flex flex-col gap-0 justify-center items-center'>
               <div className='w-full bg-[#fafafc] flex flex-row gap-0 justify-center items-center'>
                  <div className='basis-[25%]'>
                     <Image
                        src={images.ip16ProWhiteTitaniumWitb}
                        className='w-auto h-[339px] mx-auto'
                        width={2000}
                        height={2000}
                        quality={100}
                        alt=''
                     />
                  </div>
                  <div className='basis-[25%]'>
                     <Image
                        src={images.ip16ProBraidedCable}
                        className='w-auto h-[339px] mx-auto'
                        width={2000}
                        height={2000}
                        quality={100}
                        alt=''
                     />
                  </div>
               </div>
               <div className='w-full flex flex-row gap-0 justify-center items-center'>
                  <div className='basis-[25%] pt-5 pb-[50px] text-center text-[14px] font-light leading-[20px] tracking-[0.3px]'>
                     iPhone 16 Pro
                  </div>
                  <div className='basis-[25%] pt-5 pb-[50px] text-center text-[14px] font-light leading-[20px] tracking-[0.3px]'>
                     USB-C Charge Cable
                  </div>
               </div>
            </div>
         </div>

         <div className='service-container w-[980px] mx-auto'>
            <div className='w-fit pt-12 pb-5 px-5 text-[40px] mx-auto font-semibold leading-[44px] tracking-[0.5px] text-center'>
               Your new iPhone comes<br/> with so much more.
            </div>
            <div className='w-full pt-9 pb-[54px] px-4 mt-[13px] flex flex-row gap-8 justify-center items-center'>
               <div className='basis-[25%] flex flex-col justify-center items-center'>
                  <Image
                     src={images.serviceTV}
                     className='h-auto w-[46px] mx-auto'
                     width={2000}
                     height={2000}
                     quality={100}
                     alt=''
                  />
                  <div className='w-full pt-4 pb-2 text-center text-[21px] font-semibold leading-[25px] tracking-[0.3px]'>Apple TV+</div>
                  <div className='w-full px-2 text-center text-[14px] font-light leading-[18px] tracking-[0.3px]'>3 free months of original films and series.°°°</div>
               </div>
               <div className='basis-[25%] flex flex-col justify-center items-center'>
                  <Image
                     src={images.serviceFitness}
                     className='h-auto w-[46px] mx-auto'
                     width={2000}
                     height={2000}
                     quality={100}
                     alt=''
                  />
                  <div className='w-full pt-4 pb-2 text-center text-[21px] font-semibold leading-[25px] tracking-[0.3px]'>Apple Fitness+</div>
                  <div className='w-full px-2 text-center text-[14px] font-light leading-[18px] tracking-[0.3px]'>3 free months of workouts, from HIIT to Meditation.°°°</div>
               </div>
               <div className='basis-[25%] flex flex-col justify-center items-center'>
                  <Image
                     src={images.serviceArcade}
                     className='h-auto w-[46px] mx-auto'
                     width={2000}
                     height={2000}
                     quality={100}
                     alt=''
                  />
                  <div className='w-full pt-4 pb-2 text-center text-[21px] font-semibold leading-[25px] tracking-[0.3px]'>Apple Arcade</div>
                  <div className='w-full px-2 text-center text-[14px] font-light leading-[18px] tracking-[0.3px]'>3 free months of incredibly fun, uninterrupted gameplay.°°°</div>
               </div>
               <div className='basis-[25%] flex flex-col justify-center items-center'>
                  <Image
                     src={images.serviceNews}
                     className='h-auto w-[46px] mx-auto'
                     width={2000}
                     height={2000}
                     quality={100}
                     alt=''
                  />
                  <div className='w-full pt-4 pb-2 text-center text-[21px] font-semibold leading-[25px] tracking-[0.3px]'>Apple News+</div>
                  <div className='w-full px-2 text-center text-[14px] font-light leading-[18px] tracking-[0.3px]'>3 free months of top stories from leading publications.°°°</div>
               </div>
            </div>
         </div>

         <div className='compare-container w-full mb-[100px] flex flex-col justify-center items-center'>
            <div className='compare-title bg-transparent w-[996px] mx-auto text-center pb-[70px]'>
               <div className='text-[40px] font-bold'>Which iPhone is right for you?</div>    
            </div>
            <div className='compare-content bg-transparent w-[996px] mx-auto flex flex-col'> 
               <div className='w-full flex flex-row gap-3 mb-10'>
                  <div className='basis-1/4  bg-gray-300 flex flex-col items-center rounded-[10px]'>
                     <Select 
                        value={selectedOption1}
                        onValueChange={(value) => setSelectedOption1(value)}
                     >
                        <SelectTrigger className="w-full text-base font-medium bg-white">
                           <SelectValue />
                        </SelectTrigger>
                        <SelectContent className="bg-white">
                           {listProduct.map((product:any,index:any) =>(
                           <SelectItem value={product.id} key={index}>{product.name}</SelectItem>
                           ))}
                        </SelectContent>
                     </Select>
                  </div>
                  <div className='basis-1/4  bg-gray-300 flex flex-col items-center rounded-[10px]'>
                     <Select 
                        value={selectedOption2}
                        onValueChange={(value) => setSelectedOption2(value)}
                     >
                        <SelectTrigger className="w-full text-base font-medium bg-white">
                           <SelectValue />
                        </SelectTrigger>
                        <SelectContent className="bg-white">
                           {listProduct.map((product:any,index:any) =>(
                           <SelectItem value={product.id} key={index}>{product.name}</SelectItem>
                           ))}
                        </SelectContent>
                     </Select>
                  </div>
                  <div className='basis-1/4  bg-gray-300 flex flex-col items-center rounded-[10px]'>
                     <Select 
                        value={selectedOption3}
                        onValueChange={(value) => setSelectedOption3(value)}
                     >
                        <SelectTrigger className="w-full text-base font-medium bg-white">
                           <SelectValue />
                        </SelectTrigger>
                        <SelectContent className="bg-white">
                           {listProduct.map((product:any,index:any) =>(
                           <SelectItem value={product.id} key={index}>{product.name}</SelectItem>
                           ))}
                        </SelectContent>
                     </Select>
                  </div>
                  <div className='basis-1/4  bg-gray-300 flex flex-col items-center rounded-[10px]'>
                     <Select 
                        value={selectedOption4}
                        onValueChange={(value) => setSelectedOption4(value)}
                     >
                        <SelectTrigger className="w-full text-base font-medium bg-white">
                           <SelectValue />
                        </SelectTrigger>
                        <SelectContent className="bg-white">
                           {listProduct.map((product:any,index:any) =>(
                           <SelectItem value={product.id} key={index}>{product.name}</SelectItem>
                           ))}
                        </SelectContent>
                     </Select>
                  </div>
               </div>
               <div className='w-full flex flex-row gap-3'>
                  <CompareItem compare={listProduct.find((product:any) => product.id === selectedOption1)} />
                  <CompareItem compare={listProduct.find((product:any) => product.id === selectedOption2)} />
                  <CompareItem compare={listProduct.find((product:any) => product.id === selectedOption3)} />
                  <CompareItem compare={listProduct.find((product:any) => product.id === selectedOption4)} />
               </div>
            </div>
         </div>
      </div>
   );
};

export default DetailProductPage;

