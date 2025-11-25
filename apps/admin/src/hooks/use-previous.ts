import { useRef } from 'react';
/**
 * This hook can be helpful in situations where you need to have access to the previous value of a variable,
 * for example, when you want to compare the current value with the value earlier to check if it has changed or when you want to track the changes of a variable over time.
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
