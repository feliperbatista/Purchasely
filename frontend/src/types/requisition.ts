export type RequisitionStatus =
  | "Draft"
  | "Submitted"
  | "Approved"
  | "Rejected"
  | "ConvertedToPO";

export type Priority = "Low" | "Normal" | "High";

export interface RequisitionLine {
  id: string;
  productId: string;
  productName: string;
  quantityRequested: number;
  estimatedUnitPrice: number;
}

export interface Requisition {
  id: string;
  number: number;
  status: RequisitionStatus;
  priority: Priority;
  justification?: string;
  createdAt: string;
  submittedAt?: string;
  requesterId: string;
  requesterName: string;
  lines: RequisitionLine[];
}

export interface CreateRequisitionRequest {
  priority: Priority;
  justification?: string;
  lines: {
    productId: string;
    quantityRequested: number;
    estimatedUnitPrice: number;
  }[];
}