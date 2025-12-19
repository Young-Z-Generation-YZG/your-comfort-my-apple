import { cn } from '~/infrastructure/lib/utils';

interface StorageItemProps {
   storageName: string;
   price: number;
   monthlyPrice: string;
   note?: string;
   isSelected?: boolean;
   isDisabled?: boolean;
   onClick?: () => void;
   className?: string;
}

const StorageItem = ({
   storageName,
   price,
   monthlyPrice,
   note,
   isSelected = false,
   isDisabled = false,
   onClick,
   className = '',
}: StorageItemProps) => {
   return (
      <div
         className={cn(
            'w-full flex flex-row items-start justify-between p-[14px] rounded-[12px] border-2 bg-white transition-all duration-200',
            isDisabled
               ? 'cursor-not-allowed opacity-50 border-[#D2D2D7]'
               : isSelected
                 ? 'cursor-pointer border-[#0071E3]'
                 : 'cursor-pointer border-[#D2D2D7] hover:border-[#0071E3]',
            className,
         )}
         onClick={!isDisabled ? onClick : undefined}
      >
         <div className="flex flex-col gap-[2px]">
            <span className="text-[17px] font-semibold leading-[21px] tracking-[-0.02em] text-[#1D1D1F]">
               {storageName}
               <sup className="text-[8px]">1</sup>
            </span>
            {note && (
               <span className="text-[12px] leading-[16px] text-[#6E6E73]">
                  {note}
               </span>
            )}
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

export default StorageItem;
