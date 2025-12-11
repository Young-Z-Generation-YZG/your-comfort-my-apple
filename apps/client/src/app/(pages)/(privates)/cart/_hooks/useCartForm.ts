import { useCallback, useEffect, useState, useRef } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { TCartItem, IStoreBasketPayload } from '~/domain/types/basket.type';
import { TReduxCartState } from '~/infrastructure/redux/features/cart.slice';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useDispatch } from 'react-redux';
import { AddCartItems } from '~/infrastructure/redux/features/cart.slice';

const cartItemSchema = z.object({
   is_selected: z.boolean(),
   model_id: z.string().min(1, { message: 'Model ID is required' }),
   sku_id: z.string().min(1, { message: 'SKU ID is required' }),
   color: z.object({
      name: z.string().min(1, { message: 'Color name is required' }),
      normalized_name: z
         .string()
         .min(1, { message: 'Normalized name is required' }),
   }),
   model: z.object({
      name: z.string().min(1, { message: 'Model name is required' }),
      normalized_name: z
         .string()
         .min(1, { message: 'Normalized name is required' }),
   }),
   storage: z.object({
      name: z.string().min(1, { message: 'Storage name is required' }),
      normalized_name: z
         .string()
         .min(1, { message: 'Normalized name is required' }),
   }),
   quantity: z
      .number()
      .min(1, { message: 'Quantity must be at least 1' })
      .max(99, { message: 'Quantity cannot exceed 99' }),
});

export const cartFormSchema = z.object({
   cart_items: z.array(cartItemSchema),
});

export type CartFormData = z.infer<typeof cartFormSchema>;

interface UseCartFormProps {
   basketData: TReduxCartState;
   storeBasketAsync?: (payload: IStoreBasketPayload) => Promise<{
      isSuccess: boolean;
      isError: boolean;
      data: boolean | null;
      error: unknown;
   }>;
   deleteBasketAsync?: () => Promise<{
      isSuccess: boolean;
      isError: boolean;
      data: boolean | null;
      error: unknown;
   }>;
}

const useCartForm = ({
   basketData,
   storeBasketAsync,
   deleteBasketAsync,
}: UseCartFormProps) => {
   const [cartItems, setCartItems] = useState<TCartItem[]>([]);

   const cartItemState = useAppSelector((state) => state.cart.cart_items);

   const dispatch = useDispatch();

   // Track previous cart items to detect selection changes
   const prevCartItemsRef = useRef<typeof cartItems>([]);

   const form = useForm<CartFormData>({
      resolver: zodResolver(cartFormSchema),
      defaultValues: {
         cart_items: basketData.cart_items.map((item) => ({
            is_selected: item.is_selected,
            model_id: item.model_id,
            sku_id: item.sku_id,
            color: item.color,
            model: item.model,
            storage: item.storage,
            quantity: item.quantity,
         })),
      },
   });

   // Watch cart_items for changes
   const watchedCartItems = useWatch({
      control: form.control,
      name: 'cart_items',
   });

   const handleQuantityChange = useCallback(
      (index: number, newQuantity: number) => {
         const currentCartItems = form.getValues('cart_items');
         if (currentCartItems && currentCartItems[index]) {
            const updatedItems = [...currentCartItems];
            updatedItems[index] = {
               ...updatedItems[index],
               quantity: newQuantity,
            };

            form.setValue('cart_items', updatedItems);
            setCartItems(updatedItems as TCartItem[]);

            const updatedCartItems = cartItemState.map((item) =>
               item.model_id === updatedItems[index].model_id &&
               item.color.normalized_name ===
                  updatedItems[index].color.normalized_name &&
               item.storage.normalized_name ===
                  updatedItems[index].storage.normalized_name
                  ? { ...item, quantity: newQuantity }
                  : item,
            );

            dispatch(AddCartItems(updatedCartItems as TCartItem[]));
         }
      },
      [cartItemState, dispatch, form],
   );

   const handleRemoveItem = useCallback(
      (index: number) => {
         const currentItems = form.getValues('cart_items');

         if (!currentItems || !currentItems[index]) {
            return;
         }

         const targetItem = currentItems[index];
         const updatedFormItems = currentItems.filter((_, i) => i !== index);

         form.setValue('cart_items', updatedFormItems);
         setCartItems(updatedFormItems as TCartItem[]);

         const updatedCartItems = cartItemState.filter(
            (item) =>
               !(
                  item.model_id === targetItem.model_id &&
                  item.color.normalized_name ===
                     targetItem.color.normalized_name &&
                  item.storage.normalized_name ===
                     targetItem.storage.normalized_name
               ),
         );

         dispatch(AddCartItems(updatedCartItems as TCartItem[]));

         const deleteBasket = async () => {
            if (deleteBasketAsync) {
               await deleteBasketAsync();
            }
         };

         deleteBasket();
      },
      [cartItemState, dispatch, form, deleteBasketAsync],
   );

   // Update cartItems state when watched values change
   useEffect(() => {
      if (watchedCartItems) {
         setCartItems(watchedCartItems as TCartItem[]);
      }
   }, [watchedCartItems]);

   // Watch for selection changes and call API
   useEffect(() => {
      if (!watchedCartItems || !storeBasketAsync) return;

      // Check if any is_selected value has changed
      const hasSelectionChanged = prevCartItemsRef.current.some(
         (prevItem, index) => {
            const currentItem = watchedCartItems[index];
            return (
               currentItem && prevItem?.is_selected !== currentItem.is_selected
            );
         },
      );

      if (hasSelectionChanged && prevCartItemsRef.current.length > 0) {
         // Call storeBasket API
         const payload: IStoreBasketPayload = {
            cart_items: watchedCartItems.map((item) => ({
               is_selected: item.is_selected,
               sku_id: item.sku_id,
               quantity: item.quantity,
            })),
         };

         storeBasketAsync(payload);
      }

      // Update ref for next comparison
      prevCartItemsRef.current = watchedCartItems as typeof cartItems;
   }, [watchedCartItems, storeBasketAsync, cartItemState, dispatch]);

   return {
      form,
      cartItems,
      handleQuantityChange,
      handleRemoveItem,
   };
};

export default useCartForm;
