'use client';

import { useState } from 'react';
import AnimatedInput from '../_components/animated-input';
import { GoArrowUpRight } from 'react-icons/go';
import Image from 'next/image';
import googlePng from '@assets/images/google.png';
import Link from 'next/link';

const SignInPage = () => {
   const [loginData, setLoginData] = useState({
      email: '',
      password: '',
   });

   const [email, setEmail] = useState('');
   const [password, setPassword] = useState('');

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
               <p className="text-base font-SFProText font-light py-5">
                  Manage Your Account
               </p>

               <div className="w-[480px]">
                  <div className="border border-gray-300 overflow-hidden rounded-xl">
                     <div className="border border-gray-300 rounded-xl w-[480px] bg-white">
                        <div className="border-b border-gray-300">
                           <AnimatedInput
                              label="Email"
                              value={email}
                              onChange={setEmail}
                           />
                        </div>
                        <div>
                           <AnimatedInput
                              type="password"
                              label="Password"
                              value={password}
                              onChange={setPassword}
                              hasArrowButton={true}
                           />
                        </div>
                     </div>
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
                        <a
                           href="#"
                           className="text-sm font-medium text-end w-full block"
                        >
                           Forgot password?
                        </a>
                        <GoArrowUpRight className="size-4" />
                     </div>
                  </div>

                  <div className="mt-5">
                     <h3 className="text-lg font-SFProText font-medium text-center">
                        Sign In with another method
                     </h3>

                     <div className="flex flex-col justify-center items-center gap-3 mt-3">
                        <button className="w-fit px-3 py-2 rounded-full font-SFProText text-base font-medium min-w-[300px] bg-slate-100 flex items-center justify-between hover:bg-slate-200 active:bg-slate-100">
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
                        {/* <button className="w-fit px-3 py-2 rounded-full font-SFProText text-base font-medium min-w-[300px] bg-slate-300">
                           Sign in with Facebook
                        </button> */}
                     </div>
                  </div>
               </div>
            </div>
         </div>
      </div>
   );
};

export default SignInPage;
