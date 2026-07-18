import api from '../lib/axios';
import type { PagedResult } from '../types/common';
import type { CreateProductRequest, Product } from '../types/product';

export const productsApi = {
  getAll: async (page = 1, pageSize = 10): Promise<PagedResult<Product>> => {
    const res = await api.get(
      `/api/products?page=${page}&pageSize=${pageSize}`,
    );
    return res.data;
  },

  create: async (data: CreateProductRequest): Promise<Product> => {
    const res = await api.post('/api/products', data);
    return res.data;
  },

  update: async (id: string, data: CreateProductRequest): Promise<void> => {
    await api.put(`/api/products/${id}`, data);
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/api/products/${id}`);
  },
};
