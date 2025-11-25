/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from 'react';

/**
 * This hook can be helpful when you want to track when a specific DOM element comes into view or goes out of sight,
 * for example, to lazy-load images, track scroll position, or display elements on demand.
 * @param ref - The ref to the element to watch
 * @param rootMargin - The margin to use for the observer
 * @returns The visibility of the element
 */
const useOnScreen = (
   ref: React.RefObject<HTMLElement>,
   rootMargin: string = '0px',
) => {
   const [isVisible, setIsVisible] = useState(false);

   useEffect(() => {
      if (ref.current == null) return;
      const observer = new IntersectionObserver(
         ([entry]) => setIsVisible(entry.isIntersecting),
         { rootMargin },
      );
      observer.observe(ref.current);
      return () => {
         if (ref.current == null) return;
         observer.unobserve(ref.current);
      };
   }, [ref.current, rootMargin]);

   return isVisible;
};

export default useOnScreen;
