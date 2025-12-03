// ref: `https://www.browserstack.com/guide/responsive-design-breakpoints`

import { useEffect, useState } from 'react';
import useWindowSize from './use-window-size';

// const EXTRA_SMALL_MOBILE_PORTRAIT = 320; // w320px - h480px
// const SMALL_MOBILE_LANDSCAPE = 481; // w481px - h600px
// const SMALL_TABLET_PORTRAIT = 601; // w601px - h768px
// const LARGE_TABLET_LANDSCAPE = 769; // w769px - h1024px
// const SMALL_DESKTOP_AND_LAPTOPS = 1025; // w1025px - h1280px
// const LARGE_DESKTOP_AND_HIGH_RESOLUTION_SCREENS = 1281; // w1281px - h1440px
// const EXTRA_LARGE_DESKTOPS = 1441; // w1441px and up

// Tailwind CSS breakpoints
const SMALL_TAILWIND = '(max-width: 640px)';
const MEDIUM_TAILWIND = '(min-width: 641px) and (max-width: 1024px)';
const LARGE_TAILWIND = '(min-width: 1025px) and (max-width: 1280px)';
const XL_TAILWIND = '(min-width: 1281px) and (max-width: 1440px)';
const XXL_TAILWIND = '(min-width: 1441px)';

// const IS_MOBILE = '(max-width: 768px)';
// const IS_TABLET = '(min-width: 769px) and (max-width: 1024px)';
// const IS_DESKTOP = '(min-width: 1025px)';
// const IS_LARGE_DESKTOP = '(min-width: 1281px) and (max-width: 1440px)';
// const IS_EXTRA_LARGE_DESKTOP = '(min-width: 1441px)';

const useMediaQuery = () => {
   const [isMobile, setIsMobile] = useState(false);
   const [isTablet, setIsTablet] = useState(false);
   const [isDesktop, setIsDesktop] = useState(false);
   const [isLargeDesktop, setIsLargeDesktop] = useState(false);
   const [isExtraLargeDesktop, setIsExtraLargeDesktop] = useState(false);

   const { width, height } = useWindowSize();

   useEffect(() => {
      const mediaMobile = window.matchMedia(SMALL_TAILWIND);
      setIsMobile(mediaMobile.matches);

      const mediaTablet = window.matchMedia(MEDIUM_TAILWIND);
      setIsTablet(mediaTablet.matches);

      const mediaDesktop = window.matchMedia(LARGE_TAILWIND);
      setIsDesktop(mediaDesktop.matches);

      const mediaLargeDesktop = window.matchMedia(XL_TAILWIND);
      setIsLargeDesktop(mediaLargeDesktop.matches);

      const mediaExtraLargeDesktop = window.matchMedia(XXL_TAILWIND);
      setIsExtraLargeDesktop(mediaExtraLargeDesktop.matches);
   }, [width]);

   console.log('Hook: useMediaQuery', {
      isMobile,
      isTablet,
      isDesktop,
      isLargeDesktop,
      isExtraLargeDesktop,
   });

   console.log('[Hook:useMediaQuery]', {
      isMobile,
      isTablet,
      isDesktop,
      isLargeDesktop,
      isExtraLargeDesktop,
   });

   return {
      isMobile,
      isTablet,
      isDesktop,
      isLargeDesktop,
      isExtraLargeDesktop,
   };
};

export default useMediaQuery;
