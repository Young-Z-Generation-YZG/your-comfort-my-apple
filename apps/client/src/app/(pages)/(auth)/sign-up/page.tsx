'use client';

import Link from 'next/link';
import { GoArrowUpRight } from 'react-icons/go';

import Image from 'next/image';
import signUpImage from '@assets/images/sign-up.png';
import Button from '../_components/Button';
import { FieldInput } from '@components/client/forms/field-input';
import {
   RegisterFormType,
   RegisterResolver,
} from '~/domain/schemas/auth.schema';
import { FormSelector } from '../_components/selector-input';
import { FormBirthdaySelector } from '@components/client/forms/birthday-selector';
import { FormPhoneInput } from '@components/client/forms/phone-input';
import { useRouter } from 'next/navigation';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { EVerificationType } from '~/domain/enums/verification-type.enum';
import { Separator } from '@components/ui/separator';
import { useForm } from 'react-hook-form';
import withAuth from '@components/HoCs/with-auth.hoc';
import useAuth from '../../../../components/hooks/use-auth';
import { countries } from '~/domain/constants/countries';

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
   const router = useRouter();

   const form = useForm<RegisterFormType>({
      resolver: RegisterResolver,
      defaultValues: defaultValues,
   });

   const { register, isLoading } = useAuth();

   const handleSubmitRegister = async (data: RegisterFormType) => {
      const result = await register({
         ...data,
         birth_day: `${data.birth_day.year}-${data.birth_day.month}-${data.birth_day.day}`,
      });

      if (result.isSuccess && result.data) {
         if (
            result.data.verification_type ===
            EVerificationType.EMAIL_VERIFICATION
         ) {
            const params = result.data.params;

            const queryParams = new URLSearchParams();

            for (const key in params) {
               if (params.hasOwnProperty(key)) {
                  queryParams.set(key, String(params[key]));
               }
            }

            router.push(`/verify/otp?${queryParams.toString()}`, {
               scroll: false,
            });
         }
      }
   };

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
                  <LoadingOverlay isLoading={isLoading} text="Loading...">
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
                                       disabled={isLoading}
                                       className="md:pt-[20px] pt-[35px] md:h-auto h-[100px] lg:text-base md:text-xl text-3xl rounded-xl"
                                       labelClassName="lg:text-base md:text-xl text-3xl"
                                       errorTextClassName="lg:text-sm md:text-lg text-2xl pb-5"
                                    />

                                    <FieldInput
                                       form={form}
                                       name="last_name"
                                       label="Last Name"
                                       type="text"
                                       disabled={isLoading}
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
                                 disabled={isLoading}
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
                              disabled={isLoading}
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
                           disabled={isLoading}
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
