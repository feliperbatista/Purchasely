import { Bell } from 'lucide-react';
import { useLocation } from 'react-router-dom';

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

      <button className='relative p-2 rounded-lg text-gray-500 hover:bg-gray-50 hover:text-gray-900 transition'>
        <Bell className='w-5 h-5' />
        <span className='absolute top-1.5 right-1.5 w-2 h-2 bg-red-50 rounded-full' />
      </button>
    </header>
  );
}
