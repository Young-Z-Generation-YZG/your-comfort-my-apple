'use client';

import { InputField } from '@components/input-field';
import { LoadingOverlay } from '@components/loading-overlay';
import { Button } from '@components/ui/button';
import { Form } from '@components/ui/form';
import Image from 'next/image';
import { useRouter } from 'next/navigation';
import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { loginResolver, TLoginForm } from '~/src/domain/schemas/auth.schema';
import { redirectToIdentityProvider } from '~/src/infrastructure/identity-server/keycloak';
import svgs from '@assets/svgs';
import { useLoginMutation } from '~/src/infrastructure/services/auth.service';
import isServerErrorResponse from '~/src/infrastructure/utils/is-server-error';
import { useToast } from '~/src/hooks/use-toast';
import { toast as sonnerToast } from 'sonner';
import withAuth from '@components/HoCs/with-auth.hoc';

let loginWindow: Window | null = null;

const SignInPage = () => {
   const [isLoading, setIsLoading] = useState(false);
   const router = useRouter();

   const { toast } = useToast();

   const form = useForm({
      resolver: loginResolver,
      defaultValues: {
         email: '',
         password: '',
      },
   });

   const [
      login,
      { isSuccess, isError, error, data, isLoading: isLoginLoading },
   ] = useLoginMutation();

   const handleLogin = async (data: TLoginForm) => {
      console.log('data', data);

      await login(data).unwrap();
   };

   const handleAuthorizationCode = () => {
      const identityUrl = redirectToIdentityProvider();

      setIsLoading(true);

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

   useEffect(() => {
      if (isSuccess) {
         sonnerToast.success('Welcome back!', {
            style: {
               backgroundColor: '#4CAF50', // Custom green background color
               color: '#FFFFFF', // White text color
            },
         });

         window.location.href = '/dashboards';
      } else if (isError && isServerErrorResponse(error)) {
         if (error?.data?.error?.message) {
            toast({
               variant: 'destructive',
               title: `${error.data.error.message ?? 'Server busy, please try again later'}`,
               description: `Wrong email or password`,
            });
         } else {
            toast({
               variant: 'destructive',
               title: `Server busy, please try again later`,
            });
         }
      }
   }, [isSuccess, isError, error]);

   useEffect(() => {
      setIsLoading(isLoginLoading);
   }, [isLoginLoading]);

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

               sonnerToast.success('Welcome back!', {
                  style: {
                     backgroundColor: '#4CAF50', // Custom green background color
                     color: '#FFFFFF', // White text color
                  },
               });

               setTimeout(() => {
                  window.location.href = '/dashboards';
               }, 1000);
            } else if (status === 'AUTH_FAILED') {
               setTimeout(() => {
                  setIsLoading(false);

                  toast({
                     variant: 'destructive',
                     title: `Authentication failed`,
                  });
               }, 500);
            }
         }
      };

      // Add the event listener
      window.addEventListener('message', handleAuthMessages);

      // Clean up the event listener when component unmounts
      return () => {
         window.removeEventListener('message', handleAuthMessages);
      };
   }, [router]);

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
            <div className="w-full h-full flex justify-center items-center flex-col">
               <div>
                  <Image
                     src={'/images/logo.svg'}
                     alt="cover"
                     width={1000}
                     height={1000}
                     quality={100}
                     className="w-[300px] h-[100px]"
                  />

                  <div className="">
                     <Form {...form}>
                        <form
                           onSubmit={form.handleSubmit(handleLogin)}
                           className="flex flex-col"
                        >
                           <InputField form={form} name="email" label="Email" />

                           <InputField
                              form={form}
                              name="password"
                              label="Password"
                              type="password"
                           />

                           <Button
                              className="bg-primary font-sans font-bold"
                              type="submit"
                              disabled={isLoading}
                           >
                              Login
                           </Button>
                        </form>
                     </Form>
                  </div>

                  <p className="text-black font-mono font-bold text-center py-5">
                     Or login with
                  </p>

                  <Button
                     className="w-full font-semibold"
                     variant="outline"
                     onClick={() => {
                        handleAuthorizationCode();
                     }}
                     disabled={isLoading}
                  >
                     Authorization
                     <Image
                        src={svgs.keycloak}
                        width={100}
                        height={100}
                        alt="keycloak"
                     />
                  </Button>
               </div>
            </div>
         </div>
      </div>
   );
};

export default withAuth(SignInPage);
