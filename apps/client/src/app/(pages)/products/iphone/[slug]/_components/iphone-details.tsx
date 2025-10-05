/* eslint-disable react/no-unescaped-entities */
'use client';
import { useState, useEffect } from 'react';
import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
   type CarouselApi,
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
import { iphoneDetailsFakeData } from '../_data/_iphone-details';
import { ApplePickupIcon, DeliveryTruckIcon } from '@components/icon';

const resizeFromHeight = (height: number, aspectRatio: string = '16:9') => {
   const [widthRatio, heightRatio] = aspectRatio.split(':').map(Number);
   return `w_${Math.round((height * widthRatio) / heightRatio)},h_${height}`;
};

const colors = iphoneDetailsFakeData.color_items.map((color) => ({
   name: color.name,
   normalized_name: color.normalized_name,
   hex: color.hex_code,
}));

const models = iphoneDetailsFakeData.model_items.map((model) => ({
   name: model.name,
   normalized_name: model.normalized_name,
   displaySize: model.name.includes('Plus') ? '6.7' : '6.1', // Default display sizes
   price: 699, // Default price - would need to be calculated from SKUs
   monthlyPrice: '29.12', // Default monthly price - would need to be calculated
}));

const storages = iphoneDetailsFakeData.storage_items.map((storage) => ({
   name: storage.name,
   normalized_name: storage.normalized_name,
   price: 799, // Default price - would need to be calculated from SKUs
   monthlyPrice: '33.29', // Default monthly price - would need to be calculated
   note: undefined,
}));

