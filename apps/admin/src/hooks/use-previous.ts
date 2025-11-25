import { useRef } from 'react';
/**
 * A hook that returns the previous value of a given value
 * @param value - The value to track
 * @returns The previous value
 */
const usePrevious = (value: any) => {
   const currentRef = useRef(value);
   const previousRef = useRef<typeof value>(value);

   if (currentRef.current !== value) {
      previousRef.current = currentRef.current;
      currentRef.current = value;
   }

   return previousRef.current;
};

export default usePrevious;
