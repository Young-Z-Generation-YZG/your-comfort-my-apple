import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type AuthAppState = {
   userId: string | null;
   userEmail: string | null;
   username: string | null;
   accessToken: string | null;
   refreshToken: string | null;
   accessTokenExpiredIn: number | null;
   isAuthenticated: boolean;
};

const initialState: AuthAppState = {
   userId: null,
   userEmail: null,
   username: null,
   accessToken: null,
   refreshToken: null,
   accessTokenExpiredIn: null,
   isAuthenticated: false,
};

const authSlice = createSlice({
   name: 'auth',
   initialState: initialState,
   reducers: {
      setLogin: (
         state,
         action: PayloadAction<Omit<AuthAppState, 'isAuthenticated'>>,
      ) => {
         state.userId = action.payload.userId;
         state.userEmail = action.payload.userEmail;
         state.username = action.payload.username;
         state.accessToken = action.payload.accessToken;
         state.refreshToken = action.payload.refreshToken;
         state.accessTokenExpiredIn = action.payload.accessTokenExpiredIn;
         state.isAuthenticated = true;
      },

      setLogout: (state) => {
         state.userId = null;
         state.userEmail = null;
         state.username = null;
         state.accessToken = null;
         state.refreshToken = null;
         state.accessTokenExpiredIn = null;
         state.isAuthenticated = false;
      },
   },
});

export const { setLogin, setLogout } = authSlice.actions;

export default authSlice.reducer;
