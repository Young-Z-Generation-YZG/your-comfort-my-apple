'use client';

import { LoadingOverlay } from '@components/loading-overlay';
import { Button } from '@components/ui/button';
import { Form } from '@components/ui/form';
import Image from 'next/image';
import { useForm } from 'react-hook-form';
import { loginResolver, TLoginForm } from '~/src/domain/schemas/auth.schema';
import svgs from '@assets/svgs';
import withAuth from '@components/HoCs/with-auth.hoc';
import useAuthService from '~/src/hooks/api/use-auth-service';
import Input from '@components/customs/input-field';
import PasswordInputField from '@components/customs/password-input-field';
import FieldMessageError from '@components/customs/field-message-error';
import Label from '@components/customs/label';
import { useDispatch } from 'react-redux';
import { setRoles } from '~/src/infrastructure/redux/features/auth.slice';
import { redirectToIdentityProvider } from '~/src/infrastructure/identity-server/keycloak';
import { useEffect, useRef, useState } from 'react';
import { useRouter } from 'next/navigation';
import { toast } from '~/src/hooks/use-toast';

const fakeIdentityData = {
   id: '65dad719-7368-4d9f-b623-f308299e9575',
   username: 'admin@gmail.com',
   email: 'admin@gmail.com',
   email_confirmed: true,
   phone_number: '0333284890',
   roles: ['ADMIN'],
};

export type TIdentity = typeof fakeIdentityData;

