import * as z from 'zod';

const envConfig = () => {
   const clientEnvSchema = z.object({
      NEXT_PUBLIC_API_URL: z
         .string()
         .optional()
         .default('http://localhost:3001/api'),
      NEXT_PUBLIC_CLIENT_URL: z
         .string()
         .optional()
         .default('http://localhost:3000'),
   });

   const serverEnvSchema = z.object({});

   const validClientEnvSchema = clientEnvSchema.safeParse({
      NEXT_PUBLIC_API_URL: process.env.NEXT_PUBLIC_API_URL,
      NEXT_PUBLIC_CLIENT_URL: process.env.NEXT_PUBLIC_CLIENT_URL,
   });

   const validServerEnvSchema = serverEnvSchema.safeParse({});

   if (!validClientEnvSchema.success || !validServerEnvSchema.success) {
      if (!validClientEnvSchema.success) {
         console.error(validClientEnvSchema.error.errors);
      }

      if (!validServerEnvSchema.success) {
         console.error(validServerEnvSchema.error.errors);
      }

      throw new Error('Invalid environment variables');
   }

   return {
      client: validClientEnvSchema.data,
      server: validServerEnvSchema.data,
   };
};

export default envConfig();
