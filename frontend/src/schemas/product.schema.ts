import { z } from 'zod';

export const productSchema = z.object({
  name: z
    .string()
    .min(1, 'Name is required')
    .max(200, 'Name must have less than 200 characters'),
  sku: z
    .string()
    .min(1, 'SKU is required')
    .max(20, 'SKU must have less than 200 characters'),
  description: z
    .string()
    .max(500, 'Description must have less than 500 characters')
    .optional(),
  category: z
    .string()
    .min(1, 'Category is required')
    .max(100, 'Category must have less than 100 characters'),
});

export type ProductFormData = z.infer<typeof productSchema>;
