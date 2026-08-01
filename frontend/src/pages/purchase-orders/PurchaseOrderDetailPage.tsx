import { useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { useState } from 'react';
import { usePurchaseOrders } from '../../hooks/usePurchaseOrders';
import NotFoundPage from '../NotFoundPage';
import {
  ArrowLeft,
  Calendar,
  CheckCircle,
  Download,
  FileText,
  PackageCheck,
  Send,
  User,
  XCircle,
} from 'lucide-react';
import StatusBadge from '../../components/common/StatusBadge';
import Button from '../../components/common/Button';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';
import InfoItem from '../../components/common/InfoItem';
import Table from '../../components/common/Table';
import CancelModal from '../../components/purchase-orders/CancelModal';
import ReceiveModal from '../../components/purchase-orders/ReceiveModal';

export default function PurchaseOrderDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [showCancelModal, setShowCancelModal] = useState(false);
  const [showReceiveModal, setShowReceiveModal] = useState(false);

  const {
    purchaseOrder: po,
    isLoadingDetail,
    issue,
    receive,
    close,
    cancel,
  } = usePurchaseOrders(id);

  if (isLoadingDetail) {
    return (
      <div className='space-y-4 animate-pulse max-w-4xl mx-auto'>
        <div className='h-8 w-48 bg-gray-200 rounded' />
        <div className='h-40 bg-gray-200 rounded-xl' />
        <div className='h-64 bg-gray-200 rounded-xl' />
      </div>
    );
  }

  if (!po) return <NotFoundPage inline />;

  const isBuyerOrAdmin = user?.role === 'Buyer' || user?.role === 'Admin';

  const canIssue = isBuyerOrAdmin && po.status === 'Draft';
  const canReceive =
    isBuyerOrAdmin &&
    (po.status === 'Issued' || po.status === 'PartiallyReceived');
  const canClose = isBuyerOrAdmin && po.status === 'Received';
  const canCancel =
    isBuyerOrAdmin && (po.status === 'Draft' || po.status === 'Issued');

  return (
    <div className='space-y-5 max-w-4xl mx-auto'>
      <div className='flex items-center justify-between'>
        <div className='flex items-center gap-3'>
          <button
            onClick={() => navigate('/purchase-orders')}
            className='p-2 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition'
          >
            <ArrowLeft />
          </button>
          <div>
            <div className='flex items-center gap-2'>
              <h1 className='text-lg font-semibold text-gray-900'>
                {po.poNumber}
              </h1>
              <StatusBadge status={po.status} />
            </div>
            <p className='text-xs text-gray-400 mt-0.5'>
              Created {new Date(po.createdAt).toLocaleDateString()}
            </p>
          </div>
        </div>

        <div className='flex items-center gap-2'>
          {canIssue && (
            <Button
              size='sm'
              icon={<Send className='w-4 h-4' />}
              loading={issue.isPending}
              onClick={() =>
                issue.mutate(po.id, {
                  onSuccess: () => toast.success('Purchase order issued'),
                  onError: (e) => toast.error(getErrorMessage(e)),
                })
              }
            >
              Issue
            </Button>
          )}
          {canReceive && (
            <Button
              size='sm'
              icon={<PackageCheck className='w-4 h-4' />}
              onClick={() => setShowReceiveModal(true)}
            >
              Receive
            </Button>
          )}
          {canClose && (
            <Button
              size='sm'
              icon={<CheckCircle className='w-4 h-4' />}
              loading={close.isPending}
              onClick={() =>
                close.mutate(po.id, {
                  onSuccess: () => toast.success('Purchase order closed'),
                  onError: (e) => toast.error(getErrorMessage(e)),
                })
              }
            >
              Close
            </Button>
          )}
          {canCancel && (
            <Button
              variant='danger'
              size='sm'
              icon={<XCircle className='w-4 h-4' />}
              onClick={() => setShowCancelModal(true)}
            >
              Cancel
            </Button>
          )}
        </div>
      </div>

      <div className='bg-white border border-gray-200 rounded-xl p-5'>
        <h2 className='text-sm font-semibold text-gray-900 mb-4'>Details</h2>
        <div className='grid grid-cols-2 sm:grid-cols-4 gap-5'>
          <InfoItem
            label='Supplier'
            value={
              <span className='flex items-center gap-1.5'>
                <User className='w-3.5 h-3.5 text-gray-400' />
                {po.supplierName}
              </span>
            }
          />
          <InfoItem label='Created by' value={po.createdBy} />
          <InfoItem
            label='Issued At'
            value={
              po.issuedAt ? (
                <span className='flex items-center gap-1.5'>
                  <Calendar className='w-3.5 h-3.5 text-gray-400' />
                  {new Date(po.issuedAt).toLocaleDateString()}
                </span>
              ) : (
                '-'
              )
            }
          />
          <InfoItem label='Status' value={<StatusBadge status={po.status} />} />
        </div>

        <div className='mt-4 pt-4 border-t border-gray-100 flex justify-end'>
          <div className='space-y-1 text-right'>
            <div className='flex justify-between gap-16'>
              <p className='text-xs text-gray-400'>Subtotal</p>
              <p className='text-sm text-gray-700'>
                $
                {po.subtotal.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}
              </p>
            </div>
            <div className='flex justify-between gap-16'>
              <p className='text-xs text-gray-400'>Tax</p>
              <p className='text-sm text-gray-700'>
                $
                {po.taxAmount.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}
              </p>
            </div>
            <div className='flex justify-between gap-16 pt-1 border-t border-gray-100'>
              <p className='text-xs font-semibold text-gray-900'>Total</p>
              <p className='text-sm font-bold text-gray-900'>
                $
                {po.totalAmount.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}
              </p>
            </div>
          </div>
        </div>

        {po.status === 'Cancelled' && po.cancellationReason && (
          <div className='mt-4 pt-4 border-t border-gray-100'>
            <p className='text-xs text-gray-400 mb-1'>Cancellation Reason</p>
            <p className='text-sm text-red-600'>{po.cancellationReason}</p>
          </div>
        )}
      </div>

      <div className='bg-white border border-gray-200 rounded-xl overflow-hidden'>
        <div className='px-5 py-4 border-b border-gray-100'>
          <h2 className='text-sm font-semibold text-gray-900'>
            Line Items ({po.lines.length})
          </h2>
        </div>
        <Table
          data={po.lines}
          loading={isLoadingDetail}
          emptyMessage='No products'
          getRowKey={(l) => l.id}
          onRowClick={undefined}
          columns={[
            {
              header: 'Product',
              render: (line) => <p>{line.productName}</p>,
            },
            {
              header: 'Ordered',
              render: (line) => <p>{line.quantityOrdered}</p>,
            },
            {
              header: 'Received',
              render: (line) => <p>{line.quantityReceived}</p>,
            },
            {
              header: 'Unit Price',
              render: (line) => <p>{line.unitPrice.toLocaleString()}</p>,
            },
            {
              header: 'Total',
              render: (line) => <p>{line.lineTotal.toLocaleString()}</p>,
            },
          ]}
        />
      </div>

      {po.documents && po.documents.length > 0 && (
        <div className='bg-white border border-gray-200 rounded-xl p-5'>
          <h2 className='text-sm font-semibold text-gray-900 mb-4'>
            Documents ({po.documents.length})
          </h2>
          <div className='space-y-2'>
            {po.documents.map((doc) => (
              <a
                key={doc.id}
                href={doc.blobUrl}
                target='_blank'
                rel='noopener noreferrer'
                className='flex items-center justify-between px-4 py-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition group'
              >
                <div className='flex items-center gap-3'>
                  <FileText className='w-4 h-4 text-gray-400' />
                  <div>
                    <p className='text-sm font-medium text-gray-900'>
                      {doc.fileName}
                    </p>
                  </div>
                </div>
                <Download className='w-4 h-4 text-gray-400 group-hover:text-orange-500 transition' />
              </a>
            ))}
          </div>
        </div>
      )}

      {showCancelModal && (
        <CancelModal
          onClose={() => setShowCancelModal(false)}
          loading={cancel.isPending}
          onConfirm={(reason) =>
            cancel.mutate(
              { poId: po.id, reason },
              {
                onSuccess: () => {
                  toast.success('Purchase order cancelled');
                  setShowCancelModal(false);
                },
                onError: (e) => toast.error(getErrorMessage(e)),
              },
            )
          }
        />
      )}

      {showReceiveModal && (
        <ReceiveModal
          po={po}
          onClose={() => setShowReceiveModal(false)}
          loading={receive.isPending}
          onConfirm={(data) =>
            receive.mutate(
              { poId: po.id, data },
              {
                onSuccess: () => {
                  toast.success('Receipt recorded sucessfully');
                  setShowReceiveModal(false);
                },
                onError: (e) => toast.error(getErrorMessage(e)),
              },
            )
          }
        />
      )}
    </div>
  );
}
