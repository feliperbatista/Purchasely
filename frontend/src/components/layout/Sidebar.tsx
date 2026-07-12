import {
  LayoutDashboard,
  ClipboardList,
  ShoppingCart,
  Package,
  Truck,
  Users,
  LogOut,
} from 'lucide-react';
import { useAuth } from '../../hooks/useAuth';
import { NavLink } from 'react-router-dom';

const links = [
  { to: '/dashboard', label: 'Dashboard', icon: LayoutDashboard },
  { to: '/requisitions', label: 'Requisitions', icon: ClipboardList },
  { to: '/purchase-orders', label: 'Purchase Orders', icon: ShoppingCart },
  { to: '/products', label: 'Products', icon: Package },
  { to: '/suppliers', label: 'Suppliers', icon: Truck },
  { to: '/users', label: 'Users', icon: Users },
];

export default function Sidebar() {
  const { logout, user } = useAuth();

  return (
    <aside className='fixed top-0 left-0 h-screen w-56 bg-white border-r border-gray-100 flex flex-col'>
      <div className='px-5 py-5 border-b border-gray-100'>
        <h1 className='text-lg font-bold text-gray-900'>Purchasely</h1>
        <p className='text-xs text-gray-400 mt-0.5 truncate'>{user?.email}</p>
      </div>

      <nav className='flex-1 px-3 py-4 space-y-0.5 overflow-y-auto'>
        {links.map(({ to, label, icon: Icon }) => (
          <NavLink
            key={to}
            to={to}
            className={({ isActive }) =>
              `flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm transition
          ${isActive ? 'bg-blue-50 text-blue-600 font-medium' : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900'}`
            }
          >
            <Icon className='w-4 h-4 shrink-0' />
            {label}
          </NavLink>
        ))}
      </nav>

      <div className='px-3 py-4 border-t border-gray-100'>
        <div className='px-3 py-2 mb-1'>
          <p className='text-sm font-medium text-gray-900 truncate'>
            {user?.name}
          </p>
          <p className='text-xs text-gray-400 truncate'>{user?.role}</p>
        </div>
        <button
          onClick={() => logout.mutate()}
          className='flex items-center gap-3 w-full px-3 py-2.5 rounded-lg text-sm text-gray-600 hover:bg-red-50 hover:text-red-600 transition'
        >
          <LogOut />
          Sign out
        </button>
      </div>
    </aside>
  );
}
