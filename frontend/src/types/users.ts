export type UserRole = 'Admin' | 'Buyer' | 'Manager' | 'Requester';

export interface User {
  id: string;
  name: string;
  email: string;
  role: UserRole;
  createdAt: string;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  role: UserRole;
}

export interface ChangeRoleRequest {
  role: UserRole;
}
