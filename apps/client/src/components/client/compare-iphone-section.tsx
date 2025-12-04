'use client';

import { MdOutlineArrowDropDown, MdArrowDropUp } from 'react-icons/md';
import {
   Select,
   SelectContent,
   SelectTrigger,
   SelectValue,
   SelectItem,
} from '@components/ui/select';
import CompareItem, { CompareItemType } from './compare-item';
import { Button } from '@components/ui/button';
import { useState } from 'react';
import { cn } from '~/infrastructure/lib/utils';

const listProduct = [
   {
      id: 'ip14proMax',
      checkNew: false,
      name: 'iPhone 14 Pro Max',
      image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_14_pro_max_twilight_purple_large.png?1677709034078',
      price: 999,
      colors: ['#594f63', '#f4e8ce', '#f0f2f2', '#403e3d'],
      screen: [
         '6,7"',
         'Super Retina XDR display',
         'ProMotion technology',
         'Always-On display',
      ], // max 4 item
      checkDynamic: true,
      chip: [16, 'A16 Bionic chip with 5-core GPU'], // max 2 item
      battery: 'Up to 29 hours video playback',
      biometricAuthen: 'Face ID',
      crashDetection: ['Emergency SOS', 'Crash Detection'], //max 5 item
      camera: [
         'ultraWT',
         'Pro camera system',
         '48MP Fusion | Ultra Wide Telephoto',
         'Photonic Engine cho màu sắc và chi tiết ấn tượng',
         'Camera trước TrueDepth có khả năng tự động lấy nét',
      ], // max 9 item
      material: ['Titanium with textured matte glass back', 'Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C', 'Supports USB 2'],
   },
   {
      id: 'ip14plus',
      checkNew: false,
      name: 'iPhone 14 Plus',
      image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_14_plus_blue_large.png?1677709034078',
      price: 499,
      colors: [
         '#a0b4c7',
         '#e6ddeb',
         '#f9e479',
         '#222930',
         '#faf6f2',
         '#fc0324',
      ],
      screen: ['6,7"', 'Super Retina XDR display'], // max 4 item
      checkDynamic: false,
      chip: [15, 'A15 Bionic chip with 5-core GPU'], // max 2 item
      battery: 'Up to 26 hours video playback',
      biometricAuthen: 'Face ID',
      crashDetection: ['Emergency SOS', 'Crash Detection'], //max 5 item
      camera: [
         'ultraWX',
         'Advanced dual-camera system',
         '12MP Fusion | Ultra Wide',
         'Photonic Engine cho màu sắc và chi tiết ấn tượng',
         'Camera trước TrueDepth có khả năng tự động lấy nét',
      ], // max 9 item
      material: ['Titanium with textured matte glass back', 'Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C', 'Supports USB 2'],
   },
   {
      id: 'ip13proMax',
      checkNew: false,
      name: 'iPhone 13 Pro Max',
      image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_13_pro_max_alpine_green_large.png?1677709034080',
      price: 599,
      colors: ['#576856', '#f1f2ed', '#fae7cf', '#54524f', '#a7c1d9'],
      screen: ['6,7"', 'Super Retina XDR display', 'ProMotion technology'], // max 4 item
      checkDynamic: false,
      chip: [15, 'A15 Bionic chip with 5-core GPU'], // max 2 item
      battery: 'Up to 28 hours video playback',
      biometricAuthen: 'Face ID',
      crashDetection: ['Emergency SOS'], //max 5 item
      camera: [
         'ultraWT',
         'Pro camera system',
         '12MP Fusion | Ultra Wide Telephoto',
         'TrueDepth Camera',
      ], // max 9 item
      material: ['Titanium with textured matte glass back', 'Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C', 'Supports USB 2'],
   },
   {
      id: 'ip13mini',
      checkNew: false,
      name: 'iPhone 13 Mini',
      image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_13_mini_green_large.png?1677709034086',
      price: 499,
      colors: [
         '#394c38',
         '#faddd7',
         '#276787',
         '#232a31',
         '#faf6f2',
         '#be0013',
      ],
      screen: ['5,4"', 'Super Retina XDR display'], // max 4 item
      checkDynamic: false,
      chip: [15, 'A15 Bionic chip with 4-core GPU'], // max 2 item
      battery: 'Up to 17 hours video playback',
      biometricAuthen: 'Face ID',
      crashDetection: ['Emergency SOS'], //max 5 item
      camera: [
         'ultraWX',
         'Dual-camera system',
         '12MP Fusion | Ultra Wide',
         'TrueDepth Camera',
      ], // max 9 item
      material: ['Titanium with textured matte glass back', 'Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C', 'Supports USB 2'],
   },
   {
      id: 'ip12mini',
      checkNew: false,
      name: 'iPhone 12 Mini',
      image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_12_mini_purple_large.png?1677709034084',
      price: 459,
      colors: [
         '#b7afe6',
         '#023b63',
         '#d8efd5',
         '#d82e2e',
         '#f6f2ef',
         '#25212b',
      ],
      screen: ['5,4"', 'Super Retina XDR display'], // max 4 item
      checkDynamic: false,
      chip: [14, 'A14 Bionic chip with 4-core GPU'], // max 2 item
      battery: 'Up to 15 hours video playback',
      biometricAuthen: 'Face ID',
      crashDetection: ['Emergency SOS'], //max 5 item
      camera: [
         'ultraWV',
         'Dual-camera system',
         '12MP Fusion | Ultra Wide',
         'TrueDepth Camera',
      ], // max 9 item
      material: ['Titanium with textured matte glass back', 'Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C', 'Supports USB 2'],
   },
   {
      id: 'ipXSMax',
      checkNew: false,
      name: 'iPhone XS Max',
      image: 'https://shopdunk.com/images/uploaded-source/compareiphone/header_iphone_xs_max_silver_large.png?1677709034078',
      price: 599,
      colors: ['#e4e4e2', '#262529', '#fadcc2'],
      screen: ['6,5"', 'Super Retina HD display'], // max 4 item
      checkDynamic: false,
      chip: [12, 'A12 Bionic chip with 4-core GPU'], // max 2 item
      battery: 'Up to 15 hours video playback',
      biometricAuthen: 'Face ID',
      crashDetection: ['Emergency SOS'], //max 5 item
      camera: [
         'telephoto',
         'Dual-camera system',
         '12MP Fusion | Telephoto',
         'TrueDepth Camera',
      ], // max 9 item
      material: ['Titanium with textured matte glass back', 'Action button'], // max 2 item
      description: '',
      checkCameraControl: false,
      checkAppIntell: false,
      typeConnect: ['USB‑C', 'Supports USB 2'],
   },
];

