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
import { redirectToIdentityProvider } from '~/src/infrastructure/identity-server/keycloak';
import { useEffect, useRef, useState } from 'react';
import { useRouter } from 'next/navigation';
import { toast } from '~/src/hooks/use-toast';
import useMediaQuery from '~/src/hooks/use-media-query';
import { cn } from '~/src/infrastructure/lib/utils';
import { setLogin } from '~/src/infrastructure/redux/features/auth.slice';

const SignInPage = () => {
   const { isLoading, loginAsync, getIdentityAsync } = useAuthService();

   const dispatch = useDispatch();
   const router = useRouter();
   const loginWindowRef = useRef<Window | null>(null);
   const [isKeycloakLoading, setIsKeycloakLoading] = useState(false);

   const {
      isMobile,
      isTablet,
      isDesktop,
      isLargeDesktop,
      isExtraLargeDesktop,
   } = useMediaQuery();

   console.log('Responsive:', {
      isMobile,
      isTablet,
      isDesktop,
      isLargeDesktop,
      isExtraLargeDesktop,
   });

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

   // Listen for storage changes from callback window
   useEffect(() => {
      const handleStorageChange = (e: StorageEvent) => {
         // Listen for changes to redux-persist storage
         if (e.key === 'persist:admin-root' && e.newValue) {
            console.log('Storage changed from callback window');
            // The AUTH_SUCCESS message handler will manually sync the state
         }
      };

      window.addEventListener('storage', handleStorageChange);

      return () => {
         window.removeEventListener('storage', handleStorageChange);
      };
   }, []);

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
               console.log('Authentication successful, syncing state...');
               setIsKeycloakLoading(false);

               toast({
                  title: 'Authentication successful',
                  description: 'Welcome back!',
               });

               // Manually sync Redux state from localStorage
               // The callback window saved auth state to localStorage,
               // we need to manually read it and update Redux
               const syncAndNavigate = async () => {
                  try {
                     console.log('Reading auth state from localStorage...');

                     // Wait a bit for localStorage to be written by callback window
                     await new Promise((resolve) => setTimeout(resolve, 300));

                     // Read persisted state from localStorage
                     const persistKey = 'persist:admin-root';
                     const stored = localStorage.getItem(persistKey);

                     if (stored) {
                        try {
                           const parsedState = JSON.parse(stored);
                           const authStateStr = parsedState.auth;

                           if (authStateStr) {
                              const authState = JSON.parse(authStateStr);
                              console.log(
                                 'Found auth state in localStorage:',
                                 authState,
                              );

                              // Manually dispatch setLogin to update Redux state
                              if (authState.currentUser?.accessToken) {
                                 console.log(
                                    'Dispatching setLogin with stored auth state...',
                                 );
                                 dispatch(
                                    setLogin({
                                       currentUser: authState.currentUser,
                                    }),
                                 );

                                 // Wait a moment for Redux to update
                                 await new Promise((resolve) =>
                                    setTimeout(resolve, 200),
                                 );

                                 // Now fetch identity to get tenant/roles
                                 console.log(
                                    'Fetching identity to complete sync...',
                                 );
                                 const identityResult =
                                    await getIdentityAsync();

                                 if (identityResult.isSuccess) {
                                    console.log(
                                       'Identity synced, navigating to dashboard...',
                                    );
                                    // Use replace to avoid adding to history
                                    router.replace('/dashboard');
                                 } else {
                                    console.log(
                                       'Identity fetch failed, navigating anyway...',
                                    );
                                    router.replace('/dashboard');
                                 }
                              } else {
                                 console.log('No access token in stored state');
                                 // Fallback: try to fetch identity anyway
                                 const identityResult =
                                    await getIdentityAsync();
                                 if (identityResult.isSuccess) {
                                    router.replace('/dashboard');
                                 } else {
                                    // Last resort: reload page to trigger full rehydration
                                    window.location.href = '/dashboard';
                                 }
                              }
                           } else {
                              console.log('No auth state in stored data');
                              // Fallback: reload page
                              window.location.href = '/dashboard';
                           }
                        } catch (parseErr) {
                           console.error(
                              'Error parsing stored state:',
                              parseErr,
                           );
                           // Fallback: reload page
                           window.location.href = '/dashboard';
                        }
                     } else {
                        console.log('No stored state found, reloading page...');
                        // No stored state - reload to trigger rehydration
                        window.location.href = '/dashboard';
                     }
                  } catch (err) {
                     console.error('Error syncing auth state:', err);
                     // Fallback: reload page
                     window.location.href = '/dashboard';
                  }
               };

               syncAndNavigate();
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

   return (
      <div className={cn('flex bg-white min-h-screen')}>
         <LoadingOverlay
            isLoading={isLoading || isKeycloakLoading}
            fullScreen
         />
         {/* Left side - Brand/Cover section - Hidden on mobile & tablet */}
         {!isMobile && !isTablet && (
            <div
               className={cn(
                  'bg-[#ebebeb] relative h-screen flex items-center justify-center overflow-hidden',
                  {
                     'w-[70%]': isTablet,
                     'w-[75%]': !isTablet,
                  },
               )}
            >
               <div className="w-full flex items-center justify-center absolute top-3">
                  <Image
                     src={'/images/logo.svg'}
                     alt="cover"
                     width={1000}
                     height={1000}
                     quality={100}
                     className={cn('h-[100px] object-contain', {
                        'w-[500px]':
                           isDesktop || isLargeDesktop || isExtraLargeDesktop,
                        'w-[350px]': isTablet,
                     })}
                  />
               </div>

               <div className="w-full flex items-center justify-center absolute top-10">
                  <Image
                     src={'/images/products.svg'}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className={cn('object-contain', {
                        'w-[700px] h-[600px]':
                           isDesktop || isLargeDesktop || isExtraLargeDesktop,
                        'w-[500px] h-[450px]': isTablet,
                     })}
                  />
               </div>

               <div className="w-full flex items-center justify-center absolute -bottom-[500px]">
                  <Image
                     src={'/images/ip-conver.svg'}
                     alt="cover"
                     width={1200}
                     height={1000}
                     quality={100}
                     className={cn('object-contain', {
                        'w-[1000px] h-[900px]':
                           isDesktop || isLargeDesktop || isExtraLargeDesktop,
                        'w-[800px] h-[700px]': isTablet,
                     })}
                  />
               </div>

               <div
                  className={cn(
                     'flex absolute bottom-5 w-full justify-between px-5',
                     {
                        'px-10': isTablet,
                        'px-12':
                           isDesktop || isLargeDesktop || isExtraLargeDesktop,
                     },
                  )}
               >
                  <div className="flex flex-col">
                     <p
                        className={cn('text-black font-mono font-bold', {
                           'text-base': isTablet || isDesktop,
                           'text-lg': isLargeDesktop || isExtraLargeDesktop,
                        })}
                     >
                        David Bach Bale
                     </p>
                     <p
                        className={cn('text-black', {
                           'text-sm': isTablet || isDesktop,
                           'text-base': isLargeDesktop || isExtraLargeDesktop,
                        })}
                     >
                        Founder & CEO
                     </p>
                  </div>

                  <div className="flex flex-col text-right">
                     <p
                        className={cn('text-black font-mono font-bold', {
                           'text-base': isTablet || isDesktop,
                           'text-lg': isLargeDesktop || isExtraLargeDesktop,
                        })}
                     >
                        www.astoreyb.com
                     </p>
                     <p
                        className={cn('text-black', {
                           'text-sm': isTablet || isDesktop,
                           'text-base': isLargeDesktop || isExtraLargeDesktop,
                        })}
                     >
                        hello@astoreyb.com
                     </p>
                  </div>
               </div>
            </div>
         )}

         {/* Right side - Login form */}
         <div
            className={cn(
               'bg-slate-50 min-h-screen flex items-center justify-center',
               {
                  'w-full': isMobile || isTablet,
                  'w-[30%]': isDesktop,
                  'w-[25%]': isLargeDesktop || isExtraLargeDesktop,
               },
            )}
         >
            <div
               className={cn(
                  'w-full h-full flex justify-center items-center flex-col',
                  {
                     'px-6 py-8': isMobile,
                     'px-8 py-6': isTablet,
                     'px-6 py-6': !isMobile && !isTablet,
                  },
               )}
            >
               <div
                  className={cn('w-full', {
                     'max-w-md': isMobile,
                     'max-w-sm': isTablet,
                  })}
               >
                  {/* Logo - Show on mobile since left side is hidden */}
                  {isMobile && (
                     <div className="flex justify-center mb-8">
                        <Image
                           src={'/images/logo.svg'}
                           alt="cover"
                           width={1000}
                           height={1000}
                           quality={100}
                           className="w-[250px] h-[80px] object-contain"
                        />
                     </div>
                  )}

                  {!isMobile && (
                     <div className="flex justify-center mb-6">
                        <Image
                           src={'/images/logo.svg'}
                           alt="cover"
                           width={1000}
                           height={1000}
                           quality={100}
                           className={cn('object-contain', {
                              'w-[280px] h-[90px]': isTablet,
                              'w-[300px] h-[100px]': !isTablet,
                           })}
                        />
                     </div>
                  )}

                  {/* Tablet-only cover image above form inputs */}
                  {isTablet && !isMobile && (
                     <div className="flex justify-center mb-6">
                        <Image
                           src={'/images/products.svg'}
                           alt="cover"
                           width={800}
                           height={600}
                           quality={100}
                           className="w-full max-w-md h-auto object-contain"
                        />
                     </div>
                  )}

                  <div className="w-full">
                     <Form {...form}>
                        <form
                           onSubmit={form.handleSubmit((data: TLoginForm) => {
                              loginAsync(data);
                           })}
                           className={cn('flex flex-col', {
                              'gap-5': isTablet || isDesktop,
                              'gap-4': isMobile,
                           })}
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
                              className={cn(
                                 'bg-primary font-sans font-bold w-full mt-5',
                                 {
                                    'h-11': isMobile,
                                    'h-12': !isMobile,
                                 },
                              )}
                              type="submit"
                              disabled={isLoading}
                           >
                              Login
                           </Button>
                        </form>
                     </Form>
                  </div>

                  <p
                     className={cn(
                        'text-black font-mono font-bold text-center mt-10',
                        {
                           'py-5 text-base': isTablet || isDesktop,
                           'py-4 text-sm': isMobile,
                        },
                     )}
                  >
                     Or login with
                  </p>

                  <div className="flex flex-col gap-3 w-full mt-5">
                     <Button
                        className={cn(
                           'w-full font-semibold flex items-center justify-center gap-2',
                           {
                              'h-11': isMobile,
                              'h-12': !isMobile,
                           },
                        )}
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

                     <Button
                        className={cn('w-full font-semibold', {
                           'h-11': isMobile,
                           'h-12': !isMobile,
                        })}
                        variant="outline"
                     >
                        <span>Login with Google</span>
                     </Button>
                  </div>
               </div>
            </div>
         </div>
      </div>
   );
};

export default withAuth(SignInPage);
