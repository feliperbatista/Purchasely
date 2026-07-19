import api from '../lib/axios';
import type { PagedResult } from '../types/common';
import type { Requisition, RequisitionFilters } from '../types/requisition';

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
    const res = await api.get(`/api/requisiton/${id}`);
    return res.data;
  },
};
