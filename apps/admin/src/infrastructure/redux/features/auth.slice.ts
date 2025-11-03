import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type AuthAppState = {
   currentUserKey?: 'currentUser' | 'impersonatedUser' | null;
   currentUser?: {
      userId?: string | null;
      userEmail?: string | null;
      username?: string | null;
      accessToken?: string | null;
      refreshToken?: string | null;
      accessTokenExpiredIn?: number | null;
      refreshTokenExpiredIn?: number | null;
   } | null;
   impersonatedUser?: {
      userId?: string | null;
      userEmail?: string | null;
      username?: string | null;
      accessToken?: string | null;
      refreshToken?: string | null;
      accessTokenExpiredIn?: number | null;
      refreshTokenExpiredIn?: number | null;
   } | null;
};

const authSlice = createSlice({
   name: 'auth',
   initialState: {
      currentUserKey: null,
      currentUser: {
         userId: null,
         userEmail: null,
         username: null,
         accessToken: null,
         refreshToken: null,
         accessTokenExpiredIn: null,
         refreshTokenExpiredIn: null,
      },
      impersonatedUser: {
         userId: null,
         userEmail: null,
         username: null,
         accessToken: null,
         refreshToken: null,
         accessTokenExpiredIn: null,
         refreshTokenExpiredIn: null,
      },
   } as AuthAppState,
   reducers: {
      setLogin: (
         state,
         action: PayloadAction<
            Omit<AuthAppState, 'currentUserKey' | 'impersonatedUser'>
         >,
      ) => {
         state.currentUserKey = 'currentUser';
         state.currentUser = action.payload.currentUser;
      },
      setImpersonatedUser: (
         state,
         action: PayloadAction<
            Omit<AuthAppState, 'currentUserKey' | 'currentUser'>
         >,
      ) => {
         state.currentUserKey = 'impersonatedUser';
         state.impersonatedUser = action.payload.impersonatedUser;

         if (!action.payload.impersonatedUser) {
            state.currentUserKey = 'currentUser';
         }

         console.log('APP STATE: setImpersonatedUser');
      },
      setLogout: (state) => {
         state.currentUser = null;
         state.impersonatedUser = null;
      },
   },
});

export const { setLogin, setImpersonatedUser, setLogout } = authSlice.actions;

export default authSlice.reducer;
