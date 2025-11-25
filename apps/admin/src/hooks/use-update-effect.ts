/* eslint-disable react-hooks/exhaustive-deps */
import { type DependencyList, useEffect, useRef } from 'react';

/**
 * A hook that runs a callback on update, but not on initial render
 * @param callback - The callback to run on update
 * @param dependencies - The dependencies to watch
 * @returns void
 */
const useUpdateEffect = (
   callback: () => void,
   dependencies: DependencyList,
) => {
   const firstRenderRef = useRef(true);
   const callbackRef = useRef(callback);

   useEffect(() => {
      callbackRef.current = callback;
   }, [callback]);

   useEffect(() => {
      if (firstRenderRef.current) {
         firstRenderRef.current = false;
         return;
      }
      return callbackRef.current();
   }, dependencies);
};

export default useUpdateEffect;
