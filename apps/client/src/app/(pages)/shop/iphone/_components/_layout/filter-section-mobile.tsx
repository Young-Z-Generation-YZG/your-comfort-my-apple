'use client';

import { useState, useCallback, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { Button } from '@components/ui/button';
import { DualRangeSlider } from '@components/ui/dualRangeSlider';
import { cn } from '~/infrastructure/lib/utils';
import { useDebounce } from '@components/hooks/use-debounce';
import { useSearchParams } from 'next/navigation';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { GoMultiSelect } from 'react-icons/go';

const storageFilter = [
   { name: '64GB', normalizedName: '64GB', value: '64GB' },
   { name: '128GB', normalizedName: '128GB', value: '128GB' },
   { name: '256GB', normalizedName: '256GB', value: '256GB' },
   { name: '512GB', normalizedName: '512GB', value: '512GB' },
   { name: '1TB', normalizedName: '1TB', value: '1TB' },
];

const models = [
   { name: 'iPhone 15', normalizedName: 'IPHONE_15', value: 'iphone-15' },
   { name: 'iPhone 16', normalizedName: 'IPHONE_16', value: 'iphone-16' },
   { name: 'iPhone 16e', normalizedName: 'IPHONE_16E', value: 'iphone-16e' },
   {
      name: 'iPhone 16 Pro',
      normalizedName: 'IPHONE_16_PRO',
      value: 'iphone-16-pro',
   },
];

const colors = [
   { name: 'ultramarine', normalizedName: 'ULTRAMARINE', hex: '#9AADF6' },
   { name: 'green', normalizedName: 'GREEN', hex: '#D0D9CA' },
   { name: 'teal', normalizedName: 'TEAL', hex: '#B0D4D2' },
   { name: 'blue', normalizedName: 'BLUE', hex: '#D5DDDF' },
   { name: 'yellow', normalizedName: 'YELLOW', hex: '#EDE6C8' },
   { name: 'pink', normalizedName: 'PINK', hex: '#F2ADDA' },
   { name: 'white', normalizedName: 'WHITE', hex: '#FAFAFA' },
   { name: 'black', normalizedName: 'BLACK', hex: '#3C4042' },
];

type FilterSectionType = {
   className?: string;
   isOpen: boolean;
   onClose: () => void;
   minPrice?: number;
   maxPrice?: number;
};

export const MobileFilterSection = ({
   className,
   isOpen,
   onClose,
   minPrice,
   maxPrice,
}: FilterSectionType) => {
   const searchParams = useSearchParams();

   const [priceFilter, setPriceFilter] = useState<[number, number]>(() => {
      const min =
         minPrice ??
         (searchParams.get('_minPrice')
            ? Number(searchParams.get('_minPrice'))
            : 0);
      const max =
         maxPrice ??
         (searchParams.get('_maxPrice')
            ? Number(searchParams.get('_maxPrice'))
            : 2000);
      return [min, max];
   });

   // đồng bộ filter giá khi props thay đổi
   useEffect(() => {
      if (minPrice !== undefined || maxPrice !== undefined) {
         setPriceFilter([minPrice ?? 0, maxPrice ?? 2000]);
      }
   }, [minPrice, maxPrice]);

   const [selectedColors, setSelectedColors] = useState<string[]>(() =>
      searchParams.getAll('_colors'),
   );
   const [selectedModels, setSelectedModels] = useState<string[]>(() =>
      searchParams.getAll('_models'),
   );
   const [selectedStorages, setSelectedStorages] = useState<string[]>(() =>
      searchParams.getAll('_storages'),
   );

   // ✅ thêm state cho sắp xếp giá
   const [priceSort, setPriceSort] = useState<'ASC' | 'DESC' | null>(() => {
      const sortParam = searchParams.get('_priceSort');
      if (sortParam === 'ASC' || sortParam === 'DESC') return sortParam;
      return null;
   });

   const debouncedPriceFilter = useDebounce(priceFilter, 500);

   const toggleStorage = useCallback((name: string) => {
      setSelectedStorages((prev) =>
         prev.includes(name) ? prev.filter((s) => s !== name) : [...prev, name],
      );
   }, []);

   const toggleModel = useCallback((name: string) => {
      setSelectedModels((prev) =>
         prev.includes(name) ? prev.filter((m) => m !== name) : [...prev, name],
      );
   }, []);

   const toggleColor = useCallback((name: string) => {
      setSelectedColors((prev) =>
         prev.includes(name) ? prev.filter((c) => c !== name) : [...prev, name],
      );
   }, []);

   // ✅ Update URL params (bao gồm cả _priceSort)
   useEffect(() => {
      const params = new URLSearchParams();

      selectedColors.forEach((color) => params.append('_colors', color));
      selectedModels.forEach((model) => params.append('_models', model));
      selectedStorages.forEach((storage) =>
         params.append('_storages', storage),
      );

      if (debouncedPriceFilter[0] !== 0)
         params.set('_minPrice', debouncedPriceFilter[0].toString());
      if (debouncedPriceFilter[1] !== 2000)
         params.set('_maxPrice', debouncedPriceFilter[1].toString());

      if (priceSort) params.set('_priceSort', priceSort);

      const newUrl = params.toString()
         ? `${window.location.pathname}?${params.toString()}`
         : window.location.pathname;
      window.history.replaceState({}, '', newUrl);
   }, [
      selectedColors,
      selectedModels,
      selectedStorages,
      debouncedPriceFilter,
      priceSort,
   ]);

   const handleSortChange = (value: string) => {
      if (value === 'price-low-high') setPriceSort('ASC');
      else if (value === 'price-high-low') setPriceSort('DESC');
      else setPriceSort(null);
   };

   return (
      <div className={className}>
         <AnimatePresence>
            {isOpen && (
               <>
                  <motion.div
                     className="fixed inset-0 bg-black/50 z-40"
                     initial={{ opacity: 0 }}
                     animate={{ opacity: 1 }}
                     exit={{ opacity: 0 }}
                     onClick={() => onClose()}
                  />

                  <motion.div
                     className="fixed inset-y-0 right-0 w-[80%] max-w-xs bg-white z-50 p-6 flex flex-col overflow-y-auto"
                     initial={{ x: '100%' }}
                     animate={{ x: 0 }}
                     exit={{ x: '100%' }}
                     transition={{ type: 'tween', duration: 0.3 }}
                  >
                     {/* ✅ Sort by price */}
                     <div className="mb-6">
                        <h3 className="font-semibold text-sm mb-2">
                           Sort by price
                        </h3>
                        <Select
                           value={
                              priceSort === 'ASC'
                                 ? 'price-low-high'
                                 : priceSort === 'DESC'
                                   ? 'price-high-low'
                                   : 'recommended'
                           }
                           onValueChange={handleSortChange}
                        >
                           <SelectTrigger className="w-full border border-gray-300">
                              <GoMultiSelect className="mr-2" />
                              <SelectValue placeholder="Recommended" />
                           </SelectTrigger>
                           <SelectContent className="bg-white">
                              <SelectGroup>
                                 <SelectItem value="recommended">
                                    Recommended
                                 </SelectItem>
                                 <SelectItem value="price-low-high">
                                    Price: Low → High
                                 </SelectItem>
                                 <SelectItem value="price-high-low">
                                    Price: High → Low
                                 </SelectItem>
                              </SelectGroup>
                           </SelectContent>
                        </Select>
                     </div>

                     {/* Storage */}
                     <div className="mb-6">
                        <h3 className="font-semibold text-sm mb-2">Storage</h3>
                        <div className="grid grid-cols-3 gap-2">
                           {storageFilter.map((s) => (
                              <Button
                                 key={s.value}
                                 onClick={() => toggleStorage(s.normalizedName)}
                                 className={cn(
                                    'h-fit py-1 rounded-full text-[12px] border border-black bg-white text-black',
                                    {
                                       'bg-black text-white':
                                          selectedStorages.includes(
                                             s.normalizedName,
                                          ),
                                    },
                                 )}
                              >
                                 {s.name}
                              </Button>
                           ))}
                        </div>
                     </div>

                     {/* Colors */}
                     <div className="mb-6">
                        <h3 className="font-semibold text-sm mb-2">Colors</h3>
                        <div className="grid grid-cols-4 gap-2">
                           {colors.map((c) => (
                              <div
                                 key={c.name}
                                 onClick={() => toggleColor(c.normalizedName)}
                                 className={cn(
                                    'h-8 w-8 rounded-full border-2 cursor-pointer transition-all',
                                    {
                                       'ring-2 ring-blue-600':
                                          selectedColors.includes(
                                             c.normalizedName,
                                          ),
                                    },
                                 )}
                                 style={{ backgroundColor: c.hex }}
                              />
                           ))}
                        </div>
                     </div>

                     {/* Price */}
                     <div className="mb-6">
                        <h3 className="font-semibold text-sm mb-2">Price</h3>
                        <div className="flex justify-between text-sm mb-2">
                           <span>From: {priceFilter[0]}$</span>
                           <span>To: {priceFilter[1]}$</span>
                        </div>
                        <DualRangeSlider
                           value={priceFilter}
                           onValueChange={(v) =>
                              setPriceFilter(v as [number, number])
                           }
                           min={0}
                           max={2000}
                           step={50}
                        />
                     </div>

                     {/* Models */}
                     <div className="mb-6">
                        <h3 className="font-semibold text-sm mb-2">Models</h3>
                        <div className="grid grid-cols-3 gap-2">
                           {models.map((m) => (
                              <Button
                                 key={m.value}
                                 onClick={() => toggleModel(m.normalizedName)}
                                 className={cn(
                                    'h-fit py-1 rounded-full text-[12px] border border-black bg-white text-black',
                                    {
                                       'bg-black text-white':
                                          selectedModels.includes(
                                             m.normalizedName,
                                          ),
                                    },
                                 )}
                              >
                                 {m.name}
                              </Button>
                           ))}
                        </div>
                     </div>

                     {/* <Button
                        onClick={() => onClose()}
                        className="mt-auto bg-blue-600 text-white"
                     >
                        Apply Filters
                     </Button> */}
                  </motion.div>
               </>
            )}
         </AnimatePresence>
      </div>
   );
};
