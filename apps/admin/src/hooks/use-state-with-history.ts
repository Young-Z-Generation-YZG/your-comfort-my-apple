import { useCallback, useState, useRef } from 'react';

/**
 * This hook can be helpful in situations where you want to keep track of the state's history,
 * for example, when you want to implement undo or redo functionality or to allow the user to navigate through the history of changes.
 * @param initialValue - The initial value of the state
 * @param capacity - The capacity of the history
 * @returns The value of the state, the set function, and the history
 */
const useStateWithHistory = (initialValue: any, capacity: number = 10) => {
   const [value, setValue] = useState(initialValue);
   const historyRef = useRef([value]);
   const pointerRef = useRef(0);

   const set = useCallback(
      (v: any) => {
         const resolvedValue = typeof v === 'function' ? v(value) : v;
         if (historyRef.current[pointerRef.current] !== resolvedValue) {
            if (pointerRef.current < historyRef.current.length - 1) {
               historyRef.current.splice(pointerRef.current + 1);
            }
            historyRef.current.push(resolvedValue);

            while (historyRef.current.length > capacity) {
               historyRef.current.shift();
            }
            pointerRef.current = historyRef.current.length - 1;
         }
         setValue(resolvedValue);
      },
      [capacity, value],
   );

   const back = useCallback(() => {
      if (pointerRef.current <= 0) return;
      pointerRef.current--;
      setValue(historyRef.current[pointerRef.current]);
   }, []);

   const forward = useCallback(() => {
      if (pointerRef.current >= historyRef.current.length - 1) return;
      pointerRef.current++;
      setValue(historyRef.current[pointerRef.current]);
   }, []);

   const go = useCallback((index: number) => {
      if (index < 0 || index > historyRef.current.length - 1) return;
      pointerRef.current = index;
      setValue(historyRef.current[pointerRef.current]);
   }, []);

   return [
      value,
      set,
      {
         history: historyRef.current,
         pointer: pointerRef.current,
         back,
         forward,
         go,
      },
   ];
};

export default useStateWithHistory;
