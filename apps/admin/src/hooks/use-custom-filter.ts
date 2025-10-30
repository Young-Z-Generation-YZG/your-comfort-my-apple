// hooks/useCustomUserFilter.ts
import { useRouter, useSearchParams } from 'next/navigation';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';

type FilterValue =
   | string
   | number
   | boolean
   | string[]
   | number[]
   | null
   | undefined;

/**
 * Convert the **raw** `Record<string, string[]>` that comes from `searchParams`
 * into the typed `T` shape.
 */
function rawToTyped<T extends Record<string, FilterValue>>(
   raw: Record<string, string[]>,
): T {
   const typed = {} as T;

   for (const [key, values] of Object.entries(raw)) {
      // single-value keys (page, limit, email …)
      if (key === '_page' || key === '_limit') {
         typed[key as keyof T] = (values[0] ? Number(values[0]) : null) as any;
         continue;
      }

      // array-value keys
      if (key === '_orderStatus') {
         typed[key as keyof T] = (values.length ? values : null) as any;
         continue;
      }

      // fallback – treat as single string (or null)
      typed[key as keyof T] = (values[0] ?? null) as any;
   }

   return typed;
}

/**
 * Convert a **typed** filter object back to the **raw** shape required for the URL.
 * Duplicated keys are emitted multiple times.
 */
function typedToRaw<T extends Record<string, FilterValue>>(
   typed: Partial<T>,
): Record<string, string[]> {
   const raw: Record<string, string[]> = {};

   for (const [key, value] of Object.entries(typed)) {
      if (value === null || value === undefined) continue;

      if (Array.isArray(value)) {
         // arrays → repeat the key
         raw[key] = value.map(String);
      } else {
         // single value → store as a 1-element array (URLSearchParams expects strings)
         raw[key] = [String(value)];
      }
   }

   return raw;
}

/* -------------------------------------------------------------------------- */
/*                                 MAIN HOOK                                  */
/* -------------------------------------------------------------------------- */
const useFilters = <T extends Record<string, FilterValue>>() => {
   const router = useRouter();
   const searchParams = useSearchParams();
   const firstRenderRef = useRef(true);

   /* -------------------------- 1. RAW → TYPED -------------------------- */
   const rawFilters = useMemo(() => {
      const map: Record<string, string[]> = {};
      for (const [k, v] of searchParams.entries()) {
         (map[k] ??= []).push(v);
      }
      return map;
   }, [searchParams]);

   const typedFilters = useMemo(() => rawToTyped<T>(rawFilters), [rawFilters]);

   const [filters, _setFilters] = useState<T>(typedFilters);

   /* -------------------------- 2. SYNC URL → STATE -------------------------- */
   useEffect(() => {
      if (firstRenderRef.current) {
         firstRenderRef.current = false;
         return;
      }
      // URL changed **outside** of our `setFilters` → update local state
      _setFilters(typedFilters);
   }, [typedFilters]);

   /* -------------------------- 3. SET FILTERS -------------------------- */
   const setFilters = useCallback(
      (updater: Partial<T> | ((prev: T) => Partial<T>)) => {
         const newPartial =
            typeof updater === 'function' ? updater(filters) : updater;

         const merged = { ...filters, ...newPartial };
         _setFilters(merged as T); // cast is safe – we only merge known keys

         // ---- build URL ----
         const raw = typedToRaw(merged);
         const params = new URLSearchParams();

         for (const [k, vals] of Object.entries(raw)) {
            for (const v of vals) {
               params.append(k, v);
            }
         }

         const query = params.toString();
         router.push(`?${query}`, { scroll: false });
      },
      [filters, router],
   );

   /* --------------------------------------------------------------------- */
   return {
      /** Current filter values – typed as `T` (e.g. `TOrderFilter`) */
      filters: filters as T,

      /** Update any subset of the filters and push the new URL */
      setFilters,
   };
};

export default useFilters;
