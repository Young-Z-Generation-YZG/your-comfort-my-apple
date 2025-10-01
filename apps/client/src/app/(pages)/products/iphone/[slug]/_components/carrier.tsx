import images from '@components/client/images';
import Image from 'next/image';

const Carrier = () => {
   return (
      <div className="w-full pt-[14px] relative bg-transparent">
         <div className="w-[980px] py-[14px] h-[66px] rounded-[5px] flex flex-row justify-center items-center text-[12px] font-normal bg-[#f5f5f7] mx-auto">
            <div className="h-full pl-[20px] w-[176px] flex flex-col">
               <div className="leading-[18px] tracking-[0.4px] text-[15px] font-medium">
                  Carrier Deals at Apple
               </div>
               <div className="leading-[20px] tracking-[0.4px] text-[15px] font-light text-blue-500">
                  <a href="#" target="_blank">
                     See all deals
                  </a>
               </div>
            </div>
            <div className="h-full pl-[20px] w-[190px] flex flex-row items-center">
               <div className="basis-1/6 w-[24px]">
                  <Image
                     src={images.icAtt}
                     className="h-[24px] w-auto mx-auto"
                     alt="apple-logo"
                     width={1000}
                     height={1000}
                     quality={100}
                  />
               </div>
               <div className="basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light">
                  Save up to $1000 after trade-in.
               </div>
            </div>
            <div className="h-full pl-[20px] w-[190px] flex flex-row items-center">
               <div className="basis-1/6 w-[24px]">
                  <Image
                     src={images.icLightYear}
                     className="h-[24px] w-auto mx-auto"
                     alt="apple-logo"
                     width={1000}
                     height={1000}
                     quality={100}
                  />
               </div>
               <div className="basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light">
                  Save up to $1000. No trade-in needed.
               </div>
            </div>
            <div className="h-full pl-[20px] w-[190px] flex flex-row items-center">
               <div className="basis-1/6 w-[24px]">
                  <Image
                     src={images.icTmobile}
                     className="h-[24px] w-auto mx-auto"
                     alt="apple-logo"
                     width={1000}
                     height={1000}
                     quality={100}
                  />
               </div>
               <div className="basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light">
                  Save up to $1000 after trade-in.
               </div>
            </div>
            <div className="h-full pl-[20px] w-[190px] flex flex-row items-center">
               <div className="basis-1/6 w-[24px]">
                  <Image
                     src={images.icVerizon}
                     className="h-[24px] w-auto mx-auto"
                     alt="apple-logo"
                     width={1000}
                     height={1000}
                     quality={100}
                  />
               </div>
               <div className="basis-5/6 leading-[1.3333733333] tracking-[0.5px] ml-3 font-light">
                  Save up to $1000 after trade-in.
               </div>
            </div>
         </div>
      </div>
   );
};

export default Carrier;
