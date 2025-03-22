/* eslint-disable react/react-in-jsx-scope */
'use client';
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
interface ColorItemProps {
     isSelected: boolean;
     onClick: () => void;
     color: string;
}

const ColorItem = ({isSelected, onClick, color}: ColorItemProps) => {
     return(
          <div 
               className={cn("w-[28px] h-[28px] rounded-full border shadow-[rgba(50,50,93,0.25)_0px_2px_5px_-1px,_rgba(0,0,0,0.3)_0px_1px_3px_-1px]", isSelected ? 'border-2 border-[#000]':'')}
               style={{backgroundColor: color}}
               onClick={onClick}
          />               
     )
}

export default ColorItem;

{/* <div className={cn('w-[32px] h-[32px] rounded-full flex items-center justify-center',isSelected ? 'border-2 border-[#0071E3]':'')}>
     <div 
          className={cn("w-[28px] h-[28px] rounded-full shadow-[rgba(50,50,93,0.25)_0px_2px_5px_-1px,_rgba(0,0,0,0.3)_0px_1px_3px_-1px]")}
          style={{backgroundColor: color}}
          onClick={onClick}
     />               
</div> */}