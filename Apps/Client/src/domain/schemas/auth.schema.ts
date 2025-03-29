import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { ILoginPayload } from '~/domain/interfaces/auth/login.interface';

const defaultValues: Partial<LoginFormType> = {
   email: '',
   password: '',
};

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
