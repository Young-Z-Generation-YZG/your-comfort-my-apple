'use client';

import {
   Accordion,
   AccordionContent,
   AccordionItem,
   AccordionTrigger,
} from '@components/ui/accordion';
import { DualRangeSlider } from '@components/ui/dualRangeSlider';
import { Button } from '@components/ui/button';
import { useEffect, useState, useCallback } from 'react';
import { motion } from 'framer-motion';
import { cn } from '~/infrastructure/lib/utils';
import { useDebounce } from '@components/hooks/use-debounce';
import { useSearchParams } from 'next/navigation';

const storageFilter = [
   {
      name: '64GB',
      normalizedName: '64GB',
      value: '64GB',
   },
   {
      name: '128GB',
      normalizedName: '128GB',
      value: '128GB',
   },
   {
      name: '256GB',
      normalizedName: '256GB',
      value: '256GB',
   },
   {
      name: '512GB',
      normalizedName: '512GB',
      value: '512GB',
   },
   {
      name: '1TB',
      normalizedName: '1TB',
      value: '1TB',
   },
];
const models = [
   {
      name: 'iPhone 15',
      normalizedName: 'IPHONE_15',
      value: 'iphone-15',
   },
   {
      name: 'iPhone 16',
      normalizedName: 'IPHONE_16',
      value: 'iphone-16',
   },
   {
      name: 'iPhone 16e',
      normalizedName: 'IPHONE_16E',
      value: 'iphone-16e',
   },
   {
      name: 'iPhone 16 Pro',
      normalizedName: 'IPHONE_16_PRO',
      value: 'iphone-16-pro',
   },
];
const colors = [
   {
      name: 'ultramarine',
      normalizedName: 'ULTRAMARINE',
      hex: '#9AADF6',
   },
   {
      name: 'green',
      normalizedName: 'GREEN',
      hex: '#D0D9CA',
   },
   {
      name: 'teal',
      normalizedName: 'TEAL',
      hex: '#B0D4D2',
   },
   {
      name: 'blue',
      normalizedName: 'BLUE',
      hex: '#D5DDDF',
   },
   {
      name: 'yellow',
      normalizedName: 'YELLOW',
      hex: '#EDE6C8',
   },
   {
      name: 'pink',
      normalizedName: 'PINK',
      hex: '#F2ADDA',
   },
   {
      name: 'white',
      normalizedName: 'WHITE',
      hex: '#FAFAFA',
   },
   {
      name: 'black',
      normalizedName: 'BLACK',
      hex: '#3C4042',
   },
];

