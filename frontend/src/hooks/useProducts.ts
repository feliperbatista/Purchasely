import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import type { CreateProductRequest } from '../types/product';
import { productsApi } from '../api/products';
import { useState } from 'react';

export function useProducts() {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);

  const query = useQuery({
    queryKey: ['products', page],
    queryFn: () => productsApi.getAll(page),
  });

  const create = useMutation({
    mutationFn: (data: CreateProductRequest) => productsApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['products'] });
    },
  });

  const update = useMutation({
    mutationFn: ({ id, data }: { id: string; data: CreateProductRequest }) =>
      productsApi.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['products'] });
    },
  });

  const remove = useMutation({
    mutationFn: (id: string) => productsApi.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['products'] });
    },
  });

  return {
    products: query.data?.items ?? [],
    totalCount: query.data?.totalCount ?? 0,
    totalPages: query.data?.totalPages ?? 0,
    currentPage: query.data?.currentPage ?? 1,
    hasPrevious: query.data?.hasPrevious ?? false,
    hasNext: query.data?.hasNext ?? false,
    isLoading: query.isLoading,
    page,
    setPage,
    create,
    update,
    remove,
  };
}
