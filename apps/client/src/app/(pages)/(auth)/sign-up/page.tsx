'use client';

import Link from 'next/link';
import { GoArrowUpRight } from 'react-icons/go';

import Image from 'next/image';
import signUpImage from '@assets/images/sign-up.png';
import Button from '~/app/(pages)/(auth)/_components/button';
import { FieldInput } from '~/components/client/forms/field-input';
import {
   RegisterFormType,
   RegisterResolver,
} from '~/domain/schemas/auth.schema';
import { FormSelector } from '~/app/(pages)/(auth)/_components/selector-input';
import { FormBirthDateSelector } from '~/components/client/forms/birthday-selector';
import { FormPhoneInput } from '~/components/client/forms/phone-input';
import { LoadingOverlay } from '~/components/client/loading-overlay';
import { Separator } from '~/components/ui/separator';
import { useForm } from 'react-hook-form';
import withAuth from '~/components/HoCs/with-auth.hoc';
import useAuth from '~/hooks/api/use-auth-service';
import { countries } from '~/domain/constants/countries';

const SignUpPage = () => {
   const form = useForm<RegisterFormType>({
      resolver: RegisterResolver,
   });

   const { registerAsync, isLoading } = useAuth();

   const handleSubmitRegister = (data: RegisterFormType) => {
      registerAsync({
         ...data,
         birth_date: new Date(
            data.birth_date.year,
            data.birth_date.month,
            data.birth_date.day,
         ).toISOString(),
      });
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

         <div className="h-full px-4 sm:px-6 lg:px-[80px] py-10 sm:py-14">
            <div className="w-full flex flex-col justify-center items-center">
               <h3 className="text-2xl sm:text-3xl lg:text-4xl leading-tight font-SFProDisplay font-medium">
                  Create Your Account
               </h3>
               <p className="text-xs sm:text-sm md:text-base font-SFProText font-light pt-3 sm:pt-4">
                  One Account is all you need to access all of our services
               </p>

               <p className="text-xs sm:text-sm md:text-base font-SFProText font-light py-3 sm:py-4 flex gap-2 flex-wrap sm:flex-nowrap">
                  Already have an account?{' '}
                  <Link
                     href="/sign-in"
                     className="text-blue-500 flex underline"
                  >
                     Sign in
                     <GoArrowUpRight className="size-4" />
                  </Link>
               </p>

               <div className="w-full max-w-[500px]">
                  <LoadingOverlay isLoading={isLoading} text="Loading...">
                     <div className="w-full mt-2">
                        <div className="flex flex-col sm:flex-row gap-2">
                           <div className="w-full">
                              <form
                                 onSubmit={form.handleSubmit(
                                    handleSubmitRegister,
                                 )}
                                 onKeyDown={(e) => {
                                    if (
                                       e.key === 'Enter' &&
                                       !e.defaultPrevented
                                    ) {
                                       e.preventDefault();
                                       form.handleSubmit(
                                          handleSubmitRegister,
                                       )();
                                    }
                                 }}
                              >
                                 <div className="w-full flex flex-col sm:flex-row gap-2 justify-between mt-3">
                                    <FieldInput
                                       form={form}
                                       name="first_name"
                                       label="First Name"
                                       className="rounded-xl w-full"
                                       type="text"
                                       disabled={isLoading}
                                       errorTextClassName="pb-1"
                                    />

                                    <FieldInput
                                       form={form}
                                       name="last_name"
                                       label="Last Name"
                                       className="rounded-xl w-full"
                                       type="text"
                                       disabled={isLoading}
                                       errorTextClassName="pb-1"
                                    />
                                 </div>
                              </form>
                           </div>
                        </div>

                        <div className="mt-3">
                           <div className="w-full">
                              <FormSelector
                                 form={form}
                                 name="country"
                                 label="Country/Region"
                                 placeholder="Select a country"
                                 options={countries}
                                 required
                              />
                           </div>
                        </div>

                        <div className="mt-3">
                           <FormBirthDateSelector
                              form={form}
                              name="birth_date"
                              required
                           />
                        </div>

                        <Separator className="mt-3 bg-slate-200" />

                        <div className="mt-5">
                           <div className="w-full">
                              <FieldInput
                                 form={form}
                                 name="email"
                                 label="Email"
                                 className="rounded-xl w-full"
                                 type="text"
                                 disabled={isLoading}
                                 errorTextClassName="pb-5"
                              />
                              <p className="mt-2 text-xs sm:text-sm font-light text-slate-500">
                                 *Note: This will be your new Account.
                              </p>
                           </div>
                        </div>

                        <div className="w-full mt-4">
                           <FieldInput
                              form={form}
                              name="password"
                              label="Password"
                              className="rounded-xl w-full mb-2"
                              type="password"
                              disabled={isLoading}
                              errorTextClassName="pb-5"
                           />
                        </div>

                        <FieldInput
                           form={form}
                           name="confirm_password"
                           label="Confirm Password"
                           className="rounded-xl w-full"
                           type="password"
                           disabled={isLoading}
                           errorTextClassName="pb-5"
                        />
                     </div>

                     <Separator className="mt-3 bg-slate-200" />

                     <div className="mt-4">
                        <FormPhoneInput
                           form={form}
                           name="phone_number"
                           label="Phone Number"
                           required
                        />
                     </div>
                  </LoadingOverlay>

                  <Separator className="mt-10 bg-slate-200" />

                  <div className="flex justify-center flex-col items-center mt-5">
                     <Image
                        src={signUpImage}
                        alt="cover"
                        width={1000}
                        height={1000}
                        quality={100}
                        className="h-8 w-8"
                     />

                     <p className="text-xs sm:text-sm font-light mt-3 text-justify text-slate-600">
                        Your Apple Account information is used to allow you to
                        sign in securely and access your data. Apple records
                        certain data for security, support and reporting
                        purposes. If you agree, Apple may also use your Apple
                        Account information to send you marketing emails and
                        communications, including based on your use of Apple
                        services{' '}
                        <a href="#" className="text-blue-500 hover:underline">
                           See how your data is managed
                        </a>
                        .
                     </p>

                     <Button
                        className="mt-5 bg-blue-500 hover:bg-blue-400 active:bg-blue-500 cursor-pointer w-full py-2 text-white rounded-lg text-center"
                        onClick={() => {
                           form.handleSubmit(handleSubmitRegister)();
                        }}
                        disabled={
                           form.formState.isSubmitting ||
                           isLoading ||
                           form.formState.isValidating
                        }
                     >
                        {isLoading ? 'Registering...' : 'Create Account'}
                     </Button>
                  </div>
               </div>
            </div>
         </div>
      </div>
   );
};

export default withAuth(SignUpPage);
