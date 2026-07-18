import type { Product } from './product';

export interface Supplier {
  id: string;
  name: string;
  email: string;
  phone: string;
}

export interface SupplierDetails {
  id: string;
  name: string;
  email: string;
  phone: string;
  taxNumber: string;
  address: string;
  createdAt: string;
  products: Product[];
}

export interface CreateSupplierRequest {
  name: string;
  email: string;
  phone: string;
  taxNumber: string;
  address: string;
}

export interface CreateSupplierProductRequest {
  productId: string;
  unitPrice: number
}
