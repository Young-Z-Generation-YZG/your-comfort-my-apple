/* eslint-disable react-hooks/exhaustive-deps */
import { type DependencyList, useEffect, useRef } from 'react';

/**
 * This hook can be helpful in situations where you want to run some logic only when specific values change and not on the initial render.
 * For example, when you want to fetch data from an API after the user has selected a particular option from a drop-down menu or when you want to update the position of an element on the screen after the size of the window changes.
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
