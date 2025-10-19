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

export const useShopFilters = () => {
   const router = useRouter();
   const searchParams = useSearchParams();

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

   const updateFilters = useCallback(
      (updates: Partial<ShopFilters>) => {
         const params = new URLSearchParams(searchParams);

         if (updates.colors !== undefined) {
            params.delete('_colors');
            updates.colors.forEach((color) => params.append('_colors', color));
         }

         if (updates.models !== undefined) {
            params.delete('_models');
            updates.models.forEach((model) => params.append('_models', model));
         }

         if (updates.storages !== undefined) {
            params.delete('_storages');
            updates.storages.forEach((storage) =>
               params.append('_storages', storage),
            );
         }

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

         if (updates.priceSort !== undefined) {
            if (updates.priceSort) {
               params.set('_priceSort', updates.priceSort);
            } else {
               params.delete('_priceSort');
            }
         }

         if (updates.page !== undefined) {
            params.set('_page', updates.page.toString());
         }

         if (updates.pageSize !== undefined) {
            params.set('_limit', updates.pageSize.toString());
         }

         router.push(`${window.location.pathname}?${params.toString()}`, {
            scroll: false,
         });
      },
      [searchParams, router],
   );

   const clearFilters = useCallback(() => {
      const params = new URLSearchParams();
      params.set('_page', '1');
      params.set('_limit', filters.pageSize.toString());
      router.push(`${window.location.pathname}?${params.toString()}`, {
         scroll: false,
      });
   }, [router, filters.pageSize]);

   const queryString = useMemo(() => {
      return searchParams.toString();
   }, [searchParams]);

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
