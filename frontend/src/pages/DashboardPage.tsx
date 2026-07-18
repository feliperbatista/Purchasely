import { useNavigate } from 'react-router-dom';
import { useDashboard } from '../hooks/useDashboard';
import Skeleton from '../components/common/Skeleton';
import StatCard from '../components/dashboard/StatCard';
import { ClipboardList, Clock, DollarSign, ShoppingCart } from 'lucide-react';
import SectionHeader from '../components/dashboard/SectionHeader';
import StatusBadge from '../components/dashboard/StatusBadge';
import Table from '../components/common/Table';

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
              icon={<ClipboardList className='w-5 h-5 text-orange-500' />}
              color='bg-orange-50'
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
          <SectionHeader title='Recent Requisitions' to='/requisitions' />
          <Table
            data={requisitions}
            loading={reqLoading}
            emptyMessage='No requisitions yet'
            getRowKey={(req) => req.id}
            onRowClick={(req) => navigate(`/requisitions/${req.id}`)}
            columns={[
              { header: '#', render: (req) => <>#{req.number}</> },
              { header: 'Requester', render: (req) => req.requesterName },
              {
                header: 'Priority',
                render: (req) => (
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
                ),
              },
              {
                header: 'Status',
                render: (req) => <StatusBadge status={req.status} />,
              },
            ]}
          />
        </div>

        <div className='bg-white rounded-xl border border-gray-100 p-5'>
          <SectionHeader title='Recent Purchase Orders' to='/purchase-orders' />
          <Table
            data={purchaseOrders}
            loading={poLoading}
            emptyMessage='No purchase orders yet'
            getRowKey={(po) => po.id}
            onRowClick={(po) => navigate(`/purchase-orders/${po.id}`)}
            columns={[
              { header: 'PO Number', render: (po) => <>#{po.poNumber}</> },
              { header: 'Supplier', render: (po) => po.supplierName },
              {
                header: 'Total',
                render: (po) => po.totalAmount.toLocaleString(),
              },
              {
                header: 'Status',
                render: (po) => <StatusBadge status={po.status} />,
              },
            ]}
          />
        </div>
      </div>
    </div>
  );
}
