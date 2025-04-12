import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { IAddressPayload } from '../interfaces/identity/address';

const AddressSchema = z.object({
   contact_name: z.string().min(1, {
      message: 'Name is required',
   }),
   contact_email: z.string().email({
      message: 'Please enter a valid email',
   }),
   contact_label: z.string().min(1, {
      message: 'Label is required',
   }),
   contact_phone_number: z.string().min(1, {
      message: 'Phone number is required',
   }),
   contact_address_line: z.string().min(1, {
      message: 'Address line is required',
   }),
   contact_district: z.string().min(1, {
      message: 'District is required',
   }),
   contact_province: z.string().min(1, {
      message: 'Province is required',
   }),
   contact_country: z.string().min(1, {
      message: 'Country is required',
   }),
} satisfies Record<keyof IAddressPayload, any>);

export type AddressFormType = z.infer<typeof AddressSchema>;

export const AddressResolver = zodResolver(AddressSchema);
