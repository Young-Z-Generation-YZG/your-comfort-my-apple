/* eslint-disable react/react-in-jsx-scope */
'use client';
import { useEffect, useState } from 'react';
// import { useGetUsersQuery } from '~/services/example/user.service';
import { motion } from "framer-motion"
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import '@Client/globals.css';
import Image from 'next/image';

import { Carousel, CarouselContent, CarouselItem, CarouselNext, CarouselPrevious } from '~/components/ui/carousel';
import { Select, SelectContent, SelectTrigger, SelectValue, SelectItem } from '~/components/ui/select';

import LastestItem from './_component/LastestItem';
import ExperienceItem from './_component/ExperienceItem';
import CompareItem from './_component/CompareItem';
import { Button } from '~/components/ui/button';

const listLatestItem = [
    {
       id: 1,
       checkPreOrder: false,
       title: 'iPhone 16 Pro',
       subtitle: 'Apple Intelligence',
       price: 'From $999 or $41.62/mo. for 24 mo.*',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-iphone-16-pro-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1726165763260',
       checkLightImg: false,
    },
    {
       id: 2,
       checkPreOrder: true,
       title: 'iPad mini',
       subtitle: 'Apple Intelligence',
       price: 'From $499 or $41.58/mo. for 12 mo.*',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-mini-202410?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1727814498187',
       checkLightImg: true,
    },
    {
       id: 3,
       checkPreOrder: false,
       title: 'Apple Watch Series 10',
       subtitle: 'Thinstant classic.',
       price: 'From $399 or $33.25/mo. for 12 mo.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-watch-s10-202409?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1724095131742',
       checkLightImg: true,
    },
    {
       id: 4,
       checkPreOrder: false,
       title: 'iPhone 16',
       subtitle: 'Apple Intelligence',
       price: 'From $799 or $33.29/mo. for 24 mo.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-iphone-16-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1725661572513',
       checkLightImg: false,
    },
    {
       id: 5,
       checkPreOrder: false,
       title: 'AirPods 4',
       subtitle: 'Iconic. Now supersonic.',
       price: 'Starting at $129 with Active Noise Cancellation $179',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-airpods-202409?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1722974321259',
       checkLightImg: true,
    },
    {
       id: 6,
       checkPreOrder: false,
       title: 'Apple Watch Ultra 2',
       subtitle: 'New finish. Never quit.',
       price: 'From $799 or $66.58/mo. for 12 mo.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-watch-ultra-202409_GEO_US?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1725655432734',
       checkLightImg: false,
    },
    {
       id: 7,
       checkPreOrder: false,
       title: 'Apple Vision Pro',
       subtitle: 'Welcome to spatial computing.',
       price: 'From $3499 or $291.58/mo. for 12 mo.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-vision-pro-202401?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1705097770616',
       checkLightImg: true,
    },
    {
       id: 8,
       checkPreOrder: false,
       title: 'iPad Air',
       subtitle: 'Apple Intelligence',
       price: 'From $599 or $49.91/mo. for 12 mo.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-air-202405?wid=800&hei=1000&fmt=jpeg&qlt=90&.v=1713308272877',
       checkLightImg: true,
    },
    {
       id: 9,
       checkPreOrder: false,
       title: 'MacBook Pro',
       subtitle: 'Mind-blowing. Head-turning.',
       price: 'From $1599 or $133.25/mo. for 12 mo.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-macbook-pro-202310?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1696964122967',
       checkLightImg: true,
    },
    {
       id: 10,
       checkPreOrder: false,
       title: 'iPad Pro',
       subtitle: 'Apple Intelligence',
       price: 'From $999 or $83.25/mo. for 12 mo.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-ipad-pro-202405?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1713308272816',
       checkLightImg: false,
    },
    {
       id: 11,
       checkPreOrder: false,
       title: 'MacBook Air',
       subtitle: 'Designed to go places.',
       price: 'From $999 or $83.25/mo. for 12 mo.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-40-macbook-air-202402?wid=800&hei=1000&fmt=p-jpg&qlt=95&.v=1707259289595',
       checkLightImg: true,
    },
]

