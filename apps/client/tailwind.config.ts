import type { Config } from 'tailwindcss';

const config: Config = {
   darkMode: ['class'],
   content: ['./src/**/*.{ts,tsx}', './apps/**/*.{ts,tsx}'],
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
         colors: {
            background: 'hsl(var(--background))',
            foreground: 'hsl(var(--foreground))',
            card: {
               DEFAULT: 'hsl(var(--card))',
               foreground: 'hsl(var(--card-foreground))',
            },
            popover: {
               DEFAULT: 'hsl(var(--popover))',
               foreground: 'hsl(var(--popover-foreground))',
            },
            primary: {
               DEFAULT: 'hsl(var(--primary))',
               foreground: 'hsl(var(--primary-foreground))',
            },
            secondary: {
               DEFAULT: 'hsl(var(--secondary))',
               foreground: 'hsl(var(--secondary-foreground))',
            },
            muted: {
               DEFAULT: 'hsl(var(--muted))',
               foreground: 'hsl(var(--muted-foreground))',
            },
            accent: {
               DEFAULT: 'hsl(var(--accent))',
               foreground: 'hsl(var(--accent-foreground))',
            },
            destructive: {
               DEFAULT: 'hsl(var(--destructive))',
               foreground: 'hsl(var(--destructive-foreground))',
            },
            border: 'hsl(var(--border))',
            input: 'hsl(var(--input))',
            ring: 'hsl(var(--ring))',
         },
         boxShadow: {
            'color-selector': 'inset 0 3px 4px rgba(0,0,0,.25)',
         },
         keyframes: {
            shimmer: {
               '0%': { transform: 'translateX(-100%)' },
               '100%': { transform: 'translateX(100%)' },
            },
            shine: {
               '0%': { transform: 'translateX(-100%)' },
               '100%': { transform: 'translateX(200%)' },
            },
            'gradient-x': {
               '0%, 100%': {
                  'background-size': '200% 200%',
                  'background-position': 'left center',
               },
               '50%': {
                  'background-size': '200% 200%',
                  'background-position': 'right center',
               },
            },
         },
         animation: {
            shimmer: 'shimmer 3s ease-in-out infinite',
            shine: 'shine 2s ease-in-out infinite',
            'gradient-x': 'gradient-x 3s ease infinite',
         },
      },
   },
   plugins: [require('tailwindcss-animate')],
};

export default config;
