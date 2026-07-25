export interface Product {
  id: string;
  name: string;
  sku: string;
  description?: string;
  category: string;
  unitPrice?: number;
  createdAt: string;
}

export interface CreateProductRequest {
  name: string;
  sku: string;
  description?: string;
  category: string;
}

export interface SupplierForProduct {
  supplierId: string;
  supplierName: string;
  unitPrice: number;
}

