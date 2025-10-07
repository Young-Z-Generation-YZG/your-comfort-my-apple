'use client';

import { GoArrowUpRight } from 'react-icons/go';
import Link from 'next/link';
import { FieldInput } from '@components/client/forms/field-input';
import { LoadingOverlay } from '@components/client/loading-overlay';
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { useToast } from '@components/hooks/use-toast';
import { useForm } from 'react-hook-form';
import {
   sendEmailResetPasswordFormType,
   sendEmailResetPasswordResolver,
} from '~/domain/schemas/auth.schema';
import { useSendEmailResetPasswordAsyncMutation } from '~/infrastructure/services/auth.service';

const defaultValues: sendEmailResetPasswordFormType = {
   email: '',
};

const sendEmailResetPasswordPage = () => {
   const [isLoading, setIsLoading] = useState(false);
   const router = useRouter();

   const { toast } = useToast();

   const form = useForm<sendEmailResetPasswordFormType>({
      resolver: sendEmailResetPasswordResolver,
      defaultValues: defaultValues,
   });

   const [
      sendEmailAsync,
      { isLoading: isFetching, isSuccess, data, error, isError, reset },
   ] = useSendEmailResetPasswordAsyncMutation();

   const onSubmit = async (data: sendEmailResetPasswordFormType) => {
      console.log('data', data);

      await sendEmailAsync(data).unwrap();
   };

   useEffect(() => {
      setIsLoading(isFetching);
   }, [isFetching]);

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
                     <LoadingOverlay
                        isLoading={isLoading}
                        text="Email is sending..."
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
                                 className="rounded-xl"
                                 type="text"
                                 disabled={isLoading}
                                 errorTextClassName="pb-5"
                                 fetchingFunc={onSubmit}
                                 hasEnterArrowButton={true}
                              />
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
                     Email Sent
                  </h3>
                  <p className="text-base font-SFProText font-light py-5">
                     Please check your email for the verification code.
                  </p>
               </div>
            </div>
         )}
      </div>
   );
};

export default sendEmailResetPasswordPage;
