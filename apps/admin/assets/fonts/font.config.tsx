import localFont from 'next/font/local';
import { GeistSans } from 'geist/font/sans';
import { GeistMono } from 'geist/font/mono';

// local font
export const SFDisplayFont = localFont({
   src: [
      {
         path: './SFProDisplay/SF-Pro-Display-Thin.otf',
         weight: '100',
         style: 'normal',
      },
      {
         path: './SFProDisplay/SF-Pro-Display-Heavy.otf',
         weight: '200',
         style: 'normal',
      },
      {
         path: './SFProDisplay/SF-Pro-Display-Light.otf',
         weight: '300',
         style: 'normal',
      },
      {
         path: './SFProDisplay/SF-Pro-Display-Regular.otf',
         weight: '400',
         style: 'normal',
      },
      {
         path: './SFProDisplay/SF-Pro-Display-Medium.otf',
         weight: '500',
         style: 'normal',
      },
      {
         path: './SFProDisplay/SF-Pro-Display-Semibold.otf',
         weight: '600',
         style: 'normal',
      },
      {
         path: './SFProDisplay/SF-Pro-Display-Bold.otf',
         weight: '700',
         style: 'normal',
      },
      {
         path: './SFProDisplay/SF-Pro-Display-Black.otf',
         weight: '900',
         style: 'normal',
      },
   ],
   variable: '--font-SFProDisplay',
});

export const SFTextFont = localFont({
   src: [
      {
         path: './SFProText/SF-Pro-Text-Thin.otf',
         weight: '100',
         style: 'normal',
      },
      {
         path: './SFProText/SF-Pro-Text-Heavy.otf',
         weight: '200',
         style: 'normal',
      },
      {
         path: './SFProText/SF-Pro-Text-Light.otf',
         weight: '300',
         style: 'normal',
      },
      {
         path: './SFProText/SF-Pro-Text-Regular.otf',
         weight: '400',
         style: 'normal',
      },
      {
         path: './SFProText/SF-Pro-Text-Medium.otf',
         weight: '500',
         style: 'normal',
      },
      {
         path: './SFProText/SF-Pro-Text-Semibold.otf',
         weight: '600',
         style: 'normal',
      },
      {
         path: './SFProText/SF-Pro-Text-Bold.otf',
         weight: '700',
         style: 'normal',
      },
      {
         path: './SFProText/SF-Pro-Text-Black.otf',
         weight: '900',
         style: 'normal',
      },
   ],
   variable: '--font-SFProText',
});

export const fontSans = GeistSans;
export const fontMono = GeistMono;
