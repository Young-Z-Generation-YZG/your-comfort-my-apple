import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type InitialState = {
   value: AuthState;
};

type AuthState = {
   userEmail: string | null;
   username: string | null;
   accessToken: string | null;
   refreshToken: string | null;
   accessTokenExpiredIn: number | null;
   isAuthenticated: boolean;
   isLogoutTriggered: boolean;
   timerId: string | null;
};

const initialState = {
   value: {
      userEmail: null,
      username: null,
      accessToken: null,
      refreshToken: null,
      accessTokenExpiredIn: null,
      isAuthenticated: false,
      isLogoutTriggered: false,
      timerId: null,
   } as AuthState,
} as InitialState;

const authSlice = createSlice({
   name: 'auth',
   initialState: initialState,
   reducers: {
      setAccessToken: (
         state,
         action: PayloadAction<{
            user_email: string;
            username: string;
            access_token: string | null;
            refresh_token: string | null;
            access_token_expires_in: number | null;
         }>,
      ) => {
         state.value.userEmail = action.payload.user_email;
         state.value.username = action.payload.username;
         state.value.accessToken = action.payload.access_token;
         state.value.refreshToken = action.payload.refresh_token;
         state.value.accessTokenExpiredIn =
            action.payload.access_token_expires_in;
         state.value.isAuthenticated = true;
         state.value.isLogoutTriggered = false;
      },

      setTimerId: (state, action: PayloadAction<string | null>) => {
         state.value.timerId = action.payload;
      },

      setLogout: (state) => {
         state.value.accessToken = null;
         state.value.refreshToken = null;
         state.value.accessTokenExpiredIn = null;
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
