import { JetBrains_Mono } from 'next/font/google';
import type { Config } from 'tailwindcss';

const config: Config = {
   darkMode: ['class'],
   content: [
      './pages/**/*.{ts,tsx}',
      './components/**/*.{ts,tsx}',
      './app/**/*.{ts,tsx}',
      './src/**/*.{ts,tsx}',
   ],
   theme: {
      container: {
         center: true,
         padding: '',
         screens: {
            sm: '640px',
            md: '768px',
            lg: '960px',
            xl: '1200px',
         },
      },
      fontFamily: {
         SFProDisplay: 'var(--font-SFProDisplay)',
         SFProText: 'var(--font-SFProText)',
      },
      extend: {
         fontFamily: {
            SFProDisplay: ['var(--font-SFProDisplay)', 'SFProDisplay'],
            SFProText: ['var(--font-SFProText)', 'SFProText'],
         },
      },
   },
   plugins: [require('tailwindcss-animate')],
};

export default config;
