import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { IProfilePayload } from '../interfaces/identity/profile';

const ProfileSchema = z.object({
   first_name: z.string().min(1, { message: 'First name is required' }),
   last_name: z.string().min(1, { message: 'Last name is required' }),
   email: z.string().email({ message: 'Invalid email address' }),
   phone_number: z
      .string()
      .min(1, { message: 'Phone number is required' })
      .regex(/^\d+$/, { message: 'Phone number must be numeric' }),
   birth_day: z
      .date({
         required_error: 'A date of birth is required.',
      })
      .transform((date) => {
         // format to YYYY-MM-DD +7
         return new Date(date.getTime() + 7 * 60 * 60 * 1000);
      }),
   gender: z
      .string()
      .min(1, { message: 'Gender is required' })
      .default('OTHER'),
} satisfies Record<keyof IProfilePayload, any>);

export type ProfileFormType = z.infer<typeof ProfileSchema>;

export const ProfileResolver = zodResolver(ProfileSchema);
