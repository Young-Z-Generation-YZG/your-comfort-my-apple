import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type InitialState = {
   value: AppState;
};

type AppState = {
   router: RouterState;
};

type RouterState = {
   previousPath: string | null;
};

const initialState = {
   value: {
      router: {
         previousPath: null,
      },
   } as AppState,
} as InitialState;

const AppSlice = createSlice({
   name: 'app',
   initialState: initialState,
   reducers: {
      setRouter: (state, action: PayloadAction<RouterState>) => {
         state.value.router = action.payload;
      },
   },
});

export const { setRouter } = AppSlice.actions;

export default AppSlice.reducer;
