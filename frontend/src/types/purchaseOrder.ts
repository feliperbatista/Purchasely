export type PurchaseOrderStatus =
  | "Draft"
  | "Issued"
  | "PartiallyReceived"
  | "Received"
  | "Closed"
  | "Cancelled";

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
  status: PurchaseOrderStatus;
  subtotal: number;
  taxAmount: number;
  totalAmount: number;
  createdAt: string;
  issuedAt?: string;
  lines: PurchaseOrderLine[];
}