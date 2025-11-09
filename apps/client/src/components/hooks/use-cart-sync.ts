import { useAppSelector } from '~/infrastructure/redux/store';
import useBasketService from './api/use-basket-service';
import { useCallback, useEffect } from 'react';
import {
   TCartItem,
   TStoreBasketPayload,
} from '~/infrastructure/services/basket.service';
import { useDispatch } from 'react-redux';
import {
   AddCartItems,
   AddNewCartItem,
   UpdateQuantity,
} from '~/infrastructure/redux/features/cart.slice';

const useCartSync = () => {
   const dispatch = useDispatch();

   const isAuthenticated = useAppSelector(
      (state) => state.auth.isAuthenticated,
   );

   const cartAppState = useAppSelector((state) => state.cart);

   const { storeBasketAsync } = useBasketService();

   useEffect(() => {
      console.log('cartAppState in useCartSync', cartAppState);
      // if isAuthenticated == true
      // sync redux state to basket data
      if (isAuthenticated && cartAppState.cart_items.length > 0) {
         storeBasketAsync({
            cart_items: cartAppState.cart_items.map((item) => ({
               is_selected: item.is_selected,
               model_id: item.model_id,
               sku_id: item.sku_id,
               model: item.model,
               color: item.color,
               storage: item.storage,
               quantity: item.quantity,
            })),
         });
      }
   }, [isAuthenticated, cartAppState, storeBasketAsync]);

   const storeBasketSync = useCallback(
      async (cartItems: TCartItem[]) => {
         if (!cartItems || cartItems.length === 0) {
            return;
         }

         if (cartItems.length === 1) {
            const incomingItem = cartItems[0];

            const existingItem = cartAppState.cart_items.find(
               (item) =>
                  item.sku_id === incomingItem.sku_id &&
                  item.model.normalized_name ===
                     incomingItem.model.normalized_name &&
                  item.color.normalized_name ===
                     incomingItem.color.normalized_name &&
                  item.storage.normalized_name ===
                     incomingItem.storage.normalized_name,
            );

            console.log('existingItem', existingItem);

            if (existingItem) {
               dispatch(
                  UpdateQuantity({
                     ...existingItem,
                     quantity: existingItem.quantity + incomingItem.quantity,
                  }),
               );
            } else {
               dispatch(AddNewCartItem(incomingItem));
            }

            return;
         }

         const mergedItemsMap = new Map<string, TCartItem>();

         [...cartAppState.cart_items, ...cartItems].forEach((item) => {
            const key = `${item.model_id}-${item.color.normalized_name}-${item.storage.normalized_name}`;
            const existing = mergedItemsMap.get(key);

            if (existing) {
               mergedItemsMap.set(key, {
                  ...existing,
                  quantity: existing.quantity + item.quantity,
               });
            } else {
               mergedItemsMap.set(key, { ...item } as TCartItem);
            }
         });

         dispatch(AddCartItems(Array.from(mergedItemsMap.values())));
      },
      [cartAppState.cart_items, dispatch],
   );

   return {
      storeBasketSync,
   };
};

export default useCartSync;
