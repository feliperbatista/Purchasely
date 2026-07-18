import Button from './Button';
import Modal from './Modal';

type Props = {
  onClose: () => void;
  itemName?: string;
  loading: boolean;
  onConfirm: () => void;
};

export default function DeleteDialog({
  onClose,
  itemName,
  loading,
  onConfirm,
}: Props) {
  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-sm mx-4 p-6'>
        <h2 className='text-base font-semibold text-gray-900 mb-2'>
          Delete
        </h2>
        <p>
          Are you sure you want to delete <strong>{itemName}</strong>? This
          action cannot be undone.
        </p>
        <div className='flex justify-end gap-2'>
          <Button variant='secondary' onClick={onClose}>
            Cancel
          </Button>
          <Button variant='danger' loading={loading} onClick={onConfirm}>
            Delete
          </Button>
        </div>
      </div>
    </Modal>
  );
}
