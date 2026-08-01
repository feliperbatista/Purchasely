import api from '../lib/axios';
import type { PagedResult } from '../types/common';
import type {
  PurchaseOrder,
  PurchaseOrderFilters,
} from '../types/purchaseOrder';

export const purchaseOrdersApi = {
  getAll: async (
    filters: PurchaseOrderFilters = {},
  ): Promise<PagedResult<PurchaseOrder>> => {
    const params = new URLSearchParams();
    if (filters.status) params.set('status', filters.status);
    if (filters.supplierId) params.set('supplierId', filters.supplierId);
    if (filters.from) params.set('from', filters.from);
    if (filters.to) params.set('to', filters.to);
    params.set('page', String(filters.page ?? 1));
    params.set('pageSize', String(filters.pageSize ?? 10));
    const res = await api.get(`/api/purchaseorders?${params}`);
    return res.data;
  },

  getById: async (id: string): Promise<PurchaseOrder> => {
    const res = await api.get(`/api/purchaseorders/${id}`);
    return res.data;
  },

  issue: async (id: string): Promise<void> => {
    await api.post(`/api/purchaseorders/${id}/issue`);
  },

  receive: async (id: string, data: FormData): Promise<void> => {
    await api.post(`/api/purchaseorders/${id}/receive`, data, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
  },

  close: async (id: string): Promise<void> => {
    await api.post(`/api/purchaseorders/${id}/close`);
  },

  cancel: async (id: string, reason: string): Promise<void> => {
    await api.post(`/api/purchaseorders/${id}/cancel`, {
      reason,
    });
  },
};
