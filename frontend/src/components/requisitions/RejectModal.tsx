import { useState } from 'react';
import Modal from '../common/Modal';
import TextArea from '../common/TextArea';
import Button from '../common/Button';

type Props = {
  onClose: () => void;
  onConfirm: (reason: string) => void;
  loading: boolean;
};

export default function RejectModal({ onClose, onConfirm, loading }: Props) {
  const [reason, setReason] = useState('');
  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-sm mx-4 p-6'>
        <h2 className='text-base font-semibold text-gray-900 mb-4'>
          Reject Requisition
        </h2>
        <TextArea
          rows={3}
          value={reason}
          onChange={(e) => setReason(e.target.value)}
          placeholder='Provide a reason for rejection'
        />
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
            Reject
          </Button>
        </div>
      </div>
    </Modal>
  );
}
