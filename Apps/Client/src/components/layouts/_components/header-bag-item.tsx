'use client';
import Image from 'next/image';
import React from 'react';
import { IBasketItemPayload } from '~/domain/interfaces/baskets/basket.interface';

interface HeaderBagItemProps {
   key?: string | number;
   item: IBasketItemPayload;
}

const HeaderBagItem = ({ key, item }: HeaderBagItemProps) => {
   return (
      <div className="flex gap-2 items-center" key={key}>
         <picture>
            <Image
               src={`${item.product_image}?wid=64&hei=64&fmt=png-alpha`}
               height={1000}
               width={1000}
               alt="test"
               className="w-[64px]"
            />
         </picture>

         <div>
            <p className="text-sm font-normal text-slate-800">
               {item.product_name}
               <span className="ml-2 text-xs text-slate-600">
                  x{item.quantity}
               </span>
            </p>
         </div>
      </div>
   );
};

export default HeaderBagItem;
