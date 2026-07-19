import type { Priority, Requisition } from '../../types/requisition';
import StatusBadge from '../common/StatusBadge';

type Props = {
  requisition: Requisition;
  onClick: () => void;
};

const priorityStyles: Record<Priority, string> = {
  Low: 'text-gray-400',
  Normal: 'text-yellow-900',
  High: 'text-red-600',
};

export default function RequisitionCard({ requisition, onClick }: Props) {
  const totalEstimated = requisition.lines.reduce(
    (sum, l) => sum + l.quantityRequested * l.estimatedUnitPrice,
    0,
  );

  return (
    <div
      onClick={onClick}
      className='bg-white border border-gray-200 rounded-xl p-5 cursor-pointer hover:border-orange-300 hover:shadow-sm transition-all space-y-4'
    >
      <div className='flex items-start justify-between gap-2'>
        <div>
          <p className='text-xs text-gray-400 font-mono'>
            ${requisition.number}
          </p>
          <p className='text-sm font-semibold text-gray-900 mt-0.5 line-clamp-1'>
             {requisition.justification ?? "No justification"}
          </p>
        </div>
        <StatusBadge status={requisition.status} />
      </div>

      <div className='space-y-1'>
        {requisition.lines.slice(0, 2).map((line) => (
          <div key={line.id} className='flex items-center justify-between'>
            <p className='text-xs text-gray-600 truncate max-w-[60%]'>
              {line.productName}
            </p>
            <p className='text-xs text-gray-600'>{line.quantityRequested}</p>
          </div>
        ))}
        {requisition.lines.length > 2 && (
          <p className='text-xs text-gray-900'>
            +{requisition.lines.length - 2} more items
          </p>
        )}
      </div>

      <div className='border-t border-gray-200' />

      <div className='flex items-center justify-between'>
        <div className='flex items-center gap-3'>
          <span
            className={`text-xs font-medium ${priorityStyles[requisition.priority]}`}
          >
            {requisition.priority}
          </span>
          <p className='text-xs font-semibold text-gray-900'>
            ${totalEstimated.toLocaleString()}
          </p>
        </div>
      </div>

      <p className='text-xs text-gray-400'>
        by <span className='text-gray-600'>{requisition.requesterName}</span>
      </p>
    </div>
  );
}
