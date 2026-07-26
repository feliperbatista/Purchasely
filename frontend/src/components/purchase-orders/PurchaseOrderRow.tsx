import { useState } from 'react';
import type { PurchaseOrder } from '../../types/purchaseOrder';
import { ChevronDown, ChevronRight } from 'lucide-react';
import StatusBadge from '../common/StatusBadge';

type Props = {
  po: PurchaseOrder;
  onClick: () => void;
};

export default function PurchaseOrderRow({ po, onClick }: Props) {
  const [expanded, setExpanded] = useState(false);

  return (
    <>
      <tr
        className='hover:bg-gray-50 transition cursor-pointer'
        onClick={() => setExpanded((e) => !e)}
      >
        <td className='px-4 py-3'>
          <div className='flex items-center gap-2'>
            {expanded ? (
              <ChevronDown className='w-4 h-4 text-gray-400 shrink-0' />
            ) : (
              <ChevronRight className='w-4 h-4 text-gray-400 shrink-0' />
            )}
            <span className='text-sm font-medium text-gray-900'>
              {po.poNumber}
            </span>
          </div>
        </td>
        <td className='px-4 py-3 text-sm text-gray-600'>{po.supplierName}</td>
        <td className='px-4 py-3 text-sm text-gray-600'>
          {po.lines.length} item{po.lines.length !== 1 ? 's' : ''}
        </td>
        <td className='px-4 py-3 text-sm text-gray-600'>
          $
          {po.totalAmount.toLocaleString(undefined, {
            minimumFractionDigits: 2,
          })}
        </td>
        <td className='px-4 py-3'>
          <StatusBadge status={po.status} />
        </td>
        <td className='px-4 py-3 text-sm text-gray-600'>
          {new Date(po.createdAt).toLocaleDateString()}
        </td>
        <td className='px-4 py-3'>
          <button
            onClick={(e) => {
              e.stopPropagation();
              onClick();
            }}
            className='text-xs text-orange-500 hover:text-orange-600 font-medium transition'
          >
            View
          </button>
        </td>
      </tr>

      {expanded && (
        <tr>
          <td colSpan={7} className='px-4 pb-3 bg-gray-50'>
            <div className='rounded-lg border border-gray-100 overflow-hidden'>
              <table className='w-full'>
                <thead>
                  <tr className='bg-gray-100'>
                    <th className='text-left text-xs font-medium text-gray-500 px-4 py-2'>
                      Product
                    </th>
                    <th className='text-left text-xs font-medium text-gray-500 px-4 py-2'>
                      Orderd
                    </th>
                    <th className='text-left text-xs font-medium text-gray-500 px-4 py-2'>
                      Received
                    </th>
                    <th className='text-left text-xs font-medium text-gray-500 px-4 py-2'>
                      Unit Price
                    </th>
                    <th className='text-left text-xs font-medium text-gray-500 px-4 py-2'>
                      Total
                    </th>
                  </tr>
                </thead>
                <tbody className='divide-y divide-gray-100 bg-white'>
                  {po.lines.map((line) => (
                    <tr key={line.id}>
                      <td className='px-4 py-2 text-sm text-gray-900'>
                        {line.productName}
                      </td>
                      <td className='px-4 py-2 text-sm text-gray-600'>
                        {line.quantityOrdered}
                      </td>
                      <td className='px-4 py-2 text-sm text-gray-600'>
                        <span
                          className={
                            line.quantityReceived >= line.quantityOrdered
                              ? 'text-green-600'
                              : 'text-yellow-600'
                          }
                        >
                          {line.quantityReceived}
                        </span>
                        /{line.quantityOrdered}
                      </td>
                      <td className='px-4 py-2 text-sm text-gray-600'>
                        ${line.unitPrice.toLocaleString()}
                      </td>
                      <td className='px-4 py-2 text-sm font-medium text-gray-600'>
                        $
                        {line.lineTotal.toLocaleString(undefined, {
                          minimumFractionDigits: 2,
                        })}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </td>
        </tr>
      )}
    </>
  );
}
