import api from "../lib/axios";
import type { LoginRequest, User } from "../types/auth";

export const authApi = {
  login: async (data: LoginRequest): Promise<User> => {
    const res = await api.post("/api/auth/login", data);
    return res.data;
  },
  logout: () => api.post("/api/auth/logout"),
  me: async (): Promise<User> => {
    const res = await api.get("/api/auth/me");
    return res.data;
  },
};