import { createSlice, PayloadAction, current } from '@reduxjs/toolkit';
import { TCart, TCartItem } from '~/infrastructure/services/basket.service';

export type CartState = TCart;

const initialState: CartState = {
   user_email: '',
   cart_items: [],
   sub_total_amount: 0,
   promotion_id: '',
   promotion_type: '',
   discount_type: '',
   discount_value: 0,
   discount_amount: 0,
   max_discount_amount: null,
   total_amount: 0,
};

const cartSlice = createSlice({
   name: 'cart',
   initialState,
   reducers: {
      SyncCart: (state, action: PayloadAction<TCart>) => {
         state.user_email = action.payload.user_email;
         state.cart_items = action.payload.cart_items;
         state.total_amount = action.payload.total_amount;

         console.log('CART APP STATE: SyncCart', current(state));
      },
      AddNewCartItem: (state, action: PayloadAction<TCartItem>) => {
         state.cart_items = [...state.cart_items, action.payload];
      },
      AddCartItems: (state, action: PayloadAction<TCartItem[]>) => {
         state.cart_items = [...action.payload];
      },
      UpdateQuantity: (state, action: PayloadAction<TCartItem>) => {
         state.cart_items = state.cart_items.map((item) =>
            item.sku_id === action.payload.sku_id &&
            item.model.normalized_name ===
               action.payload.model.normalized_name &&
            item.color.normalized_name ===
               action.payload.color.normalized_name &&
            item.storage.normalized_name ===
               action.payload.storage.normalized_name
               ? { ...item, quantity: action.payload.quantity }
               : item,
         );

         console.log('CART APP STATE: UpdateQuantity', current(state));
      },
      UpdateSelection: (state, action: PayloadAction<TCartItem>) => {
         state.cart_items = state.cart_items.map((item) =>
            item.sku_id === action.payload.sku_id &&
            item.model.normalized_name ===
               action.payload.model.normalized_name &&
            item.color.normalized_name ===
               action.payload.color.normalized_name &&
            item.storage.normalized_name ===
               action.payload.storage.normalized_name
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
