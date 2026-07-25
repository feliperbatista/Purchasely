import { useState } from 'react';
import { useSupplierProducts } from '../../hooks/useSupplierProducts';
import type { RequisitionLine } from '../../types/requisition';
import { useNavigate } from 'react-router-dom';
import { useRequisitions } from '../../hooks/useRequisitions';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';
import Modal from '../common/Modal';
import { ShoppingCart, X } from 'lucide-react';
import Button from '../common/Button';

type LineRowProps = {
  line: RequisitionLine;
  onChange: (lineId: string, supplierId: string, unitPrice: number) => void;
};

function LineRow({ line, onChange }: LineRowProps) {
  const { data: suppliers, isLoading } = useSupplierProducts(line.productId);
  const [selectedSupplierId, setSelectedSupplierId] = useState('');
  const [unitPrice, setUnitPrice] = useState(line.estimatedUnitPrice);

  const handleSupplierChange = (supplierId: string) => {
    setSelectedSupplierId(supplierId);
    const supplier = suppliers?.find((s) => s.supplierId === supplierId);
    if (supplier) {
      setUnitPrice(supplier.unitPrice);
      onChange(line.id, supplierId, supplier.unitPrice);
    }
  };

  const handlePriceChange = (price: number) => {
    setUnitPrice(price);
    if (selectedSupplierId) {
      onChange(line.id, selectedSupplierId, price);
    }
  };

  return (
    <div className='bg-gray-50 rounded-xl p-4 space-y-3'>
      <div className='flex items-center justify-between'>
        <p className='text-sm font-medium text-gray-900'>{line.productName}</p>
        <p className='text-xs text-gray-500'>{line.quantityRequested}</p>
      </div>

      <div className='grid grid-cols-2 gap-3'>
        <div>
          <label className='block text-xs font-medium text-gray-600 mb-1'>
            Supplier <span className='text-red-500'>*</span>
          </label>
          {isLoading ? (
            <div className='h-9 bg-gray-200 rounded-lg animate-pulse' />
          ) : (
            <select
              value={selectedSupplierId}
              onChange={(e) => handleSupplierChange(e.target.value)}
              className='w-full px-3 py-2 text-sm border border-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-orange-400 bg-white'
            >
              <option value=''>Select supplier...</option>
              {suppliers?.map((s) => (
                <option key={s.supplierId} value={s.supplierId}>
                  {s.supplierName}
                </option>
              ))}
            </select>
          )}
        </div>

        <div>
          <label className='block text-xs font-medium text-gray-600 mb-1'>
            Unit Price <span className='text-red-500'>*</span>
          </label>
          <input
            type='number'
            step={0.01}
            value={unitPrice}
            onChange={(e) => handlePriceChange(Number(e.target.value))}
            className='w-full px-3 py-2 text-sm border border-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-orange-400'
          />
        </div>
      </div>

      {selectedSupplierId && (
        <p>
          Line total: $
          {(line.quantityRequested * unitPrice).toLocaleString(undefined, {
            minimumFractionDigits: 2,
          })}
        </p>
      )}
    </div>
  );
}

type ConvertToPOModalProps = {
  requisitionId: string;
  lines: RequisitionLine[];
  onClose: () => void;
};

export default function ConvertToPOModal({
  requisitionId,
  lines,
  onClose,
}: ConvertToPOModalProps) {
  const navigate = useNavigate();
  const { convertToPO } = useRequisitions(requisitionId);

  const [lineSelections, setLineSelections] = useState<
    Record<string, { supplierId: string; unitPrice: number }>
  >({});

  const handleLineChange = (
    lineId: string,
    supplierId: string,
    unitPrice: number,
  ) => {
    setLineSelections((prev) => ({
      ...prev,
      [lineId]: { supplierId, unitPrice },
    }));
  };

  const allLinesSelected = lines.every((l) => lineSelections[l.id]?.supplierId);

  const handleConvert = () => {
    if (!allLinesSelected) {
      toast.error('Please select a supplier for every line');
      return;
    }

    convertToPO.mutate(
      {
        requisitionId,
        lines: lines.map((l) => ({
          requisitionLineId: l.id,
          supplierId: lineSelections[l.id].supplierId,
          unitPrice: lineSelections[l.id].unitPrice,
        })),
      },
      {
        onSuccess: (data) => {
          toast.success(
            `${data.purchaseOrders.length} purchase order(s) created`,
          );
          onClose();
          if (data.purchaseOrders.length === 1)
            navigate(`/purchase-orders/${data.purchaseOrders[0].id}`);
          else navigate('/purchase-orders');
        },
        onError: (error) => toast.error(getErrorMessage(error)),
      },
    );
  };

  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-lg mx-4 max-h-[90vh] flex flex-col'>
        <div className='flex items-center justify-between p-6 border-b border-gray-100'>
          <div>
            <h2 className='text-base font-semibold text-gray-900'>
              Convert to Purchase Order
            </h2>
            <p className='text-xs text-gray-400 mt-0.5'>
              Select a supplier for each line
            </p>
          </div>
          <button
            onClick={onClose}
            className='text-gray-400 hover:text-gray-600 transition'
          >
            <X />
          </button>
        </div>

        <div className='flex-1 overflow-y-auto p-6 space-y-3'>
          {lines.map((line) => (
            <LineRow key={line.id} line={line} onChange={handleLineChange} />
          ))}
        </div>

        <div className='p-6 border-t border-gray-100 flex justify-end gap-2'>
          <Button variant='secondary' onClick={onClose}>
            Cancel
          </Button>
          <Button
            icon={<ShoppingCart className='w-4 h-4' />}
            loading={convertToPO.isPending}
            disabled={!allLinesSelected}
            onClick={handleConvert}
          >
            Create PO
          </Button>
        </div>
      </div>
    </Modal>
  );
}
