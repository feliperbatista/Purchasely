import type React from 'react';
import Skeleton from './Skeleton';

type Column<T> = {
  header: string;
  render: (item: T) => React.ReactNode;
};

type TableProps<T> = {
  data?: T[];
  loading?: boolean;
  emptyMessage?: string;
  columns: Column<T>[];
  getRowKey: (item: T) => string;
  onRowClick?: (item: T) => void;
  footer?: React.ReactNode;
};

export default function Table<T>({
  data,
  loading,
  emptyMessage = 'No data found',
  columns,
  getRowKey,
  onRowClick,
  footer,
}: TableProps<T>) {
  if (loading) {
    return (
      <div>
        {Array.from({ length: 5 }).map((_, i) => (
          <Skeleton key={i} className='h-10' />
        ))}
      </div>
    );
  }

  if (!data?.length) {
    return (
      <p className='text-sm text-gray-400 text-center py-8'>{emptyMessage}</p>
    );
  }

  return (
    <table className='w-full'>
      <thead>
        <tr className='border-b border-gray-100'>
          {columns.map((column) => (
            <th
              key={column.header}
              className='text-left text-xs font-medium text-gray-500 px-4 py-3'
            >
              {column.header}
            </th>
          ))}
        </tr>
      </thead>

      <tbody className='divide-y divide-gray-50'>
        {data.map((item) => (
          <tr
            key={getRowKey(item)}
            onClick={() => onRowClick?.(item)}
            className={`hover:bg-gray-50 transition
              ${onRowClick ? 'cursor-pointer' : ''}`}
          >
            {columns.map((column) => (
              <td key={column.header} className='px-4 py-3 text-gray-700 text-sm'>
                {column.render(item)}
              </td>
            ))}
          </tr>
        ))}
      </tbody>

      {footer}
    </table>
  );
}
