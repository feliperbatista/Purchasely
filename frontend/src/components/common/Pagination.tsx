import Button from './Button';

type Props = {
  currentPage: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  onPreviousClick: () => void;
  onNextClick: () => void;
};

export default function Pagination({
  currentPage,
  totalPages,
  hasPrevious,
  hasNext,
  onPreviousClick,
  onNextClick,
}: Props) {
  return (
    <div className='flex items-center justify-between px-4 py-3 border-t border-gray-100'>
      <p className='text-xs text-gray-500'>
        Page {currentPage} of {totalPages}
      </p>
      <div className='flex gap-2'>
        <Button
          variant='secondary'
          size='sm'
          disabled={!hasPrevious}
          onClick={onPreviousClick}
        >
          Previous
        </Button>
        <Button
          variant='secondary'
          size='sm'
          disabled={!hasNext}
          onClick={onNextClick}
        >
          Next
        </Button>
      </div>
    </div>
  );
}
