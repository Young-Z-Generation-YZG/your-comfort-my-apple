import { useState } from 'react';
import useEventListener from './use-event-listener';

const useWindowSize = () => {
   const [windowSize, setWindowSize] = useState({
      width: window.innerWidth,
      height: window.innerHeight,
   });

   useEventListener('resize', (event) => {
      setWindowSize({
         width: window.innerWidth,
         height: window.innerHeight,
      });
   });

   console.log('[Hook:useWindowSize] windowSize', windowSize);

   return windowSize;
};

export default useWindowSize;
