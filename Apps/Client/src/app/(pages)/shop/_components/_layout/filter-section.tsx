'use client';

import {
   Accordion,
   AccordionContent,
   AccordionItem,
   AccordionTrigger,
} from '@components/ui/accordion';
import { DualRangeSlider } from '@components/ui/dualRangeSlider';
import { Button } from '@components/ui/button';
import { useEffect, useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { cn } from '~/infrastructure/lib/utils';
import { useDispatch } from 'react-redux';
import {
   FiltersType,
   setAppFilters,
} from '~/infrastructure/redux/features/app.slice';
import { useDebounce } from '~/hooks/use-debouce';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useSearchParams } from 'next/navigation';

const storageFilter = [
   {
      name: '64GB',
      value: '64GB',
   },
   {
      name: '128GB',
      value: '128GB',
   },
   {
      name: '256GB',
      value: '256GB',
   },
   {
      name: '512GB',
      value: '512GB',
   },
   {
      name: '1TB',
      value: '1TB',
   },
];
const models = [
   {
      name: 'iPhone 15',
      value: 'iphone-15',
   },
   {
      name: 'iPhone 16',
      value: 'iphone-16',
   },
   {
      name: 'iPhone 16e',
      value: 'iphone-16e',
   },
   {
      name: 'iPhone 16 Pro',
      value: 'iphone-16-pro',
   },
];
const colors = [
   {
      name: 'ultramarine',
      hex: '#9AADF6',
   },
   {
      name: 'green',
      hex: '#D0D9CA',
   },
   {
      name: 'teal',
      hex: '#B0D4D2',
   },
   {
      name: 'blue',
      hex: '#D5DDDF',
   },
   {
      name: 'yellow',
      hex: '#EDE6C8',
   },
   {
      name: 'pink',
      hex: '#F2ADDA',
   },
   {
      name: 'white',
      hex: '#FAFAFA',
   },
   {
      name: 'black',
      hex: '#3C4042',
   },
];

const FilterSection = () => {
   const [priceFilter, setPriceFilter] = useState([0, 2000]);
   const [selectedColors, setSelectedColors] = useState<string[]>([]);
   const [selectedModels, setSelectedModels] = useState<string[]>([]);
   const [selectedStorages, setSelectedStorages] = useState<string[]>([]);

   const searchParams = useSearchParams();
   const _productColors = searchParams.getAll('_productColors');
   const _productModels = searchParams.getAll('_productModels');
   const _productStorages = searchParams.getAll('_productStorages');

   const [filters, setFilters] = useState<FiltersType>({
      colors: [],
      models: [],
      storages: [],
   });

   const dispatch = useDispatch();

   const debouceFilter = useDebounce(filters, 500);

   const toggleStorage = (storage: string) => {
      setSelectedStorages((prev) =>
         prev.includes(storage)
            ? prev.filter((s) => s !== storage)
            : [...prev, storage],
      );
   };

   const toggleModel = (model: string) => {
      setSelectedModels((prev) =>
         prev.includes(model)
            ? prev.filter((m) => m !== model)
            : [...prev, model],
      );
   };

   const toggleColor = (color: string) => {
      setSelectedColors((prev) =>
         prev.includes(color)
            ? prev.filter((c) => c !== color)
            : [...prev, color],
      );
   };

   useEffect(() => {
      setFilters({
         colors: selectedColors.map((color) => {
            const foundColor = colors.find((c) => c.name === color);
            return foundColor ? foundColor : { name: '', hex: '' };
         }),
         models: selectedModels.map((model) => {
            const foundModel = models.find((m) => m.value === model);

            return foundModel ? foundModel : { name: '', value: '' };
         }),
         storages: selectedStorages.map((storage) => {
            const foundStorage = storageFilter.find((s) => s.name === storage);
            return foundStorage ? foundStorage : { name: '', value: '' };
         }),
      });
   }, [selectedColors, selectedModels, selectedStorages]);

   useEffect(() => {
      setSelectedColors(_productColors);
      setSelectedModels(_productModels);
      setSelectedStorages(_productStorages);
   }, [_productColors.length, _productModels.length, _productStorages.length]);

   useEffect(() => {
      dispatch(setAppFilters(debouceFilter));
   }, [debouceFilter]);

   return (
      <div className="w-[354px] h-[4336px] relative">
         <div className="w-full h-fit sticky top-0">
            <Accordion type="multiple" className="w-full h-full p-6">
               {/* STORAGE FILTER */}
               <motion.div transition={{ duration: 0.3 }}>
                  <AccordionItem value="storages">
                     <AccordionTrigger className="hover:no-underline text-[15px] font-semibold pb-0 uppercase ">
                        Storages
                     </AccordionTrigger>
                     <AccordionContent className="pt-4">
                        <div className="grid grid-cols-3 gap-2">
                           {storageFilter.map((storage, index) => {
                              return (
                                 <Button
                                    key={index}
                                    onClick={() => toggleStorage(storage.name)}
                                    className={cn(
                                       'h-fit py-1 rounded-full text-[12px] font-normal border border-[#000] opacity-50 hover:opacity-100 bg-white text-black hover:bg-black hover:text-white',
                                       {
                                          'bg-black text-white opacity-100':
                                             selectedStorages.includes(
                                                storage.name,
                                             ),
                                       },
                                    )}
                                 >
                                    {storage.name}
                                 </Button>
                              );
                           })}
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
                           {colors.map((color, index) => {
                              return (
                                 <div
                                    key={index}
                                    onClick={() => toggleColor(color.name)}
                                    className={cn(
                                       'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out hover:ring-2 hover:ring-[#2563EB] hover:ring-offset-2 hover:ring-offset-white',
                                       {
                                          'ring-2 ring-[#2563EB] ring-offset-2 ring-offset-white':
                                             selectedColors.includes(
                                                color.name,
                                             ),
                                       },
                                    )}
                                    style={{
                                       backgroundColor: color.hex,
                                    }}
                                 ></div>
                              );
                           })}
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
                           onValueChange={setPriceFilter}
                           min={0}
                           max={1000}
                           step={100}
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
                     <div className="">
                        <div className="grid grid-cols-3 gap-2">
                           {models.map((model, index) => {
                              return (
                                 <Button
                                    key={index}
                                    onClick={() => toggleModel(model.value)}
                                    className={cn(
                                       'h-fit py-1 rounded-full select-none cursor-pointer text-center text-[12px] font-normal border border-[#000] opacity-50 hover:opacity-100 bg-white text-black hover:bg-black hover:text-white',
                                       {
                                          'bg-black text-white opacity-100':
                                             selectedModels.includes(
                                                model.value,
                                             ),
                                       },
                                    )}
                                 >
                                    {model.name}
                                 </Button>
                              );
                           })}
                        </div>
                     </div>
                  </AccordionContent>
               </AccordionItem>
            </Accordion>
         </div>
      </div>
   );
};

export default FilterSection;
