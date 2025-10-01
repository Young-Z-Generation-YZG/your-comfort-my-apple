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

const IphoneDetails = () => {
   const [selectedModel, setSelectedModel] = useState<string>('iPhone 16');
   const [selectedColor, setSelectedColor] = useState<string>('Ultramarine');
   const [hoveredColor, setHoveredColor] = useState<string | null>(null);

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
                     <HelpItem />
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
            </div>
         </div>
      </div>
   );
};

export default IphoneDetails;
