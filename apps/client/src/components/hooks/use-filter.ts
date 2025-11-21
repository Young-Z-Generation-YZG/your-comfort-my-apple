// hooks/useFilters.ts
import { useRouter, useSearchParams } from 'next/navigation';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';

export type FilterValue =
   | string
   | number
   | boolean
   | string[]
   | number[]
   | null
   | undefined;

export type FilterSchema = {
   [key: string]:
      | 'string'
      | 'number'
      | 'boolean'
      | { array: 'string' | 'number' };
};

function rawToTyped<T extends Record<string, FilterValue>>(
   raw: Record<string, string[]>,
   schema: FilterSchema,
): T {
   const result = {} as T;

   for (const [key, values] of Object.entries(raw)) {
      const rule = schema[key];

      if (!rule) {
         result[key as keyof T] = (values[0] ?? null) as any;
         continue;
      }

      if (rule === 'number') {
         result[key as keyof T] = (values[0] ? Number(values[0]) : null) as any;
      } else if (rule === 'boolean') {
         result[key as keyof T] = (
            values[0] ? values[0] === 'true' : null
         ) as any;
      } else if (typeof rule === 'object' && 'array' in rule) {
         const arr =
            rule.array === 'number'
               ? values
                    .map((v) => (v ? Number(v) : null))
                    .filter((v) => v !== null)
               : values;
         result[key as keyof T] = (arr.length ? arr : null) as any;
      } else {
         result[key as keyof T] = (values[0] ?? null) as any;
      }
   }

   return result;
}

function typedToRaw<T extends Record<string, FilterValue>>(
   typed: Partial<T>,
): Record<string, string[]> {
   const raw: Record<string, string[]> = {};

   for (const [key, value] of Object.entries(typed)) {
      if (value === null || value === undefined) continue;

      if (Array.isArray(value)) {
         raw[key] = value.map(String);
      } else {
         raw[key] = [String(value)];
      }
   }

   return raw;
}

// Deep equality check (safe for null, arrays, primitives)
function deepEqual(a: any, b: any): boolean {
   if (a === b) return true;
   if (a == null || b == null) return a === b;
   if (typeof a !== 'object' || typeof b !== 'object') return false;

   const keysA = Object.keys(a);
   const keysB = Object.keys(b);
   if (keysA.length !== keysB.length) return false;

   for (const key of keysA) {
      if (!keysB.includes(key)) return false;
      if (!deepEqual(a[key], b[key])) return false;
   }

   return true;
}

const useFilters = <T extends Record<string, FilterValue>>(
   schema: FilterSchema,
) => {
   const router = useRouter();
   const searchParams = useSearchParams();
   const firstRenderRef = useRef(true);

   const rawFilters = useMemo(() => {
      const map: Record<string, string[]> = {};
      for (const [k, v] of searchParams.entries()) {
         (map[k] ??= []).push(v);
      }
      return map;
   }, [searchParams]);

   const typedFilters = useMemo(
      () => rawToTyped<T>(rawFilters, schema),
      [rawFilters, schema],
   );

   const [filters, _setFilters] = useState<T>(typedFilters);

   // Sync URL → state ONLY if different
   useEffect(() => {
      if (firstRenderRef.current) {
         firstRenderRef.current = false;
         return;
      }

      if (!deepEqual(filters, typedFilters)) {
         _setFilters(typedFilters);
      }
   }, [typedFilters, filters]);

   const setFilters = useCallback(
      (updater: Partial<T> | ((prev: T) => Partial<T>)) => {
         const newPartial =
            typeof updater === 'function' ? updater(filters) : updater;
         const merged = { ...filters, ...newPartial } as T;

         // Avoid unnecessary updates
         if (deepEqual(filters, merged)) return;

         _setFilters(merged);

         const raw = typedToRaw(merged);
         const params = new URLSearchParams();
         for (const [k, vals] of Object.entries(raw)) {
            vals.forEach((v) => params.append(k, v));
         }

         const query = params.toString();
         router.push(`?${query}`, { scroll: false });
      },
      [filters, router],
   );

   return {
      filters: filters as T,
      setFilters,
   };
};

export default useFilters;
