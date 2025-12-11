import { createSlice, PayloadAction, current } from '@reduxjs/toolkit';
import { TCartItem, TCart } from '~/domain/types/basket.type';

export type TReduxCartState = TCart;

const initialState: TReduxCartState = {
   user_email: '',
   cart_items: [],
   sub_total_amount: 0,
   promotion_id: null,
   promotion_type: null,
   discount_type: null,
   discount_value: null,
   discount_amount: null,
   max_discount_amount: null,
   total_amount: 0,
};

const cartSlice = createSlice({
   name: 'cart',
   initialState,
   reducers: {
      SyncCart: (state, action: PayloadAction<TReduxCartState>) => {
         state.user_email = action.payload.user_email;
         state.cart_items = action.payload.cart_items;
         state.total_amount = action.payload.total_amount;

         console.log('CART APP STATE: SyncCart', current(state));
      },
      AddNewCartItem: (state, action: PayloadAction<TCartItem>) => {
         // Force newly added items to be selected by default
         state.cart_items = [
            ...state.cart_items,
            { ...action.payload, is_selected: false },
         ];
      },
      AddCartItems: (state, action: PayloadAction<TCartItem[]>) => {
         // Ensure items have is_selected flag; default to true when unspecified
         state.cart_items = action.payload.map((item) => ({
            ...item,
            is_selected: item.is_selected,
         }));
      },
      UpdateQuantity: (state, action: PayloadAction<TCartItem>) => {
         state.cart_items = state.cart_items.map((item) =>
            item.sku_id === action.payload.sku_id
               ? { ...item, quantity: action.payload.quantity }
               : item,
         );

         console.log('CART APP STATE: UpdateQuantity', current(state));
      },
      UpdateSelection: (state, action: PayloadAction<TCartItem>) => {
         state.cart_items = state.cart_items.map((item) =>
            item.sku_id === action.payload.sku_id
               ? { ...item, is_selected: action.payload.is_selected }
               : item,
         );
      },
      CleanSelectedItems: (state) => {
         state.cart_items = state.cart_items.filter(
            (item) => !item.is_selected,
         );
      },
      removeCartItem: (state, action: PayloadAction<number>) => {
         const index = action.payload;
         if (index >= 0 && index < state.cart_items.length) {
            state.cart_items = state.cart_items.filter((_, i) => i !== index);
         }
      },
      clearCart: (state) => {
         state.cart_items = [];
         state.total_amount = 0;
      },
   },
});

export const {
   SyncCart,
   UpdateQuantity,
   AddNewCartItem,
   AddCartItems,
   UpdateSelection,
   CleanSelectedItems,
   removeCartItem,
   clearCart,
} = cartSlice.actions;

export default cartSlice.reducer;
