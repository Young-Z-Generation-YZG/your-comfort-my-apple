/* eslint-disable react/no-unescaped-entities */
'use client';
import { useState } from 'react';
import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '~/components/ui/carousel';
import NextImage from 'next/image';
import ModelItem from './model-item';
import HelpItem from './help-item';
import ColorItem from './color-item';
import StorageItem from './storage-item';
import { cn } from '~/infrastructure/lib/utils';
import { Button } from '@components/ui/button';
import images from '@components/client/images';
import Link from 'next/link';
import Image from 'next/image';
import { MdOutlineArrowDropDown, MdArrowDropUp } from 'react-icons/md';
import CompareIPhoneSection from '@components/client/compare-iphone-section';
import AppleCareCard from './applecare-card';
import ServiceCard from './service-card';
import { appleCareOptions } from '../_constants/applecare-data';
import { appleServices } from '../_constants/services-data';

const resizeFromHeight = (height: number, aspectRatio: string = '16:9') => {
   const [widthRatio, heightRatio] = aspectRatio.split(':').map(Number);
   return `w_${Math.round((height * widthRatio) / heightRatio)},h_${height}`;
};

const colors = [
   { name: 'Ultramarine', hex: '#41679A' },
   { name: 'Teal', hex: '#9DB4AA' },
   { name: 'Pink', hex: '#F4C2D0' },
   { name: 'White', hex: '#F5F5F0' },
   { name: 'Black', hex: '#2C2C2E' },
];

const models = [
   {
      name: 'iPhone 16',
      displaySize: '6.1',
      price: 699,
      monthlyPrice: '29.12',
   },
   {
      name: 'iPhone 16 Plus',
      displaySize: '6.7',
      price: 799,
      monthlyPrice: '33.29',
   },
];

const storageOptions = [
   {
      name: '128GB',
      price: 799,
      monthlyPrice: '33.29',
      note: undefined,
   },
   {
      name: '256GB',
      price: 899,
      monthlyPrice: '37.45',
      note: 'Only available with iPhone 16 Plus',
   },
   {
      name: '512GB',
      price: 1099,
      monthlyPrice: '45.79',
      note: undefined,
   },
];

