'use client';

import { useEffect, useState } from 'react';
import { OTPInput } from '@components/client/forms/otp-input';
import { LoadingOverlay } from '@components/client/loading-overlay';
import Button from '../../_components/Button';
import { useAppSelector } from '~/infrastructure/redux/store';
import { useRouter, useSearchParams } from 'next/navigation';
import { OtpFormType, OtpResolver } from '~/domain/schemas/auth.schema';
import { useVerifyOtpAsyncMutation } from '~/infrastructure/services/auth.service';
import { useForm } from 'react-hook-form';

const defaultValues: OtpFormType = {
   email: '',
   token: '',
   otp: '',
};

const OtpPage = () => {
   const [otp, setOtp] = useState('');
   const [isVerifying, setIsVerifying] = useState(false);
   const [isError, setIsError] = useState(false);
   const [isSuccess, setIsSuccess] = useState(false);
   const [timeLeft, setTimeLeft] = useState(30);

   const router = useRouter();
   const searchParams = useSearchParams();
   const _email = searchParams.get('_email');
   const _token = searchParams.get('_token');
   const _otp = searchParams.get('_otp');

   const userEmail = useAppSelector((state) => state.auth.value.userEmail);

   const form = useForm<OtpFormType>({
      resolver: OtpResolver,
      defaultValues: defaultValues,
   });

   const [
      verifyOtpAsync,
      {
         isLoading: isFetching,
         error: verifyOtpError,
         isError: verifyOtpIsError,
         isSuccess: verifyOtpIsSuccess,
         reset,
      },
   ] = useVerifyOtpAsyncMutation();

   const handleSubmit = async (data: OtpFormType) => {
      await verifyOtpAsync(data).unwrap();
   };

   // Handle OTP change
   const handleChange = (value: string) => {
      setOtp(value);
   };

   // Handle OTP completion
   const handleComplete = async (value: string) => {
      form.setValue('otp', value);
      handleSubmit(form.getValues());
   };

   useEffect(() => {
      if (verifyOtpIsError) {
         setIsError(true);
         reset();
      }
   }, [verifyOtpIsError]);

   useEffect(() => {
      if (verifyOtpIsSuccess) {
         setIsSuccess(true);
         setIsVerifying(true);
         setIsError(false);
         reset();
         setTimeout(() => {
            window.location.replace('/sign-in');
         }, 2000);
      }
   }, [verifyOtpIsSuccess]);

   useEffect(() => {
      setIsVerifying(isFetching);
   }, [isFetching]);

   // Reset timer
   const handleResendCode = () => {
      setTimeLeft(30);
      setIsError(false);
      setIsSuccess(false);
      setOtp('');

      alert('A new verification code has been sent!');
   };

   // Countdown timer for resend code
   useEffect(() => {
      if (timeLeft <= 0) return;

      const timer = setTimeout(() => {
         setTimeLeft(timeLeft - 1);
      }, 1000);

      return () => clearTimeout(timer);
   }, [timeLeft]);

   // Get query params
   useEffect(() => {
      const _email = searchParams.get('_email');
      const _token = searchParams.get('_token');
      const _otp = searchParams.get('_otp');

      if (_email && _token) {
         form.setValue('email', _email);
         form.setValue('token', _token);
      }

      if (_email && _token && _otp) {
         form.setValue('email', _email);
         form.setValue('token', _token);
         form.setValue('otp', _otp);

         form.handleSubmit(handleSubmit)();
      }
   }, [searchParams, form]);

   return (
      <div className="w-[1180px] mx-auto px-10">
         <div className="h-full px-[200px] py-[200px]">
            <div className="w-full  flex flex-col justify-center items-center">
               <h3 className="text-4xl font-SFProDisplay font-medium">
                  OTP Verification
               </h3>
               <p className="text-base font-SFProText font-light py-4">
                  Enter the 6-digit code sent from your email
               </p>

               <div className="w-[480px]">
                  <div className="max-w-md mx-auto p-6 bg-white rounded-lg shadow-md">
                     <div className="text-center mb-6">
                        <h2 className="text-2xl font-medium font-SFProText">
                           Verification Code
                        </h2>
                        <p className="text-gray-600 mt-2 font-SFProText">
                           We've sent a 6-digit code to your email:
                           {userEmail}
                        </p>
                     </div>

                     <OTPInput
                        length={6}
                        onComplete={handleComplete}
                        onChange={handleChange}
                        disabled={isVerifying || isSuccess}
                        isError={isError}
                        errorMessage="Invalid verification code. Please try again."
                        className="mb-6"
                     />

                     {isVerifying && (
                        <div className="text-center text-blue-600 my-4">
                           <svg
                              className="animate-spin h-5 w-5 mx-auto mb-2"
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
                           Verifying code...
                        </div>
                     )}

                     {isSuccess && (
                        <div className="text-center text-green-600 my-4 flex items-center justify-center">
                           <svg
                              xmlns="http://www.w3.org/2000/svg"
                              width="20"
                              height="20"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              strokeWidth="2"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              className="mr-2"
                           >
                              <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14" />
                              <polyline points="22 4 12 14.01 9 11.01" />
                           </svg>
                           Code verified successfully!
                        </div>
                     )}

                     <div className="text-center mt-6">
                        <p className="text-gray-600 text-sm mb-2">
                           Didn't receive the code?
                        </p>
                        {timeLeft > 0 ? (
                           <p className="text-gray-500 text-sm">
                              Resend code in{' '}
                              <span className="font-medium">{timeLeft}s</span>
                           </p>
                        ) : (
                           <button
                              onClick={handleResendCode}
                              className="text-blue-600 hover:text-blue-800 text-sm font-medium"
                           >
                              Resend Code
                           </button>
                        )}
                     </div>

                     <div className="mt-8">
                        <Button
                           className="mt-5 bg-blue-500 hover:bg-blue-400 active:bg-blue-500 cursor-pointer w-full py-2 text-white rounded-lg text-center"
                           onClick={() => {
                              if (otp.length === 6) handleComplete(otp);
                           }}
                           disabled={
                              otp.length !== 6 || isVerifying || isSuccess
                           }
                        >
                           {isVerifying || isFetching
                              ? 'Verifying...'
                              : 'Verify Code'}
                        </Button>
                     </div>

                     <div className="mt-4 text-center">
                        <button
                           onClick={() => {
                              router.replace('/sign-in');
                           }}
                           className="text-gray-600 hover:text-gray-800 text-sm"
                        >
                           Back to Sign In
                        </button>
                     </div>
                  </div>
               </div>
            </div>
         </div>
      </div>
   );
};

export default OtpPage;
