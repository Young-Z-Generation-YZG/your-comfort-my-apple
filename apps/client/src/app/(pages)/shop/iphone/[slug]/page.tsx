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
import { Button } from '~/components/ui/button';
import useCatalogService from '~/hooks/api/use-catalog-service';
import { useParams } from 'next/navigation';
import useBasketService from '~/hooks/api/use-basket-service';
import { useAppSelector } from '~/infrastructure/redux/store';
import { toast } from 'sonner';
import { LoadingOverlay } from '~/components/client/loading-overlay';
import useCartSync from '~/hooks/use-cart-sync';
import {
   TBranch,
   TColor,
   TImage,
   TSku,
   TIphoneModelDetails,
   TModel,
   TStorage,
   TSkuPrice,
} from '~/domain/types/catalog.type';
import useReviewService from '~/hooks/api/use-review-service';
import ColorItem from '~/app/(pages)/shop/iphone/[slug]/_components/color-item';
import HelpItem from '~/app/(pages)/shop/iphone/_components/help-item';
import ModelItem from '~/app/(pages)/shop/iphone/[slug]/_components/model-item';
import StorageItem from '~/app/(pages)/shop/iphone/[slug]/_components/storage-item';
import ReviewItem from '~/app/(pages)/shop/iphone/[slug]/_components/review-item';
import { cn } from '~/infrastructure/lib/utils';
import { AppleIcon } from '~/components/icon';
import ProductInfo from '~/app/(pages)/shop/iphone/[slug]/_components/product-info';
import CompareIPhoneSection from '~/components/client/compare-iphone-section';
import usePaginationV2 from '~/hooks/use-pagination';
import {
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
   Ellipsis,
   TrendingUp,
   Flame,
   CheckCircle2,
   ShoppingBag,
} from 'lucide-react';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '~/components/ui/select';
import { Separator } from '~/components/ui/separator';
import RatingStar from '~/components/ui/rating-star';

const appleCareOptions = [
   {
      title: 'AppleCare+',
      price: '199.00',
      monthlyPrice: '9.99',
      features: [
         'Unlimited repairs for accidental damage protection',
         'Apple-certified repairs using genuine Apple parts',
         "Express Replacement Service - we'll ship you a replacement so you don't have to wait for a repair",
         '24/7 priority access to Apple experts',
      ],
   },
   {
      title: 'AppleCare+ with Theft and Loss',
      price: '269.00',
      monthlyPrice: '13.49',
      features: [
         'Everything in AppleCare+ with additional coverage for theft and loss',
         'We can ship your replacement iPhone to any country where AppleCare+ with Theft and Loss is available.',
      ],
   },
   {
      title: 'No AppleCare+ coverage',
      price: '0',
      monthlyPrice: '0',
      features: [],
   },
];

const toastStyle = {
   backgroundColor: '#F0FFF0',
   color: '#18673D',
   border: '1px solid #50C878',
   fontWeight: 'bold',
};

const resizeFromHeight = (height: number, aspectRatio: string = '16:9') => {
   const [widthRatio, heightRatio] = aspectRatio.split(':').map(Number);
   return `w_${Math.round((height * widthRatio) / heightRatio)},h_${height}`;
};

