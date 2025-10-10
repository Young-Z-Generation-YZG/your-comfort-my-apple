import { useSearchParams, useRouter } from 'next/navigation';
import { useCallback, useMemo } from 'react';

export type ShopFilters = {
   colors: string[];
   models: string[];
   storages: string[];
   priceRange: {
      min: number;
      max: number;
   };
   priceSort?: 'ASC' | 'DESC' | null;
   page: number;
   pageSize: number;
};

/**
 * Custom hook for managing shop filters via URL params
 * - Single source of truth: URL
 * - No Redux needed
 * - Automatic persistence and sharing
 */
export const useShopFilters = () => {
   const router = useRouter();
   const searchParams = useSearchParams();

   // Parse filters from URL
   const filters = useMemo((): ShopFilters => {
      const priceSortParam = searchParams.get('_priceSort');
      return {
         colors: searchParams.getAll('_colors'),
         models: searchParams.getAll('_models'),
         storages: searchParams.getAll('_storages'),
         priceRange: {
            min: Number(searchParams.get('_minPrice')) || 0,
            max: Number(searchParams.get('_maxPrice')) || 2000,
         },
         priceSort: priceSortParam as 'ASC' | 'DESC' | null,
         page: Number(searchParams.get('_page')) || 1,
         pageSize: Number(searchParams.get('_limit')) || 10,
      };
   }, [searchParams]);

   // Update filters
   const updateFilters = useCallback(
      (updates: Partial<ShopFilters>) => {
         const params = new URLSearchParams(searchParams);

         // Handle colors
         if (updates.colors !== undefined) {
            params.delete('_colors');
            updates.colors.forEach((color) => params.append('_colors', color));
         }

         // Handle models
         if (updates.models !== undefined) {
            params.delete('_models');
            updates.models.forEach((model) => params.append('_models', model));
         }

         // Handle storages
         if (updates.storages !== undefined) {
            params.delete('_storages');
            updates.storages.forEach((storage) =>
               params.append('_storages', storage),
            );
         }

         // Handle price range
         if (updates.priceRange) {
            if (updates.priceRange.min !== 0) {
               params.set('_minPrice', updates.priceRange.min.toString());
            } else {
               params.delete('_minPrice');
            }

            if (updates.priceRange.max !== 2000) {
               params.set('_maxPrice', updates.priceRange.max.toString());
            } else {
               params.delete('_maxPrice');
            }
         }

         // Handle price sort
         if (updates.priceSort !== undefined) {
            if (updates.priceSort) {
               params.set('_priceSort', updates.priceSort);
            } else {
               params.delete('_priceSort');
            }
         }

         // Handle pagination
         if (updates.page !== undefined) {
            params.set('_page', updates.page.toString());
         }

         if (updates.pageSize !== undefined) {
            params.set('_limit', updates.pageSize.toString());
         }

         // Update URL
         router.push(`${window.location.pathname}?${params.toString()}`, {
            scroll: false,
         });
      },
      [searchParams, router],
   );

   // Clear all filters
   const clearFilters = useCallback(() => {
      const params = new URLSearchParams();
      params.set('_page', '1');
      params.set('_limit', filters.pageSize.toString());
      router.push(`${window.location.pathname}?${params.toString()}`, {
         scroll: false,
      });
   }, [router, filters.pageSize]);

   // Build API query string
   const queryString = useMemo(() => {
      return searchParams.toString();
   }, [searchParams]);

   // Count active filters
   const activeFiltersCount = useMemo(() => {
      return (
         filters.colors.length + filters.models.length + filters.storages.length
      );
   }, [filters]);

   return {
      filters,
      updateFilters,
      clearFilters,
      queryString,
      activeFiltersCount,
   };
};
