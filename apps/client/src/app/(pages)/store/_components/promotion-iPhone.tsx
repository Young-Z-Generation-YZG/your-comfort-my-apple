import { cn } from '~/infrastructure/lib/utils';
import CardWrapper from './card-wrapper';
import NextImage from 'next/image';
import { Button } from '@components/ui/button';

interface PromotionIPhoneProps {
   className?: string;
}

const resizeFromWidth = (width: number, aspectRatio: string = '16:9') => {
   const [widthRatio, heightRatio] = aspectRatio.split(':').map(Number);
   return `w_${width},h_${Math.round((width * heightRatio) / widthRatio)}`;
};

const resizeFromHeight = (height: number, aspectRatio: string = '16:9') => {
   const [widthRatio, heightRatio] = aspectRatio.split(':').map(Number);
   return `w_${Math.round((height * widthRatio) / heightRatio)},h_${height}`;
};

const PromotionIPhone = ({ className }: PromotionIPhoneProps) => {
   return (
      <div className={cn('', className)}>
         <CardWrapper className="w-full">
            <div className="w-full overflow-hidden relative h-[300px]">
               <NextImage
                  src={`https://res.cloudinary.com/delkyrtji/image/upload/${resizeFromHeight(1000, '16:9')}/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp`}
                  alt="promotion-iPhone"
                  width={Math.round((1000 * 16) / 9)}
                  height={1000}
                  // width={500}
                  // height={Math.round((500 * 9) / 16)}
                  className="absolute top-0 left-0 w-full h-full object-cover"
               />
            </div>

            {/* Color */}
            <div className="flex justify-center mt-5">
               <span
                  className={cn(
                     'h-[40px] w-[40px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector ring-offset-white',
                  )}
                  style={{ backgroundColor: '#D5DDDF' }}
               ></span>
            </div>

            <div className="p-6">
               <h3 className="mb-4 text-2xl font-semibold">
                  iPhone 15 White 256GB
               </h3>

               <div className="flex items-end gap-2">
                  {/* discount price */}
                  <span className="text-2xl font-medium text-red-600 font-SFProText">
                     $800
                  </span>
                  {/* original price */}
                  <span className="text-lg text-gray-500 line-through font-SFProText">
                     $1000
                  </span>
               </div>

               <p className="mt-1 text-sm text-gray-500 font-SFProText">
                  Save <strong>20%</strong> for a limited time
               </p>

               <Button
                  className="w-full mt-5 cursor-pointer rounded-lg bg-blue-600 py-2 text-white font-SFProText font-medium hover:bg-blue-700"
                  onClick={() => {}}
               >
                  Buy now
               </Button>
            </div>
         </CardWrapper>
      </div>
   );
};

export default PromotionIPhone;
