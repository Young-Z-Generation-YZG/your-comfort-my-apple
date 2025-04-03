import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IBasketItem } from '~/domain/interfaces/baskets/basket.interface';

const initialState = {
   value: {
      items: [] as IBasketItem[],
      currentOrder: 0,
   },
};

const cartSlice = createSlice({
   name: 'cart',
   initialState: initialState,
   reducers: {
      addCartItem: (state, action: PayloadAction<IBasketItem>) => {
         const existingItem = state.value.items.find(
            (item) => item.product_id === action.payload.product_id,
         );

         if (existingItem) {
            existingItem.quantity += action.payload.quantity;
         } else {
            state.value.items.push({
               ...action.payload,
               order: state.value.currentOrder,
            });

            state.value.currentOrder += 1;
         }
      },
      deleteCart: (state) => {
         state.value.items = [];
      },
   },
});

export const { addCartItem, deleteCart } = cartSlice.actions;
export default cartSlice.reducer;
