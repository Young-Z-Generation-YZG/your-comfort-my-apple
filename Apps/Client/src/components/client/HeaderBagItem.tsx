'use client';
import Image from 'next/image';
import React from 'react';
import { IBasketItem } from '~/domain/interfaces/baskets/basket.interface';

interface HeaderBagItemProps {
   key?: string | number;
   item: IBasketItem;
}

const HeaderBagItem = ({ key, item }: HeaderBagItemProps) => {
   console.log('item', item);

   var image =
      item?.product_image ||
      'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/MDG04';

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
