import { useNavigate } from 'react-router-dom';
import { usePurchaseOrders } from '../../hooks/usePurchaseOrders';
import { useSuppliers } from '../../hooks/useSuppliers';
import { statuses, type Status } from '../../types/status';
import { ShoppingCart, X } from 'lucide-react';
import SkeletonRow from '../../components/purchase-orders/SkeletonRow';
import PurchaseOrderRow from '../../components/purchase-orders/PurchaseOrderRow';
import Pagination from '../../components/common/Pagination';

const STATUSES: Status[] = [
  'Draft',
  'Issued',
  'PartiallyReceived',
  'Received',
  'Closed',
  'Cancelled',
];

export default function PurchaseOrdersPage() {
  const navigate = useNavigate();
  const {
    purchaseOrders,
    totalCount,
    totalPages,
    currentPage,
    hasNext,
    hasPrevious,
    isLoading,
    filters,
    setFilter,
    clearFilters,
    setPage,
  } = usePurchaseOrders();

  const { suppliers } = useSuppliers();

  const hasActiveFilters =
    !!filters.status || !!filters.supplierId || !!filters.from || !!filters.to;

  return (
    <div className='space-y-5'>
      <div className='flex items-center justify-between'>
        <h1 className='text-lg font-semibold text-gray-900'>Purchase Orders</h1>
        <p className='text-sm text-gray-500 mt-0.5'>
          {totalCount} orders total
        </p>
      </div>

      <div className='flex flex-wrap items-center gap-3'>
        <select
          value={filters.status ?? ''}
          onChange={(e) =>
            setFilter('status', (e.target.value as Status) || undefined)
          }
          className='px-3 py-2 text-sm border border-gray-200 rounded-lg bg-white text-gray-900 outline-none focus:ring-orange-400 focus:ring-2'
        >
          <option value=''>All statuses</option>
          {STATUSES.map((s) => (
            <option key={s} value={s}>
              {statuses[s].label}
            </option>
          ))}
        </select>

        <select
          value={filters.supplierId ?? ''}
          onChange={(e) => setFilter('supplierId', e.target.value || undefined)}
          className='px-3 py-2 text-sm border border-gray-200 rounded-lg bg-white text-gray-900 outline-none focus:ring-2 focus:ring-orange-400'
        >
          <option value=''>All suppliers</option>
          {suppliers.map((s) => (
            <option key={s.id} value={s.id}>
              {s.name}
            </option>
          ))}
        </select>

        <input
          type='date'
          value={filters.from ?? ''}
          onChange={(e) => setFilter('from', e.target.value || undefined)}
          className='px-3 py-2 text-sm border border-gray-200 rounded-lg bg-white text-gray-900 outline-none focus:ring-2 focus:ring-orange-400'
        />

        <input
          type='date'
          value={filters.to ?? ''}
          onChange={(e) => setFilter('to', e.target.value || undefined)}
          className='px-3 py-2 text-sm border border-gray-200 rounded-lg bg-white text-gray-900 outline-none focus:ring-2 focus:ring-orange-400'
        />

        {hasActiveFilters && (
          <button
            onClick={clearFilters}
            className='flex items-center gap-1.5 px-3 py-2 text-sm text-red-600 hover:bg-red-50 rounded-lg transition'
          >
            <X className='w-4 h-4' />
            Clear
          </button>
        )}
      </div>

      <div className='bg-white border border-gray-200 rounded-xl overflow-hidden'>
        <table className='w-full'>
          <thead className='bg-gray-50 border-b border-gray-100'>
            <tr>
              {[
                'PO Number',
                'Supplier',
                'Items',
                'Total',
                'Status',
                'Created',
                '',
              ].map((h) => (
                <th
                  key={h}
                  className='text-left text-xs font-medium text-gray-500 px-4 py-3'
                >
                  {h}
                </th>
              ))}
            </tr>
          </thead>
          <tbody className='divide-y divide-gray-50'>
            {isLoading ? (
              <SkeletonRow />
            ) : !purchaseOrders.length ? (
              <tr>
                <td colSpan={7} className='text-center py-16'>
                  <ShoppingCart className='w-8 h-8 text-gray-300 mx-auto mb-2' />
                  <p className='text-sm text-gray-400'>
                    {hasActiveFilters
                      ? 'No orders match your filters'
                      : 'No purchase orders yet'}
                  </p>
                </td>
              </tr>
            ) : (
              purchaseOrders.map((po) => (
                <PurchaseOrderRow
                  key={po.id}
                  po={po}
                  onClick={() => navigate(`/purchase-orders/${po.id}`)}
                />
              ))
            )}
          </tbody>
        </table>

        {totalPages > 1 && (
          <Pagination
            currentPage={currentPage}
            totalPages={totalPages}
            hasPrevious={hasPrevious}
            hasNext={hasNext}
            onPreviousClick={() => setPage(currentPage - 1)}
            onNextClick={() => setPage(currentPage + 1)}
          />
        )}
      </div>
    </div>
  );
}
