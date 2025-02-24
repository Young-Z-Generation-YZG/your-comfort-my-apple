'use client';

import Image from 'next/image';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { LoadingOverlay } from '~/components/loading-overlay';
import { Button } from '~/components/ui/button';
import { setAccessToken, setIsLoggingIn } from '~/redux/slices/auth.slice';
import { AppDispatch, useAppSelector } from '~/redux/store';
import { redirectToIdentityProvider } from '~/services/auth.service';
import { authChannel } from '~/utils/broadcast-channel';

let loginWindow: Window | null = null;

const LoginPage = () => {
   const [loading, setLoading] = useState(false);

   const dispatch = useDispatch<AppDispatch>();

   const handleLogin = () => {
      setLoading(true);

      let authUrl = redirectToIdentityProvider(); // Get the authentication URL

      loginWindow = window.open(
         authUrl,
         '_blank',
         'width=800,height=600,top=100,left=100',
      );

      // Polling mechanism to check when the login is complete
      const interval = setInterval(() => {
         if (loginWindow && loginWindow.closed) {
            clearInterval(interval);

            setLoading(false);
         }
      }, 500);
   };

   useEffect(() => {
      authChannel.onmessage = (event) => {
         if (event.data.type === 'AUTH_LOGIN') {
            if (event.data.message.result === 'AUTH_SUCCESS') {
               setLoading(false);

               dispatch(setAccessToken(event.data.message.token));

               setTimeout(() => {
                  dispatch(setIsLoggingIn(false));

                  window.location.href = '/';
               }, 1000);
            }

            if (event.data.message.result === 'AUTH_FAILED') {
               setLoading(false);
            }

            // if (loginWindow) {
            //    setTimeout(() => {
            //       loginWindow?.close();
            //    }, 1000);
            // }
         }
      };
   }, []);

   return (
      <div className="bg-white flex">
         <LoadingOverlay isLoading={loading} textStyles="text-primary" />
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
            <div className="w-full h-full flex justify-center items-center flex-col">
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

export default LoginPage;
