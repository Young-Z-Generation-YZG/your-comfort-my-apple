'use client';

import { useState, useEffect, useMemo } from 'react';
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
import { Button } from '@components/ui/button';
import {
   iphoneDetailsFakeData,
   TColorItem,
   TModelItem,
   TStorageItem,
} from '../_data/fake-data';
import useCatalogService from '@components/hooks/api/use-catalog-service';
import { useParams } from 'next/navigation';
import useBasketService from '@components/hooks/api/use-basket-service';
import { toast } from 'sonner';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { useAppSelector } from '~/infrastructure/redux/store';
import useCartSync from '@components/hooks/use-cart-sync';
import { TCartItem } from '~/infrastructure/services/basket.service';
import { TIphoneModelDetails } from '~/infrastructure/services/catalog.service';
import ReviewsSection from './review-section';

const resizeFromHeight = (height: number, aspectRatio: string = '16:9') => {
   const [widthRatio, heightRatio] = aspectRatio.split(':').map(Number);
   return `w_${Math.round((height * widthRatio) / heightRatio)},h_${height}`;
};

const toastStyle = {
   backgroundColor: '#F0FFF0',
   color: '#18673D',
   border: '1px solid #50C878',
   fontWeight: 'bold',
};
const errorToastStyle = {
   backgroundColor: '#FFF5F5',
   color: '#B91C1C',
   border: '1px solid #FCA5A5',
   fontWeight: 'bold',
};

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
   const [currentSlide, setCurrentSlide] = useState(0);
   const [carouselApi, setCarouselApi] = useState<CarouselApi>();

   const { slug } = useParams();

   const { isAuthenticated } = useAppSelector((state) => state.auth);

   const { storeBasketSync } = useCartSync();

   const {
      getModelBySlugAsync,
      getModelBySlugState,
      isLoading: isGetModelBySlugLoading,
   } = useCatalogService();

   const { storeBasketAsync, isLoading: isStoreBasketLoading } =
      useBasketService();

   useEffect(() => {
      const fetchModel = async () => {
         await getModelBySlugAsync(slug as string);
      };

      fetchModel();
   }, [getModelBySlugAsync, slug]);

   const models = useMemo(() => {
      if (!getModelBySlugState.data) return [];

      return getModelBySlugState.data.model_items;
   }, [getModelBySlugState.data]);

   const colors = useMemo(() => {
      if (!getModelBySlugState.data) return [];

      return getModelBySlugState.data.color_items;
   }, [getModelBySlugState.data]);

   const storages = useMemo(() => {
      if (!getModelBySlugState.data) return [];

      return getModelBySlugState.data.storage_items;
   }, [getModelBySlugState.data]);

   const isLoading = useMemo(() => {
      return isGetModelBySlugLoading || isStoreBasketLoading;
   }, [isGetModelBySlugLoading, isStoreBasketLoading]);

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

      // Navigate to the image corresponding to the selected color
      const targetImageIndex = getImageIndexByColor(color.normalized_name);
      setCurrentSlide(targetImageIndex);
      if (carouselApi) {
         carouselApi.scrollTo(targetImageIndex);
      }

      // Only reset storage selection if not completed initial selection
      if (!hasCompletedInitialSelection) {
         setSelectedStorage(null);
      }
   };

   // Function to find the index of the image corresponding to the selected color
   const getImageIndexByColor = (colorName: string) => {
      // Find the selected color item from the original data
      const selectedColorItem = iphoneDetailsFakeData.color_items.find(
         (color) => color.normalized_name === colorName,
      );

      if (!selectedColorItem) {
         return 0; // Default to first image if color not found
      }

      // Find the index of the showcase image that matches the selected color's showcase_image_id
      const imageIndex = iphoneDetailsFakeData.showcase_images.findIndex(
         (image) => image.image_id === selectedColorItem.showcase_image_id,
      );

      // Return the index, or 0 if not found
      return imageIndex !== -1 ? imageIndex : 0;
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

      const handleAddToBag = async () => {
         console.log('selectedColor', selectedColor);
         console.log('selectedModel', selectedModel);
         console.log('selectedStorage', selectedStorage);

         if (!selectedColor || !selectedModel || !selectedStorage) {
            return;
         }

         if (isAuthenticated) {
            const result = await storeBasketAsync({
               cart_items: [
                  {
                     is_selected: false,
                     model_id: (getModelBySlugState.data as TIphoneModelDetails)
                        .id,
                     color: {
                        name: selectedColor.name,
                        normalized_name: selectedColor.normalized_name,
                     },
                     model: {
                        name: selectedModel.name,
                        normalized_name: selectedModel.normalized_name,
                     },
                     storage: {
                        name: selectedStorage.name,
                        normalized_name: selectedStorage.normalized_name,
                     },
                     quantity: 1,
                  },
               ],
            });

            if (result.isSuccess) {
               toast.success('Item added to cart', {
                  position: 'bottom-center',
                  style: toastStyle,
               });
            } else {
               toast.error('Failed to add item to cart', {
                  position: 'bottom-center',
                  style: errorToastStyle,
               });
            }
         } else {
            const data = getModelBySlugState.data as TIphoneModelDetails;

            const color = {
               name: selectedColor.name,
               normalized_name: selectedColor.normalized_name,
               hex_code:
                  data.color_items.find(
                     (color) =>
                        color.normalized_name === selectedColor.normalized_name,
                  )?.hex_code || '',
               showcase_image_id:
                  data.color_items.find(
                     (color) =>
                        color.normalized_name === selectedColor.normalized_name,
                  )?.showcase_image_id || '',
               order:
                  data.color_items.find(
                     (color) =>
                        color.normalized_name === selectedColor.normalized_name,
                  )?.order || 0,
            };

            const model = {
               name: selectedModel.name,
               normalized_name: selectedModel.normalized_name,
               order: 0,
            };

            const storage = {
               name: selectedStorage.name,
               normalized_name: selectedStorage.normalized_name,
               order: 0,
            };

            const display_image_url =
               data.showcase_images.find(
                  (image) => image.image_id === color.showcase_image_id,
               )?.image_url || '';

            storeBasketSync([
               {
                  is_selected: false,
                  model_id: data.id,
                  product_name: `${selectedModel.name} ${selectedStorage.name} ${selectedColor.name}`,
                  color: color,
                  model: model,
                  storage: storage,
                  display_image_url: display_image_url,
                  unit_price: 1000,
                  promotion: null,
                  quantity: 1,
                  sub_total_amount: 1000,
                  index: 1,
               },
            ]);

            toast.success('Item added to cart', {
               position: 'bottom-center',
               style: toastStyle,
            });
         }
      };

      return (
         <div className="fixed bottom-0 left-0 right-0 w-full bg-white border-t border-[#d2d2d7] z-50 shadow-[0_-2px_10px_rgba(0,0,0,0.1)]">
            <LoadingOverlay isLoading={isLoading} fullScreen />
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
                              // console.log('Add to Bag:', {
                              //    selectedModel,
                              //    selectedColor,
                              //    selectedStorage,
                              //    availableBranches: filteredBranches,
                              // });

                              handleAddToBag();
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
               <Carousel
                  setApi={setCarouselApi}
                  opts={{
                     loop: true,
                     align: 'center',
                  }}
               >
                  <CarouselContent>
                     {iphoneDetailsFakeData.showcase_images.map((image) => (
                        <CarouselItem key={image.image_id}>
                           <div className="w-full overflow-hidden relative h-[1000px]">
                              <NextImage
                                 src={`https://res.cloudinary.com/delkyrtji/image/upload/${resizeFromHeight(1000, '16:9')}/${image.image_url.split('/').pop()}`}
                                 alt={
                                    image.image_name || 'iPhone showcase image'
                                 }
                                 width={Math.round((1000 * 16) / 9)}
                                 height={1000}
                                 className="absolute top-0 left-0 w-full h-full object-cover rounded-[20px]"
                              />
                           </div>
                        </CarouselItem>
                     ))}
                  </CarouselContent>

                  <CarouselPrevious className="left-[1rem]" />
                  <CarouselNext className="right-[1rem]" />

                  <div className="absolute bottom-2 left-0 w-full z-50 flex flex-row items-center justify-center gap-2">
                     {Array.from({
                        length: iphoneDetailsFakeData.showcase_images.length,
                     }).map((_, index) => (
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
                        {models.map((model: TModelItem) => (
                           <ModelItem
                              key={model.name}
                              modelName={model.name}
                              displaySize={model.name}
                              price={0}
                              monthlyPrice={'0'}
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
                        {colors.map((color: TColorItem) => {
                           return (
                              <ColorItem
                                 key={color.name}
                                 colorHex={color.hex_code}
                                 colorName={color.name}
                                 isSelected={
                                    selectedColor?.normalized_name ===
                                    color.normalized_name
                                 }
                                 onClick={() => {
                                    return (
                                       !isColorDisabled &&
                                       handleColorSelection({
                                          name: color.name,
                                          normalized_name:
                                             color.normalized_name,
                                       })
                                    );
                                 }}
                                 onMouseEnter={() =>
                                    !isColorDisabled &&
                                    setHoveredColor(color.name)
                                 }
                                 onMouseLeave={() => setHoveredColor(null)}
                                 isDisabled={isColorDisabled}
                              />
                           );
                        })}
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
                        {storages.map((storage: TStorageItem) => {
                           return (
                              <StorageItem
                                 key={storage.name}
                                 storageName={storage.name}
                                 price={0}
                                 monthlyPrice={'0'}
                                 note={undefined}
                                 isSelected={
                                    selectedStorage?.normalized_name ===
                                    storage.normalized_name
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
                           );
                        })}
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
            {/* end right */}
         </div>

         {/* Checkout Bottom Bar */}
         {renderCheckoutBottom()}
      </div>
   );
};

export default IphoneDetails;
