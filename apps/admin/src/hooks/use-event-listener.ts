import { useEffect, useRef } from 'react';

const useEventListener = (
   eventType: string,
   callback: (event: Event) => void,
   element: Window = window,
) => {
   const callbackRef = useRef(callback);

   useEffect(() => {
      callbackRef.current = callback;
   }, [callback]);

   useEffect(() => {
      if (element == null) return;
      const handler = (e: Event) => callbackRef.current(e);
      element.addEventListener(eventType, handler);

      return () => element.removeEventListener(eventType, handler);
   }, [eventType, element]);
};

export default useEventListener;
