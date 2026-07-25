import { z } from 'zod';

export const requisitionHeaderSchema = z.object({
  priority: z.enum(['Low', 'Normal', 'High']),
  justification: z.string().optional(),
});

export const requisitionLineSchema = z.object({
  productId: z.string().min(1, 'Product is required'),
  productName: z.string(),
  quantityRequested: z.number().positive('Quantity must be grater than 0'),
  estimatedUnitPrice: z.number().positive('Price must be grater than 0'),
});

export const createRequisitionSchema = z.object({
  priority: z.enum(['Low', 'Normal', 'High']),
  justification: z.string().optional(),
  lines: z.array(requisitionLineSchema).min(1, 'At least one line is required'),
});

export type RequisitionHeaderFormData = z.infer<typeof requisitionHeaderSchema>;
export type RequisitionLineFormData = z.infer<typeof requisitionLineSchema>;
export type CreateRequisitionFormData = z.infer<typeof createRequisitionSchema>;
