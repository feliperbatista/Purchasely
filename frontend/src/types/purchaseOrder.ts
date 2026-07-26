import type { Status } from './status';

export interface PurchaseOrderLine {
  id: string;
  productId: string;
  productName: string;
  quantityOrdered: number;
  quantityReceived: number;
  unitPrice: number;
  lineTotal: number;
}

export interface PurchaseOrder {
  id: string;
  poNumber: string;
  supplierId: string;
  supplierName: string;
  requisitionId: string;
  status: Status;
  subtotal: number;
  taxAmount: number;
  totalAmount: number;
  createdAt: string;
  issuedAt?: string;
  lines: PurchaseOrderLine[];
}

export interface CreatePOLineRequest {
  requisitionLineId: string;
  supplierId: string;
  unitPrice: number;
}

export interface ConvertToPORequest {
  requisitionId: string;
  lines: CreatePOLineRequest[];
}

export interface CreatePurchaseOrderResponse {
  requisitionId: string;
  purchaseOrders: PurchaseOrder[];
}

export interface PurchaseOrderFilters {
  status?: Status;
  supplierId?: string;
  from?: string;
  to?: string;
  page?: number;
  pageSize?: number;
}
