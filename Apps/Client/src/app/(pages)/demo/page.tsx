'use client';

import { Fragment } from 'react';
import { useDispatch } from 'react-redux';
import { Button } from '~/components/ui/button';
import {
   addItem,
   deleteCart,
} from '~/infrastructure/redux/features/cart-demo.slice';
import { AppDispatch } from '~/infrastructure/redux/store';
import CartDemo from './cart/page';

const DemoPage = () => {
   const dispatch = useDispatch<AppDispatch>();

   return (
      <Fragment>
         <div className="p-5">
            <Button
               variant="outline"
               className="bg-blue-500 text-white cursor-pointer"
               onClick={() => {
                  dispatch(addItem({ name: 'Item 1', quantity: 1 }));
               }}
            >
               Add to cart
            </Button>

            <Button
               className="bg-red-500 text-white cursor-pointer"
               onClick={() => {
                  dispatch(deleteCart());
               }}
            >
               Delete cart
            </Button>
         </div>

         <div>
            <CartDemo />
         </div>
      </Fragment>
   );
};

export default DemoPage;
