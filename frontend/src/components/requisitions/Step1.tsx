import { ArrowRight } from 'lucide-react';
import Button from '../common/Button';
import TextArea from '../common/TextArea';
import type { UseFormReturn } from 'react-hook-form';
import type { CreateRequisitionFormData } from '../../schemas/requisition.schema';

type Props = {
  form: UseFormReturn<CreateRequisitionFormData>;
  onNext: () => void;
};

export default function Step1({ form, onNext }: Props) {
  const {
    register,
    formState: { errors },
    trigger,
  } = form;

  const handleNext = async () => {
    const valid = await trigger(['priority', 'justification']);
    if (valid) onNext();
  };

  return (
    <div className='space-y-4 max-w-lg'>
      <div>
        <label className='block text-sm font-medium text-gray-700 mb-1.5'>
          Priority <span className='text-red-500'>*</span>
        </label>
        <select
          {...register('priority')}
          className='w-full px-3.5 py-2.5 text-sm border border-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-orange-400 focus:border-transparent bg-white'
        >
          <option value='Low'>Low</option>
          <option value='Normal'>Normal</option>
          <option value='High'>High</option>
        </select>
        {errors.priority && (
          <p className='mt-1.5 text-xs text-red-500'>
            {errors.priority.message}
          </p>
        )}
      </div>
      <TextArea
        label='Justification'
        placeholder='Why is this purchase needed?'
        {...register('justification')}
      />
      <div className='flex justify-end pt-2'>
        <Button onClick={handleNext} icon={<ArrowRight className='w-4 h-4' />}>
          Next
        </Button>
      </div>
    </div>
  );
}
