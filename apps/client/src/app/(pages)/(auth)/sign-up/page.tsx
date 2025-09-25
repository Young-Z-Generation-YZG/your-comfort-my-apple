'use client';

import Link from 'next/link';
import { useEffect, useState } from 'react';
import { GoArrowUpRight } from 'react-icons/go';

import Image from 'next/image';
import signUpImage from '@assets/images/sign-up.png';
import Button from '../_components/Button';
import { FieldInput } from '@components/client/forms/field-input';
import {
   RegisterFormType,
   RegisterResolver,
} from '~/domain/schemas/auth.schema';
import { useRegisterAsyncMutation } from '~/infrastructure/services/auth.service';
import { FormSelector } from '../_components/selector-input';
import { FormBirthdaySelector } from '@components/client/forms/birthday-selector';
import { FormPhoneInput } from '@components/client/forms/phone-input';
import { ServerErrorResponse } from '~/domain/interfaces/errors/error.interface';
import { useRouter } from 'next/navigation';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { VERIFICATION_TYPES } from '~/domain/enums/verification-type.enum';
import { useToast } from '~/hooks/use-toast';
import { Separator } from '@components/ui/separator';
import { useForm } from 'react-hook-form';
import withAuth from '@components/HoCs/with-auth.hoc';

// List of countries - this would typically be more comprehensive
const countries = [
   'Afghanistan',
   'Albania',
   'Algeria',
   'Andorra',
   'Angola',
   'Argentina',
   'Armenia',
   'Australia',
   'Austria',
   'Belgium',
   'Brazil',
   'Canada',
   'China',
   'Denmark',
   'Egypt',
   'Finland',
   'France',
   'Germany',
   'Greece',
   'India',
   'Indonesia',
   'Iran',
   'Iraq',
   'Ireland',
   'Israel',
   'Italy',
   'Japan',
   'Kenya',
   'Korea, South',
   'Mexico',
   'Netherlands',
   'New Zealand',
   'Norway',
   'Pakistan',
   'Poland',
   'Portugal',
   'Russia',
   'Saudi Arabia',
   'Singapore',
   'South Africa',
   'Spain',
   'Sweden',
   'Switzerland',
   'Taiwan',
   'Tajikistan',
   'Tanzania',
   'Thailand',
   'Timor-Leste',
   'Togo',
   'Tokelau',
   'Tonga',
   'Trinidad and Tobago',
   'Tunisia',
   'Türkiye',
   'Turkmenistan',
   'Turks and Caicos Islands',
   'Tuvalu',
   'Uganda',
   'Ukraine',
   'United Arab Emirates',
   'United Kingdom',
   'United States',
   'Vietnam',
];

const defaultValues: RegisterFormType = {
   email: '',
   password: '',
   confirm_password: '',
   first_name: '',
   last_name: '',
   phone_number: '',
   birth_day: { month: 0, day: 0, year: 0 },
   country: '',
};

