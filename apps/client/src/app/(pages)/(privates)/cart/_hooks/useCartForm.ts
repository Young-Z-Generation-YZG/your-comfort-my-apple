import { useCallback, useEffect, useMemo, useRef } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import {
   TCart,
   TCartItem,
   TStoreBasketPayload,
} from '~/infrastructure/services/basket.service';
import { useAppSelector } from '~/infrastructure/redux/store';

const cartItemSchema = z.object({
   is_selected: z.boolean(),
   sku_id: z.string().min(1, { message: 'SKU ID is required' }),
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

const DEBOUNCE_DELAY = 100;

interface UseCartFormProps {
   basketData: TCart;
   storeBasketAsync: (payload: TStoreBasketPayload) => Promise<any>;
   storeBasketSync: (cartItems: TCartItem[]) => void;
   deleteBasket: () => Promise<any>;
}

export const useCartForm = ({
   basketData,
   storeBasketAsync,
   storeBasketSync,
   deleteBasket,
}: UseCartFormProps) => {
   const { isAuthenticated } = useAppSelector((state) => state.auth);

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

      // Skip if we don't have basket data yet (initial load)
      if (!basketData || !cartItems) return;

      const handleBasketUpdate = async () => {
         try {
            console.log('cartItems', cartItems);
            console.log('isAuthenticated', isAuthenticated);

            if (cartItems.length === 0) {
               // Delete basket when empty
               await deleteBasket();
            } else {
               // Store basket with items
               if (isAuthenticated) {
                  await storeBasketAsync({
                     cart_items: cartItems.map((item) => ({
                        is_selected: item.is_selected,
                        model_id: item.model_id,
                        sku_id: item.sku_id,
                        color: item.color,
                        model: item.model,
                        storage: item.storage,
                        quantity: item.quantity,
                     })),
                  });
               } else {
                  const cartItems: TCartItem[] = basketData.cart_items.map(
                     (item) => ({
                        is_selected: item.is_selected,
                        model_id: item.model_id,
                        sku_id: item.sku_id,
                        color: item.color,
                        model: item.model,
                        storage: item.storage,
                        quantity: item.quantity,
                        product_name: item.product_name,
                        display_image_url: item.display_image_url,
                        unit_price: item.unit_price,
                        promotion: item.promotion,
                        sub_total_amount: item.sub_total_amount,
                        index: item.index,
                     }),
                  );

                  // storeBasketSync(cartItems);
               }
            }
         } catch (error) {
            console.error('Failed to update basket:', error);
         }
      };

      const timeoutId = setTimeout(handleBasketUpdate, DEBOUNCE_DELAY);
      return () => clearTimeout(timeoutId);
   }, [
      cartItems,
      storeBasketAsync,
      deleteBasket,
      basketData,
      isAuthenticated,
      storeBasketSync,
   ]);

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

   // Handle remove item
   const handleRemoveItem = useCallback(
      (index: number) => {
         console.log('index', index);
         const currentCartItems = form.getValues('cart_items');
         if (currentCartItems && currentCartItems[index]) {
            const updatedItems = [...currentCartItems];
            updatedItems.splice(index, 1);

            console.log('updatedItems', updatedItems);

            form.setValue('cart_items', updatedItems);
            console.log(
               'form.getValues("cart_items")',
               form.getValues('cart_items'),
            );
         }
      },
      [form],
   );

   return {
      form,
      cartItems,
      selectedItems,
      handleQuantityChange,
      handleRemoveItem,
   };
};
