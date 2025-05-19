'use client';

import ContentWrapper from '@components/ui/content-wrapper';
import { Fragment, useState } from 'react';
import IphoneModelItem from './_components/model-item';
import { iPhoneModelData } from './_data.demo';
import { motion } from 'framer-motion';
import { Button } from '@components/ui/button';
import { Input } from '@components/ui/input';
import { Badge } from '@components/ui/badge';
import Link from 'next/link';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import {
   ChevronDown,
   Filter,
   Grid3X3,
   List,
   Plus,
   Search,
   SlidersHorizontal,
   Sparkles,
} from 'lucide-react';

// Mock iPhone specific products
const iPhoneProducts = [
   {
      id: 'iphone-16-pro-max-128gb-pink',
      general_model: 'iphone-16-pro-max',
      model: 'iPhone 16 Pro Max',
      color: {
         color_name: 'ultramarine',
         color_hex: '#9AADF6',
         color_Image:
            'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-ultramarine-202409',
      },
      storage: {
         Name: '128GB',
         Value: 128,
      },
      unit_price: 1199,
      available_in_stock: 100,
      total_sold: 0,
      state: {
         Name: 'ACTIVE',
         Value: 0,
      },
      images: [
         {
            image_id: 'image_id_1',
            image_url: '/images/iphone16.png',
            image_order: 0,
         },
      ],
      slug: 'iphone-16-pro-max-128gb-pink',
      iphone_model_id: '67346f7549189f7314e4ef0c',
      isNew: true,
      promotion: {
         id: 'prom_1',
         name: 'Summer Sale 2024',
         type: 'event',
         discountType: 'percentage',
         discountValue: 10,
         startDate: '2024-06-01',
         endDate: '2024-06-30',
         theme: 'default',
      },
      ratings: {
         average: 4.6,
         total: 100,
         breakdown: {
            '5': 50,
            '4': 20,
            '3': 15,
            '2': 10,
            '1': 5,
         },
      },
   },
   {
      id: 'iphone-16-pro-128gb-natural-titanium',
      general_model: 'iphone-16-pro',
      model: 'iPhone 16 Pro',
      color: {
         color_name: 'natural titanium',
         color_hex: '#BFBFBD',
         color_Image:
            'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-titanium-202309',
      },
      storage: {
         Name: '128GB',
         Value: 128,
      },
      unit_price: 999,
      available_in_stock: 50,
      total_sold: 25,
      state: {
         Name: 'ACTIVE',
         Value: 0,
      },
      images: [
         {
            image_id: 'image_id_1',
            image_url: '/images/iphone16.png',
            image_order: 0,
         },
      ],
      slug: 'iphone-16-pro-128gb-natural-titanium',
      iphone_model_id: '67346f7549189f7314e4ef0d',
      isNew: true,
      promotion: {
         id: 'prom_2',
         name: 'Black Friday Deal',
         type: 'event',
         discountType: 'percentage',
         discountValue: 15,
         startDate: '2024-11-25',
         endDate: '2024-11-30',
         theme: 'black-friday',
      },
      ratings: {
         average: 4.8,
         total: 75,
         breakdown: {
            '5': 60,
            '4': 10,
            '3': 3,
            '2': 1,
            '1': 1,
         },
      },
   },
   {
      id: 'iphone-15-128gb-blue',
      general_model: 'iphone-15',
      model: 'iPhone 15',
      color: {
         color_name: 'blue',
         color_hex: '#9BB5CE',
         color_Image:
            'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-blue-202309',
      },
      storage: {
         Name: '128GB',
         Value: 128,
      },
      unit_price: 699,
      available_in_stock: 30,
      total_sold: 120,
      state: {
         Name: 'ACTIVE',
         Value: 0,
      },
      images: [
         {
            image_id: 'image_id_1',
            image_url: '/images/iphone16.png',
            image_order: 0,
         },
      ],
      slug: 'iphone-15-128gb-blue',
      iphone_model_id: '67346f7549189f7314e4ef0e',
      isNew: false,
      promotion: {
         id: 'prom_3',
         name: 'Winter Holiday Special',
         type: 'product',
         discountType: 'fixed',
         discountValue: 100,
         startDate: '2024-12-01',
         endDate: '2024-12-31',
         theme: 'winter',
      },
      ratings: {
         average: 4.2,
         total: 150,
         breakdown: {
            '5': 80,
            '4': 40,
            '3': 20,
            '2': 5,
            '1': 5,
         },
      },
   },
   {
      id: 'iphone-13-pro-max-128gb-blue',
      general_model: 'iphone-13-pro-max',
      model: 'iPhone 13 Pro Max',
      color: {
         color_name: 'blue',
         color_hex: '#9BB5CE',
         color_Image:
            'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-blue-202309',
      },
      storage: {
         Name: '128GB',
         Value: 128,
      },
      unit_price: 599,
      available_in_stock: 15,
      total_sold: 250,
      state: {
         Name: 'ACTIVE',
         Value: 0,
      },
      images: [
         {
            image_id: 'image_id_1',
            image_url: '/images/iphone16.png',
            image_order: 0,
         },
      ],
      slug: 'iphone-13-pro-max-128gb-blue',
      iphone_model_id: '67346f7549189f7314e4ef0f',
      isNew: false,
      ratings: {
         average: 4.5,
         total: 200,
         breakdown: {
            '5': 120,
            '4': 50,
            '3': 20,
            '2': 5,
            '1': 5,
         },
      },
   },
];

