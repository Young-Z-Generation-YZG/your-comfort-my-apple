import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import envConfig from '~/infrastructure/config/env.config';

type AppState = {
   tenantId: string;
   route: {
      previousUnAuthenticatedPath: string | null;
   };
};

const initialState: AppState = {
   tenantId: envConfig.DEFAULT_TENANT_ID,
   route: {
      previousUnAuthenticatedPath: null,
   },
};

const AppSlice = createSlice({
   name: 'app',
   initialState,
   reducers: {
      setPreviousUnAuthenticatedPath: (
         state,
         action: PayloadAction<string | null>,
      ) => {
         state.route.previousUnAuthenticatedPath = action.payload;
      },
      setTenantId: (state, action: PayloadAction<string>) => {
         state.tenantId = action.payload;
      },
   },
});

export const { setPreviousUnAuthenticatedPath, setTenantId } = AppSlice.actions;

export default AppSlice.reducer;
