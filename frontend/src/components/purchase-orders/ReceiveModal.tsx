import { useState } from 'react';
import type { PurchaseOrder } from '../../types/purchaseOrder';
import { toast } from 'sonner';
import Modal from '../common/Modal';
import Button from '../common/Button';
import { PackageCheck } from 'lucide-react';

type Props = {
  po: PurchaseOrder;
  onClose: () => void;
  onConfirm: (data: FormData) => void;
  loading: boolean;
};

export default function ReceiveModal({
  po,
  onClose,
  onConfirm,
  loading,
}: Props) {
  const [quantities, setQuantities] = useState<Record<string, number>>(
    Object.fromEntries(
      po.lines.map((l) => [l.id, l.quantityOrdered - l.quantityReceived]),
    ),
  );
  const [files, setFiles] = useState<File[]>([]);

  const handleSubmit = () => {
    if (!files.length) {
      toast.error('Please attach at least one proof of receipt');
      return;
    }

    const formData = new FormData();
    po.lines.forEach((line, i) => {
      formData.append(`Lines[${i}].Id`, line.id);
      formData.append(`Lines[${i}].Quantity`, String(quantities[line.id]));
    });
    files.forEach((file) => formData.append('proofs', file));

    onConfirm(formData);
  };
  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-lg mx-4 max-h-[90vh] flex flex-col'>
        <div className='flex items-center justify-between p-6 border-b border-gray-100'>
          <div>
            <h2>Receive Purchase Order</h2>
            <p>Enter quantities received for each line</p>
          </div>
        </div>

        <div className='flex-1 overflow-y-auto p-6 space-y-4'>
          <div className='space-y-3'>
            {po.lines.map((line) => (
              <div
                key={line.id}
                className='flex items-center justify-between bg-gray-50 rounded-lg px-4 py-3'
              >
                <div>
                  <p className='text-sm font-medium text-gray-900'>
                    {line.productName}
                  </p>
                  <p className='text-xs text-gray-400'>
                    Ordered: {line.quantityOrdered} | Received so far:{' '}
                    {line.quantityReceived}
                  </p>
                </div>
                <input
                  type='number'
                  min={0}
                  max={line.quantityOrdered - line.quantityReceived}
                  value={quantities[line.id]}
                  onChange={(e) =>
                    setQuantities((prev) => ({
                      ...prev,
                      [line.id]: Number(e.target.value),
                    }))
                  }
                  className='w-24 px-3 py-1.5 text-sm border border-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-orange-400 text-center'
                />
              </div>
            ))}
          </div>

          <div>
            <label className='block text-sm font-medium text-gray-700 mb-1.5'>
              Proof of Receipt <span className='text-red-500'>*</span>
            </label>
            <input
              type='file'
              multiple
              accept='.pdf,.jpg,.jpeg,.png'
              onChange={(e) => setFiles(Array.from(e.target.files ?? []))}
              className='w-full px-3.5 py-2.5 text-sm border border-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-orange-400'
            />
            {files.length > 0 && (
              <p className='text-xs text-gray-400 mt-1'>
                {files.length} file(s) selected
              </p>
            )}
          </div>
        </div>

        <div className='p-6 border-t border-gray-100 flex justify-end gap-2'>
          <Button variant='secondary' onClick={onClose}>
            Cancel
          </Button>
          <Button
            icon={<PackageCheck className='w-4 h-4' />}
            loading={loading}
            onClick={handleSubmit}
          >
            Confirm Receipt
          </Button>
        </div>
      </div>
    </Modal>
  );
}
