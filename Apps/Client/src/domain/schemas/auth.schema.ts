import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { ILoginPayload } from '~/domain/interfaces/auth/login.interface';
import { IOtpPayload } from '../interfaces/auth/otp.interface';
import { IRegisterPayload } from '../interfaces/auth/register.interface';
import {
   IChangePasswordPayload,
   IResetPasswordPayload,
   ISendEmailResetPasswordPayload,
} from '../interfaces/auth/password.interface';

/// Login Schema
const LoginSchema = z.object({
   email: z.string().email({
      message: 'Please enter a valid email',
   }),
   password: z.string().min(1, {
      message: 'Password is required',
   }),
} satisfies Record<keyof ILoginPayload, any>);

export type LoginFormType = z.infer<typeof LoginSchema>;
export const LoginResolver = zodResolver(LoginSchema);

/// Register Schema
const birthdaySchema = z
   .object({
      month: z
         .number({
            required_error: 'Month is required',
            invalid_type_error: 'Month must be a number',
         })
         .min(1, 'Month must be between 1 and 12')
         .max(12, 'Month must be between 1 and 12'),
      day: z
         .number({
            required_error: 'Day is required',
            invalid_type_error: 'Day must be a number',
         })
         .min(1, 'Day is required')
         .max(31, 'Invalid day'),
      year: z
         .number({
            required_error: 'Year is required',
            invalid_type_error: 'Year must be a number',
         })
         .min(1900, 'Year must be after 1900'),
   })
   .refine(
      (data) => {
         // Validate that the date is valid
         const daysInMonth = new Date(data.year, data.month, 0).getDate();
         return data.day <= daysInMonth;
      },
      {
         message: 'Invalid date',
         path: ['day'],
      },
   )
   .refine(
      (data) => {
         // Validate that the date is not in the future
         const birthDate = new Date(data.year, data.month - 1, data.day);
         const today = new Date();

         return birthDate <= today;
      },
      {
         message: 'Birthday cannot be in the future',
         path: ['year'],
      },
   );

const vietnamesePhoneSchema = z
   .string()
   .refine((value) => !!value, {
      message: 'Phone number is required',
   })
   .refine((value) => /^0[3-9][0-9]{8}$/.test(value), {
      message: 'Invalid vietnamese phone number',
   })
   .refine((value) => value.startsWith('0'), {
      message: 'Phone number must start with 0',
   })
   .refine((value) => value.length <= 10, {
      message: 'Phone number must be at least 10 digits',
   });

const RegisterSchema = z.object({
   email: z.string().email({
      message: 'Please enter a valid email',
   }),
   password: z.string().min(1, {
      message: 'Password is required',
   }),
   confirm_password: z.string().min(1, {
      message: 'Confirm Password is required',
   }),
   first_name: z.string().min(1, {
      message: 'First Name is required',
   }),
   last_name: z.string().min(1, {
      message: 'Last Name is required',
   }),
   phone_number: vietnamesePhoneSchema,
   birth_day: birthdaySchema,
   country: z.string().min(1, {
      message: 'Country is required',
   }),
} satisfies Record<keyof IRegisterPayload, any>);

export type RegisterFormType = z.infer<typeof RegisterSchema>;
export const RegisterResolver = zodResolver(RegisterSchema);

////////////////

// OTP Schema
const OtpSchema = z.object({
   email: z.string().min(1, {
      message: 'email is required',
   }),
   token: z.string().min(1, {
      message: 'Token is required',
   }),
   otp: z.string().min(1, {
      message: 'OTP is required',
   }),
} satisfies Record<keyof IOtpPayload, any>);

export type OtpFormType = z.infer<typeof OtpSchema>;
export const OtpResolver = zodResolver(OtpSchema);

////////////////

// send email reset password schema
const sendEmailResetPasswordSchema = z.object({
   email: z.string().min(1, {
      message: 'email is required',
   }),
} satisfies Record<keyof ISendEmailResetPasswordPayload, any>);

export type sendEmailResetPasswordFormType = z.infer<
   typeof sendEmailResetPasswordSchema
>;
export const sendEmailResetPasswordResolver = zodResolver(
   sendEmailResetPasswordSchema,
);

////////////////
// reset password schema
const resetPasswordSchema = z.object({
   email: z.string().min(1, {
      message: 'email is required',
   }),
   token: z.string().min(1, {
      message: 'Token is required',
   }),
   new_password: z.string().min(1, {
      message: 'New password is required',
   }),
   confirm_password: z.string().min(1, {
      message: 'Confirm password is required',
   }),
} satisfies Record<keyof IResetPasswordPayload, any>);

export type resetPasswordFormType = z.infer<typeof resetPasswordSchema>;
export const resetPasswordResolver = zodResolver(resetPasswordSchema);

////////////////
// change password schema
const changePasswordSchema = z.object({
   old_password: z.string().min(1, {
      message: 'Old password is required',
   }),
   new_password: z.string().min(1, {
      message: 'New password is required',
   }),
} satisfies Record<keyof IChangePasswordPayload, any>);

export type changePasswordFormType = z.infer<typeof changePasswordSchema>;
export const changePasswordResolver = zodResolver(changePasswordSchema);
