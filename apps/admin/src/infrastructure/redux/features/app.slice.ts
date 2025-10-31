import { createSlice, PayloadAction } from '@reduxjs/toolkit';

const AppSlice = createSlice({
   name: 'app',
   initialState: {
      route: {
         previousUnAuthenticatedPath: null as string | null,
      },
      isImpersonating: false,
      isLoading: false,
   },
   reducers: {
      setPreviousUnAuthenticatedPath: (
         state,
         action: PayloadAction<string | null>,
      ) => {
         state.route.previousUnAuthenticatedPath = action.payload;
      },
      setIsImpersonating: (state, action: PayloadAction<boolean>) => {
         state.isImpersonating = action.payload;
      },
      setIsLoading: (state, action: PayloadAction<boolean>) => {
         state.isLoading = action.payload;
      },
   },
});

export const {
   setPreviousUnAuthenticatedPath,
   setIsImpersonating,
   setIsLoading,
} = AppSlice.actions;

export default AppSlice.reducer;
