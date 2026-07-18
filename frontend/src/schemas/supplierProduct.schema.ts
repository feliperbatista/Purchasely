import { z } from 'zod';

export const supplierProductSchema = z.object({
  productId: z.string().min(1, 'Product is required'),
  unitPrice: z.number().positive('Price must be grater than 0'),
});

export type SupplierProductFormData = z.infer<typeof supplierProductSchema>;
