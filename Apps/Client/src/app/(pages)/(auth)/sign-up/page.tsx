'use client';

import Link from 'next/link';
import { useState } from 'react';
import { GoArrowUpRight } from 'react-icons/go';
import Selector from '../_components/selector-input';
import BirthdaySelector from '~/components/client/forms/birthday-selector';
import { Separator } from '~/components/ui/Separator';
import PhoneInput from '~/components/client/forms/phone-input';
import Image from 'next/image';
import signUpImage from '@assets/images/sign-up.png';
import Button from '../_components/Button';
import { FieldInput } from '~/components/client/forms/field-input';

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

const SignUpPage = () => {
   const [email, setEmail] = useState('');
   const [password, setPassword] = useState('');

   const [phone, setPhone] = useState('');
   const [phoneValid, setPhoneValid] = useState(true);

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

         <div className="h-full px-[200px] py-[50px]">
            <div className="w-full  flex flex-col justify-center items-center">
               <h3 className="text-4xl font-SFProDisplay font-medium">
                  Create Your Account
               </h3>
               <p className="text-base font-SFProText font-light p3-5">
                  One Account is all you need to access all of our services
               </p>

               <p className="text-base font-SFProText font-light py-5 flex gap-2">
                  Already have an account?{' '}
                  <Link
                     href="/sign-in"
                     className="text-blue-500 flex underline"
                  >
                     Sign in
                     <GoArrowUpRight className="size-4" />
                  </Link>
               </p>

               <div className="w-[500px]">
                  <div className="flex gap-2">
                     <div className="w-full">
                        <FieldInput
                           label="First Name"
                           value={email}
                           onChange={setEmail}
                           className="border border-gray-300 overflow-hidden rounded-xl w-full"
                        />
                     </div>

                     <div className="w-full">
                        <FieldInput
                           label="Last Name"
                           value={email}
                           onChange={setEmail}
                           className="border border-gray-300 overflow-hidden rounded-xl w-full"
                        />
                     </div>
                  </div>

                  <div className="mt-3">
                     <div className="w-full">
                        <Selector />
                     </div>
                  </div>

                  <div className="mt-3">
                     <BirthdaySelector />
                  </div>

                  <Separator className="mt-3 bg-slate-200" />

                  <div className="mt-5">
                     <div className="w-full">
                        <FieldInput
                           label="Email"
                           value={email}
                           onChange={setEmail}
                           className="border border-gray-300 overflow-hidden rounded-xl w-full"
                        />
                        <p className="mt-2 text-sm font-light">
                           *Note: This will be your new Account.
                        </p>
                     </div>
                  </div>

                  <div className="mt-4">
                     <div className="mt-2">
                        <div className="w-full">
                           <FieldInput
                              label="Password"
                              value={email}
                              onChange={setEmail}
                              className="border border-gray-300 overflow-hidden rounded-xl w-full"
                           />
                        </div>

                        <div className="mt-3">
                           <div className="w-full">
                              <FieldInput
                                 label="Confirm Password"
                                 value={email}
                                 onChange={setEmail}
                                 className="border border-gray-300 overflow-hidden rounded-xl w-full"
                              />
                           </div>
                        </div>
                     </div>
                  </div>

                  <Separator className="mt-3 bg-slate-200" />

                  <div className="mt-4">
                     <PhoneInput
                        value={phone}
                        onChange={setPhone}
                        onValidChange={setPhoneValid}
                        required={true}
                     />
                  </div>

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

                     <Button className="mt-5 bg-blue-500 hover:bg-blue-400 active:bg-blue-500 cursor-pointer w-full py-2 text-white rounded-lg text-center">
                        Continue
                     </Button>
                  </div>
               </div>
            </div>
         </div>
      </div>
   );
};

export default SignUpPage;
