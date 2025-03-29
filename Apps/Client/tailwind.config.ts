import type { Config } from 'tailwindcss';

const config: Config = {
   darkMode: ['class'],
   content: ['./src/**/*.{ts,tsx}'],
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
      },
   },
   plugins: [require('tailwindcss-animate')],
};

export default config;
