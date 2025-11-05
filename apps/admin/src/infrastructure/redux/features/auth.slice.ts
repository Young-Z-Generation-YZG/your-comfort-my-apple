import { createSlice, PayloadAction, current } from '@reduxjs/toolkit';

type AuthAppState = {
   currentUserKey?: 'currentUser' | 'impersonatedUser' | null;
   currentUser?: {
      userId?: string | null;
      userEmail?: string | null;
      username?: string | null;
      accessToken?: string | null;
      refreshToken?: string | null;
      roles?: string[] | null;
      accessTokenExpiredIn?: number | null;
      refreshTokenExpiredIn?: number | null;
   } | null;
   impersonatedUser?: {
      userId?: string | null;
      userEmail?: string | null;
      username?: string | null;
      accessToken?: string | null;
      refreshToken?: string | null;
      roles?: string[] | null;
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
         roles: null,
         accessTokenExpiredIn: null,
         refreshTokenExpiredIn: null,
      },
      impersonatedUser: {
         userId: null,
         userEmail: null,
         username: null,
         accessToken: null,
         refreshToken: null,
         roles: null,
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

         console.log('AUTH APP STATE: setLogin', current(state));
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

         console.log('AUTH APP STATE: setImpersonatedUser', current(state));
      },
      setRoles: (
         state,
         action: PayloadAction<{
            currentUser: { roles: string[] | null };
            impersonatedUser: { roles: string[] | null };
         }>,
      ) => {
         if (state.currentUser && action.payload.currentUser.roles) {
            state.currentUser.roles = action.payload.currentUser.roles;
         }

         if (state.impersonatedUser && action.payload.impersonatedUser.roles) {
            state.impersonatedUser.roles =
               action.payload.impersonatedUser.roles;
         }

         console.log('AUTH APP STATE: setRoles', current(state));
      },
      setLogout: (state) => {
         state.currentUser = null;
         state.impersonatedUser = null;

         console.log('AUTH APP STATE: setLogout', current(state));
      },
   },
});

export const { setLogin, setImpersonatedUser, setLogout, setRoles } =
   authSlice.actions;

export default authSlice.reducer;
