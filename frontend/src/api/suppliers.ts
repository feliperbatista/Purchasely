import api from '../lib/axios';
import type { PagedResult } from '../types/common';
import type {
  CreateSupplierProductRequest,
  CreateSupplierRequest,
  Supplier,
  SupplierDetails,
} from '../types/supplier';

export const suppliersApi = {
  getAll: async (page = 1, pageSize = 10): Promise<PagedResult<Supplier>> => {
    const res = await api.get(
      `/api/suppliers?page=${page}&pageSize=${pageSize}`,
    );
    return res.data;
  },

  getOne: async (id?: string): Promise<SupplierDetails> => {
    const res = await api.get(`/api/suppliers/${id}`);
    return res.data;
  },

  create: async (data: CreateSupplierRequest): Promise<SupplierDetails> => {
    const res = await api.post(`/api/suppliers`, data);
    return res.data;
  },

  update: async (data: CreateSupplierRequest, id?: string): Promise<void> => {
    await api.put(`/api/suppliers/${id}`, data);
  },

  delete: async (id?: string): Promise<void> => {
    await api.delete(`/api/suppliers/${id}`);
  },

  createProduct: async (
    data: CreateSupplierProductRequest,
    id?: string,
  ): Promise<void> => {
    await api.post(`/api/suppliers/${id}/products`, data);
  },

  deleteProduct: async (productId: string, id?: string): Promise<void> => {
    await api.delete(`/api/suppliers/${id}/products/${productId}`);
  },
};
