import type React from 'react';

export default function InfoItem({
  label,
  value,
}: {
  label: string;
  value: React.ReactNode;
}) {
  return (
    <div>
      <p className='text-xs text-gray-400 mb-0.5'>{label}</p>
      <p className='text-sm font-medium text-gray-900'>{value}</p>
    </div>
  );
}
