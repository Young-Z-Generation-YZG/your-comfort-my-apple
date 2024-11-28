/* eslint-disable react/react-in-jsx-scope */
'use client';
import { useEffect, useState } from 'react';
import { useGetUsersQuery } from '~/services/example/user.service';
import '~/styles/globals.css';
import Image from 'next/image';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import {
   Carousel,
   CarouselApi,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from "~/components/ui/carousel"

import images from '~/components/client/images';
import KnowledgeItem from '~/app/(pages)/iphone/KnowledgeItem';
import { Product, ProductItem } from '~/types/product/product.type';
import ProductMainItem from '~/app/(pages)/iphone/ProductMainItem';
import { Select, SelectValue, SelectTrigger, SelectItem, SelectContent } from '~/components/ui/select';


const listKnowledgeItem = [
   {
      id: 1,
      title: 'AI-opening possibilities.',
      subtitle: 'Apple Intelligence',
      img: images.knowledgeAi,
      checkLightImg: false,
   },
   {
      id: 2,
      title: 'Picture your best photos and videos.',
      subtitle: 'Cutting-Edge Cameras',
      img: images.knowledgeCamera,
      checkLightImg: false,
   },
   {
      id: 3,
      title: 'Fast that lasts.',
      subtitle: 'Chip and Battery Life',
      img: images.knowledgeBattery,
      checkLightImg: false,
   },
   {
      id: 4,
      title: 'Beautiful and durable, by design.',
      subtitle: 'Innovation',
      img: images.knowledgeInnovation,
      checkLightImg: true,
   },
   {
      id: 5,
      title: 'Recycle. Reuse. Repeat.',
      subtitle: 'Environment',
      img: images.knowledgeEnvironment,
      checkLightImg: true,
   },
   {
      id: 6,
      title: 'Your data. Just where you want it.',
      subtitle: 'Privacy',
      img: images.knowledgePrivacy,
      checkLightImg: false,
   },
   {
      id: 7,
      title: 'Make it you. Through and through.',
      subtitle: 'Customize Your iPhone',
      img: images.knowledgePersonalize,
      checkLightImg: false,

   },
   {
      id: 8,
      title: 'Helpful safety features. Just in case.',
      subtitle: 'Peace of Mind',
      img: images.knowledgeSafety,
      checkLightImg: false,
   },
];



const listProduct: Product[] = [
   {
      id: "1",
      categoryId: "iphone",
      promotionId: "IPHONELOVER",
      name: "iPhone SE",
      description: "Serious power. Serious value.",
      averageRating: 4.5,
      imageUrls: [images.ipSE],
      imageIds: [],
      createdAt: new Date(),
      updatedAt: new Date()
   },
   {
      id: "2",
      categoryId: "iphone",
      promotionId: "IPHONELOVER",
      name: "iPhone 14",
      description: "All kinds of awesome.",
      averageRating: 4.5,
      imageUrls: [images.ip14],
      imageIds: [],
      createdAt: new Date(),
      updatedAt: new Date()
   },
   {
      id: "3",
      categoryId: "iphone",
      promotionId: "IPHONELOVER",
      name: "iPhone 15",
      description: "As amazing as ever.",
      averageRating: 4.5,
      imageUrls: [images.ip15],
      imageIds: [],
      createdAt: new Date(),
      updatedAt: new Date()
   },
   {
      id: "4",
      categoryId: "iphone",
      promotionId: "IPHONELOVER",
      name: "iPhone 16",
      description: "A total powerhouse.",
      averageRating: 4.5,
      imageUrls: [images.ip16],
      imageIds: [],
      createdAt: new Date(),
      updatedAt: new Date()
   },
   {
      id: "5",
      categoryId: "iphone",
      promotionId: "IPHONELOVER",
      name: "iPhone 16 Pro",
      description: "The ultimate iPhone.",
      averageRating: 4.5,
      imageUrls: [images.ip16Pro],
      imageIds: [],
      createdAt: new Date(),
      updatedAt: new Date()
   }
];

const listProductItem: ProductItem[] = [
   {
      id: '1',
      productId: '1',
      sku: '1',
      model: 'iPhone SE',
      color: '#232a31',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '2',
      productId: '1',
      sku: '1',
      model: 'iPhone SE',
      color: '#faf6f2',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '3',
      productId: '1',
      sku: '1',
      model: 'iPhone SE',
      color: '#bf0013',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '4',
      productId: '2',
      sku: '1',
      model: 'iPhone 14 Plus',
      color: '#a0b4c7',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '5',
      productId: '2',
      sku: '1',
      model: 'iPhone 14',
      color: '#e6ddeb',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '6',
      productId: '2',
      sku: '1',
      model: 'iPhone 14 Plus',
      color: '#f9e479',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '7',
      productId: '2',
      sku: '1',
      model: 'iPhone 14 Plus',
      color: '#2f363e',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '8',
      productId: '2',
      sku: '1',
      model: 'iPhone 14',
      color: '#faf6f3',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '9',
      productId: '2',
      sku: '1',
      model: 'iPhone 14',
      color: '#fc1837',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '10',
      productId: '3',
      sku: '1',
      model: 'iPhone 15',
      color: '#ebd0d2',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '11',
      productId: '3',
      sku: '1',
      model: 'iPhone 15',
      color: '#ece6c6',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '12',
      productId: '3',
      sku: '1',
      model: 'iPhone 15',
      color: '#cfd9c9',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '13',
      productId: '3',
      sku: '1',
      model: 'iPhone 15',
      color: '#d5dde0',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '14',
      productId: '3',
      sku: '1',
      model: 'iPhone 15',
      color: '#3c4042',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '15',
      productId: '4',
      sku: '1',
      model: 'iPhone 16',
      color: '#94a5eb',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '16',
      productId: '4',
      sku: '1',
      model: 'iPhone 16',
      color: '#abcfcd',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '17',
      productId: '4',
      sku: '1',
      model: 'iPhone 16',
      color: '#eda8d5',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '18',
      productId: '4',
      sku: '1',
      model: 'iPhone 16',
      color: '#f5f5f5',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '19',
      productId: '4',
      sku: '1',
      model: 'iPhone 16',
      color: '#35393b',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '20',
      productId: '5',
      sku: '1',
      model: 'iPhone 16 Pro',
      color: '#bfa48f',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '21',
      productId: '5',
      sku: '1',
      model: 'iPhone 16 Pro',
      color: '#c2bcb2',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '22',
      productId: '5',
      sku: '1',
      model: 'iPhone 16 Pro',
      color: '#3c3c3d',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   },
   {
      id: '23',
      productId: '5',
      sku: '1',
      model: 'iPhone 16 Pro',
      color: '#f7f6f2',
      storage: 128,
      imageUrls: [],
      imageIds: [],
      price: 1299,
      quantityInStock: 50,
      createdAt: new Date(),
      updatedAt: new Date(),
   }
];

const IphonePage = () => {
   const { data } = useGetUsersQuery('');

   useEffect(() => {
      console.log(data);
   }, [data]);

   const [selectedOption1, setSelectedOption1] = useState('1');
   const [selectedOption2, setSelectedOption2] = useState('1');
   const [selectedOption3, setSelectedOption3] = useState('1');
   const [selectedOption4, setSelectedOption4] = useState('1');
   const [selectedOption5, setSelectedOption5] = useState('1');

   return (
      <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay w-full flex flex-col items-start justify-center bg-[#f5f5f7]')}>
         <div className='welcome-container w-full pt-[80px]'>
            <div className='welcome-title w-[88.49%] mx-auto flex flex-row items-end pb-[80px]'>
               <div className='flex-1 text-[80px] font-semibold leading-[84px] text-[#1D1D1F]'>iPhone</div>
               <div className='flex-none text-[28px] font-semibold leading-[32px] py-2 text-[#1D1D1F]'>Designed to be loved.</div>
            </div>
            <div className='welcome-content w-full'>
               <Image 
               className='w-full h-auto'
               src={images.welcome} 
               alt='iphone' 
               width={1000} 
               height={1000} />
            </div>
         </div>

         <div className='knowledge-container w-full pt-[150px] mb-[50px]'>
               <div className='title-list w-[88.49%] mx-auto pb-[60px]'>
                  <span className='text-[56px] font-semibold text-[#1d1d1f] leading-[1.14] tracking-[0.007em]'>Get to know iPhone. </span>
               </div>
               <Carousel className="w-full max-w-screen relative flex justify-center items-center">
                  <CarouselContent className="w-screen pl-[93px] mb-5">
                     {listKnowledgeItem.map((item, index) => {
                        return (
                           <CarouselItem key={index} className="basis-[30%] mr-2">
                                 <KnowledgeItem item={item} />
                           </CarouselItem>
                        );
                     })}
                  </CarouselContent>
                  <CarouselPrevious className='absolute top-1/2 -translate-y-1/2 left-0'/>
                  <CarouselNext className='absolute top-1/2 -translate-y-1/2 right-0'/>
               </Carousel>
         </div>

         <div className='compare-container w-full pt-[150px]'>
            <div className='title-list w-[88.49%] mx-auto pb-[80px]'>
               <div className='text-[56px] font-semibold text-[#1d1d1f] leading-[1.14] tracking-[0.007em]'>Explore the lineup.</div>    
            </div>
            <Carousel className="w-full relative">
                  <CarouselContent className="ml-[93px]">
                     {[...listProduct].reverse().map((product, index) => {
                        const productItems = listProductItem.filter(item => item.productId === product.id);
                        return (
                           <CarouselItem key={index} className="basis-[372px] pl-0">
                              <ProductMainItem product={product} productItems={productItems} />
                           </CarouselItem>
                        );
                     })}
                  </CarouselContent>
                  <CarouselPrevious className='absolute top-1/2 -translate-y-1/2 left-0'/>
                  <CarouselNext className='absolute top-1/2 -translate-y-1/2 right-0'/>
            </Carousel>
         </div>
      </div>
   );
};

export default IphonePage;

