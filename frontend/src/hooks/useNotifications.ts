import { useCallback, useEffect, useState } from 'react';
import { useAuth } from './useAuth';
import type { Notification } from '../types/notification';
import { toast } from 'sonner';
import { getConnection, startConnection, stopConnection } from '../lib/signalr';
import { useMutation, useQuery } from '@tanstack/react-query';
import { notificationsApi } from '../api/notifications';
import { queryClient } from '../lib/queryClient';

export function useNotifications() {
  const { user } = useAuth();
  const [connected, setConnected] = useState(false);

  const { data: notifications = [] } = useQuery({
    queryKey: ['notifications'],
    queryFn: notificationsApi.getAll,
    enabled: !!user,
  });

  const markAsRead = useMutation({
    mutationFn: (id: string) => notificationsApi.markAsRead(id),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['notifications'] }),
  });

  const markAllAsRead = useMutation({
    mutationFn: notificationsApi.markAllAsRead,
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: ['notifications'] }),
  });

  const handleIncoming = useCallback((payload: Notification) => {
    queryClient.setQueryData<Notification[]>(['notifications'], (prev = []) => [
      payload,
      ...prev,
    ]);

    const toastFn =
      {
        success: toast.success,
        error: toast.error,
        warning: toast.warning,
        info: toast.info,
      }[payload.type] ?? toast.info;

    toastFn(payload.title, { description: payload.message });
  }, []);

  useEffect(() => {
    if (!user) return;

    let mounted = true;

    const connect = async () => {
      try {
        const conn = await startConnection();
        if (!mounted) return;

        setConnected(true);

        conn.on('ReceiveNotification', handleIncoming);
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
  }, [user, handleIncoming]);

  const unreadCount = notifications.filter((n) => !n.read).length;

  return {
    notifications,
    unreadCount,
    connected,
    markAsRead: (id: string) => markAsRead.mutate(id),
    markAllAsRead: () => markAllAsRead.mutate(),
  };
}
