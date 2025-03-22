import { createSlice, PayloadAction } from "@reduxjs/toolkit";

type ItemType = {
    name: string;
    quantity: number;
}

const initialState = {
    value: {
        items: [] as ItemType[],
    }
};


const cartSlice = createSlice({
    name: "cart",
    initialState: initialState,
    reducers: {
        addItem: (state, action: PayloadAction<ItemType>) => {
            state.value.items.push(action.payload);
        },
        deleteCart: (state) => {
            state.value.items = [];
        }
    }
})

export const { addItem, deleteCart } = cartSlice.actions;
export default cartSlice.reducer;