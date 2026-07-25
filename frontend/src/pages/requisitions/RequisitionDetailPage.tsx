import { useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { useState } from 'react';
import { useRequisitions } from '../../hooks/useRequisitions';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';
import {
  CheckCircle,
  Clock,
  Pencil,
  Send,
  ShoppingCart,
  User,
  X,
  XCircle,
} from 'lucide-react';
import GoBackButton from '../../components/common/GoBackButton';
import StatusBadge from '../../components/common/StatusBadge';
import Button from '../../components/common/Button';
import InfoItem from '../../components/common/InfoItem';
import { priorities } from '../../types/priority';
import RejectModal from '../../components/requisitions/RejectModal';
import Table from '../../components/common/Table';
import ConvertToPOModal from '../../components/requisitions/ConvertToPOModal';
import NotFoundPage from '../NotFoundPage';

export default function RequisitionDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const {
    requisition,
    isLoadingRequisition: isLoading,
    submit,
    approve,
    reject,
    removeApproval,
  } = useRequisitions(id);
  const [showRejectModal, setShowRejectModal] = useState(false);
  const [showConvertModal, setShowConvertModal] = useState(false);

  const handleSubmit = (requsitionId: string) => {
    submit.mutate(requsitionId, {
      onSuccess: () => {
        toast.success('Requisition submitted for approval');
      },
      onError: (error) => toast.error(getErrorMessage(error)),
    });
  };

  const handleApprove = (requsitionId: string) => {
    approve.mutate(requsitionId, {
      onSuccess: () => {
        toast.success('Requisition approved');
      },
      onError: (error) => toast.error(getErrorMessage(error)),
    });
  };

  const handleReject = ({
    requisitionId,
    reason,
  }: {
    requisitionId: string;
    reason: string;
  }) => {
    reject.mutate(
      { requisitionId, reason },
      {
        onSuccess: () => {
          toast.success('Requisition rejected');
          setShowRejectModal(false);
        },
        onError: (error) => toast.error(getErrorMessage(error)),
      },
    );
  };

  const handleRemoveApproval = (requsitionId: string) => {
    removeApproval.mutate(requsitionId, {
      onSuccess: () => {
        toast.success('Approval removed');
      },
      onError: (error) => toast.error(getErrorMessage(error)),
    });
  };

  if (isLoading) {
    return (
      <div className='space-y-4 animate-pulse'>
        <div className='h-8 w-48 bg-gray-100 rounded' />
        <div className='h-40 bg-gray-100 rounded-xl' />
        <div className='h-64 bg-gray-100 rounded-xl' />
      </div>
    );
  }

  if (!requisition) {
    return <NotFoundPage inline/>;
  }

  const total = requisition.lines.reduce(
    (sum, l) => sum + l.quantityRequested * l.estimatedUnitPrice,
    0,
  );

  const canSubmit =
    requisition.status === 'Draft' && user?.id === requisition.requesterId;
  const canApprove =
    (user?.role === 'Approver' ||
      user?.role === 'Buyer' ||
      user?.role === 'Admin') &&
    requisition.status === 'Submitted';
  const canRemoveApproval =
    (user?.role === 'Approver' ||
      user?.role === 'Buyer' ||
      user?.role === 'Admin') &&
    requisition.status === 'Approved' &&
    user?.id === requisition.requesterId;
  const canReject =
    (user?.role === 'Approver' ||
      user?.role === 'Buyer' ||
      user?.role === 'Admin') &&
    requisition.status === 'Submitted';
  const canConvert =
    (user?.role === 'Buyer' || user?.role === 'Admin') &&
    requisition.status === 'Approved';
  const canEdit =
    requisition.status === 'Draft' && user?.id === requisition.requesterId;

  return (
    <div className='space-y-5 max-x-auto'>
      <div className='flex items-center gap-3'>
        <GoBackButton onClick={() => navigate('/requisitions')} />
        <div>
          <div className='flex items-center gap-2'>
            <h1 className='text-lg font-semibold text-gray-900'>
              Requisition #{requisition.number}
            </h1>
            <StatusBadge status={requisition.status} />
          </div>
          <p className='text-xs text-gray-400 mt-0.5'>
            Created {new Date(requisition.createdAt).toLocaleDateString()}
          </p>
        </div>
      </div>

      <div className='flex items-center gap-2'>
        {canEdit && (
          <Button
            variant='secondary'
            size='sm'
            icon={<Pencil className='w-4 h-4' />}
            onClick={() => navigate(`/requisitions/${id}/edit`)}
          >
            Edit
          </Button>
        )}
        {canSubmit && (
          <Button
            size='sm'
            icon={<Send className='w-4 h-4' />}
            loading={submit.isPending}
            onClick={() => handleSubmit(id!)}
          >
            Submit
          </Button>
        )}
        {canApprove && (
          <Button
            variant='success'
            size='sm'
            icon={<CheckCircle className='w-4 h-4' />}
            loading={approve.isPending}
            onClick={() => handleApprove(id!)}
          >
            Approve
          </Button>
        )}
        {canRemoveApproval && (
          <Button
            variant='danger'
            size='sm'
            icon={<X className='w-4 h-4' />}
            loading={removeApproval.isPending}
            onClick={() => handleRemoveApproval(id!)}
          >
            Remove Approval
          </Button>
        )}
        {canReject && (
          <Button
            variant='danger'
            size='sm'
            icon={<XCircle className='w-4 h-4' />}
            onClick={() => setShowRejectModal(true)}
          >
            Reject
          </Button>
        )}
        {canConvert && (
          <Button
            size='sm'
            icon={<ShoppingCart className='w-4 h-4' />}
            onClick={() => setShowConvertModal(true)}
          >
            Convert to PO
          </Button>
        )}
      </div>

      <div className='bg-white border border-gray-200 rounded-xl p-5'>
        <h2 className='text-sm font-semibold text-gray-900 mb-4'>Details</h2>
        <div className='grid grid-cols-3 gap-5'>
          <InfoItem
            label='Requester'
            value={
              <span className='flex items-center gap-1.5'>
                <User className='w-3.5 h-3.5 text-gray-400' />
                {requisition.requesterName}
              </span>
            }
          />
          <InfoItem
            label='Priority'
            value={
              <span className={priorities[requisition.priority].style}>
                {requisition.priority}
              </span>
            }
          />
          <InfoItem
            label='Submitted At'
            value={
              requisition.submittedAt
                ? new Date(requisition.submittedAt).toLocaleDateString()
                : '-'
            }
          />
        </div>

        {requisition.justification && (
          <div className='mt-4 pt-4 border-t border-gray-100'>
            <p className='text-xs text-gray-400 mb-1'>Justification</p>
            <p className='text-sm text-gray-700'>{requisition.justification}</p>
          </div>
        )}
      </div>

      <div className='bg-white border border-gray-200 rounded-xl overflow-hidden p-5'>
        <div className='border-b border-gray-100 mb-4'>
          <h2 className='text-sm font-semibold text-gray-900'>
            Line Items ({requisition.lines.length})
          </h2>
        </div>
        <Table
          data={requisition.lines}
          loading={isLoading}
          getRowKey={(line) => line.id}
          columns={[
            {
              header: 'Product',
              render: (line) => line.productName,
            },
            {
              header: 'Quantity',
              render: (line) => line.quantityRequested.toLocaleString(),
            },
            {
              header: 'Price',
              render: (line) => line.estimatedUnitPrice.toLocaleString(),
            },
            {
              header: 'Total',
              render: (line) =>
                (
                  line.quantityRequested * line.estimatedUnitPrice
                ).toLocaleString(),
            },
          ]}
          footer={
            <tfoot className='border-t border-gray-100'>
              <tr>
                <td
                  colSpan={3}
                  className='px-5 py-3 text-sm font-medium text-gray-500 text-right'
                >
                  Estimated Total
                </td>
                <td className='px-5 py-3 text-sm font-bold text-gray-900'>
                  $
                  {total.toLocaleString(undefined, {
                    minimumFractionDigits: 2,
                  })}
                </td>
                <td />
              </tr>
            </tfoot>
          }
        />
      </div>

      <div className='bg-white border border-gray-200 rounded-xl p-5'>
        <h2 className='text-sm font-semibold text-gray-900 mb-4'>
          Approval History
        </h2>
        {!requisition.approvals?.length ? (
          <div className='flex items-center gap-2 text-sm text-gray-400 py-4'>
            <Clock className='w-4 h-4' /> No approvals yet
          </div>
        ) : (
          <div className='space-y-3'>
            {requisition.approvals.map((approval) => (
              <div key={approval.id} className='flex items-center gap-3'>
                <div className='w-7 h-7 rounded-full flex items-center justify-center shrink-0 mt-0.5 bg-green-100'>
                  <CheckCircle className='w-4 h-4 text-green-600' />
                </div>
                <div className='flex-1'>
                  <div className='flex items-center justify-between'>
                    <p className='text-sm font-medium text-gray-900'>
                      {approval.approver}
                    </p>
                    <p className='text-xs text-gray-400'>
                      {new Date(approval.approvedAt).toLocaleDateString()}
                    </p>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {showRejectModal && (
        <RejectModal
          onClose={() => setShowRejectModal(false)}
          onConfirm={(reason) => handleReject({ requisitionId: id!, reason })}
          loading={reject.isPending}
        />
      )}

      {showConvertModal && (
        <ConvertToPOModal
          requisitionId={requisition.id}
          lines={requisition.lines}
          onClose={() => setShowConvertModal(false)}
        />
      )}
    </div>
  );
}
