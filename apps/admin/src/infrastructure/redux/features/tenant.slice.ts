import { createSlice, PayloadAction } from '@reduxjs/toolkit';

type TenantAppState = {
   tenantId?: string | null;
   branchId?: string | null;
   tenantSubDomain?: string | null;
   tenantName?: string | null;
   tenantType?: string | null;
   tenantState?: string | null;
};

const initialState: TenantAppState = {
   tenantId: null,
   branchId: null,
   tenantSubDomain: null,
   tenantName: null,
   tenantType: null,
   tenantState: null,
};

const tenantSlice = createSlice({
   name: 'tenant',
   initialState: initialState,
   reducers: {
      setTenant: (state, action: PayloadAction<TenantAppState>) => {
         state.tenantId = action.payload.tenantId;
         state.branchId = action.payload.branchId;
         state.tenantSubDomain = action.payload.tenantSubDomain;
         state.tenantName = action.payload.tenantName;
         state.tenantType = action.payload.tenantType;
         state.tenantState = action.payload.tenantState;
      },
   },
});

export const { setTenant } = tenantSlice.actions;

export default tenantSlice.reducer;
