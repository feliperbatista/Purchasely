import { useNavigate } from 'react-router-dom';
import { useRequisitions } from '../../hooks/useRequisitions';
import Button from '../../components/common/Button';
import { ClipboardList, Plus, X } from 'lucide-react';
import type { Status } from '../../types/status';
import SkeletonCard from '../../components/requisitions/SkeletonCard';
import RequisitionCard from '../../components/requisitions/RequisitionCard';
import Pagination from '../../components/common/Pagination';

const STATUSES: Status[] = [
  'Draft',
  'Submitted',
  'Approved',
  'Rejected',
  'ConvertedToPO',
];

export default function RequisitionsPage() {
  const navigate = useNavigate();
  const {
    requistions,
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
  } = useRequisitions();

  const hasActiveFilters =
    !!filters.status ||
    !!filters.from ||
    !!filters.to ||
    !!filters.myRequisitions;

  return (
    <div className='space-y-5'>
      <div className='flex items-center justify-between'>
        <div>
          <h1 className='text-lg font-semibold text-gray-900'>Requisitions</h1>
          <p>{totalCount} requisitions total</p>
        </div>
        <Button
          icon={<Plus className='w-4 h-4' />}
          onClick={() => navigate('/requisitions/new')}
        >
          New Requisition
        </Button>
      </div>

      <div className='flex flex-wrap items-center gap-3'>
        <select
          value={filters.status ?? ''}
          onChange={(e) =>
            setFilter('status', (e.target.value as Status) || undefined)
          }
          className='px-3 py-2 text-sm border border-gray-200 rounded-lg bg-white text-gray-900 outline-none focus:ring-2 focus:ring-orange-400'
        >
          <option value=''>All statuses</option>
          {STATUSES.map((s) => (
            <option key={s} value={s}>
              {s === 'ConvertedToPO' ? 'Converted to PO' : s}
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

        <label className='flex items-center gap-2 px-3 py-2 text-sm border border-gray-200 rounded-lg bg-white cursor-pointer select-none'>
          <input
            type='checkbox'
            checked={!!filters.myRequisitions}
            onChange={(e) =>
              setFilter('myRequisitions', e.target.checked || undefined)
            }
            className='accent-orange-500'
          />
          <span className='text-gray-600'>Mine only</span>
        </label>

        {hasActiveFilters && (
          <button
            onClick={clearFilters}
            className='flex items-center gap-1.5 px-3 py-2 text-sm text-red-500 hover:bg-red-50 rounded-lg transition'
          >
            <X className='w-4 h-4' />
            Clear
          </button>
        )}
      </div>

      {isLoading ? (
        <div className='grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-4'>
          {Array.from({ length: 6 }).map((_, i) => (
            <SkeletonCard key={i} />
          ))}
        </div>
      ) : !requistions.length ? (
        <div className='flex flex-col items-center justify-center py-24 text-center'>
          <ClipboardList className='w-10 h-10 text-gray-400 mb-3' />
          <p className='text-sm font-medium text-gray-600'>
            No requistions found
          </p>
          <p className='text-xs text-gray-400 mt-1'>
            {hasActiveFilters
              ? 'Try adjusting your filters'
              : 'Create your first requisition'}
          </p>
          {!hasActiveFilters && (
            <Button
              size='sm'
              className='mt-4'
              onClick={() => navigate('/requisitions/new')}
            >
              New Requisition
            </Button>
          )}
        </div>
      ) : (
        <div className='grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-4'>
          {requistions.map((req) => (
            <RequisitionCard
              key={req.id}
              requisition={req}
              onClick={() => navigate(`/requisitions/${req.id}`)}
            />
          ))}
        </div>
      )}

      {totalPages > 1 && (
        <Pagination
          currentPage={currentPage}
          totalPages={totalPages}
          hasNext={hasNext}
          hasPrevious={hasPrevious}
          onNextClick={() => setPage(currentPage + 1)}
          onPreviousClick={() => setPage(currentPage - 1)}
        />
      )}
    </div>
  );
}
