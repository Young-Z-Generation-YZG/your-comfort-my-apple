import { createSlice, current, PayloadAction } from '@reduxjs/toolkit';

type TenantAppState = {
   tenantId?: string | null;
   branchId?: string | null;
   tenantSubDomain?: string | null;
};

const initialState: TenantAppState = {
   tenantId: null,
   branchId: null,
   tenantSubDomain: null,
};

const tenantSlice = createSlice({
   name: 'tenant',
   initialState: initialState,
   reducers: {
      setTenant: (state, action: PayloadAction<TenantAppState>) => {
         state.tenantId = action.payload.tenantId;
         state.branchId = action.payload.branchId;
         state.tenantSubDomain = action.payload.tenantSubDomain;

         console.log('TENANT APP STATE: setTenant', current(state));
      },
   },
});

export const { setTenant } = tenantSlice.actions;

export default tenantSlice.reducer;