const SignUpPage = () => {
   const [isLoading, setIsLoading] = useState(false);
   const { toast } = useToast();

   const router = useRouter();

   const form = useForm<RegisterFormType>({
      resolver: RegisterResolver,
      defaultValues: defaultValues,
   });

   const [
      registerAsync,
      { isLoading: isFetching, error: registerError, isError, reset },
   ] = useRegisterAsyncMutation();

   const handleSubmitRegister = async (data: RegisterFormType) => {
      const result = await registerAsync({
         ...data,
         birth_day: `${data.birth_day.year}-${data.birth_day.month}-${data.birth_day.day}`,
      }).unwrap();

      if (result.verification_type === VERIFICATION_TYPES.EMAIL_VERIFICATION) {
         const params = result.params;

         const queryParams = new URLSearchParams();

         for (const key in params) {
            if (params.hasOwnProperty(key)) {
               queryParams.append(key, String(params[key]));
            }
         }

         router.replace(`/verify/otp?${queryParams}`);
      }
   };

   useEffect(() => {
      if (isError) {
         const serverError = registerError as ServerErrorResponse;
         toast({
            variant: 'destructive',
            title: `${serverError?.error?.message || 'Server is busy, try again later'}`,
            // description: `${serverError.error.code}`,
         });
         reset(); // Reset the mutation state to clear isError
      }
   }, [isError, registerError, reset]);

   return (
      <div className="max-w-[1180px] mx-auto px-10">
         <section className="flex justify-between border-b h-[60px] items-center">
            <div>
               <h2 className="text-2xl font-medium">Account</h2>
            </div>

            <div className="flex gap-3 font-SFProText text-slate-500 lg:text-sm md:text-xl text-2xl">
               <Link href="/sign-in">Sign in</Link>
               <Link href="/sign-up">Create your Account</Link>
               <a href="#">FAQ</a>
            </div>
         </section>

         <div className="h-full lg:px-[200px] md:px-[100px] px-0 py-[50px]">
            <div className="w-full flex flex-col justify-center items-center">
               <h3 className="font-SFProDisplay font-medium lg:text-3xl md:text-5xl text-7xl">
                  Create Your Account
               </h3>
               <p className="font-SFProText font-light pt-5 lg:text-base md:text-xl text-2xl">
                  One Account is all you need to access all of our services
               </p>

               <p className="font-SFProText font-light pt-5 flex gap-2 lg:text-base md:text-xl text-2xl lg:pb-2 md:pb-5 pb-10">
                  Already have an account?{' '}
                  <Link
                     href="/sign-in"
                     className="text-blue-500 flex underline"
                  >
                     Sign in
                     <GoArrowUpRight className="size-4" />
                  </Link>
               </p>

               <div className="lg:w-[500px] md:w-[80%] w-full">
                  <LoadingOverlay
                     isLoading={isLoading || isFetching}
                     text="Loading..."
                  >
                     <div className="w-full mt-2">
                        <div className="flex gap-2">
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
                                 <div className="w-full grid grid-cols-2 gap-2 mt-3">
                                    <FieldInput
                                       form={form}
                                       name="first_name"
                                       label="First Name"
                                       type="text"
                                       disabled={isLoading || isFetching}
                                       className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl"
                                       labelClassName="lg:text-base md:text-xl text-3xl"
                                       errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
                                    />

                                    <FieldInput
                                       form={form}
                                       name="last_name"
                                       label="Last Name"
                                       type="text"
                                       disabled={isLoading || isFetching}
                                       className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl"
                                       labelClassName="lg:text-base md:text-xl text-3xl"
                                       errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
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
                                 className="lg:text-base md:text-xl text-3xl rounded-xl"
                                 labelClassName="lg:text-sm md:text-base text-2xl"
                                 inputClassName="lg:text-base md:text-xl text-3xl rounded-xl"
                                 errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
                              />
                           </div>
                        </div>

                        <div className="mt-3">
                           <FormBirthdaySelector
                              form={form}
                              name="birth_day"
                              required
                              className="lg:text-base md:text-xl text-3xl rounded-xl"
                              // inputClassName="lg:text-base md:text-xl text-3xl rounded-xl"
                              // errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
                           />
                        </div>

                        <Separator className="mt-3 bg-slate-200" />

                        <div className="mt-5">
                           <div className="w-full">
                              <FieldInput
                                 form={form}
                                 name="email"
                                 label="Email"
                                 type="text"
                                 disabled={isLoading || isFetching}
                                 className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl"
                                 labelClassName="lg:text-base md:text-xl text-3xl"
                                 errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
                              />
                              <p className="mt-2 lg:text-sm md:text-lg text-2xl font-light text-slate-500">
                                 *Note: This will be your new Account.
                              </p>
                           </div>
                        </div>

                        <div className="w-full my-4">
                           <FieldInput
                              form={form}
                              name="password"
                              label="Password"
                              type="password"
                              disabled={isLoading || isFetching}
                              className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl"
                              labelClassName="lg:text-base md:text-xl text-3xl"
                              errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
                           />
                        </div>

                        <FieldInput
                           form={form}
                           name="confirm_password"
                           label="Confirm Password"
                           type="password"
                           disabled={isLoading || isFetching}
                           className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl"
                           labelClassName="lg:text-base md:text-xl text-3xl"
                           errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
                        />
                     </div>

                     <Separator className="mt-3 bg-slate-200" />

                     <div className="mt-4">
                        <FormPhoneInput
                           form={form}
                           name="phone_number"
                           label="Phone Number"
                           required
                           className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl"
                           labelClassName="lg:text-base md:text-xl text-3xl"
                           errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
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

                     <p className="text-xs font-light mt-3 text-justify text-slate-600">
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
                           isFetching ||
                           form.formState.isValidating
                        }
                     >
                        {isLoading || isFetching
                           ? 'Registering...'
                           : 'Create Account'}
                     </Button>
                  </div>
               </div>
            </div>
         </div>
      </div>
   );
};

export default withAuth(SignUpPage);
