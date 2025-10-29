import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { ILoginPayload } from '../interfaces/auth/login.interface';

/// Login Schema
const LoginSchema = z.object({
   email: z.string().email({
      message: 'Please enter a valid email',
   }),
   password: z.string().min(1, {
      message: 'Password is required',
   }),
} satisfies Record<keyof any, any>);

export type TLoginForm = z.infer<typeof LoginSchema>;
export const loginResolver = zodResolver(LoginSchema);