const listExperienceItem = [
    {
       id: 1,
       subtitle: 'Apple TV+',
       title: 'Watch new Apple Originals every month.**',
       content: '',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-tv-services-202409?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1727220094622',
       checkLightImg: true,
    },
    {
       id: 2,
       subtitle: '',
       title: 'Six Apple services. One easy subscription.',
       content: '',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-subscriptions-202108_GEO_US?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1626375546000',
       checkLightImg: true,
    },
    {
       id: 3,
       subtitle: '',
       title: "We've got you covered.",
       content: 'AppleCare+ now comes with unlimited repairs for accidental damage protection.',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-applecare-202409?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1723747544269',
       checkLightImg: true,
    },
    {
       id: 4,
       subtitle: '',
       title: 'Discover all the ways to use Apple Pay.',
       content: '',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-applepay-202409_GEO_US?wid=960&hei=1000&fmt=jpeg&qlt=90&.v=1725374577628',
       checkLightImg: true,
    },
    {
       id: 5,
       subtitle: 'home',
       title: 'See how one app can control your entire home.',
       content: '',
       img: 'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/store-card-50-homekit-202405_GEO_US?wid=960&hei=1000&fmt=p-jpg&qlt=95&.v=1715960298510',
       checkLightImg: true,
    }
]

const listProduct = [
    {
       id: 'ip14proMax',
       checkNew: false,
       name: 'iPhone 14 Pro Max',
       image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_14_pro_max_twilight_purple_large.png?1677709034078',
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
       image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_14_plus_blue_large.png?1677709034078',
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
       image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_13_pro_max_alpine_green_large.png?1677709034080',
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
       image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_13_mini_green_large.png?1677709034086',
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
       image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_12_mini_purple_large.png?1677709034084',
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
       image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_xs_max_silver_large.png?1677709034078',
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

const HomePage = () => {
   // const { data } = useGetUsersQuery('');

   // useEffect(() => {
   //    console.log(data);
   // }, [data]);

    const [selectedOption1, setSelectedOption1] = useState('ip14proMax');
    const [selectedOption2, setSelectedOption2] = useState('ip14plus');
    const [selectedOption3, setSelectedOption3] = useState('ip13proMax');

    const [showDetailCompare, setShowDetailCompare] = useState(false);
   
    return (
      <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay w-full flex flex-col items-center justify-center bg-[#f5f5f7]')}>
      <div className='hero-container max-w-[1264px] h-screen px-8 mb-[100px] mx-auto flex flex-row'>
         {/* <div className='hero-gradient bg-gray-300 h-[100px] w-full absolute top-0 bottom-0 left-0 right-0 z-0'></div> */}
         <div className='hero-content basis-1/2 h-full bg-transparent relative z-10 flex flex-col items-start gap-[30px]'>
            <div className='content-header max-w-[600px] mt-8 text-[94px] font-medium leading-[97px] tracking-[-3.76px] text-[#3a3a3a]'>Financial infrastructure to grow your revenue</div>
            <div className='content-body max-w-[540px] pl-2 pr-12 text-[18px] font-normal leading-[28px] tracking-[0.2px] text-[#425466]'>
               Join the millions of companies of all sizes that use Stripe to accept   
               payments online and in person, embed financial services, power custom 
               revenue models, and build a more profitable business.
            </div>
            {/* <div className='content-form pl-2'>
               <div className='flex flex-row gap-2 border border-black rounded-[10px]'>
                  <Input type='text' placeholder='Email address' className='rounded-[10px]'/>
                  <Button variant="outline" className='bg-black text-white rounded-[10px]'>Start now</Button> 
               </div>
            </div> */}
         </div>
         <div className='hero-masonry basis-1/2 h-full bg-transparent relative z-10'> 
            <div className='bg-transparent h-full w-full absolute z-10'>
               <div className='bg-[linear-gradient(0deg,rgba(1,0,2,0),#f5f5f7)] h-10 w-full absolute top-0'></div>
               <div className='bg-[linear-gradient(180deg,rgba(1,0,2,0),#f5f5f7)] h-10 w-full absolute bottom-0'></div>
            </div>
            <div className="overflow-hidden h-full relative z-0 px-1">
               <motion.div 
                  animate={{ y: ["0px", "-3384px"] }}
                  // animate={{ y: ["-3384px", "-3384px"] }}
                  // animate={{ y: ["0px", "0px"] }}
                  transition={{
                     y: {
                        duration: 60,
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
      <div className='list-item-container w-full mb-[100px] flex flex-col justify-center items-center'>
         <div className='lastest-item-container w-full max-w-screen mb-[50px]'>
            <div className='title-list pl-[140px] pb-[24px]'>
               <span className='text-[28px] font-semibold text-[#1d1d1f] leading-[1.14] tracking-[0.007em]'>The latest. </span>
               <span className='text-[28px] font-semibold text-[#6e6e73] leading-[1.14] tracking-[0.007em]'>Take a look at what’s new, right now.</span>
            </div>
            <Carousel className="w-full max-w-screen relative flex justify-center items-center">
               <CarouselContent className="w-screen pl-[140px] mb-5">
                  {listLatestItem.map((product, index) => {
                     return (
                        <CarouselItem key={index} className="md:basis-1/3 mr-[20px]">
                              <LastestItem product={product} />
                        </CarouselItem>
                     );
                  })}
               </CarouselContent>
               {/* <CarouselPrevious className='absolute top-1/2 -translate-y-1/2 left-0 '/>
               <CarouselNext className='absolute top-1/2 -translate-y-1/2 right-0'/> */}
               <CarouselPrevious className='absolute -top-[40px] -translate-y-1/2 left-[90%] border-black bg-[#ccc]'/>
               <CarouselNext className='absolute -top-[40px] -translate-y-1/2 right-[5%] border-black bg-[#ccc]'/>
            </Carousel>
         </div>

         <div className='experience-container w-full max-w-screen'>
            <div className='title-list pl-[140px] pb-[24px]'>
               <span className='text-[28px] font-semibold text-[#1d1d1f] leading-[1.14] tracking-[0.007em]'>The Apple experience. </span>
               <span className='text-[28px] font-semibold text-[#6e6e73] leading-[1.14] tracking-[0.007em]'>Do even more with Apple products and services.</span>
            </div>
            <Carousel className="w-full max-w-screen relative flex justify-center items-center">
               <CarouselContent className="w-screen pl-[140px] mb-5">
                  {listExperienceItem.map((item, index) => {
                     return (
                        <CarouselItem key={index} className="md:basis-1/3 mr-[20px]">
                           <ExperienceItem experience={item} />
                        </CarouselItem>
                     );
                  })}
               </CarouselContent>
               <CarouselPrevious className='absolute -top-[40px] -translate-y-1/2 left-[90%] border-black bg-[#ccc]'/>
               <CarouselNext className='absolute -top-[40px] -translate-y-1/2 right-[5%] border-black bg-[#ccc]'/>
            </Carousel>
         </div>
      </div>
      <div className='compare-container w-full mb-[100px] flex flex-col justify-center items-center'>
         <div className='compare-title bg-transparent w-[996px] mx-auto text-center pb-[70px]'>
            <div className='text-[20px] font-bold'>Compare</div>    
            <div className='text-[40px] font-bold'>Which iPhone is right for you?</div>    
         </div>
         <div className='compare-content bg-transparent w-[996px]  mx-auto flex flex-col'> 
            <div className='w-full flex flex-row gap-3 mb-10'>
               <div className='basis-1/3 bg-gray-300 flex flex-col items-center rounded-[10px]'>
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
                  {/* <SelectItem value="ip14proMax">iPhone 14 Pro Max</SelectItem>
                        <SelectItem value="ip14pro">iPhone 14 Pro</SelectItem>
                        <SelectItem value="ip14plus">iPhone 14 Plus</SelectItem>
                        <SelectItem value="ip14">iPhone 14</SelectItem>
                        <SelectItem value="ipse">iPhone SE</SelectItem> */}
               </div>
               <div className='basis-1/3  bg-gray-300 flex flex-col items-center rounded-[10px]'>
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
               <div className='basis-1/3  bg-gray-300 flex flex-col items-center rounded-[10px]'>
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
            </div>
            <div className={cn('w-full flex flex-row gap-3 ', showDetailCompare?'h-fit':'h-[1250px] overflow-hidden')}>
               <CompareItem compare={listProduct.find((product:any) => product.id === selectedOption1)} />
               <CompareItem compare={listProduct.find((product:any) => product.id === selectedOption2)} />
               <CompareItem compare={listProduct.find((product:any) => product.id === selectedOption3)} />
            </div>
            <div className={cn('w-full flex items-center justify-center mt-10')}>
               <Button onClick={()=>setShowDetailCompare(!showDetailCompare)} className={cn('w-[140px] bg-black text-white uppercase px-10 text-xl hover:bg-[#333]')} variant={'default'}>
                  {showDetailCompare?'Ẩn bớt':'Xem thêm'}
               </Button>
            </div>
         </div>
      </div>
   </div>
    );
}

export default HomePage;