const IphoneDetailsPage = () => {
   const { slug } = useParams();
   const {
      getModelBySlugAsync,
      getModelBySlugState,
      isLoading: isGetModelBySlugLoading,
   } = useCatalogService();
   const { storeBasketItemAsync, isLoading: isBasketLoading } =
      useBasketService();
   const isAuthenticated = useAppSelector(
      (state) => state.auth.isAuthenticated,
   );
   const {
      getReviewByProductModelIdAsync,
      getReviewByProductModelIdState,
      isLoading: isFetchReviewLoading,
   } = useReviewService();

   const isLoading =
      isGetModelBySlugLoading || isBasketLoading || isFetchReviewLoading;

   const { storeBasketSync } = useCartSync({ autoSync: false });

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
   const [_pageReview, setPageReview] = useState<number>(1);
   const [_limitReview, setLimitReview] = useState<number>(5);

   useEffect(() => {
      getModelBySlugAsync(slug as string);
   }, [slug]);

   useEffect(() => {
      if (getModelBySlugState.isSuccess && getModelBySlugState.data.id) {
         getReviewByProductModelIdAsync(getModelBySlugState.data.id, {
            _page: _pageReview,
            _limit: _limitReview,
            _sortOrder: 'DESC',
         });
      }
   }, [getModelBySlugState.isSuccess, _pageReview, _limitReview]);

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

   const iphoneDetailsData: TIphoneModelDetails | null = useMemo(() => {
      if (getModelBySlugState.isSuccess) {
         return getModelBySlugState.data;
      }
      return null;
   }, [getModelBySlugState]);

   const reviewsData = useMemo(() => {
      if (getReviewByProductModelIdState.isSuccess) {
         return getReviewByProductModelIdState.data;
      }
      return {
         total_records: 0,
         total_pages: 0,
         page_size: 10,
         current_page: 1,
         items: [],
         links: {
            first: null,
            last: null,
            prev: null,
            next: null,
         },
      };
   }, [getReviewByProductModelIdState]);

   const {
      currentPage,
      totalPages,
      totalRecords,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue,
      getPaginationItems,
   } = usePaginationV2(reviewsData, {
      pageSizeOverride: _limitReview,
      currentPageOverride: _pageReview,
   });

   const paginationItems = getPaginationItems();

   const handlePageChange = (page: number) => {
      if (page !== currentPage && page >= 1 && page <= totalPages) {
         setPageReview(page);
      }
   };

   const handlePageSizeChange = (size: string) => {
      setLimitReview(Number(size));
      setPageReview(1);
   };

   const models = useMemo(() => {
      if (!getModelBySlugState.isSuccess || !getModelBySlugState.data)
         return [];

      if (getModelBySlugState.data.model_items.length === 1) {
         setSelectedModel(getModelBySlugState.data.model_items[0]);

         return [];
      }

      return getModelBySlugState.data.model_items;
   }, [
      getModelBySlugState.data,
      setSelectedModel,
      getModelBySlugState.isSuccess,
   ]);

   const colors = useMemo(() => {
      if (!getModelBySlugState.data) return [];

      return getModelBySlugState.data.color_items;
   }, [getModelBySlugState.data]);

   const storages = useMemo(() => {
      if (!getModelBySlugState.data) return [];

      return getModelBySlugState.data.storage_items;
   }, [getModelBySlugState.data]);

   if (!iphoneDetailsData) {
      return null;
   }

   // Check if options should be disabled (only during initial progressive selection)
   const isColorDisabled = !hasCompletedInitialSelection && !selectedModel;
   const isStorageDisabled =
      !hasCompletedInitialSelection && (!selectedModel || !selectedColor);

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
      if (targetImageIndex !== undefined) {
         setCurrentSlide(targetImageIndex);
         if (carouselApi && targetImageIndex !== undefined) {
            carouselApi.scrollTo(targetImageIndex);
         }
      }

      // Only reset storage selection if not completed initial selection
      if (!hasCompletedInitialSelection) {
         setSelectedStorage(null);
      }
   };

   // Function to find the index of the image corresponding to the selected color
   const getImageIndexByColor = (colorName: string) => {
      // Find the selected color item from the original data
      const selectedColorItem = iphoneDetailsData.color_items.find(
         (color: TColor) => color.normalized_name === colorName,
      );

      if (!selectedColorItem) {
         return 0; // Default to first image if color not found
      }

      // Find the index of the showcase image that matches the selected color's showcase_image_id
      const imageIndex = iphoneDetailsData.showcase_images.findIndex(
         (image: TImage) =>
            image.image_id === selectedColorItem.showcase_image_id,
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

   // Filter branches and SKUs based on selections
   const getFilteredBranches = () => {
      if (!selectedModel || !selectedColor || !selectedStorage) {
         return [];
      }

      return iphoneDetailsData.branchs
         .map((branchData: { branch: TBranch; skus: TSku[] }) => {
            // Find the matching SKU for this branch
            const matchingSku = branchData.skus.find((sku: TSku) => {
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

   const filteredBranches = getFilteredBranches() || [];

   const renderCheckoutBottom = () => {
      const isAllSelected = selectedModel && selectedColor && selectedStorage;

      const handleAddToBag = async () => {
         console.log('selectedColor', selectedColor);
         console.log('selectedModel', selectedModel);
         console.log('selectedStorage', selectedStorage);

         if (!selectedColor || !selectedModel || !selectedStorage) {
            return;
         }

         const data = getModelBySlugState.data as TIphoneModelDetails;

         const color = {
            name: selectedColor.name,
            normalized_name: selectedColor.normalized_name,
            hex_code:
               data.color_items.find(
                  (color: TColor) =>
                     color.normalized_name === selectedColor.normalized_name,
               )?.hex_code || '',
            showcase_image_id:
               data.color_items.find(
                  (color: TColor) =>
                     color.normalized_name === selectedColor.normalized_name,
               )?.showcase_image_id || '',
            order:
               data.color_items.find(
                  (color: TColor) =>
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
               (image: TImage) => image.image_id === color.showcase_image_id,
            )?.image_url || '';

         const sku_id =
            getModelBySlugState.data?.sku_prices.find(
               (sku: TSkuPrice) =>
                  sku.normalized_model === selectedModel.normalized_name &&
                  sku.normalized_color === selectedColor.normalized_name &&
                  sku.normalized_storage === selectedStorage.normalized_name,
            )?.sku_id || '';

         const unit_price =
            getModelBySlugState.data?.sku_prices.find(
               (sku) =>
                  sku.normalized_model === selectedModel.normalized_name &&
                  sku.normalized_color === selectedColor.normalized_name &&
                  sku.normalized_storage === selectedStorage.normalized_name,
            )?.unit_price || 0;

         const sub_total_amount = unit_price * 1;
         const discount_amount = 0;
         const total_amount = sub_total_amount - discount_amount;

         // Always update Redux immediately for UI responsiveness
         storeBasketSync([
            {
               is_selected: false,
               model_id: data.id,
               sku_id: sku_id,
               product_name: `${selectedModel.name} ${selectedStorage.name} ${selectedColor.name}`,
               color: color,
               model: model,
               storage: storage,
               display_image_url: display_image_url,
               unit_price: unit_price,
               promotion: null,
               quantity: 1,
               sub_total_amount: sub_total_amount,
               discount_amount: discount_amount,
               total_amount: total_amount,
               index: 1,
               quantity_remain: 999, // note mock
            },
         ]);

         // If logged in, also persist the single item server-side
         if (isAuthenticated) {
            await storeBasketItemAsync({
               is_selected: false,
               sku_id: sku_id,
               quantity: 1,
            });
         }

         toast.success('Item added to cart', {
            position: 'bottom-center',
            style: toastStyle,
         });
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
                                       <div className="text-sm text-[#0071E3]">
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
                           onClick={async () => {
                              await handleAddToBag();
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

   return (
      <div className="px-5">
         {/* Product Title */}
         <div className="w-full bg-transparent flex flex-row pt-[52px] pb-32">
            <div className="basis-[70%] bg-transparent">
               <span className="text-[18px] text-[#b64400] font-semibold leading-[20px]">
                  New
               </span>
               <h1 className="text-[48px] font-semibold leading-[52px] pb-2 mb-[13px]">
                  {iphoneDetailsData.name || 'NO DATA'}
               </h1>
               <div className="text-[15px] font-light leading-[20px]">
                  From $999 or $41.62/mo. for 24 mo.
               </div>
               {iphoneDetailsData.overall_sold > 0 && (
                  <div className="mt-5 flex items-center">
                     <div className="inline-flex items-center gap-2 px-4 py-2.5 bg-white border-2 border-[#0071E3] rounded-xl shadow-md hover:border-[#0077ED] hover:shadow-lg transition-all duration-200">
                        <ShoppingBag className="w-5 h-5 text-[#0071E3]" />
                        <div className="flex flex-col">
                           <span className="text-[20px] font-extrabold text-[#0071E3] leading-tight">
                              {iphoneDetailsData.overall_sold.toLocaleString()}
                           </span>
                           <span className="text-[12px] font-medium text-[#86868B] leading-tight -mt-0.5">
                              units sold
                           </span>
                        </div>
                     </div>
                  </div>
               )}
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
         <div className="w-full flex flex-col lg:flex-row gap-14 relative lg:h-[1000px]">
            {/* left */}
            <div className="order-1 w-full lg:basis-[70%] lg:sticky lg:top-[100px] lg:self-start">
               <Carousel
                  setApi={setCarouselApi}
                  opts={{
                     loop: true,
                     align: 'center',
                  }}
               >
                  <CarouselContent>
                     {iphoneDetailsData.showcase_images.map((image) => (
                        <CarouselItem key={image.image_id}>
                           <div className="w-full overflow-hidden relative h-[1000px]">
                              <NextImage
                                 src={`https://res.cloudinary.com/delkyrtji/image/upload/${resizeFromHeight(1000, '16:9')}/${image.image_url.split('/').pop()}`}
                                 alt={image.image_name}
                                 width={Math.round((1000 * 16) / 9)}
                                 height={0}
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
                        length: iphoneDetailsData.showcase_images.length || 0,
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

            {/* Right */}
            <div className="order-2 w-full lg:basis-[30%] lg:max-w-[328px]">
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
                        {models.map((model: TModel) => (
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
                        {colors.map((color: TColor) => {
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
                        {storages.map((storage: TStorage) => {
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
               </div>
            </div>
         </div>

         {/* Coverage */}
         <div className="mt-16 sm:mt-24 lg:mt-[200px]">
            <div
               className={cn('w-full bg-transparent flex flex-col mt-16 h-fit')}
            >
               <div className="coverage-title flex flex-col lg:flex-row">
                  <div className="w-full lg:basis-3/4">
                     <div className="w-full text-[24px] font-semibold leading-[28px]">
                        <span className="text-[#1D1D1F] tracking-[0.3px]">
                           AppleCare+ coverage.{' '}
                        </span>
                        <span className="text-[#86868B] tracking-[0.3px]">
                           Protect your new iPhone.
                        </span>
                     </div>
                  </div>
                  <div className="w-full lg:basis-1/4 lg:min-w-[328px] mt-4 lg:mt-0"></div>
               </div>
               <div className="coverage-items flex flex-col lg:flex-row mt-6">
                  <div className="w-full lg:basis-3/4 list-items grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 pr-0 lg:pr-12">
                     {appleCareOptions.map((option) => {
                        const isNoCoverage = option.title.startsWith('No ');

                        return (
                           <div className="border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col items-start">
                              <div className="item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] flex flex-row items-start justify-start">
                                 {!isNoCoverage ? (
                                    <span className="flex flex-row items-start justify-start gap-1">
                                       <AppleIcon size={21} color="red" />
                                       <span>{option.title}</span>
                                    </span>
                                 ) : (
                                    <span>{option.title}</span>
                                 )}
                              </div>

                              {!isNoCoverage && (
                                 <>
                                    <div className="item-price w-full pt-1 pb-[21px] border-b-[0.8px] border-[#86868b] text-[14px] font-light leading-[21px] tracking-[0.5px]">
                                       ${option.title} or ${option.monthlyPrice}
                                       /mo.
                                    </div>
                                    {option.features.length > 0 && (
                                       <div className="item-sub-title w-full pt-[18px] text-[12px] font-light leading-[16px] tracking-[0.8px]">
                                          <ul className="list-disc ml-4">
                                             {option.features.map(
                                                (feature, index) => (
                                                   <li
                                                      key={index}
                                                      className="pr-3 pb-[15px] tracking-[0.7px]"
                                                   >
                                                      {feature}
                                                   </li>
                                                ),
                                             )}
                                          </ul>
                                       </div>
                                    )}
                                 </>
                              )}
                           </div>
                        );
                     })}
                  </div>
                  <div className="w-full lg:basis-1/4 lg:min-w-[328px] mt-6 lg:mt-0">
                     <HelpItem
                        title="What kind of protection do you need?"
                        subTitle="Compare the additional features and coverage of the two AppleCare+ plans."
                     />
                  </div>
               </div>
            </div>
         </div>

         {/* start: Product Info */}
         <ProductInfo />
         {/* end: Product Info */}

         {/* Compare iPhone Section */}
         <div className="mx-auto mt-20 mb-24">
            <CompareIPhoneSection />
         </div>

         {/* Reviews section */}
         <div className="mt-16">
            {/* Header Section */}
            <div className="mb-8">
               <h2 className="text-3xl font-bold tracking-tight text-gray-900 dark:text-gray-100 mb-4">
                  Customer Reviews
               </h2>

               {reviewsData.items.length > 0 && (
                  <div className="flex items-center gap-4">
                     <div className="flex items-center gap-2">
                        <RatingStar
                           rating={
                              iphoneDetailsData.average_rating
                                 .rating_average_value
                           }
                           size="lg"
                        />
                        <span className="text-2xl font-semibold">
                           {iphoneDetailsData.average_rating.rating_average_value.toFixed(
                              1,
                           )}
                        </span>
                     </div>
                     <Separator orientation="vertical" className="h-6" />
                     <span className="text-sm text-gray-600 dark:text-gray-400">
                        {totalRecords}{' '}
                        {totalRecords === 1 ? 'review' : 'reviews'}
                     </span>
                  </div>
               )}
            </div>

            {/* Reviews List */}
            {!(reviewsData.items.length > 0) ? (
               <div className="space-y-6">
                  {[1, 2, 3].map((i) => (
                     <div
                        key={i}
                        className="animate-pulse bg-gray-100 dark:bg-gray-800 rounded-lg p-6 h-32"
                     />
                  ))}
               </div>
            ) : reviewsData.items.length > 0 ? (
               <>
                  <div className="space-y-6 mb-8">
                     {reviewsData.items.map((item) => {
                        return (
                           <ReviewItem
                              key={item.id}
                              review={item}
                              onReviewUpdated={() => {
                                 // Refresh reviews after update
                                 if (getModelBySlugState.data?.id) {
                                    getReviewByProductModelIdAsync(
                                       getModelBySlugState.data.id,
                                       {
                                          _page: _pageReview,
                                          _limit: _limitReview,
                                          _sortOrder: 'DESC',
                                       },
                                    );
                                 }
                              }}
                           />
                        );
                     })}
                  </div>

                  {/* Pagination */}
                  {totalPages > 0 && (
                     <div className="mt-8 flex flex-col sm:flex-row items-center justify-between gap-4 pb-6">
                        {/* Page Info & Size Selector */}
                        <div className="flex items-center gap-4">
                           <Select
                              value={limitSelectValue}
                              onValueChange={handlePageSizeChange}
                           >
                              <SelectTrigger className="w-auto h-9">
                                 <SelectValue />
                              </SelectTrigger>
                              <SelectContent>
                                 <SelectGroup>
                                    <SelectItem value="5">5 / page</SelectItem>
                                    <SelectItem value="10">
                                       10 / page
                                    </SelectItem>
                                    <SelectItem value="20">
                                       20 / page
                                    </SelectItem>
                                 </SelectGroup>
                              </SelectContent>
                           </Select>

                           <div className="text-sm text-gray-600">
                              Showing{' '}
                              <span className="font-semibold">
                                 {firstItemIndex}
                              </span>{' '}
                              to{' '}
                              <span className="font-semibold">
                                 {lastItemIndex}
                              </span>{' '}
                              of{' '}
                              <span className="font-semibold">
                                 {totalRecords}
                              </span>{' '}
                              reviews
                           </div>
                        </div>

                        {/* Pagination Controls */}
                        <div className="flex items-center gap-2">
                           {paginationItems.map((item, index) => {
                              if (item.type === 'ellipsis') {
                                 return (
                                    <span
                                       key={`ellipsis-${index}`}
                                       className="px-2 text-gray-400 flex items-center"
                                    >
                                       <Ellipsis className="h-4 w-4" />
                                    </span>
                                 );
                              }

                              const isCurrentPage =
                                 item.type === 'page' &&
                                 item.value === currentPage;

                              return (
                                 <Button
                                    key={`${item.type}-${item.label}-${index}`}
                                    variant={
                                       isCurrentPage ? 'default' : 'outline'
                                    }
                                    size="icon"
                                    className={cn(
                                       'h-9 w-9',
                                       isCurrentPage &&
                                          'bg-black text-white hover:bg-black/90',
                                    )}
                                    disabled={
                                       item.disabled || item.value === null
                                    }
                                    onClick={() => {
                                       if (
                                          item.value !== null &&
                                          !item.disabled
                                       ) {
                                          handlePageChange(item.value);
                                       }
                                    }}
                                 >
                                    {item.type === 'nav' ? (
                                       item.label === '<<' ? (
                                          <ChevronsLeft className="h-4 w-4" />
                                       ) : item.label === '>>' ? (
                                          <ChevronsRight className="h-4 w-4" />
                                       ) : item.label === '<' ? (
                                          <ChevronLeft className="h-4 w-4" />
                                       ) : (
                                          <ChevronRight className="h-4 w-4" />
                                       )
                                    ) : (
                                       item.label
                                    )}
                                 </Button>
                              );
                           })}
                        </div>
                     </div>
                  )}
               </>
            ) : (
               <div className="text-center py-12 border border-dashed border-gray-300 dark:border-gray-700 rounded-lg">
                  <p className="text-gray-500 dark:text-gray-400 text-lg">
                     No reviews yet. Be the first to review this product!
                  </p>
               </div>
            )}
         </div>
         {/* end: Reviews Section */}

         {/* Checkout Bottom Bar */}
         {renderCheckoutBottom()}
      </div>
   );
};

export default IphoneDetailsPage;
