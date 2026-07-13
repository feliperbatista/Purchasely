import type React from 'react';

interface StatCardProps {
  label: string;
  value: string | number;
  icon: React.ReactNode;
  color: string;
}

export default function StatCard({ label, value, icon, color }: StatCardProps) {
  return (
    <div className='bg-white rounded-xl border border-gray-100 p-5 flex items-center gap-4'>
      <div
        className={`w-11 h-11 rounded-lg flex items-center justify-center shrink-0 ${color}`}
      >
        {icon}
      </div>
      <div>
        <p className='text-xs text-gray-500'>{label}</p>
        <p className='text-2xl font-bold text-gray-900 mt-0.5'>{value}</p>
      </div>
    </div>
  );
}
