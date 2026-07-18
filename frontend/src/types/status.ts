export const statuses = {
  Draft: {
    label: 'Draft',
    style: 'bg-gray-100 text-gray-600',
  },
  Submitted: {
    label: 'Submitted',
    style: 'bg-yellow-100 text-yellow-700',
  },
  Approved: {
    label: 'Approved',
    style: 'bg-green-100 text-green-700',
  },
  Rejected: {
    label: 'Rejected',
    style: 'bg-red-100 text-red-600',
  },
  ConvertedToPO: {
    label: 'Converted To PO',
    style: 'bg-blue-100 text-blue-700',
  },
  Issued: {
    label: 'Issued',
    style: 'bg-blue-100 text-blue-700',
  },
  PartiallyReceived: {
    label: 'Partially Received',
    style: 'bg-orange-100 text-blue-700',
  },
  Received: {
    label: 'Received',
    style: 'bg-green-100 text-green-700',
  },
  Closed: {
    label: 'Closed',
    style: 'bg-gray-100 text-gray-600',
  },
  Cancelled: {
    label: 'Cancelled',
    style: 'bg-red-100 text-red-600',
  },
} as const;

export type Status = keyof typeof statuses;
