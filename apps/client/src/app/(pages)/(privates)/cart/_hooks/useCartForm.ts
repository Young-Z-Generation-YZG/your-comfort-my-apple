import { useCallback, useEffect, useMemo, useRef } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { IBasket } from '~/domain/interfaces/baskets/basket.interface';

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

const cartFormSchema = z.object({
   cart_items: z.array(cartItemSchema),
});

export type CartFormData = z.infer<typeof cartFormSchema>;

const DEBOUNCE_DELAY = 500;

interface UseCartFormProps {
   basketData: IBasket | undefined;
   storeBasket: (data: { cart_items: any[] }) => Promise<any>;
}

export const useCartForm = ({ basketData, storeBasket }: UseCartFormProps) => {
   const form = useForm<CartFormData>({
      resolver: zodResolver(cartFormSchema),
   });

   // Track if we're resetting from server data to prevent unnecessary API calls
   const isResettingFromServer = useRef(false);

   // Watch form values
   const cartItems = useWatch({
      control: form.control,
      name: 'cart_items',
   });

   // Calculate selected items
   const selectedItems = useMemo(() => {
      return cartItems?.filter((item) => item.is_selected) || [];
   }, [cartItems]);

   // Transform basket data for form
   const formData = useMemo(() => {
      if (!basketData?.cart_items) return { cart_items: [] };

      return {
         cart_items: basketData.cart_items.map((item) => ({
            is_selected: item.is_selected,
            model_id: item.model_id,
            color: item.color,
            model: item.model,
            storage: item.storage,
            quantity: item.quantity,
         })),
      };
   }, [basketData?.cart_items]);

   // Reset form when basket data changes
   useEffect(() => {
      isResettingFromServer.current = true;
      form.reset(formData);
   }, [formData, form]);

   // Debounced API call to prevent multiple requests
   useEffect(() => {
      // Skip if we're resetting from server data
      if (isResettingFromServer.current) {
         isResettingFromServer.current = false;
         return;
      }

      if (!cartItems || cartItems.length === 0) return;

      const handleStoreBasket = async () => {
         try {
            await storeBasket({
               cart_items: cartItems.map((item) => ({
                  is_selected: item.is_selected,
                  model_id: item.model_id,
                  color: item.color,
                  model: item.model,
                  storage: item.storage,
                  quantity: item.quantity,
               })),
            });
         } catch (error) {
            console.error('Failed to store basket:', error);
         }
      };

      const timeoutId = setTimeout(handleStoreBasket, DEBOUNCE_DELAY);
      return () => clearTimeout(timeoutId);
   }, [cartItems, storeBasket]);

   // Handle quantity change
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
         }
      },
      [form],
   );

   return {
      form,
      cartItems,
      selectedItems,
      handleQuantityChange,
   };
};