const CompareIPhoneSection = () => {
   const [selectedOption1, setSelectedOption1] = useState('ip14proMax');
   const [selectedOption2, setSelectedOption2] = useState('ip14plus');
   const [selectedOption3, setSelectedOption3] = useState('ip13proMax');

   const [showDetailCompare, setShowDetailCompare] = useState(false);

   return (
      <div>
         <div className="compare-container w-full mb-[100px] flex flex-col justify-center items-center px-4">
            <div className="compare-title bg-transparent w-full max-w-[996px] mx-auto text-center pb-[70px]">
               <div className="text-base md:text-[20px] font-bold">Compare</div>
               <div className="text-2xl md:text-[40px] font-bold">
                  Which iPhone is right for you?
               </div>
            </div>
            <div className="compare-content bg-transparent w-full max-w-[996px] mx-auto flex flex-col">
               <div className="w-full grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-3 mb-10">
                  <div className="bg-gray-300 flex flex-col items-center rounded-[10px]">
                     <Select
                        value={selectedOption1}
                        onValueChange={(value) => setSelectedOption1(value)}
                     >
                        <SelectTrigger className="w-full text-base font-medium bg-white">
                           <SelectValue />
                        </SelectTrigger>
                        <SelectContent className="bg-white">
                           {listProduct.map((product: any, index: number) => (
                              <SelectItem value={product.id} key={index}>
                                 {product.name}
                              </SelectItem>
                           ))}
                        </SelectContent>
                     </Select>
                  </div>
                  <div className="bg-gray-300 flex-col items-center rounded-[10px] hidden md:flex">
                     <Select
                        value={selectedOption2}
                        onValueChange={(value) => setSelectedOption2(value)}
                     >
                        <SelectTrigger className="w-full text-base font-medium bg-white">
                           <SelectValue />
                        </SelectTrigger>
                        <SelectContent className="bg-white">
                           {listProduct.map((product: any, index: number) => (
                              <SelectItem value={product.id} key={index}>
                                 {product.name}
                              </SelectItem>
                           ))}
                        </SelectContent>
                     </Select>
                  </div>
                  <div className="bg-gray-300 flex-col items-center rounded-[10px] hidden lg:flex">
                     <Select
                        value={selectedOption3}
                        onValueChange={(value) => setSelectedOption3(value)}
                     >
                        <SelectTrigger className="w-full text-base font-medium bg-white">
                           <SelectValue />
                        </SelectTrigger>
                        <SelectContent className="bg-white">
                           {listProduct.map((product: any, index: number) => (
                              <SelectItem value={product.id} key={index}>
                                 {product.name}
                              </SelectItem>
                           ))}
                        </SelectContent>
                     </Select>
                  </div>
               </div>
               <div
                  className={cn(
                     'w-full grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-3',
                     showDetailCompare ? 'h-fit' : 'h-[1250px] overflow-hidden',
                  )}
               >
                  <div>
                     <CompareItem
                        compare={
                           listProduct.find(
                              (product: CompareItemType) =>
                                 product.id === selectedOption1,
                           ) || {
                              id: '',
                              checkNew: false,
                              name: '',
                              image: '',
                              price: 0,
                              colors: [],
                              screen: [],
                              checkDynamic: false,
                              chip: [],
                              battery: '',
                              biometricAuthen: '',
                              crashDetection: [],
                              camera: [],
                              material: [],
                              description: '',
                              checkCameraControl: false,
                              checkAppIntell: false,
                              typeConnect: [],
                           }
                        }
                     />
                  </div>
                  <div className="hidden md:block">
                     <CompareItem
                        compare={
                           listProduct.find(
                              (product: CompareItemType) =>
                                 product.id === selectedOption2,
                           ) || {
                              id: '',
                              checkNew: false,
                              name: '',
                              image: '',
                              price: 0,
                              colors: [],
                              screen: [],
                              checkDynamic: false,
                              chip: [],
                              battery: '',
                              biometricAuthen: '',
                              crashDetection: [],
                              camera: [],
                              material: [],
                              description: '',
                              checkCameraControl: false,
                              checkAppIntell: false,
                              typeConnect: [],
                           }
                        }
                     />
                  </div>
                  <div className="hidden lg:block">
                     <CompareItem
                        compare={
                           listProduct.find(
                              (product: CompareItemType) =>
                                 product.id === selectedOption3,
                           ) || {
                              id: '',
                              checkNew: false,
                              name: '',
                              image: '',
                              price: 0,
                              colors: [],
                              screen: [],
                              checkDynamic: false,
                              chip: [],
                              battery: '',
                              biometricAuthen: '',
                              crashDetection: [],
                              camera: [],
                              material: [],
                              description: '',
                              checkCameraControl: false,
                              checkAppIntell: false,
                              typeConnect: [],
                           }
                        }
                     />
                  </div>
               </div>
               <div
                  className={cn(
                     'w-full flex items-center justify-center mt-10',
                  )}
               >
                  <Button
                     onClick={() => setShowDetailCompare(!showDetailCompare)}
                     className="font-thin text-xl text-blue-500 bg-transparent hover:bg-transparent border-b border-b-blue-500 rounded-none hover:text-blue-600 py-0 h-fit"
                     variant={'secondary'}
                  >
                     {showDetailCompare ? 'Collapse' : 'Expand'}
                     {showDetailCompare ? (
                        <MdArrowDropUp />
                     ) : (
                        <MdOutlineArrowDropDown />
                     )}
                  </Button>
               </div>
            </div>
         </div>
      </div>
   );
};

export default CompareIPhoneSection;
