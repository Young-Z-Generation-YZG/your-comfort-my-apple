import { useMemo } from 'react';
import {
   IBasket,
   ICartItem,
} from '~/domain/interfaces/baskets/basket.interface';
import {
   useDeleteBasketAsyncMutation,
   useGetBasketAsyncQuery,
   useStoreBasketAsyncMutation,
} from '~/infrastructure/services/basket.service';

interface UseCartProps {
   couponCode?: string;
   fallbackData?: IBasket;
}

interface CartCalculations {
   itemCount: number;
   subtotal: number;
   totalSavings: number;
   total: number;
   discount: number;
}

interface UseCartReturn {
   // Basket data
   basketData: IBasket | undefined;
   isFallbackMode: boolean;

   // Loading states
   isLoading: boolean;
   isLoadingBasket: boolean;
   isLoadingStoreBasket: boolean;
   isLoadingDeleteBasket: boolean;

   // Success/Error states
   isSuccessGetBasket: boolean;
   isErrorGetBasket: boolean;
   errorGetBasket: unknown;

   // Mutations
   storeBasket: ReturnType<typeof useStoreBasketAsyncMutation>[0];
   deleteBasket: ReturnType<typeof useDeleteBasketAsyncMutation>[0];

   // Calculated values
   cartCalculations: CartCalculations;
}

export const useCart = ({
   couponCode = '',
   fallbackData,
}: UseCartProps = {}): UseCartReturn => {
   // Single API call for basket data
   const {
      data: apiBasketData,
      isLoading: isLoadingBasket,
      isSuccess: isSuccessGetBasket,
      isError: isErrorGetBasket,
      error: errorGetBasket,
   } = useGetBasketAsyncQuery({
      _couponCode: couponCode,
   });

   // Use fallback data if API fails
   const basketData = useMemo<IBasket | undefined>(() => {
      if (isErrorGetBasket && fallbackData) {
         return fallbackData;
      }
      return apiBasketData;
   }, [isErrorGetBasket, fallbackData, apiBasketData]);

   // Track if we're in fallback mode
   const isFallbackMode = useMemo(
      () => isErrorGetBasket && !!fallbackData,
      [isErrorGetBasket, fallbackData],
   );

   const [storeBasket, { isLoading: isLoadingStoreBasket }] =
      useStoreBasketAsyncMutation();

   const [deleteBasket, { isLoading: isLoadingDeleteBasket }] =
      useDeleteBasketAsyncMutation();

   // Consolidated loading state
   const isLoading = useMemo(
      () => isLoadingBasket || isLoadingStoreBasket || isLoadingDeleteBasket,
      [isLoadingBasket, isLoadingStoreBasket, isLoadingDeleteBasket],
   );

   // Memoized calculated values
   const cartCalculations = useMemo<CartCalculations>(() => {
      const items = basketData?.cart_items || [];
      const itemCount = items.length;

      // Calculate subtotal from item sub_total_amount
      const subtotal = items.reduce(
         (sum: number, item: ICartItem) => sum + item.sub_total_amount,
         0,
      );

      // Calculate total discount from promotions
      const discount = items.reduce((sum: number, item: ICartItem) => {
         if (item.promotion) {
            return sum + (item.unit_price - 0) * item.quantity;
         }
         return sum;
      }, 0);

      const totalSavings = discount;
      const total = basketData?.total_amount || subtotal;

      return {
         itemCount,
         subtotal,
         totalSavings,
         total,
         discount,
      };
   }, [basketData]);

   return {
      basketData,
      isFallbackMode,
      isLoading,
      isLoadingBasket,
      isLoadingStoreBasket,
      isLoadingDeleteBasket,
      isSuccessGetBasket,
      isErrorGetBasket,
      errorGetBasket,
      storeBasket,
      deleteBasket,
      cartCalculations,
   };
};
