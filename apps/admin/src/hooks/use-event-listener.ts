import { useEffect, useRef } from 'react';

const useEventListener = (
   eventType: string,
   callback: (event: Event) => void,
   element: Window = window,
) => {
   const callbackRef = useRef(callback);

   console.log('[Hook:useEventListener] callbackRef', callbackRef);

   useEffect(() => {
      callbackRef.current = callback;

      console.log(
         '[Hook:useEventListener] callbackRef.current',
         callbackRef.current,
      );
   }, [callback]);

   useEffect(() => {
      if (element == null) return;
      const handler = (e: Event) => callbackRef.current(e);
      element.addEventListener(eventType, handler);

      return () => element.removeEventListener(eventType, handler);
   }, [eventType, element]);
};

export default useEventListener;
