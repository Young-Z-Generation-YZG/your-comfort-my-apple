import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { ILoginPayload } from "~/src/domain/interfaces/auth/ILoginPayload";

// Define the login schema using zod
const loginSchema = z.object({
  email: z.string().email({
    message: "Please enter a valid email",
  }),
  password: z.string().min(1, {
    message: "Password is required",
  }),
} satisfies Record<keyof ILoginPayload, any>);

export type LoginFormType = z.infer<typeof loginSchema>;

export const loginResolver = zodResolver(loginSchema);
