import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IBasketItemPayload } from '~/domain/interfaces/baskets/basket.interface';

const initialState = {
   value: {
      items: [] as IBasketItemPayload[],
      currentOrder: 0,
   },
};

const cartSlice = createSlice({
   name: 'cart',
   initialState: initialState,
   reducers: {
      addRangeItems: (state, action: PayloadAction<IBasketItemPayload[]>) => {
         if (action.payload.length === 0) return;

         state.value.items = action.payload.map((item) => {
            state.value.currentOrder++;

            return {
               ...item,
               order_index: state.value.currentOrder,
            };
         });
      },
      addCartItem: (state, action: PayloadAction<IBasketItemPayload>) => {
         const existingItem = state.value.items.find(
            (item) => item.product_id === action.payload.product_id,
         );

         if (existingItem) {
            existingItem.quantity += action.payload.quantity;
            if (existingItem.promotion) {
               existingItem.promotion.promotion_final_price =
                  existingItem.promotion.promotion_discount_unit_price *
                  existingItem.quantity;
            }
         } else {
            state.value.items.push({
               ...action.payload,
               order: state.value.currentOrder,
            });

            state.value.currentOrder += 1;
         }
      },
      updateCartQuantity: (
         state,
         action: PayloadAction<{ product_id: string; quantity: number }>,
      ) => {
         const existingItem = state.value.items.find(
            (item) => item.product_id === action.payload.product_id,
         );

         if (existingItem) {
            existingItem.quantity = action.payload.quantity;
         }
      },
      removeCartItem: (state, action: PayloadAction<string>) => {
         state.value.currentOrder = state.value.items.length;

         state.value.items = state.value.items.filter(
            (item) => item.product_id !== action.payload,
         );
      },
      deleteCart: (state) => {
         state.value.items = [];
         state.value.currentOrder = 0;
      },
   },
});

export const {
   addRangeItems,
   addCartItem,
   updateCartQuantity,
   removeCartItem,
   deleteCart,
} = cartSlice.actions;
export default cartSlice.reducer;
