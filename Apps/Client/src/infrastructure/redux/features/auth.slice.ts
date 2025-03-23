import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type InitialState = {
   value: AuthState;
};

type AuthState = {
   token: string | null;
   userEmail: string | null;
};

const initialState = {
   value: {
      token: null,
      userEmail: null,
   } as AuthState,
} as InitialState;

const authSlice = createSlice({
   name: 'auth',
   initialState: initialState,
   reducers: {
      setAccessToken: (state, action: PayloadAction<string>) => {
         state.value.token = action.payload;
      },

      logout: (state) => {
         state.value.token = null;
      },
   },
});

export const {
   setAccessToken,
   logout,
} = authSlice.actions;

export default authSlice.reducer;
