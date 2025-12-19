import Image, { StaticImageData } from 'next/image';

interface ServiceCardProps {
   icon: StaticImageData | string;
   title: string;
   description: string;
}

const ServiceCard = ({ icon, title, description }: ServiceCardProps) => {
   return (
      <div className="basis-[25%] flex flex-col justify-center items-center">
         <Image
            src={icon}
            className="h-auto w-[46px] mx-auto"
            width={2000}
            height={2000}
            quality={100}
            alt={title}
         />
         <div className="w-full pt-4 pb-2 text-center text-[21px] font-semibold leading-[25px] tracking-[0.3px]">
            {title}
         </div>
         <div className="w-full px-2 text-center text-[14px] font-light leading-[18px] tracking-[0.3px]">
            {description}
         </div>
      </div>
   );
};

export default ServiceCard;
