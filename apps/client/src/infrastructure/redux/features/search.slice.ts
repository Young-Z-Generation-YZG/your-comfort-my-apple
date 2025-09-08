import { createSlice, PayloadAction } from '@reduxjs/toolkit';

export type SearchProduct = {
   name: string;
   image: string;
   unit_price: number;
   promotion_price: number;
   promotion_rate: number;
   slug: string;
};

export type SearchLink = {
   label: string;
   slug: string;
};

const searchSlice = createSlice({
   name: 'search',
   initialState: {
      value: {
         searchText: '',
         isSearching: false,
         searchProducts: [] as SearchProduct[],
         searchLinks: [] as SearchLink[],
      },
   },
   reducers: {
      setSearchText: (state, action: PayloadAction<string>) => {
         state.value.searchText = action.payload;
      },
      setIsSearching: (state, action: PayloadAction<boolean>) => {
         state.value.isSearching = action.payload;
      },
      setSearchProducts: (
         state,
         action: {
            payload: SearchProduct[];
         },
      ) => {
         state.value.searchProducts = action.payload;
      },
      setSearchLinks: (
         state,
         action: {
            payload: SearchLink[];
         },
      ) => {
         state.value.searchLinks = action.payload;
      },
   },
});

export const {
   setSearchText,
   setIsSearching,
   setSearchProducts,
   setSearchLinks,
} = searchSlice.actions;
export default searchSlice.reducer;