const container = {
   hidden: { opacity: 0 },
   show: {
      opacity: 1,
      transition: {
         staggerChildren: 0.1,
      },
   },
};

const IPhoneModelsPage = () => {
   const [searchQuery, setSearchQuery] = useState('');
   const [sortBy, setSortBy] = useState('newest');
   const [viewMode, setViewMode] = useState('list');
   const [filterModel, setFilterModel] = useState('all');
   const [filterStorage, setFilterStorage] = useState('all');
   const [filterPromotion, setFilterPromotion] = useState('all');

   // Filter and sort iPhones
   const filteredIPhones = iPhoneProducts.filter((iphone) => {
      const matchesSearch = iphone.model
         .toLowerCase()
         .includes(searchQuery.toLowerCase());
      const matchesModel =
         filterModel === 'all' || iphone.general_model === filterModel;
      const matchesStorage =
         filterStorage === 'all' ||
         iphone.storage.Name.toLowerCase() === filterStorage.toLowerCase();
      const matchesPromotion =
         filterPromotion === 'all' ||
         (filterPromotion === 'on-promotion' && iphone.promotion) ||
         (filterPromotion === 'not-on-promotion' && !iphone.promotion);

      return (
         matchesSearch && matchesModel && matchesStorage && matchesPromotion
      );
   });

   return (
      <Fragment>
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center justify-between">
                     <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                           iPhone Models
                        </h1>
                        <p className="text-muted-foreground">
                           View and manage all iPhone models in your inventory
                        </p>
                     </div>
                     <Button asChild className="rounded-full">
                        <Link href="/dashboards/iphone/models/create">
                           <Plus className="mr-2 h-4 w-4" />
                           Add iPhone
                        </Link>
                     </Button>
                  </div>

                  <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
                     <div className="relative w-full max-w-sm">
                        <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                        <Input
                           type="search"
                           placeholder="Search iPhone models..."
                           className="w-full pl-8 rounded-full"
                           value={searchQuery}
                           onChange={(e) => setSearchQuery(e.target.value)}
                        />
                     </div>

                     <div className="flex flex-wrap items-center gap-2">
                        <DropdownMenu>
                           <DropdownMenuTrigger asChild>
                              <Button
                                 variant="outline"
                                 size="sm"
                                 className="h-8 gap-1 rounded-full"
                              >
                                 <Filter className="h-3.5 w-3.5" />
                                 <span className="hidden sm:inline-block">
                                    Filter
                                 </span>
                              </Button>
                           </DropdownMenuTrigger>
                           <DropdownMenuContent
                              align="end"
                              className="w-[200px]"
                           >
                              <div className="p-2">
                                 <div className="mb-2 text-xs font-medium">
                                    Model
                                 </div>
                                 <Select
                                    value={filterModel}
                                    onValueChange={setFilterModel}
                                 >
                                    <SelectTrigger className="h-8">
                                       <SelectValue placeholder="All Models" />
                                    </SelectTrigger>
                                    <SelectContent>
                                       <SelectItem value="all">
                                          All Models
                                       </SelectItem>
                                       <SelectItem value="iphone-16-pro-max">
                                          iPhone 16 Pro Max
                                       </SelectItem>
                                       <SelectItem value="iphone-16-pro">
                                          iPhone 16 Pro
                                       </SelectItem>
                                       <SelectItem value="iphone-15">
                                          iPhone 15
                                       </SelectItem>
                                       <SelectItem value="iphone-13-pro-max">
                                          iPhone 13 Pro Max
                                       </SelectItem>
                                    </SelectContent>
                                 </Select>
                              </div>
                              <div className="p-2">
                                 <div className="mb-2 text-xs font-medium">
                                    Storage
                                 </div>
                                 <Select
                                    value={filterStorage}
                                    onValueChange={setFilterStorage}
                                 >
                                    <SelectTrigger className="h-8">
                                       <SelectValue placeholder="All Storage" />
                                    </SelectTrigger>
                                    <SelectContent>
                                       <SelectItem value="all">
                                          All Storage
                                       </SelectItem>
                                       <SelectItem value="64GB">
                                          64GB
                                       </SelectItem>
                                       <SelectItem value="128GB">
                                          128GB
                                       </SelectItem>
                                       <SelectItem value="256GB">
                                          256GB
                                       </SelectItem>
                                       <SelectItem value="512GB">
                                          512GB
                                       </SelectItem>
                                       <SelectItem value="1TB">1TB</SelectItem>
                                    </SelectContent>
                                 </Select>
                              </div>
                              <div className="p-2">
                                 <div className="mb-2 text-xs font-medium">
                                    Promotion
                                 </div>
                                 <Select
                                    value={filterPromotion}
                                    onValueChange={setFilterPromotion}
                                 >
                                    <SelectTrigger className="h-8">
                                       <SelectValue placeholder="All Products" />
                                    </SelectTrigger>
                                    <SelectContent>
                                       <SelectItem value="all">
                                          All Products
                                       </SelectItem>
                                       <SelectItem value="on-promotion">
                                          On Promotion
                                       </SelectItem>
                                       <SelectItem value="not-on-promotion">
                                          Not On Promotion
                                       </SelectItem>
                                    </SelectContent>
                                 </Select>
                              </div>
                           </DropdownMenuContent>
                        </DropdownMenu>

                        <DropdownMenu>
                           <DropdownMenuTrigger asChild>
                              <Button
                                 variant="outline"
                                 size="sm"
                                 className="h-8 gap-1 rounded-full"
                              >
                                 <SlidersHorizontal className="h-3.5 w-3.5" />
                                 <span className="hidden sm:inline-block">
                                    Sort
                                 </span>
                                 <ChevronDown className="h-3.5 w-3.5" />
                              </Button>
                           </DropdownMenuTrigger>
                           <DropdownMenuContent align="end">
                              <DropdownMenuItem
                                 onClick={() => setSortBy('newest')}
                              >
                                 Newest First
                              </DropdownMenuItem>
                              <DropdownMenuItem
                                 onClick={() => setSortBy('price-high')}
                              >
                                 Price: High to Low
                              </DropdownMenuItem>
                              <DropdownMenuItem
                                 onClick={() => setSortBy('price-low')}
                              >
                                 Price: Low to High
                              </DropdownMenuItem>
                              <DropdownMenuItem
                                 onClick={() => setSortBy('stock-high')}
                              >
                                 Stock: High to Low
                              </DropdownMenuItem>
                              <DropdownMenuItem
                                 onClick={() => setSortBy('best-selling')}
                              >
                                 Best Selling
                              </DropdownMenuItem>
                           </DropdownMenuContent>
                        </DropdownMenu>

                        <div className="flex items-center rounded-full border overflow-hidden">
                           <Button
                              variant={
                                 viewMode === 'list' ? 'default' : 'ghost'
                              }
                              size="sm"
                              className="h-8 rounded-none px-2"
                              onClick={() => setViewMode('list')}
                           >
                              <List className="h-4 w-4" />
                           </Button>
                           <Button
                              variant={
                                 viewMode === 'grid' ? 'default' : 'ghost'
                              }
                              size="sm"
                              className="h-8 rounded-none px-2"
                              onClick={() => setViewMode('grid')}
                           >
                              <Grid3X3 className="h-4 w-4" />
                           </Button>
                        </div>
                     </div>
                  </div>

                  {/* Promotion filter quick buttons */}
                  <div className="flex flex-wrap gap-2">
                     <Button
                        variant={
                           filterPromotion === 'all' ? 'default' : 'outline'
                        }
                        size="sm"
                        className="rounded-full"
                        onClick={() => setFilterPromotion('all')}
                     >
                        All Products
                     </Button>
                     <Button
                        variant={
                           filterPromotion === 'on-promotion'
                              ? 'default'
                              : 'outline'
                        }
                        size="sm"
                        className="rounded-full bg-gradient-to-r from-amber-500 to-rose-500 text-white hover:from-amber-600 hover:to-rose-600 hover:text-white"
                        onClick={() => setFilterPromotion('on-promotion')}
                     >
                        <Sparkles className="mr-2 h-4 w-4" />
                        On Promotion
                     </Button>
                  </div>

                  {filteredIPhones.length === 0 ? (
                     <div className="flex h-[300px] items-center justify-center rounded-md border border-dashed">
                        <div className="flex flex-col items-center gap-1 text-center">
                           <p className="text-sm text-muted-foreground">
                              No iPhone models found
                           </p>
                           <Button
                              variant="link"
                              size="sm"
                              className="mt-2"
                              onClick={() => {
                                 setSearchQuery('');
                                 setFilterModel('all');
                                 setFilterStorage('all');
                                 setFilterPromotion('all');
                              }}
                           >
                              Reset filters
                           </Button>
                        </div>
                     </div>
                  ) : (
                     <motion.div
                        variants={container}
                        initial="hidden"
                        animate="show"
                        className="flex flex-col gap-4"
                     >
                        {/* {filteredIPhones.map((iphone) => (
                     <IPhoneListItem
                        key={iphone.id}
                        iphone={iphone}
                        viewMode={viewMode}
                     />
                  ))} */}
                        {iPhoneModelData.items.map((item, index) => {
                           return <IphoneModelItem key={index} item={item} />;
                        })}
                     </motion.div>
                  )}

                  <div className="flex items-center justify-between">
                     <div className="text-sm text-muted-foreground">
                        Showing <strong>{filteredIPhones.length}</strong> of{' '}
                        <strong>{iPhoneProducts.length}</strong> iPhone models
                     </div>
                     <div className="flex items-center gap-2">
                        <Badge
                           variant="outline"
                           className="px-2 py-1 rounded-full"
                        >
                           {filterModel === 'all'
                              ? 'All Models'
                              : filterModel
                                   .replace('-', ' ')
                                   .replace(/\b\w/g, (l) => l.toUpperCase())}
                        </Badge>
                        <Badge
                           variant="outline"
                           className="px-2 py-1 rounded-full"
                        >
                           {filterStorage === 'all'
                              ? 'All Storage'
                              : filterStorage}
                        </Badge>
                        {filterPromotion !== 'all' && (
                           <Badge
                              variant="outline"
                              className="px-2 py-1 rounded-full"
                           >
                              {filterPromotion === 'on-promotion'
                                 ? 'On Promotion'
                                 : 'Not On Promotion'}
                           </Badge>
                        )}
                     </div>
                  </div>
               </div>
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default IPhoneModelsPage;
