'use client';

import { LoadingOverlay } from '@components/loading-overlay';
import { Button } from '@components/ui/button';
import Image from 'next/image';
import { useRouter } from 'next/navigation';
import { useState, useEffect } from 'react';
import { toast } from 'sonner';
import { redirectToIdentityProvider } from '~/src/infrastructure/identity-server/keycloak';

let loginWindow: Window | null = null;

const SignInPage = () => {
   const [isLoading, setIsLoading] = useState(false);
   const [authError, setAuthError] = useState<string | null>(null);
   const router = useRouter();

   // Setup message event listener
   useEffect(() => {
      const handleAuthMessages = (event: MessageEvent) => {
         // Verify the origin of the message for security
         if (event.origin !== window.location.origin) return;

         // Handle different message types
         if (typeof event.data === 'object' && event.data !== null) {
            const { status, error } = event.data;

            if (status === 'AUTH_SUCCESS') {
               setIsLoading(false);
               toast.success('Authentication successful');
               // Redirect to dashboard or home page
               router.push('/dashboard');
            } else if (status === 'AUTH_FAILED') {
               setIsLoading(false);
               setAuthError(error || 'Authentication failed');
               toast.error(`Login failed: ${error || 'Unknown error'}`);
            }
         }
         // Handle the old format for backwards compatibility
         else if (event.data === 'AUTH_COMPLETED') {
            setIsLoading(false);
         }
      };

      // Add the event listener
      window.addEventListener('message', handleAuthMessages);

      // Clean up the event listener when component unmounts
      return () => {
         window.removeEventListener('message', handleAuthMessages);
      };
   }, [router]);

   const handleLogin = () => {
      const identityUrl = redirectToIdentityProvider();
      setIsLoading(true);
      setAuthError(null);

      loginWindow = window.open(
         identityUrl,
         '_blank',
         'width=800,height=600,top=100,left=100',
      );

      // Fallback for when the popup is closed manually
      const interval = setInterval(() => {
         if (loginWindow && loginWindow.closed) {
            clearInterval(interval);
            setIsLoading(false);
         }
      }, 500);
   };

   return (
      <div className="bg-white flex">
         <LoadingOverlay isLoading={isLoading} fullScreen />
         <div className="bg-[#ebebeb] relative h-screen flex items-center justify-center w-[75%] overflow-hidden">
            <div className="w-full flex items-center justify-center absolute top-3">
               <Image
                  src={'/images/logo.svg'}
                  alt="cover"
                  width={1000}
                  height={1000}
                  quality={100}
                  className="w-[500px] h-[100px]"
               />
            </div>

            <div className="w-full flex items-center justify-center absolute top-10">
               <Image
                  src={'/images/products.svg'}
                  alt="cover"
                  width={1200}
                  height={1000}
                  quality={100}
                  className="w-[700px] h-[600px]"
               />
            </div>

            <div className="w-full flex items-center justify-center absolute -bottom-[500px]">
               <Image
                  src={'/images/ip-conver.svg'}
                  alt="cover"
                  width={1200}
                  height={1000}
                  quality={100}
                  className="w-[1000px] h-[900px]"
               />
            </div>

            <div className="flex absolute bottom-5 w-full justify-between px-10">
               <div className="flex flex-col">
                  <p className="text-black font-mono font-bold">
                     David Bach Bale
                  </p>
                  <p className="text-black">Founder & CEO</p>
               </div>

               <div className="flex flex-col">
                  <p className="text-black font-mono font-bold">
                     www.astoreyb.com
                  </p>
                  <p className="text-black">hello@astoreyb.com</p>
               </div>
            </div>
         </div>

         <div className="bg-slate-50 w-[25%]">
            {' '}
            <div className="w-full h-full flex justify-center items-center flex-col">
               {authError && (
                  <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4 max-w-md">
                     <p className="text-sm">{authError}</p>
                  </div>
               )}
               <Button
                  className="bg-primary font-sans font-bold py-6"
                  onClick={() => {
                     handleLogin();
                  }}
               >
                  Login | centralized authentication system
               </Button>
            </div>
         </div>
      </div>
   );
};

export default SignInPage;
