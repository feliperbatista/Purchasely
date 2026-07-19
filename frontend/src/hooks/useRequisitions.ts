import { useState } from 'react';
import type { RequisitionFilters } from '../types/requisition';
import { useQuery } from '@tanstack/react-query';
import { requisitionsApi } from '../api/requisitions';

export function useRequisitions() {
  const [filters, setFilters] = useState<RequisitionFilters>({ page: 1 });

  const query = useQuery({
    queryKey: ['requisitions', filters],
    queryFn: () => requisitionsApi.getAll(filters),
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
  };
}
