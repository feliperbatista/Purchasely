import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useState } from 'react';
import type { PurchaseOrderFilters } from '../types/purchaseOrder';
import { purchaseOrdersApi } from '../api/purchaseOrder';

export function usePurchaseOrders(id?: string) {
  const queryClient = useQueryClient();
  const [filters, setFilters] = useState<PurchaseOrderFilters>({ page: 1 });

  const query = useQuery({
    queryKey: ['purchase-orders', filters],
    queryFn: () => purchaseOrdersApi.getAll(filters),
    enabled: !id,
  });

  const detail = useQuery({
    queryKey: ['purchase-orders', id],
    queryFn: () => purchaseOrdersApi.getById(id!),
    enabled: !!id,
  });

  const issue = useMutation({
    mutationFn: (poId: string) => purchaseOrdersApi.issue(poId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['purchase-orders'] });
    },
  });

  const receive = useMutation({
    mutationFn: ({ poId, data }: { poId: string; data: FormData }) =>
      purchaseOrdersApi.receive(poId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['purchase-orders', id] });
    },
  });

  const close = useMutation({
    mutationFn: (poId: string) => purchaseOrdersApi.close(poId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['purchase-orders'] });
    },
  });

  const cancel = useMutation({
    mutationFn: ({ poId, reason }: { poId: string; reason: string }) =>
      purchaseOrdersApi.cancel(poId, reason),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['purchase-orders'] });
    },
  });

  const setFilter = <K extends keyof PurchaseOrderFilters>(
    key: K,
    value: PurchaseOrderFilters[K],
  ) => setFilters((prev) => ({ ...prev, [key]: value, page: 1 }));

  const clearFilters = () => setFilters({ page: 1 });

  return {
    purchaseOrders: query.data?.items ?? [],
    totalCount: query.data?.totalCount ?? 0,
    totalPages: query.data?.totalPages ?? 0,
    currentPage: query.data?.currentPage ?? 1,
    hasNext: query.data?.hasNext ?? false,
    hasPrevious: query.data?.hasPrevious ?? false,
    isLoading: query.isLoading,
    filters,
    setFilter,
    clearFilters,
    setPage: (page: number) => setFilters((prev) => ({ ...prev, page })),

    purchaseOrder: detail.data,
    isLoadingDetail: detail.isLoading,

    issue,
    receive,
    close,
    cancel,
  };
}
