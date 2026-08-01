import { useNavigate } from 'react-router-dom';
import type { Notification } from '../../types/notification';
import { useEffect, useRef, useState } from 'react';
import { useNotifications } from '../../hooks/useNotifications';
import { Bell, CheckCheck, Trash2, X } from 'lucide-react';

type NotificationItemProps = {
  notification: Notification;
  onRead: (id: string) => void;
  onNavigate: (notification: Notification) => void;
};

function NotificationItem({
  notification,
  onRead,
  onNavigate,
}: NotificationItemProps) {
  const typeStyles = {
    success: 'bg-green-500',
    error: 'bg-red-500',
    warning: 'bg-yellow-500',
    info: 'bg-orange-500',
  };

  return (
    <div
      onClick={() => {
        onRead(notification.id);
        onNavigate(notification);
      }}
      className={`flex items-start gap-3 px-4 py-3 cursor-pointer hover:bg-gray-50 transition
        ${!notification.read ? 'bg-orange-50/50' : ''}`}
    >
      <div
        className={`w-2 h-2 rounded-full mt-1.5 shrink-0 ${typeStyles[notification.type]}`}
      />
      <div className='flex-1 min-w-0'>
        <p
          className={`text-sm ${!notification.read ? 'font-semibold text-gray-900' : 'font-medium text-gray-700'}`}
        >
          {notification.title}
        </p>
        <p className='text-xs text-gray-500 mt-0.5 line-clamp-2'>
          {notification.message}
        </p>
        <p className='text-xs text-gray-400 mt-1'>
          {new Date(notification.createdAt).toLocaleDateString([], {
            hour: '2-digit',
            minute: '2-digit'
          })}
        </p>
      </div>
      {!notification.read && (
        <div className='w-2 h-2 rounded-full bg-orange-500 shrink-0 mt-1.5' />
      )}
    </div>
  );
}

export default function NotificationBell() {
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  const { notifications, unreadCount, markAsRead, markAllAsRead, clearAll } =
    useNotifications();

  useEffect(() => {
    const handler = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    };

    document.addEventListener('mousedown', handler);
    return () => document.removeEventListener('mousedown', handler);
  }, []);

  const handleNavigate = (notification: Notification) => {
    setOpen(false);
    if (!notification.entityId || !notification.entityType) return;
    if (notification.entityType === 'Requisition')
      navigate(`/requisitions/${notification.entityId}`);
    else if (notification.entityType === 'PurchaseOrder')
      navigate(`purchase-orders/${notification.entityId}`);
  };

  return (
    <div ref={ref} className='relative'>
      <button
        onClick={() => setOpen((o) => !o)}
        className='relative p-2 rounded-lg text-gray-500 hover:bg-gray-100 hover:text-gray-900 transition'
      >
        <Bell className='w-5 h-5' />
        {unreadCount > 0 && (
          <span className='absolute top-1 ring-1 w-4 h-4 bg-orange-500 text-white text-[10px] font-bold rounded-full flex items-center justify-center'>
            {unreadCount > 9 ? '9+' : unreadCount}
          </span>
        )}
      </button>

      {open && (
        <div className='absolute right-0 top-10 w-80 bg-white border border-gray-200 rounded-xl shadow-xl z-50 overflow-hidden'>
          <div className='flex items-center justify-between px-4 py-3 border-b border-gray-100'>
            <div className='flex items-center gap-2'>
              <p className='text-sm font-semibold text-gray-900'>
                Notifications
              </p>
              {unreadCount > 0 && (
                <span className='px-1.5 py-0.5 bg-orange-100 text-orange-600 text-xs font-medium rounded-full'>
                  {unreadCount}
                </span>
              )}
            </div>
            <div className='flex items-center gap-1'>
              {unreadCount > 0 && (
                <button
                  onClick={markAllAsRead}
                  className='p-1.5 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition'
                  title='Mark all as read'
                >
                  <CheckCheck className='w-4 h-4' />
                </button>
              )}
              {notifications.length > 0 && (
                <button
                  onClick={clearAll}
                  className='p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition'
                  title='Clear all'
                >
                  <Trash2 className='w-4 h-4' />
                </button>
              )}
              <button
                onClick={() => setOpen(false)}
                className='p-1.5 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition'
              >
                <X className='w-4 h-4' />
              </button>
            </div>
          </div>

          <div className='max-h-96 overflow-y-auto divide-y divide-gray-50'>
            {!notifications.length ? (
              <div className='flex flex-col items-center justify-center py-12 text-center'>
                <Bell className='w-8 h-8 text-gray-200 mb-2' />
                <p className='text-sm text-gray-400'>No notifications yet</p>
              </div>
            ) : (
              notifications.map((n) => (
                <NotificationItem
                  key={n.id}
                  notification={n}
                  onRead={markAsRead}
                  onNavigate={handleNavigate}
                />
              ))
            )}
          </div>
        </div>
      )}
    </div>
  );
}
