import { useCallback, useEffect, useState } from 'react';
import { useAuth } from './useAuth';
import type { Notification } from '../types/notification';
import { toast } from 'sonner';
import { getConnection, startConnection, stopConnection } from '../lib/signalr';

export function useNotifications() {
  const { user } = useAuth();
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [connected, setConnected] = useState(false);

  const addNotification = useCallback(
    (payload: Omit<Notification, 'id' | 'createdAt' | 'read'>) => {
      const notification: Notification = {
        ...payload,
        id: crypto.randomUUID(),
        createdAt: new Date().toISOString(),
        read: false,
      };

      setNotifications((prev) => [notification, ...prev]);

      const toastFn =
        {
          sucess: toast.success,
          error: toast.error,
          warning: toast.warning,
          info: toast.info,
        }[payload.type] ?? toast.info;

      toastFn(payload.title, { description: payload.message });
    },
    [],
  );

  useEffect(() => {
    if (!user) return;

    let mounted = true;

    const connect = async () => {
      try {
        const conn = await startConnection();
        if (!mounted) return;

        setConnected(true);

        conn.on('ReceiveNotification', (payload) => {
          if (!mounted) return;
          addNotification(payload);
        });
      } catch (err) {
        console.error('SignalR connection failed:', err);
      }
    };

    connect();

    return () => {
      mounted = false;
      const conn = getConnection();
      conn.off('ReceiveNotification');
      stopConnection();
      setConnected(false);
    };
  }, [user, addNotification]);

  const markAsRead = useCallback((id: string) => {
    setNotifications((prev) =>
      prev.map((n) => (n.id === id ? { ...n, read: true } : n)),
    );
  }, []);

  const markAllAsRead = useCallback(() => {
    setNotifications((prev) => prev.map((n) => ({ ...n, read: true })));
  }, []);

  const clearAll = useCallback(() => setNotifications([]), []);

  const unreadCount = notifications.filter((n) => !n.read).length;

  return {
    notifications,
    unreadCount,
    connected,
    markAsRead,
    markAllAsRead,
    clearAll,
  };
}
