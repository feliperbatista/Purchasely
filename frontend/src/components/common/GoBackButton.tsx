import { ArrowLeft } from 'lucide-react';

type Props = {
  onClick: () => void;
};

export default function GoBackButton({ onClick }: Props) {
  return (
    <button
      onClick={onClick}
      className='p-2 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition'
    >
      <ArrowLeft className='w-4 h-4' />
    </button>
  );
}
