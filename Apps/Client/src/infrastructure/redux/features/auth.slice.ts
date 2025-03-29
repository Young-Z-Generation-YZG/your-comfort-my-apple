import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { ILoginResponse } from '~/domain/interfaces/auth/login.interface';

type InitialState = {
   value: AuthState;
};

type AuthState = {
   userEmail: string | null;
   accessToken: string | null;
   refreshToken: string | null;
   AT_expireIn: string | null;
   isAuthenticated: boolean;
   timerId: string | null;
};

const initialState = {
   value: {
      userEmail: null,
      accessToken: null,
      refreshToken: null,
      AT_expireIn: null,
   } as AuthState,
} as InitialState;

const authSlice = createSlice({
   name: 'auth',
   initialState: initialState,
   reducers: {
      setAccessToken: (state, action: PayloadAction<ILoginResponse>) => {
         console.log('setAccessToken action:', action.payload);

         state.value.accessToken = action.payload.access_token;
         state.value.refreshToken = action.payload.refresh_token;
         state.value.AT_expireIn = action.payload.expiration;
         state.value.isAuthenticated = true;
      },

      setTimerId: (state, action: PayloadAction<string | null>) => {
         state.value.timerId = action.payload;
      },

      logout: (state) => {
         state.value.accessToken = null;
         state.value.refreshToken = null;
         state.value.AT_expireIn = null;
         state.value.userEmail = null;
         state.value.isAuthenticated = false;
      },
   },
});

export const { setAccessToken, logout, setTimerId } = authSlice.actions;

export default authSlice.reducer;
