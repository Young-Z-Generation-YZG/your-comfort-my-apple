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

const AppSlice = createSlice({
   name: 'app',
   initialState: initialState,
   reducers: {
      setAppFilters: (state, action: PayloadAction<FiltersType>) => {
         state.value.filters = action.payload;
      },
      clearAppFilters: (state) => {
         state.value.filters = initialState.value.filters;
      },
   },
});

export const { setAppFilters, clearAppFilters } = AppSlice.actions;

export default AppSlice.reducer;
