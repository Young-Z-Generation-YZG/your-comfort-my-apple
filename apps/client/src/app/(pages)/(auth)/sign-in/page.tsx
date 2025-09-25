'use client';

import { GoArrowUpRight } from 'react-icons/go';
import Image from 'next/image';
import googlePng from '@assets/images/google.png';
import Link from 'next/link';
import { LoginFormType, LoginResolver } from '~/domain/schemas/auth.schema';
import { FieldInput } from '@components/client/forms/field-input';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { useEffect, useState } from 'react';
import { useLoginAsyncMutation } from '~/infrastructure/services/auth.service';
import { useRouter } from 'next/navigation';
import { VERIFICATION_TYPES } from '~/domain/enums/verification-type.enum';
import { useToast } from '~/hooks/use-toast';
import { useForm } from 'react-hook-form';
import withAuth from '@components/HoCs/with-auth.hoc';
import isServerErrorResponse from '~/infrastructure/utils/http/is-server-error';

const defaultValues: LoginFormType = {
   email: '',
   password: '',
};

const SignInPage = () => {
   const [isLoading, setIsLoading] = useState(false);
   const router = useRouter();

   const { toast } = useToast();

   const form = useForm<LoginFormType>({
      resolver: LoginResolver,
      defaultValues: defaultValues,
   });

   const [
      loginAsync,
      {
         isLoading: isFetching,
         data: dataAsync,
         isSuccess,
         error: errorAsync,
         isError,
         reset,
      },
   ] = useLoginAsyncMutation();

   const onSubmit = async (data: LoginFormType) => {
      await loginAsync(data).unwrap();
   };

   useEffect(() => {
      if (typeof dataAsync === 'object' && dataAsync !== null) {
         if (
            dataAsync.verification_type ===
            VERIFICATION_TYPES.EMAIL_VERIFICATION
         ) {
            const queryParams = new URLSearchParams();

            if (dataAsync.params) {
               for (const [key, value] of Object.entries(dataAsync.params)) {
                  queryParams.append(key, value as string);
               }
            }

            router.replace(`/verify/otp?${queryParams}`);
         } else {
            router.replace('/account');
         }
      }
   }, [isSuccess, dataAsync]);

   useEffect(() => {
      if (isServerErrorResponse(errorAsync)) {
         if (errorAsync?.data?.error?.message) {
            toast({
               variant: 'destructive',
               title: `${errorAsync.data.error.message ?? 'Server busy, please try again later'}`,
               description: `Wrong email or password`,
            });
         } else {
            toast({
               variant: 'destructive',
               title: `Server busy, please try again later`,
            });
         }
      }
   }, [isError]);

   useEffect(() => {
      setIsLoading(isFetching);
   }, [isFetching]);

   return (
      <div className="max-w-[1180px] mx-auto px-10">
         <section className="flex justify-between border-b h-[60px] items-center">
            <div>
               <h2 className="text-2xl font-medium">Account</h2>
            </div>

            <div className="flex lg:gap-3 gap-7 font-SFProText text-slate-500 lg:text-sm md:text-xl text-2xl">
               <Link href="/sign-in">Sign in</Link>
               <Link href="/sign-up">Create your Account</Link>
               <a href="#">FAQ</a>
            </div>
         </section>

         <div className="h-full lg:px-[200px] md:px-[100px] px-0 py-[200px]">
            <div className="w-full flex flex-col justify-center items-center">
               <h3 className="font-SFProDisplay font-medium lg:text-3xl md:text-5xl text-7xl">
                  Account
               </h3>
               <p className="font-SFProText font-light pt-5 lg:text-base md:text-xl text-2xl">
                  Manage Your Account
               </p>
               <p className="font-SFProText font-light pt-5 flex gap-2 lg:text-base md:text-xl text-2xl lg:pb-2 md:pb-5 pb-10">
                  Not have account yet?{' '}
                  <Link
                     href="/sign-up"
                     className="text-blue-500 flex underline"
                  >
                     Create your account
                     <GoArrowUpRight className="size-4" />
                  </Link>
               </p>

               <div className="lg:w-[500px] md:w-[80%] w-full">
                  <LoadingOverlay
                     isLoading={isLoading || isFetching}
                     text="Signing in..."
                  >
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
                              type="text"
                              disabled={isLoading || isFetching}
                              className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl rounded-b-none"
                              labelClassName="lg:text-base md:text-xl text-3xl"
                              errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
                           />

                           <FieldInput
                              form={form}
                              name="password"
                              label="Password"
                              type="password"
                              disabled={isLoading || isFetching}
                              fetchingFunc={onSubmit}
                              visibleEyeIcon={false}
                              hasEnterArrowButton={true}
                              className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl rounded-t-none"
                              labelClassName="lg:text-base md:text-xl text-3xl"
                              errorTextClassName="lg:text-sm md:text-lg text-2xl"
                           />
                        </form>
                     </div>

                     <div className="mt-3 ml-auto w-fit">
                        <div className="flex items-center justify-center space-x-2 lg:mt-0 mt-8 lg:mb-0 mb-4">
                           <input
                              type="checkbox"
                              id="remember"
                              className="lg:h-4 md:h-6 h-8 lg:w-4 md:w-6 w-8 rounded border-gray-300 text-blue-600 focus:ring-blue-500 "
                           />
                           <label
                              htmlFor="remember"
                              className="text-gray-600 lg:text-base md:text-xl text-2xl"
                           >
                              Remember me
                           </label>
                        </div>

                        <div className="mt-2 flex text-blue-500">
                           <Link
                              href="/forgot-password"
                              className="lg:text-base md:text-xl text-2xl font-normal text-end w-full block hover:underline"
                           >
                              Forgot password?
                           </Link>
                           <GoArrowUpRight className="size-4" />
                        </div>
                     </div>

                     <div className="lg:mt-5 mt-10">
                        <h3 className="lg:text-base md:text-xl text-2xl font-SFProText font-medium text-center">
                           Sign In with another method
                        </h3>

                        <div className="flex flex-col justify-center items-center gap-3 mt-3">
                           <button className="w-fit px-3 py-2 rounded-full font-SFProText lg:text-base md:text-xl text-3xl font-medium min-w-[300px] bg-slate-100 flex items-center justify-between hover:bg-slate-200 active:bg-slate-100">
                              <Image
                                 src={googlePng}
                                 alt="cover"
                                 width={1000}
                                 height={1000}
                                 quality={100}
                                 className="lg:h-8 md:h-12 h-20 lg:w-8 md:w-12 w-20"
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
