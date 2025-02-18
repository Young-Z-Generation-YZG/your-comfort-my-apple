/* eslint-disable react/react-in-jsx-scope */
'use client';
import { Fullscreen } from 'lucide-react';
import Image from 'next/image';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';

const CompareItem = ({compare}: {compare: any}) => {
     const {
          id,
          checkNew,
          name,
          image,
          description,
          price,
          colors,
          screen,
          material,
          checkAppIntell,
          checkDynamic,
          chip,
          battery,
          biometricAuthen,
          crashDetection,
          camera,
          checkCameraControl,
          typeConnect
     } = compare;
     
     let imgBiometricAuthen = biometricAuthen === "Face ID" ? 
                                                  '/images/compare-imgs/icon-biometric-authen/icon-face-id.jpg' : 
                                                  '/images/compare-imgs/icon-biometric-authen/icon-touch-id.jpg';
     
     let imgChip='';
     switch (chip[0]) {
          case 16:
               imgChip = '/images/compare-imgs/icon-chip/icon-A16-chip.png';
               break;
          case 15:
               imgChip = '/images/compare-imgs/icon-chip/icon-A15-chip.png';
               break;
          case 14:
               imgChip = '/images/compare-imgs/icon-chip/icon-A14-chip.png';
               break;
          case 13:
               imgChip = '/images/compare-imgs/icon-chip/icon-A13-chip.png';
               break;
          default:
               imgChip = '/images/compare-imgs/icon-chip/icon-A16-chip.png';
               break;
     }

     let imgCamera = '';
     switch (camera[0]) {
          case 'ultraWT':
               imgCamera = '/images/compare-imgs/icon-camera/icon-ultra-wide-telephoto.png';
               break;
          case 'ultraWV':
               imgCamera = '/images/compare-imgs/icon-camera/icon-ultra-wide-vertical.png';
               break;
          case 'ultraWX':
               imgCamera = '/images/compare-imgs/icon-camera/icon-ultra-wide-X.png';
               break;
          case 'telephoto':
               imgCamera = '/images/compare-imgs/icon-camera/icon-telephoto.png';
               break;
          case 'singleV':
               imgCamera = '/images/compare-imgs/icon-camera/icon-single-vertical.png';
               break;
          case 'singleH':
               imgCamera = '/images/compare-imgs/icon-camera/icon-single-horizontal.png';
               break;
          default:
               imgCamera = '/images/compare-imgs/icon-camera/icon-ultra-wide-telephoto.png';
               break;
     }

     return (
          <div className={cn(SFDisplayFont.variable, "font-SFProDisplay basis-1/5 flex flex-col items-center justify-center h-fit ")}>
               <div className='product-img w-full'>
                    <div className='w-[164px] h-[210px] mx-auto flex items-end justify-center'>
                         <Image  
                              className='h-auto w-auto' src={image} 
                              alt="Picture of the author" 
                              width={1000} 
                              height={1000} 
                              quality={100}/>
                    </div>
               </div>

               <div className='product-colors flex flex-col mx-auto pt-[23px] pb-[38px] relative justify-start h-[100px]'>
                    <div className='flex flex-row gap-2 pb-[10px] justify-center'>
                         {colors.slice(0,4).map((color: any, index: any) => (
                         <div className='product-color rounded-[50%] h-[15px] w-[15px] shadow-[rgba(0,0,0,0.15)_1.95px_1.95px_2.6px]' key={index} style={{backgroundColor: color}}/>
                         ))}
                    </div>
                    <div className='flex flex-row gap-2 pb-[10px] justify-center '>
                         {colors.slice(4,8).map((color: any, index: any) => (
                         <div className='product-color rounded-[50%] h-[15px] w-[15px] shadow-[rgba(0,0,0,0.15)_1.95px_1.95px_2.6px]' key={index} style={{backgroundColor: color}}/>
                         ))}
                    </div>
                    {checkNew && 
                    <div className='absolute bottom-3 left-0 right-0 text-[#b64400] text-[12px] text-center font-medium'>New</div>
                    }
               </div>
               
               <div className='product-price w-full text-center text-[16px] text-[#1d1d1f] font-normal leading-[1.43] mb-[33px]'>
                    From ${price}
               </div>

               <div className='line-horizontal w-[80%] border-b border-[#ccc]'>
               </div>

               <div className='product-screen w-full pb-11'>
                    <div className='text-center text-[20px] text-[#1d1d1f] font-semibold leading-[25px] tracking-[-0.374px] pt-[34px]'>{screen[0]}</div>
                    {screen.map((item:any,index:any) => {
                         if (index === 0 ) {
                              return ;
                         }
                         else {
                              return <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px]' key={index}>{item}</div>
                         }
                    })}
                    {Array.from({ length: Math.max(0, 4 - screen.length) }).map((_, i) => (
                         <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px]' key={i + screen.length}>-</div>
                    ))}
               </div>

               <div className='product-chip w-full pb-11'>
                    <Image className='w-[38px] h-[38px] mx-auto mb-[12px]' src={imgChip} alt='' width={1000} height={1000} quality={100}/>
                    {chip.map((item:any,index:any) => {
                         if(index === 0){
                              return ;
                         }
                         else {
                              return <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[36px]' key={index}>{item}</div>
                         }
                    })}
               </div>

               <div className='product-dynamic w-full pb-11 h-[170px]'>
                    {checkDynamic?
                    <>
                         <Image className='w-[38px] h-[38px] mx-auto mb-[12px]' src={'/images/compare-imgs/icon-dynamic.jpg'} alt='' width={1000} height={1000} quality={100}/>
                         <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[20px]'>Dynamic Island</div>
                         <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[20px]'>A magical way to interact with iPhone</div>
                    </>
                    :
                    <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[20px]'>-</div>
                    }
               </div>

               <div className='product-battery w-full pb-11'>
                    <Image className='w-[48px] h-[26px] mx-auto mb-[12px]' src={'/images/compare-imgs/icon-battery.jpg'} alt='' width={1000} height={1000} quality={100}/>
                    <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[36px]'>{battery}</div>
               </div>

               <div className='product-biometric-authen w-full pb-11'>
                    <Image className='w-[38px] h-[38px] mx-auto mb-[12px]' src={imgBiometricAuthen} alt='' width={1000} height={1000} quality={100}/>
                    <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[36px]'>{biometricAuthen}</div>
               </div>
               
               <div className='product-camera w-full pb-11'>
                    <Image className='w-[39px] h-[39px] mx-auto mb-[12px]' src={imgCamera} alt='' width={1000} height={1000} quality={100}/>
                    {camera.map((item:any,index:any) => {
                         if(index === 0){
                              return ;
                         }
                         else {
                              return <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[15px] mt-[6px] h-[30px]' key={index}>{item}</div>
                         }
                    })}
                    {Array.from({ length: Math.max(0, 8 - camera.length) }).map((_, i) => (
                    <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[30px] mt-[6px]' key={i + camera.length}>-</div>
                    ))}
               </div>

               <div className='product-crash-detection w-full pb-11 h-[250px]'>
                    <Image className='w-[39px] h-[39px] mx-auto mb-[12px]' src={'/images/compare-imgs/icon-crash-detection.jpg'} alt='' width={1000} height={1000} quality={100}/>
                    {crashDetection.map((item:any,index:any) => 
                    <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px]' key={index}>{item}</div>
                    )}
                    {Array.from({ length: Math.max(0, 5 - crashDetection.length) }).map((_, i) => (
                    <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px]' key={i + crashDetection.length}>-</div>
                    ))}
               </div>

               <div className='product-material w-full pb-11'>
                    <Image className='w-[52px] h-[41px] mx-auto mb-[12px]' src={'/images/compare-imgs/icon-material.jpg'} alt='' width={1000} height={1000} quality={100}/>
                    {material.map((item:any,index:any) => {
                         return <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[24px]' key={index}>{item}</div>
                    })}
                    {Array.from({ length: Math.max(0, 2 - material.length) }).map((_, i) => (
                         <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[24px]' key={i + screen.length}>-</div>
                    ))}
               </div>

               <div className='product-app-intell w-full pb-11 h-[124px]'>
                    {checkAppIntell?
                    <>
                         <Image className='w-[42px] h-[42px] mx-auto mb-[12px]' src={'/images/compare-imgs/icon-app-intell.jpg'} alt='' width={1000} height={1000} quality={100}/>
                         <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[36px]'>Apple Intelligence⁸</div>
                    </>
                    :
                    <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[20px]'>-</div>
                    }
               </div>

               <div className='product-camera-control w-full pb-11 h-[196px]'>
                    {checkCameraControl ? 
                         <>
                              <Image className='w-[38px] h-[36px] mx-auto mb-[12px]' src={'/images/compare-imgs/icon-camera-control.jpg'} alt='' width={1000} height={1000} quality={100}/>
                              <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px]'>Camera Control</div>
                              <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px]'>Easier way to capture</div>
                              <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[6px] mx-[20px]'>Faster access to photo and video tools</div>
                         </> 
                         :
                         <div className='text-center text-[15px] text-[#1d1d1f] font-light leading-[20px] mt-[20px]'>-</div>
                    }
               </div>

          </div>
     );
}

export default CompareItem;