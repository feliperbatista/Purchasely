import api from '../lib/axios';
import type { PagedResult } from '../types/common';
import type { ChangeRoleRequest, CreateUserRequest, User } from '../types/users';

export const usersApi = {
  getAll: async (page = 1, pageSize = 10): Promise<PagedResult<User>> => {
    const res = await api.get(`/api/users?page=${page}&pageSize=${pageSize}`);
    return res.data;
  },

  create: async (data: CreateUserRequest): Promise<User> => {
    const res = await api.post('/api/users', data);
    return res.data;
  },

  changeRole: async (id: string, data: ChangeRoleRequest): Promise<void> => {
    await api.patch(`/api/users/${id}/role`, data);
  },
};
