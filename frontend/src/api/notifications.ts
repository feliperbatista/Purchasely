import api from "../lib/axios";
import type { Notification } from "../types/notification";

export const notificationsApi = {
  getAll: async (): Promise<Notification[]> => {
    const res = await api.get("/api/notifications");
    return res.data;
  },
  markAsRead: async (id: string): Promise<void> => {
    await api.patch(`/api/notifications/${id}/read`);
  },
  markAllAsRead: async (): Promise<void> => {
    await api.patch("/api/notifications/read-all");
  },
};