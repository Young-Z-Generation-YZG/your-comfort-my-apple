import { useEffect } from 'react';
import { useAppSelector } from '~/infrastructure/redux/store';
import useBasketService from './api/use-basket-service';

/**
 * Custom hook to automatically sync basket with Redux when user logs in
 * This hook should be used in the root layout or main app component
 */
export const useBasketSync = () => {
   const isAuthenticated = useAppSelector(
      (state) => state.auth.isAuthenticated,
   );
   const basketIsLoaded = useAppSelector((state) => state.cart.value.isLoaded);
   const { getBasketAsync } = useBasketService();

   useEffect(() => {
      // Fetch and sync basket when user logs in and basket hasn't been loaded yet
      if (isAuthenticated && !basketIsLoaded) {
         const syncBasketData = async () => {
            await getBasketAsync({});
         };

         syncBasketData();
      }
   }, [isAuthenticated, basketIsLoaded, getBasketAsync]);

   // Return loading state or basket info if needed
   return {
      isAuthenticated,
      basketIsLoaded,
   };
};
