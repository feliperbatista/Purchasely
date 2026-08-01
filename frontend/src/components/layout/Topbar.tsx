import { useLocation } from 'react-router-dom';
import NotificationBell from './NotificationBell';

const titles: Record<string, string> = {
  '/dashboard': 'Dashboard',
  '/requisitions': 'Requisitions',
  '/purchase-orders': 'Purchase Orders',
  '/products': 'Products',
  '/suppliers': 'Suppliers',
  '/users': 'Users',
};

export default function Topbar() {
  const { pathname } = useLocation();

  const title =
    Object.entries(titles).find(([path]) => pathname.startsWith(path))?.[1] ??
    'Purchasely';

  return (
    <header className='h-14 border-b border-gray-100 bg-white flex items-center justify-between px-6'>
      <h2 className='text-sm font-semibold text-gray-900'>{title}</h2>

      <NotificationBell/>
    </header>
  );
}
