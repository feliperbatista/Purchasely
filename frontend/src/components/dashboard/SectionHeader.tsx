import { ArrowRight } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

export default function SectionHeader({
  title,
  to,
}: {
  title: string;
  to: string;
}) {
  const navigate = useNavigate();
  return (
    <div className='flex items-center justify-between mb-4'>
      <h3 className='text-sm font-semibold text-gray-900'>{title}</h3>
      <button
        onClick={() => navigate(to)}
        className='flex items-center gap-1 text-xs text-blue-600 hover:text-blue-700 transition'
      >
        View all
        <ArrowRight className='w-3 h-3' />
      </button>
    </div>
  );
}
