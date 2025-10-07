import { createSlice, PayloadAction } from '@reduxjs/toolkit';

const AppSlice = createSlice({
   name: 'app',
   initialState: {
      route: {
         previousUnAuthenticatedPath: null as string | null,
      },
   },
   reducers: {
      setPreviousUnAuthenticatedPath: (
         state,
         action: PayloadAction<string | null>,
      ) => {
         state.route.previousUnAuthenticatedPath = action.payload;
      },
   },
});

export const { setPreviousUnAuthenticatedPath } = AppSlice.actions;

export default AppSlice.reducer;
