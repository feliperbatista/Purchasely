import { useState } from 'react';
import type {
  CreateRequisitionRequest,
  RequisitionFilters,
} from '../types/requisition';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { requisitionsApi } from '../api/requisitions';
import type { ConvertToPORequest } from '../types/purchaseOrder';

export function useRequisitions(id?: string) {
  const queryClient = useQueryClient();
  const [filters, setFilters] = useState<RequisitionFilters>({ page: 1 });

  const query = useQuery({
    queryKey: ['requisitions', filters],
    queryFn: () => requisitionsApi.getAll(filters),
  });

  const { data: requisition, isLoading: isLoadingRequisition } = useQuery({
    queryKey: ['requisitions', id],
    queryFn: () => requisitionsApi.getById(id!),
    enabled: !!id,
  });

  const create = useMutation({
    mutationFn: (data: CreateRequisitionRequest) =>
      requisitionsApi.create(data),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['requisitions'] }),
  });

  const submit = useMutation({
    mutationFn: (requisitionId: string) =>
      requisitionsApi.submit(requisitionId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['requisitions'] });
      queryClient.invalidateQueries({ queryKey: ['requisitions', id] });
    },
  });

  const approve = useMutation({
    mutationFn: (requisitionId: string) =>
      requisitionsApi.approve(requisitionId),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['requisitions', id] }),
  });

  const reject = useMutation({
    mutationFn: ({
      requisitionId,
      reason,
    }: {
      requisitionId: string;
      reason: string;
    }) => requisitionsApi.reject(requisitionId, reason),
     onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['requisitions'] });
      queryClient.invalidateQueries({ queryKey: ['requisitions', id] });
    },
  });

  const removeApproval = useMutation({
    mutationFn: (requisitionId: string) =>
      requisitionsApi.removeApproval(requisitionId),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['requisitions', id] }),
  });

  const convertToPO = useMutation({
    mutationFn: (data: ConvertToPORequest) => requisitionsApi.convertToPO(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['requisitions'] });
      queryClient.invalidateQueries({ queryKey: ['requisitions', id] });
      queryClient.invalidateQueries({ queryKey: ['purchase-orders'] });
    },
  });

  const update = useMutation({
    mutationFn: ({
      id,
      data,
    }: {
      id: string;
      data: CreateRequisitionRequest;
    }) => requisitionsApi.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['requisitions'] });
      queryClient.invalidateQueries({ queryKey: ['requisitions', id] });
    },
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
    requisition,
    isLoadingRequisition,
    approve,
    reject,
    removeApproval,
    convertToPO,
    update,
  };
}
