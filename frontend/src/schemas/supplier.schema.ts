import { z } from 'zod';

export const supplierSchema = z.object({
  name: z
    .string()
    .min(1, 'Name is required')
    .max(200, 'Name must have less than 200 characters'),
  email: z
    .email()
    .min(1, 'Email is required')
    .max(200, 'Email must have less than 200 characters'),
  phone: z
    .string()
    .min(1, 'Phone is required')
    .max(30, 'Phone must have less than 30 characters'),
  address: z
    .string()
    .min(1, 'Address is required')
    .max(500, 'Address must have less than 500 characters'),
  taxNumber: z
    .string()
    .regex(/^\d{14}$/, 'Tax number must contain exactly 14 digits'),
});

export type SupplierFormData = z.infer<typeof supplierSchema>;
