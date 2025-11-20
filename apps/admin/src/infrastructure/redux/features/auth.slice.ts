import { createSlice, PayloadAction, current } from '@reduxjs/toolkit';

type AuthAppState = {
   currentUserKey?: 'currentUser' | 'impersonatedUser' | null;
   useRefreshToken?: boolean;
   currentUser?: {
      userId?: string | null;
      tenantId?: string | null;
      branchId?: string | null;
      tenantSubDomain?: string | null;
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
      tenantId?: string | null;
      branchId?: string | null;
      tenantSubDomain?: string | null;
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
      useRefreshToken: false,
      currentUser: {
         userId: null,
         tenantId: null,
         branchId: null,
         tenantSubDomain: null,
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
         tenantId: null,
         branchId: null,
         tenantSubDomain: null,
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
         state.useRefreshToken = false;

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
            currentUser: { roles: string[] | null } | null;
            impersonatedUser: { roles: string[] | null } | null;
         }>,
      ) => {
         if (action.payload.currentUser && state.currentUser) {
            state.currentUser.roles = action.payload.currentUser.roles;
         }
         if (action.payload.impersonatedUser && state.impersonatedUser) {
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
      setUseRefreshToken: (state, action: PayloadAction<boolean>) => {
         state.useRefreshToken = action.payload;
      },
      setIdentity: (
         state,
         action: PayloadAction<{
            currentUser: {
               userId: string | null;
               tenantId: string | null;
               branchId: string | null;
               tenantSubDomain: string | null;
            } | null;
            impersonatedUser: {
               userId: string | null;
               tenantId: string | null;
               branchId: string | null;
               tenantSubDomain: string | null;
            } | null;
         }>,
      ) => {
         if (state.currentUser && action.payload.currentUser) {
            state.currentUser.userId = action.payload.currentUser.userId;
            state.currentUser.tenantId = action.payload.currentUser.tenantId;
            state.currentUser.branchId = action.payload.currentUser.branchId;
            state.currentUser.tenantSubDomain =
               action.payload.currentUser.tenantSubDomain;
         }

         if (state.impersonatedUser && action.payload.impersonatedUser) {
            state.impersonatedUser.userId =
               action.payload.impersonatedUser.userId;
            state.impersonatedUser.tenantId =
               action.payload.impersonatedUser.tenantId;
            state.impersonatedUser.branchId =
               action.payload.impersonatedUser.branchId;
            state.impersonatedUser.tenantSubDomain =
               action.payload.impersonatedUser.tenantSubDomain;
         }

         console.log('AUTH APP STATE: setIdentity', current(state));
      },
      setCurrentUserKey: (
         state,
         action: PayloadAction<'currentUser' | 'impersonatedUser'>,
      ) => {
         state.currentUserKey = action.payload;
      },
   },
});

export const {
   setLogin,
   setImpersonatedUser,
   setLogout,
   setRoles,
   setUseRefreshToken,
   setIdentity,
   setCurrentUserKey,
} = authSlice.actions;

export default authSlice.reducer;
