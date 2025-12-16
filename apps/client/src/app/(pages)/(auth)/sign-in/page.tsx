'use client';

import { GoArrowUpRight } from 'react-icons/go';
import Image from 'next/image';
import googlePng from '@assets/images/google.png';
import Link from 'next/link';
import { LoginFormType, LoginResolver } from '~/domain/schemas/auth.schema';
import { FieldInput } from '~/components/client/forms/field-input';
import { LoadingOverlay } from '~/components/client/loading-overlay';
import { useForm } from 'react-hook-form';
import withAuth from '~/components/HoCs/with-auth.hoc';
import useAuthService from '~/hooks/api/use-auth-service';
import { useRememberMe } from '~/hooks/use-remember-me';
import { useEffect, useState } from 'react';

const SignInPage = () => {
   const { loginAsync, isLoading } = useAuthService();
   const {
      saveCredentials,
      getCredentials,
      clearCredentials,
      hasRememberedCredentials,
   } = useRememberMe();
   const [rememberMe, setRememberMe] = useState(false);

   const form = useForm<LoginFormType>({
      resolver: LoginResolver,
   });

   // Load saved credentials on mount
   useEffect(() => {
      if (hasRememberedCredentials()) {
         const credentials = getCredentials();
         if (credentials) {
            form.setValue('email', credentials.email);
            form.setValue('password', credentials.password);
            setRememberMe(true);
         }
      }
   }, [hasRememberedCredentials, getCredentials, form]);

   const onSubmit = (data: LoginFormType) => {
      // Save or clear credentials based on remember me checkbox
      if (rememberMe) {
         saveCredentials(data.email, data.password);
      } else {
         clearCredentials();
      }

      loginAsync(data);
   };

   return (
      <div className="w-full max-w-[1180px] mx-auto px-4 sm:px-6 lg:px-10">
         <section className="flex flex-col sm:flex-row items-center justify-between border-b py-5 gap-2">
            <div>
               <h2 className="text-xl sm:text-2xl font-medium text-center sm:text-left">
                  Account
               </h2>
            </div>
            <div className="flex flex-wrap justify-center sm:justify-end gap-6 font-SFProText text-slate-500 text-xs sm:text-sm">
               <Link
                  href="/sign-in"
                  className="hover:text-blue-500 hover:underline"
               >
                  Sign in
               </Link>
               <Link
                  href="/sign-up"
                  className="hover:text-blue-500 hover:underline"
               >
                  Create your Account
               </Link>
               <a href="#" className="hover:text-blue-500 hover:underline">
                  FAQ
               </a>
            </div>
         </section>

         <div className="min-h-[calc(100vh-160px)] px-4 sm:px-6 lg:px-[80px] py-12 sm:py-16 lg:py-[80px] flex items-center">
            <div className="w-full flex flex-col items-center">
               <h3 className="text-3xl sm:text-4xl font-SFProDisplay font-medium">
                  Account
               </h3>
               <p className="text-sm sm:text-base font-SFProText font-light pt-4 sm:pt-5">
                  Manage Your Account
               </p>

               <p className="text-sm sm:text-base font-SFProText font-light py-1 flex gap-2 flex-wrap sm:flex-nowrap">
                  Not have account yet?{' '}
                  <Link
                     href="/sign-up"
                     className="text-blue-500 flex underline"
                  >
                     Create your account
                     <GoArrowUpRight className="size-4" />
                  </Link>
               </p>

               <div className="w-full max-w-[480px]">
                  <LoadingOverlay isLoading={isLoading} text="Signing in...">
                     <div>
                        <form
                           onSubmit={form.handleSubmit(onSubmit)}
                           onKeyDown={(e) => {
                              if (e.key === 'Enter' && !e.defaultPrevented) {
                                 e.preventDefault();
                                 form.handleSubmit(onSubmit)();
                              }
                           }}
                        >
                           <FieldInput
                              form={form}
                              name="email"
                              label="Email"
                              className="rounded-xl rounded-b-none"
                              type="text"
                              disabled={isLoading}
                              errorTextClassName="pb-5"
                           />

                           <FieldInput
                              form={form}
                              name="password"
                              label="Password"
                              type="password"
                              disabled={isLoading}
                              visibleEyeIcon={true}
                              className="rounded-xl rounded-t-none"
                              fetchingFunc={onSubmit}
                              // hasEnterArrowButton={true}
                           />
                        </form>
                     </div>

                     <div className="mt-3 flex items-center justify-between">
                        <div className="flex items-center space-x-2">
                           <input
                              type="checkbox"
                              id="remember"
                              checked={rememberMe}
                              onChange={(e) => setRememberMe(e.target.checked)}
                              className="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                           />
                           <label
                              htmlFor="remember"
                              className="text-gray-600 text-sm cursor-pointer"
                           >
                              Remember me
                           </label>
                        </div>

                        <div className="flex text-blue-500">
                           <Link
                              href="/forgot-password"
                              className="text-sm font-normal hover:underline"
                           >
                              Forgot password?
                           </Link>
                           <GoArrowUpRight className="size-4" />
                        </div>
                     </div>

                     <button
                        type="button"
                        onClick={form.handleSubmit(onSubmit)}
                        disabled={isLoading}
                        className="w-full mt-5 px-6 py-3 bg-blue-600 text-white font-SFProText text-base font-medium rounded-xl hover:bg-blue-700 active:bg-blue-800 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors duration-200 flex items-center justify-center gap-2"
                     >
                        {isLoading ? (
                           <>
                              <svg
                                 className="animate-spin h-5 w-5 text-white"
                                 xmlns="http://www.w3.org/2000/svg"
                                 fill="none"
                                 viewBox="0 0 24 24"
                              >
                                 <circle
                                    className="opacity-25"
                                    cx="12"
                                    cy="12"
                                    r="10"
                                    stroke="currentColor"
                                    strokeWidth="4"
                                 ></circle>
                                 <path
                                    className="opacity-75"
                                    fill="currentColor"
                                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                                 ></path>
                              </svg>
                              Signing in...
                           </>
                        ) : (
                           'Log in'
                        )}
                     </button>

                     <div className="mt-8">
                        <h3 className="text-lg font-SFProText font-medium text-center">
                           Sign In with another method
                        </h3>

                        <div className="flex flex-col justify-center items-center gap-3 mt-3">
                           <button className="w-full sm:w-fit max-w-[400px] px-4 py-2 rounded-full font-SFProText text-base font-medium sm:min-w-[300px] bg-slate-200 flex items-center justify-between hover:bg-slate-300 active:bg-slate-200">
                              <Image
                                 src={googlePng}
                                 alt="cover"
                                 width={1000}
                                 height={1000}
                                 quality={100}
                                 className="h-8 w-8"
                              />
                              Sign in with Google
                              <Image
                                 src={googlePng}
                                 alt="cover"
                                 width={1000}
                                 height={1000}
                                 quality={100}
                                 className="h-8 w-8 invisible"
                              />
                           </button>
                        </div>
                     </div>
                  </LoadingOverlay>
               </div>
            </div>
         </div>
      </div>
   );
};

export default withAuth(SignInPage);
