import { useNavigate } from 'react-router-dom';
import { useDashboard } from '../hooks/useDashboard';
import Skeleton from '../components/common/Skeleton';
import StatCard from '../components/dashboard/StatCard';
import { ClipboardList, Clock, DollarSign, ShoppingCart } from 'lucide-react';
import SectionHeader from '../components/dashboard/SectionHeader';
import type { Requisition } from '../types/requisition';
import StatusBadge from '../components/dashboard/StatusBadge';
import type { PurchaseOrder } from '../types/purchaseOrder';

export default function DashboardPage() {
  const navigate = useNavigate();

  const {
    stats,
    statsLoading,
    requisitions,
    reqLoading,
    purchaseOrders,
    poLoading,
  } = useDashboard();

  return (
    <div className='space-y-6'>
      <div className='grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-4'>
        {statsLoading ? (
          Array.from({ length: 4 }).map((_, i) => (
            <Skeleton key={i} className='h-24 rounded-xl' />
          ))
        ) : (
          <>
            <StatCard
              label='Total Requisitions'
              value={stats?.totalRequisitions ?? 0}
              icon={<ClipboardList className='w-5 h-5 text-blue-600' />}
              color='bg-blue-50'
            />
            <StatCard
              label='Pending Approvals'
              value={stats?.pendingApprovals ?? 0}
              icon={<Clock className='w-5 h-5 text-yellow-600' />}
              color='bg-yellow-50'
            />
            <StatCard
              label='Open Purchase Orders'
              value={stats?.openPurchaseOrders ?? 0}
              icon={<ShoppingCart className='w-5 h-5 text-purple-600' />}
              color='bg-purple-50'
            />
            <StatCard
              label='Total Spend This Month'
              value={`$${(stats?.totalSpendThisMonth ?? 0).toLocaleString()}`}
              icon={<DollarSign className='w-5 h-5 text-green-600' />}
              color='bg-green-50'
            />
          </>
        )}
      </div>

      <div className='grid grid-cols-1 xl:grid-cols-2 gap-6'>
        <div className='bg-white rounded-xl border border-gray-100 p-5'>
          <SectionHeader title='Recent Requisitions' to='/reqisitions' />
          {reqLoading ? (
            <div className='space-y-3'>
              {Array.from({ length: 5 }).map((_, i) => (
                <Skeleton key={i} className='h-10' />
              ))}
            </div>
          ) : !requisitions?.length ? (
            <p className='text-sm text-gray-400 text-center py-8'>
              No requisitions yet
            </p>
          ) : (
            <table className='w-full'>
              <thead>
                <tr className='border-b border-gray-50'>
                  <th className='text-left text-xs font-medium text-gray-400 pb-2'>
                    #
                  </th>
                  <th className='text-left text-xs font-medium text-gray-400 pb-2'>
                    Requester
                  </th>
                  <th className='text-left text-xs font-medium text-gray-400 pb-2'>
                    Priority
                  </th>
                  <th className='text-left text-xs font-medium text-gray-400 pb-2'>
                    Status
                  </th>
                </tr>
              </thead>
              <tbody className='divide-y divide-gray-50'>
                {requisitions.map((req: Requisition) => (
                  <tr
                    key={req.id}
                    onClick={() => navigate(`/requisitions/${req.id}`)}
                    className='cursor-pointer hover:bg-gray-50 transition'
                  >
                    <td className='py-4 text-sm font-medium text-gray-900'>
                      #{req.number}
                    </td>
                    <td className='py-4 text-sm font-medium text-gray-600'>
                      {req.requesterName}
                    </td>
                    <td className='py-3'>
                      <span
                        className={`text-xs font-medium ${
                          req.priority === 'High'
                            ? 'text-red-600'
                            : req.priority === 'Normal'
                              ? 'text-yellow-600'
                              : 'text-gray-500'
                        }`}
                      >
                        {req.priority}
                      </span>
                    </td>
                    <td className='py-3'>
                      <StatusBadge status={req.status} />
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>

        <div className='bg-white rounded-xl border border-gray-100 p-5'>
          <SectionHeader title='Recent Purchase Orders' to='/purchase-orders' />
          {poLoading ? (
            <div className='space-y-3'>
              {Array.from({ length: 5 }).map((_, i) => (
                <Skeleton key={i} className='h-10' />
              ))}
            </div>
          ) : !purchaseOrders?.length ? (
            <p className='text-sm text-gray-400 text-center py-8'>
              No purchase orders yet
            </p>
          ) : (
            <table className='w-full'>
              <thead>
                <tr className='border-b border-gray-50'>
                  <th className='text-left text-xs font-medium text-gray-400 pb-2'>
                    PO Number
                  </th>
                  <th className='text-left text-xs font-medium text-gray-400 pb-2'>
                    Supplier
                  </th>
                  <th className='text-left text-xs font-medium text-gray-400 pb-2'>
                    Total
                  </th>
                  <th className='text-left text-xs font-medium text-gray-400 pb-2'>
                    Status
                  </th>
                </tr>
              </thead>
              <tbody className='divide-y divide-gray-50'>
                {purchaseOrders.map((po: PurchaseOrder) => (
                  <tr
                    key={po.id}
                    onClick={() => navigate(`/purchase-orders/${po.id}`)}
                    className='cursor-pointer hover:bg-gray-50 transition'
                  >
                    <td className='py-4 text-sm font-medium text-gray-900'>
                      #{po.poNumber}
                    </td>
                    <td className='py-4 text-sm font-medium text-gray-600'>
                      {po.supplierName}
                    </td>
                    <td className='py-3 text-sm font-medium text-gray-900'>
                      {po.totalAmount.toLocaleString()}
                    </td>
                    <td className='py-3'>
                      <StatusBadge status={po.status} />
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </div>
  );
}
