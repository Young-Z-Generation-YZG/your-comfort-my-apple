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
      setTenant: (state, action: PayloadAction<Partial<TenantAppState>>) => {
         // Partial update: only update provided fields
         if (action.payload.tenantId !== undefined) {
            state.tenantId = action.payload.tenantId;
         }
         if (action.payload.branchId !== undefined) {
            state.branchId = action.payload.branchId;
         }
         if (action.payload.tenantSubDomain !== undefined) {
            state.tenantSubDomain = action.payload.tenantSubDomain;
         }

         console.log(
            'TENANT APP STATE: setTenant',
            JSON.stringify(current(state), null, 2),
         );
      },
      clearTenant: (state) => {
         // Explicitly reset all fields to null
         state.tenantId = null;
         state.branchId = null;
         state.tenantSubDomain = null;

         // Force Immer to recognize the changes
         const clearedState = current(state);
         console.log(
            'TENANT APP STATE: clearTenant',
            JSON.stringify(clearedState, null, 2),
         );

         // Verify all fields are null
         if (
            clearedState.tenantId !== null ||
            clearedState.branchId !== null ||
            clearedState.tenantSubDomain !== null
         ) {
            console.error(
               'TENANT APP STATE: clearTenant failed to clear all fields!',
               clearedState,
            );
         }
      },
   },
});

export const { setTenant, clearTenant } = tenantSlice.actions;

export default tenantSlice.reducer;
