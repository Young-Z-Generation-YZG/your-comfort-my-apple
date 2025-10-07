import { createSlice, PayloadAction } from '@reduxjs/toolkit';

export type FiltersType = {
   colors: {
      name: string;
      hex: string;
   }[];
   models: {
      name: string;
      value: string;
   }[];
   storages: {
      name: string;
      value: string;
   }[];
};

const initialState = {
   value: {
      filters: {
         colors: [],
         models: [],
         storages: [],
      } as FiltersType,
   },
};

const ShopFilterSlice = createSlice({
   name: 'shopFilter',
   initialState: initialState,
   reducers: {
      setShopFilter: (state, action: PayloadAction<FiltersType>) => {
         state.value.filters = action.payload;
      },
      clearShopFilter: (state) => {
         state.value.filters = initialState.value.filters;
      },
   },
});

export const { setShopFilter, clearShopFilter } = ShopFilterSlice.actions;

export default ShopFilterSlice.reducer;
