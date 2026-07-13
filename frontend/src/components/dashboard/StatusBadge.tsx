export default function StatusBadge({ status }: { status: string }) {
  const styles: Record<string, string> = {
    Draft: 'bg-gray-100 text-gray-600',
    Submitted: 'bg-yellow-100 text-yellow-700',
    Approved: 'bg-green-100 text-green-700',
    Rejected: 'bg-red-100 text-red-600',
    ConvertedToPO: 'bg-blue-100 text-blue-700',
    Issued: 'bg-blue-100 text-blue-700',
    PartiallyReceived: 'bg-orange-100 text-orange-700',
    Received: 'bg-green-100 text-green-700',
    Closed: 'bg-gray-100 text-gray-600',
    Cancelled: 'bg-red-100 text-red-600',
  };
  return (
    <span
      className={`px-2 py-0.5 rounded-full text-xs font-medium ${styles[status] ?? 'bg-gray-100 text-gray-600'}`}
    >
      {status}
    </span>
  );
}
