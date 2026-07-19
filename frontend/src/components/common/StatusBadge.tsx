import { type Status, statuses } from '../../types/status';

type Props = {
  status: Status;
};

export default function StatusBadge({ status }: Props) {
  const { label, style } = statuses[status];
  return (
    <span
      className={`px-2 py-0.5 rounded-full text-xs font-medium ${style ?? 'bg-gray-100 text-gray-600'}`}
    >
      {label}
    </span>
  );
}
