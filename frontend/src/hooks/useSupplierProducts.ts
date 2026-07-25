import { useQuery } from '@tanstack/react-query';
import type { SupplierForProduct } from '../types/product';
import api from '../lib/axios';

export function useSupplierProducts(productId: string) {
  return useQuery({
    queryKey: ['supplier-products', productId],
    queryFn: async (): Promise<SupplierForProduct[]> => {
      const res = await api.get(`/api/products/${productId}/suppliers`);
      return res.data;
    },
    enabled: !!productId,
  });
}
