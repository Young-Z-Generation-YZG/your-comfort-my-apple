import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { ILoginPayload } from '~/domain/interfaces/auth/login.interface';
import { IOtpPayload } from '../interfaces/auth/otp.interface';

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

////////////////

// OTP Schema
const OtpSchema = z.object({
   _q: z.string().min(1, {
      message: '_q is required',
   }),
   otp: z.string().min(1, {
      message: 'OTP is required',
   }),
} satisfies Record<keyof IOtpPayload, any>);

export type OtpFormType = z.infer<typeof OtpSchema>;

export const OtpResolver = zodResolver(OtpSchema);
