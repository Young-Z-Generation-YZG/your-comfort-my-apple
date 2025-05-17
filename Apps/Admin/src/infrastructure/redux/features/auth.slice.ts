import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type InitialState = {
   value: AuthState;
};

type AuthState = {
   userEmail: string | null;
   accessToken: string | null;
   refreshToken: string | null;
};

const initialState = {
   value: {
      userEmail: null,
      accessToken: null,
      refreshToken: null,
      accessTokenExpiredIn: null,
   } as AuthState,
} as InitialState;

const authSlice = createSlice({
   name: 'auth',
   initialState: initialState,
   reducers: {
      setLogin: (
         state,
         action: PayloadAction<{
            user_email: string;
            access_token: string | null;
            refresh_token: string | null;
         }>,
      ) => {
         state.value.userEmail = action.payload.user_email;
         state.value.accessToken = action.payload.access_token;
         state.value.refreshToken = action.payload.refresh_token;
      },
      setLogout: (state) => {
         state.value.accessToken = null;
         state.value.refreshToken = null;
         state.value.userEmail = null;
      },
   },
});

export const { setLogin, setLogout } = authSlice.actions;

export default authSlice.reducer;
