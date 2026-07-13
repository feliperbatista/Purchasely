import api from '../lib/axios';
import type { PagedResult } from '../types/common';
import type { DashboardStats } from '../types/dashboard';
import type { PurchaseOrder } from '../types/purchaseOrder';
import type { Requisition } from '../types/requisition';

export const dashboardApi = {
  getStats: async (): Promise<DashboardStats> => {
    const res = await api.get('/api/dashboard/stats');
    return res.data;
  },

  getRecentRequisitions: async (): Promise<Requisition[]> => {
    const res = await api.get<PagedResult<Requisition>>('/api/requisition?page=1&pageSize=5');
    return res.data.items;
  },

  getRecentPurchaseOrders: async (): Promise<PurchaseOrder[]> => {
    const res = await api.get<PagedResult<PurchaseOrder>>('/api/purchaseorders?page=1&pageSize=5');
    return res.data.items;
  },
};
