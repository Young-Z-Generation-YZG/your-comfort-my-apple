import type { Metadata } from 'next';
import '~/globals.css';
import {
   fontMono,
   fontSans,
   SFDisplayFont,
   SFTextFont,
} from '~/assets/fonts/font.config';
import { cn } from '~/src/infrastructure/lib/utils';
import { ReduxProvider } from '~/src/infrastructure/redux/provider';
import { ThemeProvider } from '@components/ui/theme-provider';
import { Toaster } from '@components/ui/toaster';
import { Toaster as SonnerToaster } from '@components/ui/sonner';
import OrderNotificationListener from '~/src/components/order-notification-listener';

export const metadata: Metadata = {
   title: 'YB Store Admin Portal',
   description: 'Administrative portal for TLCN services',
   icons: {
      icon: '/images/favicon.ico',
   },
};

export default function RootLayout({
   children,
}: Readonly<{
   children: React.ReactNode;
}>) {
   return (
      <html lang="en" suppressHydrationWarning>
         <body
            className={cn(
               'antialiased',
               fontSans.variable,
               fontMono.variable,
               SFDisplayFont.variable,
               SFTextFont.variable,
            )}
            suppressHydrationWarning
         >
            <main>
               <ReduxProvider>
                  <ThemeProvider
                     attribute="class"
                     defaultTheme="system"
                     enableSystem
                     disableTransitionOnChange
                  >
                     <main>
                        <Toaster />
                        {/* <OrderNotificationListener /> */}
                        {children}
                     </main>
                  </ThemeProvider>
               </ReduxProvider>
            </main>
            <Toaster />
            <SonnerToaster />
         </body>
      </html>
   );
}
