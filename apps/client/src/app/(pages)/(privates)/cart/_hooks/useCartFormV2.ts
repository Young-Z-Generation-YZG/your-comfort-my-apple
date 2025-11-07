import { useCallback, useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { TCart, TCartItem } from '~/infrastructure/services/basket.service';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useDispatch } from 'react-redux';
import { AddCartItems } from '~/infrastructure/redux/features/cart.slice';

const cartItemSchema = z.object({
   is_selected: z.boolean(),
   model_id: z.string().min(1, { message: 'Model ID is required' }),
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
   basketData: TCart;
}

const useCartFormV2 = ({ basketData }: UseCartFormProps) => {
   const [cartItems, setCartItems] = useState<TCartItem[]>([]);

   const cartItemState = useAppSelector((state) => state.cart.cart_items);

   const dispatch = useDispatch();

   const form = useForm<CartFormData>({
      resolver: zodResolver(cartFormSchema),
      defaultValues: {
         cart_items: basketData.cart_items.map((item) => ({
            is_selected: item.is_selected,
            model_id: item.model_id,
            color: item.color,
            model: item.model,
            storage: item.storage,
            quantity: item.quantity,
         })),
      },
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
      },
      [cartItemState, dispatch, form],
   );

   useEffect(() => {
      setCartItems(form.getValues('cart_items') as TCartItem[]);
   }, [form]);

   return {
      form,
      cartItems,
      handleQuantityChange,
      handleRemoveItem,
   };
};

export default useCartFormV2;
