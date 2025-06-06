/* eslint-disable react/react-in-jsx-scope */
import type { Metadata } from 'next';
import '/globals.css';
import { SFDisplayFont, SFTextFont } from '@assets/fonts/font.config';
import { cn } from '~/infrastructure/lib/utils';
import { ReduxProvider } from '~/infrastructure/redux/provider';
import Header from '@components/layouts/Header';
import Footer from '@components/layouts/Footer';
import { LoadingProvider } from '@components/contexts/loading.context';
import { Toaster } from '@components/ui/toaster';
import { Toaster as SonnerToaster } from '@components/ui/sonner';

export const metadata: Metadata = {
   title: 'YB Store',
   description: 'Generated by create next app',
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
      <html lang="en">
         <body
            className={cn(
               SFDisplayFont.variable,
               SFTextFont.variable,
               'font-SFProDisplay',
            )}
         >
            <main>
               <LoadingProvider>
                  <ReduxProvider>
                     <Header />
                     {children}
                     <Footer />
                  </ReduxProvider>
               </LoadingProvider>
            </main>
            <Toaster />
            <SonnerToaster />
         </body>
      </html>
   );
}