const IphoneDetails = () => {
   const [selectedModel, setSelectedModel] = useState<{
      name: string;
      normalized_name: string;
   } | null>(null);
   const [selectedColor, setSelectedColor] = useState<{
      name: string;
      normalized_name: string;
   } | null>(null);
   const [selectedStorage, setSelectedStorage] = useState<{
      name: string;
      normalized_name: string;
   } | null>(null);
   const [hasCompletedInitialSelection, setHasCompletedInitialSelection] =
      useState(false);
   const [hoveredColor, setHoveredColor] = useState<string | null>(null);
   const [isShowMoreInfo, setIsShowMoreInfo] = useState(false);
   const [currentSlide, setCurrentSlide] = useState(0);
   const [carouselApi, setCarouselApi] = useState<CarouselApi>();

   // Progressive selection handlers
   const handleModelSelection = (model: {
      name: string;
      normalized_name: string;
   }) => {
      setSelectedModel(model);
      // Only reset dependent selections if not completed initial selection
      if (!hasCompletedInitialSelection) {
         setSelectedColor(null);
         setSelectedStorage(null);
      }
   };

   const handleColorSelection = (color: {
      name: string;
      normalized_name: string;
   }) => {
      setSelectedColor(color);
      // Only reset storage selection if not completed initial selection
      if (!hasCompletedInitialSelection) {
         setSelectedStorage(null);
      }
   };

   const handleStorageSelection = (storage: {
      name: string;
      normalized_name: string;
   }) => {
      setSelectedStorage(storage);
      // Mark initial selection as completed when all three are selected
      if (!hasCompletedInitialSelection && selectedModel && selectedColor) {
         setHasCompletedInitialSelection(true);
      }
   };

   // Check if options should be disabled (only during initial progressive selection)
   const isColorDisabled = !hasCompletedInitialSelection && !selectedModel;
   const isStorageDisabled =
      !hasCompletedInitialSelection && (!selectedModel || !selectedColor);

   // Filter branches and SKUs based on selections
   const getFilteredBranches = () => {
      if (!selectedModel || !selectedColor || !selectedStorage) {
         return [];
      }

      return iphoneDetailsFakeData.branchs
         .map((branchData) => {
            // Find the matching SKU for this branch
            const matchingSku = branchData.skus.find((sku) => {
               return (
                  sku.model.normalized_name === selectedModel.normalized_name &&
                  sku.color.normalized_name === selectedColor.normalized_name &&
                  sku.storage.normalized_name ===
                     selectedStorage.normalized_name
               );
            });

            return {
               branch: branchData.branch,
               sku: matchingSku,
            };
         })
         .filter((branchData) => branchData.sku !== undefined);
   };

   const filteredBranches = getFilteredBranches();

   const renderCheckoutBottom = () => {
      const isAllSelected = selectedModel && selectedColor && selectedStorage;

      return (
         <div className="fixed bottom-0 left-0 right-0 w-full bg-white border-t border-[#d2d2d7] z-50 shadow-[0_-2px_10px_rgba(0,0,0,0.1)]">
            <div className="max-w-[1240px] mx-auto px-6 py-4">
               {!isAllSelected ? (
                  <div className="flex flex-row items-center justify-between gap-4">
                     {/* Left: Product Summary */}
                     <div className="flex flex-col gap-1">
                        <div className="text-[14px] font-semibold text-[#1D1D1F]">
                           Configure your iPhone
                        </div>
                     </div>

                     {/* Right: Action Buttons */}
                     <div className="flex flex-row items-center gap-3">
                        <div className="text-[12px] text-[#86868B] mr-2">
                           Please select all options
                        </div>
                        <Button
                           disabled={true}
                           className="px-6 py-3 h-auto text-[15px] font-normal rounded-full bg-[#f5f5f7] text-[#86868B] cursor-not-allowed"
                        >
                           Add to Bag
                        </Button>
                     </div>
                  </div>
               ) : (
                  <div className="flex flex-col gap-4">
                     {/* Product Summary */}
                     <div className="flex flex-row items-center justify-between">
                        <div className="flex flex-col gap-1">
                           <div className="text-[14px] font-semibold text-[#1D1D1F]">
                              {selectedModel?.name} - {selectedStorage?.name} -{' '}
                              {selectedColor?.name}
                           </div>
                           <div className="text-[12px] text-[#86868B]">
                              Available in {filteredBranches.length} location
                              {filteredBranches.length !== 1 ? 's' : ''}
                           </div>
                        </div>
                     </div>

                     {/* Available Branches */}
                     {filteredBranches.length > 0 && (
                        <div className="max-h-[200px] overflow-y-auto">
                           <div className="text-[12px] font-semibold text-[#1D1D1F] mb-2">
                              Available Locations:
                           </div>
                           <div className="space-y-2">
                              {filteredBranches.map((branchData) => (
                                 <div
                                    key={branchData.branch.id}
                                    className="flex flex-row items-center justify-between p-3 rounded-lg border border-[#D2D2D7] bg-white"
                                 >
                                    <div className="flex flex-col gap-1">
                                       <div className="text-[13px] font-semibold text-[#1D1D1F]">
                                          {branchData.branch.name}
                                       </div>
                                       <div className="text-[11px] text-[#86868B]">
                                          {branchData.branch.address}
                                       </div>
                                       <div className="text-[11px] text-[#0071E3]">
                                          In Stock:{' '}
                                          {branchData.sku?.available_in_stock ||
                                             0}{' '}
                                          units
                                       </div>
                                    </div>
                                    <div className="text-right">
                                       <div className="text-[13px] font-semibold text-[#1D1D1F]">
                                          ${branchData.sku?.unit_price || 0}
                                       </div>
                                       {branchData.sku?.unit_price && (
                                          <div className="text-[11px] text-[#86868B]">
                                             $
                                             {Math.round(
                                                (branchData.sku.unit_price /
                                                   24) *
                                                   100,
                                             ) / 100}
                                             /mo. for 24 mo.
                                          </div>
                                       )}
                                    </div>
                                 </div>
                              ))}
                           </div>
                        </div>
                     )}

                     {/* Action Button */}
                     <div className="flex flex-row items-center justify-end">
                        <Button
                           className="px-6 py-3 h-auto text-[15px] font-normal rounded-full bg-[#0071E3] hover:bg-[#0077ED] text-white transition-all duration-200"
                           onClick={() => {
                              console.log('Add to Bag:', {
                                 selectedModel,
                                 selectedColor,
                                 selectedStorage,
                                 availableBranches: filteredBranches,
                              });
                           }}
                        >
                           Add to Bag
                        </Button>
                     </div>
                  </div>
               )}
            </div>
         </div>
      );
   };

   useEffect(() => {
      if (!carouselApi) return;

      const onSelect = () => {
         setCurrentSlide(carouselApi.selectedScrollSnap());
      };

      carouselApi.on('select', onSelect);
      onSelect(); // Set initial slide

      return () => {
         carouselApi.off('select', onSelect);
      };
   }, [carouselApi]);

   return (
      <div>
         {/* Product Title */}
         <div className="w-full bg-transparent flex flex-row pt-[52px] pb-32">
            <div className="basis-[70%] bg-transparent">
               <span className="text-[18px] text-[#b64400] font-semibold leading-[20px]">
                  New
               </span>
               <h1 className="text-[48px] font-semibold leading-[52px] pb-2 mb-[13px]">
                  {iphoneDetailsFakeData.name}
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
         <div className="w-full flex flex-row gap-14 relative h-[1000px]">
            {/* left */}
            <div className="basis-[70%] sticky top-[100px] self-start">
               <Carousel setApi={setCarouselApi}>
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
                     {Array.from({ length: 2 }).map((_, index) => (
                        <div
                           className="w-[10px] h-[10px] rounded-full transition-colors duration-200"
                           style={{
                              backgroundColor:
                                 currentSlide === index ? '#6b7280' : '#d1d5db',
                           }}
                           key={index}
                        />
                     ))}
                  </div>
               </Carousel>
            </div>
            {/* end left */}

            {/* Right */}
            <div className="basis-[30%] max-w-[328px]">
               <div className="flex flex-col gap-24">
                  {/* Models */}
                  <div className="w-full">
                     <div className="w-full text-center text-[24px] font-semibold leading-[28px] pb-10">
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
                              isSelected={selectedModel?.name === model.name}
                              onClick={() =>
                                 handleModelSelection({
                                    name: model.name,
                                    normalized_name: model.normalized_name,
                                 })
                              }
                              isDisabled={false}
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
                  {/* End Models */}

                  {/* Color */}
                  <div className="w-full">
                     <div className="w-full text-center text-[24px] font-semibold leading-[28px]">
                        <span className="text-[#1D1D1F]">Finish. </span>
                        <span className="text-[#86868B]">
                           Pick your favorite.
                        </span>
                     </div>

                     <div className="pt-5 pb-[17px] text-[17px] font-semibold leading-[25px] tracking-[-0.02em] text-[#1D1D1F]">
                        {isColorDisabled
                           ? 'Color (Select a model first)'
                           : hoveredColor
                             ? `Color - ${hoveredColor}`
                             : selectedColor
                               ? `Color - ${selectedColor.name}`
                               : 'Color'}
                     </div>

                     <div className="flex flex-row gap-[12px] items-center">
                        {colors.map((color) => (
                           <ColorItem
                              key={color.name}
                              colorHex={color.hex}
                              colorName={color.name}
                              isSelected={selectedColor?.name === color.name}
                              onClick={() =>
                                 !isColorDisabled &&
                                 handleColorSelection({
                                    name: color.name,
                                    normalized_name: color.normalized_name,
                                 })
                              }
                              onMouseEnter={() =>
                                 !isColorDisabled && setHoveredColor(color.name)
                              }
                              onMouseLeave={() => setHoveredColor(null)}
                              isDisabled={isColorDisabled}
                           />
                        ))}
                     </div>
                  </div>
                  {/* End Color */}

                  {/* Storage */}
                  <div className="w-full">
                     <div className="w-full text-start text-[24px] font-semibold leading-[28px] pb-10">
                        <span className="text-[#1D1D1F]">Storage. </span>
                        <span className="text-[#86868B]">
                           {isStorageDisabled
                              ? 'Select model and color first'
                              : 'How much space do you need?'}
                        </span>
                     </div>

                     <div className="w-full flex flex-col gap-2">
                        {storages.map((storage) => (
                           <StorageItem
                              key={storage.name}
                              storageName={storage.name}
                              price={storage.price}
                              monthlyPrice={storage.monthlyPrice}
                              note={storage.note}
                              isSelected={
                                 selectedStorage?.name === storage.name
                              }
                              onClick={() =>
                                 !isStorageDisabled &&
                                 handleStorageSelection({
                                    name: storage.name,
                                    normalized_name: storage.normalized_name,
                                 })
                              }
                              isDisabled={isStorageDisabled}
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
                  {/* End Storage */}
               </div>
            </div>
         </div>

         {/* Coverage */}
         <div
            className={cn(
               'w-full bg-transparent flex flex-col mt-[200px] h-fit',
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
                        {iphoneDetailsFakeData.name}.
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
                           {selectedModel?.name} {selectedStorage?.name}{' '}
                           {selectedColor?.name}
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
                     <DeliveryTruckIcon
                        width={25}
                        height={25}
                        className="mr-2"
                     />
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
                     <ApplePickupIcon width={25} height={25} className="mr-2" />
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
                           {iphoneDetailsFakeData.name}
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

         <div className="mx-auto mt-20 mb-24">
            <CompareIPhoneSection />
         </div>

         {/* Checkout Bottom Bar */}
         {renderCheckoutBottom()}
      </div>
   );
};

export default IphoneDetails;
