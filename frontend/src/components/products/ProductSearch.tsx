import { useState } from 'react';
import type { Product } from '../../types/product';
import { useProducts } from '../../hooks/useProducts';
import { ChevronDown, Search, X } from 'lucide-react';

type ProductSearchProps = {
  value: string;
  onChange: (productId: string) => void;
  error?: string;
  disabled?: boolean;
  initialProduct?: Product;
};

export default function ProductSearch({
  value,
  onChange,
  error,
  disabled,
  initialProduct,
}: ProductSearchProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState('');
  const [selected, setSelected] = useState<Product | null>(
    initialProduct ?? null,
  );

  const { searchResults, isSearching } = useProducts(search);

  const handleSelect = (product: Product) => {
    setSelected(product);
    onChange(product.id);
    setSearch('');
    setOpen(false);
  };

  const handleClear = () => {
    setSelected(null);
    onChange('');
  };

  return (
    <div className='relative'>
      <label className='block text-sm font-medium text-gray-700 mb-1.5'>
        Product <span className='text-red-500'>*</span>
      </label>

      <button
        type='button'
        disabled={disabled}
        onClick={() => setOpen((o) => !o)}
        className={`w-full flex items-center justify-between px-3.5 py-2.5 text-sm border rounded-lg outline-none transition text-left disabled:bg-gray-50 disabled:text-blue-900 disabled:cursor-not-allowed disabled:border-none hover:border-orange-400 hover:cursor-pointer
          ${error ? 'border-red-400 bg-red-50' : 'border-gray-100'}
          `}
      >
        <span className={selected ? 'text-black' : 'text-gray-700'}>
          {selected
            ? `${selected.name} - ${selected.sku}`
            : 'Search products...'}
        </span>
        <div className='flex items-center gap-1'>
          {selected && !disabled && (
            <span
              onClick={(e) => {
                e.stopPropagation();
                handleClear();
              }}
              className='p-0.5 rounded hover:bg-gray-50 text-gray-700 hover:text-black'
            >
              <X className='w-3 h-3' />
            </span>
          )}
          <ChevronDown
            className={`w-4 h-4 text-gray-700 transition ${open ? 'rotate-180' : ''}`}
          />
        </div>
      </button>

      {open && (
        <div className='absolute z-50 w-full mt-1 bg-white border border-gray-100 rounded-lg shadow overflow-hidden'>
          <div className='flex items-center gap-2 px-3 py-2 border-b border-gray-100'>
            <Search className='w-4 h-4 text-gray-700 shrink-0' />
            <input
              autoFocus
              type='text'
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              placeholder='Type to search'
              className='flex-1 text-sm outline-none placeholder-gray-700'
            />
          </div>

          <ul className='max-h-52 overflow-y-auto'>
            {isSearching ? (
              <li className='px-3 py-8 text-center text-sm text-gray-700'>
                Searching...
              </li>
            ) : !searchResults.length ? (
              <li className='px-3 py-8 text-center text-sm text-gray-700'>
                No products found
              </li>
            ) : (
              searchResults.map((product) => (
                <li
                  key={product.id}
                  onClick={() => handleSelect(product)}
                  className={`flex items-center justify-between px-3 py-2.5 cursor-pointer hover:bg-gray-50 transition
                    ${value === product.id ? 'bg-orange-50 text-orange-600' : 'text-black'}`}
                >
                  <span className='text-sm font-medium'>{product.name}</span>
                  <span className='text-xs text-content-tertiary font-mono'>
                    {product.sku}
                  </span>
                </li>
              ))
            )}
          </ul>
        </div>
      )}

      {error && <p className='mt-1.5 text-xs text-red-500'>{error}</p>}
    </div>
  );
}
