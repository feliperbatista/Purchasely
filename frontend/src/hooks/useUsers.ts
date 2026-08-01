import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useState } from 'react';
import { usersApi } from '../api/users';
import type { CreateUserRequest, UserRole } from '../types/users';

export function useUsers() {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);

  const query = useQuery({
    queryKey: ['users', page],
    queryFn: () => usersApi.getAll(page),
  });

  const create = useMutation({
    mutationFn: (data: CreateUserRequest) => usersApi.create(data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
  });

  const changeRole = useMutation({
    mutationFn: ({ id, role }: { id: string; role: UserRole }) =>
      usersApi.changeRole(id, { role }),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
  });

  return {
    users: query.data?.items ?? [],
    totalCount: query.data?.totalCount ?? 0,
    totalPages: query.data?.totalPages ?? 0,
    currentPage: query.data?.currentPage ?? 1,
    hasNext: query.data?.hasNext ?? false,
    hasPrevious: query.data?.hasPrevious ?? false,
    isLoading: query.isLoading,
    page,
    setPage,
    create,
    changeRole,
  };
}
