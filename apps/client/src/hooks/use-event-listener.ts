import { useEffect, useRef } from 'react';

/**
 * A hook that listens for a specific event on a given element
 * @param eventType - The type of event to listen for
 * @param callback - The callback to call when the event is triggered
 * @param element - The element to listen for the event on
 * @returns void
 */
const useEventListener = (
   eventType: string,
   callback: (event: Event) => void,
   element: Window = window,
) => {
   const callbackRef = useRef(callback);

   //  console.log('[Hook:useEventListener] callbackRef', callbackRef);

   useEffect(() => {
      callbackRef.current = callback;

      // console.log(
      //    '[Hook:useEventListener] callbackRef.current',
      //    callbackRef.current,
      // );
   }, [callback]);

   useEffect(() => {
      if (element == null) return;
      const handler = (e: Event) => callbackRef.current(e);
      element.addEventListener(eventType, handler);

      return () => element.removeEventListener(eventType, handler);
   }, [eventType, element]);
};

export default useEventListener;
