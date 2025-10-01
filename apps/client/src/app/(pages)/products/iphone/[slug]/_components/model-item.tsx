import { cn } from '~/infrastructure/lib/utils';

interface ModelItemProps {
   modelName: string;
   displaySize: string;
   price: number;
   monthlyPrice: string;
   isSelected?: boolean;
   onClick?: () => void;
   className?: string;
}

const ModelItem = ({
   modelName,
   displaySize,
   price,
   monthlyPrice,
   isSelected = false,
   onClick,
   className = '',
}: ModelItemProps) => {
   return (
      <div
         className={cn(
            'w-full flex flex-row items-center justify-between p-[14px] cursor-pointer rounded-[12px] border-2 bg-white transition-all duration-200',
            isSelected
               ? 'border-[#0071E3]'
               : 'border-[#D2D2D7] hover:border-[#0071E3]',
            className,
         )}
         onClick={onClick}
      >
         <div className="flex flex-col gap-[2px]">
            <span className="text-[17px] font-semibold leading-[21px] tracking-[-0.02em] text-[#1D1D1F]">
               {modelName}
            </span>
            <span className="text-[12px] leading-[16px] text-[#6E6E73]">
               {displaySize}-inch display
               <sup className="text-[8px]">2</sup>
            </span>
         </div>
         <div className="text-right">
            <div className="text-[12px] leading-[16px] text-[#1D1D1F]">
               From ${price}
            </div>
            <div className="text-[12px] leading-[16px] text-[#6E6E73]">
               or ${monthlyPrice}/mo.
            </div>
            <div className="text-[12px] leading-[16px] text-[#6E6E73]">
               for 24 mo.
               <sup className="text-[8px]">2</sup>
            </div>
         </div>
      </div>
   );
};

export default ModelItem;
