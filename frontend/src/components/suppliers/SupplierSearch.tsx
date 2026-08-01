import { useState } from 'react';
import type { Supplier } from '../../types/supplier';
import { useSuppliers } from '../../hooks/useSuppliers';
import { ChevronDown, Search, X } from 'lucide-react';

type SupplierSearchProps = {
  value: string;
  onChange: (supplierId: string, supplier?: Supplier) => void;
};

export default function SupplierSearch({
  value,
  onChange,
}: SupplierSearchProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState('');
  const [selectedSupplier, setSelectedSupplier] = useState<Supplier | null>(
    null,
  );

  const { searchResults, isSearching } = useSuppliers(undefined, search);

  const displaySupplier = value ? selectedSupplier : null;

  const handleSelect = (supplier: Supplier) => {
    setSelectedSupplier(supplier);
    onChange(supplier.id, supplier);
    setSearch('');
    setOpen(false);
  };

  const handleClear = () => {
    setSelectedSupplier(null);
    onChange('');
  };

  return (
    <div className='relative w-96'>
      <button
        type='button'
        onClick={() => setOpen((o) => !o)}
        className='w-full flex items-center justify-between px-3 py-2 text-sm bg-white border border-gray-200 rounded-lg outline-none transition text-left focus:ring-2 focus:ring-orange-400 hover:cursor-pointer'
      >
        <span className={displaySupplier ? 'text-black' : 'text-gray-900'}>
          {displaySupplier ? `${displaySupplier.name}` : 'Search Suppliers...'}
        </span>
        <div className='flex items-center gap-1'>
          {displaySupplier && (
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
                No Suppliers found
              </li>
            ) : (
              searchResults.map((supplier) => (
                <li
                  key={supplier.id}
                  onClick={() => handleSelect(supplier)}
                  className={`flex items-center justify-between px-3 py-2.5 cursor-pointer hover:bg-gray-50 transition
                    ${value === supplier.id ? 'bg-orange-50 text-orange-600' : 'text-black'}`}
                >
                  <span className='text-sm font-medium'>{supplier.name}</span>
                </li>
              ))
            )}
          </ul>
        </div>
      )}
    </div>
  );
}