const FilterSection = () => {
   const searchParams = useSearchParams();

   // Initialize state from URL params
   const [priceFilter, setPriceFilter] = useState<[number, number]>(() => {
      const minPrice = searchParams.get('_minPrice');
      const maxPrice = searchParams.get('_maxPrice');
      return [
         minPrice ? Number(minPrice) : 0,
         maxPrice ? Number(maxPrice) : 2000,
      ];
   });

   const [selectedColors, setSelectedColors] = useState<string[]>(() => {
      return searchParams.getAll('_colors');
   });

   const [selectedModels, setSelectedModels] = useState<string[]>(() => {
      return searchParams.getAll('_models');
   });

   const [selectedStorages, setSelectedStorages] = useState<string[]>(() => {
      return searchParams.getAll('_storages');
   });

   const debouncedPriceFilter = useDebounce(priceFilter, 500);

   const toggleStorage = useCallback((normalizedName: string) => {
      setSelectedStorages((prev) =>
         prev.includes(normalizedName)
            ? prev.filter((s) => s !== normalizedName)
            : [...prev, normalizedName],
      );
   }, []);

   const toggleModel = useCallback((normalizedName: string) => {
      setSelectedModels((prev) =>
         prev.includes(normalizedName)
            ? prev.filter((m) => m !== normalizedName)
            : [...prev, normalizedName],
      );
   }, []);

   const toggleColor = useCallback((normalizedName: string) => {
      setSelectedColors((prev) =>
         prev.includes(normalizedName)
            ? prev.filter((c) => c !== normalizedName)
            : [...prev, normalizedName],
      );
   }, []);

   // Update URL params when filters change
   useEffect(() => {
      const params = new URLSearchParams();

      // Add color filters
      selectedColors.forEach((color) => params.append('_colors', color));

      // Add model filters
      selectedModels.forEach((model) => params.append('_models', model));

      // Add storage filters
      selectedStorages.forEach((storage) =>
         params.append('_storages', storage),
      );

      // Add price filters
      if (debouncedPriceFilter[0] !== 0) {
         params.set('_minPrice', debouncedPriceFilter[0].toString());
      }
      if (debouncedPriceFilter[1] !== 2000) {
         params.set('_maxPrice', debouncedPriceFilter[1].toString());
      }

      // Update URL without reloading
      const newUrl = params.toString()
         ? `${window.location.pathname}?${params.toString()}`
         : window.location.pathname;

      window.history.replaceState({}, '', newUrl);
   }, [selectedColors, selectedModels, selectedStorages, debouncedPriceFilter]);

   return (
      <div className="w-[354px] min-h-screen relative">
         <div className="w-full h-fit sticky top-0 bg-white">
            <Accordion
               type="multiple"
               defaultValue={['storages', 'colors', 'price', 'models']}
               className="w-full h-full p-6"
            >
               {/* STORAGE FILTER */}
               <motion.div transition={{ duration: 0.3 }}>
                  <AccordionItem value="storages">
                     <AccordionTrigger className="hover:no-underline text-[15px] font-semibold pb-0 uppercase ">
                        Storages
                     </AccordionTrigger>
                     <AccordionContent className="pt-4">
                        <div className="grid grid-cols-3 gap-2">
                           {storageFilter.map((storage) => (
                              <Button
                                 key={storage.value}
                                 onClick={() =>
                                    toggleStorage(storage.normalizedName)
                                 }
                                 className={cn(
                                    'h-fit py-1 rounded-full text-[12px] font-normal border border-[#000] opacity-50 hover:opacity-100 bg-white text-black hover:bg-black hover:text-white transition-all',
                                    {
                                       'bg-black text-white opacity-100':
                                          selectedStorages.includes(
                                             storage.normalizedName,
                                          ),
                                    },
                                 )}
                              >
                                 {storage.name}
                              </Button>
                           ))}
                        </div>
                     </AccordionContent>
                  </AccordionItem>
               </motion.div>

               {/* COLORS FILTER */}
               <motion.div transition={{ duration: 0.3 }}>
                  <AccordionItem value="colors">
                     <AccordionTrigger className="hover:no-underline text-[15px] font-semibold pb-0 uppercase ">
                        COLORS
                     </AccordionTrigger>
                     <AccordionContent className="pt-4">
                        <div className="grid grid-cols-4 gap-x-0 gap-y-4 ml-2">
                           {colors.map((color) => (
                              <div
                                 key={color.name}
                                 onClick={() =>
                                    toggleColor(color.normalizedName)
                                 }
                                 className={cn(
                                    'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out hover:ring-2 hover:ring-[#2563EB] hover:ring-offset-2 hover:ring-offset-white',
                                    {
                                       'ring-2 ring-[#2563EB] ring-offset-2 ring-offset-white':
                                          selectedColors.includes(
                                             color.normalizedName,
                                          ),
                                    },
                                 )}
                                 style={{
                                    backgroundColor: color.hex,
                                 }}
                              />
                           ))}
                        </div>
                     </AccordionContent>
                  </AccordionItem>
               </motion.div>

               {/* PRICE FILTER */}
               <AccordionItem value="price">
                  <AccordionTrigger className="hover:no-underline text-[15px] font-semibold pb-0 uppercase ">
                     Price
                  </AccordionTrigger>
                  <AccordionContent className="pt-4">
                     <div className="w-full flex flex-col justify-center">
                        <div className="w-full flex flex-row justify-between">
                           <div>From: {priceFilter[0]}$</div>
                           <div>To: {priceFilter[1]}$</div>
                        </div>
                        <DualRangeSlider
                           className="mt-3 bg-gray-200 rounded-lg"
                           value={priceFilter}
                           onValueChange={(value) =>
                              setPriceFilter(value as [number, number])
                           }
                           min={0}
                           max={2000}
                           step={50}
                        />
                     </div>
                  </AccordionContent>
               </AccordionItem>

               {/* MODELS FILTER */}
               <AccordionItem value="models">
                  <AccordionTrigger className="hover:no-underline text-[15px] font-semibold pb-0 uppercase">
                     Models
                  </AccordionTrigger>
                  <AccordionContent className="pt-4">
                     <div className="grid grid-cols-3 gap-2">
                        {models.map((model) => (
                           <Button
                              key={model.value}
                              onClick={() => toggleModel(model.normalizedName)}
                              className={cn(
                                 'h-fit py-1 rounded-full select-none cursor-pointer text-center text-[12px] font-normal border border-[#000] opacity-50 hover:opacity-100 bg-white text-black hover:bg-black hover:text-white transition-all',
                                 {
                                    'bg-black text-white opacity-100':
                                       selectedModels.includes(
                                          model.normalizedName,
                                       ),
                                 },
                              )}
                           >
                              {model.name}
                           </Button>
                        ))}
                     </div>
                  </AccordionContent>
               </AccordionItem>
            </Accordion>
         </div>
      </div>
   );
};

export default FilterSection;
