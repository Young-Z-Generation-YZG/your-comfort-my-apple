'use client';

import { GoArrowUpRight } from 'react-icons/go';
import Link from 'next/link';
import { FieldInput } from '~/components/client/forms/field-input';
import { LoadingOverlay } from '~/components/client/loading-overlay';
import { useSearchParams } from 'next/navigation';
import { useForm } from 'react-hook-form';
import {
   resetPasswordFormType,
   resetPasswordResolver,
} from '~/domain/schemas/auth.schema';
import { Button } from '~/components/ui/button';
import useAuthService from '~/hooks/api/use-auth-service';

const ResetPasswordPage = () => {
   const searchParams = useSearchParams();
   const { resetPassword, isLoading, resetPasswordState } = useAuthService();

   const _email = searchParams.get('_email');
   const _token = searchParams.get('_token');

   const form = useForm<resetPasswordFormType>({
      resolver: resetPasswordResolver,
      defaultValues: {
         email: _email || '',
         token: _token || '',
         new_password: '',
         confirm_password: '',
      },
   });

   const onSubmit = async (data: resetPasswordFormType) => {
      resetPassword(data);
   };

   const isSuccess = resetPasswordState.isSuccess;

   return (
      <div className="w-[1180px] mx-auto px-10">
         <section className="flex justify-between border-b h-[60px] items-center">
            <div>
               <h2 className="text-2xl font-medium">Forgot Password</h2>
            </div>

            <div className="flex gap-3 font-SFProText text-slate-500 text-sm">
               <Link href="/sign-in">Sign in</Link>
               <Link href="/sign-up">Create your Account</Link>
               <a href="#">FAQ</a>
            </div>
         </section>

         {!isSuccess && (
            <div className="h-full px-[200px] py-[200px]">
               <div className="w-full  flex flex-col justify-center items-center">
                  <h3 className="text-4xl font-SFProDisplay font-medium">
                     Forgot Password
                  </h3>
                  <p className="text-base font-SFProText font-light py-5">
                     Please enter your email address to receive a verification
                     code to reset your password.
                  </p>

                  <div className="w-[480px]">
                     <LoadingOverlay isLoading={isLoading} text="loading...">
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
                                 name="new_password"
                                 label="New Password"
                                 type="password"
                                 disabled={isLoading}
                                 errorTextClassName="mb-5"
                                 className="rounded-xl rounded-b-none"
                              />

                              <FieldInput
                                 form={form}
                                 name="confirm_password"
                                 label="Confirm Password"
                                 type="password"
                                 disabled={isLoading}
                                 className="rounded-xl rounded-t-none"
                                 fetchingFunc={onSubmit}
                              />

                              <Button
                                 className="mt-5 bg-blue-500 hover:bg-blue-400 active:bg-blue-500 cursor-pointer w-full py-2 text-white rounded-lg text-center"
                                 onClick={() => {
                                    form.handleSubmit(onSubmit)();
                                 }}
                                 disabled={
                                    form.formState.isSubmitting ||
                                    isLoading ||
                                    form.formState.isValidating
                                 }
                              >
                                 {isLoading ? 'Loading...' : 'Submit'}
                              </Button>
                           </form>
                        </div>

                        <div className="mt-3  ml-auto w-fit">
                           <div className="mt-2 flex text-blue-500">
                              <Link
                                 href="/sign-in"
                                 className="text-sm font-normal text-end w-full block hover:underline"
                              >
                                 Sign in?
                              </Link>
                              <GoArrowUpRight className="size-4" />
                           </div>
                        </div>
                     </LoadingOverlay>
                  </div>
               </div>
            </div>
         )}

         {isSuccess && (
            <div className="h-full px-[200px] py-[200px]">
               <div className="w-full  flex flex-col justify-center items-center">
                  <h3 className="text-4xl font-SFProDisplay font-medium">
                     Reset Password Success
                  </h3>
                  <p className="text-base font-SFProText font-light py-5">
                     Your password has been reset successfully. You can now log
                     in with your new password.
                  </p>
                  <div className="mt-3 w-fit">
                     <div className="mt-2 flex text-blue-500">
                        <Link
                           href="/sign-in"
                           className="text-sm font-normal text-end w-full block hover:underline"
                        >
                           Sign in?
                        </Link>
                        <GoArrowUpRight className="size-4" />
                     </div>
                  </div>
               </div>
            </div>
         )}
      </div>
   );
};

export default ResetPasswordPage;
