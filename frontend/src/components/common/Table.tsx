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
  onRowClick: (item: T) => void;
};

export default function Table<T>({
  data,
  loading,
  emptyMessage = 'No data found',
  columns,
  getRowKey,
  onRowClick,
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
        <tr className='border-b border-gray-50'>
          {columns.map((column) => (
            <th
              key={column.header}
              className='text-left text-xs font-medium text-gray-400 pb-2'
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
            className='cursor-pointer hover:bg-gray-50 transition'
          >
            {columns.map((column) => (
              <td key={column.header} className='py-4 text-sm'>
                {column.render(item)}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
}
