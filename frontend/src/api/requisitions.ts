import api from '../lib/axios';
import type { PagedResult } from '../types/common';
import type {
  ConvertToPORequest,
  CreatePurchaseOrderResponse,
} from '../types/purchaseOrder';
import type {
  CreateRequisitionRequest,
  Requisition,
  RequisitionFilters,
} from '../types/requisition';

export const requisitionsApi = {
  getAll: async (
    filters: RequisitionFilters = {},
  ): Promise<PagedResult<Requisition>> => {
    const params = new URLSearchParams();
    if (filters.status) params.set('status', filters.status);
    if (filters.from) params.set('from', filters.from);
    if (filters.to) params.set('to', filters.to);
    if (filters.myRequisitions) params.set('myRequisitions', 'true');
    params.set('page', String(filters.page ?? 1));
    params.set('pageSize', String(filters.pageSize ?? 12));

    const res = await api.get(`/api/requisition?${params}`);
    return res.data;
  },

  getById: async (id: string): Promise<Requisition> => {
    const res = await api.get(`/api/requisition/${id}`);
    return res.data;
  },

  create: async (data: CreateRequisitionRequest): Promise<Requisition> => {
    const res = await api.post('/api/requisition', data);
    return res.data;
  },

  submit: async (id: string): Promise<void> => {
    await api.post(`/api/requisition/${id}/submit`);
  },

  approve: async (id: string): Promise<void> => {
    await api.post(`/api/requisition/${id}/approve`);
  },

  removeApproval: async (id: string): Promise<void> => {
    await api.post(`/api/requisition/${id}/remove-approval`);
  },

  reject: async (id: string, reason: string): Promise<void> => {
    await api.post(`/api/requisition/${id}/reject`, { reason });
  },

  convertToPO: async (
    data: ConvertToPORequest,
  ): Promise<CreatePurchaseOrderResponse> => {
    const res = await api.post(
      `/api/requisition/${data.requisitionId}/convert-to-po`,
      data.lines,
    );
    return res.data;
  },

  update: async (
    id: string,
    data: CreateRequisitionRequest,
  ): Promise<Requisition> => {
    const res = await api.put(`/api/requisition/${id}`, data);
    return res.data;
  },
};