const IphoneDetails = () => {
   const [selectedModel, setSelectedModel] = useState<string>('iPhone 16');
   const [selectedColor, setSelectedColor] = useState<string>('Ultramarine');
   const [selectedStorage, setSelectedStorage] = useState<string>('128GB');
   const [hoveredColor, setHoveredColor] = useState<string | null>(null);
   const [isShowMoreInfo, setIsShowMoreInfo] = useState(false);

   return (
      <div>
         {/* Product Title */}
         <div className="w-full bg-transparent flex flex-row pt-[52px] pb-32">
            <div className="basis-[70%] bg-transparent">
               <span className="text-[18px] text-[#b64400] font-semibold leading-[20px]">
                  New
               </span>
               <h1 className="text-[48px] font-semibold leading-[52px] pb-2 mb-[13px]">
                  Iphone 16 Pro
               </h1>
               <div className="text-[15px] font-light leading-[20px]">
                  From $999 or $41.62/mo. for 24 mo.
               </div>
            </div>
            <div className="basis-[30%] bg-transparent">
               <div className="h-full flex flex-col mt-1">
                  <div className="w-full basis-1/2 flex flex-row justify-end">
                     <div className="w-fit text-[13px] font-light leading-[16px] tracking-[0.7px] px-[16px] py-[12px] my-[6px] flex items-center bg-[#f5f5f7] rounded-full">
                        Get $40–$650 for your trade-in.
                     </div>
                  </div>
                  <div className="w-full basis-1/2 flex flex-row justify-end">
                     <div className="w-fit text-[13px] font-light leading-[16px] tracking-[0.7px] px-[16px] py-[12px] my-[6px] flex items-center bg-[#f5f5f7] rounded-full">
                        Get 3% Daily Cash back with Apple Card.
                     </div>
                  </div>
               </div>
            </div>
         </div>

         {/* Product Details */}
         <div className="w-full flex flex-row gap-4 relative h-[1000px]">
            {/* Left */}
            <div className="basis-[70%] sticky top-[100px] self-start">
               <Carousel>
                  <CarouselContent>
                     <CarouselItem>
                        <div className="w-full overflow-hidden relative h-[1000px]">
                           <NextImage
                              src={`https://res.cloudinary.com/delkyrtji/image/upload/${resizeFromHeight(1000, '16:9')}/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp`}
                              alt="promotion-iPhone"
                              width={Math.round((1000 * 16) / 9)}
                              height={1000}
                              // width={500}
                              // height={Math.round((500 * 9) / 16)}
                              className="absolute top-0 left-0 w-full h-full object-cover rounded-[20px]"
                           />
                        </div>
                     </CarouselItem>
                     <CarouselItem>
                        <div className="w-full overflow-hidden relative h-[1000px]">
                           <NextImage
                              src={`https://res.cloudinary.com/delkyrtji/image/upload/${resizeFromHeight(1000, '16:9')}/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp`}
                              alt="promotion-iPhone"
                              width={Math.round((1000 * 16) / 9)}
                              height={1000}
                              // width={500}
                              // height={Math.round((500 * 9) / 16)}
                              className="absolute top-0 left-0 w-full h-full object-cover rounded-[20px]"
                           />
                        </div>
                     </CarouselItem>
                  </CarouselContent>

                  <CarouselPrevious className="left-[1rem]" />
                  <CarouselNext className="right-[1rem]" />

                  <div className="absolute bottom-2 left-0 w-full z-50 flex flex-row items-center justify-center gap-2">
                     {/* Slide {current} of {count} */}
                     {Array.from({ length: 2 }).map((_, index) => (
                        <div
                           className="w-[10px] h-[10px] rounded-full"
                           style={{
                              backgroundColor:
                                 1 === index + 1 ? '#6b7280' : '#d1d5db',
                           }}
                           key={index}
                        />
                     ))}
                  </div>
               </Carousel>
            </div>

            {/* Right */}
            <div className="basis-[30%] flex flex-col gap-24">
               {/* Models */}
               <div className="w-full">
                  <div className="w-full text-center text-[24px] font-semibold leading-[28px] pb-[13px]">
                     <span className="text-[#1D1D1F]">Model. </span>
                     <span className="text-[#86868B]">
                        Which is best for you?
                     </span>
                  </div>

                  <div className="w-full flex flex-col gap-2">
                     {models.map((model) => (
                        <ModelItem
                           key={model.name}
                           modelName={model.name}
                           displaySize={model.displaySize}
                           price={model.price}
                           monthlyPrice={model.monthlyPrice}
                           isSelected={selectedModel === model.name}
                           onClick={() => setSelectedModel(model.name)}
                        />
                     ))}
                  </div>

                  <div className="mt-[20px] mb-[6px]">
                     <HelpItem
                        title="Need help choosing a model?"
                        subTitle="Explore the differences in screen size and battery life."
                     />
                  </div>
               </div>

               {/* Color */}
               <div className="w-full">
                  <div className="w-full text-center text-[24px] font-semibold leading-[28px] pb-[13px]">
                     <span className="text-[#1D1D1F]">Finish. </span>
                     <span className="text-[#86868B]">Pick your favorite.</span>
                  </div>

                  <div className="pt-5 pb-[17px] text-[17px] font-semibold leading-[25px] tracking-[-0.02em] text-[#1D1D1F]">
                     {hoveredColor ? `Color - ${hoveredColor}` : 'Color'}
                  </div>

                  <div className="flex flex-row gap-[12px] items-center">
                     {colors.map((color) => (
                        <ColorItem
                           key={color.name}
                           colorHex={color.hex}
                           colorName={color.name}
                           isSelected={selectedColor === color.name}
                           onClick={() => setSelectedColor(color.name)}
                           onMouseEnter={() => setHoveredColor(color.name)}
                           onMouseLeave={() => setHoveredColor(null)}
                        />
                     ))}
                  </div>
               </div>

               {/* Storage */}
               <div className="w-full">
                  <div className="w-full text-center text-[24px] font-semibold leading-[28px] pb-[13px]">
                     <span className="text-[#1D1D1F]">Storage. </span>
                     <span className="text-[#86868B]">
                        How much space do you need?
                     </span>
                  </div>

                  <div className="w-full flex flex-col gap-2">
                     {storageOptions.map((storage) => (
                        <StorageItem
                           key={storage.name}
                           storageName={storage.name}
                           price={storage.price}
                           monthlyPrice={storage.monthlyPrice}
                           note={storage.note}
                           isSelected={selectedStorage === storage.name}
                           onClick={() => setSelectedStorage(storage.name)}
                        />
                     ))}
                  </div>

                  <div className="mt-[20px] mb-[6px]">
                     <HelpItem
                        title="Not sure how much storage to get?"
                        subTitle="Get a better understanding of how much space you’ll need."
                     />
                  </div>
               </div>
            </div>
         </div>

         {/* Coverage */}
         <div
            className={cn(
               'w-full bg-transparent flex flex-col mt-[74px] h-fit',
            )}
         >
            <div className="coverage-title flex flex-row">
               <div className="basis-3/4">
                  <div className="w-full text-[24px] font-semibold leading-[28px]">
                     <span className="text-[#1D1D1F] tracking-[0.3px]">
                        AppleCare+ coverage.{' '}
                     </span>
                     <span className="text-[#86868B] tracking-[0.3px]">
                        Protect your new iPhone.
                     </span>
                  </div>
               </div>
               <div className="basis-1/4 min-w-[328px]"></div>
            </div>
            <div className="coverage-items flex flex-row mt-6">
               <div className="basis-3/4 list-items w-full grid grid-cols-3 gap-4 pr-12">
                  {appleCareOptions.map((option, index) => (
                     <AppleCareCard
                        key={index}
                        title={option.title}
                        price={option.price}
                        monthlyPrice={option.monthlyPrice}
                        features={option.features}
                     />
                  ))}
               </div>
               <div className="basis-1/4 min-w-[328px]">
                  <HelpItem
                     title="What kind of protection do you need?"
                     subTitle="Compare the additional features and coverage of the two AppleCare+ plans."
                  />
               </div>
            </div>
         </div>
         {/* End Coverage */}

         {/* Product info */}
         <div
            className={cn(
               'w-full mt-[73px] bg-[#f5f5f7] flex flex-col items-center overflow-hidden relative',
               {
                  'h-[550px]': !isShowMoreInfo,
               },
            )}
         >
            <div
               id="info"
               className="min-w-[980px] max-w-[1240px] w-[87.5%] mx-auto mt-[39px] flex flex-row gap-0"
            >
               <div className="basis-[34.76%]">
                  <div className="pr-[10px] mr-[41px] flex flex-col items-start justify-start">
                     <span className="text-[#1D1D1F] tracking-[0.3px] w-full text-[38px] font-semibold leading-[46px]">
                        Your new <br />
                        iPhone 16 Pro.
                     </span>
                     <span className="text-[#86868B] tracking-[0.3px] w-full text-[38px] font-semibold leading-[46px]">
                        Just the way you <br />
                        want it.
                     </span>
                     <Image
                        src={images.ip16ProWhiteTitaniumSelect}
                        className="w-full h-auto pt-[42px]"
                        width={2000}
                        height={2000}
                        quality={100}
                        alt=""
                     />
                  </div>
               </div>
               <div className="basis-[38.9%]">
                  <div className="pr-[20px] mr-[41px] flex flex-col items-start justify-start">
                     <div className="product-info w-full pb-11 text-[17px] text-[#1D1D1F] leading-[25px] tracking-[0.3px]">
                        <div className="w-full pb-[7px] font-light">
                           iPhone 16 Pro 512GB White Titanium
                        </div>
                        <div className="w-full font-semibold">$1,299.00</div>
                        <div className="w-full pt-[35px] font-semibold">
                           One-time payment
                        </div>
                        <div className="w-full pt-[14px] text-[14px] font-light">
                           Get 3% Daily Cash with Apple Card
                        </div>
                     </div>
                     <div className="save-info w-full py-[21px] opacity-50 border-t-[0.8px] border-[#86868b] text-[14px] text-[#1D1D1F] leading-[20px] tracking-[0.3px]">
                        <div className="w-full font-semibold">
                           Need a moment?
                        </div>
                        <div className="w-full font-light pr-5 pb-2">
                           Keep all your selections by saving this device to
                           Your Saves, then come back anytime and pick up right
                           where you left off.
                        </div>
                        <Link
                           className="w-full font-light flex flex-row text-[#06c]"
                           href="#"
                        >
                           <svg
                              width="21"
                              height="21"
                              className="as-svgicon as-svgicon-bookmark as-svgicon-tiny as-svgicon-bookmarktiny"
                              role="img"
                              aria-hidden="true"
                           >
                              <path fill="none" d="M0 0h21v21H0z"></path>
                              <path
                                 fill="#06c"
                                 d="M12.8 4.25a1.202 1.202 0 0 1 1.2 1.2v10.818l-2.738-2.71a1.085 1.085 0 0 0-1.524 0L7 16.269V5.45a1.202 1.202 0 0 1 
                              1.2-1.2h4.6m0-1H8.2A2.2 2.2 0 0 0 6 5.45v11.588a.768.768 0 0 0 .166.522.573.573 0 0 0 .455.19.644.644 0 0 0 .38-.128 5.008 5.008 0 0 0 
                              .524-.467l2.916-2.885a.084.084 0 0 1 .118 0l2.916 2.886a6.364 6.364 0 0 0 .52.463.628.628 0 0 0 .384.131.573.573 0 0 0 .456-.19.768.768 0 0 0 
                              .165-.522V5.45a2.2 2.2 0 0 0-2.2-2.2Z"
                              />
                           </svg>
                           Save for later
                        </Link>
                     </div>
                  </div>
               </div>
               <div className="basis-[26.34%]">
                  <div className="flex flex-row">
                     <svg
                        width="25"
                        height="25"
                        viewBox="0 0 25 25"
                        className="icon mr-2"
                     >
                        <path
                           d="M23.4824,12.8467,20.5615,9.6382A1.947,1.947,0,0,0,18.9863,9H17V6.495a2.5,2.5,0,0,0-2.5-2.5H3.5A2.5,2.5,0,0,0,1,6.495v9.75a2.5,2.5,0,0,0,2.5,
                           2.5h.5479A2.7457,2.7457,0,0,0,6.75,21.02,2.6183,2.6183,0,0,0,9.4222,19H16.103a2.7445,2.7445,0,0,0,5.3467-.23h.7349A1.6564,1.6564,0,0,0,24,
                           16.9805V14.1724A1.9371,1.9371,0,0,0,23.4824,12.8467ZM8.4263,18.745a1.7394,1.7394,0,0,1-3.3526,0,1.5773,1.5773,0,0,1,.0157-1,1.7382,1.7382,0,0,1,3.3213,
                           0,1.5782,1.5782,0,0,1,.0156,1ZM9.447,18a2.7258,2.7258,0,0,0-5.394-.255H3.5a1.5016,1.5016,0,0,1-1.5-1.5V6.495a1.5017,1.5017,0,0,1,1.5-1.5h11a1.5016,1.5016,
                           0,0,1,1.5,1.5V18Zm10.9715.77a1.7385,1.7385,0,0,1-3.3369,0,1.5727,1.5727,0,0,1,0-1,1.742,1.742,0,1,1,3.3369,1ZM23,16.9805c0,.5684-.2285.79-.8154.79H21.45A2.73,
                           2.73,0,0,0,17,16.165V10h1.9863a.9758.9758,0,0,1,.8379.3135l2.9268,3.2148a.95.95,0,0,1,.249.6441ZM21.6762,13.62A.5117.5117,0,0,1,21.85,14H18.5435A.499.499,0,
                           0,1,18,13.4718V11h1.0725a.7592.7592,0,0,1,.594.2676Z"
                           fill="#1d1d1f"
                        ></path>
                     </svg>
                     <div className="flex flex-col text-[15px] text-[#1D1D1F] font-light leading-[25px] tracking-[0.3px]">
                        <div className="font-semibold">Delivery:</div>
                        <div>In Stock</div>
                        <div>Free Shipping</div>
                        <Link href="#" className="text-[#06c]">
                           Get delivery dates
                        </Link>
                     </div>
                  </div>
                  <div className="flex flex-row mt-4">
                     <svg
                        width="25"
                        height="25"
                        viewBox="0 0 25 25"
                        className="icon mr-2"
                     >
                        <path d="m0 0h25v25h-25z" fill="none"></path>
                        <path
                           d="m18.5 5.0005h-1.7755c-.1332-2.2255-1.967-4.0005-4.2245-4.0005s-4.0913 1.775-4.2245 4.0005h-1.7755c-1.3789 0-2.5 1.1216-2.5 
                           2.5v11c0 1.3784 1.1211 2.4995 2.5 2.4995h12c1.3789 0 2.5-1.1211 2.5-2.4995v-11c0-1.3784-1.1211-2.5-2.5-2.5zm-6-3.0005c1.7058 0 3.0935 1.3264 
                           3.2245 3.0005h-6.449c.131-1.6741 1.5187-3.0005 3.2245-3.0005zm7.5 16.5005c0 .8271-.6729 1.5-1.5 1.5h-12c-.8271 0-1.5-.6729-1.5-1.5v-11c0-.8271.6729-1.5005 
                           1.5-1.5005h12c.8271 0 1.5.6734 1.5 1.5005zm-4.8633-7.5066c-.0377.0378-.7383.4304-.7383 1.3289 0 1.0344.8965 1.3968.9266 1.4044 0 
                           .0227-.1356.5059-.4746.9891-.2938.4228-.6177.8532-1.0848.8532-.4746 0-.5876-.2794-1.1375-.2794-.5273 0-.7157.2869-1.1451.2869-.4369 
                           0-.7383-.3926-1.0848-.8834-.3917-.5663-.7232-1.4572-.7232-2.3028 0-1.3515.8814-2.0688 1.7402-2.0688.4671 0 .8437.302 1.13.302.2787 0 .7006-.3171 
                           1.2204-.3171.2034-.0001.9115.015 1.3711.687zm-2.5538-.7626c-.0377 0-.0678-.0076-.0979-.0076 0-.0227-.0075-.0755-.0075-.1284 0-.3624.1883-.7097.3842-.9438.2486-.2945.6629-.521 
                           1.017-.5285.0075.0378.0075.0831.0075.1359 0 .3624-.1507.7097-.3616.974-.2336.287-.6253.4984-.9417.4984z"
                           fill="#1d1d1f"
                        ></path>
                     </svg>
                     <div className="flex flex-col text-[15px] text-[#1D1D1F] font-light leading-[25px] tracking-[0.3px]">
                        <div className="font-semibold">Pickup:</div>
                        <Link href="#" className="text-[#06c]">
                           Check availability
                        </Link>
                     </div>
                  </div>

                  <div className="button mt-12" onClick={() => {}}>
                     <Button className="w-full bg-[#06c] text-white text-[15px] font-light leading-[18px] tracking-[0.8px] h-fit py-3">
                        Continue
                     </Button>
                  </div>
               </div>
            </div>
            <div
               className={cn('w-full bg-[#fff]', {
                  invisible: !isShowMoreInfo,
               })}
            >
               <div className="w-[980px] mx-auto">
                  <div className="w-full pt-16 pb-[41px] text-[40px] font-semibold leading-[44x] tracking-[0.5px] text-center">
                     What’s in the Box
                  </div>
                  <div className="w-full flex flex-col gap-0 justify-center items-center">
                     <div className="w-full bg-[#fafafc] flex flex-row gap-0 justify-center items-center">
                        <div className="basis-[25%]">
                           <Image
                              src={images.ip16ProWhiteTitaniumWitb}
                              className="w-auto h-[339px] mx-auto"
                              width={2000}
                              height={2000}
                              quality={100}
                              alt=""
                           />
                        </div>
                        <div className="basis-[25%]">
                           <Image
                              src={images.ip16ProBraidedCable}
                              className="w-auto h-[339px] mx-auto"
                              width={2000}
                              height={2000}
                              quality={100}
                              alt=""
                           />
                        </div>
                     </div>
                     <div className="w-full flex flex-row gap-0 justify-center items-center">
                        <div className="basis-[25%] pt-5 pb-[50px] text-center text-[14px] font-light leading-[20px] tracking-[0.3px]">
                           iPhone 16 Pro
                        </div>
                        <div className="basis-[25%] pt-5 pb-[50px] text-center text-[14px] font-light leading-[20px] tracking-[0.3px]">
                           USB-C Charge Cable
                        </div>
                     </div>
                  </div>
               </div>
            </div>
            <div className="w-full bg-[#fff] pb-10">
               <div className="w-[980px] mx-auto">
                  <div className="w-fit pt-12 pb-5 px-5 text-[40px] mx-auto font-semibold leading-[44px] tracking-[0.5px] text-center">
                     Your new iPhone comes
                     <br /> with so much more.
                  </div>
                  <div className="w-full pt-9 pb-[54px] px-4 mt-[13px] flex flex-row gap-8 justify-center items-center">
                     {appleServices.map((service, index) => (
                        <ServiceCard
                           key={index}
                           icon={service.icon}
                           title={service.title}
                           description={service.description}
                        />
                     ))}
                  </div>
               </div>
            </div>

            <Button
               className="button-more absolute bottom-12 left-1/2 -translate-x-1/2 bg-transparent font-thin text-xl hover:bg-transparent border-t-0 border-l-0 border-r-0 text-blue-500 border-b border-b-blue-500 rounded-none hover:text-blue-600 py-0 h-fit"
               variant="outline"
               onClick={() => {
                  setIsShowMoreInfo(!isShowMoreInfo);
               }}
            >
               {isShowMoreInfo ? 'Collapse' : 'Expand'}
               {isShowMoreInfo ? <MdArrowDropUp /> : <MdOutlineArrowDropDown />}
            </Button>
         </div>
         {/* End Product info */}

         <div className="mx-auto mt-20">
            <CompareIPhoneSection />
         </div>
      </div>
   );
};

export default IphoneDetails;
