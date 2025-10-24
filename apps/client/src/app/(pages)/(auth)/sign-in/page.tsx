'use client';

import { GoArrowUpRight } from 'react-icons/go';
import Image from 'next/image';
import googlePng from '@assets/images/google.png';
import Link from 'next/link';
import { LoginFormType, LoginResolver } from '~/domain/schemas/auth.schema';
import { FieldInput } from '@components/client/forms/field-input';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import withAuth from '@components/HoCs/with-auth.hoc';
import useAuthService from '~/components/hooks/api/use-auth-service';
import { useAppSelector } from '~/infrastructure/redux/store';
import { EVerificationType } from '~/domain/enums/verification-type.enum';

const SignInPage = () => {
   const { login, isLoading } = useAuthService();

   const router = useRouter();

   const appStateRoute = useAppSelector((state) => state.app.route);

   const form = useForm<LoginFormType>({
      resolver: LoginResolver,
      defaultValues: {
         email: '',
         password: '',
      },
   });

   const onSubmit = async (data: LoginFormType) => {
      const result = await login(data);

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
         } else {
            router.replace(
               appStateRoute.previousUnAuthenticatedPath || '/shop/iphone',
            );
         }
      }
   };

   return (
      <div className="w-[1180px] mx-auto px-10">
         <section className="flex justify-between border-b h-[60px] items-center">
            <div>
               <h2 className="text-2xl font-medium">Account</h2>
            </div>

            <div className="flex gap-3 font-SFProText text-slate-500 text-sm">
               <Link href="/sign-in">Sign in</Link>
               <Link href="/sign-up">Create your Account</Link>
               <a href="#">FAQ</a>
            </div>
         </section>

         <div className="h-full px-[200px] py-[200px]">
            <div className="w-full  flex flex-col justify-center items-center">
               <h3 className="text-4xl font-SFProDisplay font-medium">
                  Account
               </h3>
               <p className="text-base font-SFProText font-light pt-5">
                  Manage Your Account
               </p>

               <p className="text-base font-SFProText font-light py-1 flex gap-2">
                  Not have account yet?{' '}
                  <Link
                     href="/sign-up"
                     className="text-blue-500 flex underline"
                  >
                     Create your account
                     <GoArrowUpRight className="size-4" />
                  </Link>
               </p>

               <div className="w-[480px]">
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
                              visibleEyeIcon={false}
                              className="rounded-xl rounded-t-none"
                              fetchingFunc={onSubmit}
                              hasEnterArrowButton={true}
                           />
                        </form>
                     </div>

                     <div className="mt-3  ml-auto w-fit">
                        <div className="flex items-center space-x-2">
                           <input
                              type="checkbox"
                              id="remember"
                              className="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                           />
                           <label
                              htmlFor="remember"
                              className="text-gray-600 text-sm"
                           >
                              Remember me
                           </label>
                        </div>

                        <div className="mt-2 flex text-blue-500">
                           <Link
                              href="/forgot-password"
                              className="text-sm font-normal text-end w-full block hover:underline"
                           >
                              Forgot password?
                           </Link>
                           <GoArrowUpRight className="size-4" />
                        </div>
                     </div>

                     <div className="mt-5">
                        <h3 className="text-lg font-SFProText font-medium text-center">
                           Sign In with another method
                        </h3>

                        <div className="flex flex-col justify-center items-center gap-3 mt-3">
                           <button className="w-fit px-3 py-2 rounded-full font-SFProText text-base font-medium min-w-[300px] bg-slate-200 flex items-center justify-between hover:bg-slate-300 active:bg-slate-200">
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
