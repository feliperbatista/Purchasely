import { useState } from 'react';
import type {
  CreateRequisitionRequest,
  RequisitionFilters,
} from '../types/requisition';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { requisitionsApi } from '../api/requisitions';

export function useRequisitions() {
  const queryClient = useQueryClient();
  const [filters, setFilters] = useState<RequisitionFilters>({ page: 1 });

  const query = useQuery({
    queryKey: ['requisitions', filters],
    queryFn: () => requisitionsApi.getAll(filters),
  });

  const create = useMutation({
    mutationFn: (data: CreateRequisitionRequest) =>
      requisitionsApi.create(data),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['requisitions'] }),
  });

  const submit = useMutation({
    mutationFn: (id: string) => requisitionsApi.submit(id),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['requisitions'] }),
  });

  const setFilter = <K extends keyof RequisitionFilters>(
    key: K,
    value: RequisitionFilters[K],
  ) => {
    setFilters((prev) => ({ ...prev, [key]: value, page: 1 }));
  };

  const clearFilters = () => setFilters({ page: 1 });

  return {
    requistions: query.data?.items ?? [],
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
    create,
    submit,
  };
}
