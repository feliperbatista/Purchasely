import { useState } from 'react';
import Modal from '../common/Modal';
import Button from '../common/Button';

type Props = {
  onClose: () => void;
  onConfirm: (reason: string) => void;
  loading: boolean;
};

export default function CancelModal({ onClose, onConfirm, loading }: Props) {
  const [reason, setReason] = useState('');
  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-sm mx-4 p-6'>
        <h2 className='text-base font-semibold text-gray-900 mb-4'>
          Cancel Purchase Order
        </h2>
        <div>
          <label className='block text-sm font-medium text-gray-700 mb-1.5'>
            Reason <span className='text-red-500'>*</span>
          </label>
          <textarea
            rows={3}
            value={reason}
            onChange={(e) => setReason(e.target.value)}
            placeholder='Provide a reason for cancellation'
            className='w-full px-3.5 py-2.5 text-sm border border-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-orange-400 resize-none placeholder-gray-400'
          />
        </div>
        <div className='flex justify-end gap-2 mt-4'>
          <Button variant='secondary' onClick={onClose}>
            Cancel
          </Button>
          <Button
            variant='danger'
            loading={loading}
            disabled={!reason.trim()}
            onClick={() => onConfirm(reason)}
          >
            Cancel PO
          </Button>
        </div>
      </div>
    </Modal>
  );
}
