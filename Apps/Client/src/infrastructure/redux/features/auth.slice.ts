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
   isLogoutTriggered: boolean;
   timerId: string | null;
};

const initialState = {
   value: {
      userEmail: null,
      accessToken: null,
      refreshToken: null,
      AT_expireIn: null,
      isAuthenticated: false,
      isLogoutTriggered: false,
      timerId: null,
   } as AuthState,
} as InitialState;

const authSlice = createSlice({
   name: 'auth',
   initialState: initialState,
   reducers: {
      setAccessToken: (state, action: PayloadAction<ILoginResponse>) => {
         state.value.userEmail = action.payload.user_email;
         state.value.accessToken = action.payload.access_token;
         state.value.refreshToken = action.payload.refresh_token;
         state.value.AT_expireIn = action.payload.expiration;
         state.value.isAuthenticated = true;
         state.value.isLogoutTriggered = false;
      },

      setTimerId: (state, action: PayloadAction<string | null>) => {
         state.value.timerId = action.payload;
      },

      setLogout: (state) => {
         state.value.accessToken = null;
         state.value.refreshToken = null;
         state.value.AT_expireIn = null;
         state.value.userEmail = null;
         state.value.isLogoutTriggered = true;
         state.value.isAuthenticated = false;
         state.value.timerId = null;
         state.value.isLogoutTriggered = true;
      },
   },
});

export const { setAccessToken, setLogout, setTimerId } = authSlice.actions;

export default authSlice.reducer;
