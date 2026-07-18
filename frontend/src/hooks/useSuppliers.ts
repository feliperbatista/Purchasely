import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useState } from 'react';
import { suppliersApi } from '../api/suppliers';
import type {
  CreateSupplierProductRequest,
  CreateSupplierRequest,
} from '../types/supplier';

export function useSuppliers(id?: string) {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const invalidateQueries = queryClient.invalidateQueries({
    queryKey: ['suppliers'],
  });

  const queryAll = useQuery({
    queryKey: ['suppliers', page],
    queryFn: () => suppliersApi.getAll(page),
  });

  const queryOne = useQuery({
    queryKey: ['suppliers', id],
    queryFn: () => suppliersApi.getOne(id),
    enabled: !!id,
  });

  const create = useMutation({
    mutationFn: (data: CreateSupplierRequest) => suppliersApi.create(data),
    onSuccess: () => invalidateQueries,
  });

  const update = useMutation({
    mutationFn: (data: CreateSupplierRequest) => suppliersApi.update(data, id),
    onSuccess: () => invalidateQueries,
  });

  const remove = useMutation({
    mutationFn: (id: string) => suppliersApi.delete(id),
    onSuccess: () => invalidateQueries,
  });

  const addProduct = useMutation({
    mutationFn: (data: CreateSupplierProductRequest) =>
      suppliersApi.createProduct(data, id),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['suppliers', id] }),
  });

  const removeProduct = useMutation({
    mutationFn: (productId: string) =>
      suppliersApi.deleteProduct(productId, id),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['suppliers', id] }),
  });

  return {
    suppliers: queryAll.data?.items ?? [],
    totalCount: queryAll.data?.totalCount ?? 0,
    totalPages: queryAll.data?.totalPages ?? 0,
    currentPage: queryAll.data?.currentPage ?? 1,
    hasPrevious: queryAll.data?.hasPrevious ?? false,
    hasNext: queryAll.data?.hasNext ?? false,
    isLoadingAll: queryAll.isLoading,
    isLoadingOne: queryOne.isLoading,
    page,
    setPage,
    queryAll,
    supplier: queryOne.data,
    create,
    update,
    remove,
    addProduct,
    removeProduct,
  };
}
