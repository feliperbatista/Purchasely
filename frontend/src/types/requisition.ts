import type { Status } from './status';

export type Priority = 'Low' | 'Normal' | 'High';

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
  status: Status;
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

export interface RequisitionFilters {
  status?: Status;
  from?: string;
  to?: string;
  myRequisitions?: boolean;
  page?: number;
  pageSize?: number;
}
