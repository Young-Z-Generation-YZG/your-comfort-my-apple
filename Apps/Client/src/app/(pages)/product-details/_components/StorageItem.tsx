/* eslint-disable react/react-in-jsx-scope */
'use client';
import { cn } from '~/infrastructure/lib/utils';

type StorageItemProps = {
   storageName: string;
   className?: string;
};

const StorageItem = ({ storageName, className }: StorageItemProps) => {
   return (
      <div className={cn('w-full h-fit rounded-[10px] p-[14px]')}>
         <div className="w-full flex flex-row items-center">
            <div className="basis-2/3">
               <span className="text-[17px] font-semibold leading-[21px] tracking-[0.3px]">
                  {storageName.toUpperCase()}
               </span>
            </div>
            <div className="basis-1/3 text-end text-[12px] font-light leading-[16px] tracking-[0.4px]">
               <span>From $999</span>
               <br />
               <span>or $41.62/mo</span>
               <br />
               <span>for 24 mo.</span>
            </div>
         </div>
      </div>
   );
};

export default StorageItem;