const SignInPage = () => {
   const { isLoading, loginAsync, getIdentityAsync } = useAuthService();

   const dispatch = useDispatch();
   const router = useRouter();
   const loginWindowRef = useRef<Window | null>(null);
   const [isKeycloakLoading, setIsKeycloakLoading] = useState(false);

   const form = useForm({
      resolver: loginResolver,
      defaultValues: {
         email: '',
         password: '',
      },
   });

   const handleKeycloakLogin = () => {
      const identityUrl = redirectToIdentityProvider();
      setIsKeycloakLoading(true);

      // Open popup window
      loginWindowRef.current = window.open(
         identityUrl,
         'keycloak-login',
         'width=800,height=600,top=100,left=100',
      );

      // Check if popup was blocked
      if (!loginWindowRef.current || loginWindowRef.current.closed) {
         setIsKeycloakLoading(false);
         toast({
            variant: 'destructive',
            title: 'Popup blocked',
            description: 'Please allow popups for this site to continue.',
         });
         return;
      }

      // Monitor popup window closure
      const checkClosed = setInterval(() => {
         if (loginWindowRef.current?.closed) {
            clearInterval(checkClosed);
            setIsKeycloakLoading(false);
         }
      }, 500);
   };

   // Setup message event listener for popup communication
   useEffect(() => {
      const handleAuthMessages = (event: MessageEvent) => {
         console.log(
            'Message received:',
            event.data,
            'from origin:',
            event.origin,
         );

         // Verify the origin of the message for security
         if (event.origin !== window.location.origin) {
            console.log(
               'Origin mismatch:',
               event.origin,
               'expected:',
               window.location.origin,
            );
            return;
         }

         // Handle different message types
         if (typeof event.data === 'object' && event.data !== null) {
            const { status } = event.data;
            console.log('Auth status:', status);

            if (status === 'AUTH_SUCCESS') {
               console.log('Authentication successful, navigating...');
               setIsKeycloakLoading(false);

               toast({
                  title: 'Authentication successful',
                  description: 'Welcome back!',
               });

               // Fetch identity to get roles, then navigate
               getIdentityAsync()
                  .then((identityResult) => {
                     if (identityResult.isSuccess && identityResult.data) {
                        dispatch(
                           setRoles({
                              currentUser: {
                                 roles: identityResult.data.roles,
                              },
                              impersonatedUser: {
                                 roles: null,
                              },
                           }),
                        );
                     }
                  })
                  .finally(() => {
                     // Navigate to revenue analytics dashboard after identity fetch
                     console.log('Navigating to /dashboard/revenue-analytics');
                     window.location.href = '/dashboard/revenue-analytics';
                  });
            } else if (status === 'AUTH_FAILED') {
               setIsKeycloakLoading(false);
               toast({
                  variant: 'destructive',
                  title: 'Authentication failed',
                  description: 'Please try again.',
               });
            }
         }
      };

      // Add the event listener
      window.addEventListener('message', handleAuthMessages);

      // Clean up the event listener when component unmounts
      return () => {
         window.removeEventListener('message', handleAuthMessages);
         if (loginWindowRef.current && !loginWindowRef.current.closed) {
            loginWindowRef.current.close();
         }
      };
   }, [dispatch, getIdentityAsync, router]);

   //    useEffect(() => {
   //       setIsLoading(isLoginLoading);
   //    }, [isLoginLoading]);

   //    // Setup message event listener
   //    useEffect(() => {
   //       const handleAuthMessages = (event: MessageEvent) => {
   //          // Verify the origin of the message for security
   //          if (event.origin !== window.location.origin) return;

   //          // Handle different message types
   //          if (typeof event.data === 'object' && event.data !== null) {
   //             const { status, error } = event.data;

   //             if (status === 'AUTH_SUCCESS') {
   //                setIsLoading(false);

   //                sonnerToast.success('Welcome back!', {
   //                   style: {
   //                      backgroundColor: '#4CAF50', // Custom green background color
   //                      color: '#FFFFFF', // White text color
   //                   },
   //                });

   //                setTimeout(() => {
   //                   window.location.href = '/dashboard';
   //                }, 1000);
   //             } else if (status === 'AUTH_FAILED') {
   //                setTimeout(() => {
   //                   setIsLoading(false);

   //                   toast({
   //                      variant: 'destructive',
   //                      title: `Authentication failed`,
   //                   });
   //                }, 500);
   //             }
   //          }
   //       };

   //       // Add the event listener
   //       window.addEventListener('message', handleAuthMessages);

   //       // Clean up the event listener when component unmounts
   //       return () => {
   //          window.removeEventListener('message', handleAuthMessages);
   //       };
   //    }, [router]);

   return (
      <div className="bg-white flex">
         <LoadingOverlay
            isLoading={isLoading || isKeycloakLoading}
            fullScreen
         />
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
                           onSubmit={form.handleSubmit(
                              async (data: TLoginForm) => {
                                 const result = await loginAsync(data);

                                 if (result.isSuccess && result.data) {
                                    const identityResult =
                                       await getIdentityAsync();

                                    if (
                                       identityResult.isSuccess &&
                                       identityResult.data
                                    ) {
                                       dispatch(
                                          setRoles({
                                             currentUser: {
                                                roles: identityResult.data
                                                   .roles,
                                             },
                                             impersonatedUser: {
                                                roles: null,
                                             },
                                          }),
                                       );
                                    }
                                 }
                              },
                           )}
                           className="flex flex-col gap-5"
                        >
                           <div className="flex flex-col gap-2">
                              <Label htmlFor="email">Email</Label>
                              <Input form={form} name="email" />
                              <FieldMessageError form={form} name="email" />
                           </div>

                           <div className="flex flex-col gap-2">
                              <Label htmlFor="password">Password</Label>
                              <PasswordInputField form={form} name="password" />
                              <FieldMessageError form={form} name="password" />
                           </div>

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
                     className="w-full font-semibold flex items-center justify-center gap-2"
                     variant="outline"
                     onClick={handleKeycloakLogin}
                     disabled={isLoading || isKeycloakLoading}
                  >
                     <span>Login with Keycloak</span>
                     <Image
                        src={svgs.keycloak}
                        width={24}
                        height={24}
                        alt="keycloak"
                        className="w-6 h-6"
                     />
                  </Button>
               </div>
            </div>
         </div>
      </div>
   );
};

export default withAuth(SignInPage);
