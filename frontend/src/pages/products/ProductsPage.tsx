import { useState } from 'react';
import type { Product } from '../../types/product';
import { useProducts } from '../../hooks/useProducts';
import Button from '../../components/common/Button';
import { Plus } from 'lucide-react';
import Table from '../../components/common/Table';
import ProductModal from '../../components/products/ProductModal';
import Pagination from '../../components/common/Pagination';

export default function ProductsPage() {
  const [modal, setModal] = useState<{ open: boolean; product?: Product }>({
    open: false,
  });

  const {
    products,
    totalCount,
    totalPages,
    currentPage,
    hasPrevious,
    hasNext,
    isLoading,
    page,
    setPage,
  } = useProducts();
  return (
    <div className='space-y-5'>
      <div className='flex items-center justify-between'>
        <div>
          <h1 className='text-lg font-semibold text-gray-900'>Products</h1>
          <p>{totalCount} products total</p>
        </div>
        <Button
          icon={<Plus className='w-4 h-4' />}
          onClick={() => setModal({ open: true })}
        >
          New Product
        </Button>
      </div>

      <div className='bg-white rounded-xl border border-gray-100 overflow-hidden p-5'>
        <Table
          data={products}
          loading={isLoading}
          emptyMessage='No products yet'
          getRowKey={(p) => p.id}
          onRowClick={(product) => setModal({ open: true, product: product })}
          columns={[
            {
              header: 'Name',
              render: (product) => (
                <>
                  <p>{product.name}</p>
                  {product.description && (
                    <p className='text-xs text-gray-400 truncate max-w-sm'>
                      {product.description}
                    </p>
                  )}
                </>
              ),
            },
            {
              header: 'SKU',
              render: (product) => product.sku,
            },
            {
              header: 'Category',
              render: (product) => product.category,
            }
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

        {modal.open && (
          <ProductModal
            product={modal.product}
            onClose={() => setModal({ open: false })}
          />
        )}
      </div>
    </div>
  );
}
