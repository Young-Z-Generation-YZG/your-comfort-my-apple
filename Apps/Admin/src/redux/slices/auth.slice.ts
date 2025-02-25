import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type InitialState = {
   value: AuthState;
};

type AuthState = {
   isLoggingIn: boolean;
   isFailed: boolean;
   isAuth: boolean;
   token: string | null;
};

const initialState = {
   value: {
      isLoggingIn: false,
      isFailed: false,
      isAuth: false,
      token: null,
   } as AuthState,
} as InitialState;

const authSlice = createSlice({
   name: 'auth',
   initialState: initialState,
   reducers: {
      setIsLoggingIn: (
         state = initialState,
         action: PayloadAction<boolean>,
      ) => {
         state.value.isLoggingIn = action.payload;
      },
      setAccessToken: (state, action: PayloadAction<string>) => {
         state.value.token = action.payload;
      },
      loginSuccess: (state, action: PayloadAction<string>) => {
         console.log('[DISPATCH] Login success', action.payload);

         state.value.isAuth = true;
         state.value.isLoggingIn = false;
         state.value.token = action.payload;
      },
      loginFailed: (state) => {
         state.value.isFailed = true;
         state.value.isLoggingIn = false;
      },
      logout: (state) => {
         state.value.token = null;
      },
   },
});

export const {
   setAccessToken,
   logout,
   setIsLoggingIn,
   loginFailed,
   loginSuccess,
} = authSlice.actions;

export default authSlice.reducer;
