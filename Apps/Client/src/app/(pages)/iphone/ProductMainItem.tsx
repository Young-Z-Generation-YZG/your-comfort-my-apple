/* eslint-disable react/react-in-jsx-scope */
'use client';
import Image from 'next/image';
import images from '~/components/client/images';
import { Button } from '~/components/ui/button';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import { Product, ProductItem } from '~/types/product/product.type';
interface ProductProps {
     product: Product;
     productItems: ProductItem[];
}

const ProductMainItem = ({ product, productItems }: ProductProps) => {

     return (
          <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay w-full h-fit bg-transparent')}>
               <div className='w-full flex flex-col'>
                    <div className='w-full flex items-end'>
                         <Image 
                              src={product.imageUrls[0]} 
                              alt="iPhone SE" 
                              className="w-auto h-[262px] mx-auto mb-[33px]" 
                              width={1000} 
                              height={1000} 
                              quality={100} 
                         />
                    </div>
                    <div className='w-full flex justify-center items-center gap-[6px] mb-[27px]'>
                         {productItems.map((item, index) => {
                              return (
                                   <div key={index} className='w-[11px] h-[11px] rounded-full 
                                   shadow-[rgba(9,30,66,0.25)_0px_4px_8px_-2px,rgba(9,30,66,0.08)_0px_0px_0px_1px]' 
                                   style={{backgroundColor: item.color}}/>
                              );
                         })}
                    </div>
                    <div className='text-[28px] font-semibold leading-[32px] text-[#1d1d1f] tracking-[0.007em] text-center mb-1 mt-3'>
                         {product.name}
                    </div>
                    <div className='text-[17px] font-normal leading-[25px] text-[#1d1d1f] tracking-[0.007em] text-center mb-1 mt-3'>
                         {product.description}
                    </div>
                    <div className='text-[17px] font-semibold leading-[25px] text-[#1d1d1f] tracking-[0.007em] text-center mb-1 mt-3'>
                         From ${productItems.length > 0 ? productItems[0].price : 'N/A'}
                    </div>
                    <div className='w-full flex justify-center items-center my-3'>
                         <Button className='w-[160px] h-fit rounded-[8px] bg-[#1d1d1f] text-[16px] 
                         text-white font-normal leading-[24px] tracking-[0.007em]'>
                              Buy now
                         </Button>
                    </div>
               </div>
               <div className='w-full border-t border-[#ccc] px-5 pt-[52px] flex flex-col items-center gap-12'>
                    <div className='intelligence w-full flex flex-col items-center'>
                         <Image 
                         src={images.iconIntelligence} 
                         alt='' 
                         className='w-[42px] h-[56px]'
                         width={1000} 
                         height={1000} 
                         quality={100}/>
                         <div className='text-[13px] text-[#1d1d1f] font-light leading-[20px] mt-3 mx-[36px]'>Apple Intelligence</div>
                    </div>
                    <div className='chip w-full flex flex-col items-center'>
                         <Image 
                         src={images.iconChipA16} 
                         alt='' 
                         className='w-[36px] h-auto'
                         width={1000} 
                         height={1000} 
                         quality={100}/>
                         <div className='text-[13px] text-[#1d1d1f] font-light leading-[20px] mt-3 mx-[36px]'>A16 Bionic chip with 5-core GPU</div>
                    </div>
                    <div className='camera-control w-full flex flex-col items-center'>
                         <Image 
                         src={images.iconCameraControl} 
                         alt='' 
                         className='w-[36px] h-auto'
                         width={1000} 
                         height={1000} 
                         quality={100}/>
                         <div className='text-[13px] text-[#1d1d1f] font-light leading-[20px] mt-1 mx-[36px]'>Camera Control</div>
                    </div>
                    <div className='camera w-full flex flex-col items-center'>
                         <Image 
                         src={images.iconCameraUltraWideTelephoto} 
                         alt='' 
                         className='w-[36px] h-auto'
                         width={1000} 
                         height={1000} 
                         quality={100}/>
                         <div className='text-[13px] text-[#1d1d1f] font-light leading-[20px] mt-1 mx-[36px]'>Pro camera system</div>
                         <div className='text-[13px] text-[#1d1d1f] font-light leading-[20px] mt-1 mx-[36px]'>Our most advanced 48MP Fusion camera</div>
                         <div className='text-[13px] text-[#1d1d1f] font-light leading-[20px] mt-1 mx-[36px]'>-</div>
                         <div className='text-[13px] text-[#1d1d1f] font-light leading-[20px] mt-1 mx-[36px]'>48MP Ultra Wide camera</div>
                    </div>
                    <div className='battery w-full flex flex-col items-center'>
                         <Image 
                         src={images.iconBattery} 
                         alt='' 
                         className='w-[48px] h-auto'
                         width={1000} 
                         height={1000} 
                         quality={100}/>
                         <div className='text-[13px] text-[#1d1d1f] font-light leading-[20px] mt-1 mx-[36px]'>Up to 33 hours video playback</div>
                    </div>
               </div>
          </div>
     );
};

export default ProductMainItem;

