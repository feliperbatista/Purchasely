import { Plus } from 'lucide-react';
import Button from '../../components/common/Button';
import { useSuppliers } from '../../hooks/useSuppliers';
import { useNavigate } from 'react-router-dom';
import Table from '../../components/common/Table';
import Pagination from '../../components/common/Pagination';

export default function SuppliersPage() {
  const {
    suppliers,
    totalCount,
    totalPages,
    currentPage,
    hasPrevious,
    hasNext,
    isLoadingAll,
    page,
    setPage,
  } = useSuppliers();

  const navigate = useNavigate();

  return (
    <div className='space-y-5'>
      <div className='flex items-center justify-between'>
        <div>
          <h1 className='text-lg font-semibold text-gray-900'>Suppliers</h1>
          <p>{totalCount} suppliers total</p>
        </div>
        <Button
          icon={<Plus className='w-4 h-4' />}
          onClick={() => navigate('/suppliers/new')}
        >
          New Supplier
        </Button>
      </div>
      <div className='bg-white rounded-xl border border-gray-100 overflow-hidden p-5'>
        <Table
          data={suppliers}
          loading={isLoadingAll}
          emptyMessage='No suppliers yet'
          getRowKey={(supplier) => supplier.id}
          onRowClick={(supplier) => navigate(`/suppliers/${supplier.id}`)}
          columns={[
            {
              header: 'Name',
              render: (supplier) => supplier.name,
            },
            {
              header: 'Email',
              render: (supplier) => supplier.email,
            },
            {
              header: 'Phone',
              render: (supplier) => supplier.phone,
            },
          ]}
        />

        {totalPages > 1 && (
          <Pagination
            currentPage={currentPage}
            totalPages={totalPages}
            hasPrevious={hasPrevious}
            hasNext={hasNext}
            onPreviousClick={() => setPage(page - 1)}
            onNextClick={() => setPage(page + 1)}
          />
        )}
      </div>
    </div>
  );
}
