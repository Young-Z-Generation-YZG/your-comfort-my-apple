import { useSearchParams, useRouter } from 'next/navigation';
import { useCallback, useMemo } from 'react';

type FilterValue =
   | string
   | number
   | boolean
   | string[]
   | number[]
   | null
   | undefined;

type FilterOptions = {
   shallow?: boolean;
   scroll?: boolean;
};

const useFilter = <T extends Record<string, FilterValue>>() => {
   const router = useRouter();
   const searchParams = useSearchParams();

   // Parse search params into typed filter object
   const filters = useMemo((): Partial<T> => {
      const params: Record<string, any> = {};
      const processedKeys = new Set<string>();

      // Get all unique keys
      const keys = Array.from(searchParams.keys());

      keys.forEach((key) => {
         if (processedKeys.has(key)) return;
         processedKeys.add(key);

         // Get all values for this key
         const allValues = searchParams.getAll(key);

         // If there are multiple values for the same key, treat as array
         if (allValues.length > 1) {
            params[key] = allValues.map((v) => {
               // Try to parse as number
               if (!isNaN(Number(v)) && v !== '') {
                  return Number(v);
               }
               return v;
            });
         } else {
            const value = allValues[0];

            // Handle array values (comma-separated)
            if (value.includes(',')) {
               params[key] = value.split(',').filter(Boolean);
            }
            // Handle boolean values
            else if (value === 'true' || value === 'false') {
               params[key] = value === 'true';
            }
            // Handle numeric values
            else if (!isNaN(Number(value)) && value !== '') {
               params[key] = Number(value);
            }
            // Handle null
            else if (value === 'null') {
               params[key] = null;
            }
            // Handle string values
            else {
               params[key] = value;
            }
         }
      });

      return params as Partial<T>;
   }, [searchParams]);

   // Set multiple filters at once
   const setFilters = useCallback(
      (
         newFilters: Partial<T>,
         options: FilterOptions = { shallow: true, scroll: false },
      ) => {
         const params = new URLSearchParams(searchParams.toString());

         Object.entries(newFilters).forEach(([key, value]) => {
            // First delete existing values for this key
            params.delete(key);

            if (value === null || value === undefined || value === '') {
               // Already deleted, do nothing
            } else if (Array.isArray(value)) {
               if (value.length > 0) {
                  // Append each value separately for proper multi-value support
                  value.forEach((v) => {
                     params.append(key, String(v));
                  });
               }
            } else {
               params.set(key, String(value));
            }
         });

         router.push(`?${params.toString()}`, { scroll: options.scroll });
      },
      [searchParams, router],
   );

   // Set a single filter
   const setFilter = useCallback(
      (
         key: keyof T,
         value: FilterValue,
         options: FilterOptions = { shallow: true, scroll: false },
      ) => {
         setFilters({ [key]: value } as Partial<T>, options);
      },
      [setFilters],
   );

   // Remove specific filters
   const removeFilters = useCallback(
      (
         keys: (keyof T)[],
         options: FilterOptions = { shallow: true, scroll: false },
      ) => {
         const params = new URLSearchParams(searchParams.toString());

         keys.forEach((key) => {
            params.delete(key as string);
         });

         router.push(`?${params.toString()}`, { scroll: options.scroll });
      },
      [searchParams, router],
   );

   // Remove a single filter
   const removeFilter = useCallback(
      (
         key: keyof T,
         options: FilterOptions = { shallow: true, scroll: false },
      ) => {
         removeFilters([key], options);
      },
      [removeFilters],
   );

   // Clear all filters
   const clearFilters = useCallback(
      (options: FilterOptions = { shallow: true, scroll: false }) => {
         router.push(window.location.pathname, { scroll: options.scroll });
      },
      [router],
   );

   // Check if a specific filter is active
   const hasFilter = useCallback(
      (key: keyof T): boolean => {
         return searchParams.has(key as string);
      },
      [searchParams],
   );

   // Get the count of active filters
   const activeFilterCount = useMemo(() => {
      return Array.from(searchParams.keys()).length;
   }, [searchParams]);

   // Check if any filters are active
   const hasActiveFilters = useMemo(() => {
      return activeFilterCount > 0;
   }, [activeFilterCount]);

   // Toggle a value in an array filter (add if not present, remove if present)
   const toggleArrayFilter = useCallback(
      (
         key: keyof T,
         value: string | number,
         options: FilterOptions = { shallow: true, scroll: false },
      ) => {
         const currentValue = filters[key];
         let newValue: any[];

         if (Array.isArray(currentValue)) {
            // Check if value exists in array
            if (currentValue.includes(value as never)) {
               // Remove it
               newValue = currentValue.filter((v: any) => v !== value);
            } else {
               // Add it
               newValue = [...currentValue, value];
            }
         } else {
            // Initialize as array with single value
            newValue = [value];
         }

         setFilters({ [key]: newValue } as Partial<T>, options);
      },
      [filters, setFilters],
   );

   // Add a value to an array filter (without removing)
   const addToArrayFilter = useCallback(
      (
         key: keyof T,
         value: string | number,
         options: FilterOptions = { shallow: true, scroll: false },
      ) => {
         const currentValue = filters[key];
         let newValue: any[];

         if (Array.isArray(currentValue)) {
            if (!currentValue.includes(value as never)) {
               newValue = [...currentValue, value];
            } else {
               // Already exists, no change needed
               return;
            }
         } else {
            newValue = [value];
         }

         setFilters({ [key]: newValue } as Partial<T>, options);
      },
      [filters, setFilters],
   );

   // Remove a value from an array filter
   const removeFromArrayFilter = useCallback(
      (
         key: keyof T,
         value: string | number,
         options: FilterOptions = { shallow: true, scroll: false },
      ) => {
         const currentValue = filters[key];

         if (Array.isArray(currentValue)) {
            const newValue = currentValue.filter((v: any) => v !== value);
            if (newValue.length > 0) {
               setFilters({ [key]: newValue } as Partial<T>, options);
            } else {
               // Remove the filter entirely if array is empty
               removeFilter(key, options);
            }
         }
      },
      [filters, setFilters, removeFilter],
   );

   return {
      filters,
      setFilter,
      setFilters,
      removeFilter,
      removeFilters,
      clearFilters,
      hasFilter,
      activeFilterCount,
      hasActiveFilters,
      toggleArrayFilter,
      addToArrayFilter,
      removeFromArrayFilter,
   };
};

export default useFilter;
