import { FileQuestion } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import Button from '../components/common/Button';

export default function NotFoundPage({ inline }: { inline?: boolean }) {
  const navigate = useNavigate();

  const content = (
    <div className='text-center flex flex-col items-center justify-center h-full'>
      <div className='w-16 h-16 bg-orange-50 rounded-2xl flex items-center justify-center mx-auto mb-4'>
        <FileQuestion className='w-8 h-8 text-orange-500' />
      </div>
      <h1 className='text-4xl font-bold text-gray-900'>404</h1>
      <p className='text-gray-500 font-bold mt-2 mb-6'>
        The page you're looking for doesn't exist
      </p>
      <div className='flex items-center justify-center gap-2'>
        <Button variant='secondary' onClick={() => navigate(-1)}>
          Go back
        </Button>
        <Button onClick={() => navigate('/dashboard')}>Dashboard</Button>
      </div>
    </div>
  );

  if (inline) {
    return (
      <div className='flex items-center justify-center py-24'>{content}</div>
    );
  }

  return (
    <div className='min-h-screen bg-gray-50 flex items-center justify-center'>
      {content}
    </div>
  );
}
