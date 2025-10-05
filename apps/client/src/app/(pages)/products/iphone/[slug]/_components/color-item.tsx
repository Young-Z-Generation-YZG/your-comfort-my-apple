import { cn } from '~/infrastructure/lib/utils';

interface ColorItemProps {
   colorHex: string;
   colorName: string;
   isSelected?: boolean;
   isDisabled?: boolean;
   onClick?: () => void;
   onMouseEnter?: () => void;
   onMouseLeave?: () => void;
   className?: string;
}

const ColorItem = ({
   colorHex,
   colorName,
   isSelected = false,
   isDisabled = false,
   onClick,
   onMouseEnter,
   onMouseLeave,
   className = '',
}: ColorItemProps) => {
   return (
      <div
         className={cn(
            'h-[32px] w-[32px] rounded-full border-2 border-solid shadow-[inset_0_3px_4px_rgba(0,0,0,0.25)] transition-all duration-300 ease-in-out',
            isDisabled
               ? 'cursor-not-allowed opacity-50'
               : isSelected
                 ? 'cursor-pointer ring-2 ring-[#0071E3] ring-offset-2 ring-offset-white'
                 : 'cursor-pointer hover:ring-2 hover:ring-[#0071E3] hover:ring-offset-2 hover:ring-offset-white',
            className,
         )}
         style={{
            backgroundColor: colorHex,
         }}
         onClick={!isDisabled ? onClick : undefined}
         onMouseEnter={!isDisabled && !isSelected ? onMouseEnter : undefined}
         onMouseLeave={!isDisabled ? onMouseLeave : undefined}
         role="button"
         aria-label={`Select ${colorName} color`}
         tabIndex={isDisabled ? -1 : 0}
      />
   );
};

export default ColorItem;
